// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows.Data;
using Wpf.Ui.Controls;

namespace Wpf.Ui.Converters;

internal class BackButtonVisibilityToVisibilityConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value switch
        {
            not NavigationViewBackButtonVisible _ => Visibility.Collapsed,
            NavigationViewBackButtonVisible.Collapsed => Visibility.Collapsed,
            NavigationViewBackButtonVisible.Visible => Visibility.Visible,
            NavigationViewBackButtonVisible.Auto => Visibility.Visible,
            _ => Visibility.Collapsed,
        };
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
