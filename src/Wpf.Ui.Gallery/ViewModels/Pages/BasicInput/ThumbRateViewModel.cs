// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Wpf.Ui.Controls;

namespace Wpf.Ui.Gallery.ViewModels.Pages.BasicInput;

public partial class ThumbRateViewModel : ObservableObject
{
    [ObservableProperty]
    private string _thumRateStateText = "Liked";

    [ObservableProperty]
    private string _thumRateStateCodeText = "<ui:ThumbRate State=\"Liked\" />";

    private ThumbRateState _thumbRateState = ThumbRateState.Liked;

    public ThumbRateState ThumbRateState
    {
        get => _thumbRateState;
        set
        {
            ThumRateStateText = value switch
            {
                ThumbRateState.Liked => "Liked",
                ThumbRateState.Disliked => "Disliked",
                _ => "None",
            };

            ThumRateStateCodeText = $"<ui:ThumbRate State=\"{ThumRateStateText}\" />";
            _ = SetProperty(ref _thumbRateState, value);
        }
    }
}
