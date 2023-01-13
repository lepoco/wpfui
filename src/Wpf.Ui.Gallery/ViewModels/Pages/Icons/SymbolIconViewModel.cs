// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows.Controls;

namespace Wpf.Ui.Gallery.ViewModels.Pages.Icons;

public partial class SymbolIconViewModel : ObservableObject
{
    [ObservableProperty]
    private bool _isIconFilled = false;

    [ObservableProperty]
    private string _codeText = "<SymbolIcon Symbol=\"Heart24\" />";

    [RelayCommand]
    private void OnCheckboxChecked(object sender)
    {
        if (sender is not CheckBox checkbox)
            return;

        var isFilled = checkbox?.IsChecked ?? false;

        IsIconFilled = isFilled;
        CodeText = $"<SymbolIcon Symbol=\"Heart24\"{(isFilled ? " Filled=\"True\"" : "")} />";
    }
}
