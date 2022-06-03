// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using WPFUI.Mvvm.Contracts;

namespace WPFUI.Demo.ViewModels;

public class ButtonsViewModel : WPFUI.Mvvm.ViewModelBase
{
    private readonly INavigationWindow _navigationWindow;
    public ButtonsViewModel(INavigationWindow navigationWindow)
    {
        _navigationWindow = navigationWindow;
    }

    protected override void OnViewCommand(object parameter = null)
    {
        if ((string)parameter == "show_more")
            _navigationWindow.Navigate(typeof(Views.Pages.Input));
    }
}
