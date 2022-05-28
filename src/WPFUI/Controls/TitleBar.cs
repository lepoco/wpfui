// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using WPFUI.Common;
using WPFUI.Controls.Interfaces;

namespace WPFUI.Controls;

/// <summary>
/// Custom navigation buttons for the window.
/// </summary>
public class TitleBar : System.Windows.Controls.Control, IThemeControl
{
    private System.Windows.Window _parent;

    internal Interop.WinDef.POINT _doubleClickPoint;

    internal SnapLayout _snapLayout;

    /// <summary>
    /// Property for <see cref="Theme"/>.
    /// </summary>
    public static readonly DependencyProperty ThemeProperty = DependencyProperty.Register(nameof(Theme),
        typeof(Appearance.ThemeType), typeof(TitleBar), new PropertyMetadata(Appearance.ThemeType.Unknown));

    /// <summary>
    /// Property for <see cref="Title"/>.
    /// </summary>
    public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(nameof(Title),
        typeof(string), typeof(TitleBar), new PropertyMetadata(null));

    /// <summary>
    /// Property for <see cref="Header"/>.
    /// </summary>
    public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(nameof(Header),
        typeof(object), typeof(TitleBar), new PropertyMetadata(null));

    /// <summary>
    /// Property for <see cref="ButtonsForeground"/>.
    /// </summary>
    public static readonly DependencyProperty ButtonsForegroundProperty = DependencyProperty.Register(
        nameof(ButtonsForeground),
        typeof(Brush), typeof(TitleBar), new FrameworkPropertyMetadata(SystemColors.ControlTextBrush,
            FrameworkPropertyMetadataOptions.Inherits));

    /// <summary>
    /// Property for <see cref="ButtonsBackground"/>.
    /// </summary>
    public static readonly DependencyProperty ButtonsBackgroundProperty = DependencyProperty.Register(
        nameof(ButtonsBackground),
        typeof(Brush), typeof(TitleBar), new FrameworkPropertyMetadata(SystemColors.ControlBrush,
            FrameworkPropertyMetadataOptions.Inherits));

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
    /// Property for <see cref="ForceShutdown"/>.
    /// </summary>
    public static readonly DependencyProperty ForceShutdownProperty =
        DependencyProperty.Register(nameof(ForceShutdown), typeof(bool), typeof(TitleBar),
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
    /// Property for <see cref="CanMaximize"/>
    /// </summary>
    public static readonly DependencyProperty CanMaximizeProperty = DependencyProperty.Register(
        nameof(CanMaximize),
        typeof(bool), typeof(TitleBar), new PropertyMetadata(true));

    /// <summary>
    /// Property for <see cref="Icon"/>.
    /// </summary>
    public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
        nameof(Icon),
        typeof(ImageSource), typeof(TitleBar), new PropertyMetadata(null));

    /// <summary>
    /// Property for <see cref="Tray"/>.
    /// </summary>
    public static readonly DependencyProperty TrayProperty = DependencyProperty.Register(
        nameof(Tray),
        typeof(NotifyIcon), typeof(TitleBar), new PropertyMetadata(null));

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

    /// <inheritdoc />
    public Appearance.ThemeType Theme
    {
        get => (Appearance.ThemeType)GetValue(ThemeProperty);
        set => SetValue(ThemeProperty, value);
    }

    /// <summary>
    /// Gets or sets title displayed on the left.
    /// </summary>
    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    /// <summary>
    /// Gets or sets the content displayed in the <see cref="TitleBar"/>.
    /// </summary>
    public object Header
    {
        get => GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }

    /// <summary>
    /// Foreground of the navigation buttons.
    /// </summary>
    [Bindable(true), Category("Appearance")]
    public Brush ButtonsForeground
    {
        get => (Brush)GetValue(ButtonsForegroundProperty);
        set => SetValue(ButtonsForegroundProperty, value);
    }

    /// <summary>
    /// Background of the navigation buttons when hovered.
    /// </summary>
    [Bindable(true), Category("Appearance")]
    public Brush ButtonsBackground
    {
        get => (Brush)GetValue(ButtonsBackgroundProperty);
        set => SetValue(ButtonsBackgroundProperty, value);
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
    public bool ForceShutdown
    {
        get => (bool)GetValue(ForceShutdownProperty);
        set => SetValue(ForceShutdownProperty, value);
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
    /// Enables or disables the maximize functionality if disables the MaximizeActionOverride action won't be called
    /// </summary>
    public bool CanMaximize
    {
        get => (bool)GetValue(CanMaximizeProperty);
        set => SetValue(CanMaximizeProperty, value);
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
    /// Tray icon.
    /// </summary>
    public NotifyIcon Tray
    {
        get => (NotifyIcon)GetValue(TrayProperty);
        set => SetValue(TrayProperty, value);
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
    /// Lets you override the behavior of the Maximize/Restore button with an <see cref="Action"/>.
    /// </summary>
    public Action<TitleBar, System.Windows.Window> MaximizeActionOverride { get; set; } = null;

    /// <summary>
    /// Lets you override the behavior of the Minimize button with an <see cref="Action"/>.
    /// </summary>
    public Action<TitleBar, System.Windows.Window> MinimizeActionOverride { get; set; } = null;

    /// <summary>
    /// Window containing the TitleBar.
    /// </summary>
    internal System.Windows.Window ParentWindow => _parent ??= System.Windows.Window.GetWindow(this);

    /// <summary>
    /// Creates a new instance of the class and sets the default <see cref="FrameworkElement.Loaded"/> event.
    /// </summary>
    public TitleBar()
    {
        SetValue(ButtonCommandProperty, new Common.RelayCommand(o => TemplateButton_OnClick(this, o)));

        Loaded += TitleBar_Loaded;
    }

    /// <inheritdoc />
    protected override void OnInitialized(EventArgs e)
    {
        base.OnInitialized(e);

        Theme = Appearance.Theme.GetAppTheme();
        Appearance.Theme.Changed += OnThemeChanged;
    }

    /// <summary>
    /// This virtual method is triggered when the app's theme changes.
    /// </summary>
    protected virtual void OnThemeChanged(Appearance.ThemeType currentTheme, Color systemAccent)
    {
#if DEBUG
        System.Diagnostics.Debug.WriteLine($"INFO | {typeof(TitleBar)} received theme -  {currentTheme}",
            "WPFUI.TitleBar");
#endif
        Theme = currentTheme;

        if (_snapLayout != null)
            _snapLayout.Theme = currentTheme;
    }

    private void CloseWindow()
    {
#if DEBUG
        System.Diagnostics.Debug.WriteLine($"INFO | {typeof(TitleBar)}.CloseWindow:ForceShutdown -  {ForceShutdown}",
            "WPFUI.TitleBar");
#endif

        if (ForceShutdown)
        {
            Application.Current.Shutdown();

            return;
        }

        ParentWindow.Close();
    }

    private void MinimizeWindow()
    {
        if (MinimizeToTray && Tray.Registered && MinimizeWindowToTray())
            return;

        if (MinimizeActionOverride != null)
        {
            MinimizeActionOverride(this, _parent);

            return;
        }

        ParentWindow.WindowState = WindowState.Minimized;
    }

    private void MaximizeWindow()
    {
        if (!CanMaximize)
            return;

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


    private bool MinimizeWindowToTray()
    {
        if (!Tray.Registered)
            return false;

        ParentWindow.WindowState = WindowState.Minimized;
        ParentWindow.Hide();

        return true;
    }

    private void InitializeSnapLayout(WPFUI.Controls.Button maximizeButton)
    {
        if (!Common.SnapLayout.IsSupported())
            return;

        _snapLayout = new Common.SnapLayout();
        _snapLayout.Theme = Theme;

        // Can be taken it from the Template, but honestly - a classic - TODO
        // ButtonsBackground, but
        _snapLayout.HoverColorLight = new SolidColorBrush(Color.FromArgb(
            (byte)0x1A,
            (byte)0x00,
            (byte)0x00,
            (byte)0x00)
        );
        _snapLayout.HoverColorDark = new SolidColorBrush(Color.FromArgb(
            (byte)0x17,
            (byte)0xFF,
            (byte)0xFF,
            (byte)0xFF)
        );
        //_snapLayout.HoverColorLight = ButtonsBackground as SolidColorBrush;
        //_snapLayout.HoverColorDark = ButtonsBackground as SolidColorBrush;

        _snapLayout.Register(maximizeButton);
    }

    private void TitleBar_Loaded(object sender, RoutedEventArgs e)
    {
        // It may look ugly, but at the moment it works surprisingly well

        var maximizeButton = (WPFUI.Controls.Button)Template.FindName("ButtonMaximize", this);

        if (maximizeButton != null && ShowMaximize && UseSnapLayout)
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
        if (e.LeftButton != MouseButtonState.Pressed || ParentWindow == null)
            return;

        // prevent firing from double clicking when the mouse never actually moved
        Interop.User32.GetCursorPos(out var currentMousePos);

        if (currentMousePos.x == _doubleClickPoint.x && currentMousePos.y == _doubleClickPoint.y)
            return;

        if (IsMaximized)
        {
            var screenPoint = PointToScreen(e.MouseDevice.GetPosition(this));
            screenPoint.X /= DpiHelper.SystemDpiXScale();
            screenPoint.Y /= DpiHelper.SystemDpiYScale();

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
        if (e.LeftButton == MouseButtonState.Pressed)
            ParentWindow.DragMove();
    }

    private void ParentWindow_StateChanged(object sender, EventArgs e)
    {
        if (ParentWindow == null)
            return;

        if (IsMaximized != (ParentWindow.WindowState == WindowState.Maximized))
            IsMaximized = ParentWindow.WindowState == WindowState.Maximized;
    }

    private void RootGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ClickCount != 2)
            return;

        Interop.User32.GetCursorPos(out _doubleClickPoint);

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
}
