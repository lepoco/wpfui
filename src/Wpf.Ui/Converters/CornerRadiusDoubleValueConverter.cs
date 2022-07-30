// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Wpf.Ui.Converters
{
    public enum CornerRadiusOption
    {
        BottomLeft, BottomRight, TopLeft, TopRight
    }

    public class CornerRadiusDoubleValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is CornerRadius cornerRadius)
            {
                if (parameter is CornerRadiusOption option)
                {
                    double radius;

                    switch (option)
                    {
                        case CornerRadiusOption.BottomLeft: radius = cornerRadius.BottomLeft; break;
                        case CornerRadiusOption.BottomRight: radius = cornerRadius.BottomRight; break;
                        case CornerRadiusOption.TopLeft: radius = cornerRadius.TopLeft; break;
                        case CornerRadiusOption.TopRight: radius = cornerRadius.TopRight; break;
                        default: radius = cornerRadius.TopLeft; break;
                    }

                    return radius;
                }

                throw new ArgumentException("Expected a ColorPicker.Converters.CornerRadiusOption value", nameof(parameter));
            }

            throw new ArgumentException("Expected a System.Windows.CornerRadius object", nameof(value));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
