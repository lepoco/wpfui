// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;

namespace Wpf.Ui.Common;

/// <summary>
/// Set of static methods to operate on <see cref="SymbolRegular"/> and <see cref="SymbolFilled"/>.
/// </summary>
public static class Glyph
{
    /// <summary>
    /// If the icon is not found in some places, this one will be displayed.
    /// </summary>
    public const SymbolRegular DefaultIcon = SymbolRegular.Heart28;

    /// <summary>
    /// If the filled icon is not found in some places, this one will be displayed.
    /// </summary>
    public const SymbolFilled DefaultFilledIcon = SymbolFilled.Heart28;

    /// <summary>
    /// Finds icon based on name.
    /// </summary>
    /// <param name="name">Name of the icon.</param>
    public static Common.SymbolRegular Parse(string name)
    {
        if (String.IsNullOrEmpty(name))
            return DefaultIcon;

        return (Common.SymbolRegular)Enum.Parse(typeof(Common.SymbolRegular), name);
    }

    /// <summary>
    /// Finds icon based on name.
    /// </summary>
    /// <param name="name">Name of the icon.</param>
    public static Common.SymbolFilled ParseFilled(string name)
    {
        if (String.IsNullOrEmpty(name))
            return DefaultFilledIcon;

        return (Common.SymbolFilled)Enum.Parse(typeof(Common.SymbolFilled), name);
    }
}
