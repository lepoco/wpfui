// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Wpf.Ui.Gallery.Helpers;

namespace Wpf.Ui.Gallery.ViewModels.Pages;

public partial class DashboardViewModel(INavigationService navigationService) : ViewModel
{
    [RelayCommand]
    private void OnCardClick(string parameter)
    {
        if (string.IsNullOrWhiteSpace(parameter))
        {
            return;
        }

        Type? pageType = NameToPageTypeConverter.Convert(parameter);

        if (pageType == null)
        {
            return;
        }

        _ = navigationService.Navigate(pageType);
    }
}
