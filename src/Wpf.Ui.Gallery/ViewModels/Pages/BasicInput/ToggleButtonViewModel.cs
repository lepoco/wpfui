// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows.Controls;

namespace Wpf.Ui.Gallery.ViewModels.Pages.BasicInput;

public partial class ToggleButtonViewModel : ViewModel
{
    [ObservableProperty]
    private bool _isToggleButtonEnabled = true;

    [RelayCommand]
    private void OnToggleButtonCheckboxChecked(object sender)
    {
        if (sender is not CheckBox checkbox)
        {
            return;
        }

        IsToggleButtonEnabled = !(checkbox?.IsChecked ?? false);
    }
}
