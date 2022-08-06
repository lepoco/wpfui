// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Demo.Services.Contracts;
using Wpf.Ui.Mvvm.Contracts;

namespace Wpf.Ui.Demo.ViewModels;

public partial class DashboardViewModel : ObservableObject, INavigationAware
{
    private readonly INavigationService _navigationService;

    private readonly ITestWindowService _testWindowService;

    public DashboardViewModel(INavigationService navigationService, ITestWindowService testWindowService)
    {
        _navigationService = navigationService;
        _testWindowService = testWindowService;
    }

    public void OnNavigatedTo()
    {
        System.Diagnostics.Debug.WriteLine($"INFO | {typeof(DashboardViewModel)} navigated", "Wpf.Ui.Demo");
    }

    public void OnNavigatedFrom()
    {
        System.Diagnostics.Debug.WriteLine($"INFO | {typeof(DashboardViewModel)} navigated", "Wpf.Ui.Demo");
    }

    [RelayCommand]
    private void OnNavigate(string parameter)
    {
        switch (parameter)
        {
            case "navigate_to_input":
                _navigationService.Navigate(typeof(Views.Pages.Input));
                return;

            case "navigate_to_controls":
                _navigationService.Navigate(typeof(Views.Pages.Controls));
                return;

            case "navigate_to_colors":
                _navigationService.Navigate(typeof(Views.Pages.Colors));
                return;

            case "navigate_to_icons":
                _navigationService.Navigate(typeof(Views.Pages.Icons));
                return;
        }
    }

    [RelayCommand]
    private void OnOpenWindow(string parameter)
    {
        switch (parameter)
        {
            case "open_window_store":
                _testWindowService.Show<Views.Windows.StoreWindow>();
                return;

            case "open_window_manager":
                _testWindowService.Show<Views.Windows.TaskManagerWindow>();
                return;

            case "open_window_editor":
                _testWindowService.Show<Views.Windows.EditorWindow>();
                return;

            case "open_window_settings":
                _testWindowService.Show<Views.Windows.SettingsWindow>();
                return;

            case "open_window_experimental":
                _testWindowService.Show<Views.Windows.ExperimentalWindow>();
                return;
        }
    }
}

