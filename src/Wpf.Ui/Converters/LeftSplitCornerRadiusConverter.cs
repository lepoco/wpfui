// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Windows;
using System.Windows.Data;

namespace Wpf.Ui.Converters;

/// <summary>
/// Converts CornerRadius to CornerRadius.
/// </summary>
internal class LeftSplitCornerRadiusConverter : IValueConverter
{
    /// <summary>
    /// Checks if the <see cref="CornerRadius"/> is valid and then, removes corners.
    /// </summary>
    public object Convert(
        object value,
        Type targetType,
        object parameter,
        System.Globalization.CultureInfo culture
    )
    {
        if (value is not CornerRadius cornerRadius)
        {
            return value;
        }

        return new CornerRadius(cornerRadius.TopLeft, 0, 0, cornerRadius.BottomLeft);
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
