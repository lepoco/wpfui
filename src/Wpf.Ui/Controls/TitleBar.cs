// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using Wpf.Ui.Common;
using Wpf.Ui.Dpi;
using Wpf.Ui.Extensions;
using Wpf.Ui.Interop;

namespace Wpf.Ui.Controls;

/// <summary>
/// Custom navigation buttons for the window.
/// </summary>
[TemplatePart(Name = ElementMainGrid, Type = typeof(System.Windows.Controls.Grid))]
[TemplatePart(Name = ElementIcon, Type = typeof(System.Windows.Controls.Image))]
[TemplatePart(Name = ElementHelpButton, Type = typeof(TitleBarButton))]
[TemplatePart(Name = ElementMinimizeButton, Type = typeof(TitleBarButton))]
[TemplatePart(Name = ElementMaximizeButton, Type = typeof(TitleBarButton))]
[TemplatePart(Name = ElementRestoreButton, Type = typeof(TitleBarButton))]
[TemplatePart(Name = ElementCloseButton, Type = typeof(TitleBarButton))]
public class TitleBar : System.Windows.Controls.Control, IThemeControl
{
    private const string ElementIcon = "PART_Icon";
    private const string ElementMainGrid = "PART_MainGrid";
    private const string ElementHelpButton = "PART_HelpButton";
    private const string ElementMinimizeButton = "PART_MinimizeButton";
    private const string ElementMaximizeButton = "PART_MaximizeButton";
    private const string ElementRestoreButton = "PART_RestoreButton";
    private const string ElementCloseButton = "PART_CloseButton";

    #region Static properties

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
    /// Property for <see cref="ShowClose"/>.
    /// </summary>
    public static readonly DependencyProperty ShowCloseProperty = DependencyProperty.Register(
        nameof(ShowClose),
        typeof(bool), typeof(TitleBar), new PropertyMetadata(true));

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
    /// Property for <see cref="CloseWindowByDoubleClickOnIcon"/>.
    /// </summary>
    public static readonly DependencyProperty CloseWindowByDoubleClickOnIconProperty = DependencyProperty.Register(
        nameof(CloseWindowByDoubleClickOnIcon),
        typeof(bool), typeof(TitleBar), new PropertyMetadata(true));

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
    /// Property for <see cref="TemplateButtonCommand"/>.
    /// </summary>
    public static readonly DependencyProperty TemplateButtonCommandProperty =
        DependencyProperty.Register(nameof(TemplateButtonCommand),
            typeof(Common.IRelayCommand), typeof(TitleBar), new PropertyMetadata(null));

    #endregion

    #region Properties

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
    /// Gets or sets information whether to show close button.
    /// </summary>
    public bool ShowClose
    {
        get => (bool)GetValue(ShowCloseProperty);
        set => SetValue(ShowCloseProperty, value);
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
    /// Enables or disable closing the window by double clicking on the icon
    /// </summary>
    public bool CloseWindowByDoubleClickOnIcon
    {
        get => (bool)GetValue(CloseWindowByDoubleClickOnIconProperty);
        set => SetValue(CloseWindowByDoubleClickOnIconProperty, value);
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
    public IRelayCommand TemplateButtonCommand => (IRelayCommand)GetValue(TemplateButtonCommandProperty);

    /// <summary>
    /// Lets you override the behavior of the Maximize/Restore button with an <see cref="Action"/>.
    /// </summary>
    public Action<TitleBar, System.Windows.Window>? MaximizeActionOverride { get; set; }

    /// <summary>
    /// Lets you override the behavior of the Minimize button with an <see cref="Action"/>.
    /// </summary>
    public Action<TitleBar, System.Windows.Window>? MinimizeActionOverride { get; set; }

    #endregion

    private Interop.WinDef.POINT _doubleClickPoint;
    private System.Windows.Window _currentWindow = null!;
    private System.Windows.Controls.Grid _mainGrid = null!;
    private System.Windows.Controls.Image _icon = null!;
    private readonly TitleBarButton[] _buttons = new TitleBarButton[4];

    /// <summary>
    /// Creates a new instance of the class and sets the default <see cref="FrameworkElement.Loaded"/> event.
    /// </summary>
    public TitleBar()
    {
        SetValue(TemplateButtonCommandProperty, new Common.RelayCommand<TitleBarButtonType>(OnTemplateButtonClick));

        Loaded += OnLoaded;
        Unloaded += OnUnloaded;
    }

    /// <inheritdoc />
    protected override void OnInitialized(EventArgs e)
    {
        base.OnInitialized(e);

        Theme = Appearance.Theme.GetAppTheme();
        Appearance.Theme.Changed += OnThemeChanged;
    }

    protected virtual void OnLoaded(object sender, RoutedEventArgs e)
    {
        if (DesignerHelper.IsInDesignMode)
            return;

        _currentWindow = System.Windows.Window.GetWindow(this) ?? throw new ArgumentNullException("Window is null");
        _currentWindow.StateChanged += OnParentWindowStateChanged;

        var handle = new WindowInteropHelper(_currentWindow).EnsureHandle();
        var windowSource = HwndSource.FromHwnd(handle) ?? throw new ArgumentNullException("Window source is null");
        windowSource.AddHook(HwndSourceHook);
    }

    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
        Loaded -= OnLoaded;
        Unloaded -= OnUnloaded;

        Appearance.Theme.Changed -= OnThemeChanged;

        _mainGrid.MouseLeftButtonDown -= OnMainGridMouseLeftButtonDown;
        _mainGrid.MouseMove -= OnMainGridMouseMove;
    }

    /// <summary>
    /// Invoked whenever application code or an internal process,
    /// such as a rebuilding layout pass, calls the ApplyTemplate method.
    /// </summary>
    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        _mainGrid = GetTemplateChild<System.Windows.Controls.Grid>(ElementMainGrid);
        _mainGrid.MouseLeftButtonDown += OnMainGridMouseLeftButtonDown;
        _mainGrid.MouseMove += OnMainGridMouseMove;

        _icon = GetTemplateChild<System.Windows.Controls.Image>(ElementIcon);

        var helpButton = GetTemplateChild<TitleBarButton>(ElementHelpButton);
        var minimizeButton = GetTemplateChild<TitleBarButton>(ElementMinimizeButton);
        var maximizeButton = GetTemplateChild<TitleBarButton>(ElementMaximizeButton);
        var closeButton = GetTemplateChild<TitleBarButton>(ElementCloseButton);

        _buttons[0] = maximizeButton;
        _buttons[1] = minimizeButton;
        _buttons[2] = closeButton;
        _buttons[3] = helpButton;
    }

    /// <summary>
    /// This virtual method is triggered when the app's theme changes.
    /// </summary>
    protected virtual void OnThemeChanged(Appearance.ThemeType currentTheme, Color systemAccent)
    {
        Debug.WriteLine($"INFO | {typeof(TitleBar)} received theme -  {currentTheme}",
            "Wpf.Ui.TitleBar");

        Theme = currentTheme;
    }

    private void CloseWindow()
    {
        Debug.WriteLine($"INFO | {typeof(TitleBar)}.CloseWindow:ForceShutdown -  {ForceShutdown}",
            "Wpf.Ui.TitleBar");

        if (ForceShutdown)
        {
            Application.Current.Shutdown();
            return;
        }

        _currentWindow.Close();
    }

    private void MinimizeWindow()
    {
        if (MinimizeToTray && Tray.IsRegistered && MinimizeWindowToTray())
            return;

        if (MinimizeActionOverride is not null)
        {
            MinimizeActionOverride(this, _currentWindow);

            return;
        }

        _currentWindow.WindowState = WindowState.Minimized;
    }

    private void MaximizeWindow()
    {
        if (!CanMaximize)
            return;

        if (MaximizeActionOverride is not null)
        {
            MaximizeActionOverride(this, _currentWindow);

            return;
        }

        if (_currentWindow.WindowState == WindowState.Normal)
        {
            IsMaximized = true;
            _currentWindow.WindowState = WindowState.Maximized;
        }
        else
        {
            IsMaximized = false;
            _currentWindow.WindowState = WindowState.Normal;
        }
    }

    private void RestoreWindow()
    {
        if (!CanMaximize)
            return;

        if (MaximizeActionOverride is not null)
        {
            MaximizeActionOverride(this, _currentWindow);

            return;
        }

        if (_currentWindow.WindowState == WindowState.Normal)
        {
            IsMaximized = true;
            _currentWindow.WindowState = WindowState.Maximized;
        }
        else
        {
            IsMaximized = false;
            _currentWindow.WindowState = WindowState.Normal;
        }
    }

    private bool MinimizeWindowToTray()
    {
        if (!Tray.IsRegistered)
            return false;

        _currentWindow.WindowState = WindowState.Minimized;
        _currentWindow.Hide();

        return true;
    }

    private void OnMainGridMouseMove(object sender, MouseEventArgs e)
    {
        if (e.LeftButton != MouseButtonState.Pressed)
            return;

        // prevent firing from double clicking when the mouse never actually moved
        User32.GetCursorPos(out var currentMousePos);

        if (currentMousePos.x == _doubleClickPoint.x && currentMousePos.y == _doubleClickPoint.y)
            return;

        // prevent firing when there's an open popup
        if (PresentationSource.CurrentSources.OfType<HwndSource>().Any(source => source.RootVisual is FrameworkElement
            {
                Parent: Popup { IsOpen: true }
            }))
            return;

        if (IsMaximized)
        {
            var screenPoint = PointToScreen(e.MouseDevice.GetPosition(this));
            var systemDpi = DpiHelper.GetSystemDpi();

            screenPoint.X /= systemDpi.DpiScaleX;
            screenPoint.Y /= systemDpi.DpiScaleY;

            // TODO: refine the Left value to be more accurate
            // - This calculation is good enough using the center
            //   of the titlebar, however this isn't quite accurate for
            //   how the OS operates.
            // - It should be set as a % (e.g. screen X / maximized width),
            //   then offset from the left to line up more naturally.
            _currentWindow.Left = screenPoint.X - (_currentWindow.RestoreBounds.Width * 0.5);
            _currentWindow.Top = screenPoint.Y;

            // style has to be quickly swapped to avoid restore animation delay
            var style = _currentWindow.WindowStyle;
            _currentWindow.WindowStyle = WindowStyle.None;
            _currentWindow.WindowState = WindowState.Normal;
            _currentWindow.WindowStyle = style;
        }

        // Call drag move only when mouse down, check again
        // if()
        if (e.LeftButton == MouseButtonState.Pressed)
            _currentWindow.DragMove();
    }

    private void OnParentWindowStateChanged(object sender, EventArgs e)
    {
        if (IsMaximized != (_currentWindow.WindowState == WindowState.Maximized))
            IsMaximized = _currentWindow.WindowState == WindowState.Maximized;
    }

    private void OnMainGridMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ClickCount != 2)
            return;

        Interop.User32.GetCursorPos(out _doubleClickPoint);

        MaximizeWindow();
    }

    private void OnTemplateButtonClick(TitleBarButtonType buttonType)
    {
        switch (buttonType)
        {
            case TitleBarButtonType.Maximize:
                RaiseEvent(new RoutedEventArgs(MaximizeClickedEvent, this));
                MaximizeWindow();
                break;

            case TitleBarButtonType.Restore:
                RaiseEvent(new RoutedEventArgs(MaximizeClickedEvent, this));
                RestoreWindow();
                break;

            case TitleBarButtonType.Close:
                RaiseEvent(new RoutedEventArgs(CloseClickedEvent, this));
                CloseWindow();
                break;

            case TitleBarButtonType.Minimize:
                RaiseEvent(new RoutedEventArgs(MinimizeClickedEvent, this));
                MinimizeWindow();
                break;

            case TitleBarButtonType.Help:
                RaiseEvent(new RoutedEventArgs(HelpClickedEvent, this));
                break;
        }
    }

    private IntPtr HwndSourceHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
    {
        var message = (User32.WM)msg;

        if (message is not (User32.WM.NCHITTEST or User32.WM.NCMOUSELEAVE or User32.WM.NCLBUTTONDOWN or User32.WM.NCLBUTTONUP))
            return IntPtr.Zero;

        foreach (var button in _buttons)
        {
            if (!button.ReactToHwndHook(message, lParam, out var returnIntPtr))
                continue;

            //It happens that the background is not removed from the buttons and you can make all the buttons are in the IsHovered=true
            //It cleans up
            foreach (var anotherButton in _buttons)
            {
                if (anotherButton == button)
                    continue;

                if (anotherButton.IsHovered && button.IsHovered)
                {
                    anotherButton.RemoveHover();
                }
            }

            handled = true;
            return returnIntPtr;
        }

        switch (message)
        {
            case User32.WM.NCHITTEST when CloseWindowByDoubleClickOnIcon && _icon.IsMouseOverElement(lParam):
                handled = true;
                //Ideally, clicking on the icon should open the system menu, but when the system menu is opened manually, double-clicking on the icon does not close the window
                return (IntPtr)User32.WM_NCHITTEST.HTSYSMENU;
            case User32.WM.NCHITTEST when this.IsMouseOverElement(lParam):
                handled = true;
                return (IntPtr)User32.WM_NCHITTEST.HTCAPTION;
            default:
                return IntPtr.Zero;
        }
    }

    private T GetTemplateChild<T>(string name) where T : DependencyObject
    {
        var element = base.GetTemplateChild(name);

        if (element is null)
            throw new ArgumentNullException($"{name} is null");

        return (T)element;
    }
}
