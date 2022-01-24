// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Windows;
using WPFUI.Controls.Interfaces;

namespace WPFUI.Controls
{
    /// <summary>
    /// Displays the name of the current <see cref="NavigationItem"/> and it's parents that can be navigated using <see cref="INavigation"/>.
    /// </summary>
    public class Breadcrumb : System.Windows.Controls.Control
    {
        /// <summary>
        /// Property for <see cref="Current"/>.
        /// </summary>
        public static readonly DependencyProperty CurrentProperty = DependencyProperty.Register(nameof(Current),
            typeof(string), typeof(Breadcrumb), new PropertyMetadata(String.Empty));

        /// <summary>
        /// Property for <see cref="Navigation"/>.
        /// </summary>
        public static readonly DependencyProperty NavigationProperty = DependencyProperty.Register(nameof(Navigation),
            typeof(INavigation), typeof(Breadcrumb),
            new PropertyMetadata(null, NavigationPropertyChangedCallback));

        /// <summary>
        /// <see cref="INavigation"/> based on which <see cref="Breadcrumb"/> displays the titles.
        /// </summary>
        public string Current
        {
            get => (string)GetValue(CurrentProperty);
            set => SetValue(CurrentProperty, value);
        }

        /// <summary>
        /// <see cref="INavigation"/> based on which <see cref="Breadcrumb"/> displays the titles.
        /// </summary>
        public INavigation Navigation
        {
            get => GetValue(NavigationProperty) as INavigation;
            set => SetValue(NavigationProperty, value);
        }

        private void BuildBreadcrumb()
        {
#if DEBUG
            System.Diagnostics.Debug.WriteLine("Navigated");
            System.Diagnostics.Debug.WriteLine(Navigation.GetType());
#endif

            //TODO: Navigate with previous levels

            if (Navigation?.Current is INavigationItem item)
            {
                string pageName = item.Content as string;

                if (String.IsNullOrEmpty(pageName)) return;

                Current = pageName;
            }
        }

        private static void NavigationPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not Breadcrumb control) return;

            control.Navigation.Navigated += control.NavigationOnNavigated;
        }

        private void NavigationOnNavigated(object sender, RoutedEventArgs e)
        {
            BuildBreadcrumb();
        }
    }
}
