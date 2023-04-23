// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

namespace Wpf.Ui.Gallery.ViewModels.Pages.Navigation;

public partial class MultilevelNavigationSample
{
    public MultilevelNavigationSample(INavigationService navigationService)
    {
        _navigationService = navigationService;
    }

    private readonly INavigationService _navigationService;

    [RelayCommand]
    private void NavigateForward(Type type)
    {
        _navigationService.NavigateWithHierarchy(type);
    }

    [RelayCommand]
    private void NavigateBack()
    {
        _navigationService.GoBack();
    }
}
