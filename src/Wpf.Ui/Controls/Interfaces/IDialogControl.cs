// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Threading.Tasks;
using System.Windows;
using Wpf.Ui.Common;

namespace Wpf.Ui.Controls.Interfaces;

/// <summary>
/// Represents a Dialog control.
/// </summary>
public interface IDialogControl
{
    /// <summary>
    /// Which of the button is pressed.
    /// </summary>
    public enum ButtonPressed
    {
        /// <summary>
        /// None.
        /// </summary>
        None,

        /// <summary>
        /// Left button.
        /// </summary>
        Left,

        /// <summary>
        /// Right button.
        /// </summary>
        Right
    }

    /// <summary>
    /// Gets the information whether the <see cref="IDialogControl"/> is visible.
    /// </summary>
    bool IsShown { get; }

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
    /// Gets or sets the title displayed at the top of the <see cref="IDialogControl"/>.
    /// </summary>
    string Title { get; set; }

    /// <summary>
    /// Gets or sets the message displayed inside the <see cref="IDialogControl"/>.
    /// </summary>
    string Message { get; set; }

    /// <summary>
    /// <see cref="FrameworkElement"/> or <see langword="string"/> displayed below the <see cref="Title"/> and <see cref="Message"/>.
    /// </summary>
    object Content { get; set; }

    /// <summary>
    /// Gets or sets the name of the left button displayed in the <see cref="IDialogControl"/> footer.
    /// </summary>
    string ButtonLeftName { get; set; }

    /// <summary>
    /// Gets or sets the name of the right button displayed in the <see cref="IDialogControl"/> footer.
    /// </summary>
    string ButtonRightName { get; set; }

    /// <summary>
    /// Occurs when the right button in the dialog footer is clicked.
    /// </summary>
    event RoutedEventHandler ButtonRightClick;

    /// <summary>
    /// Occurs when the left button in the dialog footer is clicked.
    /// </summary>
    event RoutedEventHandler ButtonLeftClick;

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
    Task<ButtonPressed> ShowAndWaitAsync();

    /// <summary>
    /// Reveals the <see cref="IDialogControl"/> and waits for the user to click on of the footer buttons.
    /// </summary>
    /// <param name="hideOnClick">Whether the dialogue should be hidden after pressing any button in the footer.</param>
    /// <returns>Information about which button was pressed.</returns>
    Task<ButtonPressed> ShowAndWaitAsync(bool hideOnClick);

    /// <summary>
    /// Reveals the <see cref="IDialogControl"/> and waits for the user to click on of the footer buttons.
    /// </summary>
    /// <param name="title"><see cref="Title"/> at the top of the dialog.</param>
    /// <param name="message"><see cref="Message"/> above the <see cref="Content"/> of the dialog.</param>
    /// <returns>Information about which button was pressed.</returns>
    Task<ButtonPressed> ShowAndWaitAsync(string title, string message);

    /// <summary>
    /// Reveals the <see cref="IDialogControl"/> and waits for the user to click on of the footer buttons.
    /// </summary>
    /// <param name="title"><see cref="Title"/> at the top of the dialog.</param>
    /// <param name="message"><see cref="Message"/> above the <see cref="Content"/> of the dialog.</param>
    /// <param name="hideOnClick">Whether the dialogue should be hidden after pressing any button in the footer.</param>
    /// <returns>Information about which button was pressed.</returns>
    Task<ButtonPressed> ShowAndWaitAsync(string title, string message, bool hideOnClick);

    /// <summary>
    /// Hides the <see cref="IDialogControl"/>.
    /// <returns><see langword="true"/> if the operation was successful.</returns>
    /// </summary>
    bool Hide();
}
