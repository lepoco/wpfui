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

namespace Wpf.Ui.Gallery.ViewModels.Pages.StatusAndInfo;

public partial class StatusAndInfoViewModel : ObservableObject
{
    private readonly INavigationService _navigationService;

    [ObservableProperty]
    private ICollection<NavigationCard> _navigationCards;

    public StatusAndInfoViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;

        NavigationCards = new ObservableCollection<NavigationCard>
        {
            new()
            {
                Name = "InfoBar",
                Icon = SymbolRegular.BookInformation24,
                Description = "InfoBar",
                Link = "InfoBar"
            },
            new()
            {
                Name = "ProgressBar",
                Icon = SymbolRegular.ArrowDownload24,
                Description = "ProgressBar",
                Link = "ProgressBar"
            },
            new()
            {
                Name = "ProgressRing",
                Icon = SymbolRegular.ArrowClockwise24,
                Description = "ProgressRing",
                Link = "ProgressRing"
            },
            new()
            {
                Name = "ToolTip",
                Icon = SymbolRegular.Chat24,
                Description = "ToolTip",
                Link = "ToolTip"
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
        System.Diagnostics.Debug.WriteLine($"INFO | {nameof(StatusAndInfoViewModel)} navigated, {parameter} ({pageType})", "Wpf.Ui.Gallery");
#endif
    }
}
