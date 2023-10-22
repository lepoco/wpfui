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
        if (value is NavigationViewPaneDisplayMode.LeftFluent)
        {
            return 1;
        }

        if (value is NavigationViewPaneDisplayMode.Top)
        {
            return 2;
        }

        if (value is NavigationViewPaneDisplayMode.Bottom)
        {
            return 3;
        }

        return 0;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is 1)
        {
            return NavigationViewPaneDisplayMode.LeftFluent;
        }

        if (value is 2)
        {
            return NavigationViewPaneDisplayMode.Top;
        }

        if (value is 3)
        {
            return NavigationViewPaneDisplayMode.Bottom;
        }

        return NavigationViewPaneDisplayMode.Left;
    }
}
