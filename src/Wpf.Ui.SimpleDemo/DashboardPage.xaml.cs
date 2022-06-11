using System.Windows.Controls;

namespace Wpf.Ui.SimpleDemo;

/// <summary>
/// Interaction logic for DashboardPage.xaml
/// </summary>
public partial class DashboardPage : Page
{
    public DashboardPage()
    {
        InitializeComponent();
    }

    private void TaskbarStateComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender is not ComboBox comboBox)
            return;

        var parentWindow = System.Windows.Window.GetWindow(this);

        if (parentWindow == null)
            return;

        var selectedIndex = comboBox.SelectedIndex;

        switch (selectedIndex)
        {
            case 1:
                Wpf.Ui.TaskBar.TaskBarProgress.SetValue(
                    parentWindow,
                    Wpf.Ui.TaskBar.TaskBarProgressState.Normal,
                    80);
                break;

            case 2:
                Wpf.Ui.TaskBar.TaskBarProgress.SetValue(
                    parentWindow,
                    Wpf.Ui.TaskBar.TaskBarProgressState.Error,
                    80);
                break;

            case 3:
                Wpf.Ui.TaskBar.TaskBarProgress.SetValue(
                    parentWindow,
                    Wpf.Ui.TaskBar.TaskBarProgressState.Paused,
                    80);
                break;

            case 4:
                Wpf.Ui.TaskBar.TaskBarProgress.SetValue(
                    parentWindow,
                    Wpf.Ui.TaskBar.TaskBarProgressState.Indeterminate,
                    80);
                break;

            default:
                Wpf.Ui.TaskBar.TaskBarProgress.SetState(parentWindow, Wpf.Ui.TaskBar.TaskBarProgressState.None);
                break;
        }
    }
}
