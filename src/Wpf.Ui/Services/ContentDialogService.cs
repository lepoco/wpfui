// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using Wpf.Ui.Contracts;
using Wpf.Ui.Controls.ContentDialogControl;

namespace Wpf.Ui.Services;

/// <summary>
/// Represents a contract with the service that provides global <see cref="IContentDialogService"/>.
/// </summary>
public class ContentDialogService : IContentDialogService
{
    private ContentPresenter? _contentPresenter;
    private ContentDialog? _dialog;

    /// <inheritdoc/>
    public void SetContentPresenter(ContentPresenter contentPresenter) => _contentPresenter = contentPresenter;

    /// <inheritdoc/>
    public ContentPresenter GetContentPresenter()
    {
        if (_contentPresenter is null)
            throw new ArgumentNullException($"The ContentPresenter didn't set previously.");

        return _contentPresenter;
    }

    /// <inheritdoc/>
    public Task<ContentDialogResult> ShowAlertAsync(string title, string message, string closeButtonText, CancellationToken cancellationToken = default)
    {
        if (_contentPresenter is null)
            throw new ArgumentNullException($"The ContentPresenter didn't set previously.");

        _dialog ??= new ContentDialog(_contentPresenter);

        _dialog.Title = title;
        _dialog.Content = message;
        _dialog.CloseButtonText = closeButtonText;

        return _dialog.ShowAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public Task<ContentDialogResult> ShowSimpleDialogAsync(SimpleContentDialogCreateOptions options, CancellationToken cancellationToken = default)
    {
        if (_contentPresenter is null)
            throw new ArgumentNullException($"The ContentPresenter didn't set previously.");

        var dialog = new ContentDialog(_contentPresenter)
        {
            Title = options.Title,
            Content = options.Content,
            CloseButtonText = options.CloseButtonText,
            PrimaryButtonText = options.PrimaryButtonText,
            SecondaryButtonText = options.SecondaryButtonText
        };

        return dialog.ShowAsync(cancellationToken);
    }
}
