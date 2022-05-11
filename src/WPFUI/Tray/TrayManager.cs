// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Windows;
using System.Windows.Interop;

namespace WPFUI.Tray;

/// <summary>
/// Responsible for managing the icons in the Tray bar.
/// </summary>
public static class TrayManager
{
    /// <summary>
    /// Tries to register the <see cref="Controls.NotifyIcon"/> in the shell.
    /// </summary>
    internal static bool Register(Controls.NotifyIcon notifyIcon)
    {
        return Register(notifyIcon, GetParentSource());
    }

    /// <summary>
    /// Tries to register the <see cref="Controls.NotifyIcon"/> in the shell.
    /// </summary>
    internal static bool Register(Controls.NotifyIcon notifyIcon, Window parentWindow)
    {
        if (parentWindow == null)
            return false;

        var parentSource = (HwndSource)PresentationSource.FromVisual(parentWindow);

        if (parentSource == null)
            return false;

        return Register(notifyIcon, parentSource);
    }

    /// <summary>
    /// Tries to register the <see cref="Controls.NotifyIcon"/> in the shell.
    /// </summary>
    internal static bool Register(Controls.NotifyIcon notifyIcon, HwndSource parentSource)
    {
        if (notifyIcon.Registered)
            Unregister(notifyIcon);

#if DEBUG
        System.Diagnostics.Debug.WriteLine($"INFO | {typeof(Controls.NotifyIcon)} registration started.",
            "WPFUI.TrayManager");
#endif

        if (parentSource == null)
            return false;

        notifyIcon.Id = TrayData.NotifyIcons.Count + 1;

        var hookWindow =
            new TrayHandler($"wpfui_th_{parentSource.Handle}_{notifyIcon.Id}", parentSource.Handle) { ElementId = notifyIcon.Id };

        notifyIcon.ParentHandle = parentSource.Handle;
        notifyIcon.HookWindow = hookWindow;

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

        hookWindow.AddHook(notifyIcon.WndProc);

        Interop.Shell32.Shell_NotifyIcon(Interop.Shell32.NIM.ADD, notifyIcon.ShellIconData);

        TrayData.NotifyIcons.Add(notifyIcon);

        notifyIcon.Registered = true;

        return true;
    }

    /// <summary>
    /// Tries to unregister the <see cref="Controls.NotifyIcon"/> from the shell.
    /// </summary>
    internal static bool Unregister(Controls.NotifyIcon notifyIcon)
    {
#if DEBUG
        System.Diagnostics.Debug.WriteLine($"INFO | {typeof(Controls.NotifyIcon)} unregistration started.",
            "WPFUI.TrayManager");
#endif
        if (notifyIcon.ShellIconData == null)
            return false;

        Interop.Shell32.Shell_NotifyIcon(Interop.Shell32.NIM.DELETE, notifyIcon.ShellIconData);

        notifyIcon.Registered = false;

        return true;
    }

    /// <summary>
    /// Gets application source.
    /// </summary>
    internal static HwndSource GetParentSource()
    {
        var mainWindow = Application.Current.MainWindow;

        if (mainWindow == null)
            return null;

        return (HwndSource)PresentationSource.FromVisual(mainWindow);
    }
}
