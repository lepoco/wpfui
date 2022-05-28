// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows.Media;

namespace WPFUI.Controls.Interfaces;

/// <summary>
/// UI <see cref="System.Windows.Controls.Control"/> with <see cref="Common.SymbolRegular"/> attributes.
/// </summary>
public interface IIconControl
{
    /// <summary>
    /// Gets or sets displayed <see cref="Common.SymbolRegular"/>.
    /// </summary>
    Common.SymbolRegular Icon { get; set; }

    /// <summary>
    /// Defines whether or not we should use the <see cref="Common.SymbolFilled"/>.
    /// </summary>
    bool IconFilled { get; set; }

    /// <summary>
    /// Foreground of the <see cref="Common.SymbolRegular"/>.
    /// </summary>
    Brush IconForeground { get; set; }
}
