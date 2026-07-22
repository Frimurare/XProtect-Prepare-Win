using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Win32;

namespace XProtectPrepareWin.Tasks;

public sealed class SshServerTask : IPrepTask
{
    private const string FirewallRuleName = "SSH Server (XProtect)";

    public string Id => "ssh-server";
    public string Name => "SSH server";
    public string Category => "Optional extras";
    public string Description => "OpenSSH Server (bundled installer, no download needed) with password auth for admins, autostart and TCP 22 open (optional)";
    public bool Optional => true;
    public bool CheckOnly => false;

    private static string SshdConfigPath => Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "ssh", "sshd_config");

    private static (bool Exists, bool PasswordYes, bool MatchGroupCommented) InspectSshdConfig()
    {
        var path = SshdConfigPath;
        if (!File.Exists(path)) return (false, false, false);

        var passwordYes = false;
        var activeMatchGroup = false;
        foreach (var raw in File.ReadAllLines(path))
        {
            if (Regex.IsMatch(raw.Trim(), @"^PasswordAuthentication\s+yes$", RegexOptions.IgnoreCase))
                passwordYes = true;
            // Windows blocks password auth for admins by default via this Match block.
            if (Regex.IsMatch(raw, @"^Match\s+Group\s+administrators", RegexOptions.IgnoreCase))
                activeMatchGroup = true;
        }
        return (true, passwordYes, !activeMatchGroup);
    }

    public async Task<CheckResult> CheckAsync(PrepContext ctx)
    {
        if (!ctx.Settings.WithSsh)
            return CheckResult.NotApplicable("SSH server not selected in settings");

        var installed = Svc.Exists("sshd");
        var issues = new List<string>();
        var compliant = 0;
        const int total = 5;

        if (installed) compliant++;
        else issues.Add("OpenSSH Server not installed");

        var (configExists, passwordYes, matchCommented) = InspectSshdConfig();
        if (configExists && passwordYes) compliant++;
        else issues.Add(configExists ? "PasswordAuthentication not set to yes" : "sshd_config not found");
        if (configExists && matchCommented) compliant++;
        else if (configExists) issues.Add("'Match Group administrators' block still active");
        else issues.Add("admin password auth block state unknown (no sshd_config)");

        var status = installed ? await Svc.GetStatusAsync(ctx, "sshd") : "MISSING";
        if (Svc.GetStart("sshd") == 2 && status.Equals("Running", StringComparison.OrdinalIgnoreCase)) compliant++;
        else issues.Add("sshd service not Automatic and running");

        if (await Firewall.RuleExistsAsync(ctx, FirewallRuleName)) compliant++;
        else issues.Add($"firewall rule '{FirewallRuleName}' missing");

        if (compliant == total)
            return CheckResult.Done("OpenSSH Server installed, password auth enabled, autostart, TCP 22 open");
        var detail = $"{total - compliant} of {total} SSH settings not compliant: {string.Join("; ", issues)}";
        return compliant == 0 ? CheckResult.NotDone(detail) : CheckResult.Partial(detail);
    }

    public async Task ApplyAsync(PrepContext ctx)
    {
        if (!ctx.Settings.WithSsh) return;

        if (!Svc.Exists("sshd"))
        {
            var installed = false;

            // Preferred path: the OpenSSH-Win64 MSI is embedded in this exe, so it
            // installs locally in seconds with no Windows Update / internet access -
            // exactly the locked-down servers where you most want SSH.
            if (TryExtractBundledMsi(out var msiPath))
            {
                var msi = await ctx.RunAsync("msiexec.exe", $"/i \"{msiPath}\" /qn /norestart", 600);
                installed = msi.Ok && Svc.Exists("sshd");
                try { File.Delete(msiPath); } catch { /* temp file, best effort */ }
                if (!installed)
                    ctx.Log("Bundled OpenSSH install did not register the sshd service; falling back to the Windows capability.");
            }

            // Fallback: the Windows "Feature on Demand" capability (needs WU access).
            if (!installed)
            {
                var install = await ctx.PowerShellAsync(
                    "Add-WindowsCapability -Online -Name OpenSSH.Server~~~~0.0.1.0 -ErrorAction Stop | Out-Null", 900);
                if (!install.Ok)
                    throw new InvalidOperationException(
                        "OpenSSH Server install failed. The bundled installer did not register sshd and the " +
                        "Windows capability fallback failed (needs Windows Update access): " +
                        Ps.Truncate(install.AllOutput, 300));
            }
        }

        // Start sshd once so it generates ProgramData\ssh\sshd_config, then stop to edit it.
        await ctx.PowerShellAsync(
            "Start-Service sshd -ErrorAction SilentlyContinue; Start-Sleep -Seconds 2\n" +
            "Stop-Service sshd -ErrorAction SilentlyContinue; Start-Sleep -Seconds 1", 120);

        EditSshdConfig(ctx);

        var start = await ctx.PowerShellAsync("Start-Service sshd -ErrorAction Stop", 120);
        if (!start.Ok)
            throw new InvalidOperationException("Could not start sshd: " + Ps.Truncate(start.AllOutput, 300));
        await ctx.RunAsync("sc.exe", "config sshd start= auto", 60);

        await Firewall.RecreateInboundRuleAsync(ctx, FirewallRuleName, "TCP", 22);
    }

    /// <summary>Write the embedded OpenSSH-Win64 MSI to a temp file. Returns false
    /// if the resource is missing (e.g. a build without the bundle).</summary>
    private static bool TryExtractBundledMsi(out string path)
    {
        path = "";
        try
        {
            using var stream = typeof(SshServerTask).Assembly.GetManifestResourceStream("OpenSSH-Win64.msi");
            if (stream is null) return false;
            var tmp = Path.Combine(Path.GetTempPath(), "XProtectPrepareWin-OpenSSH-Win64.msi");
            using (var file = File.Create(tmp)) stream.CopyTo(file);
            path = tmp;
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>Line edits ported 1:1 from Install-SSHServer.</summary>
    private static void EditSshdConfig(PrepContext ctx)
    {
        var path = SshdConfigPath;
        if (!File.Exists(path))
        {
            ctx.Log($"Warning: sshd_config not found at {path}");
            return;
        }

        var lines = File.ReadAllLines(path);
        for (var i = 0; i < lines.Length; i++)
        {
            var l = lines[i];
            l = l.Replace("#PasswordAuthentication yes", "PasswordAuthentication yes");
            l = l.Replace("PasswordAuthentication no", "PasswordAuthentication yes");
            l = Regex.Replace(l, "^Match Group administrators", "#Match Group administrators");
            l = Regex.Replace(l, @"^\s+AuthorizedKeysFile __PROGRAMDATA__", "#       AuthorizedKeysFile __PROGRAMDATA__");
            lines[i] = l;
        }
        File.WriteAllLines(path, lines);
    }
}

public sealed class SnmpTask : IPrepTask
{
    private static readonly string[] Capabilities =
    {
        "SNMP.Client~~~~0.0.1.0",
        "WMI-SNMP-Provider.Client~~~~0.0.1.0",
    };

    public string Id => "snmp";
    public string Name => "SNMP capabilities";
    public string Category => "Optional extras";
    public string Description => "SNMP client and WMI SNMP provider for network monitoring systems (optional)";
    public bool Optional => true;
    public bool CheckOnly => false;

    private static string WindowsUpdateNote()
    {
        var noAutoUpdate = Reg.GetDword(Registry.LocalMachine,
            @"SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU", "NoAutoUpdate");
        return noAutoUpdate == 1
            ? " (note: Windows Update is disabled - capability install requires it)"
            : "";
    }

    public async Task<CheckResult> CheckAsync(PrepContext ctx)
    {
        if (!ctx.Settings.WithSnmp)
            return CheckResult.NotApplicable("SNMP not selected in settings");

        var r = await ctx.PowerShellAsync(
            "$a = (Get-WindowsCapability -Online -Name " + Ps.Quote(Capabilities[0]) + " -ErrorAction SilentlyContinue).State\n" +
            "$b = (Get-WindowsCapability -Online -Name " + Ps.Quote(Capabilities[1]) + " -ErrorAction SilentlyContinue).State\n" +
            "'A=' + $a\n" +
            "'B=' + $b", 300);

        var snmpInstalled = false;
        var wmiInstalled = false;
        foreach (var raw in r.StdOut.Split('\n'))
        {
            var line = raw.Trim();
            if (line.StartsWith("A=", StringComparison.Ordinal))
                snmpInstalled = line[2..].Equals("Installed", StringComparison.OrdinalIgnoreCase);
            else if (line.StartsWith("B=", StringComparison.Ordinal))
                wmiInstalled = line[2..].Equals("Installed", StringComparison.OrdinalIgnoreCase);
        }

        if (snmpInstalled && wmiInstalled)
            return CheckResult.Done("SNMP client and WMI SNMP provider installed");
        if (!snmpInstalled && !wmiInstalled)
            return CheckResult.NotDone("SNMP capabilities not installed" + WindowsUpdateNote());
        return CheckResult.Partial((snmpInstalled ? "WMI SNMP provider" : "SNMP client") + " missing" + WindowsUpdateNote());
    }

    public async Task ApplyAsync(PrepContext ctx)
    {
        if (!ctx.Settings.WithSnmp) return;

        var r = await ctx.PowerShellAsync(
            "Add-WindowsCapability -Online -Name " + Ps.Quote(Capabilities[0]) + " -ErrorAction Stop | Out-Null\n" +
            "Add-WindowsCapability -Online -Name " + Ps.Quote(Capabilities[1]) + " -ErrorAction Stop | Out-Null", 900);
        if (!r.Ok)
            throw new InvalidOperationException(
                "SNMP capability install failed" + WindowsUpdateNote() + ": " + Ps.Truncate(r.AllOutput, 300));
    }
}
