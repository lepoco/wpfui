// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using Wpf.Ui.Contracts;
using Wpf.Ui.Controls;
using Wpf.Ui.Controls.IconElements;
using Wpf.Ui.Controls.SnackbarControl;

namespace Wpf.Ui.Services;

/// <summary>
/// A service that provides methods related to displaying the <see cref="Snackbar"/>.
/// </summary>
public class SnackbarService : ISnackbarService
{
    private SnackbarPresenter? _presenter;
    private Snackbar? _snackbar;

    /// <inheritdoc />
    public TimeSpan DefaultTimeOut { get; set; } = TimeSpan.FromSeconds(2);

    /// <inheritdoc />
    public void SetSnackbarPresenter(SnackbarPresenter contentPresenter)
    {
        _presenter = contentPresenter;
    }

    /// <inheritdoc />
    public SnackbarPresenter GetSnackbarPresenter()
    {
        if (_presenter is null)
            throw new ArgumentNullException($"The SnackbarPresenter didn't set previously.");

        return _presenter;
    }

    /// <inheritdoc />
    public void Show(string title, string message, ControlAppearance appearance = ControlAppearance.Secondary, IconElement? icon = null, TimeSpan timeout = default)
    {
        if (_presenter is null)
            throw new ArgumentNullException($"The SnackbarPresenter didn't set previously.");

        _snackbar ??= new Snackbar(_presenter);

        _snackbar.Title = title;
        _snackbar.Content = message;
        _snackbar.Appearance = appearance;
        _snackbar.Icon = icon;
        _snackbar.Timeout = timeout.TotalSeconds == 0 ? DefaultTimeOut : timeout;

        _snackbar.Show(true);
    }
}
