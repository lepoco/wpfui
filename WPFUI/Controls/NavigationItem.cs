// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Windows;
using System.Windows.Media;
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
            typeof(char), typeof(NavigationItem),
            new PropertyMetadata(null));

        /// <summary>
        /// Property for <see cref="FontIconFontFamily"/>.
        /// </summary>
        public static readonly DependencyProperty FontIconFontFamilyProperty = DependencyProperty.Register(nameof(FontIconFontFamily),
            typeof(FontFamily), typeof(NavigationItem),
            new PropertyMetadata(null));

        /// <summary>
        /// Property for <see cref="Image"/>.
        /// </summary>
        public static readonly DependencyProperty ImageProperty = DependencyProperty.Register(nameof(Image),
            typeof(BitmapSource), typeof(NavigationItem),
            new PropertyMetadata(null));

        /// <summary>
        /// Routed event for <see cref="Activated"/>.
        /// </summary>
        public static readonly RoutedEvent ActivatedEvent = EventManager.RegisterRoutedEvent(
            nameof(Activated), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(NavigationItem));

        /// <summary>
        /// Routed event for <see cref="Deactivated"/>.
        /// </summary>
        public static readonly RoutedEvent DeactivatedEvent = EventManager.RegisterRoutedEvent(
            nameof(Deactivated), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(NavigationItem));

        /// <summary>
        /// Gets information whether the current element is active.
        /// </summary>
        public bool IsActive
        {
            get => (bool)GetValue(IsActiveProperty);
            set
            {
                if (value == IsActive)
                {
                    return;
                }

                if (value)
                {
                    RaiseEvent(new RoutedEventArgs(ActivatedEvent, this));
                }
                else
                {
                    RaiseEvent(new RoutedEventArgs(DeactivatedEvent, this));
                }

                SetValue(IsActiveProperty, value);
            }
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
        public char FontIcon
        {
            get => (char)GetValue(FontIconProperty);
            set => SetValue(FontIconProperty, value);
        }

        /// <summary>
        /// Gets or sets font icon font family.
        /// </summary>
        public FontFamily FontIconFontFamily
        {
            get => (FontFamily)GetValue(FontIconProperty);
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
        /// Occurs when <see cref="NavigationItem"/> is activated via <see cref="IsActive"/>.
        /// </summary>
        public event RoutedEventHandler Activated
        {
            add => AddHandler(ActivatedEvent, value);
            remove => RemoveHandler(ActivatedEvent, value);
        }

        /// <summary>
        /// Occurs when <see cref="NavigationItem"/> is deactivated via <see cref="IsActive"/>.
        /// </summary>
        public event RoutedEventHandler Deactivated
        {
            add => AddHandler(DeactivatedEvent, value);
            remove => RemoveHandler(DeactivatedEvent, value);
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