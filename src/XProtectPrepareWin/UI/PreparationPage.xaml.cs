using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace XProtectPrepareWin.UI;

public partial class PreparationPage : UserControl
{
    private readonly AppState _state;

    public PreparationPage(AppState state)
    {
        InitializeComponent();
        _state = state;

        var view = new ListCollectionView(_state.Tasks);
        view.GroupDescriptions.Add(new PropertyGroupDescription(nameof(TaskRowViewModel.Category)));
        TaskList.ItemsSource = view;

        _state.RunStateChanged += UpdateButtons;
        Loaded += (_, _) => UpdateButtons();
        UpdateButtons();
    }

    private void UpdateButtons()
    {
        RunButton.IsEnabled = !_state.IsRunning;
        CheckOnlyButton.IsEnabled = !_state.IsRunning;
    }

    private async void RunButton_Click(object sender, RoutedEventArgs e)
    {
        await _state.RunPreparationAsync();
    }

    private async void CheckOnlyButton_Click(object sender, RoutedEventArgs e)
    {
        await _state.CheckOnlyAsync();
    }
}
