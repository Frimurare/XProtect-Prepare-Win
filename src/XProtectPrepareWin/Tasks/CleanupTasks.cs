using System.IO;
using System.Text;
using Microsoft.Win32;

namespace XProtectPrepareWin.Tasks;

public sealed class BloatwareAppsTask : IPrepTask
{
    // Ported 1:1 from $global:bloatwareApps (incl. 25H2 additions). Xbox apps are
    // deliberately NOT on this list - removing them breaks Win+G screen recording.
    private static readonly (string Name, string Display)[] Apps =
    {
        ("Microsoft.3DBuilder", "3D Builder"),
        ("Microsoft.WindowsAlarms", "Alarms and Clock"),
        ("Microsoft.WindowsCommunicationsApps", "Mail and Calendar"),
        ("Microsoft.GetHelp", "Get Help"),
        ("Microsoft.Getstarted", "Microsoft Tips"),
        ("Microsoft.SkypeApp", "Skype"),
        ("Microsoft.ZuneMusic", "Groove Music"),
        ("Microsoft.WindowsMaps", "Maps"),
        ("Microsoft.MicrosoftSolitaireCollection", "Microsoft Solitaire Collection"),
        ("Microsoft.BingFinance", "Money"),
        ("Microsoft.ZuneVideo", "Movies and TV"),
        ("Microsoft.BingNews", "News"),
        ("Microsoft.Office.OneNote", "OneNote"),
        ("Microsoft.People", "People"),
        ("Microsoft.BingSports", "Sports"),
        ("Microsoft.BingWeather", "Weather"),
        ("Microsoft.MicrosoftStickyNotes", "Sticky Notes"),
        ("Microsoft.OneConnect", "Mobile Plans"),
        ("Microsoft.549981C3F5F10", "Cortana"),
        ("Microsoft.WindowsFeedbackHub", "Feedback Hub"),
        ("Microsoft.YourPhone", "Your Phone"),
        ("Microsoft.MixedReality.Portal", "Mixed Reality Portal"),
        // 25H2 additions
        ("Clipchamp.Clipchamp", "Clipchamp"),
        ("Microsoft.Windows.DevHome", "Dev Home"),
        ("MicrosoftCorporationII.MicrosoftFamily", "Microsoft Family"),
        ("Microsoft.PowerAutomateDesktop", "Power Automate Desktop"),
        ("Microsoft.BingSearch", "Bing Search"),
        ("Disney.37853FC22B2CE", "Disney+"),
        ("SpotifyAB.SpotifyMusic", "Spotify"),
        ("BytedancePte.Ltd.TikTok", "TikTok"),
        ("FACEBOOK.FACEBOOK", "Facebook"),
        ("AmazonVideo.PrimeVideo", "Amazon Prime Video"),
        ("5A894077.McAfeeSecurity", "McAfee Security"),
        ("Microsoft.Windows.Ai.Copilot.Provider", "Copilot Provider"),
        ("Microsoft.Copilot", "Microsoft Copilot"),
        ("Microsoft.LinkedInForWindows", "LinkedIn"),
    };

    public string Id => "cleanup-bloatware";
    public string Name => "Remove bloatware apps";
    public string Category => Categories.Cleanup;
    public string Description => $"Removes {Apps.Length} preinstalled consumer apps (installed and provisioned). Xbox apps untouched";
    public bool Optional => false;
    public bool CheckOnly => false;

    public async Task<CheckResult> CheckAsync(PrepContext ctx)
    {
        var snap = await AppxInventory.GetAsync(ctx);
        if (snap is null)
            return CheckResult.NotDone("Could not enumerate installed AppX packages");

        var present = Apps.Where(a => snap.HasMatch(a.Name)).ToList();
        if (present.Count == 0)
            return CheckResult.Done($"None of the {Apps.Length} bloatware apps present (Xbox apps deliberately untouched)");

        var detail = $"{present.Count} of {Apps.Length} bloatware apps present: {CheckHelper.JoinLimited(present.Select(a => a.Display))}";
        return present.Count == Apps.Length ? CheckResult.NotDone(detail) : CheckResult.Partial(detail);
    }

    public async Task ApplyAsync(PrepContext ctx)
    {
        var snap = await AppxInventory.GetAsync(ctx);
        var targets = (snap is null
                ? Apps.Select(a => a.Name)
                : Apps.Where(a => snap.HasMatch(a.Name)).Select(a => a.Name))
            .ToList();
        if (targets.Count == 0) return;

        // One roundtrip for all apps; per-app errors are swallowed so the rest continue.
        var script =
            "$ErrorActionPreference = 'SilentlyContinue'\n" +
            "$prov = Get-AppxProvisionedPackage -Online\n" +
            "foreach ($n in " + Ps.Array(targets) + ") {\n" +
            "  try { Get-AppxPackage -Name \"*$n*\" -AllUsers | Remove-AppxPackage -AllUsers } catch { }\n" +
            "  try { $prov | Where-Object { $_.DisplayName -like \"*$n*\" } | Remove-AppxProvisionedPackage -Online | Out-Null } catch { }\n" +
            "}";
        await ctx.PowerShellAsync(script, 900);
        AppxInventory.Invalidate();
    }
}

public sealed class OneDriveRemovalTask : IPrepTask
{
    public string Id => "cleanup-onedrive";
    public string Name => "Remove OneDrive";
    public string Category => Categories.Cleanup;
    public string Description => "Uninstalls OneDrive and deletes its leftover folders";
    public bool Optional => false;
    public bool CheckOnly => false;

    private static string[] SetupPaths() => new[]
    {
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "System32", "OneDriveSetup.exe"),
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "SysWOW64", "OneDriveSetup.exe"),
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Microsoft OneDrive", "OneDriveSetup.exe"),
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "Microsoft OneDrive", "OneDriveSetup.exe"),
    };

    private static string[] Folders() => new[]
    {
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Microsoft", "OneDrive"),
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Microsoft", "OneDrive"),
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Microsoft OneDrive"),
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "OneDrive"),
    };

    public Task<CheckResult> CheckAsync(PrepContext ctx)
    {
        var setups = SetupPaths().Where(File.Exists).ToList();
        var folders = Folders().Where(Directory.Exists).ToList();

        if (setups.Count == 0 && folders.Count == 0)
            return Task.FromResult(CheckResult.Done("OneDrive not present (no installer, no folders)"));

        var found = setups.Concat(folders).ToList();
        var detail = $"OneDrive artifacts present: {CheckHelper.JoinLimited(found, 6)}";
        // Installer still present = OneDrive installed; only folders left = partial cleanup.
        return Task.FromResult(setups.Count > 0 ? CheckResult.NotDone(detail) : CheckResult.Partial(detail));
    }

    public async Task ApplyAsync(PrepContext ctx)
    {
        Proc.KillWhere(n => n.Equals("OneDrive", StringComparison.OrdinalIgnoreCase));

        foreach (var setup in SetupPaths())
        {
            if (!File.Exists(setup)) continue;
            await ctx.RunAsync(setup, "/uninstall", 300);
            break; // first found wins, like the PS script
        }

        foreach (var folder in Folders())
            FileUtil.TryDeleteDirectory(folder);
    }
}

public sealed class TeamsRemovalTask : IPrepTask
{
    public string Id => "cleanup-teams";
    public string Name => "Remove Microsoft Teams";
    public string Category => Categories.Cleanup;
    public string Description => "Removes new (AppX) and classic Teams plus leftover folders";
    public bool Optional => false;
    public bool CheckOnly => false;

    private static string[] ClassicUninstallers() => new[]
    {
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Microsoft", "Teams", "Update.exe"),
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Microsoft", "Teams", "Update.exe"),
    };

    private static string[] Folders() => new[]
    {
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Microsoft", "Teams"),
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Microsoft", "Teams"),
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Microsoft", "Teams"),
    };

    public async Task<CheckResult> CheckAsync(PrepContext ctx)
    {
        var snap = await AppxInventory.GetAsync(ctx);
        if (snap is null)
            return CheckResult.NotDone("Could not enumerate installed AppX packages");

        var found = new List<string>();
        var appx = snap.MatchedNames("Teams");
        if (appx.Count > 0) found.Add($"AppX package(s): {CheckHelper.JoinLimited(appx, 4)}");
        found.AddRange(ClassicUninstallers().Where(File.Exists).Select(p => "classic install: " + p));
        found.AddRange(Folders().Where(Directory.Exists).Select(p => "folder: " + p));

        if (found.Count == 0)
            return CheckResult.Done("Teams not present (no AppX packages, classic installs or folders)");

        var detail = "Teams artifacts present: " + CheckHelper.JoinLimited(found, 6);
        // Only leftover folders = partial; any actual install = not done.
        var installRemains = appx.Count > 0 || ClassicUninstallers().Any(File.Exists);
        return installRemains ? CheckResult.NotDone(detail) : CheckResult.Partial(detail);
    }

    public async Task ApplyAsync(PrepContext ctx)
    {
        Proc.KillWhere(n => n.Contains("Teams", StringComparison.OrdinalIgnoreCase));

        await ctx.PowerShellAsync(
            "$ErrorActionPreference = 'SilentlyContinue'\n" +
            "Get-AppxPackage -Name '*Teams*' -AllUsers | Remove-AppxPackage -AllUsers\n" +
            "Get-AppxProvisionedPackage -Online | Where-Object { $_.DisplayName -like '*Teams*' } | " +
            "Remove-AppxProvisionedPackage -Online | Out-Null", 600);
        AppxInventory.Invalidate();

        foreach (var uninstaller in ClassicUninstallers())
        {
            if (!File.Exists(uninstaller)) continue;
            await ctx.RunAsync(uninstaller, "--uninstall", 300);
            break;
        }

        foreach (var folder in Folders())
            FileUtil.TryDeleteDirectory(folder);
    }
}

public sealed class OutlookOfficeHubRemovalTask : IPrepTask
{
    private static readonly (string Fragment, string Display)[] Patterns =
    {
        ("OutlookForWindows", "Outlook App"),
        ("OfficeHub", "Office Hub/365 installer"),
    };

    public string Id => "cleanup-outlook-officehub";
    public string Name => "Remove Outlook App and Office Hub";
    public string Category => Categories.Cleanup;
    public string Description => "Removes the new Outlook app and the Office Hub/365 installer (installed and provisioned)";
    public bool Optional => false;
    public bool CheckOnly => false;

    public async Task<CheckResult> CheckAsync(PrepContext ctx)
    {
        var snap = await AppxInventory.GetAsync(ctx);
        if (snap is null)
            return CheckResult.NotDone("Could not enumerate installed AppX packages");

        var present = Patterns.Where(p => snap.HasMatch(p.Fragment)).ToList();
        if (present.Count == 0)
            return CheckResult.Done("Outlook App and Office Hub not present");

        var detail = "Present: " + string.Join(", ", present.Select(p => p.Display));
        return present.Count == Patterns.Length ? CheckResult.NotDone(detail) : CheckResult.Partial(detail);
    }

    public async Task ApplyAsync(PrepContext ctx)
    {
        await ctx.PowerShellAsync(
            "$ErrorActionPreference = 'SilentlyContinue'\n" +
            "foreach ($pat in @('OutlookForWindows','OfficeHub')) {\n" +
            "  Get-AppxPackage -Name \"*$pat*\" -AllUsers | Remove-AppxPackage -AllUsers\n" +
            "  Get-AppxProvisionedPackage -Online | Where-Object { $_.DisplayName -like \"*$pat*\" } | " +
            "Remove-AppxProvisionedPackage -Online | Out-Null\n" +
            "}", 600);
        AppxInventory.Invalidate();
    }
}

public sealed class CopilotRecallTask : IPrepTask
{
    private const string CopilotKey = @"SOFTWARE\Policies\Microsoft\Windows\WindowsCopilot";
    private const string WindowsAiKey = @"SOFTWARE\Policies\Microsoft\Windows\WindowsAI";

    public string Id => "cleanup-copilot-recall";
    public string Name => "Disable Copilot and Recall";
    public string Category => Categories.Cleanup;
    public string Description => "Group Policy: TurnOffWindowsCopilot=1 and DisableAIDataAnalysis=1";
    public bool Optional => false;
    public bool CheckOnly => false;

    public Task<CheckResult> CheckAsync(PrepContext ctx)
    {
        var copilotOff = Reg.GetDword(Registry.LocalMachine, CopilotKey, "TurnOffWindowsCopilot") == 1;
        var recallOff = Reg.GetDword(Registry.LocalMachine, WindowsAiKey, "DisableAIDataAnalysis") == 1;

        if (copilotOff && recallOff)
            return Task.FromResult(CheckResult.Done("Copilot and Recall disabled by policy"));
        if (!copilotOff && !recallOff)
            return Task.FromResult(CheckResult.NotDone("Neither Copilot nor Recall policy is set"));
        return Task.FromResult(CheckResult.Partial(copilotOff
            ? "Copilot disabled, but Recall policy (DisableAIDataAnalysis) missing"
            : "Recall disabled, but Copilot policy (TurnOffWindowsCopilot) missing"));
    }

    public Task ApplyAsync(PrepContext ctx)
    {
        Reg.SetDword(Registry.LocalMachine, CopilotKey, "TurnOffWindowsCopilot", 1);
        Reg.SetDword(Registry.LocalMachine, WindowsAiKey, "DisableAIDataAnalysis", 1);
        return Task.CompletedTask;
    }
}

public sealed class TelemetryTasksTask : IPrepTask
{
    private static readonly (string Path, string Name)[] TelemetryTasks =
    {
        (@"\Microsoft\Windows\Application Experience\", "Microsoft Compatibility Appraiser"),
        (@"\Microsoft\Windows\Customer Experience Improvement Program\", "Consolidator"),
        (@"\Microsoft\Windows\Customer Experience Improvement Program\", "UsbCeip"),
        (@"\Microsoft\Windows\DiskDiagnostic\", "Microsoft-Windows-DiskDiagnosticDataCollector"),
    };

    public string Id => "cleanup-telemetry";
    public string Name => "Disable telemetry scheduled tasks";
    public string Category => Categories.Cleanup;
    public string Description => "Disables the four CEIP/compatibility/disk telemetry scheduled tasks";
    public bool Optional => false;
    public bool CheckOnly => false;

    private static string BuildQueryScript()
    {
        var sb = new StringBuilder();
        foreach (var (path, name) in TelemetryTasks)
        {
            sb.AppendLine($"$t = Get-ScheduledTask -TaskPath {Ps.Quote(path)} -TaskName {Ps.Quote(name)} -ErrorAction SilentlyContinue");
            sb.AppendLine($"if ($t) {{ {Ps.Quote(name + "=")} + $t.State }} else {{ {Ps.Quote(name + "=MISSING")} }}");
        }
        return sb.ToString();
    }

    public async Task<CheckResult> CheckAsync(PrepContext ctx)
    {
        var r = await ctx.PowerShellAsync(BuildQueryScript(), 120);
        var states = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        foreach (var raw in r.StdOut.Split('\n'))
        {
            var line = raw.Trim();
            var i = line.LastIndexOf('=');
            if (i > 0) states[line[..i]] = line[(i + 1)..];
        }

        var active = new List<string>();
        var compliant = 0;
        foreach (var (_, name) in TelemetryTasks)
        {
            var state = states.GetValueOrDefault(name, "UNKNOWN");
            // Tasks missing on this OS build count as compliant.
            if (state.Equals("Disabled", StringComparison.OrdinalIgnoreCase) ||
                state.Equals("MISSING", StringComparison.OrdinalIgnoreCase))
                compliant++;
            else
                active.Add($"{name} ({state})");
        }

        if (active.Count == 0)
            return CheckResult.Done("All 4 telemetry tasks disabled or absent");
        var detail = $"{active.Count} of {TelemetryTasks.Length} telemetry tasks still enabled: {string.Join(", ", active)}";
        return compliant == 0 ? CheckResult.NotDone(detail) : CheckResult.Partial(detail);
    }

    public async Task ApplyAsync(PrepContext ctx)
    {
        var sb = new StringBuilder();
        foreach (var (path, name) in TelemetryTasks)
        {
            sb.AppendLine($"$t = Get-ScheduledTask -TaskPath {Ps.Quote(path)} -TaskName {Ps.Quote(name)} -ErrorAction SilentlyContinue");
            sb.AppendLine("if ($t) { $t | Disable-ScheduledTask -ErrorAction SilentlyContinue | Out-Null }");
        }
        await ctx.PowerShellAsync(sb.ToString(), 300);
    }
}
