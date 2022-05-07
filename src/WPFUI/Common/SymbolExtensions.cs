// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;

namespace WPFUI.Common;

/// <summary>
/// Set of extensions for the enumeration of icons to facilitate their management and replacement.
/// </summary>
public static class SymbolExtensions
{
    /// <summary>
    /// Replaces <see cref="SymbolRegular"/> with <see cref="SymbolFilled"/>.
    /// </summary>
    public static SymbolFilled Swap(this SymbolRegular icon)
    {
        // TODO: It is possible that the alternative icon does not exist
        return Glyph.ParseFilled(icon.ToString());
    }

    /// <summary>
    /// Replaces <see cref="SymbolFilled"/> with <see cref="SymbolRegular"/>.
    /// </summary>
    public static SymbolRegular Swap(this SymbolFilled icon)
    {
        // TODO: It is possible that the alternative icon does not exist
        return Glyph.Parse(icon.ToString());
    }

    /// <summary>
    /// Converts <see cref="SymbolRegular"/> to <see langword="char"/> based on the ID.
    /// </summary>
    public static char GetGlyph(this SymbolRegular icon)
    {
        return ToChar(icon);
    }

    /// <summary>
    /// Converts <see cref="SymbolFilled"/> to <see langword="char"/> based on the ID.
    /// </summary>
    public static char GetGlyph(this SymbolFilled icon)
    {
        return ToChar(icon);
    }

    /// <summary>
    /// Converts <see cref="SymbolRegular"/> to <see langword="string"/> based on the ID.
    /// </summary>
    public static string GetString(this Common.SymbolRegular icon)
    {
        return icon.GetGlyph().ToString();
    }

    /// <summary>
    /// Converts <see cref="SymbolFilled"/> to <see langword="string"/> based on the ID.
    /// </summary>
    public static string GetString(this Common.SymbolFilled icon)
    {
        return icon.GetGlyph().ToString();
    }

    /// <summary>
    /// Converts <see cref="SymbolRegular"/> to <see langword="char"/>.
    /// </summary>
    private static char ToChar(Common.SymbolRegular icon)
    {
        return Convert.ToChar(icon);
    }

    /// <summary>
    /// Converts <see cref="SymbolFilled"/> to <see langword="char"/>.
    /// </summary>
    private static char ToChar(Common.SymbolFilled icon)
    {
        return Convert.ToChar(icon);
    }
}
