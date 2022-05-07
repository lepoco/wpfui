// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Windows.Data;
using WPFUI.Common;

namespace WPFUI.Converters;

/// <summary>
/// Tries to convert <see langword="object"/>  to <see cref="SymbolRegular"/>.
/// </summary>
internal class ObjectToSymbolConverter : IValueConverter
{
    /// <summary>
    /// Converts <see langword="object"/> to <see cref="SymbolRegular"/>.
    /// <para>If the given value is <see cref="SymbolRegular"/> or <see cref="SymbolFilled"/> it will simply be returned as a <see cref="SymbolRegular"/>.</para>
    /// </summary>
    /// <returns>Valid <see cref="SymbolRegular"/> or <see cref="SymbolRegular.Empty"/> if failed.</returns>
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        if (value is SymbolRegular symbol)
            return symbol;

        if (value is SymbolFilled symbolFilled)
            return symbolFilled.Swap();

        return SymbolRegular.Empty;
    }

    /// <summary>
    /// Not Implemented.
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

