using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using Wpf.Ui.Controls;

namespace Wpf.Ui.Converters;

public class EnumToBoolConverter<TEnum> : IValueConverter where TEnum : Enum
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not TEnum valueEnum)
            throw new ArgumentException($"{nameof(value)} is not type: {typeof(TEnum)}");

        if (parameter is not TEnum parameterEnum)
            throw new ArgumentException($"{nameof(parameter)} is not type: {typeof(TEnum)}");

        return EqualityComparer<TEnum>.Default.Equals(valueEnum, parameterEnum);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

internal class ContentDialogButtonEnumToBoolConverter : EnumToBoolConverter<ContentDialogButton> { }
