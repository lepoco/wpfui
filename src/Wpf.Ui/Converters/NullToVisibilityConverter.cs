// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;
using System.Windows.Data;

namespace Wpf.Ui.Converters;

internal class NullToVisibilityConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string str)
        {
            value = string.IsNullOrEmpty(str);
        }
        else if (value == null)
        {
            value = true;
        }
        else
        {
            value = false;
        }

        if (parameter is "negate")
        {
            return (bool)value ? Visibility.Visible : Visibility.Collapsed;
        }

        return (bool)value ? Visibility.Collapsed : Visibility.Visible;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
