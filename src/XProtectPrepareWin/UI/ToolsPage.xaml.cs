using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using XProtectPrepareWin.Tasks;

namespace XProtectPrepareWin.UI;

public partial class ToolsPage : UserControl
{
    private readonly AppState _state;
    private bool _loadedOnce;
    private bool _ntpBusy;
    private bool _wuBusy;

    public ToolsPage(AppState state)
    {
        InitializeComponent();
        _state = state;
        Loaded += ToolsPage_Loaded;
    }

    private PrepContext Ctx() => new() { Settings = _state.Settings, Log = _state.AddLog };

    private async void ToolsPage_Loaded(object sender, RoutedEventArgs e)
    {
        if (_loadedOnce) return;
        _loadedOnce = true;
        await RefreshNtpAsync();
        await RefreshWindowsUpdateAsync();
    }

    // ---------- NTP recovery ----------

    private async Task RefreshNtpAsync()
    {
        if (_ntpBusy) return;
        _ntpBusy = true;
        NtpFixButton.IsEnabled = false;
        NtpRecheckButton.IsEnabled = false;
        SetBanner(NtpBanner, NtpStatusText, UiPalette.GreyBg, UiPalette.Muted, "Checking NTP configuration…");
        try
        {
            var (bugDetected, currentValue) = await Task.Run(() => NtpRecoveryTool.CheckAsync(Ctx()));
            if (bugDetected)
            {
                SetBanner(NtpBanner, NtpStatusText, UiPalette.AmberBg, UiPalette.Amber,
                    $"Bug detected — current value: {currentValue}");
                NtpFixButton.IsEnabled = true;
            }
            else
            {
                SetBanner(NtpBanner, NtpStatusText, UiPalette.GreenBg, UiPalette.Green,
                    $"OK — {currentValue}");
            }
        }
        catch (Exception ex)
        {
            SetBanner(NtpBanner, NtpStatusText, UiPalette.RedBg, UiPalette.Red,
                "Check failed: " + ex.Message);
        }
        finally
        {
            NtpRecheckButton.IsEnabled = true;
            _ntpBusy = false;
        }
    }

    private async void NtpFixButton_Click(object sender, RoutedEventArgs e)
    {
        if (_ntpBusy) return;
        _ntpBusy = true;
        NtpFixButton.IsEnabled = false;
        NtpRecheckButton.IsEnabled = false;
        SetBanner(NtpBanner, NtpStatusText, UiPalette.AccentBg, UiPalette.AccentLight, "Applying fix…");
        try
        {
            bool ok = await Task.Run(() => NtpRecoveryTool.ApplyFixAsync(Ctx()));
            if (!ok)
            {
                SetBanner(NtpBanner, NtpStatusText, UiPalette.RedBg, UiPalette.Red,
                    "The fix could not be applied — see the Logs page.");
                NtpRecheckButton.IsEnabled = true;
                _ntpBusy = false;
                return;
            }
        }
        catch (Exception ex)
        {
            SetBanner(NtpBanner, NtpStatusText, UiPalette.RedBg, UiPalette.Red,
                "Fix failed: " + ex.Message);
            NtpRecheckButton.IsEnabled = true;
            _ntpBusy = false;
            return;
        }
        _ntpBusy = false;
        await RefreshNtpAsync();
    }

    private async void NtpRecheckButton_Click(object sender, RoutedEventArgs e)
    {
        await RefreshNtpAsync();
    }

    // ---------- Windows Update ----------

    private async Task RefreshWindowsUpdateAsync()
    {
        if (_wuBusy) return;
        _wuBusy = true;
        WuEnableButton.IsEnabled = false;
        WuDisableButton.IsEnabled = false;
        WuRefreshButton.IsEnabled = false;
        SetBanner(WuBanner, WuStatusText, UiPalette.GreyBg, UiPalette.Muted, "Checking Windows Update state…");
        try
        {
            bool enabled = await Task.Run(() => WindowsUpdateTool.IsEnabledAsync(Ctx()));
            if (enabled)
            {
                SetBanner(WuBanner, WuStatusText, UiPalette.AmberBg, UiPalette.Amber,
                    "Windows Update is ENABLED.");
                WuDisableButton.IsEnabled = true;
            }
            else
            {
                SetBanner(WuBanner, WuStatusText, UiPalette.GreenBg, UiPalette.Green,
                    "Windows Update is DISABLED.");
                WuEnableButton.IsEnabled = true;
            }
        }
        catch (Exception ex)
        {
            SetBanner(WuBanner, WuStatusText, UiPalette.RedBg, UiPalette.Red,
                "Check failed: " + ex.Message);
            WuEnableButton.IsEnabled = true;
            WuDisableButton.IsEnabled = true;
        }
        finally
        {
            WuRefreshButton.IsEnabled = true;
            _wuBusy = false;
        }
    }

    private async void WuEnableButton_Click(object sender, RoutedEventArgs e)
    {
        await SetWindowsUpdateAsync(true);
    }

    private async void WuDisableButton_Click(object sender, RoutedEventArgs e)
    {
        await SetWindowsUpdateAsync(false);
    }

    private async void WuRefreshButton_Click(object sender, RoutedEventArgs e)
    {
        await RefreshWindowsUpdateAsync();
    }

    private async Task SetWindowsUpdateAsync(bool enabled)
    {
        if (_wuBusy) return;
        _wuBusy = true;
        WuEnableButton.IsEnabled = false;
        WuDisableButton.IsEnabled = false;
        WuRefreshButton.IsEnabled = false;
        SetBanner(WuBanner, WuStatusText, UiPalette.AccentBg, UiPalette.AccentLight,
            enabled ? "Enabling Windows Update…" : "Disabling Windows Update…");
        try
        {
            await Task.Run(() => WindowsUpdateTool.SetEnabledAsync(Ctx(), enabled));
        }
        catch (Exception ex)
        {
            SetBanner(WuBanner, WuStatusText, UiPalette.RedBg, UiPalette.Red,
                "Change failed: " + ex.Message);
            WuRefreshButton.IsEnabled = true;
            _wuBusy = false;
            return;
        }
        _wuBusy = false;
        await RefreshWindowsUpdateAsync();
    }

    // ---------- Simple launchers ----------

    private void OpenPSToolsButton_Click(object sender, RoutedEventArgs e) => AppState.OpenPSTools();

    private void OpenLogFolderButton_Click(object sender, RoutedEventArgs e) => AppState.OpenLogFolder();

    private async void ReportButton_Click(object sender, RoutedEventArgs e)
    {
        ReportButton.IsEnabled = false;
        ReportStatusText.Text = "Running check-only sweep… this can take a few minutes.";
        try
        {
            var path = await _state.GenerateSystemReportAsync();
            ReportStatusText.Text = "Report written: " + path;
            AppState.OpenExternal(path);
        }
        catch (Exception ex)
        {
            ReportStatusText.Text = "Report failed: " + ex.Message;
        }
        finally
        {
            ReportButton.IsEnabled = true;
        }
    }

    private static void SetBanner(Border banner, TextBlock text, Brush background, Brush foreground, string message)
    {
        banner.Background = background;
        text.Foreground = foreground;
        text.Text = message;
    }
}
