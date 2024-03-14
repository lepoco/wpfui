// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows.Controls;
using System.Windows.Data;
using Wpf.Ui.Controls;

namespace Wpf.Ui.Converters;

internal class ViewToListViewViewStateConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value switch
        {
            GridView => ListViewViewState.GridView,
            null => ListViewViewState.Default,
            _ => ListViewViewState.Default
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException("Converting from ListViewViewState to View is not supported.");
    }
}
