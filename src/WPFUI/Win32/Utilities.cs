// This Source Code is partially based on the source code provided by the .NET Foundation.
// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski.
// All Rights Reserved.

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace WPFUI.Win32;

/// <summary>
/// Common Window utilities.
/// </summary>
// ReSharper disable InconsistentNaming
internal class Utilities
{
    private static readonly PlatformID _osPlatform = Environment.OSVersion.Platform;

    private static readonly Version _osVersion = Environment.OSVersion.Version;

    /// <summary>
    /// Whether the operating system is NT or newer. 
    /// </summary>
    public static bool IsNT => _osPlatform == PlatformID.Win32NT;

    /// <summary>
    /// Whether the operating system version is greater than or equal to 6.0.
    /// </summary>
    public static bool IsOSVistaOrNewer => _osVersion >= new Version(6, 0);

    /// <summary>
    /// Whether the operating system version is greater than or equal to 6.1.
    /// </summary>
    public static bool IsOSWindows7OrNewer => _osVersion >= new Version(6, 1);

    /// <summary>
    /// Whether the operating system version is greater than or equal to 6.2.
    /// </summary>
    public static bool IsOSWindows8OrNewer => _osVersion >= new Version(6, 2);

    /// <summary>
    /// Whether the operating system version is greater than or equal to 10.0* (build 10240).
    /// </summary>
    public static bool IsOSWindows10OrNewer => _osVersion.Build >= 10240;

    /// <summary>
    /// Whether the operating system version is greater than or equal to 10.0* (build 22000).
    /// </summary>
    public static bool IsOSWindows11OrNewer => _osVersion.Build >= 22000;

    /// <summary>
    /// Whether the operating system version is greater than or equal to 10.0* (build 22523).
    /// </summary>
    public static bool IsOSWindows11Insider1OrNewer => _osVersion.Build >= 22523;

    /// <summary>
    /// Whether the operating system version is greater than or equal to 10.0* (build 22557).
    /// </summary>
    public static bool IsOSWindows11Insider2OrNewer => _osVersion.Build >= 22557;

    /// <summary>
    /// Indicates whether Desktop Window Manager (DWM) composition is enabled.
    /// </summary>
    public static bool IsCompositionEnabled
    {
        get
        {
            if (!IsOSVistaOrNewer)
                return false;

            Interop.Dwmapi.DwmIsCompositionEnabled(out var pfEnabled);

            return pfEnabled != 0;
        }
    }

    public static void SafeDispose<T>(ref T disposable) where T : IDisposable
    {
        // Dispose can safely be called on an object multiple times.
        IDisposable t = disposable;
        disposable = default(T);

        if (null == t)
            return;

        t.Dispose();
    }

    public static void SafeRelease<T>(ref T comObject) where T : class
    {
        T t = comObject;
        comObject = default(T);

        if (null == t)
            return;

        Debug.Assert(Marshal.IsComObject(t));
        Marshal.ReleaseComObject(t);
    }
}
