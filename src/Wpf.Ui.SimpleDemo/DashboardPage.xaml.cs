// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

namespace Wpf.Ui.SimpleDemo;

/// <summary>
/// Interaction logic for DashboardPage.xaml
/// </summary>
public partial class DashboardPage
{
    public DashboardPage()
    {
        InitializeComponent();
    }

    private void TaskbarStateComboBox_OnSelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
        if (sender is not System.Windows.Controls.ComboBox comboBox)
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
