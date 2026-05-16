// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows.Data;
using Wpf.Ui.Controls;

namespace Wpf.Ui.Converters;

public class TimeSpanToHourMultiConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values.Length < 2 || !(values[0] is TimeSpan time) || !(values[1] is ClockIdentifier clock))
        {
            return "00";
        }

        if (clock == ClockIdentifier.Clock24Hour)
        {
            return time.Hours.ToString("D2");
        }
        else // Clock12Hour
        {
            int hour = time.Hours % 12;
            if (hour == 0)
            {
                hour = 12;
            }

            return hour.ToString();
        }
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
