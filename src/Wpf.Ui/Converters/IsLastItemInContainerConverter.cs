using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows;

namespace Wpf.Ui.Converters;

public class IsLastItemInContainerConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        DependencyObject item = (DependencyObject)value;
        ItemsControl ic = ItemsControl.ItemsControlFromItemContainer(item);

        return ic != null && ic.ItemContainerGenerator.IndexFromContainer(item) == ic.Items.Count - 1;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
