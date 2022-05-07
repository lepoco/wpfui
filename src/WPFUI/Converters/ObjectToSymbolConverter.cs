// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Windows.Data;
using WPFUI.Common;

namespace WPFUI.Converters;

/// <summary>
/// Tries to convert <see langword="object"/>  to <see cref="SymbolFilled"/>.
/// </summary>
internal class ObjectToSymbolConverter : IValueConverter
{
    /// <summary>
    /// Converts <see cref="SymbolRegular"/> or <see cref="SymbolFilled"/> to <see langword="string"/>.
    /// <para>If the given value is <see langword="char"/> or <see langword="string"/> it will simply be returned as a <see langword="string"/>.</para>
    /// </summary>
    /// <returns><see langword="string"/> representing <see cref="SymbolRegular"/> or <see cref="SymbolFilled"/>.</returns>
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

