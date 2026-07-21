using System.Windows;
using System.Windows.Controls;
using XProtectPrepareWin.Tasks;

namespace XProtectPrepareWin.UI;

public partial class SettingsPage : UserControl
{
    private readonly AppState _state;
    private readonly bool _initialized;

    public SettingsPage(AppState state)
    {
        InitializeComponent();
        _state = state;

        var settings = _state.Settings;
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

        _initialized = true;
    }

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
