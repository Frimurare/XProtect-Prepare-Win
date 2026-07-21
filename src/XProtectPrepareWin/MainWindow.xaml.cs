using System.Windows;
using System.Windows.Controls;
using XProtectPrepareWin.UI;

namespace XProtectPrepareWin;

public partial class MainWindow : Window
{
    private readonly Dictionary<string, UserControl> _pages = new();
    private readonly AppState _state;

    public MainWindow()
    {
        InitializeComponent();
        _state = AppState.Current;

        _pages["Dashboard"] = new DashboardPage(_state);
        _pages["System"] = new SystemPage(_state);
        _pages["Preparation"] = new PreparationPage(_state);
        _pages["Tools"] = new ToolsPage(_state);
        _pages["Logs"] = new LogsPage(_state);
        _pages["Settings"] = new SettingsPage(_state);
        _pages["About"] = new AboutPage();

        NavDashboard.IsChecked = true;

        StateChanged += (_, _) =>
        {
            // Compensate for the off-screen frame when a borderless window maximizes.
            RootBorder.Padding = WindowState == WindowState.Maximized ? new Thickness(7) : new Thickness(0);
            MaxButton.Content = WindowState == WindowState.Maximized ? "\uE923" : "\uE922"; // restore / maximize
        };

        Loaded += MainWindow_Loaded;
    }

    private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
        await _state.LoadSnapshotAsync();
    }

    private void Nav_Checked(object sender, RoutedEventArgs e)
    {
        if (sender is RadioButton rb && rb.Content is string key && _pages.TryGetValue(key, out var page))
            PageHost.Content = page;
    }

    private void MinButton_Click(object sender, RoutedEventArgs e) => SystemCommands.MinimizeWindow(this);

    private void MaxButton_Click(object sender, RoutedEventArgs e)
    {
        if (WindowState == WindowState.Maximized)
            SystemCommands.RestoreWindow(this);
        else
            SystemCommands.MaximizeWindow(this);
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e) => Close();
}
