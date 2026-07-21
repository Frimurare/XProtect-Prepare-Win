using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using XProtectPrepareWin.Tasks;
using XProtectPrepareWin.UI;

namespace XProtectPrepareWin;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        if (e.Args.Any(a => a.Equals("--silent", StringComparison.OrdinalIgnoreCase)))
        {
            Loc.Init("en"); // silent runs log in English
            RunSilentAndExit(e.Args);
            return; // never reached - RunSilentAndExit calls Environment.Exit
        }

        // Language and theme must be resolved before any window is created.
        Loc.Init(PrepSettings.Load().Language);
        ThemeManager.Init();

        DispatcherUnhandledException += OnDispatcherUnhandledException;
        var window = new MainWindow();
        MainWindow = window;
        window.Show();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        ThemeManager.Shutdown();
        base.OnExit(e);
    }

    private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        MessageBox.Show(string.Format(Loc.T("app.unexpectedErrorFmt"), e.Exception.Message),
            "XProtect Prepare Win", MessageBoxButton.OK, MessageBoxImage.Error);
        e.Handled = true;
    }

    /// <summary>
    /// Headless mode: --silent [--with-ssh] [--drives &lt;storage|D,E&gt;] [--ntp &lt;windows|global|cc&gt;].
    /// Runs on a dedicated thread (no dispatcher, no deadlock), then exits the process
    /// with 0 when nothing failed, 1 otherwise.
    /// </summary>
    private static void RunSilentAndExit(string[] args)
    {
        int exitCode = 1;
        try
        {
            var worker = new Thread(() =>
            {
                try
                {
                    exitCode = RunSilentAsync(args).GetAwaiter().GetResult();
                }
                catch (Exception ex)
                {
                    try { Console.Error.WriteLine("Silent run failed: " + ex.Message); } catch { }
                    exitCode = 1;
                }
            })
            { IsBackground = false, Name = "SilentPrepRun" };
            worker.Start();
            worker.Join();
        }
        catch
        {
            exitCode = 1;
        }
        Environment.Exit(exitCode);
    }

    private static async Task<int> RunSilentAsync(string[] args)
    {
        var settings = PrepSettings.Load();
        settings.WithSsh = args.Any(a => a.Equals("--with-ssh", StringComparison.OrdinalIgnoreCase));
        settings.DrivesSelection = "";
        for (int i = 0; i < args.Length - 1; i++)
        {
            if (args[i].Equals("--drives", StringComparison.OrdinalIgnoreCase))
                settings.DrivesSelection = args[i + 1].Trim();
            else if (args[i].Equals("--ntp", StringComparison.OrdinalIgnoreCase))
            {
                settings.NtpSource = args[i + 1].Trim().ToLowerInvariant();
                // An explicit --ntp is an operator decision - persist it so later GUI
                // runs check against the same source instead of "correcting" it back.
                var persist = PrepSettings.Load();
                persist.NtpSource = settings.NtpSource;
                persist.Save();
            }
        }

        var tasks = TaskCatalog.CreateAll()
            .Where(t => IncludeInSilentRun(t, settings))
            .ToList();

        var log = new StringBuilder();
        var ctx = new PrepContext
        {
            Settings = settings,
            Log = line =>
            {
                log.AppendLine(line);
                try { Console.WriteLine(line); } catch { }
            }
        };

        var runner = new TaskRunner();
        var summary = await runner.RunAsync(tasks, ctx); // the runner writes the log file itself
        return summary.FailedCount == 0 ? 0 : 1;
    }

    private static bool IncludeInSilentRun(IPrepTask task, PrepSettings settings)
    {
        if (!task.Optional) return true;
        return task switch
        {
            SshServerTask => settings.WithSsh,
            SnmpTask => settings.WithSnmp,
            AvStorageDrivesTask => settings.DrivesSelection.Trim().Length > 0,
            _ => false,
        };
    }
}
