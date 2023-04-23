// Based on Windows UI Library
// Copyright(c) Microsoft Corporation.All rights reserved.

// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;

namespace Wpf.Ui.Controls.Navigation;

public partial class NavigationView
{
    /// <summary>
    /// Attached property for <see cref="NavigationView"/>'s to get its parent.
    /// </summary>
    internal static readonly DependencyProperty NavigationParentProperty = DependencyProperty.RegisterAttached(
        nameof(NavigationParent), typeof(INavigationView), typeof(INavigationView),
        new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));

    /// <summary>
    /// 
    /// </summary>
    internal INavigationView NavigationParent
    {
        get => (INavigationView)GetValue(NavigationParentProperty);
        private set => SetValue(NavigationParentProperty, value);
    }

    /// <summary>
    /// Gets the <see cref="NavigationView"/> parent view for its <see cref="INavigationViewItem"/> children.
    /// </summary>
    /// <param name="navigationItem"></param>
    /// <returns>Instance of the <see cref="NavigationView"/> or <see langword="null"/>.</returns>
    internal static NavigationView? GetNavigationParent<T>(T navigationItem) where T : DependencyObject, INavigationViewItem
    {
        if (navigationItem.GetValue(NavigationParentProperty) is NavigationView navigationView)
            return navigationView;

        return null;
    }
}
