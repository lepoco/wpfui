// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Wpf.Ui.Controls;

namespace Wpf.Ui.Gallery.ViewModels.Pages.StatusAndInfo;

public partial class InfoBarViewModel : ObservableObject
{
    [ObservableProperty]
    private bool _isShortInfoBarOpened = true;

    [ObservableProperty]
    private bool _isLongInfoBarOpened = true;

    [ObservableProperty]
    private InfoBarSeverity _shortInfoBarSeverity = InfoBarSeverity.Informational;

    [ObservableProperty]
    private InfoBarSeverity _longInfoBarSeverity = InfoBarSeverity.Informational;

    private int _shortInfoBarSeverityComboBoxSelectedIndex = 0;

    public int ShortInfoBarSeverityComboBoxSelectedIndex
    {
        get => _shortInfoBarSeverityComboBoxSelectedIndex;
        set
        {
            _ = SetProperty(ref _shortInfoBarSeverityComboBoxSelectedIndex, value);

            ShortInfoBarSeverity = ConvertIndexToInfoBarSeverity(value);
        }
    }

    private int _longInfoBarSeverityComboBoxSelectedIndex = 0;

    public int LongInfoBarSeverityComboBoxSelectedIndex
    {
        get => _longInfoBarSeverityComboBoxSelectedIndex;
        set
        {
            _ = SetProperty(ref _longInfoBarSeverityComboBoxSelectedIndex, value);

            LongInfoBarSeverity = ConvertIndexToInfoBarSeverity(value);
        }
    }

    private static InfoBarSeverity ConvertIndexToInfoBarSeverity(int value)
    {
        return value switch
        {
            1 => InfoBarSeverity.Success,
            2 => InfoBarSeverity.Warning,
            3 => InfoBarSeverity.Error,
            _ => InfoBarSeverity.Informational
        };
    }
}
