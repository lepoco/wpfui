// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Diagnostics.CodeAnalysis;

namespace Wpf.Ui.Gallery.ViewModels.Pages.DialogsAndFlyouts;

public partial class MessageBoxViewModel : ViewModel
{
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "relay command")]
    [RelayCommand]
    private void OnOpenStandardMessageBox(object sender)
    {
        _ = MessageBox.Show("Something about to happen", "I can feel it");
    }

    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "relay command")]
    [RelayCommand]
    private async Task OnOpenCustomMessageBox(object sender)
    {
        var uiMessageBox = new Wpf.Ui.Controls.MessageBox
        {
            Title = "WPF UI Message Box",
            Content =
                "Never gonna give you up, never gonna let you down Never gonna run around and desert you Never gonna make you cry, never gonna say goodbye",
        };

        _ = await uiMessageBox.ShowDialogAsync();
    }

    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "relay command")]
    [RelayCommand]
    private async Task OnOpenThreeButtonMessageBox(object sender)
    {
        var uiMessageBox = new Wpf.Ui.Controls.MessageBox
        {
            Title = "Confirmation",
            Content = "Do you want to save your changes before closing?",
            PrimaryButtonText = "Save",
            SecondaryButtonText = "Don't Save",
            CloseButtonText = "Cancel",
            DefaultFocusedButton = Wpf.Ui.Controls.MessageBoxButton.Secondary,
        };

        var result = await uiMessageBox.ShowDialogAsync();
        _ = MessageBox.Show($"You selected: {result}", "Result");
    }

    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "relay command")]
    [RelayCommand]
    private async Task OnOpenMessageBoxWithFocusableContent(object sender)
    {
        var textBox = new System.Windows.Controls.TextBox
        {
            Text = "Type something here...",
            Margin = new System.Windows.Thickness(0, 8, 0, 0),
        };

        var uiMessageBox = new Wpf.Ui.Controls.MessageBox
        {
            Title = "Input Required",
            Content = new System.Windows.Controls.StackPanel
            {
                Children =
                {
                    new System.Windows.Controls.TextBlock
                    {
                        Text = "Please enter your name:",
                        TextWrapping = System.Windows.TextWrapping.Wrap,
                    },
                    textBox,
                },
            },
            PrimaryButtonText = "OK",
            SecondaryButtonText = "Cancel",
            DefaultFocusedButton = Wpf.Ui.Controls.MessageBoxButton.Primary,
        };

        var result = await uiMessageBox.ShowDialogAsync();
        if (result == Wpf.Ui.Controls.MessageBoxResult.Primary)
        {
            _ = MessageBox.Show($"Hello, {textBox.Text}!", "Greeting");
        }
    }

    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "relay command")]
    [RelayCommand]
    private async Task OnOpenMessageBoxWithIcons(object sender)
    {
        var uiMessageBox = new Wpf.Ui.Controls.MessageBox
        {
            Title = "Warning",
            Content = "This action cannot be undone. Are you sure you want to continue?",
            PrimaryButtonText = "Yes, Continue",
            SecondaryButtonText = "No, Cancel",
            PrimaryButtonIcon = new Wpf.Ui.Controls.SymbolIcon
            {
                Symbol = Wpf.Ui.Controls.SymbolRegular.Warning24,
            },
            DefaultFocusedButton = Wpf.Ui.Controls.MessageBoxButton.Secondary,
        };

        _ = await uiMessageBox.ShowDialogAsync();
    }

    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "relay command")]
    [RelayCommand]
    private async Task OnOpenMessageBoxWithAutoFocus(object sender)
    {
        // DefaultFocusedButton is not set, so focus will automatically be set to primaryButton
        // when no button has focus and no non-button control has focus
        var uiMessageBox = new Wpf.Ui.Controls.MessageBox
        {
            Title = "Auto Focus",
            Content = "This MessageBox will automatically set focus to the primary button when no button or non-button control has focus.",
            PrimaryButtonText = "OK",
            SecondaryButtonText = "Cancel",
            // DefaultFocusedButton is intentionally not set
        };

        var result = await uiMessageBox.ShowDialogAsync();
        _ = MessageBox.Show($"You selected: {result}", "Result");
    }
}
