// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Runtime.InteropServices;

namespace WPFUI.Win32
{
    /// <summary>
    /// The Microsoft Windows graphics device interface (GDI) enables applications to use graphics and formatted text on both the video display and the printer.
    /// Windows-based applications do not access the graphics hardware directly.
    /// </summary>
    internal class Gdi32
    {
        [DllImport("gdi32.dll", EntryPoint = "DeleteObject", CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteObject([In] IntPtr hObject);
    }
}
