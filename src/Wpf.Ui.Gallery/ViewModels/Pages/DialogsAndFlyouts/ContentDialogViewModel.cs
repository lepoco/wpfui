// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Wpf.Ui.Controls;
using Wpf.Ui.Extensions;
using Wpf.Ui.Gallery.Controls;

namespace Wpf.Ui.Gallery.ViewModels.Pages.DialogsAndFlyouts;

public partial class ContentDialogViewModel(IContentDialogService contentDialogService) : ObservableObject
{
    [ObservableProperty]
    private string _dialogResultText = String.Empty;

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
            _ => "User cancelled the dialog"
        };
    }

    [RelayCommand]
    private async Task OnShowSignInContentDialog()
    {
        var termsOfUseContentDialog = new TermsOfUseContentDialog(contentDialogService.GetContentPresenter());

        _ = await termsOfUseContentDialog.ShowAsync();
    }
}
