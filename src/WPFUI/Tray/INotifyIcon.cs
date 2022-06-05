// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;

namespace WPFUI.Tray;

/// <summary>
/// Represents an icon in the tray menu.
/// </summary>
public interface INotifyIcon : IDisposable
{
    /// <summary>
    /// Whether the icon is currently registered in the tray area.
    /// </summary>
    public bool IsRegistered { get; }

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
}
