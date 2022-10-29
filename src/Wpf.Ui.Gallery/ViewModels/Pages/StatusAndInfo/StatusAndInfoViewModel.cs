// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Wpf.Ui.Common;
using Wpf.Ui.Gallery.Models;

namespace Wpf.Ui.Gallery.ViewModels.Pages.StatusAndInfo;

public partial class StatusAndInfoViewModel : ObservableObject
{
    [ObservableProperty]
    private ICollection<NavigationCard> _navigationCards = new ObservableCollection<NavigationCard>
    {
        new()
        {
            Name = "InfoBar",
            Icon = SymbolRegular.ErrorCircle24,
            Description = "Inline message card.",
            Link = "InfoBar"
        },
        new()
        {
            Name = "ProgressBar",
            Icon = SymbolRegular.ArrowDownload24,
            Description = "Shows the app progress on a task.",
            Link = "ProgressBar"
        },
        new()
        {
            Name = "ProgressRing",
            Icon = SymbolRegular.ArrowClockwise24,
            Description = "Shows the app progress on a task.",
            Link = "ProgressRing"
        },
        new()
        {
            Name = "ToolTip",
            Icon = SymbolRegular.Comment24,
            Description = "Information in popup window.",
            Link = "ToolTip"
        }
    };
}
