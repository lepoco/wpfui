// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Windows;
using System.Windows.Interop;

namespace Wpf.Ui.Tray;

/*
 * TODO: Handle closing of the parent window.
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

/// <summary>
/// Responsible for managing the icons in the Tray bar.
/// </summary>
internal static class TrayManager
{
    public static bool Register(INotifyIcon notifyIcon)
    {
        return Register(notifyIcon, GetParentSource());
    }

    public static bool Register(INotifyIcon notifyIcon, Window parentWindow)
    {
        if (parentWindow == null)
            return false;

        return Register(notifyIcon, (HwndSource)PresentationSource.FromVisual(parentWindow));
    }

    public static bool Register(INotifyIcon notifyIcon, HwndSource parentSource)
    {
        if (parentSource == null)
        {
            if (!notifyIcon.IsRegistered)
                return false;

            Unregister(notifyIcon);

            return false;
        }

        if (parentSource.Handle == IntPtr.Zero)
            return false;

        if (notifyIcon.IsRegistered)
            Unregister(notifyIcon);

        notifyIcon.Id = TrayData.NotifyIcons.Count + 1;

        notifyIcon.HookWindow =
            new TrayHandler($"wpfui_th_{parentSource.Handle}_{notifyIcon.Id}", parentSource.Handle) { ElementId = notifyIcon.Id };

        notifyIcon.ShellIconData = new Interop.Shell32.NOTIFYICONDATA
        {
            uID = notifyIcon.Id,
            uFlags = Interop.Shell32.NIF.MESSAGE,
            uCallbackMessage = (int)Interop.User32.WM.TRAYMOUSEMESSAGE,
            hWnd = notifyIcon.HookWindow.Handle,
            dwState = 0x2
        };

        if (!String.IsNullOrEmpty(notifyIcon.TooltipText))
        {
            notifyIcon.ShellIconData.szTip = notifyIcon.TooltipText;
            notifyIcon.ShellIconData.uFlags |= Interop.Shell32.NIF.TIP;
        }

        ReloadHicon(notifyIcon);

        notifyIcon.HookWindow.AddHook(notifyIcon.WndProc);

        Interop.Shell32.Shell_NotifyIcon(Interop.Shell32.NIM.ADD, notifyIcon.ShellIconData);

        TrayData.NotifyIcons.Add(notifyIcon);

        notifyIcon.IsRegistered = true;

        return true;
    }

    public static bool ModifyIcon(INotifyIcon notifyIcon)
    {
        if (!notifyIcon.IsRegistered)
            return true;

        ReloadHicon(notifyIcon);

        return Interop.Shell32.Shell_NotifyIcon(Interop.Shell32.NIM.MODIFY, notifyIcon.ShellIconData);
    }

    /// <summary>
    /// Tries to remove the <see cref="INotifyIcon"/> from the shell.
    /// </summary>
    public static bool Unregister(INotifyIcon notifyIcon)
    {
        if (notifyIcon.ShellIconData == null || !notifyIcon.IsRegistered)
            return false;

        Interop.Shell32.Shell_NotifyIcon(Interop.Shell32.NIM.DELETE, notifyIcon.ShellIconData);

        notifyIcon.IsRegistered = false;

        return true;
    }

    /// <summary>
    /// Gets application source.
    /// </summary>
    private static HwndSource GetParentSource()
    {
        var mainWindow = Application.Current.MainWindow;

        if (mainWindow == null)
            return null;

        return (HwndSource)PresentationSource.FromVisual(mainWindow);
    }

    private static void ReloadHicon(INotifyIcon notifyIcon)
    {
        var hIcon = IntPtr.Zero;

        if (notifyIcon.Icon != null)
            hIcon = Hicon.FromSource(notifyIcon.Icon);

        if (hIcon == IntPtr.Zero)
            hIcon = Hicon.FromApp();

        if (hIcon != IntPtr.Zero)
        {
            notifyIcon.ShellIconData.hIcon = hIcon;
            notifyIcon.ShellIconData.uFlags |= Interop.Shell32.NIF.ICON;
        }
    }
}
