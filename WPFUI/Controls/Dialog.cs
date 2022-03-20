// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Windows;
using WPFUI.Common;

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
        public static readonly DependencyProperty ShowProperty = DependencyProperty.Register(nameof(Show),
            typeof(bool), typeof(Dialog), new PropertyMetadata(false, ShowPropertyChangedCallback));

        /// <summary>
        /// Property for <see cref="DialogWidth"/>.
        /// </summary>
        public static readonly DependencyProperty DialogWidthProperty =
            DependencyProperty.Register(nameof(DialogWidth),
                typeof(double), typeof(Dialog), new PropertyMetadata(420.0));

        /// <summary>
        /// Property for <see cref="DialogHeight"/>.
        /// </summary>
        public static readonly DependencyProperty DialogHeightProperty =
            DependencyProperty.Register(nameof(DialogHeight),
                typeof(double), typeof(Dialog), new PropertyMetadata(200.0));

        /// <summary>
        /// Property for <see cref="ButtonLeftName"/>.
        /// </summary>
        public static readonly DependencyProperty ButtonLeftNameProperty = DependencyProperty.Register(nameof(ButtonLeftName),
            typeof(string), typeof(Dialog), new PropertyMetadata("Action"));

        /// <summary>
        /// Routed event for <see cref="ButtonLeftClick"/>.
        /// </summary>
        public static readonly RoutedEvent ButtonLeftClickEvent = EventManager.RegisterRoutedEvent(
            nameof(ButtonLeftClick), RoutingStrategy.Bubble, typeof(Dialog), typeof(Dialog));

        /// <summary>
        /// Property for <see cref="ButtonRightName"/>.
        /// </summary>
        public static readonly DependencyProperty ButtonRightNameProperty = DependencyProperty.Register(nameof(ButtonRightName),
            typeof(string), typeof(Dialog), new PropertyMetadata("Close"));

        /// <summary>
        /// Property for <see cref="ButtonLeftAppearance"/>.
        /// </summary>
        public static readonly DependencyProperty ButtonLeftAppearanceProperty = DependencyProperty.Register(nameof(ButtonLeftAppearance),
            typeof(Common.Appearance), typeof(Dialog),
            new PropertyMetadata(Common.Appearance.Primary));

        /// <summary>
        /// Property for <see cref="ButtonLeftVisibility"/>.
        /// </summary>
        public static readonly DependencyProperty ButtonLeftVisibilityProperty = DependencyProperty.Register(
            nameof(ButtonLeftVisibility),
            typeof(System.Windows.Visibility), typeof(Dialog),
            new PropertyMetadata(System.Windows.Visibility.Visible));

        /// <summary>
        /// Routed event for <see cref="ButtonRightClick"/>.
        /// </summary>
        public static readonly RoutedEvent ButtonRightClickEvent = EventManager.RegisterRoutedEvent(
            nameof(ButtonRightClick), RoutingStrategy.Bubble, typeof(Dialog), typeof(Dialog));


        /// <summary>
        /// Property for <see cref="ButtonRightAppearance"/>.
        /// </summary>
        public static readonly DependencyProperty ButtonRightAppearanceProperty = DependencyProperty.Register(nameof(ButtonRightAppearance),
            typeof(Common.Appearance), typeof(Dialog),
            new PropertyMetadata(Common.Appearance.Secondary));

        /// <summary>
        /// Property for <see cref="ButtonRightVisibility"/>.
        /// </summary>
        public static readonly DependencyProperty ButtonRightVisibilityProperty = DependencyProperty.Register(
            nameof(ButtonRightVisibility),
            typeof(System.Windows.Visibility), typeof(Dialog),
            new PropertyMetadata(System.Windows.Visibility.Visible));


        /// <summary>
        /// Property for <see cref="TemplateButtonCommand"/>.
        /// </summary>
        public static readonly DependencyProperty TemplateButtonCommandProperty =
            DependencyProperty.Register(nameof(TemplateButtonCommand),
                typeof(Common.IRelayCommand), typeof(Dialog), new PropertyMetadata(null));

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
        /// Name of the button on the left side of footer.
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
        /// Gets or sets the visibility of the button on the left.
        /// </summary>
        public System.Windows.Visibility ButtonLeftVisibility
        {
            get => (System.Windows.Visibility)GetValue(ButtonLeftVisibilityProperty);
            set => SetValue(ButtonLeftVisibilityProperty, value);
        }

        /// <summary>
        /// Action triggered after clicking left button.
        /// </summary>
        public event RoutedEventHandler ButtonLeftClick
        {
            add => AddHandler(ButtonLeftClickEvent, value);
            remove => RemoveHandler(ButtonLeftClickEvent, value);
        }

        /// <summary>
        /// Name of the button on the right side of footer.
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
        /// Gets or sets the visibility of the button on the right.
        /// </summary>
        public System.Windows.Visibility ButtonRightVisibility
        {
            get => (System.Windows.Visibility)GetValue(ButtonRightVisibilityProperty);
            set => SetValue(ButtonRightVisibilityProperty, value);
        }

        /// <summary>
        /// Action triggered after clicking right button.
        /// </summary>
        public event RoutedEventHandler ButtonRightClick
        {
            add => AddHandler(ButtonRightClickEvent, value);
            remove => RemoveHandler(ButtonRightClickEvent, value);
        }

        /// <summary>
        /// Command triggered after clicking the button on the Footer.
        /// </summary>
        public Common.IRelayCommand TemplateButtonCommand => (Common.IRelayCommand)GetValue(TemplateButtonCommandProperty);

        /// <summary>
        /// Event triggered when <see cref="Dialog"/> opens.
        /// </summary>
        public event DialogEvent Opened;

        /// <summary>
        /// Event triggered when <see cref="Dialog"/> gets closed.
        /// </summary>
        public event DialogEvent Closed;

        /// <summary>
        /// Creates new instance and sets default <see cref="TemplateButtonCommandProperty"/>.
        /// </summary>
        public Dialog() =>
            SetValue(TemplateButtonCommandProperty, new Common.RelayCommand(o => Button_OnClick(this, o)));

        private static void ShowPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not Dialog control) return;

            if (control.Show)
                control.Opened?.Invoke(control);
            else
                control.Closed?.Invoke(control);
        }

        private void Button_OnClick(object sender, object parameter)
        {
            if (parameter == null) return;

            string param = parameter as string ?? String.Empty;

#if DEBUG
            System.Diagnostics.Debug.WriteLine($"INFO: {typeof(Dialog)} button clicked with param: {param}", "WPFUI.Dialog");
#endif

            switch (param)
            {
                case "left":
                    RaiseEvent(new RoutedEventArgs(ButtonLeftClickEvent, this));

                    break;

                case "right":
                    RaiseEvent(new RoutedEventArgs(ButtonRightClickEvent, this));

                    break;
            }
        }
    }
}