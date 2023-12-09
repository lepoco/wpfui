// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Wpf.Ui.Controls;

namespace Wpf.Ui.Gallery.ViewModels.Pages.StatusAndInfo;

public partial class InfoBadgeViewModel : ObservableObject
{
    [ObservableProperty]
    private InfoBadgeSeverity _infoBadgeSeverity = InfoBadgeSeverity.Attention;

    private int _infoBadgeSeverityComboBoxSelectedIndex = 0;

    public int InfoBadgeSeverityComboBoxSelectedIndex
    {
        get => _infoBadgeSeverityComboBoxSelectedIndex;
        set
        {
            SetProperty<int>(ref _infoBadgeSeverityComboBoxSelectedIndex, value);

            InfoBadgeSeverity = ConvertIndexToInfoBadgeSeverity(value);
        }
    }

    private InfoBadgeSeverity ConvertIndexToInfoBadgeSeverity(int value)
    {
        return value switch
        {
            1 => InfoBadgeSeverity.Informational,
            2 => InfoBadgeSeverity.Success,
            3 => InfoBadgeSeverity.Caution,
            4 => InfoBadgeSeverity.Critical,
            _ => InfoBadgeSeverity.Attention
        };
    }
}
