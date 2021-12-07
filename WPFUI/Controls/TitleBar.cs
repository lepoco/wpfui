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
    /// Custom navigation buttons for the window.
    /// </summary>
    public class TitleBar : UserControl
    {
        // TODO: Icon
        // TODO: Title

        Common.SnapLayout _snapLayout;

        /// <summary>
        /// Property for <see cref="IsMaximized"/>.
        /// </summary>
        public static readonly DependencyProperty IsMaximizedProperty = DependencyProperty.Register(nameof(IsMaximized),
            typeof(bool), typeof(TitleBar), new PropertyMetadata(false));

        /// <summary>
        /// Property for <see cref="ApplicationNavigation"/>.
        /// </summary>
        public static readonly DependencyProperty ApplicationNavigationProperty =
            DependencyProperty.Register(nameof(ApplicationNavigation), typeof(bool), typeof(TitleBar),
                new PropertyMetadata(false));

        /// <summary>
        /// Property for <see cref="ShowMaximize"/>.
        /// </summary>
        public static readonly DependencyProperty ShowMaximizeProperty = DependencyProperty.Register(nameof(ShowMaximize),
            typeof(bool), typeof(TitleBar), new PropertyMetadata(true));

        /// <summary>
        /// Property for <see cref="ShowMinimize"/>.
        /// </summary>
        public static readonly DependencyProperty ShowMinimizeProperty = DependencyProperty.Register(nameof(ShowMinimize),
            typeof(bool), typeof(TitleBar), new PropertyMetadata(true));

        /// <summary>
        /// Property for <see cref="ButtonCommand"/>.
        /// </summary>
        public static readonly DependencyProperty ButtonCommandProperty =
            DependencyProperty.Register(nameof(ButtonCommand),
                typeof(Common.RelayCommand), typeof(TitleBar), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets information whether the current window is maximized.
        /// </summary>
        public bool IsMaximized
        {
            get => (bool)((bool?)GetValue(IsMaximizedProperty));
            set => SetValue(IsMaximizedProperty, value);
        }

        /// <summary>
        /// Gets or sets information whether the controls affect main application window.
        /// </summary>
        public bool ApplicationNavigation
        {
            get => (bool)((bool?)GetValue(ApplicationNavigationProperty));
            set => SetValue(ApplicationNavigationProperty, value);
        }

        /// <summary>
        /// Gets or sets information whether to show maximize button.
        /// </summary>
        public bool ShowMaximize
        {
            get => (bool)((bool?)GetValue(ShowMaximizeProperty));
            set => SetValue(ShowMaximizeProperty, value);
        }

        /// <summary>
        /// Gets or sets information whether to show minimize button.
        /// </summary>
        public bool ShowMinimize
        {
            get => (bool)((bool?)GetValue(ShowMinimizeProperty));
            set => SetValue(ShowMinimizeProperty, value);
        }

        /// <summary>
        /// Command triggered after clicking the titlebar button.
        /// </summary>
        public Common.RelayCommand ButtonCommand => (Common.RelayCommand)GetValue(ButtonCommandProperty);

        private Window _parent;

        private Window ParentWindow
        {
            get
            {
                if (_parent == null)
                    _parent = Window.GetWindow(this);

                return _parent;
            }
        }

        /// <summary>
        /// Creates a new instance of the class and sets the default <see cref="Common.RelayCommand"/> of <see cref="BorderCommand"/>.
        /// </summary>
        public TitleBar()
        {
            SetValue(ButtonCommandProperty, new Common.RelayCommand(o => ButtonOnClick(this, o)));

            Loaded += TitleBar_Loaded;
        }

        private void ButtonOnClick(TitleBar sender, object parameter)
        {
            string command = parameter as string;

            switch (command)
            {
                case "minimize":
                    MinimizeWindow();
                    break;

                case "maximize":
                    MaximizeWindow();
                    break;

                case "close":
                    CloseWindow();
                    break;
            }
        }

        private void MinimizeWindow()
        {
            ParentWindow.WindowState = WindowState.Minimized;
        }

        private void MaximizeWindow()
        {
            if (ParentWindow.WindowState == WindowState.Normal)
            {
                IsMaximized = true;
                ParentWindow.WindowState = WindowState.Maximized;
            }
            else
            {
                IsMaximized = false;
                ParentWindow.WindowState = WindowState.Normal;
            }
        }

        private void CloseWindow()
        {
            if (ApplicationNavigation)
                Application.Current.Shutdown();
            else
                ParentWindow.Close();
        }

        private void TitleBar_Loaded(object sender, RoutedEventArgs e)
        {
            // It may look ugly, but at the moment it works surprisingly well

            var rootGrid = (System.Windows.Controls.Grid)Template.FindName("RootGrid", this);

            if (rootGrid != null)
            {
                rootGrid.MouseDown += RootGrid_MouseDown;
                rootGrid.MouseLeftButtonDown += RootGrid_MouseLeftButtonDown;
            }

            var maximizeButton = (System.Windows.Controls.Button)Template.FindName("ButtonMaximize", this);

            //if (maximizeButton != null && SnapLayout.IsSupported())
            //{
            //    _snapLayout = new SnapLayout();
            //    _snapLayout.Register(ParentWindow, maximizeButton);
            //}
        }

        private void RootGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left)
                return;

            // TODO: Restore on maximize and move when single click and hold

            //if (e.ClickCount == 1 && e.ButtonState == MouseButtonState.Pressed && ParentWindow.WindowState == WindowState.Maximized)
            //{
            //    MaximizeWindow();
            //    ParentWindow.DragMove();
            //}
            //else
            //{
            //    ParentWindow.DragMove();
            //}

            ParentWindow.DragMove();
        }

        private void RootGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                MaximizeWindow();
            }
        }
    }
}