using Microsoft.Win32;

namespace XProtectPrepareWin.Tasks;

/// <summary>Windows Update toggle - deliberately a standalone tool, never part of a
/// preparation run (matches the PS script where it is a separate menu option).</summary>
public static class WindowsUpdateTool
{
    private const string AuKey = @"SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU";

    /// <summary>Enabled = NoAutoUpdate absent or != 1.</summary>
    public static Task<bool> IsEnabledAsync(PrepContext ctx)
    {
        var noAutoUpdate = Reg.GetDword(Registry.LocalMachine, AuKey, "NoAutoUpdate");
        return Task.FromResult(noAutoUpdate != 1);
    }

    public static async Task SetEnabledAsync(PrepContext ctx, bool enabled)
    {
        using (var key = Registry.LocalMachine.CreateSubKey(AuKey))
        {
            if (enabled)
            {
                key.DeleteValue("NoAutoUpdate", throwOnMissingValue: false);
                key.DeleteValue("AUOptions", throwOnMissingValue: false);
            }
            else
            {
                key.SetValue("NoAutoUpdate", 1, RegistryValueKind.DWord);
                key.SetValue("AUOptions", 1, RegistryValueKind.DWord);
            }
        }
        await ctx.PowerShellAsync("Restart-Service -Name wuauserv -Force -ErrorAction SilentlyContinue", 120);
    }
}

/// <summary>Fixes the v1.9x NTP peer-format bug (commas without ,0x9 flags) on already
/// deployed machines. Narrow scope by design: no firewall rule, no taskbar seconds.</summary>
public static class NtpRecoveryTool
{
    public static Task<(bool BugDetected, string CurrentValue)> CheckAsync(PrepContext ctx)
    {
        var value = Reg.GetString(Registry.LocalMachine, NtpCore.ParametersKey, "NtpServer") ?? "";
        var bug = value.Contains(',') && !value.Contains(",0x");
        return Task.FromResult((bug, value));
    }

    public static async Task<bool> ApplyFixAsync(PrepContext ctx)
    {
        try
        {
            var isDomain = await MachineInfo.IsDomainJoinedAsync(ctx);
            var syncFlags = isDomain ? "ALL" : "manual";

            await ctx.PowerShellAsync("Stop-Service w32time -Force -ErrorAction SilentlyContinue; Start-Sleep -Seconds 2", 120);

            Reg.SetString(Registry.LocalMachine, NtpCore.ParametersKey, "NtpServer", NtpCore.Peers);
            Reg.SetString(Registry.LocalMachine, NtpCore.ParametersKey, "Type", isDomain ? "AllSync" : "NTP");

            await ctx.RunAsync("sc.exe", "config w32time start= auto", 60);

            Reg.SetDword(Registry.LocalMachine, NtpCore.ConfigKey, "AnnounceFlags", 10);

            await ctx.RunAsync("w32tm.exe",
                $"/config /manualpeerlist:\"{NtpCore.Peers}\" /syncfromflags:{syncFlags} /reliable:yes /update", 120);

            // CRITICAL: w32tm /reliable:yes resets AnnounceFlags back to 5 - re-apply after w32tm.
            Reg.SetDword(Registry.LocalMachine, NtpCore.ConfigKey, "AnnounceFlags", 10);

            var start = await ctx.PowerShellAsync("Start-Service w32time -ErrorAction Stop; Start-Sleep -Seconds 3", 120);
            if (!start.Ok)
                throw new InvalidOperationException("Could not start Windows Time service: " + Ps.Truncate(start.AllOutput, 300));

            // Non-fatal if it fails right after the service starts.
            await ctx.RunAsync("w32tm.exe", "/resync /rediscover", 120);

            await NtpCore.FixDstAsync(ctx);
            return true;
        }
        catch (Exception ex)
        {
            ctx.Log("NTP recovery failed: " + ex.Message);
            await ctx.PowerShellAsync("Start-Service w32time -ErrorAction SilentlyContinue", 60);
            return false;
        }
    }
}
