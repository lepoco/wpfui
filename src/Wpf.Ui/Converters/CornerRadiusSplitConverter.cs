// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows.Data;

namespace Wpf.Ui.Converters;

public class CornerRadiusSplitConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        var original = new CornerRadius(0);
        if (values.Length > 0 && values[0] is CornerRadius cornerRadius)
        {
            original = cornerRadius;
        }

        bool isExpanded = false;
        if (values.Length > 1 && values[1] is bool isExpand)
        {
            isExpanded = isExpand;
        }

        var side = (parameter as string) ?? "Top";

        if (string.Equals(side, "Top", StringComparison.OrdinalIgnoreCase))
        {
            return isExpanded
                ? new CornerRadius(original.TopLeft, original.TopRight, 0, 0)
                : original;
        }
        else
        {
            return isExpanded
                ? new CornerRadius(0, 0, original.BottomRight, original.BottomLeft)
                : new CornerRadius(0);
        }
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        => throw new NotSupportedException();
}