// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;

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
    /// <para>If not added, or <see cref="String.Empty"/>, it will not be displayed.</para>
    /// </summary>
    public string PrimaryButtonText { get; set; } = String.Empty;

    /// <summary>
    /// Gets or sets the default text of the secondary button at the bottom of the content dialog.
    /// <para>If not added, or <see cref="String.Empty"/>, it will not be displayed.</para>
    /// </summary>
    public string SecondaryButtonText { get; set; } = String.Empty;
}
