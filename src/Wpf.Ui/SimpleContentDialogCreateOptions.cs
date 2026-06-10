// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Wpf.Ui.Controls;

namespace Wpf.Ui;

/// <summary>
/// Set of properties used when creating a new simple content dialog.
/// </summary>
public class SimpleContentDialogCreateOptions
{
    /// <summary>
    /// Gets or sets a name at the top of the content dialog.
    /// </summary>
    public required string Title { get; set; }

    /// <summary>
    /// Gets or sets a message displayed in the content dialog.
    /// </summary>
    public required object Content { get; set; }

    /// <summary>
    /// Gets or sets the name of the button that closes the content dialog.
    /// </summary>
    public required string CloseButtonText { get; set; }

    /// <summary>
    /// Gets or sets the default text of the primary button at the bottom of the content dialog.
    /// <para>If not added, or <see cref="string.Empty"/>, it will not be displayed.</para>
    /// </summary>
    public string PrimaryButtonText { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the default text of the secondary button at the bottom of the content dialog.
    /// <para>If not added, or <see cref="string.Empty"/>, it will not be displayed.</para>
    /// </summary>
    public string SecondaryButtonText { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the button that is activated by default when the user presses the Enter key in the dialog.
    /// </summary>
    /// <remarks>Use this property to specify which button receives keyboard focus and is triggered by default
    /// when the dialog is shown. This can improve usability by guiding users toward the recommended action.</remarks>
    public ContentDialogButton DefaultButton { get; set; } = ContentDialogButton.Primary;
}
