// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Wpf.Ui.Common;
using Wpf.Ui.Gallery.Models;

namespace Wpf.Ui.Gallery.ViewModels.Pages.Collections;

public partial class CollectionsViewModel : ObservableObject
{
    [ObservableProperty]
    private ICollection<NavigationCard> _navigationCards = new ObservableCollection<NavigationCard>
    {
        new()
        {
            Name = "DataGrid",
            Icon = SymbolRegular.GridKanban20,
            Description = "Complex data presenter.",
            Link = "DataGrid"
        },
        new()
        {
            Name = "ListBox",
            Icon = SymbolRegular.AppsListDetail24,
            Description = "Selectable list.",
            Link = "ListBox"
        },
        new()
        {
            Name = "ListView",
            Icon = SymbolRegular.GroupList24,
            Description = "Selectable list.",
            Link = "ListView"
        },
        new()
        {
            Name = "TreeView",
            Icon = SymbolRegular.TextBulletListTree24,
            Description = "Collapsable list.",
            Link = "TreeView"
        },
        new()
        {
            Name = "TreeList",
            Icon = SymbolRegular.TextBulletListTree24,
            Description = "List inside the TreeView.",
            Link = "TreeList"
        },
    };
}
