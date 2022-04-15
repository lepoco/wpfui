// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using WPFUI.Controls.Interfaces;

namespace WPFUI.Controls
{
    /// <summary>
    /// Navigation element.
    /// </summary>
    public class NavigationItem : System.Windows.Controls.Primitives.ButtonBase, INavigationItem, IIconControl
    {
        private static readonly Type WindowsPage = typeof(System.Windows.Controls.Page);

        private Type _pageType = null;

        /// <summary>
        /// Property for <see cref="IsActive"/>.
        /// </summary>
        public static readonly DependencyProperty IsActiveProperty = DependencyProperty.Register(nameof(IsActive),
            typeof(bool), typeof(NavigationItem), new PropertyMetadata(false));

        /// <summary>
        /// Property for <see cref="Icon"/>.
        /// </summary>
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(nameof(Icon),
            typeof(Common.SymbolRegular), typeof(NavigationItem),
            new PropertyMetadata(Common.SymbolRegular.Empty));

        /// <summary>
        /// Property for <see cref="IconSize"/>.
        /// </summary>
        public static readonly DependencyProperty IconSizeProperty = DependencyProperty.Register(nameof(IconSize),
            typeof(double), typeof(NavigationItem),
            new PropertyMetadata(18d));

        /// <summary>
        /// Property for <see cref="IconFilled"/>.
        /// </summary>
        public static readonly DependencyProperty IconFilledProperty = DependencyProperty.Register(nameof(IconFilled),
            typeof(bool), typeof(NavigationItem), new PropertyMetadata(false));

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

        /// <inheritdoc />
        public bool IsActive
        {
            get => (bool)GetValue(IsActiveProperty);
            set
            {
                if (value == IsActive) return;

                RaiseEvent(value
                    ? new RoutedEventArgs(ActivatedEvent, this)
                    : new RoutedEventArgs(DeactivatedEvent, this));

                SetValue(IsActiveProperty, value);
            }
        }

        /// <inheritdoc />
        [Bindable(true), Category("Appearance")]
        [Localizability(LocalizationCategory.None)]
        public Common.SymbolRegular Icon
        {
            get => (Common.SymbolRegular)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        /// <inheritdoc />
        [Bindable(true), Category("Appearance")]
        [Localizability(LocalizationCategory.None)]
        public bool IconFilled
        {
            get => (bool)GetValue(IconFilledProperty);
            set => SetValue(IconFilledProperty, value);
        }

        /// <summary>
        /// 
        /// </summary>
        [TypeConverter(typeof(FontSizeConverter))]
        [Bindable(true), Category("Appearance")]
        [Localizability(LocalizationCategory.None)]
        public double IconSize
        {
            get => (double)GetValue(IconSizeProperty);
            set => SetValue(IconSizeProperty, value);
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

        /// <inheritdoc/>
        public bool IsValid => !String.IsNullOrEmpty(Tag as string) && Page != null;

        /// <inheritdoc/>
        public Page Instance { get; set; } = null;

        /// <inheritdoc/>
        public Type Page
        {
            get => _pageType;

            set
            {
                if (value.IsAssignableFrom(WindowsPage))
                    throw new ArgumentException(
                        "Page of NavigationItem must be inherited from System.Windows.Controls.Page");

                _pageType = value;
            }

        }
    }
}