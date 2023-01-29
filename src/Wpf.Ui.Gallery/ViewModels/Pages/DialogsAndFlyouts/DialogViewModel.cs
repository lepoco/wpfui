// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;
using System.Windows.Controls;

namespace Wpf.Ui.Gallery.ViewModels.Pages.DialogsAndFlyouts;

public partial class DialogViewModel : ObservableObject
{
    private readonly IDialogService _dialogService;
    public DialogViewModel(IDialogService dialogService)
    {
        _dialogService = dialogService;
    }

    [RelayCommand]
    private void OnShowDialog(object sender)
    {
        var rootDialog = _dialogService.GetDialogControl();

        rootDialog.DialogHeight = 240;
        rootDialog.Header = "This is global Dialog control managed by IDialogService";
        rootDialog.Content = new TextBlock
        {
            Margin = new Thickness(0, 8, 0, 0),
            TextWrapping = TextWrapping.WrapWithOverflow,
            Text = "This dialog is placed under the TitleBar but above the NavigationView. This allows us to enable the window to be navigated, but cover the application's navigation. You can add Dialog anywhere you want and arrange it as you like."
        };

        var buttonLeft = new Wpf.Ui.Controls.Button
        {
            Content = "Left button",
            Appearance = Ui.Controls.ControlAppearance.Primary,
            HorizontalAlignment = HorizontalAlignment.Stretch
        };
        buttonLeft.Click += (_, _) => rootDialog.Hide();
        rootDialog.ButtonLeft = buttonLeft;

        var buttonRight = new Wpf.Ui.Controls.Button
        {
            Content = "Right button",
            HorizontalAlignment = HorizontalAlignment.Stretch
        };
        buttonRight.Click += (_, _) => rootDialog.Hide();
        rootDialog.ButtonRight = buttonRight;

        rootDialog.Show();
    }
}
