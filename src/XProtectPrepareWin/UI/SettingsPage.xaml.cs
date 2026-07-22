using System.Windows;
using System.Windows.Controls;
using XProtectPrepareWin.Tasks;

namespace XProtectPrepareWin.UI;

public partial class SettingsPage : UserControl
{
    private readonly AppState _state;
    private readonly bool _initialized;
    private readonly string _startupLanguageSetting;

    private static readonly (string Key, string Display)[] Languages =
    {
        ("auto", ""),        // display resolved via Loc at runtime
        ("en", "English"),   // endonyms - identical in every UI language
        ("sv", "Svenska"),
        ("da", "Dansk"),
        ("no", "Norsk"),
        ("fi", "Suomi"),
    };

    public SettingsPage(AppState state)
    {
        InitializeComponent();
        _state = state;

        var settings = _state.Settings;
        _startupLanguageSetting = settings.Language;

        // Language picker
        foreach (var (key, display) in Languages)
        {
            LanguageCombo.Items.Add(new ComboBoxItem
            {
                Content = key == "auto" ? Loc.T("settings.lang.auto") : display,
                Tag = key,
            });
        }
        var langIdx = Array.FindIndex(Languages, l => l.Key.Equals(settings.Language, StringComparison.OrdinalIgnoreCase));
        LanguageCombo.SelectedIndex = langIdx >= 0 ? langIdx : 0;

        if (settings.DrivesSelection.Equals("storage", StringComparison.OrdinalIgnoreCase))
        {
            DrivesStorageRadio.IsChecked = true;
        }
        else if (string.IsNullOrWhiteSpace(settings.DrivesSelection))
        {
            DrivesNoneRadio.IsChecked = true;
        }
        else
        {
            DrivesSpecificRadio.IsChecked = true;
            DrivesBox.Text = settings.DrivesSelection;
            DrivesBox.IsEnabled = true;
        }

        SshToggle.IsChecked = settings.WithSsh;
        SnmpToggle.IsChecked = settings.WithSnmp;
        RestartExplorerToggle.IsChecked = settings.RestartExplorer;

        foreach (var (key, name) in NtpCore.Sources)
            NtpSourceCombo.Items.Add(new ComboBoxItem { Content = Loc.T($"ntp.{key}", name), Tag = key });
        var idx = NtpCore.Sources.ToList().FindIndex(s => s.Key == settings.NtpSource);
        NtpSourceCombo.SelectedIndex = idx >= 0 ? idx : 0;
        UpdateNtpPreview();

        _initialized = true;
    }

    private void LanguageCombo_Changed(object sender, SelectionChangedEventArgs e)
    {
        if (!_initialized) return;
        if (LanguageCombo.SelectedItem is ComboBoxItem { Tag: string key })
        {
            _state.Settings.Language = key;
            Save();
            // Applied on next start - Loc strings are resolved once at startup.
            LanguageRestartHint.Visibility =
                key.Equals(_startupLanguageSetting, StringComparison.OrdinalIgnoreCase)
                    ? Visibility.Collapsed
                    : Visibility.Visible;
        }
    }

    private void NtpSourceCombo_Changed(object sender, SelectionChangedEventArgs e)
    {
        if (!_initialized) return;
        if (NtpSourceCombo.SelectedItem is ComboBoxItem { Tag: string key })
        {
            _state.Settings.NtpSource = key;
            Save();
            UpdateNtpPreview();
        }
    }

    private void UpdateNtpPreview() =>
        NtpPeersPreview.Text = string.Format(Loc.T("settings.ntp.peersFmt"), NtpCore.PeersFor(
            (NtpSourceCombo.SelectedItem as ComboBoxItem)?.Tag as string ?? _state.Settings.NtpSource));

    private void Save() => _state.Settings.Save();

    private void DrivesRadio_Checked(object sender, RoutedEventArgs e)
    {
        if (!_initialized) return;

        DrivesBox.IsEnabled = DrivesSpecificRadio.IsChecked == true;
        if (DrivesStorageRadio.IsChecked == true)
            _state.Settings.DrivesSelection = "storage";
        else if (DrivesNoneRadio.IsChecked == true)
            _state.Settings.DrivesSelection = "";
        else
            _state.Settings.DrivesSelection = DrivesBox.Text.Trim();
        Save();
    }

    private void DrivesBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (!_initialized) return;
        if (DrivesSpecificRadio.IsChecked == true)
        {
            _state.Settings.DrivesSelection = DrivesBox.Text.Trim();
            Save();
        }
    }

    private void SshToggle_Changed(object sender, RoutedEventArgs e)
    {
        if (!_initialized) return;
        _state.Settings.WithSsh = SshToggle.IsChecked == true;
        Save();
        SyncOptionalRow<SshServerTask>(_state.Settings.WithSsh);
    }

    private void SnmpToggle_Changed(object sender, RoutedEventArgs e)
    {
        if (!_initialized) return;
        _state.Settings.WithSnmp = SnmpToggle.IsChecked == true;
        Save();
        SyncOptionalRow<SnmpTask>(_state.Settings.WithSnmp);
    }

    private void RestartExplorerToggle_Changed(object sender, RoutedEventArgs e)
    {
        if (!_initialized) return;
        _state.Settings.RestartExplorer = RestartExplorerToggle.IsChecked == true;
        Save();
    }

    /// <summary>Mirror the optional-component setting onto the Preparation page checkbox.</summary>
    private void SyncOptionalRow<T>(bool included) where T : IPrepTask
    {
        var row = _state.Tasks.FirstOrDefault(r => r.Task is T);
        if (row != null && row.CanToggle)
            row.Included = included;
    }
}
