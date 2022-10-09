// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Wpf.Ui.Common;
using Wpf.Ui.Gallery.Helpers;
using Wpf.Ui.Gallery.Models;
using Wpf.Ui.Mvvm.Contracts;

namespace Wpf.Ui.Gallery.ViewModels.Pages.Media;

public partial class MediaViewModel : ObservableObject
{
    private readonly INavigationService _navigationService;

    [ObservableProperty]
    private ICollection<NavigationCard> _navigationCards;

    public MediaViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;

        NavigationCards = new ObservableCollection<NavigationCard>
        {
            new()
            {
                Name = "WebView",
                Icon = SymbolRegular.GlobeDesktop24,
                Description = "Embedded browser window.",
                Link = "WebView"
            }
        };
    }

    [RelayCommand]
    private void OnNavigatedTo(string parameter)
    {
        var pageType = NameToPageTypeConverter.Convert(parameter);

        if (pageType != null)
            _navigationService.Navigate(pageType);

#if DEBUG
        System.Diagnostics.Debug.WriteLine($"INFO | {nameof(MediaViewModel)} navigated, {parameter} ({pageType})", "Wpf.Ui.Gallery");
#endif
    }
}
