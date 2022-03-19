// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using WPFUI.Win32;

namespace WPFUI.Controls
{
    /// <summary>
    /// Custom navigation buttons for the window.
    /// </summary>
    public class TitleBar : UserControl
    {
        private Window _parent;

        private User32.POINT _doubleClickPoint;

        private Tray.NotifyIcon _notifyIcon;

        private Common.SnapLayout _snapLayout;

        /// <summary>
        /// Property for <see cref="Title"/>.
        /// </summary>
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(nameof(Title),
            typeof(string), typeof(TitleBar), new PropertyMetadata(null));

        /// <summary>
        /// Property for <see cref="MinimizeToTray"/>.
        /// </summary>
        public static readonly DependencyProperty MinimizeToTrayProperty = DependencyProperty.Register(
            nameof(MinimizeToTray),
            typeof(bool), typeof(TitleBar), new PropertyMetadata(false));

        /// <summary>
        /// Property for <see cref="UseSnapLayout"/>.
        /// </summary>
        public static readonly DependencyProperty UseSnapLayoutProperty = DependencyProperty.Register(
            nameof(UseSnapLayout),
            typeof(bool), typeof(TitleBar), new PropertyMetadata(false));

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
        public static readonly DependencyProperty ShowMaximizeProperty = DependencyProperty.Register(
            nameof(ShowMaximize),
            typeof(bool), typeof(TitleBar), new PropertyMetadata(true));

        /// <summary>
        /// Property for <see cref="ShowMinimize"/>.
        /// </summary>
        public static readonly DependencyProperty ShowMinimizeProperty = DependencyProperty.Register(
            nameof(ShowMinimize),
            typeof(bool), typeof(TitleBar), new PropertyMetadata(true));

        /// <summary>
        /// Property for <see cref="ShowHelp"/>
        /// </summary>
        public static readonly DependencyProperty ShowHelpProperty = DependencyProperty.Register(
            nameof(ShowHelp),
            typeof(bool), typeof(TitleBar), new PropertyMetadata(false));

        /// <summary>
        /// Property for <see cref="Icon"/>.
        /// </summary>
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
            nameof(Icon),
            typeof(ImageSource), typeof(TitleBar), new PropertyMetadata(null));

        /// <summary>
        /// Property for <see cref="NotifyIconTooltip"/>.
        /// </summary>
        public static readonly DependencyProperty NotifyIconTooltipProperty = DependencyProperty.Register(
            nameof(NotifyIconTooltip),
            typeof(string), typeof(TitleBar), new PropertyMetadata(String.Empty, NotifyIconTooltip_OnChanged));

        /// <summary>
        /// Property for <see cref="NotifyIconImage"/>.
        /// </summary>
        public static readonly DependencyProperty NotifyIconImageProperty = DependencyProperty.Register(
            nameof(NotifyIconImage),
            typeof(ImageSource), typeof(TitleBar), new PropertyMetadata(null));

        /// <summary>
        /// Property for <see cref="UseNotifyIcon"/>.
        /// </summary>
        public static readonly DependencyProperty UseNotifyIconProperty = DependencyProperty.Register(
            nameof(UseNotifyIcon),
            typeof(bool), typeof(TitleBar), new PropertyMetadata(false, UseNotifyIcon_OnChanged));

        /// <summary>
        /// Property for <see cref="NotifyIconMenu"/>.
        /// </summary>
        public static readonly DependencyProperty NotifyIconMenuProperty = DependencyProperty.Register(
            nameof(NotifyIconMenu),
            typeof(ContextMenu), typeof(TitleBar), new PropertyMetadata(null, NotifyIconMenu_OnChanged));

        /// <summary>
        /// Routed event for <see cref="NotifyIconClick"/>.
        /// </summary>
        public static readonly RoutedEvent NotifyIconClickEvent = EventManager.RegisterRoutedEvent(
            nameof(NotifyIconClick), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TitleBar));

        /// <summary>
        /// Routed event for <see cref="NotifyIconDoubleClick"/>.
        /// </summary>
        public static readonly RoutedEvent NotifyIconDoubleClickEvent = EventManager.RegisterRoutedEvent(
            nameof(NotifyIconDoubleClick), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TitleBar));

        /// <summary>
        /// Routed event for <see cref="CloseClicked"/>.
        /// </summary>
        public static readonly RoutedEvent CloseClickedEvent = EventManager.RegisterRoutedEvent(
            nameof(CloseClicked), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TitleBar));

        /// <summary>
        /// Routed event for <see cref="MaximizeClicked"/>.
        /// </summary>
        public static readonly RoutedEvent MaximizeClickedEvent = EventManager.RegisterRoutedEvent(
            nameof(MaximizeClicked), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TitleBar));

        /// <summary>
        /// Routed event for <see cref="MinimizeClicked"/>.
        /// </summary>
        public static readonly RoutedEvent MinimizeClickedEvent = EventManager.RegisterRoutedEvent(
            nameof(MinimizeClicked), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TitleBar));

        /// <summary>
        /// Routed event for <see cref="HelpClicked"/>.
        /// </summary>
        public static readonly RoutedEvent HelpClickedEvent = EventManager.RegisterRoutedEvent(
            nameof(HelpClicked), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TitleBar));

        /// <summary>
        /// Property for <see cref="ButtonCommand"/>.
        /// </summary>
        public static readonly DependencyProperty ButtonCommandProperty =
            DependencyProperty.Register(nameof(ButtonCommand),
                typeof(Common.IRelayCommand), typeof(TitleBar), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets title displayed on the left.
        /// </summary>
        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        /// <summary>
        /// Gets or sets information whether to minimize the application to tray.
        /// </summary>
        public bool MinimizeToTray
        {
            get => (bool)GetValue(MinimizeToTrayProperty);
            set => SetValue(MinimizeToTrayProperty, value);
        }

        /// <summary>
        /// Gets or sets information whether the use Windows 11 Snap Layout.
        /// </summary>
        public bool UseSnapLayout
        {
            get => (bool)GetValue(UseSnapLayoutProperty);
            set => SetValue(UseSnapLayoutProperty, value);
        }

        /// <summary>
        /// Gets or sets information whether the current window is maximized.
        /// </summary>
        public bool IsMaximized
        {
            get => (bool)GetValue(IsMaximizedProperty);
            internal set => SetValue(IsMaximizedProperty, value);
        }

        /// <summary>
        /// Gets or sets information whether the controls affect main application window.
        /// </summary>
        public bool ApplicationNavigation
        {
            get => (bool)GetValue(ApplicationNavigationProperty);
            set => SetValue(ApplicationNavigationProperty, value);
        }

        /// <summary>
        /// Gets or sets information whether to show maximize button.
        /// </summary>
        public bool ShowMaximize
        {
            get => (bool)GetValue(ShowMaximizeProperty);
            set => SetValue(ShowMaximizeProperty, value);
        }

        /// <summary>
        /// Gets or sets information whether to show minimize button.
        /// </summary>
        public bool ShowMinimize
        {
            get => (bool)GetValue(ShowMinimizeProperty);
            set => SetValue(ShowMinimizeProperty, value);
        }

        /// <summary>
        /// Gets or sets information whether to show help button
        /// </summary>
        public bool ShowHelp
        {
            get => (bool)GetValue(ShowHelpProperty);
            set => SetValue(ShowHelpProperty, value);
        }

        /// <summary>
        /// Titlebar icon.
        /// </summary>
        public ImageSource Icon
        {
            get => (ImageSource)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        /// <summary>
        /// Gets or sets text displayed when hover NotifyIcon in system tray.
        /// </summary>
        public string NotifyIconTooltip
        {
            get => (string)GetValue(NotifyIconTooltipProperty);
            set => SetValue(NotifyIconTooltipProperty, value);
        }

        /// <summary>
        /// BitmapSource of tray icon.
        /// </summary>
        public ImageSource NotifyIconImage
        {
            get => (ImageSource)GetValue(NotifyIconImageProperty);
            set => SetValue(NotifyIconImageProperty, value);
        }

        /// <summary>
        /// Gets or sets information whether to use shell icon with menu in system tray.
        /// </summary>
        public bool UseNotifyIcon
        {
            get => (bool)GetValue(UseNotifyIconProperty);
            set => SetValue(UseNotifyIconProperty, value);
        }

        /// <summary>
        /// Menu displayed when left click on NotifyIcon.
        /// </summary>
        public ContextMenu NotifyIconMenu
        {
            get => (ContextMenu)GetValue(NotifyIconMenuProperty);
            set => SetValue(NotifyIconMenuProperty, value);
        }

        /// <summary>
        /// Event triggered after clicking the left mouse button on the tray icon.
        /// </summary>
        public event RoutedEventHandler NotifyIconClick
        {
            add => AddHandler(NotifyIconClickEvent, value);
            remove => RemoveHandler(NotifyIconClickEvent, value);
        }

        /// <summary>
        /// Event triggered after double-clicking the left mouse button on the tray icon.
        /// </summary>
        public event RoutedEventHandler NotifyIconDoubleClick
        {
            add => AddHandler(NotifyIconDoubleClickEvent, value);
            remove => RemoveHandler(NotifyIconDoubleClickEvent, value);
        }

        /// <summary>
        /// Event triggered after clicking close button.
        /// </summary>
        public event RoutedEventHandler CloseClicked
        {
            add => AddHandler(CloseClickedEvent, value);
            remove => RemoveHandler(CloseClickedEvent, value);
        }

        /// <summary>
        /// Event triggered after clicking maximize or restore button.
        /// </summary>
        public event RoutedEventHandler MaximizeClicked
        {
            add => AddHandler(MaximizeClickedEvent, value);
            remove => RemoveHandler(MaximizeClickedEvent, value);
        }

        /// <summary>
        /// Event triggered after clicking minimize button.
        /// </summary>
        public event RoutedEventHandler MinimizeClicked
        {
            add => AddHandler(MinimizeClickedEvent, value);
            remove => RemoveHandler(MinimizeClickedEvent, value);
        }

        /// <summary>
        /// Event triggered after clicking help button
        /// </summary>
        public event RoutedEventHandler HelpClicked
        {
            add => AddHandler(HelpClickedEvent, value);
            remove => RemoveHandler(HelpClickedEvent, value);
        }

        /// <summary>
        /// Command triggered after clicking the titlebar button.
        /// </summary>
        public Common.IRelayCommand ButtonCommand => (Common.IRelayCommand)GetValue(ButtonCommandProperty);

        /// <summary>
        /// Lets you override the behavior of the Close button with an <see cref="Action"/>.
        /// </summary>
        public Action<TitleBar, Window> CloseActionOverride { get; set; } = null;

        /// <summary>
        /// Lets you override the behavior of the Maximize/Restore button with an <see cref="Action"/>.
        /// </summary>
        public Action<TitleBar, Window> MaximizeActionOverride { get; set; } = null;

        /// <summary>
        /// Lets you override the behavior of the Minimize button with an <see cref="Action"/>.
        /// </summary>
        public Action<TitleBar, Window> MinimizeActionOverride { get; set; } = null;

        private Window ParentWindow => _parent ??= Window.GetWindow(this);

        /// <summary>
        /// Creates a new instance of the class and sets the default <see cref="FrameworkElement.Loaded"/> event.
        /// </summary>
        public TitleBar()
        {
            SetValue(ButtonCommandProperty, new Common.RelayCommand(o => TemplateButton_OnClick(this, o)));

            Loaded += TitleBar_Loaded;
        }

        /// <summary>
        /// Resets icon.
        /// </summary>
        public void ResetIcon()
        {
            if (_notifyIcon != null)
                _notifyIcon.Destroy();

            InitializeNotifyIcon();
        }

        private void CloseWindow()
        {
            if (CloseActionOverride != null)
            {
                CloseActionOverride(this, _parent);

                return;
            }

            if (ApplicationNavigation)
                Application.Current.Shutdown();
            else
                ParentWindow.Close();
        }

        private void MinimizeWindow()
        {
            if (MinimizeToTray && UseNotifyIcon && MinimizeWindowToTray()) return;

            if (MinimizeActionOverride != null)
            {
                MinimizeActionOverride(this, _parent);

                return;
            }

            ParentWindow.WindowState = WindowState.Minimized;
        }

        private void MaximizeWindow()
        {
            if (MaximizeActionOverride != null)
            {
                MaximizeActionOverride(this, _parent);

                return;
            }

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

        private void InitializeNotifyIcon()
        {
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject())) return;

            NotifyIconClick += OnNotifyIconClick;

            _notifyIcon = new()
            {
                Parent = this,
                Tooltip = NotifyIconTooltip,
                ContextMenu = NotifyIconMenu,
                Icon = NotifyIconImage,
                Click = icon => { RaiseEvent(new RoutedEventArgs(NotifyIconClickEvent, this)); },
                DoubleClick = icon => { RaiseEvent(new RoutedEventArgs(NotifyIconDoubleClickEvent, this)); }
            };

            _notifyIcon.Show();
        }

        private bool MinimizeWindowToTray()
        {
            if (_notifyIcon == null)
                return false;

            ParentWindow.WindowState = WindowState.Minimized;
            ParentWindow.Hide();

            return true;
        }

        private void OnNotifyIconClick(object sender, RoutedEventArgs e)
        {
            if (!MinimizeToTray) return;

            if (ParentWindow.WindowState != WindowState.Minimized) return;

            ParentWindow.Show();
            ParentWindow.WindowState = WindowState.Normal;

            ParentWindow.Topmost = true;
            ParentWindow.Topmost = false;

            Focus();
        }

        private void InitializeSnapLayout(WPFUI.Controls.Button maximizeButton)
        {
            if (!Common.SnapLayout.IsSupported()) return;

            _snapLayout = new Common.SnapLayout();
            _snapLayout.Register(maximizeButton);
        }

        private void TitleBar_Loaded(object sender, RoutedEventArgs e)
        {
            if (UseNotifyIcon)
                InitializeNotifyIcon();

            // It may look ugly, but at the moment it works surprisingly well

            var maximizeButton = (WPFUI.Controls.Button)Template.FindName("ButtonMaximize", this);

            if (maximizeButton != null && UseSnapLayout)
                InitializeSnapLayout(maximizeButton);

            var rootGrid = (System.Windows.Controls.Grid)Template.FindName("RootGrid", this);

            if (rootGrid != null)
            {
                rootGrid.MouseLeftButtonDown += RootGrid_MouseLeftButtonDown;
                rootGrid.MouseMove += RootGrid_MouseMove;
            }

            if (ParentWindow != null)
                ParentWindow.StateChanged += ParentWindow_StateChanged;
        }

        private void RootGrid_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed || ParentWindow == null) return;

            // prevent firing from double clicking when the mouse never actually moved
            User32.GetCursorPos(out var currentMousePos);

            if (currentMousePos.X == _doubleClickPoint.X && currentMousePos.Y == _doubleClickPoint.Y) return;

            if (IsMaximized)
            {
                var screenPoint = PointToScreen(e.MouseDevice.GetPosition(this));
                screenPoint.X /= Common.Dpi.SystemDpiXScale();
                screenPoint.Y /= Common.Dpi.SystemDpiYScale();

                // TODO: refine the Left value to be more accurate
                // - This calculation is good enough using the center
                //   of the titlebar, however this isn't quite accurate for
                //   how the OS operates.
                // - It should be set as a % (e.g. screen X / maximized width),
                //   then offset from the left to line up more naturally.
                ParentWindow.Left = screenPoint.X - (ParentWindow.RestoreBounds.Width * 0.5);
                ParentWindow.Top = screenPoint.Y;

                // style has to be quickly swapped to avoid restore animation delay
                var style = ParentWindow.WindowStyle;
                ParentWindow.WindowStyle = WindowStyle.None;
                ParentWindow.WindowState = WindowState.Normal;
                ParentWindow.WindowStyle = style;
            }

            // Call drag move only when mouse down, check again
            // if()
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                ParentWindow.DragMove();
            }
        }

        private void ParentWindow_StateChanged(object sender, EventArgs e)
        {
            if (ParentWindow == null) return;

            if (IsMaximized != (ParentWindow.WindowState == WindowState.Maximized))
                IsMaximized = ParentWindow.WindowState == WindowState.Maximized;
        }

        private void RootGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount != 2) return;

            User32.GetCursorPos(out _doubleClickPoint);

            MaximizeWindow();
        }

        private void TemplateButton_OnClick(TitleBar sender, object parameter)
        {
            string command = parameter as string;

            switch (command)
            {
                case "close":
                    RaiseEvent(new RoutedEventArgs(CloseClickedEvent, this));
                    CloseWindow();
                    break;

                case "minimize":
                    RaiseEvent(new RoutedEventArgs(MinimizeClickedEvent, this));
                    MinimizeWindow();
                    break;

                case "maximize":
                    RaiseEvent(new RoutedEventArgs(MaximizeClickedEvent, this));
                    MaximizeWindow();
                    break;
                case "help":
                    RaiseEvent(new RoutedEventArgs(HelpClickedEvent, this));
                    break;
            }
        }

        private static void NotifyIconTooltip_OnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not TitleBar { UseNotifyIcon: true } titleBar) return;

            titleBar.ResetIcon();
        }

        private static void UseNotifyIcon_OnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not TitleBar titleBar) return;

            if (titleBar.UseNotifyIcon)
                titleBar.ResetIcon();
            else
                titleBar._notifyIcon.Destroy();
        }

        private static void NotifyIconMenu_OnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //if (d is not TitleBar titleBar) return;

            //if (titleBar.UseNotifyIcon == false)
            //{
            //    return;
            //}

            //titleBar.ResetIcon();
        }
    }
}