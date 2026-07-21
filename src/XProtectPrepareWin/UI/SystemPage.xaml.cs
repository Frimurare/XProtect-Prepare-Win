using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using XProtectPrepareWin.Tasks;

namespace XProtectPrepareWin.UI;

public partial class SystemPage : UserControl
{
    private readonly AppState _state;

    public SystemPage(AppState state)
    {
        InitializeComponent();
        _state = state;
        _state.SnapshotLoaded += Refresh;
        ThemeManager.ThemeChanged += Refresh; // drive badge brushes are precomputed
        Loaded += (_, _) => Refresh();
        Refresh();
    }

    private void Refresh()
    {
        var snap = _state.Snapshot;
        if (snap == null)
        {
            OsText.Text = Loc.T("common.detecting");
            VersionText.Text = "…";
            CpuText.Text = "…";
            RamText.Text = "…";
            DomainText.Text = "…";
            XProtectText.Text = "…";
            DrivesEmptyText.Text = Loc.T("sys.detectingDrives");
            DrivesEmptyText.Visibility = Visibility.Visible;
            return;
        }

        OsText.Text = snap.OsCaption + (Environment.Is64BitOperatingSystem ? " (64-bit)" : " (32-bit)");
        VersionText.Text = snap.OsVersion;
        CpuText.Text = snap.Cpu;
        RamText.Text = $"{snap.RamGb:N1} GB";
        DomainText.Text = snap.IsDomainJoined ? Loc.T("sys.domainJoined") : Loc.T("sys.workgroup");
        XProtectText.Text = snap.XProtectProduct ?? Loc.T("sys.notInstalledReady");

        DrivesList.ItemsSource = snap.Drives.Select(d => new DriveDisplayRow(d)).ToList();
        DrivesEmptyText.Text = Loc.T("sys.noDrives");
        DrivesEmptyText.Visibility = snap.Drives.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
    }
}

/// <summary>Display wrapper around Tasks.DriveRow with badge colors precomputed.</summary>
public sealed class DriveDisplayRow
{
    public string Letter { get; }
    public string Label { get; }
    public string SizeText { get; }
    public string FreeText { get; }
    public string BlockText { get; }
    public Brush BlockBrush { get; }
    public Brush BlockBg { get; }
    public string IndexText { get; }
    public Brush IndexBrush { get; }
    public Brush IndexBg { get; }
    public string SystemTag { get; }

    public DriveDisplayRow(DriveRow d)
    {
        Letter = d.Letter;
        Label = string.IsNullOrWhiteSpace(d.Label) ? "—" : d.Label;
        SizeText = $"{d.SizeGb:N1} GB";
        FreeText = $"{d.FreeGb:N1} GB";
        SystemTag = d.IsSystem ? Loc.T("sys.systemTag") : "";

        if (d.BytesPerCluster is int bytes)
        {
            bool good = bytes == 65536;
            BlockText = bytes >= 1024 ? $"{bytes / 1024} KB" : $"{bytes} B";
            BlockBrush = good ? UiPalette.Green : UiPalette.Amber;
            BlockBg = good ? UiPalette.GreenBg : UiPalette.AmberBg;
        }
        else
        {
            BlockText = "—";
            BlockBrush = UiPalette.Muted;
            BlockBg = UiPalette.GreyBg;
        }

        if (d.IndexingEnabled is bool indexing)
        {
            // For recording drives, indexing off is the desired state.
            IndexText = indexing ? Loc.T("sys.on") : Loc.T("sys.off");
            IndexBrush = indexing ? UiPalette.Amber : UiPalette.Green;
            IndexBg = indexing ? UiPalette.AmberBg : UiPalette.GreenBg;
        }
        else
        {
            IndexText = "—";
            IndexBrush = UiPalette.Muted;
            IndexBg = UiPalette.GreyBg;
        }
    }
}
