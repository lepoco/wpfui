// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows.Controls;
using Wpf.Ui.Controls;

namespace Wpf.Ui.Extensions;

public static class ContentDialogServiceExtensions
{
    /// <summary>
    /// Shows the simple alert-like dialog.
    /// </summary>
    /// <returns>Result of the life cycle of the <see cref="ContentDialog"/>.</returns>
    public static Task<ContentDialogResult> ShowAlertAsync(
        this IContentDialogService dialogService,
        string title,
        string message,
        string closeButtonText,
        CancellationToken cancellationToken = default
    )
    {
        var dialog = new ContentDialog();

        dialog.SetCurrentValue(ContentDialog.TitleProperty, title);
        dialog.SetCurrentValue(ContentControl.ContentProperty, message);
        dialog.SetCurrentValue(ContentDialog.CloseButtonTextProperty, closeButtonText);

        return dialogService.ShowAsync(dialog, cancellationToken);
    }

    /// <summary>
    /// Shows simple dialog
    /// </summary>
    /// <param name="dialogService">The <see cref="IContentDialogService"/>.</param>
    /// <param name="options">Set of parameters of the basic dialog box.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Result of the life cycle of the <see cref="ContentDialog"/>.</returns>
    public static Task<ContentDialogResult> ShowSimpleDialogAsync(
        this IContentDialogService dialogService,
        SimpleContentDialogCreateOptions options,
        CancellationToken cancellationToken = default
    )
    {
        var dialog = new ContentDialog()
        {
            Title = options.Title,
            Content = options.Content,
            CloseButtonText = options.CloseButtonText,
            PrimaryButtonText = options.PrimaryButtonText,
            SecondaryButtonText = options.SecondaryButtonText,
        };

        return dialogService.ShowAsync(dialog, cancellationToken);
    }
}
