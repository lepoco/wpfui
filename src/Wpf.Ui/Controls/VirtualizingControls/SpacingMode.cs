// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Based on VirtualizingWrapPanel created by S. Bäumlisberger licensed under MIT license.
// https://github.com/sbaeumlisberger/VirtualizingWrapPanel
// Copyright (C) S. Bäumlisberger, Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

namespace Wpf.Ui.Controls.VirtualizingControls;

/// <summary>
/// Specifies how remaining space is distributed.
/// <para>Based on <see href="https://github.com/sbaeumlisberger/VirtualizingWrapPanel"/>.</para>
/// </summary>
public enum SpacingMode
{
    /// <summary>
    /// Spacing is disabled and all items will be arranged as closely as possible.
    /// </summary>
    None,

    /// <summary>
    /// The remaining space is evenly distributed between the items on a layout row, as well as the start and end of each row.
    /// </summary>
    Uniform,

    /// <summary>
    /// The remaining space is evenly distributed between the items on a layout row, excluding the start and end of each row.
    /// </summary>
    BetweenItemsOnly,

    /// <summary>
    /// The remaining space is evenly distributed between start and end of each row.
    /// </summary>
    StartAndEndOnly
}
