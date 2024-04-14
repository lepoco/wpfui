// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Wpf.Ui.Controls;

namespace Wpf.Ui;

/// <summary>
/// A service that provides methods related to displaying the <see cref="Snackbar"/>.
/// </summary>
public class SnackbarService : ISnackbarService
{
    private SnackbarPresenter? _presenter;

    private Snackbar? _snackbar;

    /// <inheritdoc />
    public TimeSpan DefaultTimeOut { get; set; } = TimeSpan.FromSeconds(5);

    /// <inheritdoc />
    public void SetSnackbarPresenter(SnackbarPresenter contentPresenter)
    {
        _presenter = contentPresenter;
    }

    /// <inheritdoc />
    public SnackbarPresenter? GetSnackbarPresenter()
    {
        return _presenter;
    }

    /// <inheritdoc />
    public void Show(
        string title,
        string message,
        ControlAppearance appearance,
        IconElement? icon,
        TimeSpan timeout
    )
    {
        if (_presenter is null)
        {
            throw new InvalidOperationException($"The SnackbarPresenter was never set");
        }

        _snackbar ??= new Snackbar(_presenter);

        _snackbar.SetCurrentValue(Snackbar.TitleProperty, title);
        _snackbar.SetCurrentValue(System.Windows.Controls.ContentControl.ContentProperty, message);
        _snackbar.SetCurrentValue(Snackbar.AppearanceProperty, appearance);
        _snackbar.SetCurrentValue(Snackbar.IconProperty, icon);
        _snackbar.SetCurrentValue(
            Snackbar.TimeoutProperty,
            timeout.TotalSeconds == 0 ? DefaultTimeOut : timeout
        );

        _snackbar.Show(true);
    }
}
