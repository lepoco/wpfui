// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Wpf.Ui.Controls;

namespace Wpf.Ui.Gallery.Helpers;

internal sealed class PaneDisplayModeToIndexConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value switch
        {
            NavigationViewPaneDisplayMode.LeftMinimal => 1,
            NavigationViewPaneDisplayMode.LeftFluent => 2,
            NavigationViewPaneDisplayMode.Top => 3,
            NavigationViewPaneDisplayMode.Bottom => 4,
            _ => 0,
        };
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value switch
        {
            1 => NavigationViewPaneDisplayMode.LeftMinimal,
            2 => NavigationViewPaneDisplayMode.LeftFluent,
            3 => NavigationViewPaneDisplayMode.Top,
            4 => NavigationViewPaneDisplayMode.Bottom,
            _ => NavigationViewPaneDisplayMode.Left,
        };
    }
}
