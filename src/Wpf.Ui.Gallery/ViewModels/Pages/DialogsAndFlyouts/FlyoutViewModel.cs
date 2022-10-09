// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Wpf.Ui.Gallery.ViewModels.Pages.DialogsAndFlyouts;

public partial class FlyoutViewModel : ObservableObject
{
    [ObservableProperty]
    private bool _isFlyoutOpen = false;

    [RelayCommand]
    private void OnButtonClick(object sender)
    {
        if (!IsFlyoutOpen)
            IsFlyoutOpen = true;
    }
}
