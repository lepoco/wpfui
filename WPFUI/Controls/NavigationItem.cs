// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace WPFUI.Controls
{
    /// <summary>
    /// Navigation element.
    /// </summary>
    public class NavigationItem : System.Windows.Controls.Button, IIconElement
    {
        private static readonly Type WindowsPage = typeof(System.Windows.Controls.Page);

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
        /// Property for <see cref="FontIcon"/>.
        /// </summary>
        public static readonly DependencyProperty FontIconProperty = DependencyProperty.Register(nameof(FontIcon),
            typeof(string), typeof(NavigationItem),
            new PropertyMetadata(null));

        /// <summary>
        /// Property for <see cref="Image"/>.
        /// </summary>
        public static readonly DependencyProperty ImageProperty = DependencyProperty.Register(nameof(Image),
            typeof(BitmapSource), typeof(NavigationItem),
            new PropertyMetadata(null));

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
        /// Gets or sets font icon displayed next to the card name instead of the icon.
        /// </summary>
        public string FontIcon
        {
            get => (string)GetValue(FontIconProperty);
            set => SetValue(FontIconProperty, value);
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
        /// Gets information whether the page has a tag and type.
        /// </summary>
        public bool IsValid => !String.IsNullOrEmpty(Tag as string) && Type != null;

        /// <summary>
        /// Instance of <see cref="Type"/>.
        /// </summary>
        public Object Instance { get; set; } = null;

        private Type _pageType;

        /// <summary>
        /// <see cref="System.Windows.Controls.Page"/> type.
        /// </summary>
        public Type Type
        {
            get => _pageType ?? null;

            set
            {
                if (value.IsAssignableFrom(WindowsPage))
                {
                    throw new ArgumentException(
                        "Type of NavigationItem must be inherited from System.Windows.Controls.Page");
                }

                _pageType = value;
            }

        }
    }
}