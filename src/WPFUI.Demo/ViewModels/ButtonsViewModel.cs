// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using WPFUI.Mvvm.Contracts;

namespace WPFUI.Demo.ViewModels;

public class ButtonsViewModel : WPFUI.Mvvm.ViewModelBase
{
    private readonly INavigationService _navigationService;
    public ButtonsViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;
    }

    protected override void OnViewCommand(object parameter = null)
    {
        if (parameter is "show_more")
            _navigationService.Navigate(typeof(Views.Pages.Input));
    }
}
