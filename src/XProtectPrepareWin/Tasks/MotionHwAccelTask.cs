using System.IO;

namespace XProtectPrepareWin.Tasks;

/// <summary>
/// Sets motion detection hardware acceleration to Off on every camera.
/// With "Automatic" on an NVIDIA GPU (NVDEC/CUDA) the Recording Server leaks roughly
/// 100 MB per camera and hour (seen on 2025 R2, 2025 R3 and 2026 R1, with and without
/// hotfixes) until it runs out of memory. Off moves motion detection to the CPU, which
/// costs some CPU but keeps memory flat. Remove this task once Milestone ships a fix.
/// No restart needed: the Recording Server reloads the camera configuration by itself.
/// </summary>
public sealed class MotionHwAccelTask : IPrepTask
{
    private const string ManagementServerService = "Milestone XProtect Management Server";

    public string Id => "motion-hwaccel";
    public string Name => "Motion detection HW acceleration Off";
    public string Category => Categories.PSTools;
    public string Description => "Works around the Recording Server memory leak with NVIDIA-accelerated motion detection";
    public bool Optional => false;
    public bool CheckOnly => false;

    private const string ConnectScript =
        "$ErrorActionPreference = 'Stop'\n" +
        "Import-Module MilestonePSTools\n" +
        "Connect-Vms -ServerAddress http://localhost -AcceptEula | Out-Null\n";

    public async Task<CheckResult> CheckAsync(PrepContext ctx)
    {
        if (!Svc.Exists(ManagementServerService))
            return CheckResult.NotApplicable("No Management Server on this machine - run on the Management Server");

        var gpu = await ctx.PowerShellAsync(
            "(Get-CimInstance Win32_VideoController | Where-Object Name -match 'NVIDIA' | Select-Object -First 1).Name", 120);
        if (string.IsNullOrWhiteSpace(gpu.StdOut))
            return CheckResult.NotApplicable("No NVIDIA GPU - the leak is only confirmed with NVIDIA acceleration");

        var r = await ctx.PowerShellAsync(ConnectScript +
            "$total = 0; $bad = @()\n" +
            "foreach ($cam in Get-VmsCamera -EnableFilter All) {\n" +
            "  $md = $cam.MotionDetectionFolder.MotionDetections | Select-Object -First 1\n" +
            "  if (-not $md) { continue }\n" +
            "  $total++\n" +
            "  if ($md.HardwareAccelerationMode -ne 'Off') { $bad += $cam.Name }\n" +
            "}\n" +
            "'TOTAL=' + $total\n" +
            "'BAD=' + ($bad -join '|')", 600);
        if (!r.Ok)
            return CheckResult.Warning("Could not read cameras via MilestonePSTools: " + Ps.Truncate(r.AllOutput, 300));

        var lines = r.StdOut.Split('\n').Select(l => l.Trim()).ToList();
        var total = int.TryParse(lines.FirstOrDefault(l => l.StartsWith("TOTAL="))?[6..], out var t) ? t : 0;
        var bad = (lines.FirstOrDefault(l => l.StartsWith("BAD="))?[4..] ?? "")
            .Split('|', StringSplitOptions.RemoveEmptyEntries).ToList();

        if (total == 0)
            return CheckResult.NotApplicable("No cameras added yet - run again after adding cameras");
        return CheckHelper.FromMissing(total, bad, "cameras with HW acceleration Off",
            $"HW acceleration Off on all {total} cameras");
    }

    public async Task ApplyAsync(PrepContext ctx)
    {
        // The backout file lists every camera we changed and its previous mode.
        var backout = Path.Combine(PrepContext.LogFolder,
            $"motion-hwaccel-backout_{DateTime.Now:yyyyMMdd_HHmmss}.txt");
        var r = await ctx.PowerShellAsync(ConnectScript +
            "$log = " + Ps.Quote(backout) + "\n" +
            "New-Item -ItemType Directory -Force -Path (Split-Path $log) | Out-Null\n" +
            "'Backout: $cam.MotionDetectionFolder.MotionDetections[0].HardwareAccelerationMode = <previous>; .Save()' | Out-File $log -Encoding UTF8\n" +
            "$changed = 0\n" +
            "foreach ($cam in Get-VmsCamera -EnableFilter All) {\n" +
            "  $md = $cam.MotionDetectionFolder.MotionDetections | Select-Object -First 1\n" +
            "  if (-not $md -or $md.HardwareAccelerationMode -eq 'Off') { continue }\n" +
            "  \"$($cam.Id)`t$($cam.Name)`t$($md.HardwareAccelerationMode)\" | Out-File $log -Append -Encoding UTF8\n" +
            "  $md.HardwareAccelerationMode = 'Off'\n" +
            "  $md.Save()\n" +
            "  $changed++\n" +
            "}\n" +
            "'CHANGED=' + $changed", 900);
        if (!r.Ok)
            throw new InvalidOperationException("Could not set HW acceleration Off: " + Ps.Truncate(r.AllOutput, 400));
        ctx.Log($"Motion detection HW acceleration set to Off ({r.StdOut.Trim()}), backout list: {backout}");
    }
}
