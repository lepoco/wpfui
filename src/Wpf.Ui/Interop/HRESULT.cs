// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Runtime.InteropServices;

namespace Wpf.Ui.Interop;

/// <summary>
/// Common Windows API result;
/// </summary>
internal struct HRESULT
{
    ///<summary>
    /// Operation successful.
    ///</summary>
    public const int S_OK = unchecked((int)0x00000000);

    ///<summary>
    /// Operation successful.
    ///</summary>
    public const int NO_ERROR = unchecked((int)0x00000000);

    ///<summary>
    /// Operation successful.
    ///</summary>
    public const int NOERROR = unchecked((int)0x00000000);

    ///<summary>
    /// Unspecified failure.
    ///</summary>
    public const int S_FALSE = unchecked((int)0x00000001);

    public static void Check(int hr)
    {
        if (hr >= S_OK)
            return;

        Marshal.ThrowExceptionForHR(hr, (IntPtr)(-1));
    }
}
