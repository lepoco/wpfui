// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;

namespace Wpf.Ui.Tray.Controls;

/// <summary>
/// Represents the implementation of icon in the tray menu as <see cref="FrameworkElement"/>.
/// </summary>
/// <example>
/// <code lang="xml">
/// &lt;tray:NotifyIcon
///     Grid.Row="0"
///     FocusOnLeftClick="True"
///     Icon="pack://application:,,,/Assets/wpfui.png"
///     MenuOnRightClick="True"
///     TooltipText="WPF UI"&gt;
///         &lt;tray:NotifyIcon.Menu&gt;
///             &lt;ContextMenu ItemsSource = "{Binding ViewModel.TrayMenuItems, Mode=OneWay}" /&gt;
///         &lt;/tray:NotifyIcon.Menu&gt;
/// &lt;/tray:NotifyIcon&gt;
/// </code>
/// </example>
//[ToolboxItem(true)]
//[ToolboxBitmap(typeof(NotifyIcon), "NotifyIcon.bmp")]
public class NotifyIcon : System.Windows.FrameworkElement
{
    private readonly Wpf.Ui.Tray.Internal.InternalNotifyIconManager internalNotifyIconManager;

    /// <summary>
    /// Whether the control is disposed.
    /// </summary>
    protected bool Disposed = false;

    #region Public variables

    /// <inheritdoc />
    public int Id => this.internalNotifyIconManager.Id;

    /// <summary>
    /// Whether the icon is  registered in the tray menu.
    /// </summary>
    public bool IsRegistered => this.internalNotifyIconManager.IsRegistered;

    /// <inheritdoc />
    public HwndSource? HookWindow { get; set; }

    /// <inheritdoc />
    public IntPtr ParentHandle { get; set; }

    #endregion

    #region Properties

    /// <summary>
    /// Property for <see cref="TooltipText"/>.
    /// </summary>
    public static readonly DependencyProperty TooltipTextProperty = DependencyProperty.Register(
        nameof(TooltipText),
        typeof(string),
        typeof(NotifyIcon),
        new PropertyMetadata(String.Empty, OnTooltipTextChanged)
    );

    /// <summary>
    /// Property for <see cref="FocusOnLeftClick"/>.
    /// </summary>
    public static readonly DependencyProperty FocusOnLeftClickProperty =
        DependencyProperty.Register(
            nameof(FocusOnLeftClick),
            typeof(bool),
            typeof(NotifyIcon),
            new PropertyMetadata(true, OnFocusOnLeftClickChanged)
        );

    /// <summary>
    /// Property for <see cref="MenuOnRightClick"/>.
    /// </summary>
    public static readonly DependencyProperty MenuOnRightClickProperty =
        DependencyProperty.Register(
            nameof(MenuOnRightClick),
            typeof(bool),
            typeof(NotifyIcon),
            new PropertyMetadata(true, OnMenuOnRightClickChanged)
        );

    /// <summary>
    /// Property for <see cref="Icon"/>.
    /// </summary>
    public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
        nameof(Icon),
        typeof(ImageSource),
        typeof(NotifyIcon),
        new PropertyMetadata((ImageSource)null!, OnIconChanged)
    );

    /// <summary>
    /// Property for <see cref="Menu"/>.
    /// </summary>
    public static readonly DependencyProperty MenuProperty = DependencyProperty.Register(
        nameof(Menu),
        typeof(ContextMenu),
        typeof(NotifyIcon),
        new PropertyMetadata(null, OnMenuChanged)
    );

    /// <summary>
    /// Property for <see cref="MenuFontSize"/>.
    /// </summary>
    public static readonly DependencyProperty MenuFontSizeProperty = DependencyProperty.Register(
        nameof(MenuFontSize),
        typeof(double),
        typeof(NotifyIcon),
        new PropertyMetadata(14d)
    );

    /// <inheritdoc />
    public string TooltipText
    {
        get => (string)GetValue(TooltipTextProperty);
        set => SetValue(TooltipTextProperty, value);
    }

    /// <summary>
    /// Gets or sets the value indicating whether to show the <see cref="Menu"/> on single right click.
    /// </summary>
    public bool MenuOnRightClick
    {
        get => (bool)GetValue(MenuOnRightClickProperty);
        set => SetValue(MenuOnRightClickProperty, value);
    }

    /// <summary>
    /// Gets or sets the value indicating whether to focus the <see cref="Application.MainWindow"/> on single left click.
    /// </summary>
    public bool FocusOnLeftClick
    {
        get => (bool)GetValue(FocusOnLeftClickProperty);
        set => SetValue(FocusOnLeftClickProperty, value);
    }

    /// <inheritdoc />
    public ImageSource Icon
    {
        get => (ImageSource)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    /// <summary>
    /// Context menu.
    /// </summary>
    public ContextMenu Menu
    {
        get => (ContextMenu)GetValue(MenuProperty);
        set => SetValue(MenuProperty, value);
    }

    public double MenuFontSize
    {
        get => (double)GetValue(MenuFontSizeProperty);
        set => SetValue(MenuFontSizeProperty, value);
    }

    #endregion

    #region Events

    /// <summary>
    /// Registration for <see cref="LeftClick"/>.
    /// </summary>
    public static readonly RoutedEvent LeftClickEvent = EventManager.RegisterRoutedEvent(
        nameof(LeftClick),
        RoutingStrategy.Bubble,
        typeof(RoutedNotifyIconEvent),
        typeof(NotifyIcon)
    );

    /// <summary>
    /// Registration for <see cref="LeftDoubleClick"/>.
    /// </summary>
    public static readonly RoutedEvent LeftDoubleClickEvent = EventManager.RegisterRoutedEvent(
        nameof(LeftDoubleClick),
        RoutingStrategy.Bubble,
        typeof(RoutedNotifyIconEvent),
        typeof(NotifyIcon)
    );

    /// <summary>
    /// Registration for <see cref="RightClick"/>.
    /// </summary>
    public static readonly RoutedEvent RightClickEvent = EventManager.RegisterRoutedEvent(
        nameof(RightClick),
        RoutingStrategy.Bubble,
        typeof(RoutedNotifyIconEvent),
        typeof(NotifyIcon)
    );

    /// <summary>
    /// Registration for <see cref="RightDoubleClick"/>.
    /// </summary>
    public static readonly RoutedEvent RightDoubleClickEvent = EventManager.RegisterRoutedEvent(
        nameof(RightDoubleClick),
        RoutingStrategy.Bubble,
        typeof(RoutedNotifyIconEvent),
        typeof(NotifyIcon)
    );

    /// <summary>
    /// Registration for <see cref="MiddleClick"/>.
    /// </summary>
    public static readonly RoutedEvent MiddleClickEvent = EventManager.RegisterRoutedEvent(
        nameof(MiddleClick),
        RoutingStrategy.Bubble,
        typeof(RoutedNotifyIconEvent),
        typeof(NotifyIcon)
    );

    /// <summary>
    /// Registration for <see cref="MiddleDoubleClick"/>.
    /// </summary>
    public static readonly RoutedEvent MiddleDoubleClickEvent = EventManager.RegisterRoutedEvent(
        nameof(MiddleDoubleClick),
        RoutingStrategy.Bubble,
        typeof(RoutedNotifyIconEvent),
        typeof(NotifyIcon)
    );

    /// <summary>
    /// Triggered when the user left-clicks on the <see cref="INotifyIcon"/>.
    /// </summary>
    public event RoutedNotifyIconEvent LeftClick
    {
        add => AddHandler(LeftClickEvent, value);
        remove => RemoveHandler(LeftClickEvent, value);
    }

    /// <summary>
    /// Triggered when the user double-clicks the <see cref="INotifyIcon"/> with the left mouse button.
    /// </summary>
    public event RoutedNotifyIconEvent LeftDoubleClick
    {
        add => AddHandler(LeftDoubleClickEvent, value);
        remove => RemoveHandler(LeftDoubleClickEvent, value);
    }

    /// <summary>
    /// Triggered when the user right-clicks on the <see cref="INotifyIcon"/>.
    /// </summary>
    public event RoutedNotifyIconEvent RightClick
    {
        add => AddHandler(RightClickEvent, value);
        remove => RemoveHandler(RightClickEvent, value);
    }

    /// <summary>
    /// Triggered when the user double-clicks the <see cref="INotifyIcon"/> with the right mouse button.
    /// </summary>
    public event RoutedNotifyIconEvent RightDoubleClick
    {
        add => AddHandler(RightDoubleClickEvent, value);
        remove => RemoveHandler(RightDoubleClickEvent, value);
    }

    /// <summary>
    /// Triggered when the user middle-clicks on the <see cref="INotifyIcon"/>.
    /// </summary>
    public event RoutedNotifyIconEvent MiddleClick
    {
        add => AddHandler(MiddleClickEvent, value);
        remove => RemoveHandler(MiddleClickEvent, value);
    }

    /// <summary>
    /// Triggered when the user double-clicks the <see cref="INotifyIcon"/> with the middle mouse button.
    /// </summary>
    public event RoutedNotifyIconEvent MiddleDoubleClick
    {
        add => AddHandler(MiddleDoubleClickEvent, value);
        remove => RemoveHandler(MiddleDoubleClickEvent, value);
    }

    #endregion

    #region General methods

    public NotifyIcon()
    {
        this.internalNotifyIconManager = new Wpf.Ui.Tray.Internal.InternalNotifyIconManager();

        RegisterHandlers();
    }

    /// <summary>
    /// Control finalizer.
    /// </summary>
    ~NotifyIcon() => Dispose(false);

    /// <summary>
    /// Tries to register the <see cref="NotifyIcon"/> in the shell.
    /// </summary>
    public void Register() => this.internalNotifyIconManager.Register();

    /// <summary>
    /// Tries to unregister the <see cref="NotifyIcon"/> from the shell.
    /// </summary>
    public void Unregister() => this.internalNotifyIconManager.Unregister();

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(true);

        GC.SuppressFinalize(this);
    }

    #endregion

    #region Protected methods

    /// <inheritdoc />
    protected override void OnRender(DrawingContext drawingContext)
    {
        base.OnRender(drawingContext);

        if (this.internalNotifyIconManager.IsRegistered)
            return;

        InitializeIcon();

        Register();
    }

    /// <summary>
    /// This virtual method is called when <see cref="NotifyIcon"/> is left-clicked and it raises the <see cref="LeftClick"/> <see langword="event"/>.
    /// </summary>
    protected virtual void OnLeftClick()
    {
        var newEvent = new RoutedEventArgs(LeftClickEvent, this);
        RaiseEvent(newEvent);
    }

    /// <summary>
    /// This virtual method is called when <see cref="NotifyIcon"/> is left-clicked and it raises the <see cref="LeftDoubleClick"/> <see langword="event"/>.
    /// </summary>
    protected virtual void OnLeftDoubleClick()
    {
        var newEvent = new RoutedEventArgs(LeftDoubleClickEvent, this);
        RaiseEvent(newEvent);
    }

    /// <summary>
    /// This virtual method is called when <see cref="NotifyIcon"/> is left-clicked and it raises the <see cref="RightClick"/> <see langword="event"/>.
    /// </summary>
    protected virtual void OnRightClick()
    {
        var newEvent = new RoutedEventArgs(RightClickEvent, this);
        RaiseEvent(newEvent);
    }

    /// <summary>
    /// This virtual method is called when <see cref="NotifyIcon"/> is left-clicked and it raises the <see cref="RightDoubleClick"/> <see langword="event"/>.
    /// </summary>
    protected virtual void OnRightDoubleClick()
    {
        var newEvent = new RoutedEventArgs(RightDoubleClickEvent, this);
        RaiseEvent(newEvent);
    }

    /// <summary>
    /// This virtual method is called when <see cref="NotifyIcon"/> is left-clicked and it raises the <see cref="MiddleClick"/> <see langword="event"/>.
    /// </summary>
    protected virtual void OnMiddleClick()
    {
        var newEvent = new RoutedEventArgs(MiddleClickEvent, this);
        RaiseEvent(newEvent);
    }

    /// <summary>
    /// This virtual method is called when <see cref="NotifyIcon"/> is left-clicked and it raises the <see cref="MiddleDoubleClick"/> <see langword="event"/>.
    /// </summary>
    protected virtual void OnMiddleDoubleClick()
    {
        var newEvent = new RoutedEventArgs(MiddleDoubleClickEvent, this);
        RaiseEvent(newEvent);
    }

    /// <summary>
    /// If disposing equals <see langword="true"/>, the method has been called directly or indirectly
    /// by a user's code. Managed and unmanaged resources can be disposed. If disposing equals <see langword="false"/>,
    /// the method has been called by the runtime from inside the finalizer and you should not
    /// reference other objects.
    /// <para>Only unmanaged resources can be disposed.</para>
    /// </summary>
    /// <param name="disposing">If disposing equals <see langword="true"/>, dispose all managed and unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (Disposed)
            return;

        Disposed = true;

        if (!disposing)
            return;

#if DEBUG
        System.Diagnostics.Debug.WriteLine(
            $"INFO | {typeof(NotifyIcon)} disposed.",
            "Wpf.Ui.NotifyIcon"
        );
#endif

        Unregister();

        this.internalNotifyIconManager.Dispose();
    }

    #endregion

    /// <summary>
    /// This virtual method is called when <see cref="ContextMenu"/> of <see cref="NotifyIcon"/> is changed.
    /// </summary>
    /// <param name="contextMenu">New context menu object.</param>
    protected virtual void OnMenuChanged(ContextMenu contextMenu)
    {
        this.internalNotifyIconManager.ContextMenu = contextMenu;
        this.internalNotifyIconManager.ContextMenu.FontSize = MenuFontSize;
    }

    private static void OnTooltipTextChanged(
        DependencyObject d,
        DependencyPropertyChangedEventArgs e
    )
    {
        if (d is not NotifyIcon notifyIcon)
        {
            return;
        }

        notifyIcon.TooltipText = e.NewValue as string ?? String.Empty;
    }

    private static void OnIconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not NotifyIcon notifyIcon)
        {
            return;
        }

        notifyIcon.internalNotifyIconManager.Icon = e.NewValue as ImageSource;
        notifyIcon.internalNotifyIconManager.ModifyIcon();
    }

    private static void OnFocusOnLeftClickChanged(
        DependencyObject d,
        DependencyPropertyChangedEventArgs e
    )
    {
        if (d is not NotifyIcon notifyIcon)
            return;

        if (e.NewValue is not bool newValue)
        {
            notifyIcon.FocusOnLeftClick = false;

            return;
        }

        notifyIcon.FocusOnLeftClick = newValue;
    }

    private static void OnMenuOnRightClickChanged(
        DependencyObject d,
        DependencyPropertyChangedEventArgs e
    )
    {
        if (d is not NotifyIcon notifyIcon)
            return;

        if (e.NewValue is not bool newValue)
        {
            notifyIcon.MenuOnRightClick = false;

            return;
        }

        notifyIcon.MenuOnRightClick = newValue;
    }

    private static void OnMenuChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not NotifyIcon notifyIcon)
            return;

        if (e.NewValue is not ContextMenu contextMenu)
            return;

        notifyIcon.OnMenuChanged(contextMenu);
    }

    private void InitializeIcon()
    {
        this.internalNotifyIconManager.TooltipText = TooltipText;
        this.internalNotifyIconManager.Icon = Icon;
        this.internalNotifyIconManager.MenuOnRightClick = MenuOnRightClick;
        this.internalNotifyIconManager.FocusOnLeftClick = FocusOnLeftClick;
    }

    private void RegisterHandlers()
    {
        this.internalNotifyIconManager.LeftClick += OnLeftClick;
        this.internalNotifyIconManager.LeftDoubleClick += OnLeftDoubleClick;
        this.internalNotifyIconManager.RightClick += OnRightClick;
        this.internalNotifyIconManager.RightDoubleClick += OnRightDoubleClick;
        this.internalNotifyIconManager.MiddleClick += OnMiddleClick;
        this.internalNotifyIconManager.MiddleDoubleClick += OnMiddleDoubleClick;
    }
}
