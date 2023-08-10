// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Windows.Controls;
using Wpf.Ui.Controls;

namespace Wpf.Ui;

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
    /// <param name="contentPresenter"><see cref="ContentPresenter"/> inside of which the snackbar will be placed. The new <see cref="Snackbar"/> will replace the current <see cref="ContentPresenter.Content"/>.</param>
    void SetSnackbarPresenter(SnackbarPresenter contentPresenter);

    /// <summary>
    /// Provides direct access to the <see cref="ContentPresenter"/>
    /// </summary>
    /// <returns><see cref="Snackbar"/> currently in use.</returns>
    SnackbarPresenter GetSnackbarPresenter();

    /// <summary>
    /// Shows the snackbar. If it is already visible, firstly hides it for a moment, changes its content, and then shows it again.
    /// </summary>
    /// <param name="title">Name displayed on top of snackbar.</param>
    /// <param name="message">Message inside the snackbar.</param>
    void Show(
        string title,
        string message
    );

    /// <summary>
    /// Shows the snackbar. If it is already visible, firstly hides it for a moment, changes its content, and then shows it again.
    /// </summary>
    /// <param name="title">Name displayed on top of snackbar.</param>
    /// <param name="message">Message inside the snackbar.</param>
    /// <param name="appearance">Display style.</param>
    void Show(
        string title,
        string message,
        ControlAppearance appearance
    );

    /// <summary>
    /// Shows the snackbar. If it is already visible, firstly hides it for a moment, changes its content, and then shows it again.
    /// </summary>
    /// <param name="title">Name displayed on top of snackbar.</param>
    /// <param name="message">Message inside the snackbar.</param>
    /// <param name="icon">Additional icon on the left.</param>
    void Show(
        string title,
        string message,
        IconElement icon
    );

    /// <summary>
    /// Shows the snackbar. If it is already visible, firstly hides it for a moment, changes its content, and then shows it again.
    /// </summary>
    /// <param name="title">Name displayed on top of snackbar.</param>
    /// <param name="message">Message inside the snackbar.</param>
    /// <param name="timeout">The time after which the snackbar should disappear.</param>
    void Show(
        string title,
        string message,
        TimeSpan timeout
    );

    /// <summary>
    /// Shows the snackbar. If it is already visible, firstly hides it for a moment, changes its content, and then shows it again.
    /// </summary>
    /// <param name="title">Name displayed on top of snackbar.</param>
    /// <param name="message">Message inside the snackbar.</param>
    /// <param name="appearance">Display style.</param>
    /// <param name="timeout">The time after which the snackbar should disappear.</param>
    void Show(
        string title,
        string message,
        ControlAppearance appearance,
        TimeSpan timeout
    );

    /// <summary>
    /// Shows the snackbar. If it is already visible, firstly hides it for a moment, changes its content, and then shows it again.
    /// </summary>
    /// <param name="title">Name displayed on top of snackbar.</param>
    /// <param name="message">Message inside the snackbar.</param>
    /// <param name="icon">Additional icon on the left.</param>
    /// <param name="timeout">The time after which the snackbar should disappear.</param>
    void Show(
        string title,
        string message,
        IconElement icon,
        TimeSpan timeout
    );

    /// <summary>
    /// Shows the snackbar. If it is already visible, firstly hides it for a moment, changes its content, and then shows it again.
    /// </summary>
    /// <param name="title">Name displayed on top of snackbar.</param>
    /// <param name="message">Message inside the snackbar.</param>
    /// <param name="appearance">Display style.</param>
    /// <param name="icon">Additional icon on the left.</param>
    /// <param name="timeout">The time after which the snackbar should disappear.</param>
    void Show(
        string title,
        string message,
        ControlAppearance appearance,
        IconElement? icon,
        TimeSpan timeout
    );
}
