// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows.Controls;

namespace Wpf.Ui.Gallery.ViewModels.Pages.BasicInput;

public partial class RatingViewModel : ObservableObject
{
    [ObservableProperty]
    private bool _isFirstRatingEnabled = true;

    [ObservableProperty]
    private double _firstRatingValue = 1.5D;

    [ObservableProperty]
    private bool _isSecondRatingEnabled = true;

    [ObservableProperty]
    private double _secondRatingValue = 3D;

    [RelayCommand]
    private void OnFirstRatingCheckboxChecked(object sender)
    {
        if (sender is not CheckBox checkbox)
            return;

        IsFirstRatingEnabled = !(checkbox?.IsChecked ?? false);
    }

    [RelayCommand]
    private void OnSecondRatingCheckboxChecked(object sender)
    {
        if (sender is not CheckBox checkbox)
            return;

        IsSecondRatingEnabled = !(checkbox?.IsChecked ?? false);
    }
}
