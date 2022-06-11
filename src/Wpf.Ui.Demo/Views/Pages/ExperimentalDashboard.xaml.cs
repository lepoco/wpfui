// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Demo.ViewModels;

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

    private void ButtonTaskbar_OnClick(object sender, RoutedEventArgs e)
    {
        //switch (TaskbarStateComboBox.SelectedIndex)
        //{
        //    case 0:
        //        TaskbarProgress.SetState()
        //}
    }
}
