// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows.Controls;

namespace Wpf.Ui.Gallery.ViewModels.Pages.BasicInput;

public partial class CheckBoxViewModel : ObservableObject
{
    [ObservableProperty]
    private bool? _selectAllCheckBoxChecked = null;

    [ObservableProperty]
    private bool _optionOneCheckBoxChecked = false;

    [ObservableProperty]
    private bool _optionTwoCheckBoxChecked = true;

    [ObservableProperty]
    private bool _optionThreeCheckBoxChecked = false;

    [RelayCommand]
    private void OnSelectAllChecked(object sender)
    {
        if (sender is not CheckBox checkBox)
            return;

        if (checkBox.IsChecked == null)
            checkBox.IsChecked = !(OptionOneCheckBoxChecked && OptionTwoCheckBoxChecked && OptionThreeCheckBoxChecked);

        if (checkBox.IsChecked == true)
        {
            OptionOneCheckBoxChecked = true;
            OptionTwoCheckBoxChecked = true;
            OptionThreeCheckBoxChecked = true;
        }
        else if (checkBox.IsChecked == false)
        {
            OptionOneCheckBoxChecked = false;
            OptionTwoCheckBoxChecked = false;
            OptionThreeCheckBoxChecked = false;
        }
    }

    [RelayCommand]
    private void OnSingleChecked(string option)
    {
        if (OptionOneCheckBoxChecked && OptionTwoCheckBoxChecked && OptionThreeCheckBoxChecked)
            SelectAllCheckBoxChecked = true;
        else if (!OptionOneCheckBoxChecked && !OptionTwoCheckBoxChecked && !OptionThreeCheckBoxChecked)
            SelectAllCheckBoxChecked = false;
        else
            SelectAllCheckBoxChecked = null;
    }
}
