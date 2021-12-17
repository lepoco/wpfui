// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Runtime.InteropServices;

namespace WPFUI.Unmanaged
{
    /// <summary>
    /// Used by Desktop Window Manager (DWM)
    /// </summary>
    internal class Dwmapi
    {
        [DllImport("dwmapi.dll")]
        public static extern int DwmSetWindowAttribute(IntPtr hwnd, DWMWINDOWATTRIBUTE dwAttribute, ref int pvAttribute,
            int cbAttribute);
    }
}
