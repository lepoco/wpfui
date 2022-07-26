// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Demo.ViewModels;
using Wpf.Ui.TaskBar;

namespace Wpf.Ui.Demo.Views.Pages;

/// <summary>
/// Interaction logic for ExperimentalDashboard.xaml
/// </summary>
public partial class ExperimentalDashboard : Wpf.Ui.Controls.UiPage, INavigationAware
{
    public ExperimentalDashboard()
    {
        InitializeComponent();

        Loaded += OnLoaded;
    }

    public void OnNavigatedTo()
    {
        System.Diagnostics.Debug.WriteLine($"DEBUG | {typeof(ExperimentalDashboard)} navigated", "Experimental");
    }

    public void OnNavigatedFrom()
    {
        System.Diagnostics.Debug.WriteLine($"DEBUG | {typeof(ExperimentalDashboard)} navigated out", "Experimental");
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        if (DataContext == null)
        {
            System.Diagnostics.Debug.WriteLine($"DEBUG | Experimental dashboard DataContext is null.", "Experimental");
        }
        else
        {
            System.Diagnostics.Debug.WriteLine($"DEBUG | Experimental dashboard DataContext is {DataContext.GetType()}.", "Experimental");
        }
    }

    private void UpdateIdButton_OnClick(object sender, RoutedEventArgs e)
    {
        if (DataContext is not ExperimentalViewModel)
            return;

        ((ExperimentalViewModel)DataContext).GeneralId++;
    }

    private void ButtonExternal_OnClick(object sender, RoutedEventArgs e)
    {
        if (DataContext is not ExperimentalViewModel viewData)
            return;

        viewData.ParentWindow.Navigate(typeof(ExperimentalDashboard));
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
                TaskBarProgress.SetValue(parentWindow, TaskBarProgressState.Normal, 80);
                break;

            case 2:
                TaskBarProgress.SetValue(parentWindow, TaskBarProgressState.Error, 80);
                break;

            case 3:
                TaskBarProgress.SetValue(parentWindow, TaskBarProgressState.Paused, 80);
                break;

            case 4:
                TaskBarProgress.SetValue(parentWindow, TaskBarProgressState.Indeterminate, 80);
                break;

            default:
                TaskBarProgress.SetState(parentWindow, TaskBarProgressState.None);
                break;
        }
    }

    private void ButtonShowFlyoutOnClick(object sender, RoutedEventArgs e)
    {
        MyTestFlyout.Show();
    }
}
