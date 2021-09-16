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

        public static readonly DependencyProperty IsAppProperty = DependencyProperty.Register("SubTitle", typeof(bool), typeof(Controls.WindowNavigation), new PropertyMetadata(false));
        public static readonly DependencyProperty ShowMaximizeProperty = DependencyProperty.Register("ShowMaximize", typeof(bool), typeof(Controls.WindowNavigation), new PropertyMetadata(true));
        public static readonly DependencyProperty ShowMinimizeProperty = DependencyProperty.Register("ShowMinimize", typeof(bool), typeof(Controls.WindowNavigation), new PropertyMetadata(true));

        public bool ApplicationNavigation
        {
            get
            {
                return (bool)(this.GetValue(IsAppProperty) as bool?);
            }
            set
            {
                this.SetValue(IsAppProperty, value);
            }
        }

        public bool ShowMaximize
        {
            get
            {
                return (bool)(this.GetValue(ShowMaximizeProperty) as bool?);
            }
            set
            {
                this.SetValue(ShowMaximizeProperty, value);
            }
        }

        public bool ShowMinimize
        {
            get
            {
                return (bool)(this.GetValue(ShowMinimizeProperty) as bool?);
            }
            set
            {
                this.SetValue(ShowMinimizeProperty, value);
            }
        }

        public WindowNavigation()
        {
            InitializeComponent();
            this.Loaded += OnLoaded;
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
                    this.ParentWindow.WindowStyle = WindowStyle.SingleBorderWindow; //Force animation
                    this.ParentWindow.WindowState = WindowState.Minimized;
                    break;
                case "maximize":
                    this.Maximize();
                    break;
                case "close":
                    if (ApplicationNavigation)
                        Application.Current.Shutdown();
                    else
                        this.ParentWindow.Close();
                    break;
            }
        }

        private void DragWindow(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.ParentWindow.DragMove();
        }

        private void DragMaximize(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
                this.Maximize();
        }

        private void Maximize()
        {
            this.ParentWindow.WindowStyle = WindowStyle.SingleBorderWindow; //Force animation

            if (this.ParentWindow.WindowState == WindowState.Normal)
            {
                this.ParentWindow.ResizeMode = ResizeMode.NoResize;
                MaximizeButton.Style = (Style)Application.Current.Resources["WUWinNavButtonRestore"];
                this.ParentWindow.WindowState = WindowState.Maximized;
            }
            else
            {
                this.ParentWindow.ResizeMode = ResizeMode.CanResize;
                MaximizeButton.Style = (Style)Application.Current.Resources["WUWinNavButtonMaximize"];
                this.ParentWindow.WindowState = WindowState.Normal;
            }
        }
    }
}
