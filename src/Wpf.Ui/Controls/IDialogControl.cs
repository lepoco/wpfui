// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Threading.Tasks;
using System.Windows;
using Wpf.Ui.Common;

namespace Wpf.Ui.Controls;

/// <summary>
/// Represents a Dialog control.
/// </summary>
public interface IDialogControl
{
    /// <summary>
    /// Gets the information whether the <see cref="IDialogControl"/> is visible.
    /// </summary>
    bool IsShown { get; }

    /// <summary>
    /// Gets or sets the header displayed at the top of the <see cref="IDialogControl"/>.
    /// </summary>
    object Header { get; set; }

    /// <summary>
    /// <see cref="FrameworkElement"/> or <see langword="string"/> displayed below the <see cref="Header"/> and <see cref="Message"/>.
    /// </summary>
    object Content { get; set; }

    /// <summary>
    /// Custom control or text displayed at the bottom of the <see cref="IDialogControl"/> instead of the buttons.
    /// </summary>
    object Footer { get; set; }

    /// <summary>
    /// Gets or sets maximum dialog width.
    /// </summary>
    double DialogWidth { get; set; }

    /// <summary>
    /// Gets or sets maximum dialog height.
    /// </summary>
    double DialogHeight { get; set; }

    /// <summary>
    /// Gets or sets the name of the left button displayed in the <see cref="IDialogControl"/> footer.
    /// </summary>
    System.Windows.Controls.Primitives.ButtonBase ButtonLeft { get; set; }

    /// <summary>
    /// Gets or sets the name of the right button displayed in the <see cref="IDialogControl"/> footer.
    /// </summary>
    System.Windows.Controls.Primitives.ButtonBase ButtonRight { get; set; }

    /// <summary>
    /// Occurs when the dialog is about to open.
    /// </summary>
    public event RoutedDialogEvent Opened;

    /// <summary>
    /// Occurs when the dialog is about to close.
    /// </summary>
    public event RoutedDialogEvent Closed;

    /// <summary>
    /// Reveals the <see cref="IDialogControl"/>.
    /// </summary>
    /// <returns><see langword="true"/> if the operation was successful.</returns>
    bool Show();

    /// <summary>
    /// Reveals the <see cref="IDialogControl"/>.
    /// </summary>
    /// <param name="title"><see cref="Title"/> at the top of the dialog.</param>
    /// <param name="message"><see cref="Message"/> above the <see cref="Content"/> of the dialog.</param>
    /// <returns><see langword="true"/> if the operation was successful.</returns>
    bool Show(string title, string message);

    /// <summary>
    /// Reveals the <see cref="IDialogControl"/> and waits for the user to click on of the footer buttons.
    /// </summary>
    /// <returns>Information about which button was pressed.</returns>
    Task<DialogButtonPressed> ShowAndWaitAsync();

    /// <summary>
    /// Reveals the <see cref="IDialogControl"/> and waits for the user to click on of the footer buttons.
    /// </summary>
    /// <param name="hideOnClick">Whether the dialogue should be hidden after pressing any button in the footer.</param>
    /// <returns>Information about which button was pressed.</returns>
    Task<DialogButtonPressed> ShowAndWaitAsync(bool hideOnClick);

    /// <summary>
    /// Reveals the <see cref="IDialogControl"/> and waits for the user to click on of the footer buttons.
    /// </summary>
    /// <param name="title"><see cref="Title"/> at the top of the dialog.</param>
    /// <param name="message"><see cref="Message"/> above the <see cref="Content"/> of the dialog.</param>
    /// <returns>Information about which button was pressed.</returns>
    Task<DialogButtonPressed> ShowAndWaitAsync(string title, string message);

    /// <summary>
    /// Reveals the <see cref="IDialogControl"/> and waits for the user to click on of the footer buttons.
    /// </summary>
    /// <param name="title"><see cref="Title"/> at the top of the dialog.</param>
    /// <param name="message"><see cref="Message"/> above the <see cref="Content"/> of the dialog.</param>
    /// <param name="hideOnClick">Whether the dialogue should be hidden after pressing any button in the footer.</param>
    /// <returns>Information about which button was pressed.</returns>
    Task<DialogButtonPressed> ShowAndWaitAsync(string title, string message, bool hideOnClick);

    /// <summary>
    /// Hides the <see cref="IDialogControl"/>.
    /// <returns><see langword="true"/> if the operation was successful.</returns>
    /// </summary>
    bool Hide();
}
