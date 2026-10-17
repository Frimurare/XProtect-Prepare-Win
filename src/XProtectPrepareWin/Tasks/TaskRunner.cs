using System.Diagnostics;
using System.IO;
using System.Text;

namespace XProtectPrepareWin.Tasks;

/// <summary>
/// Executes a preparation run: check first, apply only what is missing, re-verify.
/// This is where "Already done" vs "Corrected now" is decided.
/// </summary>
public sealed class TaskRunner
{
    /// <summary>Fired on every task state transition (marshal to UI thread in handlers).</summary>
    public event Action<IPrepTask, RunStatus, string>? TaskStatusChanged;

    public async Task<RunSummary> RunAsync(IReadOnlyList<IPrepTask> tasks, PrepContext ctx, CancellationToken ct = default)
    {
        var sw = Stopwatch.StartNew();
        var summary = new RunSummary { StartedLocal = DateTime.Now };
        var log = new StringBuilder();
        void L(string msg)
        {
            var line = $"[{DateTime.Now:HH:mm:ss}] {msg}";
            log.AppendLine(line);
            ctx.Log(line);
        }

        L($"=== XProtect Prepare Win {AppVersion.Version} - preparation run started ===");
        L($"Machine: {Environment.MachineName}  OS: {Environment.OSVersion}");

        // One registry backup per run, before we change anything.
        try
        {
            var backup = await RegistryBackup.CreateAsync(ctx, "PrepareRun");
            L($"Registry backup: {backup}");
        }
        catch (Exception ex)
        {
            L($"WARNING: registry backup failed: {ex.Message}");
        }

        foreach (var task in tasks)
        {
            ct.ThrowIfCancellationRequested();
            RunStatus status;
            string detail;
            try
            {
                Report(task, RunStatus.Checking, "Checking...");
                var check = await task.CheckAsync(ctx);

                if (check.State == CheckState.Done)
                {
                    (status, detail) = (RunStatus.AlreadyDone, check.Detail);
                }
                else if (check.State == CheckState.NotApplicable)
                {
                    (status, detail) = (RunStatus.NotApplicable, check.Detail);
                }
                else if (check.State == CheckState.WarningOnly || task.CheckOnly)
                {
                    (status, detail) = (RunStatus.Warning, check.Detail);
                }
                else
                {
                    Report(task, RunStatus.Applying, "Applying...");
                    L($"{task.Name}: {check.Detail} -> applying");
                    await task.ApplyAsync(ctx);

                    var recheck = await task.CheckAsync(ctx);
                    (status, detail) = recheck.State switch
                    {
                        CheckState.Done => (RunStatus.CorrectedNow, "Corrected and verified"),
                        CheckState.Partial => (RunStatus.Warning, $"Partially corrected: {recheck.Detail}"),
                        CheckState.NotApplicable => (RunStatus.NotApplicable, recheck.Detail),
                        _ => (RunStatus.Failed, $"Applied but verification failed: {recheck.Detail}"),
                    };
                }
            }
            catch (OperationCanceledException) { throw; }
            catch (Exception ex)
            {
                (status, detail) = (RunStatus.Failed, ex.Message);
            }

            L($"{task.Name}: {status} - {detail}");
            Report(task, status, detail);
            summary.Results.Add(new TaskResult(task.Id, task.Name, task.Category, status, detail));
        }

        sw.Stop();
        summary.Duration = sw.Elapsed;
        L($"=== Run finished in {sw.Elapsed:mm\\:ss} - {summary.Results.Count(r => r.Status == RunStatus.AlreadyDone)} already done, " +
          $"{summary.Results.Count(r => r.Status == RunStatus.CorrectedNow)} corrected, {summary.FailedCount} failed ===");

        summary.LogFile = WriteLogFile(log.ToString());
        PrepSettings.SaveLastRun(summary);
        return summary;
    }

    private void Report(IPrepTask task, RunStatus status, string detail) =>
        TaskStatusChanged?.Invoke(task, status, detail);

    private static string WriteLogFile(string content)
    {
        try
        {
            Directory.CreateDirectory(PrepContext.LogFolder);
            var path = Path.Combine(PrepContext.LogFolder, $"XProtectPrepareWin_{DateTime.Now:yyyyMMdd_HHmmss}.txt");
            var header =
                "XProtect Prepare Win - preparation log\r\n" +
                "Built by Ulf Holmström, Happy Problem Solver at Manvarg AB, Sweden\r\n" +
                "Not developed by or affiliated with Milestone Systems A/S\r\n" +
                new string('=', 70) + "\r\n";
            File.WriteAllText(path, header + content, Encoding.UTF8);
            return path;
        }
        catch { return ""; }
    }
}

public static class AppVersion
{
    public const string Version = "1.0.4";
}
