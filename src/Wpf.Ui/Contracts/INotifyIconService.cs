// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Wpf.Ui.Contracts;

/// <summary>
/// Represents a contract with a service that provides methods for displaying the icon and menu in the tray area.
/// </summary>
public interface INotifyIconService
{
    /// <summary>
    /// Whether the notify icon is registered in the tray.
    /// </summary>
    public int Id { get; }

    /// <summary>
    /// Whether the notify icon is registered in the tray.
    /// </summary>
    public bool IsRegistered { get; }

    /// <summary>
    /// Gets or sets the ToolTip text displayed when the mouse pointer rests on a notification area icon.
    /// </summary>
    public string TooltipText { get; set; }

    /// <summary>
    /// Context menu displayed after clicking the icon.
    /// </summary>
    ContextMenu ContextMenu { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="System.Windows.Media.Imaging.BitmapFrame"/> of the tray icon.
    /// </summary>
    public ImageSource Icon { get; set; }

    /// <summary>
    /// Tries to register the Notify Icon in the shell.
    /// </summary>
    public bool Register();

    /// <summary>
    /// Tries to unregister the Notify Icon from the shell.
    /// </summary>
    public bool Unregister();

    /// <summary>
    /// Sets parent window of the tray icon.
    /// </summary>
    public void SetParentWindow(Window window);
}

