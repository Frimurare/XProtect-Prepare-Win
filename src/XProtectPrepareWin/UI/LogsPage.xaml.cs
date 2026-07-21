using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using XProtectPrepareWin.Tasks;

namespace XProtectPrepareWin.UI;

public partial class LogsPage : UserControl
{
    private readonly AppState _state;

    public LogsPage(AppState state)
    {
        InitializeComponent();
        _state = state;

        LogBox.Text = string.Join(Environment.NewLine, _state.LogLines);
        _state.LogAdded += OnLogAdded;
        _state.RunStateChanged += RefreshFileList;
        Loaded += (_, _) => RefreshFileList();
    }

    private void OnLogAdded(string line)
    {
        LogBox.AppendText(line + Environment.NewLine);
        if (AutoScrollToggle.IsChecked == true)
            LogBox.ScrollToEnd();
    }

    private void RefreshFileList()
    {
        try
        {
            if (!Directory.Exists(PrepContext.LogFolder))
            {
                FilesList.ItemsSource = Array.Empty<LogFileItem>();
                return;
            }
            FilesList.ItemsSource = Directory.GetFiles(PrepContext.LogFolder, "*.txt")
                .Select(p => new FileInfo(p))
                .OrderByDescending(f => f.LastWriteTime)
                .Select(f => new LogFileItem(f.Name, f.LastWriteTime.ToString("yyyy-MM-dd HH:mm"), f.FullName))
                .ToList();
        }
        catch
        {
            FilesList.ItemsSource = Array.Empty<LogFileItem>();
        }
    }

    private void FilesList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (FilesList.SelectedItem is LogFileItem item && File.Exists(item.Path))
            AppState.OpenExternal(item.Path);
    }

    private void OpenLogFolderButton_Click(object sender, RoutedEventArgs e) => AppState.OpenLogFolder();
}

public sealed record LogFileItem(string Name, string Date, string Path);
