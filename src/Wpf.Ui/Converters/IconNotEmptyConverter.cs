// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Windows.Data;

namespace Wpf.Ui.Converters;

/// <summary>
/// Checks if the <see cref="Common.SymbolRegular"/> is valid and not empty.
/// </summary>
internal class IconNotEmptyConverter : IValueConverter
{
    /// <summary>
    /// Checks if the <see cref="Common.SymbolRegular"/> is valid and not empty.
    /// </summary>
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        if (value is Common.SymbolRegular icon)
            return icon != Common.SymbolRegular.Empty;

        return false;
    }

    /// <summary>
    /// Not Implemented.
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    public object ConvertBack(object value, Type targetType, object parameter,
        System.Globalization.CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
