// This Source Code is partially based on the source code provided by the .NET Foundation.
// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) .NET Foundation Contributors, WPF UI Contributors, Leszek Pomianowski.
// All Rights Reserved.

namespace Wpf.Ui.Controls.NumberBoxControl;

/// <summary>
/// Defines values that specify how the spin buttons used to increment or decrement the <see cref="NumberBox.Value"/> are displayed.
/// </summary>
public enum NumberBoxSpinButtonPlacementMode
{
    /// <summary>
    /// The spin buttons are not displayed.
    /// </summary>
    Hidden,

    /// <summary>
    /// The spin buttons have two visual states, depending on focus. By default, the spin buttons are displayed in a compact, vertical orientation. When the Numberbox gets focus, the spin buttons expand.
    /// </summary>
    Compact,

    /// <summary>
    /// The spin buttons are displayed in an expanded, horizontal orientation.
    /// </summary>
    Inline
}
