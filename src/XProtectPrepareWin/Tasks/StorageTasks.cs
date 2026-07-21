namespace XProtectPrepareWin.Tasks;

/// <summary>Check-only: recording storage must use 64 KB NTFS allocation units.
/// A wrong block size can only be fixed by reformatting, so Apply never runs.</summary>
public sealed class StorageBlockSizeTask : IPrepTask
{
    public string Id => "storage-blocksize";
    public string Name => "64 KB allocation unit check";
    public string Category => Categories.Storage;
    public string Description => "Verifies that storage drives are formatted with 64 KB NTFS clusters (check only)";
    public bool Optional => false;
    public bool CheckOnly => true;

    public async Task<CheckResult> CheckAsync(PrepContext ctx)
    {
        var drives = DriveUtil.FixedNonSystemLetters();
        if (drives.Count == 0)
            return CheckResult.NotApplicable("No non-OS fixed drives");

        var offenders = new List<string>();
        var okDrives = new List<string>();
        foreach (var drive in drives)
        {
            var bytesPerCluster = await DriveUtil.GetBytesPerClusterAsync(ctx, drive);
            if (bytesPerCluster == 65536)
                okDrives.Add(drive);
            else if (bytesPerCluster.HasValue)
                offenders.Add($"{drive} is {bytesPerCluster.Value} — reformat with 64 KB allocation unit for recording storage");
            else
                offenders.Add($"{drive}: block size could not be determined (not NTFS?)");
        }

        return offenders.Count == 0
            ? CheckResult.Done($"All {drives.Count} storage drives use 64 KB allocation units ({string.Join(", ", okDrives)})")
            : CheckResult.Warning(string.Join("; ", offenders));
    }

    public Task ApplyAsync(PrepContext ctx) => Task.CompletedTask;
}

public sealed class StorageIndexingTask : IPrepTask
{
    public string Id => "storage-indexing";
    public string Name => "Disable indexing on storage drives";
    public string Category => Categories.Storage;
    public string Description => "Turns off Windows Search indexing on all non-OS fixed drives";
    public bool Optional => false;
    public bool CheckOnly => false;

    private const string VolumeFilter =
        "Get-CimInstance -ClassName Win32_Volume -ErrorAction SilentlyContinue | " +
        "Where-Object { $_.DriveType -eq 3 -and $_.DriveLetter -and $_.DriveLetter -ne $env:SystemDrive }";

    public async Task<CheckResult> CheckAsync(PrepContext ctx)
    {
        var r = await ctx.PowerShellAsync(
            VolumeFilter + " | ForEach-Object { \"$($_.DriveLetter)=$($_.IndexingEnabled)\" }", 120);

        var enabled = new List<string>();
        var disabled = new List<string>();
        foreach (var raw in r.StdOut.Split('\n'))
        {
            var line = raw.Trim();
            var i = line.IndexOf('=');
            if (i <= 0) continue;
            var drive = line[..i];
            var value = line[(i + 1)..];
            if (value.Equals("True", StringComparison.OrdinalIgnoreCase)) enabled.Add(drive);
            else if (value.Equals("False", StringComparison.OrdinalIgnoreCase)) disabled.Add(drive);
        }

        var total = enabled.Count + disabled.Count;
        if (total == 0)
            return CheckResult.NotApplicable("No non-OS fixed drives");
        if (enabled.Count == 0)
            return CheckResult.Done($"Indexing off on all {total} storage drives ({string.Join(", ", disabled)})");

        var detail = $"Indexing enabled on {enabled.Count} of {total} storage drives: {string.Join(", ", enabled)}";
        return disabled.Count == 0 ? CheckResult.NotDone(detail) : CheckResult.Partial(detail);
    }

    public async Task ApplyAsync(PrepContext ctx)
    {
        var r = await ctx.PowerShellAsync(
            VolumeFilter + " | Where-Object { $_.IndexingEnabled } | " +
            "ForEach-Object { $_ | Set-CimInstance -Property @{ IndexingEnabled = $false } } | Out-Null", 600);
        if (!r.Ok)
            throw new InvalidOperationException("Failed to disable drive indexing: " + Ps.Truncate(r.AllOutput, 300));
    }
}
