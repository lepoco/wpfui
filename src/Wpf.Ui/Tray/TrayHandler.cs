// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Windows.Interop;

namespace Wpf.Ui.Tray;

/// <summary>
/// Manages the Win32 API and Windows messages.
/// </summary>
internal class TrayHandler : HwndSource
{
    /// <summary>
    /// Id of the hooked element.
    /// </summary>
    public int ElementId { get; internal set; }

    /// <summary>
    /// Creates a new hWnd as a child with transparency parameters, no size and in the default position. Then, it attach the default delegation to the messages it receives.
    /// </summary>
    /// <param name="name">The name of the created window.</param>
    /// <param name="parent">Parent of the created window.</param>
    public TrayHandler(string name, IntPtr parent)
        : base(0x0, 0x4000000, 0x80000 | 0x20 | 0x00000008 | 0x08000000, 0, 0, 0, 0, name, parent)
    {
#if DEBUG
        System.Diagnostics.Debug.WriteLine($"INFO | New {typeof(TrayHandler)} registered with handle: #{Handle}, and parent: #{parent}", "Wpf.Ui.TrayHandler");
#endif
    }
}
