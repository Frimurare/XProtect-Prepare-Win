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

        _pages["NavDashboard"] = new DashboardPage(_state);
        _pages["NavSystem"] = new SystemPage(_state);
        _pages["NavPreparation"] = new PreparationPage(_state);
        _pages["NavTools"] = new ToolsPage(_state);
        _pages["NavLogs"] = new LogsPage(_state);
        _pages["NavSettings"] = new SettingsPage(_state);
        _pages["NavAbout"] = new AboutPage();

        NavDashboard.IsChecked = true;

        StateChanged += (_, _) =>
        {
            // Compensate for the off-screen frame when a borderless window maximizes.
            RootBorder.Padding = WindowState == WindowState.Maximized ? new Thickness(7) : new Thickness(0);
            MaxButton.Content = WindowState == WindowState.Maximized ? "\uE923" : "\uE922"; // restore / maximize
        };

        // Status badge brushes are provided by code (UiPalette), so nudge every
        // row to re-resolve them when the Windows light/dark preference flips.
        ThemeManager.ThemeChanged += OnThemeChanged;
        Closed += (_, _) => ThemeManager.ThemeChanged -= OnThemeChanged;

        Loaded += MainWindow_Loaded;
    }

    private void OnThemeChanged()
    {
        foreach (var row in _state.Tasks)
            row.RefreshTheme();
    }

    private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
        await _state.LoadSnapshotAsync();
    }

    private void Nav_Checked(object sender, RoutedEventArgs e)
    {
        // Keyed on the control name (content strings are localized).
        if (sender is RadioButton rb && _pages.TryGetValue(rb.Name, out var page))
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

    private void Exit_Click(object sender, RoutedEventArgs e) => Close();
}
