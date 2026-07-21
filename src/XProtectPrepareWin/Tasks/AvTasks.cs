using System.IO;

namespace XProtectPrepareWin.Tasks;

/// <summary>Exclusion lists ported 1:1 from XProtect-Prepare 2.1.</summary>
internal static class AvData
{
    public static readonly string[] MilestoneFolders =
    {
        @"C:\Program Files\Milestone\",
        @"C:\Program Files (x86)\Milestone\",
        @"C:\ProgramData\Milestone\",
        @"C:\ProgramData\VideoDeviceDrivers\",
        @"C:\ProgramData\VideoOS\",
    };

    public static readonly string[] Extensions = { "blk", "idx", "pic", "pqz", "sts", "ts" };

    public static readonly string[] CoreProcesses =
    {
        "VideoOS.Recorder.Service.exe",
        "VideoOS.Server.Service.exe",
        "VideoOS.Administration.exe",
        "VideoOS.Event.Server.exe",
        "VideoOS.Failover.Service.exe",
        "VideoOS.MobileServer.Service.exe",
        "VideoOS.LPR.Server.exe",
    };

    // Covers BOTH the 2026 R1 VictoriaLogs engine and the legacy SQL Log Server;
    // excluding a process that does not exist is harmless.
    public static readonly string[] LogEngineProcesses =
    {
        "victoria-logs-windows-amd64-prod.exe",   // VictoriaLogs engine (2026 R1+)
        "VideoOS.ServiceWrapper.exe",             // VictoriaLogs service wrapper (2026 R1+)
        "VideoOS.LogServer.exe",                  // legacy SQL Log Server (pre-2026 R1)
    };

    public const string SmartClientExe = @"C:\Program Files\Milestone\XProtect Smart Client\Client.exe";

    public static readonly string[] SmartClientProcesses =
    {
        "Client.exe",
        SmartClientExe,
    };

    // Per-user Smart Client cache is NOT covered by the Program Files / ProgramData folders above.
    public static readonly string[] SmartClientPaths =
    {
        @"C:\Program Files\Milestone\XProtect Smart Client",
        @"C:\Users\*\AppData\Local\VideoOS",
        @"C:\Users\*\AppData\Roaming\VideoOS",
    };
}

public sealed class AvMilestoneFoldersTask : IPrepTask
{
    public string Id => "av-milestone-folders";
    public string Name => "Milestone folder exclusions";
    public string Category => Categories.Antivirus;
    public string Description => "Defender path exclusions for the five Milestone program and data folders on C:";
    public bool Optional => false;
    public bool CheckOnly => false;

    public async Task<CheckResult> CheckAsync(PrepContext ctx)
    {
        var snap = await Defender.GetAsync(ctx);
        if (snap is null)
            return CheckResult.NotDone("Unable to read Windows Defender preferences (Get-MpPreference failed)");
        var missing = AvData.MilestoneFolders.Where(f => !snap.HasPath(f)).ToList();
        return CheckHelper.FromMissing(AvData.MilestoneFolders.Length, missing,
            "Milestone folder exclusions", "All 5 Milestone folder exclusions present");
    }

    public Task ApplyAsync(PrepContext ctx) =>
        Defender.AddExclusionsAsync(ctx, "Path", AvData.MilestoneFolders);
}

public sealed class AvExtensionsTask : IPrepTask
{
    public string Id => "av-extensions";
    public string Name => "Media file extension exclusions";
    public string Category => Categories.Antivirus;
    public string Description => "Defender exclusions for XProtect media database extensions (blk, idx, pic, pqz, sts, ts)";
    public bool Optional => false;
    public bool CheckOnly => false;

    public async Task<CheckResult> CheckAsync(PrepContext ctx)
    {
        var snap = await Defender.GetAsync(ctx);
        if (snap is null)
            return CheckResult.NotDone("Unable to read Windows Defender preferences (Get-MpPreference failed)");
        var missing = AvData.Extensions.Where(e => !snap.HasExtension(e)).ToList();
        return CheckHelper.FromMissing(AvData.Extensions.Length, missing,
            "extension exclusions", "All 6 media file extension exclusions present");
    }

    public Task ApplyAsync(PrepContext ctx) =>
        Defender.AddExclusionsAsync(ctx, "Extension", AvData.Extensions);
}

public sealed class AvProcessesTask : IPrepTask
{
    private static readonly string[] AllProcesses =
        AvData.CoreProcesses.Concat(AvData.LogEngineProcesses).ToArray();

    public string Id => "av-processes";
    public string Name => "VideoOS process exclusions";
    public string Category => Categories.Antivirus;
    public string Description => "Defender process exclusions for the VideoOS services, VictoriaLogs (2026 R1) and legacy Log Server";
    public bool Optional => false;
    public bool CheckOnly => false;

    public async Task<CheckResult> CheckAsync(PrepContext ctx)
    {
        var snap = await Defender.GetAsync(ctx);
        if (snap is null)
            return CheckResult.NotDone("Unable to read Windows Defender preferences (Get-MpPreference failed)");
        var missing = AllProcesses.Where(p => !snap.HasProcess(p)).ToList();
        return CheckHelper.FromMissing(AllProcesses.Length, missing,
            "process exclusions", $"All {AllProcesses.Length} process exclusions present (incl. VictoriaLogs and legacy Log Server)");
    }

    public Task ApplyAsync(PrepContext ctx) =>
        Defender.AddExclusionsAsync(ctx, "Process", AllProcesses);
}

public sealed class AvSmartClientTask : IPrepTask
{
    public string Id => "av-smartclient";
    public string Name => "Smart Client exclusions";
    public string Category => Categories.Antivirus;
    public string Description => "Defender exclusions for XProtect Smart Client and its per-user cache (only when installed)";
    public bool Optional => false;
    public bool CheckOnly => false;

    public async Task<CheckResult> CheckAsync(PrepContext ctx)
    {
        if (!File.Exists(AvData.SmartClientExe))
            return CheckResult.NotApplicable("XProtect Smart Client not installed");

        var snap = await Defender.GetAsync(ctx);
        if (snap is null)
            return CheckResult.NotDone("Unable to read Windows Defender preferences (Get-MpPreference failed)");

        var missing = new List<string>();
        missing.AddRange(AvData.SmartClientProcesses.Where(p => !snap.HasProcess(p)));
        missing.AddRange(AvData.SmartClientPaths.Where(p => !snap.HasPath(p)));
        var total = AvData.SmartClientProcesses.Length + AvData.SmartClientPaths.Length;
        return CheckHelper.FromMissing(total, missing,
            "Smart Client exclusions", "All 5 Smart Client exclusions present (2 processes, 3 paths)");
    }

    public async Task ApplyAsync(PrepContext ctx)
    {
        if (!File.Exists(AvData.SmartClientExe)) return;
        await Defender.AddExclusionsAsync(ctx, "Process", AvData.SmartClientProcesses);
        await Defender.AddExclusionsAsync(ctx, "Path", AvData.SmartClientPaths);
    }
}

public sealed class AvStorageDrivesTask : IPrepTask
{
    public string Id => "av-storage-drives";
    public string Name => "Storage drive exclusions";
    public string Category => Categories.Antivirus;
    public string Description => "Defender path exclusions for whole recording storage drives (C: always excluded)";
    public bool Optional => false;
    public bool CheckOnly => false;

    public async Task<CheckResult> CheckAsync(PrepContext ctx)
    {
        var selection = ctx.Settings.DrivesSelection?.Trim() ?? "";
        if (selection.Length == 0)
            return CheckResult.NotApplicable("No storage drives selected");

        var drives = DriveUtil.ResolveSelection(selection);
        if (drives.Count == 0)
            return CheckResult.NotApplicable($"No matching storage drives for selection '{selection}'");

        var snap = await Defender.GetAsync(ctx);
        if (snap is null)
            return CheckResult.NotDone("Unable to read Windows Defender preferences (Get-MpPreference failed)");

        var missing = drives.Where(d => !snap.HasPath(d + "\\")).Select(d => d + "\\").ToList();
        return CheckHelper.FromMissing(drives.Count, missing,
            "storage drive exclusions", $"All {drives.Count} storage drive exclusions present ({string.Join(", ", drives)})");
    }

    public async Task ApplyAsync(PrepContext ctx)
    {
        var selection = ctx.Settings.DrivesSelection?.Trim() ?? "";
        if (selection.Length == 0) return;
        var drives = DriveUtil.ResolveSelection(selection);
        if (drives.Count == 0) return;
        await Defender.AddExclusionsAsync(ctx, "Path", drives.Select(d => d + "\\"));
    }
}
