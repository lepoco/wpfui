using System.Windows.Data;

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

public class SliderThumbPositionConverter : IMultiValueConverter
{
    public object Convert(object[]? values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values is [double trackActualDimension, double trackValue, double trackMinimum, double trackMaximum])
        {
            return trackActualDimension * (trackValue - trackMinimum) / (trackMaximum - trackMinimum);
        }

        return Binding.DoNothing;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}