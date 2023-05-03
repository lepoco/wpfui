// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Windows.Controls;
using Wpf.Ui.Controls;
using Wpf.Ui.Controls.IconElements;
using Wpf.Ui.Controls.SnackbarControl;

namespace Wpf.Ui.Contracts;

/// <summary>
/// Represents a contract with the service that provides global <see cref="Snackbar"/>.
/// </summary>
public interface ISnackbarService
{
    /// <summary>
    /// Gets or sets a time for which the <see cref="Snackbar"/> should be visible. (By default 2 seconds)
    /// </summary>
    TimeSpan DefaultTimeOut { get; set; }

    /// <summary>
    /// Sets the <see cref="SnackbarPresenter"/>
    /// </summary>
    /// <param name="contentPresenter"></param>
    void SetSnackbarPresenter(SnackbarPresenter  contentPresenter);

    /// <summary>
    /// Provides direct access to the <see cref="ContentPresenter"/>
    /// </summary>
    /// <returns></returns>
    SnackbarPresenter GetSnackbarPresenter();

    /// <summary>
    /// Shows the snackbar. If it is already visible, firstly hides it for a moment, changes its content, and then shows it again.
    /// </summary>
    /// <param name="title"></param>
    /// <param name="message"></param>
    /// <param name="appearance"></param>
    /// <param name="icon"></param>
    /// <param name="timeout"></param>
    void Show(string title, string message, ControlAppearance appearance = ControlAppearance.Secondary, IconElement? icon = null, TimeSpan timeout = default);
}
