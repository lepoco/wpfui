// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Windows;
using System.Windows.Data;

namespace Wpf.Ui.Converters;

/// <summary>
/// Converts Thickness to Thickness.
/// </summary>
internal class RightSplitThicknessConverter : IValueConverter
{
    /// <summary>
    /// Checks if the <see cref="Thickness"/> is valid and then, removes corners.
    /// </summary>
    public object Convert(
        object value,
        Type targetType,
        object parameter,
        System.Globalization.CultureInfo culture
    )
    {
        if (value is not Thickness thickness)
        {
            return value;
        }

        return new Thickness(thickness.Left, thickness.Top, thickness.Right, thickness.Bottom);
    }

    /// <summary>
    /// Not Implemented.
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    public object ConvertBack(
        object value,
        Type targetType,
        object parameter,
        System.Globalization.CultureInfo culture
    )
    {
        throw new NotImplementedException();
    }
}
