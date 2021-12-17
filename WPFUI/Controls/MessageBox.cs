// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Windows;

namespace WPFUI.Controls
{
    /// <summary>
    /// Customized window for notifications.
    /// </summary>
    public class MessageBox : System.Windows.Window
    {
        /// <summary>
        /// Property for <see cref="ShowTitle"/>.
        /// </summary>
        public static readonly DependencyProperty ShowTitleProperty = DependencyProperty.Register(nameof(ShowTitle),
            typeof(bool), typeof(MessageBox), new PropertyMetadata(true));

        /// <summary>
        /// Property for <see cref="ShowFooter"/>.
        /// </summary>
        public static readonly DependencyProperty ShowFooterProperty = DependencyProperty.Register(nameof(ShowFooter),
            typeof(bool), typeof(MessageBox), new PropertyMetadata(true));

        /// <summary>
        /// Property for <see cref="MicaEnabled"/>.
        /// </summary>
        public static readonly DependencyProperty MicaEnabledProperty = DependencyProperty.Register(nameof(MicaEnabled),
            typeof(bool), typeof(MessageBox), new PropertyMetadata(true));

        /// <summary>
        /// Property for <see cref="LeftButtonName"/>.
        /// </summary>
        public static readonly DependencyProperty LeftButtonNameProperty = DependencyProperty.Register(nameof(LeftButtonName),
            typeof(string), typeof(MessageBox), new PropertyMetadata("Action"));

        /// <summary>
        /// Routed event for <see cref="LeftButtonClick"/>.
        /// </summary>
        public static readonly RoutedEvent LeftButtonClickEvent = EventManager.RegisterRoutedEvent(
            nameof(LeftButtonClick), RoutingStrategy.Bubble, typeof(MessageBox), typeof(MessageBox));

        /// <summary>
        /// Property for <see cref="RightButtonName"/>.
        /// </summary>
        public static readonly DependencyProperty RightButtonNameProperty = DependencyProperty.Register(nameof(RightButtonName),
            typeof(string), typeof(MessageBox), new PropertyMetadata("Close"));

        /// <summary>
        /// Routed event for <see cref="RightButtonClick"/>.
        /// </summary>
        public static readonly RoutedEvent RightButtonClickEvent = EventManager.RegisterRoutedEvent(
            nameof(RightButtonClick), RoutingStrategy.Bubble, typeof(MessageBox), typeof(MessageBox));

        /// <summary>
        /// Property for <see cref="TemplateButtonCommand"/>.
        /// </summary>
        public static readonly DependencyProperty TemplateButtonCommandProperty =
            DependencyProperty.Register(nameof(TemplateButtonCommand),
                typeof(Common.RelayCommand), typeof(MessageBox), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets a value that determines whether to show the <see cref="System.Windows.Window.Title"/> in <see cref="WPFUI.Controls.TitleBar"/>.
        /// </summary>
        public bool ShowTitle
        {
            get => (bool)GetValue(ShowTitleProperty);
            set => SetValue(ShowTitleProperty, value);
        }

        /// <summary>
        /// Gets or sets a value that determines whether to show the Footer.
        /// </summary>
        public bool ShowFooter
        {
            get => (bool)GetValue(ShowFooterProperty);
            set => SetValue(ShowFooterProperty, value);
        }

        /// <summary>
        /// Gets or sets a value that determines whether <see cref="MessageBox"/> should contain a <see cref="WPFUI.Background.Mica"/> effect.
        /// </summary>
        public bool MicaEnabled
        {
            get => (bool)GetValue(MicaEnabledProperty);
            set => SetValue(MicaEnabledProperty, value);
        }

        /// <summary>
        /// Name of the button on the left side of footer.
        /// </summary>
        public string LeftButtonName
        {
            get => (string)GetValue(LeftButtonNameProperty);
            set => SetValue(LeftButtonNameProperty, value);
        }

        /// <summary>
        /// Action triggered after clicking left button.
        /// </summary>
        public event RoutedEventHandler LeftButtonClick
        {
            add => AddHandler(LeftButtonClickEvent, value);
            remove => RemoveHandler(LeftButtonClickEvent, value);
        }

        /// <summary>
        /// Name of the button on the right side of footer.
        /// </summary>
        public string RightButtonName
        {
            get => (string)GetValue(RightButtonNameProperty);
            set => SetValue(RightButtonNameProperty, value);
        }

        /// <summary>
        /// Action triggered after clicking right button.
        /// </summary>
        public event RoutedEventHandler RightButtonClick
        {
            add => AddHandler(RightButtonClickEvent, value);
            remove => RemoveHandler(RightButtonClickEvent, value);
        }

        /// <summary>
        /// Command triggered after clicking the button on the Footer.
        /// </summary>
        public Common.RelayCommand TemplateButtonCommand => (Common.RelayCommand)GetValue(TemplateButtonCommandProperty);

        /// <summary>
        /// Creates new instance and sets default <see cref="FrameworkElement.Loaded"/> event.
        /// </summary>
        public MessageBox()
        {
            Owner = Application.Current.MainWindow;

            Topmost = true;

            Height = 200;
            Width = 400;

            WindowStartupLocation = WindowStartupLocation.CenterOwner;

            SetValue(TemplateButtonCommandProperty, new Common.RelayCommand(o => Button_OnClick(this, o)));
        }

        /// Shows a <see cref="System.Windows.MessageBox"/>.
        public new void Show()
        {
            if (MicaEnabled && WPFUI.Background.Mica.IsSupported() && WPFUI.Background.Mica.IsSystemThemeCompatible())
            {
                WPFUI.Background.Mica.Apply(this);
            }

            base.Show();
        }

        /// <summary>
        /// Sets <see cref="System.Windows.Window.Title"/> and <see cref="System.Windows.Window.Content"/>, then calls <see cref="Show"/>.
        /// </summary>
        /// <param name="title"><see cref="System.Windows.Window.Title"/></param>
        /// <param name="content"><see cref="System.Windows.Window.Content"/></param>
        public void Show(string title, object content)
        {
            Title = title;
            Content = content;

            Show();
        }

        // TODO: Window height match content height.

        //protected override void OnContentChanged(object oldContent, object newContent)
        //{
        //    System.Diagnostics.Debug.WriteLine("New content");
        //    System.Diagnostics.Debug.WriteLine(newContent.GetType());

        //    if (newContent != null && newContent.GetType() == typeof(System.Windows.Controls.Grid))
        //    {
        //        Height = (newContent as System.Windows.Controls.Grid).ActualHeight;
        //    }

        //    base.OnContentChanged(oldContent, newContent);
        //}

        private void Button_OnClick(object sender, object parameter)
        {
            if (parameter == null)
            {
                return;
            }

            string param = parameter as string ?? String.Empty;

#if DEBUG
            System.Diagnostics.Debug.WriteLine("MessageBox button clicked: " + param);
#endif

            switch (param)
            {
                case "left":
                    RaiseEvent(new RoutedEventArgs(LeftButtonClickEvent, this));

                    break;

                case "right":
                    RaiseEvent(new RoutedEventArgs(RightButtonClickEvent, this));

                    break;
            }
        }
    }
}
