using System.Text.RegularExpressions;
using Microsoft.Win32;

namespace XProtectPrepareWin.Tasks;

public sealed class PowerPlanTask : IPrepTask
{
    private const string HighPerformanceGuid = "8c5e7fda-e8bf-4a96-9a85-a6e23a8c635c";

    // Subgroup GUID, setting GUID, label - ported 1:1 from Set-PowerManagement.
    private static readonly (string Sub, string Setting, string Label)[] SubSettings =
    {
        ("238c9fa8-0aad-41ed-83f4-97be242c8f20", "29f6c1db-86da-48c5-9fdb-f2b67b1f44da", "sleep timeout"),
        ("7516b95f-f776-4464-8c53-06167f40cc99", "3c0bc021-c8a8-4e07-a973-6b14cbcb2b7e", "display timeout"),
        ("0012ee47-9041-4b5d-9b77-535fba8b1442", "6738e2c4-e8a5-4a42-b16a-e040e769756e", "disk timeout"),
    };

    public string Id => "perf-powerplan";
    public string Name => "Power plan: High Performance, never sleep";
    public string Category => Categories.Performance;
    public string Description => "High Performance plan with sleep, display and disk timeouts off (critical for 24/7 recording)";
    public bool Optional => false;
    public bool CheckOnly => false;

    public async Task<CheckResult> CheckAsync(PrepContext ctx)
    {
        var issues = new List<string>();
        var total = 1 + SubSettings.Length;

        var active = await ctx.RunAsync("powercfg.exe", "/getactivescheme", 60);
        var guid = Regex.Match(active.StdOut,
            "[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}");
        if (!guid.Success || !guid.Value.Equals(HighPerformanceGuid, StringComparison.OrdinalIgnoreCase))
            issues.Add("active plan is not High Performance");

        foreach (var (sub, setting, label) in SubSettings)
        {
            var q = await ctx.RunAsync("powercfg.exe", $"/query SCHEME_CURRENT {sub} {setting}", 60);
            // powercfg output is localized; the trailing two 0x values are always the
            // current AC and DC setting indexes regardless of language.
            var hex = Regex.Matches(q.StdOut, "0x[0-9a-fA-F]{1,8}");
            if (hex.Count >= 2)
            {
                var ac = Convert.ToUInt32(hex[hex.Count - 2].Value, 16);
                var dc = Convert.ToUInt32(hex[hex.Count - 1].Value, 16);
                if (ac != 0 || dc != 0)
                    issues.Add($"{label} not set to Never (AC={ac}, DC={dc})");
            }
            else
            {
                issues.Add($"could not read {label}");
            }
        }

        if (issues.Count == 0)
            return CheckResult.Done("High Performance plan active; sleep, display and disk timeouts set to Never");
        var detail = string.Join("; ", issues);
        return issues.Count == total ? CheckResult.NotDone(detail) : CheckResult.Partial(detail);
    }

    public async Task ApplyAsync(PrepContext ctx)
    {
        await ctx.RunAsync("powercfg.exe", $"/setactive {HighPerformanceGuid}", 60);
        foreach (var (sub, setting, _) in SubSettings)
        {
            await ctx.RunAsync("powercfg.exe", $"/setacvalueindex SCHEME_CURRENT {sub} {setting} 0", 60);
            await ctx.RunAsync("powercfg.exe", $"/setdcvalueindex SCHEME_CURRENT {sub} {setting} 0", 60);
        }
        await ctx.RunAsync("powercfg.exe", "/setactive SCHEME_CURRENT", 60);
    }
}

public sealed class ServicesDisableTask : IPrepTask
{
    private static readonly (string Name, string Display)[] Services =
    {
        ("Fax", "Fax Service"),
        ("WSearch", "Windows Search"),
        ("Spooler", "Print Spooler"),
        ("SysMain", "Superfetch/SysMain"),
        ("HomeGroupListener", "HomeGroup Listener"),
        ("HomeGroupProvider", "HomeGroup Provider"),
        ("Themes", "Themes"),
        ("TabletInputService", "Tablet PC Input Service"),
    };

    public string Id => "perf-services";
    public string Name => "Disable unnecessary services";
    public string Category => Categories.Performance;
    public string Description => "Stops and disables Fax, Windows Search, Print Spooler, SysMain, HomeGroup, Themes and Tablet Input";
    public bool Optional => false;
    public bool CheckOnly => false;

    public async Task<CheckResult> CheckAsync(PrepContext ctx)
    {
        var statuses = await Svc.GetStatusesAsync(ctx, Services.Select(s => s.Name));
        var problems = new List<string>();
        var absent = new List<string>();
        var compliant = 0;

        foreach (var (name, display) in Services)
        {
            var status = statuses.GetValueOrDefault(name, "MISSING");
            var exists = !status.Equals("MISSING", StringComparison.OrdinalIgnoreCase) || Svc.Exists(name);
            if (!exists)
            {
                // Services missing on this OS (e.g. HomeGroup on Win11) count as compliant.
                absent.Add(display);
                compliant++;
                continue;
            }

            var start = Svc.GetStart(name);
            var running = status.Equals("Running", StringComparison.OrdinalIgnoreCase);
            if (start == 4 && !running)
                compliant++;
            else
                problems.Add($"{display} (Start={(start.HasValue ? start.Value.ToString() : "?")}{(running ? ", running" : "")})");
        }

        if (problems.Count == 0)
        {
            var suffix = absent.Count > 0 ? $"; {absent.Count} not present on this OS: {string.Join(", ", absent)}" : "";
            return CheckResult.Done($"All {Services.Length} services disabled or absent{suffix}");
        }

        var detail = $"{problems.Count} of {Services.Length} services not disabled: {string.Join(", ", problems)}";
        return compliant == 0 ? CheckResult.NotDone(detail) : CheckResult.Partial(detail);
    }

    public async Task ApplyAsync(PrepContext ctx)
    {
        var statuses = await Svc.GetStatusesAsync(ctx, Services.Select(s => s.Name));
        var present = Services
            .Select(s => s.Name)
            .Where(n => !statuses.GetValueOrDefault(n, "MISSING").Equals("MISSING", StringComparison.OrdinalIgnoreCase) || Svc.Exists(n))
            .ToList();
        if (present.Count == 0) return;

        await ctx.PowerShellAsync(
            "foreach ($n in " + Ps.Array(present) + ") {\n" +
            "  Stop-Service -Name $n -Force -ErrorAction SilentlyContinue\n" +
            "  Set-Service -Name $n -StartupType Disabled -ErrorAction SilentlyContinue\n" +
            "}", 300);

        // Registry fallback for services Set-Service refuses to touch.
        foreach (var name in present)
            if (Svc.Exists(name) && Svc.GetStart(name) != 4)
                Svc.SetStart(name, 4);
    }
}

public sealed class LargeSystemCacheTask : IPrepTask
{
    private const string MemoryManagementKey = @"SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management";

    public string Id => "perf-largesystemcache";
    public string Name => "LargeSystemCache";
    public string Category => Categories.Performance;
    public string Description => "Enables LargeSystemCache for file server style workloads (video recording)";
    public bool Optional => false;
    public bool CheckOnly => false;

    public Task<CheckResult> CheckAsync(PrepContext ctx)
    {
        var value = Reg.GetDword(Registry.LocalMachine, MemoryManagementKey, "LargeSystemCache");
        return Task.FromResult(value == 1
            ? CheckResult.Done("LargeSystemCache = 1")
            : CheckResult.NotDone($"LargeSystemCache is {(value.HasValue ? value.Value.ToString() : "not set")}, expected 1"));
    }

    public Task ApplyAsync(PrepContext ctx)
    {
        Reg.SetDword(Registry.LocalMachine, MemoryManagementKey, "LargeSystemCache", 1);
        return Task.CompletedTask;
    }
}

public sealed class RemoteDesktopTask : IPrepTask
{
    private const string TerminalServerKey = @"SYSTEM\CurrentControlSet\Control\Terminal Server";
    private const string RdpTcpKey = TerminalServerKey + @"\WinStations\RDP-Tcp";
    private static readonly string[] RdpPerfValues = { "fDisableWallpaper", "fDisableFullWindowDrag", "fDisableMenuAnims" };

    // '@FirewallAPI.dll,-28752' is the locale-independent group id behind
    // DisplayGroup "Remote Desktop" (which is localized on non-English Windows).
    private const string FirewallQuery =
        "$r = Get-NetFirewallRule -Group '@FirewallAPI.dll,-28752' -ErrorAction SilentlyContinue\n" +
        "if (-not $r) { $r = Get-NetFirewallRule -DisplayGroup 'Remote Desktop' -ErrorAction SilentlyContinue }\n";

    public string Id => "rdp";
    public string Name => "Enable and optimize Remote Desktop";
    public string Category => Categories.RemoteDesktop;
    public string Description => "Enables RDP, opens its firewall group and turns off wallpaper/animations over RDP";
    public bool Optional => false;
    public bool CheckOnly => false;

    public async Task<CheckResult> CheckAsync(PrepContext ctx)
    {
        var issues = new List<string>();
        var total = 2 + RdpPerfValues.Length;
        var compliant = 0;

        if (Reg.GetDword(Registry.LocalMachine, TerminalServerKey, "fDenyTSConnections") == 0) compliant++;
        else issues.Add("Remote Desktop connections not enabled (fDenyTSConnections != 0)");

        foreach (var name in RdpPerfValues)
        {
            if (Reg.GetDword(Registry.LocalMachine, RdpTcpKey, name) == 1) compliant++;
            else issues.Add($"{name} != 1");
        }

        var fw = await ctx.PowerShellAsync(FirewallQuery +
            "\"$(@($r).Count);$(@($r | Where-Object { $_.Enabled -eq 'True' }).Count)\"", 120);
        var parts = fw.StdOut.Trim().Split(';');
        var fwOk = parts.Length == 2
                   && int.TryParse(parts[0], out var ruleCount)
                   && int.TryParse(parts[1], out var enabledCount)
                   && ruleCount > 0 && enabledCount == ruleCount;
        if (fwOk) compliant++;
        else issues.Add("Remote Desktop firewall rules not all enabled");

        if (issues.Count == 0)
            return CheckResult.Done("Remote Desktop enabled, firewall open, RDP performance options set");
        var detail = string.Join("; ", issues);
        return compliant == 0 ? CheckResult.NotDone(detail) : CheckResult.Partial(detail);
    }

    public async Task ApplyAsync(PrepContext ctx)
    {
        Reg.SetDword(Registry.LocalMachine, TerminalServerKey, "fDenyTSConnections", 0);
        foreach (var name in RdpPerfValues)
            Reg.SetDword(Registry.LocalMachine, RdpTcpKey, name, 1);

        await ctx.PowerShellAsync(FirewallQuery + "$r | Enable-NetFirewallRule -ErrorAction SilentlyContinue", 120);
    }
}
