// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Collections.ObjectModel;
using Wpf.Ui.Gallery.Models;

namespace Wpf.Ui.Gallery.ViewModels.Pages.DialogsAndFlyouts;

public partial class DialogsAndFlyoutsViewModel : ObservableObject
{
    [ObservableProperty]
    private ICollection<NavigationCard> _navigationCards = new ObservableCollection<NavigationCard>
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
            Name = "ContentDialog",
            Icon = SymbolRegular.CalendarMultiple24,
            Description = "Card covering the app content",
            Link = "ContentDialog"
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
