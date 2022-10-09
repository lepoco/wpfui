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

namespace Wpf.Ui.Gallery.ViewModels.Pages.DialogsAndFlyouts;

public partial class DialogsAndFlyoutsViewModel : ObservableObject
{
    private readonly INavigationService _navigationService;

    [ObservableProperty]
    private ICollection<NavigationCard> _navigationCards;

    public DialogsAndFlyoutsViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;

        NavigationCards = new ObservableCollection<NavigationCard>
        {
            new()
            {
                Name = "Snackbar",
                Icon = SymbolRegular.PlayingCards20,
                Description = "Information card at the bottom.",
                Link = "Snackbar"
            },
            new()
            {
                Name = "Dialog",
                Icon = SymbolRegular.CalendarMultiple24,
                Description = "Card covering the app content",
                Link = "Dialog"
            },
            new()
            {
                Name = "Flyout",
                Icon = SymbolRegular.AppTitle24,
                Description = "Contextual popup.",
                Link = "Flyout"
            },
            new()
            {
                Name = "MessageBox",
                Icon = SymbolRegular.CalendarInfo20,
                Description = "MessageBox",
                Link = "MessageBox"
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
        System.Diagnostics.Debug.WriteLine($"INFO | {nameof(DialogsAndFlyoutsViewModel)} navigated, {parameter} ({pageType})", "Wpf.Ui.Gallery");
#endif
    }
}
