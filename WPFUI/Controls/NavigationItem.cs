// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;
using System.Windows.Media.Imaging;

namespace WPFUI.Controls
{
    /// <summary>
    /// Navigation element.
    /// </summary>
    public class NavigationItem : System.Windows.Controls.Button, IIconElement
    {
        /// <summary>
        /// Property for <see cref="IsActive"/>.
        /// </summary>
        public static readonly DependencyProperty IsActiveProperty = DependencyProperty.Register(nameof(IsActive),
            typeof(bool), typeof(NavigationItem), new PropertyMetadata(false));

        /// <summary>
        /// Property for <see cref="Icon"/>.
        /// </summary>
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(nameof(Icon),
            typeof(Common.Icon), typeof(NavigationItem),
            new PropertyMetadata(Common.Icon.Empty));

        /// <summary>
        /// Property for <see cref="IconFilled"/>.
        /// </summary>
        public static readonly DependencyProperty IconFilledProperty = DependencyProperty.Register(nameof(IconFilled),
            typeof(bool), typeof(NavigationItem), new PropertyMetadata(false));

        /// <summary>
        /// Property for <see cref="Image"/>.
        /// </summary>
        public static readonly DependencyProperty ImageProperty = DependencyProperty.Register("Image",
            typeof(BitmapSource), typeof(NavigationItem),
            new PropertyMetadata(null, OnImageChanged));

        /// <summary>
        /// Property for <see cref="IsImage"/>.
        /// </summary>
        public static readonly DependencyProperty IsImageProperty = DependencyProperty.Register("IsImage",
            typeof(bool), typeof(NavigationItem), new PropertyMetadata(false));

        /// <summary>
        /// Gets information whether the current element is active.
        /// </summary>
        public bool IsActive
        {
            get => (bool)GetValue(IsActiveProperty);
            set => SetValue(IsActiveProperty, value);
        }

        /// <inheritdoc />
        public Common.Icon Icon
        {
            get => (Common.Icon)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        /// <inheritdoc />
        public bool IconFilled
        {
            get => (bool)GetValue(IconFilledProperty);
            set => SetValue(IconFilledProperty, value);
        }

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
        public bool IsImage
        {
            get => (bool)GetValue(IsImageProperty);
            set => SetValue(IsImageProperty, value);
        }

        private static void OnImageChanged(DependencyObject dependency, DependencyPropertyChangedEventArgs eventArgs)
        {
            if (dependency is not NavigationItem control) return;

            control.SetValue(IsImageProperty, control.Image != null);
        }
    }
}