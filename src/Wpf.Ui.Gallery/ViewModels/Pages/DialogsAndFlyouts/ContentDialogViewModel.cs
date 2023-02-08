// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;
using Wpf.Ui.Controls;
using Wpf.Ui.Controls.ContentDialogControl;
using Wpf.Ui.Gallery.Controls;
using Wpf.Ui.Gallery.Views.Windows;

namespace Wpf.Ui.Gallery.ViewModels.Pages.DialogsAndFlyouts;

public partial class ContentDialogViewModel : ObservableObject
{
    public ContentDialogViewModel(IContentDialogService contentDialogService)
    {
        _contentDialogService = contentDialogService;
    }

    private readonly IContentDialogService _contentDialogService;

    [ObservableProperty]
    private string _dialogResultText = string.Empty;

    [RelayCommand]
    private async Task OnShowDialog(object content)
    {
        var dialog = _contentDialogService.CreateDialog();
        dialog.Title = "Save your work?";
        dialog.Content = content;
        dialog.PrimaryButtonText = "Save";
        dialog.SecondaryButtonText = "Don't Save";
        dialog.CloseButtonText = "Cancel";
        dialog.DefaultButton = ContentDialogButton.Primary;

        var result = await dialog.ShowAsync();

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
        var termsOfUseContentDialog = new TermsOfUseContentDialog(_contentDialogService.GetContentPresenter());
        await termsOfUseContentDialog.ShowAsync();
    }
}
