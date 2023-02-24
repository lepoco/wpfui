using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Wpf.Ui.Controls.IconElements;

namespace Wpf.Ui.Converters;

public class IconSourceElementConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
        ConvertToIconElement(value);

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();

    public static object ConvertToIconElement(DependencyObject d, object basevalue) => ConvertToIconElement(basevalue);

    private static object ConvertToIconElement(object value)
    {
        if (value is not IconSourceElement iconSourceElement)
            return value;

        if (iconSourceElement.IconSource is null)
            throw new ArgumentException(nameof(iconSourceElement.IconSource));

        return iconSourceElement.IconSource.CreateIconElement();
    }
}
