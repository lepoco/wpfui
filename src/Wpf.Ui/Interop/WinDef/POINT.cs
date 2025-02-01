// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

/* This Source Code is partially based on reverse engineering of the Windows Operating System,
   and is intended for use on Windows systems only.
   This Source Code is partially based on the source code provided by the .NET Foundation. */

using System.Runtime.InteropServices;

namespace Wpf.Ui.Interop.WinDef;

// ReSharper disable InconsistentNaming
#pragma warning disable SA1307 // Accessible fields should begin with upper-case letter

/// <summary>
/// The POINT structure defines the x- and y-coordinates of a point.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct POINT
{
    /// <summary>
    /// Specifies the x-coordinate of the point.
    /// </summary>
    public int x;

    /// <summary>
    /// Specifies the y-coordinate of the point.
    /// </summary>
    public int y;
}

#pragma warning restore SA1307 // Accessible fields should begin with upper-case letter
