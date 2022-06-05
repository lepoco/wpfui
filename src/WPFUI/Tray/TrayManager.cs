// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

namespace WPFUI.Tray;

/// <summary>
/// Responsible for managing the icons in the Tray bar.
/// </summary>
internal static class TrayManager
{
    #region Notify Icon control

    /// <summary>
    /// Tries to register the <see cref="Controls.NotifyIcon"/> in the shell.
    /// </summary>
    public static bool Register(Controls.NotifyIcon notifyIcon)
    {
        return Register(notifyIcon, GetParentSource());
    }

    /// <summary>
    /// Tries to register the <see cref="Controls.NotifyIcon"/> in the shell.
    /// </summary>
    public static bool Register(Controls.NotifyIcon notifyIcon, Window parentWindow)
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
    public static bool Register(Controls.NotifyIcon notifyIcon, HwndSource parentSource)
    {
        if (notifyIcon.IsRegistered)
            Unregister(notifyIcon);

#if DEBUG
        System.Diagnostics.Debug.WriteLine($"INFO | {typeof(Controls.NotifyIcon)} registration started.",
            "WPFUI.TrayManager");
#endif

        if (parentSource == null)
            return false;

        notifyIcon.Id = TrayData.NotifyIcons.Count + 1;

        var shellIconData = notifyIcon.ShellIconData;

        notifyIcon.HookWindow = RegisterIconInternal(ref shellIconData, notifyIcon.ParentHandle,
            notifyIcon.Id, notifyIcon.TooltipText, notifyIcon.Icon);


        notifyIcon.HookWindow.AddHook(notifyIcon.WndProc);

        TrayData.NotifyIcons.Add(notifyIcon);

        notifyIcon.IsRegistered = true;

        return true;
    }

    /// <summary>
    /// Tries to unregister the <see cref="Controls.NotifyIcon"/> from the shell.
    /// </summary>
    public static bool Unregister(Controls.NotifyIcon notifyIcon)
    {
#if DEBUG
        System.Diagnostics.Debug.WriteLine($"INFO | {typeof(Controls.NotifyIcon)} unregistration started.",
            "WPFUI.TrayManager");
#endif
        if (notifyIcon.ShellIconData == null)
            return false;

        Interop.Shell32.Shell_NotifyIcon(Interop.Shell32.NIM.DELETE, notifyIcon.ShellIconData);

        notifyIcon.IsRegistered = false;

        return true;
    }

    #endregion Notify Icon control

    #region Notify Icon service or others

    public static bool Register(NotifyIconBase notifyIconService)
    {
        if (notifyIconService.IsRegistered)
            Unregister(notifyIconService);

        if (notifyIconService.ParentHandle == IntPtr.Zero)
            return false;

        notifyIconService.Id = TrayData.NotifyIcons.Count + 1;

        var shellIconData = notifyIconService.ShellIconData;

        notifyIconService.HookWindow = RegisterIconInternal(ref shellIconData, notifyIconService.ParentHandle,
            notifyIconService.Id, notifyIconService.TooltipText, notifyIconService.Icon);

        notifyIconService.ShellIconData = shellIconData;

        notifyIconService.HookWindow.AddHook(notifyIconService.WndProc);

        notifyIconService.IsRegistered = true;

        return true;
    }

    public static bool Unregister(NotifyIconBase notifyIconService)
    {
        if (notifyIconService.ShellIconData == null)
            return false;

        Interop.Shell32.Shell_NotifyIcon(Interop.Shell32.NIM.DELETE, notifyIconService.ShellIconData);

        notifyIconService.IsRegistered = false;

        return true;
    }

    #endregion Notify Icon service or others

    /// <summary>
    /// Gets application source.
    /// </summary>
    public static HwndSource GetParentSource()
    {
        var mainWindow = Application.Current.MainWindow;

        if (mainWindow == null)
            return null;

        return (HwndSource)PresentationSource.FromVisual(mainWindow);
    }

    private static TrayHandler RegisterIconInternal(ref Interop.Shell32.NOTIFYICONDATA shellIconData, IntPtr parentHandle, int iconId, string tooltipText, ImageSource imageSource)
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
