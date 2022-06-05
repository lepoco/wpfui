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
using WPFUI.Extensions;

namespace WPFUI.Tray;

/// <summary>
/// Base implementation of NotifyIcon.
/// </summary>
public abstract class NotifyIconBase : INotifyIcon, IDisposable
{
    /// <summary>
    /// Provides a set of information for Shell32 to manipulate the icon.
    /// </summary>
    internal Interop.Shell32.NOTIFYICONDATA ShellIconData { get; set; }

    /// <summary>
    /// Whether the class is disposed.
    /// </summary>
    protected bool Disposed { get; set; }

    /// <summary>
    /// Whether the class is disposed.
    /// </summary>
    public bool IsRegistered { get; internal set; }

    /// <summary>
    /// Gets or sets the hWnd that will receive messages for the icon.
    /// </summary>
    public virtual HwndSource HookWindow { get; set; }

    /// <summary>
    /// Gets or sets the hWnd that the icon belongs to.
    /// </summary>
    public virtual IntPtr ParentHandle { get; set; }

    /// <summary>
    /// Gets the Shell identifier of the icon.
    /// </summary>
    public virtual int Id { get; internal set; }

    /// <summary>
    /// Gets or sets the ToolTip text displayed when the mouse pointer rests on a notification area icon.
    /// </summary>
    public virtual string TooltipText { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="System.Windows.Media.Imaging.BitmapSource"/> of the tray icon.
    /// </summary>
    public virtual ImageSource Icon { get; set; }

    /// <summary>
    /// Gets or sets the menu displayed when the icon is right-clicked.
    /// </summary>
    public virtual ContextMenu ContextMenu { get; set; }

    /// <summary>
    /// Gets or sets the value indicating whether to focus the <see cref="Application.MainWindow"/> on single left click.
    /// </summary>
    public bool FocusOnLeftClick { get; set; } = true;

    /// <summary>
    /// Gets or sets the value indicating whether to show the <see cref="Menu"/> on single right click.
    /// </summary>
    public bool MenuOnRightClick { get; set; } = true;

    /// <summary>
    /// Class destructor.
    /// </summary>
    ~NotifyIconBase()
    {
        Dispose(false);
    }

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(true);

        GC.SuppressFinalize(this);
    }

    public virtual bool Register()
    {
        IsRegistered = TrayManager.Register(this);

        return IsRegistered;
    }

    public virtual bool Unregister()
    {
        IsRegistered = false;

        return TrayManager.Unregister(this);
    }

    /// <summary>
    /// Focus the application main window.
    /// </summary>
    protected virtual void FocusApp()
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

    /// <summary>
    /// Shows the menu if it has been added.
    /// </summary>
    protected virtual void OpenMenu()
    {
#if DEBUG
        System.Diagnostics.Debug.WriteLine($"INFO | {typeof(TrayHandler)} invoked {nameof(OpenMenu)} method.",
            "WPFUI.NotifyIcon");
#endif
        if (ContextMenu == null)
            return;

        // Without setting the handler window at the front, menu may appear behind the taskbar
        Interop.User32.SetForegroundWindow(HookWindow.Handle);
        ContextMenuService.SetPlacement(ContextMenu, PlacementMode.MousePoint);

        ContextMenu.ApplyMica();
        ContextMenu.IsOpen = true;
    }

    /// <summary>
    /// This virtual method is called when tray icon is left-clicked and it raises the left click <see langword="event"/>.
    /// </summary>
    protected virtual void OnLeftClick()
    {
    }

    /// <summary>
    /// This virtual method is called when tray icon is left-clicked and it raises the left double click <see langword="event"/>.
    /// </summary>
    protected virtual void OnLeftDoubleClick()
    {
    }

    /// <summary>
    /// This virtual method is called when tray icon is left-clicked and it raises the right click <see langword="event"/>.
    /// </summary>
    protected virtual void OnRightClick()
    {
    }

    /// <summary>
    /// This virtual method is called when tray icon is left-clicked and it raises the right double click <see langword="event"/>.
    /// </summary>
    protected virtual void OnRightDoubleClick()
    {
    }

    /// <summary>
    /// This virtual method is called when tray icon is left-clicked and it raises the middle click <see langword="event"/>.
    /// </summary>
    protected virtual void OnMiddleClick()
    {
    }

    /// <summary>
    /// This virtual method is called when tray icon is left-clicked and it raises the middle double click <see langword="event"/>.
    /// </summary>
    protected virtual void OnMiddleDoubleClick()
    {
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
        System.Diagnostics.Debug.WriteLine($"INFO | {typeof(NotifyIconBase)} disposed.", "WPFUI.NotifyIcon");
#endif

        Unregister();
    }

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
                    OpenMenu();
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
}
