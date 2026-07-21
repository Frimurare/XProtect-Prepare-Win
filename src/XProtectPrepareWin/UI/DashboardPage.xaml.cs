using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Win32;
using XProtectPrepareWin.Tasks;

namespace XProtectPrepareWin.UI;

public partial class DashboardPage : UserControl
{
    private readonly AppState _state;
    private readonly List<CategoryRowViewModel> _categories = new();

    private static readonly (string Name, string SubtitleFallback)[] CategoryMeta =
    {
        (Categories.Antivirus, "Windows Defender exclusions configured"),
        (Categories.Storage, "Drives verified and optimized for recording"),
        (Categories.Performance, "System and power settings optimized"),
        (Categories.Ntp, "Time synchronization configured"),
        (Categories.RemoteDesktop, "Remote access configured and enabled"),
        (Categories.Cleanup, "Unnecessary features and files removed"),
        (Categories.PSTools, "MilestonePSTools installed and configured"),
        (Categories.Logging, "System validated and log created"),
    };

    public DashboardPage(AppState state)
    {
        InitializeComponent();
        _state = state;

        foreach (var (name, subtitleFallback) in CategoryMeta)
        {
            var vm = new CategoryRowViewModel(
                Loc.T($"cat.{name}", name),
                Loc.T($"catsub.{name}", subtitleFallback));
            foreach (var row in _state.Tasks.Where(r => r.Category == name))
                vm.Tasks.Add(row);
            _categories.Add(vm);
        }
        CategoryList.ItemsSource = _categories;

        _state.SnapshotLoaded += RefreshCards;
        _state.RunStateChanged += RefreshAll;
        _state.TaskStatusesChanged += RefreshStatuses;
        ThemeManager.ThemeChanged += RefreshStatuses;
        Loaded += (_, _) => RefreshAll();

        RefreshAll();
    }

    private void RefreshAll()
    {
        RefreshCards();
        RefreshStatuses();
    }

    private void RefreshCards()
    {
        var snap = _state.Snapshot;
        if (snap == null)
        {
            OsValueText.Text = Loc.T("common.detecting");
            OsSubText.Text = "";
            XpValueText.Text = Loc.T("common.detecting");
            XpSubText.Text = "";
        }
        else
        {
            OsValueText.Text = snap.OsCaption;
            OsSubText.Text = snap.OsVersion +
                (Environment.Is64BitOperatingSystem ? " · 64-bit" : " · 32-bit");
            if (snap.XProtectProduct != null)
            {
                XpValueText.Text = snap.XProtectProduct;
                XpSubText.Text = Loc.T("dash.detectedHere");
            }
            else
            {
                XpValueText.Text = Loc.T("dash.notInstalled");
                XpSubText.Text = Loc.T("dash.readyToPrepare");
            }
        }

        var lastRun = _state.LastRun;
        if (lastRun == null)
        {
            LastRunValueText.Text = Loc.T("dash.never");
            LastRunSubText.Text = Loc.T("dash.noRunYet");
            RunButtonText.Text = Loc.T("dash.run");
        }
        else
        {
            LastRunValueText.Text = lastRun.StartedLocal.ToString("d MMM yyyy, HH:mm");
            LastRunSubText.Text = string.Format(Loc.T("dash.durationFmt"), lastRun.Duration.ToString("mm\\:ss"));
            RunButtonText.Text = Loc.T("dash.runAgain");
        }

        RunButton.IsEnabled = !_state.IsRunning;
        ExportLogButton.IsEnabled = !_state.IsRunning;
    }

    private void RefreshStatuses()
    {
        foreach (var category in _categories)
            category.Recompute(_state.LastRun);
        UpdateRingAndStatusPanel();
    }

    private void UpdateRingAndStatusPanel()
    {
        double pct = 0;
        bool anyFailed = false;

        bool anyLive = _state.Tasks.Any(r => r.Status is not (RunStatus.Pending or RunStatus.Skipped));
        if (_state.IsRunning || anyLive)
        {
            var relevant = _state.Tasks.Where(r => r.Included).ToList();
            if (relevant.Count > 0)
            {
                int done = relevant.Count(r => r.Status is RunStatus.AlreadyDone or RunStatus.CorrectedNow);
                pct = 100.0 * done / relevant.Count;
            }
            anyFailed = _state.Tasks.Any(r => r.Status == RunStatus.Failed);
        }
        else if (_state.LastRun != null)
        {
            var results = _state.LastRun.Results;
            if (results.Count > 0)
            {
                int done = results.Count(r => r.Status is RunStatus.AlreadyDone or RunStatus.CorrectedNow);
                pct = 100.0 * done / results.Count;
            }
            anyFailed = _state.LastRun.FailedCount > 0;
        }
        SetRing(pct, anyFailed);

        if (_state.IsRunning)
        {
            ReadyTitleText.Text = Loc.T("dash.preparing");
            ReadySubText.Text = Loc.T("dash.preparingSub");
        }
        else if (_state.LastRun == null)
        {
            ReadyTitleText.Text = Loc.T("dash.noRunYet");
            ReadySubText.Text = Loc.T("dash.runToOptimize");
        }
        else if (_state.LastRun.AllOk)
        {
            ReadyTitleText.Text = Loc.T("dash.systemReady");
            ReadySubText.Text = Loc.T("dash.systemReadySub");
        }
        else
        {
            ReadyTitleText.Text = Loc.T("dash.issuesTitle");
            ReadySubText.Text = string.Format(Loc.T("dash.issuesSubFmt"), _state.LastRun.FailedCount);
        }

        var logFile = _state.LastRun?.LogFile;
        ViewReportButton.IsEnabled = !string.IsNullOrEmpty(logFile) && File.Exists(logFile);
    }

    private void SetRing(double pct, bool failed)
    {
        var brush = failed ? UiPalette.Red : (Brush)UiPalette.Green;
        RingArc.Stroke = brush;
        RingFull.Stroke = brush;
        RingText.Text = $"{Math.Round(pct)}%";

        double angle = Math.Clamp(pct, 0, 100) * 3.6;
        if (angle >= 359.5)
        {
            RingArc.Visibility = Visibility.Collapsed;
            RingFull.Visibility = Visibility.Visible;
            return;
        }
        RingFull.Visibility = Visibility.Collapsed;
        if (angle <= 0.5)
        {
            RingArc.Visibility = Visibility.Collapsed;
            return;
        }

        const double cx = 75, cy = 75, r = 70;
        double rad = (angle - 90) * Math.PI / 180.0;
        var end = new Point(cx + r * Math.Cos(rad), cy + r * Math.Sin(rad));
        var figure = new PathFigure { StartPoint = new Point(cx, cy - r), IsClosed = false, IsFilled = false };
        figure.Segments.Add(new ArcSegment(end, new Size(r, r), 0, angle > 180, SweepDirection.Clockwise, true));
        var geometry = new PathGeometry();
        geometry.Figures.Add(figure);
        RingArc.Data = geometry;
        RingArc.Visibility = Visibility.Visible;
    }

    private async void RunButton_Click(object sender, RoutedEventArgs e)
    {
        await _state.RunPreparationAsync();
    }

    private void ExportLogButton_Click(object sender, RoutedEventArgs e)
    {
        var source = _state.LatestLogFile();
        if (source == null)
        {
            MessageBox.Show(Loc.T("dash.noLogYet"),
                "XProtect Prepare Win", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }
        var dialog = new SaveFileDialog
        {
            Title = Loc.T("dash.exportTitle"),
            FileName = Path.GetFileName(source),
            Filter = Loc.T("dash.logFilter"),
        };
        if (dialog.ShowDialog(Window.GetWindow(this)) == true)
        {
            try
            {
                File.Copy(source, dialog.FileName, overwrite: true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format(Loc.T("dash.exportFailedFmt"), ex.Message),
                    "XProtect Prepare Win", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    private void OpenPSToolsButton_Click(object sender, RoutedEventArgs e) => AppState.OpenPSTools();

    private void ViewReportButton_Click(object sender, RoutedEventArgs e)
    {
        var logFile = _state.LastRun?.LogFile;
        if (!string.IsNullOrEmpty(logFile) && File.Exists(logFile))
            AppState.OpenExternal(logFile);
    }
}

/// <summary>Category roll-up row on the dashboard ("Antivirus exclusions - Completed").</summary>
public sealed class CategoryRowViewModel : INotifyPropertyChanged
{
    public string Title { get; }
    public string Subtitle { get; }
    public ObservableCollection<TaskRowViewModel> Tasks { get; } = new();

    private bool _isExpanded;
    public bool IsExpanded
    {
        get => _isExpanded;
        set
        {
            if (_isExpanded != value)
            {
                _isExpanded = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsExpanded)));
            }
        }
    }

    public string StatusText { get; private set; } = Loc.T("status.notrun");
    public Brush StatusBrush { get; private set; } = UiPalette.Muted;
    public string LeadGlyph { get; private set; } = "\uE823";
    public Brush LeadBrush { get; private set; } = UiPalette.Muted;

    public CategoryRowViewModel(string title, string subtitle)
    {
        Title = title;
        Subtitle = subtitle;
    }

    public void Recompute(RunSummary? lastRun)
    {
        string text;
        Brush brush;
        string glyph;

        if (Tasks.Count == 0)
        {
            // "Logging and verification" has no discrete tasks - it reflects the run itself.
            if (lastRun != null) { text = Loc.T("status.completed"); brush = UiPalette.Green; glyph = "\uE930"; }
            else { text = Loc.T("status.notrun"); brush = UiPalette.Muted; glyph = "\uE823"; }
        }
        else if (Tasks.Any(r => r.Status is RunStatus.Checking or RunStatus.Applying))
        {
            text = Loc.T("status.running"); brush = UiPalette.AccentLight; glyph = "\uE895";
        }
        else if (Tasks.Any(r => r.Status == RunStatus.Failed))
        {
            text = Loc.T("status.failed"); brush = UiPalette.Red; glyph = "\uE711";
        }
        else if (Tasks.Any(r => r.Status == RunStatus.Warning))
        {
            text = Loc.T("status.issues"); brush = UiPalette.Amber; glyph = "\uE7BA";
        }
        else if (Tasks.Any(r => r.Status is RunStatus.AlreadyDone or RunStatus.CorrectedNow))
        {
            text = Loc.T("status.completed"); brush = UiPalette.Green; glyph = "\uE930";
        }
        else if (Tasks.Any(r => r.Status == RunStatus.NotApplicable))
        {
            text = Loc.T("status.na"); brush = UiPalette.Muted; glyph = "\uE738";
        }
        else
        {
            text = Loc.T("status.notrun"); brush = UiPalette.Muted; glyph = "\uE823";
        }

        StatusText = text;
        StatusBrush = brush;
        LeadGlyph = glyph;
        LeadBrush = brush;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(string.Empty));
    }

    public event PropertyChangedEventHandler? PropertyChanged;
}
