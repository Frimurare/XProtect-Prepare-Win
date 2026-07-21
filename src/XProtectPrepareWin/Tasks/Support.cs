using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Win32;

namespace XProtectPrepareWin.Tasks;

/// <summary>PowerShell literal helpers.</summary>
internal static class Ps
{
    public static string Quote(string s) => "'" + s.Replace("'", "''") + "'";

    public static string Array(IEnumerable<string> items) =>
        "@(" + string.Join(",", items.Select(Quote)) + ")";

    public static string Truncate(string s, int max)
    {
        s = s.Trim();
        return s.Length <= max ? s : s[..max] + "...";
    }
}

internal static class CheckHelper
{
    public static string JoinLimited(IEnumerable<string> items, int max = 10)
    {
        var list = items.ToList();
        return list.Count <= max
            ? string.Join(", ", list)
            : string.Join(", ", list.Take(max)) + $" (+{list.Count - max} more)";
    }

    /// <summary>Standard Done/Partial/NotDone mapping for "N of M items missing" checks.</summary>
    public static CheckResult FromMissing(int total, IReadOnlyCollection<string> missing, string noun, string doneDetail)
    {
        if (missing.Count == 0) return CheckResult.Done(doneDetail);
        var detail = $"{missing.Count} of {total} {noun} missing: {JoinLimited(missing)}";
        return missing.Count == total ? CheckResult.NotDone(detail) : CheckResult.Partial(detail);
    }
}

/// <summary>Registry convenience wrappers (app is x64, so no WOW redirection surprises).</summary>
internal static class Reg
{
    public static int? GetDword(RegistryKey hive, string subKey, string name)
    {
        try
        {
            using var k = hive.OpenSubKey(subKey);
            return k?.GetValue(name) as int?;
        }
        catch { return null; }
    }

    public static string? GetString(RegistryKey hive, string subKey, string name)
    {
        try
        {
            using var k = hive.OpenSubKey(subKey);
            return k?.GetValue(name) as string;
        }
        catch { return null; }
    }

    public static void SetDword(RegistryKey hive, string subKey, string name, int value)
    {
        using var k = hive.CreateSubKey(subKey);
        k.SetValue(name, value, RegistryValueKind.DWord);
    }

    public static void SetString(RegistryKey hive, string subKey, string name, string value)
    {
        using var k = hive.CreateSubKey(subKey);
        k.SetValue(name, value, RegistryValueKind.String);
    }

    public static bool KeyExists(RegistryKey hive, string subKey)
    {
        try
        {
            using var k = hive.OpenSubKey(subKey);
            return k != null;
        }
        catch { return false; }
    }
}

/// <summary>Windows service state helpers. Start type comes from the registry; run state
/// via Get-Service, whose Status enum names are English on all Windows locales
/// (sc.exe output is localized and therefore unsafe to parse).</summary>
internal static class Svc
{
    private const string ServicesRoot = @"SYSTEM\CurrentControlSet\Services\";

    public static bool Exists(string name) => Reg.KeyExists(Registry.LocalMachine, ServicesRoot + name);

    /// <summary>2 = Automatic, 3 = Manual, 4 = Disabled. Null when the service does not exist.</summary>
    public static int? GetStart(string name) => Reg.GetDword(Registry.LocalMachine, ServicesRoot + name, "Start");

    public static void SetStart(string name, int value) =>
        Reg.SetDword(Registry.LocalMachine, ServicesRoot + name, "Start", value);

    /// <summary>Batch status query. Values: Running/Stopped/StartPending/... or MISSING.</summary>
    public static async Task<Dictionary<string, string>> GetStatusesAsync(PrepContext ctx, IEnumerable<string> names)
    {
        var map = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        var script =
            "foreach ($n in " + Ps.Array(names) + ") {\n" +
            "  $s = Get-Service -Name $n -ErrorAction SilentlyContinue\n" +
            "  if ($s) { \"$n=$($s.Status)\" } else { \"$n=MISSING\" }\n" +
            "}";
        var r = await ctx.PowerShellAsync(script, 120);
        foreach (var raw in r.StdOut.Split('\n'))
        {
            var line = raw.Trim();
            var i = line.IndexOf('=');
            if (i > 0) map[line[..i]] = line[(i + 1)..];
        }
        return map;
    }

    public static async Task<string> GetStatusAsync(PrepContext ctx, string name)
    {
        var map = await GetStatusesAsync(ctx, new[] { name });
        return map.GetValueOrDefault(name, "MISSING");
    }
}

internal static class DriveUtil
{
    /// <summary>System drive letter, e.g. "C:".</summary>
    public static string SystemLetter => Path.GetPathRoot(Environment.SystemDirectory)!.TrimEnd('\\');

    /// <summary>All fixed drives except the OS drive, as "D:", "E:".</summary>
    public static List<string> FixedNonSystemLetters()
    {
        var sys = SystemLetter;
        var list = new List<string>();
        foreach (var d in DriveInfo.GetDrives())
        {
            try
            {
                if (d.DriveType != DriveType.Fixed) continue;
                var letter = d.Name.TrimEnd('\\');
                if (letter.Length != 2 || letter[1] != ':') continue;
                if (string.Equals(letter, sys, StringComparison.OrdinalIgnoreCase)) continue;
                list.Add(letter.ToUpperInvariant());
            }
            catch { /* drive not ready */ }
        }
        list.Sort(StringComparer.OrdinalIgnoreCase);
        return list;
    }

    /// <summary>Resolves the settings drive selection: "storage" = all fixed non-OS drives,
    /// otherwise a "D,E" list. C: is always excluded; letters without a matching fixed drive are dropped.</summary>
    public static List<string> ResolveSelection(string selection)
    {
        var fixedDrives = FixedNonSystemLetters();
        if (string.Equals(selection.Trim(), "storage", StringComparison.OrdinalIgnoreCase))
            return fixedDrives;

        var wanted = selection
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(s => s.TrimEnd(':').ToUpperInvariant() + ":")
            .Where(s => s.Length == 2 && char.IsLetter(s[0]))
            .Where(s => !string.Equals(s, SystemLetter, StringComparison.OrdinalIgnoreCase))
            .Distinct()
            .ToList();
        return wanted.Where(w => fixedDrives.Contains(w, StringComparer.OrdinalIgnoreCase)).ToList();
    }

    /// <summary>"Bytes Per Cluster" from fsutil fsinfo ntfsinfo; null when it cannot be determined.</summary>
    public static async Task<int?> GetBytesPerClusterAsync(PrepContext ctx, string letter)
    {
        var r = await ctx.RunAsync("fsutil.exe", $"fsinfo ntfsinfo {letter}", 60);
        var m = Regex.Match(r.StdOut, @"(?im)^\s*bytes\s+per\s+cluster\s*:\s*([0-9][0-9., ]*)");
        if (!m.Success) return null;
        var digits = new string(m.Groups[1].Value.Where(char.IsDigit).ToArray());
        return int.TryParse(digits, NumberStyles.None, CultureInfo.InvariantCulture, out var v) ? v : null;
    }
}

internal sealed class DefenderSnapshot
{
    public HashSet<string> Paths { get; } = new(StringComparer.OrdinalIgnoreCase);
    public HashSet<string> Extensions { get; } = new(StringComparer.OrdinalIgnoreCase);
    public HashSet<string> Processes { get; } = new(StringComparer.OrdinalIgnoreCase);

    public bool HasPath(string path) => Paths.Contains(path.TrimEnd('\\'));
    public bool HasExtension(string ext) => Extensions.Contains(ext.TrimStart('.'));
    public bool HasProcess(string proc) => Processes.Contains(proc);
}

/// <summary>Cached view of Windows Defender exclusions so a full check sweep costs one
/// Get-MpPreference roundtrip, not one per AV task. Applies must call Invalidate().</summary>
internal static class Defender
{
    private static DefenderSnapshot? _cache;
    private static DateTime _cacheTimeUtc;
    private static readonly TimeSpan CacheTtl = TimeSpan.FromSeconds(30);

    public static void Invalidate() => _cache = null;

    /// <summary>Null when Get-MpPreference fails (Defender disabled / third-party AV).</summary>
    public static async Task<DefenderSnapshot?> GetAsync(PrepContext ctx)
    {
        if (_cache != null && DateTime.UtcNow - _cacheTimeUtc < CacheTtl) return _cache;

        var script =
            "try { $p = Get-MpPreference -ErrorAction Stop } catch { Write-Output 'MPERROR'; exit 1 }\n" +
            "'PATH:' + (@($p.ExclusionPath) -join '|')\n" +
            "'EXT:' + (@($p.ExclusionExtension) -join '|')\n" +
            "'PROC:' + (@($p.ExclusionProcess) -join '|')";
        var r = await ctx.PowerShellAsync(script, 120);
        if (r.StdOut.Contains("MPERROR") || !r.StdOut.Contains("PATH:")) return null;

        var snap = new DefenderSnapshot();
        foreach (var raw in r.StdOut.Split('\n'))
        {
            var line = raw.Trim();
            if (line.StartsWith("PATH:", StringComparison.Ordinal))
                AddAll(snap.Paths, line[5..], s => s.TrimEnd('\\'));
            else if (line.StartsWith("EXT:", StringComparison.Ordinal))
                AddAll(snap.Extensions, line[4..], s => s.TrimStart('.'));
            else if (line.StartsWith("PROC:", StringComparison.Ordinal))
                AddAll(snap.Processes, line[5..], s => s);
        }
        _cache = snap;
        _cacheTimeUtc = DateTime.UtcNow;
        return snap;
    }

    private static void AddAll(HashSet<string> target, string joined, Func<string, string> normalize)
    {
        foreach (var item in joined.Split('|', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            target.Add(normalize(item));
    }

    /// <summary>type: "Path", "Extension" or "Process". Continues past per-item errors,
    /// throws if any item failed (recheck decides final state).</summary>
    public static async Task AddExclusionsAsync(PrepContext ctx, string type, IEnumerable<string> values)
    {
        var items = values.ToList();
        if (items.Count == 0) return;
        var script =
            "$errs = @()\n" +
            "foreach ($x in " + Ps.Array(items) + ") {\n" +
            "  try { Add-MpPreference -Exclusion" + type + " $x -ErrorAction Stop } catch { $errs += \"${x}: $($_.Exception.Message)\" }\n" +
            "}\n" +
            "if ($errs.Count -gt 0) { $errs -join '; '; exit 1 }";
        var r = await ctx.PowerShellAsync(script, 300);
        Invalidate();
        if (!r.Ok)
            throw new InvalidOperationException($"Add-MpPreference ({type}) failed: " + Ps.Truncate(r.AllOutput, 400));
    }
}

internal sealed class AppxSnapshot
{
    public List<string> PackageNames { get; } = new();
    public List<string> ProvisionedNames { get; } = new();

    /// <summary>Wildcard "*fragment*" semantics of the PS script (case-insensitive contains).</summary>
    public bool HasMatch(string fragment) =>
        PackageNames.Any(n => n.Contains(fragment, StringComparison.OrdinalIgnoreCase)) ||
        ProvisionedNames.Any(n => n.Contains(fragment, StringComparison.OrdinalIgnoreCase));

    public List<string> MatchedNames(string fragment) =>
        PackageNames.Concat(ProvisionedNames)
            .Where(n => n.Contains(fragment, StringComparison.OrdinalIgnoreCase))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
}

/// <summary>Cached AppX inventory shared by the cleanup tasks - one enumeration roundtrip
/// per sweep instead of one per task. Applies must call Invalidate().</summary>
internal static class AppxInventory
{
    private static AppxSnapshot? _cache;
    private static DateTime _cacheTimeUtc;
    private static readonly TimeSpan CacheTtl = TimeSpan.FromSeconds(30);

    public static void Invalidate() => _cache = null;

    public static async Task<AppxSnapshot?> GetAsync(PrepContext ctx)
    {
        if (_cache != null && DateTime.UtcNow - _cacheTimeUtc < CacheTtl) return _cache;

        var script =
            "$ErrorActionPreference = 'SilentlyContinue'\n" +
            "'PKG:' + ((Get-AppxPackage -AllUsers | Select-Object -ExpandProperty Name -Unique) -join '|')\n" +
            "'PROV:' + ((Get-AppxProvisionedPackage -Online | Select-Object -ExpandProperty DisplayName -Unique) -join '|')";
        var r = await ctx.PowerShellAsync(script, 300);
        if (!r.StdOut.Contains("PKG:")) return null;

        var snap = new AppxSnapshot();
        foreach (var raw in r.StdOut.Split('\n'))
        {
            var line = raw.Trim();
            if (line.StartsWith("PKG:", StringComparison.Ordinal))
                snap.PackageNames.AddRange(line[4..].Split('|', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries));
            else if (line.StartsWith("PROV:", StringComparison.Ordinal))
                snap.ProvisionedNames.AddRange(line[5..].Split('|', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries));
        }
        _cache = snap;
        _cacheTimeUtc = DateTime.UtcNow;
        return snap;
    }
}

internal static class Firewall
{
    public static async Task<bool> RuleExistsAsync(PrepContext ctx, string displayName)
    {
        var r = await ctx.PowerShellAsync(
            "@(Get-NetFirewallRule -DisplayName " + Ps.Quote(displayName) + " -ErrorAction SilentlyContinue).Count", 120);
        return int.TryParse(r.StdOut.Trim(), out var count) && count > 0;
    }

    /// <summary>netsh delete + add, exactly like the PS script (delete failure = rule absent, ignored).</summary>
    public static async Task RecreateInboundRuleAsync(PrepContext ctx, string name, string protocol, int port)
    {
        await ctx.RunAsync("netsh.exe", $"advfirewall firewall delete rule name=\"{name}\"", 60);
        var r = await ctx.RunAsync("netsh.exe",
            $"advfirewall firewall add rule name=\"{name}\" dir=in action=allow protocol={protocol} localport={port}", 60);
        if (!r.Ok)
            throw new InvalidOperationException($"Failed to create firewall rule '{name}': " + Ps.Truncate(r.AllOutput, 300));
    }
}

internal static class MachineInfo
{
    public static async Task<bool> IsDomainJoinedAsync(PrepContext ctx)
    {
        var r = await ctx.PowerShellAsync("(Get-CimInstance Win32_ComputerSystem).PartOfDomain", 120);
        return r.StdOut.Trim().Equals("True", StringComparison.OrdinalIgnoreCase);
    }
}

internal static class Proc
{
    public static void KillWhere(Func<string, bool> nameMatch)
    {
        foreach (var p in Process.GetProcesses())
        {
            try
            {
                if (nameMatch(p.ProcessName))
                {
                    p.Kill(entireProcessTree: true);
                    p.WaitForExit(5000);
                }
            }
            catch { /* access denied / already exited */ }
            finally { p.Dispose(); }
        }
    }
}

internal static class FileUtil
{
    public static bool TryDeleteDirectory(string path)
    {
        try
        {
            if (Directory.Exists(path)) Directory.Delete(path, recursive: true);
            return true;
        }
        catch { return false; }
    }
}
