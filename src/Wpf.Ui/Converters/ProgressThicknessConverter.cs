// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Windows.Data;

namespace Wpf.Ui.Converters;

/// <summary>
/// Converts Height to Thickness.
/// </summary>
class ProgressThicknessConverter : IValueConverter
{
    /// <summary>
    /// Checks if the <see cref="Common.SymbolRegular"/> is valid and not empty.
    /// </summary>
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        // TODO: It's too hardcoded, we should define better formula.

        if (value is double height)
            return height / 8;

        return 12.0d;
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
