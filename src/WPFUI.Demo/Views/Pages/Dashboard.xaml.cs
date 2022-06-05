// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Windows;
using WPFUI.Common.Interfaces;
using WPFUI.Demo.Services.Contracts;
using WPFUI.Mvvm.Contracts;

namespace WPFUI.Demo.Views.Pages;

/// <summary>
/// Interaction logic for Dashboard.xaml
/// </summary>
public partial class Dashboard : INavigationAware
{
    private readonly INavigationService _navigationService;

    private readonly ITestWindowService _testWindowService;

    public Dashboard(INavigationService navigationService, ITestWindowService testWindowService)
    {
        _navigationService = navigationService;
        _testWindowService = testWindowService;

        InitializeComponent();
    }

    public void OnNavigatedTo()
    {
        System.Diagnostics.Debug.WriteLine($"INFO | {typeof(Dashboard)} navigated", "WPFUI.Demo");
    }

    public void OnNavigatedFrom()
    {
        System.Diagnostics.Debug.WriteLine($"INFO | {typeof(Dashboard)} navigated out", "WPFUI.Demo");
    }

    private void ButtonControls_OnClick(object sender, RoutedEventArgs e)
    {
        _navigationService.Navigate(typeof(Views.Pages.Controls));
    }

    private bool TryOpenWindow(string name)
    {
        switch (name)
        {
            case "window_store":
                _testWindowService.Show<Views.Windows.StoreWindow>();
                return true;

            case "window_manager":
                _testWindowService.Show<Views.Windows.TaskManagerWindow>();
                return true;

            case "window_editor":
                _testWindowService.Show<Views.Windows.EditorWindow>();
                return true;

            case "window_settings":
                _testWindowService.Show<Views.Windows.SettingsWindow>();
                return true;
        }

        return false;
    }

    private void ButtonAction_OnClick(object sender, RoutedEventArgs e)
    {
        if (sender is not WPFUI.Controls.CardAction cardAction)
            return;

        var tag = cardAction.Tag as string;

        if (TryOpenWindow(tag))
            return;

        if (String.IsNullOrWhiteSpace(tag))
            return;

        switch (tag)
        {
            case "input":
                _navigationService.Navigate(typeof(Views.Pages.Input));
                return;

            case "controls":
                _navigationService.Navigate(typeof(Views.Pages.Controls));
                return;

            case "colors":
                _navigationService.Navigate(typeof(Views.Pages.Colors));
                return;

            case "icons":
                _navigationService.Navigate(typeof(Views.Pages.Icons));
                return;
        }
    }

    private void ButtonExperimental_OnClick(object sender, RoutedEventArgs e)
    {
        _testWindowService.Show(typeof(Views.Windows.ExperimentalWindow));
    }
}
