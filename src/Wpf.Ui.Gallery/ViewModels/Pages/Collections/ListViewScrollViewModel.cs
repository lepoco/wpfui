// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

namespace Wpf.Ui.Gallery.ViewModels.Pages.Collections;

public sealed partial class ListViewScrollViewModel : ViewModel
{
    [ObservableProperty]
    private ObservableCollection<string> _bigArray = GenerateBigArray();

    private static ObservableCollection<string> GenerateBigArray()
    {
        var result = new ObservableCollection<string>();
        for (int i = 0; i < 500; i++)
        {
            result.Add($"Item {i + 1}");
        }

        return result;
    }
}
