// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Wpf.Ui.Gallery.ControlsLookup;
using Wpf.Ui.Gallery.Models;

namespace Wpf.Ui.Gallery.ViewModels.Pages;

public partial class AllControlsViewModel : ViewModel
{
    [ObservableProperty]
    private ICollection<NavigationCard> _navigationCards = new ObservableCollection<NavigationCard>(
        ControlPages
            .All()
            .Select(x => new NavigationCard()
            {
                Name = x.Name,
                Icon = x.Icon,
                Description = x.Description,
                PageType = x.PageType,
            })
            .OrderBy(x => x.Name)
    );
}
