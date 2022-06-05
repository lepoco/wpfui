// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Interop;
using System.Windows.Media;
using WPFUI.Common;
using WPFUI.Extensions;
using WPFUI.Tray;

/*
 * TODO: Handle closing of the main window.
 * NOTE
 * The problem is as follows:
 * If the main window is closed with the Debugger or simply destroyed,
 * it will not send WM_CLOSE or WM_DESTROY to its child windows. This
 * way, we can't tell tray to close the icon. Thus, we need to add to
 * the TrayHandler a mechanism that detects that the parent window has
 * been closed and then send
 * Shell32.Shell_NotifyIcon(Shell32.NIM.DELETE, Shell32.NOTIFYICONDATA);
 *
 * In another situation, the TrayHandler can also be forced to close,
 * so there is need to detect from the side somehow if this has happened
 * and remove the icon.
 */

namespace WPFUI.Controls;

/// <summary>
/// Represents the implementation of icon in the tray menu as <see cref="FrameworkElement"/>.
/// </summary>
public class NotifyIcon : System.Windows.FrameworkElement, INotifyIcon
{
    private ContextMenu _contextMenu;

    /// <summary>
    /// Whether the control is disposed.
    /// </summary>
    protected bool Disposed = false;

    #region Internal variables

    /// <summary>
    /// Provides a set of information for Shell32 to manipulate the icon.
    /// </summary>
    internal Interop.Shell32.NOTIFYICONDATA ShellIconData { get; set; }

    #endregion

    #region Public variables

    /// <inheritdoc />
    public int Id { get; internal set; } = -1;

    /// <summary>
    /// Whether the control is attached to the shell.
    /// </summary>
    public bool Attached { get; internal set; } = false;

    /// <summary>
    /// Whether the icon is  registered in the tray menu.
    /// </summary>
    public bool IsRegistered { get; internal set; } = false;

    /// <inheritdoc />
    public HwndSource HookWindow { get; set; }

    /// <inheritdoc />
    public IntPtr ParentHandle { get; set; }

    #endregion

    #region Properties

    /// <summary>
    /// Property for <see cref="TooltipText"/>.
    /// </summary>
    public static readonly DependencyProperty TooltipTextProperty = DependencyProperty.Register(nameof(TooltipText),
        typeof(string), typeof(NotifyIcon),
        new PropertyMetadata(String.Empty));

    /// <summary>
    /// Property for <see cref="FocusOnLeftClick"/>.
    /// </summary>
    public static readonly DependencyProperty FocusOnLeftClickProperty = DependencyProperty.Register(
        nameof(FocusOnLeftClick),
        typeof(bool), typeof(NotifyIcon),
        new PropertyMetadata(true));

    /// <summary>
    /// Property for <see cref="MenuOnRightClick"/>.
    /// </summary>
    public static readonly DependencyProperty MenuOnRightClickProperty = DependencyProperty.Register(
        nameof(MenuOnRightClick),
        typeof(bool), typeof(NotifyIcon),
        new PropertyMetadata(true));

    /// <summary>
    /// Property for <see cref="Icon"/>.
    /// </summary>
    public static readonly DependencyProperty IconProperty = DependencyProperty.Register(nameof(Icon),
        typeof(ImageSource), typeof(NotifyIcon),
        new PropertyMetadata(null));

    /// <summary>
    /// Property for <see cref="Menu"/>.
    /// </summary>
    public static readonly DependencyProperty MenuProperty = DependencyProperty.Register(nameof(Menu),
        typeof(ContextMenu), typeof(NotifyIcon),
        new PropertyMetadata(null, MenuProperty_OnChanged));

    /// <summary>
    /// Property for <see cref="MenuFontSize"/>.
    /// </summary>
    public static readonly DependencyProperty MenuFontSizeProperty = DependencyProperty.Register(
        nameof(MenuFontSize),
        typeof(double), typeof(NotifyIcon),
        new PropertyMetadata(14d));

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
    public static readonly RoutedEvent LeftClickEvent =
        EventManager.RegisterRoutedEvent(nameof(LeftClick), RoutingStrategy.Bubble,
            typeof(RoutedNotifyIconEvent), typeof(NotifyIcon));

    /// <summary>
    /// Registration for <see cref="LeftDoubleClick"/>.
    /// </summary>
    public static readonly RoutedEvent LeftDoubleClickEvent =
        EventManager.RegisterRoutedEvent(nameof(LeftDoubleClick), RoutingStrategy.Bubble,
            typeof(RoutedNotifyIconEvent), typeof(NotifyIcon));

    /// <summary>
    /// Registration for <see cref="RightClick"/>.
    /// </summary>
    public static readonly RoutedEvent RightClickEvent =
        EventManager.RegisterRoutedEvent(nameof(RightClick), RoutingStrategy.Bubble,
            typeof(RoutedNotifyIconEvent), typeof(NotifyIcon));

    /// <summary>
    /// Registration for <see cref="RightDoubleClick"/>.
    /// </summary>
    public static readonly RoutedEvent RightDoubleClickEvent =
        EventManager.RegisterRoutedEvent(nameof(RightDoubleClick), RoutingStrategy.Bubble,
            typeof(RoutedNotifyIconEvent), typeof(NotifyIcon));

    /// <summary>
    /// Registration for <see cref="MiddleClick"/>.
    /// </summary>
    public static readonly RoutedEvent MiddleClickEvent =
        EventManager.RegisterRoutedEvent(nameof(MiddleClick), RoutingStrategy.Bubble,
            typeof(RoutedNotifyIconEvent), typeof(NotifyIcon));

    /// <summary>
    /// Registration for <see cref="MiddleDoubleClick"/>.
    /// </summary>
    public static readonly RoutedEvent MiddleDoubleClickEvent =
        EventManager.RegisterRoutedEvent(nameof(MiddleDoubleClick), RoutingStrategy.Bubble,
            typeof(RoutedNotifyIconEvent), typeof(NotifyIcon));

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

    /// <summary>
    /// Control destructor.
    /// </summary>
    ~NotifyIcon()
    {
        Dispose(false);
    }


    /// <summary>
    /// Tries to register the <see cref="NotifyIcon"/> in the shell.
    /// </summary>
    public void Register()
    {
        Attached = TrayManager.Register(this, Window.GetWindow(this));
    }

    /// <summary>
    /// Tries to unregister the <see cref="NotifyIcon"/> from the shell.
    /// </summary>
    public void Unregister()
    {
        Attached = !TrayManager.Unregister(this);
    }

    /// <inheritdoc />
    public void ShowMenu()
    {
#if DEBUG
        System.Diagnostics.Debug.WriteLine($"INFO | {typeof(TrayHandler)} invoked {nameof(ShowMenu)} method.",
            "WPFUI.NotifyIcon");
#endif
        if (_contextMenu == null)
            return;

        // Without setting the handler window at the front, menu may appear behind the taskbar
        Interop.User32.SetForegroundWindow(HookWindow.Handle);
        ContextMenuService.SetPlacement(_contextMenu, PlacementMode.MousePoint);

        _contextMenu.ApplyMica();
        _contextMenu.IsOpen = true;
    }

    /// <summary>
    /// Tries to focus the <see cref="Application.MainWindow"/>.
    /// </summary>
    public void FocusApp()
    {
#if DEBUG
        System.Diagnostics.Debug.WriteLine($"INFO | {typeof(TrayHandler)} invoked {nameof(FocusApp)} method.",
            "WPFUI.NotifyIcon");
#endif
        var mainWindow = Application.Current.MainWindow;

        if (mainWindow == null)
            return;

        if (mainWindow.WindowState == WindowState.Minimized)
            mainWindow.WindowState = WindowState.Normal;

        mainWindow.Show();

        if (mainWindow.Topmost)
        {
            mainWindow.Topmost = false;
            mainWindow.Topmost = true;
        }
        else
        {
            mainWindow.Topmost = true;
            mainWindow.Topmost = false;
        }

        mainWindow.Focus();
    }

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

        if (Attached)
            return;

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
        System.Diagnostics.Debug.WriteLine($"INFO | {typeof(NotifyIcon)} disposed.", "WPFUI.NotifyIcon");
#endif

        Unregister();
    }

    #endregion

    #region Windows messages handler

    /// <summary>
    /// A callback function that processes messages sent to a window.
    /// The WNDPROC type defines a pointer to this callback function.
    /// </summary>
    internal IntPtr WndProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
    {
        var uMsg = (Interop.User32.WM)msg;

        switch (uMsg)
        {
            case Interop.User32.WM.DESTROY:
#if DEBUG
                System.Diagnostics.Debug.WriteLine($"INFO | {typeof(TrayHandler)} received {uMsg} message.",
                    "WPFUI.NotifyIcon");
#endif
                Dispose();

                handled = true;

                return IntPtr.Zero;

            case Interop.User32.WM.NCDESTROY:
#if DEBUG
                System.Diagnostics.Debug.WriteLine($"INFO | {typeof(TrayHandler)} received {uMsg} message.",
                    "WPFUI.NotifyIcon");
#endif
                handled = false;

                return IntPtr.Zero;

            case Interop.User32.WM.CLOSE:
#if DEBUG
                System.Diagnostics.Debug.WriteLine($"INFO | {typeof(TrayHandler)} received {uMsg} message.",
                    "WPFUI.NotifyIcon");
#endif
                handled = true;

                return IntPtr.Zero;
        }

        if (uMsg != Interop.User32.WM.TRAYMOUSEMESSAGE)
        {
            handled = false;

            return IntPtr.Zero;
        }

        var lMsg = (Interop.User32.WM)lParam;

        switch (lMsg)
        {
            case Interop.User32.WM.LBUTTONDOWN:
                OnLeftClick();

                if (FocusOnLeftClick)
                    FocusApp();
                break;

            case Interop.User32.WM.LBUTTONDBLCLK:
                OnLeftDoubleClick();
                break;

            case Interop.User32.WM.RBUTTONDOWN:
                OnRightClick();

                if (MenuOnRightClick)
                    ShowMenu();
                break;

            case Interop.User32.WM.RBUTTONDBLCLK:
                OnRightDoubleClick();
                break;

            case Interop.User32.WM.MBUTTONDOWN:
                OnMiddleClick();
                break;

            case Interop.User32.WM.MBUTTONDBLCLK:
                OnMiddleDoubleClick();
                break;
        }

        handled = true;

        return IntPtr.Zero;
    }

    #endregion

    #region Private methods

    private static void MenuProperty_OnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not NotifyIcon notifyIcon)
            return;

        if (e.NewValue is not ContextMenu contextMenu)
            return;

        notifyIcon._contextMenu = contextMenu;
        notifyIcon._contextMenu.FontSize = notifyIcon.MenuFontSize;
    }

    #endregion
}
