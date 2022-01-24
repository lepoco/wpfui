// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;

namespace WPFUI.Controls
{
    /// <summary>
    /// Displays a large card with a slightly transparent background and two action buttons.
    /// </summary>
    public class Dialog : System.Windows.Controls.ContentControl
    {
        /// <summary>
        /// Property for <see cref="Show"/>.
        /// </summary>
        public static readonly DependencyProperty ShowProperty = DependencyProperty.Register("Show",
            typeof(bool), typeof(Dialog), new PropertyMetadata(false));

        /// <summary>
        /// Property for <see cref="DialogWidth"/>.
        /// </summary>
        public static readonly DependencyProperty DialogWidthProperty =
            DependencyProperty.Register("DialogWidth",
                typeof(double), typeof(Dialog), new PropertyMetadata(420.0));

        /// <summary>
        /// Property for <see cref="DialogHeight"/>.
        /// </summary>
        public static readonly DependencyProperty DialogHeightProperty =
            DependencyProperty.Register("DialogHeight",
                typeof(double), typeof(Dialog), new PropertyMetadata(200.0));

        /// <summary>
        /// Property for <see cref="ButtonLeftName"/>.
        /// </summary>
        public static readonly DependencyProperty ButtonLeftNameProperty = DependencyProperty.Register("ButtonLeftName",
            typeof(string), typeof(Dialog), new PropertyMetadata("Action"));

        /// <summary>
        /// Property for <see cref="ButtonLeftAppearance"/>.
        /// </summary>
        public static readonly DependencyProperty ButtonLeftAppearanceProperty = DependencyProperty.Register("ButtonLeftAppearance",
            typeof(Common.Appearance), typeof(Dialog),
            new PropertyMetadata(Common.Appearance.Primary));

        /// <summary>
        /// Property for <see cref="ButtonLeftCommand"/>.
        /// </summary>
        public static readonly DependencyProperty ButtonLeftCommandProperty =
            DependencyProperty.Register("ButtonLeftCommand",
                typeof(Common.IRelayCommand), typeof(Dialog), new PropertyMetadata(null));

        /// <summary>
        /// Property for <see cref="ButtonRightName"/>.
        /// </summary>
        public static readonly DependencyProperty ButtonRightNameProperty = DependencyProperty.Register("ButtonRightName",
            typeof(string), typeof(Dialog), new PropertyMetadata("Close"));

        /// <summary>
        /// Property for <see cref="ButtonRightAppearance"/>.
        /// </summary>
        public static readonly DependencyProperty ButtonRightAppearanceProperty = DependencyProperty.Register("ButtonRightAppearance",
            typeof(Common.Appearance), typeof(Dialog),
            new PropertyMetadata(Common.Appearance.Secondary));

        /// <summary>
        /// Property for <see cref="ButtonRightCommand"/>.
        /// </summary>
        public static readonly DependencyProperty ButtonRightCommandProperty =
            DependencyProperty.Register("ButtonRightCommand",
                typeof(Common.IRelayCommand), typeof(Dialog), new PropertyMetadata(null));

        /// <summary>
        /// Triggered after clicking action button.
        /// </summary>
        public event RoutedEventHandler Click;

        /// <summary>
        /// Triggered after clicking action button.
        /// </summary>
        public RoutedEventHandler ButtonRightClick
        {
            set
            {
                SetValue(ButtonRightCommandProperty, new Common.RelayCommand(o => value?.Invoke(this, new RoutedEventArgs { })));
            }
        }

        /// <summary>
        /// Gets or sets information whether the dialog should be displayed.
        /// </summary>
        public bool Show
        {
            get => (bool)GetValue(ShowProperty);
            set => SetValue(ShowProperty, value);
        }

        /// <summary>
        /// Gets or sets maximum dialog width.
        /// </summary>
        public double DialogWidth
        {
            get => (int)GetValue(DialogWidthProperty);
            set => SetValue(DialogWidthProperty, value);
        }

        /// <summary>
        /// Gets or sets dialog height.
        /// </summary>
        public double DialogHeight
        {
            get => (int)GetValue(DialogHeightProperty);
            set => SetValue(DialogHeightProperty, value);
        }

        /// <summary>
        /// Gets or sets the name of the button on the left.
        /// </summary>
        public string ButtonLeftName
        {
            get => (string)GetValue(ButtonLeftNameProperty);
            set => SetValue(ButtonLeftNameProperty, value);
        }

        /// <summary>
        /// Gets or sets the <see cref="Common.Appearance"/> of the button on the left, if available.
        /// </summary>
        public Common.Appearance ButtonLeftAppearance
        {
            get => (Common.Appearance)GetValue(ButtonLeftAppearanceProperty);
            set => SetValue(ButtonLeftAppearanceProperty, value);
        }

        /// <summary>
        /// Gets the <see cref="Common.IRelayCommand"/> triggered after clicking left action button.
        /// </summary>
        public Common.IRelayCommand ButtonLeftCommand => (Common.IRelayCommand)GetValue(ButtonLeftCommandProperty);

        /// <summary>
        /// Gets or sets the name of the button on the right.
        /// </summary>
        public string ButtonRightName
        {
            get => (string)GetValue(ButtonRightNameProperty);
            set => SetValue(ButtonRightNameProperty, value);
        }

        /// <summary>
        /// Gets or sets the <see cref="Common.Appearance"/> of the button on the right, if available.
        /// </summary>
        public Common.Appearance ButtonRightAppearance
        {
            get => (Common.Appearance)GetValue(ButtonRightAppearanceProperty);
            set => SetValue(ButtonRightAppearanceProperty, value);
        }

        /// <summary>
        /// Gets the <see cref="Common.IRelayCommand"/> triggered after clicking right action button.
        /// </summary>
        public Common.IRelayCommand ButtonRightCommand => (Common.IRelayCommand)GetValue(ButtonRightCommandProperty);

        /// <summary>
        /// Creates new instance and sets default <see cref="ButtonLeftCommand"/> and <see cref="ButtonRightCommand"/>.
        /// </summary>
        public Dialog()
        {
            SetValue(ButtonLeftCommandProperty,
                new Common.RelayCommand(o => Click?.Invoke(this, new RoutedEventArgs { })));
            SetValue(ButtonRightCommandProperty, new Common.RelayCommand(o => SetValue(ShowProperty, false)));
        }
    }
}