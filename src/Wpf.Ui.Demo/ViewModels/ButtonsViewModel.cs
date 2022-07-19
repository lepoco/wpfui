// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Wpf.Ui.Mvvm.Contracts;

namespace Wpf.Ui.Demo.ViewModels;

public partial class ButtonsViewModel : ObservableObject
{
    private readonly INavigationService _navigationService;

    public ButtonsViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;

        // Experimental
        var testGetThemeService = App.GetService<IThemeService>();
        var currentTheme = testGetThemeService.GetSystemTheme();
    }

    [ICommand]
    private void OnShowMore(string parameter)
    {
        _navigationService.Navigate(typeof(Views.Pages.Input));
    }
}
