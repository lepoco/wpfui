// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Wpf.Ui.Gallery.ViewModels.Pages.BasicInput;

public partial class ComboBoxViewModel : ObservableObject
{
    [ObservableProperty]
    private IList<string> _comboBoxFontFamilies;

    [ObservableProperty]
    private IList<int> _comboBoxFontSizes;

    public ComboBoxViewModel()
    {
        ComboBoxFontFamilies = new ObservableCollection<string>
        {
            "Arial",
            "Comic Sans MS",
            "Segoe UI",
            "Times New Roman"
        };

        ComboBoxFontSizes = new ObservableCollection<int>
         {
            8,
            9,
            10,
            11,
            12,
            14,
            16,
            18,
            20,
            24,
            28,
            36,
            48,
            72
         };
    }
}
