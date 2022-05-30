// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;
using WPFUI.Controls.Interfaces;
using WPFUI.Demo.Views.Windows;

namespace WPFUI.Demo.Views.Pages;

/// <summary>
/// Interaction logic for ExperimentalDashboard.xaml
/// </summary>
public partial class ExperimentalDashboard : WPFUI.Controls.UiPage, INavigable
{
    public ExperimentalDashboard()
    {
        InitializeComponent();

        Loaded += OnLoaded;
    }

    public void OnNavigationRequest(INavigation sender)
    {
        System.Diagnostics.Debug.WriteLine($"DEBUG | {typeof(ExperimentalDashboard)} navigated by {sender.GetType()}", "Experimental");
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
        if (DataContext is not ExperimentalViewData)
            return;

        ((ExperimentalViewData)DataContext).GeneralId++;
    }

    private void ButtonExternal_OnClick(object sender, RoutedEventArgs e)
    {
        if (DataContext is not ExperimentalViewData viewData)
            return;

        viewData.Navigation.NavigateExternal(new ExperimentalDashboard(), viewData);
    }
}
