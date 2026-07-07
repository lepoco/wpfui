// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

namespace Wpf.Ui.Gallery.ViewModels.Pages.Navigation;

public partial class SelectorBarViewModel : ViewModel
{
    private static readonly string[] Contents =
    [
        "Files you've viewed or modified recently.",
        "Files and folders others have shared with you.",
        "Files and folders you've marked as favorites.",
    ];

    [ObservableProperty]
    private int _selectedIndex;

    [ObservableProperty]
    private string? _selectedContent = Contents[0];

    partial void OnSelectedIndexChanged(int value)
    {
        SelectedContent = value >= 0 && value < Contents.Length
            ? Contents[value]
            : null;
    }
}
