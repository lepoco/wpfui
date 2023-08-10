// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

namespace Wpf.Ui.Controls;

/// <summary>
/// Specifies identifiers to indicate the return value of a <see cref="ContentDialog"/>.
/// </summary>
public enum ContentDialogResult
{
    /// <summary>
    /// No button was tapped.
    /// </summary>
    None,

    /// <summary>
    /// The primary button was tapped by the user.
    /// </summary>
    Primary,

    /// <summary>
    /// The secondary button was tapped by the user.
    /// </summary>
    Secondary
}
