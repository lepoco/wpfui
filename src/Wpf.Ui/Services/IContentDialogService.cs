// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Windows.Controls;
using Wpf.Ui.Contracts;
using Wpf.Ui.Controls;

namespace Wpf.Ui.Services;

/// <summary>
/// Represents a contract with the service that provides global <see cref="IContentDialogService"/>.
/// </summary>
public class ContentDialogService : IContentDialogService
{
    private ContentPresenter? _contentPresenter;

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
    public ContentDialog CreateDialog()
    {
        if (_contentPresenter is null)
            throw new ArgumentNullException($"The ContentPresenter didn't set previously.");

        return new ContentDialog(_contentPresenter);
    }
}
