// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

namespace Wpf.Ui.Gallery.ViewModels.Pages.Navigation;

public partial class SelectorBarViewModel : ViewModel
{
    private readonly string[] _contents =
    [
        "Files you've viewed or modified recently.",
        "Files and folders others have shared with you.",
        "Files and folders you've marked as favorites.",
    ];

    [ObservableProperty]
    private int _selectedIndex;

    [ObservableProperty]
    private string? _selectedContent = "Files you've viewed or modified recently.";

    /// <summary>
    /// Updates the displayed content whenever the selection changes.
    /// The initial value is set by the field initializer above, so this
    /// only runs for subsequent changes driven by the two-way binding.
    /// </summary>
    partial void OnSelectedIndexChanged(int value)
    {
        SelectedContent = value >= 0 && value < _contents.Length
            ? _contents[value]
            : null;
    }
}
