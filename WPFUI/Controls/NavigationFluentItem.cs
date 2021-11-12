// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;
using System.Windows.Media.Imaging;

namespace WPFUI.Controls
{
    /// <summary>
    /// Navigation element for <see cref="NavigationFluent"/>.
    /// </summary>
    public class NavigationFluentItem : NavigationItem
    {
        /// <summary>
        /// Property for <see cref="Image"/>.
        /// </summary>
        public static readonly DependencyProperty ImageProperty = DependencyProperty.Register("Image",
            typeof(BitmapSource), typeof(NavigationFluentItem),
            new PropertyMetadata(new BitmapImage(), OnImageChanged));

        /// <summary>
        /// Property for <see cref="IsImage"/>.
        /// </summary>
        public static readonly DependencyProperty IsImageProperty = DependencyProperty.Register("IsImage",
            typeof(bool), typeof(NavigationFluentItem), new PropertyMetadata(false));

        /// <summary>
        /// Gets or sets image displayed next to the card name instead of the icon.
        /// </summary>
        public BitmapSource Image
        {
            get => GetValue(ImageProperty) as BitmapSource;
            set => SetValue(ImageProperty, value);
        }

        /// <summary>
        /// Gets or sets information whether the element contains image icon.
        /// </summary>
        public bool IsImage { get; set; }

        private static void OnImageChanged(DependencyObject dependency, DependencyPropertyChangedEventArgs eventArgs)
        {
            if (dependency is not NavigationFluentItem control) return;
            control.SetValue(IsImageProperty, true);
        }
    }
}