// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using WPFUI.Common;

namespace WPFUI.Controls.Interfaces;

/// <summary>
/// Represents an icon in the tray menu.
/// </summary>
public interface INotifyIcon : IDisposable
{
    /// <summary>
    /// Gets the Shell identifier of the icon.
    /// </summary>
    public int Id { get; }

    /// <summary>
    /// Gets or sets the ToolTip text displayed when the mouse pointer rests on a notification area icon.
    /// </summary>
    public string TooltipText { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="System.Windows.Media.Imaging.BitmapSource"/> of the tray icon.
    /// </summary>
    public ImageSource Icon { get; set; }

    /// <summary>
    /// Gets or sets the hWnd that will receive messages for the icon.
    /// </summary>
    public HwndSource HookWindow { get; set; }

    /// <summary>
    /// Gets or sets the hWnd that the icon belongs to.
    /// </summary>
    public IntPtr ParentHandle { get; set; }

    /// <summary>
    /// Gets or sets the menu displayed when the icon is right-clicked.
    /// </summary>
    public ContextMenu ContextMenu { get; set; }

    /// <summary>
    /// Triggered when the user left-clicks on the <see cref="INotifyIcon"/>.
    /// </summary>
    public event RoutedNotifyIconEvent LeftClick;

    /// <summary>
    /// Triggered when the user double-clicks the <see cref="INotifyIcon"/> with the left mouse button.
    /// </summary>
    public event RoutedNotifyIconEvent LeftDoubleClick;

    /// <summary>
    /// Triggered when the user right-clicks on the <see cref="INotifyIcon"/>.
    /// </summary>
    public event RoutedNotifyIconEvent RightClick;

    /// <summary>
    /// Triggered when the user double-clicks the <see cref="INotifyIcon"/> with the right mouse button.
    /// </summary>
    public event RoutedNotifyIconEvent RightDoubleClick;

    /// <summary>
    /// Triggered when the user middle-clicks on the <see cref="INotifyIcon"/>.
    /// </summary>
    public event RoutedNotifyIconEvent MiddleClick;

    /// <summary>
    /// Triggered when the user double-clicks the <see cref="INotifyIcon"/> with the middle mouse button.
    /// </summary>
    public event RoutedNotifyIconEvent MiddleDoubleClick;

    /// <summary>
    /// Shows the menu if it has been added.
    /// </summary>
    public void ShowMenu();
}
