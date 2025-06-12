// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows.Controls;

namespace Wpf.Ui.Gallery.ViewModels.Pages.Text;

public partial class AutoSuggestBoxViewModel : ViewModel
{
    [ObservableProperty]
    private List<string> _autoSuggestBoxSuggestions = new()
    {
        "John",
        "Winston",
        "Adrianna",
        "Spencer",
        "Phoebe",
        "Lucas",
        "Carl",
        "Marissa",
        "Brandon",
        "Antoine",
        "Arielle",
        "Arielle",
        "Jamie",
        "Alexzander",
    };

    [ObservableProperty]
    private bool _showClearButton = true;

    [RelayCommand]
    private void OnShowClearButtonChecked(object sender)
    {
        if (sender is not CheckBox checkbox)
        {
            return;
        }

        ShowClearButton = !(checkbox.IsChecked ?? false);
    }
}
