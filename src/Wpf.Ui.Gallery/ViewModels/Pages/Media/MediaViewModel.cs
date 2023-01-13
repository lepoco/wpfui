// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Collections.ObjectModel;
using Wpf.Ui.Gallery.Models;

namespace Wpf.Ui.Gallery.ViewModels.Pages.Media;

public partial class MediaViewModel : ObservableObject
{
    [ObservableProperty]
    private ICollection<NavigationCard> _navigationCards = new ObservableCollection<NavigationCard>
    {
        new()
        {
            Name = "Image",
            Icon = SymbolRegular.ImageMultiple24,
            Description = "Image presenter.",
            Link = "Image"
        },
        new()
        {
            Name = "Canvas",
            Icon = SymbolRegular.InkStroke24,
            Description = "Canvas presenter.",
            Link = "Canvas"
        },
        new()
        {
            Name = "WebView",
            Icon = SymbolRegular.GlobeDesktop24,
            Description = "Embedded browser window.",
            Link = "WebView"
        },
        new()
        {
            Name = "WebBrowser",
            Icon = SymbolRegular.GlobeProhibited20,
            Description = "(Obsolete) Embedded browser.",
            Link = "WebBrowser"
        }
    };
}
