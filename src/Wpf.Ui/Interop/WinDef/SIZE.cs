// This Source Code is partially based on reverse engineering of the Windows Operating System,
// and is intended for use on Windows systems only.
// This Source Code is partially based on the source code provided by the .NET Foundation.
// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski.
// All Rights Reserved.

using System.Runtime.InteropServices;

namespace Wpf.Ui.Interop.WinDef;

/// <summary>
/// The SIZE structure defines the width and height of a rectangle.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
// ReSharper disable InconsistentNaming
public struct SIZE
{
    /// <summary>
    /// Specifies the rectangle's width. The units depend on which function uses this structure.
    /// </summary>
    public long cx;

    /// <summary>
    /// Specifies the rectangle's height. The units depend on which function uses this structure.
    /// </summary>
    public long cy;
}
