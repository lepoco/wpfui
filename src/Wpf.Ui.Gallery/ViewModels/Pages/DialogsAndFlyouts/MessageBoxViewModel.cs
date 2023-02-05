// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Wpf.Ui.Gallery.ViewModels.Pages.DialogsAndFlyouts;

public partial class MessageBoxViewModel : ObservableObject
{
    [RelayCommand]
    private void OnOpenStandardMessageBox(object sender)
    {
        System.Windows.MessageBox.Show("Something about to happen", "I can feel it");
    }

    [RelayCommand]
    private async void OnOpenCustomMessageBox(object sender)
    {
        var uiMessageBox = new Wpf.Ui.Controls.MessageBoxControl.MessageBox
        {
            Title = "WPF UI Message Box",
            Content = "Never gonna give you up, never gonna let you down Never gonna run around and desert you Never gonna make you cry, never gonna say goodbye",
        };

        var result = await uiMessageBox.ShowDialogAsync();
    }
}
