// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Wpf.Ui.Controls.IconElements;

namespace Wpf.Ui.Converters;

/// <summary>
/// Converts <see cref="IconSourceElement.IconSource"/> to <see cref="IconElement"/>
/// </summary>
public sealed class IconSourceElementConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => ConvertToIconElement(value);

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();

    public static object ConvertToIconElement(DependencyObject _, object baseValue) => ConvertToIconElement(baseValue);

    private static object ConvertToIconElement(object value)
    {
        if (value is not IconSourceElement iconSourceElement)
            return value;

        if (iconSourceElement.IconSource is null)
            throw new ArgumentException(nameof(iconSourceElement.IconSource));

        return iconSourceElement.IconSource.CreateIconElement();
    }
}
