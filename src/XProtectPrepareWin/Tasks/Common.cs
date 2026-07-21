using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.Json;

namespace XProtectPrepareWin.Tasks;

/// <summary>Result of a task's idempotent state check.</summary>
public enum CheckState
{
    Done,           // everything this task manages is already in the desired state
    NotDone,        // nothing (or almost nothing) is in the desired state
    Partial,        // some items done, some not
    NotApplicable,  // e.g. Smart Client not installed, no storage drives selected
    WarningOnly,    // check-only finding that Apply cannot fix (e.g. wrong block size)
}

public sealed record CheckResult(CheckState State, string Detail)
{
    public static CheckResult Done(string detail = "Already configured") => new(CheckState.Done, detail);
    public static CheckResult NotDone(string detail) => new(CheckState.NotDone, detail);
    public static CheckResult Partial(string detail) => new(CheckState.Partial, detail);
    public static CheckResult NotApplicable(string detail) => new(CheckState.NotApplicable, detail);
    public static CheckResult Warning(string detail) => new(CheckState.WarningOnly, detail);
}

/// <summary>Live status of a task during a preparation run (drives the UI row).</summary>
public enum RunStatus
{
    Pending,
    Checking,
    AlreadyDone,    // green check - was compliant before we touched anything
    Applying,
    CorrectedNow,   // green/blue check - we changed it and re-verified
    Warning,        // check-only finding, or partially corrected
    Failed,
    Skipped,        // deselected by the user
    NotApplicable,  // grey
}

public static class Categories
{
    public const string Antivirus = "Antivirus exclusions";
    public const string Storage = "Storage optimization";
    public const string Performance = "Performance tuning";
    public const string Ntp = "NTP time server";
    public const string RemoteDesktop = "Remote Desktop";
    public const string Cleanup = "Windows cleanup";
    public const string PSTools = "MilestonePSTools";
    public const string Logging = "Logging and verification";
    public const string Optional = "Optional extras";
}

public interface IPrepTask
{
    string Id { get; }
    string Name { get; }
    string Category { get; }
    /// <summary>One line shown under the name, e.g. "Windows Defender exclusions configured".</summary>
    string Description { get; }
    /// <summary>Optional tasks (SSH, SNMP, whole-drive AV) are off by default in a run.</summary>
    bool Optional { get; }
    /// <summary>Check-only tasks report state but are never applied (64 KB block size).</summary>
    bool CheckOnly { get; }
    Task<CheckResult> CheckAsync(PrepContext ctx);
    Task ApplyAsync(PrepContext ctx);
}

public sealed record ProcResult(int ExitCode, string StdOut, string StdErr)
{
    public bool Ok => ExitCode == 0;
    public string AllOutput => (StdOut + "\n" + StdErr).Trim();
}

/// <summary>User-tunable settings, persisted to ProgramData.</summary>
public sealed class PrepSettings
{
    /// <summary>"" = Milestone folders only, "storage" = all non-OS drives, or "D,E".</summary>
    public string DrivesSelection { get; set; } = "storage";
    /// <summary>Upstream NTP source key: "windows" (time.windows.com, default), "global",
    /// or an ISO country code for that country's pool.ntp.org zone. See NtpCore.Sources.</summary>
    public string NtpSource { get; set; } = "windows";
    /// <summary>UI language: "auto" (follow Windows, default), "en", "sv", "da" or "no".</summary>
    public string Language { get; set; } = "auto";
    public bool WithSsh { get; set; }
    public bool WithSnmp { get; set; }
    /// <summary>Restart explorer.exe after enabling seconds in the system clock.</summary>
    public bool RestartExplorer { get; set; } = true;

    public static string SettingsDir =>
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "XProtectPrepareWin");
    private static string SettingsPath => Path.Combine(SettingsDir, "settings.json");
    private static string LastRunPath => Path.Combine(SettingsDir, "lastrun.json");

    public static PrepSettings Load()
    {
        try
        {
            if (File.Exists(SettingsPath))
                return JsonSerializer.Deserialize<PrepSettings>(File.ReadAllText(SettingsPath)) ?? new PrepSettings();
        }
        catch { /* corrupt settings -> defaults */ }
        return new PrepSettings();
    }

    public void Save()
    {
        Directory.CreateDirectory(SettingsDir);
        File.WriteAllText(SettingsPath, JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true }));
    }

    public static RunSummary? LoadLastRun()
    {
        try
        {
            if (File.Exists(LastRunPath))
                return JsonSerializer.Deserialize<RunSummary>(File.ReadAllText(LastRunPath));
        }
        catch { }
        return null;
    }

    public static void SaveLastRun(RunSummary summary)
    {
        Directory.CreateDirectory(SettingsDir);
        File.WriteAllText(LastRunPath, JsonSerializer.Serialize(summary, new JsonSerializerOptions { WriteIndented = true }));
    }
}

public sealed record TaskResult(string Id, string Name, string Category, RunStatus Status, string Detail);

public sealed class RunSummary
{
    public DateTime StartedLocal { get; set; }
    public TimeSpan Duration { get; set; }
    public List<TaskResult> Results { get; set; } = new();
    public string LogFile { get; set; } = "";

    public bool AllOk => Results.All(r => r.Status is RunStatus.AlreadyDone or RunStatus.CorrectedNow
                                                     or RunStatus.NotApplicable or RunStatus.Skipped or RunStatus.Warning)
                         && Results.Any(r => r.Status is RunStatus.AlreadyDone or RunStatus.CorrectedNow);
    public int FailedCount => Results.Count(r => r.Status == RunStatus.Failed);
}

/// <summary>Shared context handed to every task: settings, logging and process helpers.</summary>
public sealed class PrepContext
{
    public const string LogFolder = @"C:\Milestonecheck-ulfh";

    public required PrepSettings Settings { get; init; }
    public required Action<string> Log { get; init; }

    /// <summary>Run an executable, capture output. Never throws on non-zero exit.</summary>
    public async Task<ProcResult> RunAsync(string fileName, string arguments, int timeoutSec = 300)
    {
        var psi = new ProcessStartInfo
        {
            FileName = fileName,
            Arguments = arguments,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true,
            StandardOutputEncoding = Encoding.UTF8,
            StandardErrorEncoding = Encoding.UTF8,
        };
        try
        {
            using var p = Process.Start(psi)!;
            var stdout = p.StandardOutput.ReadToEndAsync();
            var stderr = p.StandardError.ReadToEndAsync();
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(timeoutSec));
            await p.WaitForExitAsync(cts.Token);
            return new ProcResult(p.ExitCode, await stdout, await stderr);
        }
        catch (OperationCanceledException)
        {
            return new ProcResult(-1, "", $"Timeout after {timeoutSec}s: {fileName} {arguments}");
        }
        catch (Exception ex)
        {
            return new ProcResult(-1, "", ex.Message);
        }
    }

    /// <summary>Run a command in Windows PowerShell 5.1 (always present on supported OSes).</summary>
    public Task<ProcResult> PowerShellAsync(string command, int timeoutSec = 600)
    {
        var encoded = Convert.ToBase64String(Encoding.Unicode.GetBytes(command));
        return RunAsync("powershell.exe",
            $"-NoProfile -NonInteractive -ExecutionPolicy Bypass -EncodedCommand {encoded}", timeoutSec);
    }
}
