// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Wpf.Ui.Common;
using Wpf.Ui.Gallery.Models;

namespace Wpf.Ui.Gallery.ViewModels.Pages.DateAndTime;

public partial class DateAndTimeViewModel : ObservableObject
{
    [ObservableProperty]
    private ICollection<NavigationCard> _navigationCards = new ObservableCollection<NavigationCard>
        {
            new()
            {
                Name = "Calendar",
                Icon = SymbolRegular.CalendarLtr24,
                Description = "Presents a calendar to the user.",
                Link = "Calendar"
            },
            new()
            {
                Name = "DatePicker",
                Icon = SymbolRegular.CalendarSearch20,
                Description = "Control that lets pick a date.",
                Link = "DatePicker"
            }
        };
}
