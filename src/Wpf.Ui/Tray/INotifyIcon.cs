// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;

namespace Wpf.Ui.Tray;

/// <summary>
/// Represents an icon in the tray menu.
/// </summary>
internal interface INotifyIcon
{
    /// <summary>
    /// Notify icon shell data.
    /// </summary>
    public Interop.Shell32.NOTIFYICONDATA ShellIconData { get; set; }

    /// <summary>
    /// Whether the icon is currently registered in the tray area.
    /// </summary>
    bool IsRegistered { get; set; }

    /// <summary>
    /// Gets the Shell identifier of the icon.
    /// </summary>
    int Id { get; set; }

    /// <summary>
    /// Gets or sets the ToolTip text displayed when the mouse pointer rests on a notification area icon.
    /// </summary>
    string TooltipText { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="System.Windows.Media.Imaging.BitmapSource"/> of the tray icon.
    /// </summary>
    ImageSource Icon { get; set; }

    /// <summary>
    /// Gets or sets the hWnd that will receive messages for the icon.
    /// </summary>
    HwndSource HookWindow { get; set; }

    /// <summary>
    /// Gets or sets the menu displayed when the icon is right-clicked.
    /// </summary>
    ContextMenu ContextMenu { get; set; }

    /// <summary>
    /// Gets or sets the value indicating whether to focus the <see cref="Application.MainWindow"/> on single left click.
    /// </summary>
    bool FocusOnLeftClick { get; set; }

    /// <summary>
    /// Gets or sets the value indicating whether to show the <see cref="Menu"/> on single right click.
    /// </summary>
    bool MenuOnRightClick { get; set; }

    /// <summary>
    /// A callback function that processes messages sent to a window.
    /// The WNDPROC type defines a pointer to this callback function.
    /// </summary>
    public IntPtr WndProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled);

    /// <summary>
    /// Tries to register the <see cref="INotifyIcon"/> in the shell.
    /// </summary>
    bool Register();

    /// <summary>
    /// Tries to register the <see cref="INotifyIcon"/> in the shell.
    /// </summary>
    bool Register(Window parentWindow);

    /// <summary>
    /// Tries to modify the icon of the <see cref="INotifyIcon"/> in the shell.
    /// </summary>
    bool ModifyIcon();

    /// <summary>
    /// Tries to remove the <see cref="INotifyIcon"/> from the shell.
    /// </summary>
    bool Unregister();
}
