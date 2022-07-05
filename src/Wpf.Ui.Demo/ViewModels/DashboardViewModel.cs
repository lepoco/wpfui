// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Demo.Services.Contracts;
using Wpf.Ui.Mvvm.Contracts;

namespace Wpf.Ui.Demo.ViewModels;

public class DashboardViewModel : Wpf.Ui.Mvvm.ViewModelBase, INavigationAware
{
    private readonly INavigationService _navigationService;

    private readonly ITestWindowService _testWindowService;

    public DashboardViewModel(INavigationService navigationService, ITestWindowService testWindowService)
    {
        _navigationService = navigationService;
        _testWindowService = testWindowService;
    }

    protected override void OnViewCommand(object parameter = null)
    {
        if (parameter is not string parameterString)
            return;

        if (parameterString.StartsWith("navigate_to"))
            OnNavigateCommand(parameterString);

        if (parameterString.StartsWith("open_window"))
            OnOpenWindowCommand(parameterString);
    }

    public void OnNavigatedTo()
    {
        System.Diagnostics.Debug.WriteLine($"INFO | {typeof(DashboardViewModel)} navigated", "Wpf.Ui.Demo");
    }

    public void OnNavigatedFrom()
    {
        System.Diagnostics.Debug.WriteLine($"INFO | {typeof(DashboardViewModel)} navigated", "Wpf.Ui.Demo");
    }

    private void OnNavigateCommand(string parameter)
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

    private void OnOpenWindowCommand(string parameter)
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

