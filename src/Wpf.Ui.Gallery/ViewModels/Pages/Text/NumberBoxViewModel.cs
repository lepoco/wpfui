// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Wpf.Ui.Controls;

namespace Wpf.Ui.Gallery.ViewModels.Pages.Text;

public partial class NumberBoxViewModel : ViewModel
{
    private int _numberBoxSpinButtonPlacementModeSelectedIndex = 2;

    public int NumberBoxSpinButtonPlacementModeSelectedIndex
    {
        get => _numberBoxSpinButtonPlacementModeSelectedIndex;
        set
        {
            _ = SetProperty(ref _numberBoxSpinButtonPlacementModeSelectedIndex, value);

            UpdateSpinButtonPlacementMode(value);
        }
    }

    [ObservableProperty]
    private NumberBoxSpinButtonPlacementMode _spinButtonPlacementMode = NumberBoxSpinButtonPlacementMode.Inline;

    private void UpdateSpinButtonPlacementMode(int placementModeIndex)
    {
        SpinButtonPlacementMode = placementModeIndex switch
        {
            0 => NumberBoxSpinButtonPlacementMode.Hidden,
            1 => NumberBoxSpinButtonPlacementMode.Compact,
            _ => NumberBoxSpinButtonPlacementMode.Inline,
        };
    }
}