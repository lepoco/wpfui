// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows.Data;

namespace Wpf.Ui.Converters;

internal class DatePickerButtonPaddingConverter : IMultiValueConverter
{
    /// <inheritdoc />
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values is [Thickness padding, Thickness buttonMargin, double buttonWidth])
        {
            return new Thickness(
                padding.Left,
                padding.Top,
                padding.Right + buttonMargin.Left + buttonMargin.Right + buttonWidth,
                padding.Bottom
            );
        }

        return default(Thickness);
    }

    /// <inheritdoc />
    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
