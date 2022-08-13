// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Wpf.Ui.Mvvm.Contracts;

namespace Wpf.Ui.Demo.ViewModels;

public class ButtonsViewModel : ObservableObject
{
    private readonly INavigationService _navigationService;

    private ICommand _showMoreCommand;

    public ICommand ShowMoreCommand => _showMoreCommand ??= new RelayCommand<string>(OnShowMore);

    public ButtonsViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;

        // Experimental
        var testGetThemeService = App.GetService<IThemeService>();
        var currentTheme = testGetThemeService.GetSystemTheme();
    }


    private void OnShowMore(string parameter)
    {
        _navigationService.Navigate(typeof(Views.Pages.Input));
    }
}
