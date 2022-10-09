// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Wpf.Ui.Gallery.ViewModels.Pages.BasicInput;

public partial class ToggleSwitchViewModel : ObservableObject
{
    [ObservableProperty]
    private bool _isToggleSwitchEnabled = true;


    [RelayCommand]
    private void OnToggleSwitchCheckboxChecked(object sender)
    {
        if (sender is not CheckBox checkbox)
            return;

        IsToggleSwitchEnabled = !(checkbox?.IsChecked ?? false);
    }
}
