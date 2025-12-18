// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Wpf.Ui.Controls;
using Wpf.Ui.Extensions;
using Wpf.Ui.Gallery.Controls;

namespace Wpf.Ui.Gallery.ViewModels.Pages.DialogsAndFlyouts;

public partial class ContentDialogViewModel(IContentDialogService contentDialogService) : ViewModel
{
    [ObservableProperty]
    private string _dialogResultText = string.Empty;

    [RelayCommand]
    private async Task OnShowDialog(object content)
    {
        ContentDialogResult result = await contentDialogService.ShowSimpleDialogAsync(
            new SimpleContentDialogCreateOptions()
            {
                Title = "Save your work?",
                Content = content,
                PrimaryButtonText = "Save",
                SecondaryButtonText = "Don't Save",
                CloseButtonText = "Cancel",
            }
        );

        DialogResultText = result switch
        {
            ContentDialogResult.Primary => "User saved their work",
            ContentDialogResult.Secondary => "User did not save their work",
            _ => "User cancelled the dialog",
        };
    }

    [RelayCommand]
    private async Task OnShowSignInContentDialog()
    {
        var termsOfUseContentDialog = new TermsOfUseContentDialog(contentDialogService.GetDialogHost());

        _ = await termsOfUseContentDialog.ShowAsync();
    }

    [RelayCommand]
    private async Task OnShowThreeButtonContentDialog()
    {
        var dialog = new ContentDialog(contentDialogService.GetDialogHost())
        {
            Title = "Confirmation",
            Content = "Do you want to save your changes before closing?",
            PrimaryButtonText = "Save",
            SecondaryButtonText = "Don't Save",
            CloseButtonText = "Cancel",
            DefaultButton = ContentDialogButton.Secondary,
        };

        var result = await dialog.ShowAsync();
        DialogResultText = result switch
        {
            ContentDialogResult.Primary => "User saved their work",
            ContentDialogResult.Secondary => "User did not save their work",
            _ => "User cancelled the dialog",
        };
    }

    [RelayCommand]
    private async Task OnShowContentDialogWithFocusableContent()
    {
        var textBox = new System.Windows.Controls.TextBox
        {
            Text = "Type something here...",
            Margin = new System.Windows.Thickness(0, 8, 0, 0),
        };

        var dialog = new ContentDialog(contentDialogService.GetDialogHost())
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
            DefaultButton = ContentDialogButton.Primary,
        };

        var result = await dialog.ShowAsync();
        if (result == ContentDialogResult.Primary)
        {
            DialogResultText = $"Hello, {textBox.Text}!";
        }
        else
        {
            DialogResultText = "User cancelled the dialog";
        }
    }

    [RelayCommand]
    private async Task OnShowContentDialogWithIcons()
    {
        var dialog = new ContentDialog(contentDialogService.GetDialogHost())
        {
            Title = "Warning",
            Content = "This action cannot be undone. Are you sure you want to continue?",
            PrimaryButtonText = "Yes, Continue",
            SecondaryButtonText = "No, Cancel",
            PrimaryButtonIcon = new SymbolIcon
            {
                Symbol = SymbolRegular.Warning24,
            },
            DefaultButton = ContentDialogButton.Secondary,
        };

        var result = await dialog.ShowAsync();
        DialogResultText = result switch
        {
            ContentDialogResult.Primary => "User chose to continue",
            ContentDialogResult.Secondary => "User chose to cancel",
            _ => "User closed the dialog",
        };
    }

    [RelayCommand]
    private async Task OnShowContentDialogWithAutoFocus()
    {
        // DefaultButton is set to Primary (default), so focus will automatically be set to primaryButton
        // when no button has focus and no non-button control has focus
        var dialog = new ContentDialog(contentDialogService.GetDialogHost())
        {
            Title = "Auto Focus",
            Content = "This ContentDialog will automatically set focus to the primary button when no button or non-button control has focus.",
            PrimaryButtonText = "OK",
            SecondaryButtonText = "Cancel",
            // DefaultButton is intentionally set to Primary (default value)
        };

        var result = await dialog.ShowAsync();
        DialogResultText = result switch
        {
            ContentDialogResult.Primary => "User clicked OK",
            ContentDialogResult.Secondary => "User clicked Cancel",
            _ => "User closed the dialog",
        };
    }
}
