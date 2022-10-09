// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Wpf.Ui.Gallery.ViewModels.Pages.Icons;

public partial class SymbolIconViewModel : ObservableObject
{
    [ObservableProperty]
    private bool _isIconFilled = false;

    [RelayCommand]
    private void OnCheckboxChecked(object sender)
    {
        if (sender is not CheckBox checkbox)
            return;

        IsIconFilled = checkbox?.IsChecked ?? false;
    }
}
