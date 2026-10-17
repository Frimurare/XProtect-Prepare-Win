namespace XProtectPrepareWin.Tasks;

/// <summary>The ordered list of everything a preparation run covers (ported 1:1 from XProtect-Prepare 2.1).</summary>
public static class TaskCatalog
{
    public static List<IPrepTask> CreateAll() => new()
    {
        // Antivirus exclusions
        new AvMilestoneFoldersTask(),
        new AvExtensionsTask(),
        new AvProcessesTask(),
        new AvSmartClientTask(),
        new AvStorageDrivesTask(),

        // Storage optimization
        new StorageBlockSizeTask(),     // check-only: 64 KB allocation unit
        new StorageIndexingTask(),

        // Performance tuning
        new PowerPlanTask(),
        new ServicesDisableTask(),
        new LargeSystemCacheTask(),

        // Remote Desktop
        new RemoteDesktopTask(),

        // Windows cleanup
        new BloatwareAppsTask(),
        new OneDriveRemovalTask(),
        new TeamsRemovalTask(),
        new OutlookOfficeHubRemovalTask(),
        new CopilotRecallTask(),
        new TelemetryTasksTask(),

        // NTP time server
        new NtpServerTask(),

        // MilestonePSTools
        new MilestonePSToolsTask(),
        new MotionHwAccelTask(),        // workaround for the Recording Server memory leak

        // Optional extras (off by default; enabled from Settings)
        new SshServerTask(),
        new SnmpTask(),
    };
}
