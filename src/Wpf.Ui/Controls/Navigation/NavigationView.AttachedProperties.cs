// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;

namespace Wpf.Ui.Controls.Navigation;

public partial class NavigationView
{
    public static readonly DependencyProperty HeaderContentProperty = 
        DependencyProperty.RegisterAttached(
            "HeaderContent",
            typeof(object),
            typeof(FrameworkElement),
            new FrameworkPropertyMetadata(null)
        );

    public static object? GetHeaderContent(FrameworkElement target) => target.GetValue(HeaderContentProperty);
    public static void SetHeaderContent(FrameworkElement target, object headerContent) => target.SetValue(HeaderContentProperty, headerContent);
}
