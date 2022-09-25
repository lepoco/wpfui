// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Threading.Tasks;
using Wpf.Ui.Common;

namespace Wpf.Ui.Controls.Interfaces;

/// <summary>
/// Snackbar inform user of a process that an app has performed or will perform. It appears temporarily, towards the bottom of the window.
/// </summary>
public interface ISnackbarControl
{
    /// <summary>
    /// Gets the information whether the <see cref="ISnackbarControl"/> is visible.
    /// </summary>
    bool IsShown { get; }

    /// <summary>
    /// Gets or sets a time for which the <see cref="ISnackbarControl"/> should be visible.
    /// </summary>
    int Timeout { get; set; }

    /// <summary>
    /// Gets or sets the text displayed on the top of the <see cref="ISnackbarControl"/>.
    /// </summary>
    string Title { get; set; }

    /// <summary>
    /// Gets or sets the text displayed on the bottom of the <see cref="ISnackbarControl"/>.
    /// </summary>
    string Message { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the <see cref="ISnackbarControl"/> close button should be visible.
    /// </summary>
    bool CloseButtonEnabled { get; set; }

    /// <summary>
    /// Occurs when the snackbar is about to open.
    /// </summary>
    event RoutedSnackbarEvent Opened;

    /// <summary>
    /// Occurs when the snackbar is about to close.
    /// </summary>
    event RoutedSnackbarEvent Closed;

    /// <summary>
    /// Shows the snackbar. If it is already visible, firstly hides it for a moment, changes its content, and then shows it again.
    /// </summary>
    /// <returns><see langword="true"/> if invocation of <see langword="async"/> method succeeded, Exception otherwise.</returns>
    bool Show();

    /// <summary>
    /// Shows the snackbar. If it is already visible, firstly hides it for a moment, changes its content, and then shows it again.
    /// </summary>
    /// <param name="title"><see cref="Title"/> at the top of the snackbar.</param>
    /// <returns><see langword="true"/> if invocation of <see langword="async"/> method succeeded, Exception otherwise.</returns>
    bool Show(string title);

    /// <summary>
    /// Shows the snackbar. If it is already visible, firstly hides it for a moment, changes its content, and then shows it again.
    /// </summary>
    /// <param name="title"><see cref="Title"/> at the top of the snackbar.</param>
    /// <param name="message"><see cref="Message"/> in the content of the snackbar.</param>
    /// <returns><see langword="true"/> if invocation of <see langword="async"/> method succeeded, Exception otherwise.</returns>
    bool Show(string title, string message);

    /// <summary>
    /// Shows the snackbar. If it is already visible, firstly hides it for a moment, changes its content, and then shows it again.
    /// </summary>
    /// <param name="title"><see cref="Title"/> at the top of the snackbar.</param>
    /// <param name="message"><see cref="Message"/> in the content of the snackbar.</param>
    /// <param name="icon">Icon on the left.</param>
    /// <returns><see langword="true"/> if invocation of <see langword="async"/> method succeeded, Exception otherwise.</returns>
    bool Show(string title, string message, SymbolRegular icon);

    /// <summary>
    /// Shows the snackbar. If it is already visible, firstly hides it for a moment, changes its content, and then shows it again.
    /// </summary>
    /// <param name="title"><see cref="Title"/> at the top of the snackbar.</param>
    /// <param name="message"><see cref="Message"/> in the content of the snackbar.</param>
    /// <param name="icon">Icon on the left.</param>
    /// <param name="appearance"><see cref="IAppearanceControl.Appearance"/> of the snackbar.</param>
    /// <returns><see langword="true"/> if invocation of <see langword="async"/> method succeeded, Exception otherwise.</returns>
    bool Show(string title, string message, SymbolRegular icon, ControlAppearance appearance);

    /// <summary>
    /// Asynchronously shows the snackbar. If it is already visible, firstly hides it for a moment, changes its content, and then shows it again.
    /// </summary>
    /// <returns><see langword="true"/> if the operation was successful.</returns>
    Task<bool> ShowAsync();

    /// <summary>
    /// Asynchronously shows the snackbar. If it is already visible, firstly hides it for a moment, changes its content, and then shows it again.
    /// </summary>
    /// <param name="title"><see cref="Title"/> at the top of the snackbar.</param>
    /// <returns><see langword="true"/> if the operation was successful.</returns>
    Task<bool> ShowAsync(string title);

    /// <summary>
    /// Asynchronously shows the snackbar. If it is already visible, firstly hides it for a moment, changes its content, and then shows it again.
    /// </summary>
    /// <param name="title"><see cref="Title"/> at the top of the snackbar.</param>
    /// <param name="message"><see cref="Message"/> in the content of the snackbar.</param>
    /// <returns><see langword="true"/> if the operation was successful.</returns>
    Task<bool> ShowAsync(string title, string message);

    /// <summary>
    /// Asynchronously shows the snackbar. If it is already visible, firstly hides it for a moment, changes its content, and then shows it again.
    /// </summary>
    /// <param name="title"><see cref="Title"/> at the top of the snackbar.</param>
    /// <param name="message"><see cref="Message"/> in the content of the snackbar.</param>
    /// <param name="icon"><see cref="IIconControl.Icon"/> on the left.</param>
    /// <returns><see langword="true"/> if the operation was successful.</returns>
    Task<bool> ShowAsync(string title, string message, SymbolRegular icon);

    /// <summary>
    /// Asynchronously shows the snackbar. If it is already visible, firstly hides it for a moment, changes its content, and then shows it again.
    /// </summary>
    /// <param name="title"><see cref="Title"/> at the top of the snackbar.</param>
    /// <param name="message"><see cref="Message"/> in the content of the snackbar.</param>
    /// <param name="icon"><see cref="IIconControl.Icon"/> on the left.</param>
    /// <param name="appearance"><see cref="IAppearanceControl.Appearance"/> of the snackbar.</param>
    /// <returns><see langword="true"/> if the operation was successful.</returns>
    Task<bool> ShowAsync(string title, string message, SymbolRegular icon, ControlAppearance appearance);

    /// <summary>
    /// Hides the snackbar based on the selected animation, if control is visible.
    /// </summary>
    /// <returns><see langword="true"/> if invocation of <see langword="async"/> method succeeded, Exception otherwise.</returns>
    bool Hide();

    /// <summary>
    /// Asynchronously hides the snackbar based on the selected animation, if control is visible.
    /// </summary>
    /// <returns><see langword="true"/> if the operation was successful.</returns>
    Task<bool> HideAsync();
}
