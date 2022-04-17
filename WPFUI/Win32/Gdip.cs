// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Runtime.InteropServices;

namespace WPFUI.Win32;

/// <summary>
/// Windows GDI+ exposes a flat API that consists of about 600 functions, which are implemented in Gdiplus.dll and declared in Gdiplusflat.h.
/// </summary>
internal class Gdip
{
    [DllImport("gdiplus.dll", CharSet = CharSet.Auto)]
    public static extern int GdipCreateHICONFromBitmap(HandleRef nativeBitmap, out IntPtr hicon);
}

