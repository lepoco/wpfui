// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using CommunityToolkit.Mvvm.ComponentModel;
using Wpf.Ui.Controls;

namespace Wpf.Ui.Gallery.ViewModels.Pages.StatusAndInfo;

public partial class InfoBarViewModel : ObservableObject
{
    [ObservableProperty]
    private bool _isInfoBarOpened = true;

    private int _infoBarSeverityComboBoxSelectedIndex = 0;

    [ObservableProperty]
    private InfoBarSeverity _infoBarSeverity = InfoBarSeverity.Informational;

    public int InfoBarSeverityComboBoxSelectedIndex
    {
        get => _infoBarSeverityComboBoxSelectedIndex;
        set
        {
            SetProperty<int>(ref _infoBarSeverityComboBoxSelectedIndex, value);
            UpdateSelectedInfoBarSeverity(value);
        }
    }

    private void UpdateSelectedInfoBarSeverity(int severityIndex)
    {
        InfoBarSeverity = severityIndex switch
        {
            1 => InfoBarSeverity.Success,
            2 => InfoBarSeverity.Warning,
            3 => InfoBarSeverity.Error,
            _ => InfoBarSeverity.Informational
        };
    }
}
