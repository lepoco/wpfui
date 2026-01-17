// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Runtime.InteropServices;
using Windows.Win32.UI.WindowsAndMessaging;

namespace Windows.Win32;

internal static partial class PInvoke
{
    [
        DllImport("USER32.dll", ExactSpelling = true, EntryPoint = "SetWindowLongPtrW", SetLastError = true),
        DefaultDllImportSearchPaths(DllImportSearchPath.System32)
    ]
    internal static extern nint SetWindowLongPtr(Windows.Win32.Foundation.HWND hWnd, WINDOW_LONG_PTR_INDEX nIndex, nint dwNewLong);

    [
        DllImport("USER32.dll", ExactSpelling = true, EntryPoint = "GetCursorPos", SetLastError = true),
        DefaultDllImportSearchPaths(DllImportSearchPath.System32)
    ]
    internal static extern bool GetCursorPos(out POINT lpPoint);
}

[StructLayout(LayoutKind.Sequential)]
internal struct POINT
{
    public int X;
    public int Y;
}
