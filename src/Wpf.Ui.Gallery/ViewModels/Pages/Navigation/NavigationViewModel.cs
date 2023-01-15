// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Collections.ObjectModel;
using Wpf.Ui.Gallery.Models;

namespace Wpf.Ui.Gallery.ViewModels.Pages.Navigation;

public partial class NavigationViewModel : ObservableObject
{
    [ObservableProperty]
    private ICollection<NavigationCard> _navigationCards = new ObservableCollection<NavigationCard>
    {
        new()
        {
            Name = "BreadcrumbBar",
            Icon = SymbolRegular.Navigation24,
            Description = "Shows the trail of navigation taken to the current location.",
            Link = "BreadcrumbBar"
        },
        new()
        {
            Name = "NavigationView",
            Icon = SymbolRegular.Navigation24,
            Description = "Main navigation for the app.",
            Link = "NavigationView"
        },
        new()
        {
            Name = "TabControl",
            Icon = SymbolRegular.NavigationUnread24,
            Description = "Tab control like in browser.",
            Link = "TabControl"
        }
    };
}
