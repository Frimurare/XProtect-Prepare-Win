using Microsoft.Win32;

namespace XProtectPrepareWin.Tasks;

/// <summary>Shared NTP constants and steps used by NtpServerTask and NtpRecoveryTool.</summary>
internal static class NtpCore
{
    // Swedish NTP pool with the ,0x9 flags (client + special poll interval). The v1.9x
    // scripts wrote plain commas without flags, which breaks w32time peer parsing.
    public const string Peers = "0.se.pool.ntp.org,0x9 1.se.pool.ntp.org,0x9 2.se.pool.ntp.org,0x9 3.se.pool.ntp.org,0x9";

    public const string ParametersKey = @"SYSTEM\CurrentControlSet\Services\W32Time\Parameters";
    public const string ConfigKey = @"SYSTEM\CurrentControlSet\Services\W32Time\Config";
    public const string NtpProviderKey = @"SYSTEM\CurrentControlSet\Services\W32Time\TimeProviders\NtpServer";
    public const string TimeZoneKey = @"SYSTEM\CurrentControlSet\Control\TimeZoneInformation";
    public const string ExplorerAdvancedKey = @"Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced";
    public const string FirewallRuleName = "NTP Server (XProtect)";

    /// <summary>Ensure automatic DST is on and re-apply the timezone to force DST recalculation.</summary>
    public static async Task FixDstAsync(PrepContext ctx)
    {
        if (Reg.GetDword(Registry.LocalMachine, TimeZoneKey, "DynamicDaylightTimeDisabled") == 1)
            Reg.SetDword(Registry.LocalMachine, TimeZoneKey, "DynamicDaylightTimeDisabled", 0);
        await ctx.PowerShellAsync("Set-TimeZone -Id (Get-TimeZone).Id", 120);
    }
}

public sealed class NtpServerTask : IPrepTask
{
    public string Id => "ntp-server";
    public string Name => "NTP time server for cameras";
    public string Category => Categories.Ntp;
    public string Description => "Windows Time as reliable NTP server (se.pool upstream, UDP 123 open, seconds in taskbar clock)";
    public bool Optional => false;
    public bool CheckOnly => false;

    public async Task<CheckResult> CheckAsync(PrepContext ctx)
    {
        // One roundtrip for the facts registry reads cannot give us.
        var ps = await ctx.PowerShellAsync(
            "$fw = @(Get-NetFirewallRule -DisplayName 'NTP Server (XProtect)' -ErrorAction SilentlyContinue).Count\n" +
            "$dom = (Get-CimInstance Win32_ComputerSystem).PartOfDomain\n" +
            "$svc = Get-Service w32time -ErrorAction SilentlyContinue\n" +
            "'FW=' + $fw\n" +
            "'DOM=' + $dom\n" +
            "'SVC=' + $(if ($svc) { $svc.Status } else { 'MISSING' })", 120);

        var firewallOk = false;
        var isDomain = false;
        var svcStatus = "MISSING";
        foreach (var raw in ps.StdOut.Split('\n'))
        {
            var line = raw.Trim();
            if (line.StartsWith("FW=", StringComparison.Ordinal))
                firewallOk = int.TryParse(line[3..], out var c) && c > 0;
            else if (line.StartsWith("DOM=", StringComparison.Ordinal))
                isDomain = line[4..].Equals("True", StringComparison.OrdinalIgnoreCase);
            else if (line.StartsWith("SVC=", StringComparison.Ordinal))
                svcStatus = line[4..];
        }

        var issues = new List<string>();
        var compliant = 0;
        const int total = 8;

        var peers = Reg.GetString(Registry.LocalMachine, NtpCore.ParametersKey, "NtpServer");
        if (peers != null && peers.Contains(",0x9") && peers.Contains("se.pool.ntp.org")) compliant++;
        else issues.Add("NTP peers not set to se.pool.ntp.org with ,0x9 flags");

        if (Reg.GetDword(Registry.LocalMachine, NtpCore.ConfigKey, "AnnounceFlags") == 10) compliant++;
        else issues.Add("AnnounceFlags is not 10");

        if (Reg.GetDword(Registry.LocalMachine, NtpCore.NtpProviderKey, "Enabled") == 1) compliant++;
        else issues.Add("NtpServer provider not enabled");

        var expectedType = isDomain ? "AllSync" : "NTP";
        var type = Reg.GetString(Registry.LocalMachine, NtpCore.ParametersKey, "Type");
        if (string.Equals(type, expectedType, StringComparison.OrdinalIgnoreCase)) compliant++;
        else issues.Add($"sync type is '{type ?? "(none)"}', expected {expectedType}");

        if (Svc.GetStart("w32time") == 2 && svcStatus.Equals("Running", StringComparison.OrdinalIgnoreCase)) compliant++;
        else issues.Add("w32time service not Automatic and running");

        if (firewallOk) compliant++;
        else issues.Add("firewall rule 'NTP Server (XProtect)' missing");

        if (Reg.GetDword(Registry.CurrentUser, NtpCore.ExplorerAdvancedKey, "ShowSecondsInSystemClock") == 1) compliant++;
        else issues.Add("taskbar seconds display not enabled");

        var dst = Reg.GetDword(Registry.LocalMachine, NtpCore.TimeZoneKey, "DynamicDaylightTimeDisabled");
        if (dst is null or 0) compliant++;
        else issues.Add("automatic DST is disabled");

        if (compliant == total)
            return CheckResult.Done("NTP server fully configured (peers, flags, service, firewall, taskbar seconds, DST)");
        var detail = $"{total - compliant} of {total} NTP settings not compliant: {string.Join("; ", issues)}";
        return compliant == 0 ? CheckResult.NotDone(detail) : CheckResult.Partial(detail);
    }

    public async Task ApplyAsync(PrepContext ctx)
    {
        var isDomain = await MachineInfo.IsDomainJoinedAsync(ctx);
        var syncFlags = isDomain ? "ALL" : "manual";

        await ctx.PowerShellAsync("Stop-Service w32time -Force -ErrorAction SilentlyContinue; Start-Sleep -Seconds 2", 120);

        // AnnounceFlags=10 (0x0A): act as NTP server for the cameras but ALSO keep syncing
        // from upstream. 5 would make the box believe it IS a primary source and stop syncing.
        Reg.SetDword(Registry.LocalMachine, NtpCore.ConfigKey, "AnnounceFlags", 10);
        Reg.SetDword(Registry.LocalMachine, NtpCore.NtpProviderKey, "Enabled", 1);
        Reg.SetString(Registry.LocalMachine, NtpCore.ParametersKey, "NtpServer", NtpCore.Peers);
        Reg.SetString(Registry.LocalMachine, NtpCore.ParametersKey, "Type", isDomain ? "AllSync" : "NTP");

        await ctx.RunAsync("sc.exe", "config w32time start= auto", 60);

        await ctx.RunAsync("w32tm.exe",
            $"/config /manualpeerlist:\"{NtpCore.Peers}\" /syncfromflags:{syncFlags} /reliable:yes /update", 120);

        // CRITICAL: w32tm /reliable:yes silently resets AnnounceFlags back to 5, so it MUST
        // be re-applied AFTER w32tm - otherwise the server refuses to sync from upstream.
        Reg.SetDword(Registry.LocalMachine, NtpCore.ConfigKey, "AnnounceFlags", 10);

        await Firewall.RecreateInboundRuleAsync(ctx, NtpCore.FirewallRuleName, "UDP", 123);

        Reg.SetDword(Registry.CurrentUser, NtpCore.ExplorerAdvancedKey, "ShowSecondsInSystemClock", 1);
        if (ctx.Settings.RestartExplorer)
        {
            await ctx.PowerShellAsync(
                "Stop-Process -Name explorer -Force -ErrorAction SilentlyContinue\n" +
                "Start-Sleep -Seconds 2\n" +
                "Start-Process explorer.exe", 60);
        }

        var start = await ctx.PowerShellAsync("Start-Service w32time -ErrorAction Stop; Start-Sleep -Seconds 3", 120);
        if (!start.Ok)
            throw new InvalidOperationException("Could not start Windows Time service: " + Ps.Truncate(start.AllOutput, 300));

        // Non-fatal: resync often fails right after service start before peers respond.
        await ctx.RunAsync("w32tm.exe", "/resync /rediscover", 120);

        await NtpCore.FixDstAsync(ctx);
    }
}
