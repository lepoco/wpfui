// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

namespace Wpf.Ui.Tray;

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

        var shellIconData = notifyIcon.ShellIconData;

        notifyIcon.HookWindow = RegisterNotifyIconData(ref shellIconData, parentSource.Handle,
                    notifyIcon.Id, notifyIcon.TooltipText, notifyIcon.Icon);

        notifyIcon.HookWindow.AddHook(notifyIcon.WndProc);

        TrayData.NotifyIcons.Add(notifyIcon);

        notifyIcon.IsRegistered = true;

        return true;
    }

    /// <summary>
    /// Tries to remove the <see cref="INotifyIcon"/> from the shell.
    /// </summary>
    public static bool Unregister(INotifyIcon notifyIcon)
    {
        if (notifyIcon.ShellIconData == null)
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

    private static TrayHandler RegisterNotifyIconData(ref Interop.Shell32.NOTIFYICONDATA shellIconData, IntPtr parentHandle, int iconId, string tooltipText, ImageSource imageSource)
    {
        var hookWindow =
            new TrayHandler($"wpfui_th_{parentHandle}_{iconId}", parentHandle) { ElementId = iconId };

        shellIconData = new Interop.Shell32.NOTIFYICONDATA
        {
            uID = iconId,
            uFlags = Interop.Shell32.NIF.MESSAGE,
            uCallbackMessage = (int)Interop.User32.WM.TRAYMOUSEMESSAGE,
            hWnd = hookWindow.Handle,
            dwState = 0x2
        };

        if (!String.IsNullOrEmpty(tooltipText))
        {
            shellIconData.szTip = tooltipText;
            shellIconData.uFlags |= Interop.Shell32.NIF.TIP;
        }

        var hIcon = IntPtr.Zero;

        if (imageSource != null)
            hIcon = Hicon.FromSource(imageSource);

        if (hIcon == IntPtr.Zero)
            hIcon = Hicon.FromApp();

        if (hIcon != IntPtr.Zero)
        {
            shellIconData.hIcon = hIcon;
            shellIconData.uFlags |= Interop.Shell32.NIF.ICON;
        }

        Interop.Shell32.Shell_NotifyIcon(Interop.Shell32.NIM.ADD, shellIconData);

        return hookWindow;
    }
}
