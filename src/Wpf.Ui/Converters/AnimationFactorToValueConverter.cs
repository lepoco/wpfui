using System;
using System.Globalization;
using System.Windows.Data;

namespace Wpf.Ui.Converters;

public class AnimationFactorToValueConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values[0] is not double completeValue) return 0.0;
        if (values[1] is not double factor) return 0.0;
        if (parameter is "negative") factor = -factor;

        return factor * completeValue;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
