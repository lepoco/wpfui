// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Runtime.InteropServices;

namespace WPFUI.Interop;

/// <summary>
/// Windows kernel module.
/// </summary>
internal class Kernel32
{
    /// <summary>
    /// Copies a block of memory from one location to another.
    /// </summary>
    [DllImport("kernel32.dll", EntryPoint = "CopyMemory", SetLastError = false, CharSet = CharSet.Auto)]
    public static extern void CopyMemory(IntPtr dest, IntPtr src, uint count);
}
