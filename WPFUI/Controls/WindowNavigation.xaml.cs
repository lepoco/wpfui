// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPFUI.Controls
{
    /// <summary>
    /// Interaction logic for WindowNavigation.xaml
    /// </summary>
    public partial class WindowNavigation : UserControl
    {
        public static readonly DependencyProperty IsAppProperty = DependencyProperty.Register("SubTitle", typeof(bool), typeof(Controls.WindowNavigation), new PropertyMetadata(false));

        public static readonly DependencyProperty ShowMaximizeProperty = DependencyProperty.Register("ShowMaximize", typeof(bool), typeof(Controls.WindowNavigation), new PropertyMetadata(true));

        public static readonly DependencyProperty ShowMinimizeProperty = DependencyProperty.Register("ShowMinimize", typeof(bool), typeof(Controls.WindowNavigation), new PropertyMetadata(true));


        public bool ApplicationNavigation
        {
            get => (bool)(GetValue(IsAppProperty) as bool?);
            set => SetValue(IsAppProperty, value);
        }

        public bool ShowMaximize
        {
            get => (bool)(GetValue(ShowMaximizeProperty) as bool?);
            set => SetValue(ShowMaximizeProperty, value);
        }

        public bool ShowMinimize
        {
            get => (bool)(GetValue(ShowMinimizeProperty) as bool?);
            set => SetValue(ShowMinimizeProperty, value);
        }

        private Window _parent;

        private Window ParentWindow
        {
            get
            {
                if (this._parent == null)
                    this._parent = Window.GetWindow(this);

                return this._parent;
            }
        }

        public WindowNavigation()
        {
            InitializeComponent();

            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (!ShowMaximize)
                TitleBarNavigationStack.Children.Remove(TitleBarNavigationStack.FindName("MaximizeButton") as UIElement);

            if (!ShowMinimize)
                TitleBarNavigationStack.Children.Remove(TitleBarNavigationStack.FindName("MinimizeButton") as UIElement);
        }

        private void AppBarButton(object sender, RoutedEventArgs e)
        {
            switch ((sender as System.Windows.Controls.Button).Tag.ToString())
            {
                case "minimize":
                    ParentWindow.WindowState = WindowState.Minimized;
                    break;

                case "maximize":
                    Maximize();
                    break;

                case "close":
                    if (ApplicationNavigation)
                        Application.Current.Shutdown();
                    else
                        ParentWindow.Close();
                    break;
            }
        }

        private void DragWindow(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left)
                return;

            //if (e.ClickCount == 2)
            //    return;

            //if (ParentWindow.WindowState == WindowState.Maximized)
            //{
            //    MaximizeButton.Style = (Style)Application.Current.Resources["WUWinNavButtonMaximize"];
            //    ParentWindow.WindowState = WindowState.Normal;
            //}

            ParentWindow.DragMove();
        }

        private void DragMaximize(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                Maximize();
            }
        }

        private void Maximize()
        {
            if (ParentWindow.WindowState == WindowState.Normal)
            {
                MaximizeButton.Style = (Style)Application.Current.Resources["UiTitlebarButtonRestore"];
                ParentWindow.WindowState = WindowState.Maximized;
            }
            else
            {
                MaximizeButton.Style = (Style)Application.Current.Resources["UiTitlebarButtonMaximize"];
                ParentWindow.WindowState = WindowState.Normal;
            }
        }
    }
}
