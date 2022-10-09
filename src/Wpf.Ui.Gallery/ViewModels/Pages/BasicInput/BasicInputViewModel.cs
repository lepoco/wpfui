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

namespace Wpf.Ui.Gallery.ViewModels.Pages.BasicInput;

public partial class BasicInputViewModel : ObservableObject
{
    private readonly INavigationService _navigationService;

    [ObservableProperty]
    private ICollection<NavigationCard> _navigationCards;

    public BasicInputViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;

        NavigationCards = new ObservableCollection<NavigationCard>
        {
            new()
            {
                Name = "Anchor",
                Icon = SymbolRegular.Link24,
                Description = "Anchor",
                Link = "Anchor"
            },
            new()
            {
                Name = "Button",
                Icon = SymbolRegular.Link24,
                Description = "Button",
                Link = "Button"
            },
            new()
            {
                Name = "Hyperlink",
                Icon = SymbolRegular.Link24,
                Description = "Hyperlink",
                Link = "Hyperlink"
            },
            new()
            {
                Name = "ToggleButton",
                Icon = SymbolRegular.Link24,
                Description = "ToggleButton",
                Link = "ToggleButton"
            },
            new()
            {
                Name = "ToggleSwitch",
                Icon = SymbolRegular.Link24,
                Description = "ToggleSwitch",
                Link = "ToggleSwitch"
            },
            new()
            {
                Name = "CheckBox",
                Icon = SymbolRegular.Link24,
                Description = "CheckBox",
                Link = "CheckBox"
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
        System.Diagnostics.Debug.WriteLine($"INFO | {nameof(BasicInputViewModel)} navigated, {parameter} ({pageType})", "Wpf.Ui.Gallery");
#endif
    }
}
