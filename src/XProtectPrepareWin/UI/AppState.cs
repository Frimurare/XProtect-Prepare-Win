using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using XProtectPrepareWin.Tasks;

namespace XProtectPrepareWin.UI;

/// <summary>
/// Brushes used by the code-driven status badges. Resolved from the active theme
/// dictionary (ThemeDark/ThemeLight) so they follow light/dark switches; the
/// hex values are dark-theme fallbacks in case a resource is ever missing.
/// </summary>
internal static class UiPalette
{
    public static SolidColorBrush Text => Get("TextBrush", "#E8ECF4");
    public static SolidColorBrush Muted => Get("MutedBrush", "#8A94AD");
    public static SolidColorBrush Accent => Get("AccentBrush", "#2B6CEB");
    public static SolidColorBrush AccentLight => Get("AccentLightBrush", "#5B8DEF");
    public static SolidColorBrush Green => Get("GreenBrush", "#35C759");
    public static SolidColorBrush Amber => Get("AmberBrush", "#F5A623");
    public static SolidColorBrush Red => Get("RedBrush", "#E5484D");
    public static SolidColorBrush Teal => Get("TealBrush", "#2EC4B6");

    public static SolidColorBrush GreyBg => Get("GreyBgBrush", "#1A8A94AD");
    public static SolidColorBrush AccentBg => Get("AccentBgBrush", "#1F2B6CEB");
    public static SolidColorBrush GreenBg => Get("GreenBgBrush", "#2035C759");
    public static SolidColorBrush AmberBg => Get("AmberBgBrush", "#20F5A623");
    public static SolidColorBrush RedBg => Get("RedBgBrush", "#20E5484D");
    public static SolidColorBrush TealBg => Get("TealBgBrush", "#202EC4B6");

    private static SolidColorBrush Get(string key, string fallbackHex)
    {
        if (Application.Current?.TryFindResource(key) is SolidColorBrush brush)
            return brush;
        var fallback = new SolidColorBrush((Color)ColorConverter.ConvertFromString(fallbackHex));
        fallback.Freeze();
        return fallback;
    }
}

/// <summary>One row on the Preparation page - wraps an IPrepTask with live run status.</summary>
public sealed class TaskRowViewModel : INotifyPropertyChanged
{
    public IPrepTask Task { get; }

    public TaskRowViewModel(IPrepTask task)
    {
        Task = task;
        _included = !task.Optional;
    }

    public string Name => Loc.T($"task.{Task.Id}.name", Task.Name);
    public string Description => Loc.T($"task.{Task.Id}.desc", Task.Description);
    /// <summary>Engine category (English) - used for matching, never shown directly.</summary>
    public string Category => Task.Category;
    /// <summary>Localized category name for group headers.</summary>
    public string CategoryDisplay => Loc.T($"cat.{Task.Category}", Task.Category);
    public bool Optional => Task.Optional;

    private bool _included;
    public bool Included
    {
        get => _included;
        set { if (_included != value) { _included = value; OnPropertyChanged(); } }
    }

    private bool _canToggle = true;
    public bool CanToggle
    {
        get => _canToggle;
        set { if (_canToggle != value) { _canToggle = value; OnPropertyChanged(); } }
    }

    private RunStatus _status = RunStatus.Pending;
    public RunStatus Status
    {
        get => _status;
        set { _status = value; RaiseAll(); }
    }

    private string _detail = "";
    public string Detail
    {
        get => _detail;
        set { _detail = value ?? ""; RaiseAll(); }
    }

    public bool HasDetail => _detail.Length > 0;
    public bool IsBusy => _status is RunStatus.Checking or RunStatus.Applying;
    public bool HasGlyph => !IsBusy && StatusGlyph.Length > 0;

    public string StatusGlyph => _status switch
    {
        RunStatus.AlreadyDone => "\uE73E",   // check mark
        RunStatus.CorrectedNow => "\uE90F",  // wrench (repair)
        RunStatus.Warning => "\uE7BA",       // warning triangle
        RunStatus.Failed => "\uE711",        // cross
        _ => "",
    };

    public string StatusText => _status switch
    {
        RunStatus.Pending => Loc.T("status.notrun"),
        RunStatus.Checking => Loc.T("status.checking"),
        RunStatus.Applying => Loc.T("status.applying"),
        RunStatus.AlreadyDone => Loc.T("status.alreadydone"),
        RunStatus.CorrectedNow => Loc.T("status.corrected"),
        RunStatus.Warning => Loc.T("status.warning"),
        RunStatus.Failed => Loc.T("status.failed"),
        RunStatus.Skipped => Loc.T("status.skipped"),
        RunStatus.NotApplicable => Loc.T("status.na"),
        _ => "",
    };

    public Brush StatusForeground => _status switch
    {
        RunStatus.Checking or RunStatus.Applying => UiPalette.AccentLight,
        RunStatus.AlreadyDone => UiPalette.Green,
        RunStatus.CorrectedNow => UiPalette.Teal,
        RunStatus.Warning => UiPalette.Amber,
        RunStatus.Failed => UiPalette.Red,
        _ => UiPalette.Muted,
    };

    public Brush StatusBackground => _status switch
    {
        RunStatus.Checking or RunStatus.Applying => UiPalette.AccentBg,
        RunStatus.AlreadyDone => UiPalette.GreenBg,
        RunStatus.CorrectedNow => UiPalette.TealBg,
        RunStatus.Warning => UiPalette.AmberBg,
        RunStatus.Failed => UiPalette.RedBg,
        _ => UiPalette.GreyBg,
    };

    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>Re-raise all bindings so status brushes re-resolve after a theme switch.</summary>
    public void RefreshTheme() => RaiseAll();

    private void OnPropertyChanged([CallerMemberName] string? name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    private void RaiseAll() =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(string.Empty));
}

/// <summary>Shared UI state: settings, task rows, run engine plumbing and log stream.</summary>
public sealed class AppState
{
    private static AppState? _current;
    public static AppState Current => _current ??= new AppState();

    public PrepSettings Settings { get; }
    public ObservableCollection<TaskRowViewModel> Tasks { get; } = new();
    public RunSummary? LastRun { get; private set; }
    public SystemSnapshot? Snapshot { get; private set; }
    public bool IsRunning { get; private set; }
    public List<string> LogLines { get; } = new();

    /// <summary>Run started or finished.</summary>
    public event Action? RunStateChanged;
    /// <summary>Any task row changed status.</summary>
    public event Action? TaskStatusesChanged;
    public event Action? SnapshotLoaded;
    public event Action<string>? LogAdded;

    private readonly Dispatcher _dispatcher;

    private AppState()
    {
        Settings = PrepSettings.Load();
        LastRun = PrepSettings.LoadLastRun();
        _dispatcher = Application.Current.Dispatcher;
        foreach (var task in TaskCatalog.CreateAll())
            Tasks.Add(new TaskRowViewModel(task));

        // Seed the rows with the persisted result of the last run, so the
        // Dashboard rollup and Preparation page reflect reality on startup
        // instead of showing everything as "Not run".
        if (LastRun != null)
        {
            foreach (var row in Tasks)
            {
                var result = LastRun.Results.FirstOrDefault(r => r.Id == row.Task.Id);
                if (result != null)
                {
                    row.Status = result.Status;
                    row.Detail = result.Detail;
                }
            }
        }
    }

    /// <summary>Thread-safe log append; marshals to the UI thread.</summary>
    public void AddLog(string line)
    {
        if (_dispatcher.CheckAccess())
        {
            LogLines.Add(line);
            LogAdded?.Invoke(line);
        }
        else
        {
            _dispatcher.InvokeAsync(() =>
            {
                LogLines.Add(line);
                LogAdded?.Invoke(line);
            });
        }
    }

    public async Task LoadSnapshotAsync()
    {
        try
        {
            var ctx = new PrepContext { Settings = Settings, Log = _ => { } };
            Snapshot = await System.Threading.Tasks.Task.Run(() => SystemInfo.GetAsync(ctx));
            SnapshotLoaded?.Invoke();
        }
        catch (Exception ex)
        {
            AddLog("WARNING: system detection failed: " + ex.Message);
        }
    }

    /// <summary>Full preparation run (check + apply) on the tasks the user has included.</summary>
    public async Task RunPreparationAsync()
    {
        if (IsRunning) return;
        IsRunning = true;
        foreach (var row in Tasks)
        {
            row.CanToggle = false;
            row.Detail = "";
            row.Status = row.Included ? RunStatus.Pending : RunStatus.Skipped;
        }
        RunStateChanged?.Invoke();
        TaskStatusesChanged?.Invoke();

        var runner = new TaskRunner();
        runner.TaskStatusChanged += (task, status, detail) =>
        {
            _dispatcher.InvokeAsync(() =>
            {
                var row = Tasks.FirstOrDefault(r => r.Task.Id == task.Id);
                if (row != null)
                {
                    row.Status = status;
                    row.Detail = detail;
                }
                TaskStatusesChanged?.Invoke();
            });
        };

        var ctx = new PrepContext { Settings = Settings, Log = AddLog };
        var selected = Tasks.Where(r => r.Included).Select(r => r.Task).ToList();
        try
        {
            LastRun = await System.Threading.Tasks.Task.Run(() => runner.RunAsync(selected, ctx));
        }
        catch (Exception ex)
        {
            AddLog("ERROR: preparation run crashed: " + ex.Message);
        }
        finally
        {
            IsRunning = false;
            foreach (var row in Tasks) row.CanToggle = true;
            RunStateChanged?.Invoke();
            TaskStatusesChanged?.Invoke();
        }
    }

    /// <summary>Check-only sweep: reports state on the included tasks, never applies anything.</summary>
    public async Task CheckOnlyAsync()
    {
        if (IsRunning) return;
        IsRunning = true;
        foreach (var row in Tasks)
        {
            row.CanToggle = false;
            row.Detail = "";
            row.Status = row.Included ? RunStatus.Pending : RunStatus.Skipped;
        }
        RunStateChanged?.Invoke();
        TaskStatusesChanged?.Invoke();

        var ctx = new PrepContext { Settings = Settings, Log = AddLog };
        try
        {
            foreach (var row in Tasks.Where(r => r.Included))
            {
                row.Status = RunStatus.Checking;
                TaskStatusesChanged?.Invoke();
                try
                {
                    var check = await System.Threading.Tasks.Task.Run(() => row.Task.CheckAsync(ctx));
                    row.Status = check.State switch
                    {
                        CheckState.Done => RunStatus.AlreadyDone,
                        CheckState.NotApplicable => RunStatus.NotApplicable,
                        _ => RunStatus.Warning,
                    };
                    row.Detail = check.Detail;
                }
                catch (Exception ex)
                {
                    row.Status = RunStatus.Failed;
                    row.Detail = ex.Message;
                }
                TaskStatusesChanged?.Invoke();
            }
        }
        finally
        {
            IsRunning = false;
            foreach (var row in Tasks) row.CanToggle = true;
            RunStateChanged?.Invoke();
            TaskStatusesChanged?.Invoke();
        }
    }

    /// <summary>Writes a check-only text report to the log folder; returns the file path.</summary>
    public async Task<string> GenerateSystemReportAsync()
    {
        var ctx = new PrepContext { Settings = Settings, Log = AddLog };
        return await System.Threading.Tasks.Task.Run(async () =>
        {
            var sb = new StringBuilder();
            sb.AppendLine($"XProtect Prepare Win {AppVersion.Version} - system report (check only, no changes made)");
            sb.AppendLine($"Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}   Machine: {Environment.MachineName}");
            sb.AppendLine("Built by Ulf Holmström, Manvarg AB. Not affiliated with Milestone Systems A/S.");
            sb.AppendLine(new string('=', 70));

            try
            {
                var snap = await SystemInfo.GetAsync(ctx);
                sb.AppendLine($"OS:       {snap.OsCaption} ({snap.OsVersion})");
                sb.AppendLine($"CPU:      {snap.Cpu}");
                sb.AppendLine($"RAM:      {snap.RamGb:N1} GB");
                sb.AppendLine($"Domain:   {(snap.IsDomainJoined ? "Domain joined" : "Workgroup")}");
                sb.AppendLine($"XProtect: {snap.XProtectProduct ?? "Not installed"}");
                foreach (var d in snap.Drives)
                {
                    var block = d.BytesPerCluster.HasValue ? $"{d.BytesPerCluster.Value / 1024} KB" : "?";
                    var idx = d.IndexingEnabled.HasValue ? (d.IndexingEnabled.Value ? "on" : "off") : "?";
                    sb.AppendLine($"Drive {d.Letter}  {d.Label}  {d.SizeGb:N1} GB total, {d.FreeGb:N1} GB free, block {block}, indexing {idx}{(d.IsSystem ? " [system]" : "")}");
                }
            }
            catch (Exception ex)
            {
                sb.AppendLine("System detection failed: " + ex.Message);
            }

            sb.AppendLine(new string('-', 70));
            foreach (var task in TaskCatalog.CreateAll())
            {
                try
                {
                    var check = await task.CheckAsync(ctx);
                    sb.AppendLine($"[{check.State,-14}] {task.Name}: {check.Detail}");
                }
                catch (Exception ex)
                {
                    sb.AppendLine($"[ERROR         ] {task.Name}: {ex.Message}");
                }
            }

            Directory.CreateDirectory(PrepContext.LogFolder);
            var path = Path.Combine(PrepContext.LogFolder, $"SystemReport_{DateTime.Now:yyyyMMdd_HHmmss}.txt");
            File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
            return path;
        });
    }

    /// <summary>Most recent preparation log file, or null if none exists.</summary>
    public string? LatestLogFile()
    {
        var fromRun = LastRun?.LogFile;
        if (!string.IsNullOrEmpty(fromRun) && File.Exists(fromRun)) return fromRun;
        try
        {
            if (Directory.Exists(PrepContext.LogFolder))
                return Directory.GetFiles(PrepContext.LogFolder, "*.txt")
                    .OrderByDescending(File.GetLastWriteTime)
                    .FirstOrDefault();
        }
        catch { }
        return null;
    }

    public static void OpenExternal(string path)
    {
        try { Process.Start(new ProcessStartInfo(path) { UseShellExecute = true }); }
        catch { }
    }

    public static void OpenLogFolder()
    {
        try
        {
            Directory.CreateDirectory(PrepContext.LogFolder);
            Process.Start(new ProcessStartInfo("explorer.exe", PrepContext.LogFolder) { UseShellExecute = true });
        }
        catch { }
    }

    public static void OpenPSTools()
    {
        try
        {
            Process.Start(new ProcessStartInfo("powershell.exe",
                "-NoExit -Command \"Import-Module MilestonePSTools\"") { UseShellExecute = true });
        }
        catch { }
    }
}
