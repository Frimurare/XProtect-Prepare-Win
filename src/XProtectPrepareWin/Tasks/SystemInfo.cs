using System.Globalization;
using System.IO;
using Microsoft.Win32;

namespace XProtectPrepareWin.Tasks;

public sealed record DriveRow(
    string Letter,
    string Label,
    double SizeGb,
    double FreeGb,
    int? BytesPerCluster,
    bool? IndexingEnabled,
    bool IsSystem);

public sealed record SystemSnapshot(
    string OsCaption,
    string OsVersion,
    string Cpu,
    double RamGb,
    string? XProtectProduct,
    List<DriveRow> Drives,
    bool IsDomainJoined);

public static class SystemInfo
{
    private const string CurrentVersionKey = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion";
    private const double OneGb = 1073741824.0;

    public static async Task<SystemSnapshot> GetAsync(PrepContext ctx)
    {
        var osCaption = GetOsCaption();
        var osVersion = Environment.OSVersion.Version.ToString();
        var cpu = Reg.GetString(Registry.LocalMachine,
            @"HARDWARE\DESCRIPTION\System\CentralProcessor\0", "ProcessorNameString")?.Trim() ?? "Unknown CPU";

        // One PS roundtrip for RAM, domain membership and volume details.
        var r = await ctx.PowerShellAsync(
            "$ErrorActionPreference = 'SilentlyContinue'\n" +
            "$cs = Get-CimInstance Win32_ComputerSystem\n" +
            "'RAM=' + $cs.TotalPhysicalMemory\n" +
            "'DOM=' + $cs.PartOfDomain\n" +
            "Get-CimInstance Win32_Volume | Where-Object { $_.DriveType -eq 3 -and $_.DriveLetter } | ForEach-Object {\n" +
            "  'VOL=' + $_.DriveLetter + '|' + $_.Capacity + '|' + $_.FreeSpace + '|' + $_.IndexingEnabled + '|' + $_.Label\n" +
            "}", 120);

        double ramGb = 0;
        var isDomain = false;
        var drives = new List<DriveRow>();
        var systemLetter = DriveUtil.SystemLetter;

        foreach (var raw in r.StdOut.Split('\n'))
        {
            var line = raw.Trim();
            if (line.StartsWith("RAM=", StringComparison.Ordinal))
            {
                if (ulong.TryParse(line[4..], NumberStyles.None, CultureInfo.InvariantCulture, out var bytes))
                    ramGb = Math.Round(bytes / OneGb, 1);
            }
            else if (line.StartsWith("DOM=", StringComparison.Ordinal))
            {
                isDomain = line[4..].Equals("True", StringComparison.OrdinalIgnoreCase);
            }
            else if (line.StartsWith("VOL=", StringComparison.Ordinal))
            {
                // VOL=<letter>|<capacity>|<free>|<indexing>|<label> - label last, it may contain anything.
                var parts = line[4..].Split('|', 5);
                if (parts.Length < 5) continue;
                var letter = parts[0].TrimEnd('\\').ToUpperInvariant();
                if (letter.Length != 2) continue;
                double sizeGb = 0, freeGb = 0;
                if (ulong.TryParse(parts[1], NumberStyles.None, CultureInfo.InvariantCulture, out var cap))
                    sizeGb = Math.Round(cap / OneGb, 2);
                if (ulong.TryParse(parts[2], NumberStyles.None, CultureInfo.InvariantCulture, out var free))
                    freeGb = Math.Round(free / OneGb, 2);
                bool? indexing = parts[3] switch
                {
                    "True" => true,
                    "False" => false,
                    _ => null,
                };
                drives.Add(new DriveRow(letter, parts[4].Trim(), sizeGb, freeGb,
                    BytesPerCluster: null, indexing,
                    IsSystem: string.Equals(letter, systemLetter, StringComparison.OrdinalIgnoreCase)));
            }
        }

        if (drives.Count == 0)
            drives = FallbackDrives(systemLetter);
        if (ramGb == 0)
            ramGb = Math.Round(GC.GetGCMemoryInfo().TotalAvailableMemoryBytes / OneGb, 1);

        // fsutil per drive for the NTFS cluster size (nullable when it cannot be read).
        for (var i = 0; i < drives.Count; i++)
        {
            var bpc = await DriveUtil.GetBytesPerClusterAsync(ctx, drives[i].Letter);
            drives[i] = drives[i] with { BytesPerCluster = bpc };
        }

        drives = drives.OrderBy(d => d.Letter, StringComparer.OrdinalIgnoreCase).ToList();

        return new SystemSnapshot(osCaption, osVersion, cpu, ramGb, FindXProtectProduct(), drives, isDomain);
    }

    private static string GetOsCaption()
    {
        var product = Reg.GetString(Registry.LocalMachine, CurrentVersionKey, "ProductName") ?? "Windows";
        // ProductName still says "Windows 10" on Windows 11 - correct it via the build number.
        var build = Reg.GetString(Registry.LocalMachine, CurrentVersionKey, "CurrentBuildNumber");
        if (int.TryParse(build, NumberStyles.None, CultureInfo.InvariantCulture, out var b) && b >= 22000)
            product = product.Replace("Windows 10", "Windows 11");
        var displayVersion = Reg.GetString(Registry.LocalMachine, CurrentVersionKey, "DisplayVersion");
        return string.IsNullOrWhiteSpace(displayVersion) ? product : $"{product} {displayVersion}";
    }

    private static List<DriveRow> FallbackDrives(string systemLetter)
    {
        var drives = new List<DriveRow>();
        foreach (var d in DriveInfo.GetDrives())
        {
            try
            {
                if (d.DriveType != DriveType.Fixed || !d.IsReady) continue;
                var letter = d.Name.TrimEnd('\\').ToUpperInvariant();
                if (letter.Length != 2) continue;
                drives.Add(new DriveRow(letter, d.VolumeLabel,
                    Math.Round(d.TotalSize / OneGb, 2),
                    Math.Round(d.AvailableFreeSpace / OneGb, 2),
                    BytesPerCluster: null, IndexingEnabled: null,
                    IsSystem: string.Equals(letter, systemLetter, StringComparison.OrdinalIgnoreCase)));
            }
            catch { }
        }
        return drives;
    }

    /// <summary>Scans both 64-bit and Wow6432Node uninstall keys for a Milestone/XProtect
    /// product, preferring the main VMS entry over device packs, hotfixes and tools.</summary>
    private static string? FindXProtectProduct()
    {
        var entries = new List<(string Name, string? Version)>();
        var roots = new[]
        {
            @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall",
            @"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall",
        };
        foreach (var root in roots)
        {
            try
            {
                using var key = Registry.LocalMachine.OpenSubKey(root);
                if (key is null) continue;
                foreach (var sub in key.GetSubKeyNames())
                {
                    try
                    {
                        using var k = key.OpenSubKey(sub);
                        if (k?.GetValue("DisplayName") is string dn &&
                            (dn.Contains("XProtect", StringComparison.OrdinalIgnoreCase) ||
                             dn.Contains("Milestone", StringComparison.OrdinalIgnoreCase)))
                            entries.Add((dn.Trim(), k.GetValue("DisplayVersion") as string));
                    }
                    catch { }
                }
            }
            catch { }
        }

        // A Milestone install registers 20+ component entries (Log Server, Event Server,
        // Data Collector, hotfixes...). Rank so the actual product wins, and never match
        // ourselves, hotfixes, device packs or SDK tooling.
        static int Rank(string n)
        {
            if (n.Contains("Prepare Win", StringComparison.OrdinalIgnoreCase)) return -1;
            if (n.Contains("Hotfix", StringComparison.OrdinalIgnoreCase)) return -1;
            if (n.Contains("Device Pack", StringComparison.OrdinalIgnoreCase)) return -1;
            if (n.Contains("MIP SDK", StringComparison.OrdinalIgnoreCase)) return -1;
            if (n.Contains("XProtect VMS", StringComparison.OrdinalIgnoreCase)) return 0;
            if (n.Contains("Management Server", StringComparison.OrdinalIgnoreCase) &&
                !n.Contains("Failover", StringComparison.OrdinalIgnoreCase)) return 1;
            if (n.Contains("Recording Server", StringComparison.OrdinalIgnoreCase)) return 2;
            if (n.Contains("Smart Client", StringComparison.OrdinalIgnoreCase)) return 3;
            if (n.Contains("Event Server", StringComparison.OrdinalIgnoreCase)) return 4;
            if (n.Contains("Management Client", StringComparison.OrdinalIgnoreCase)) return 5;
            return 6;
        }

        var best = entries
            .Where(e => Rank(e.Name) >= 0)
            .OrderBy(e => Rank(e.Name))
            .Cast<(string Name, string? Version)?>()
            .FirstOrDefault();
        if (best is null) return null;

        var (name, version) = best.Value;
        if (name.StartsWith("Milestone ", StringComparison.OrdinalIgnoreCase))
            name = name["Milestone ".Length..];

        // Append the release (e.g. DisplayVersion 25.3.x -> "2025 R3") when the entry
        // name doesn't already carry it.
        if (!System.Text.RegularExpressions.Regex.IsMatch(name, @"20\d\d\s*R\d") && version != null)
        {
            var m = System.Text.RegularExpressions.Regex.Match(version, @"^(\d\d)\.(\d)");
            if (m.Success)
                name += $" — 20{m.Groups[1].Value} R{m.Groups[2].Value}";
        }
        return name;
    }
}
