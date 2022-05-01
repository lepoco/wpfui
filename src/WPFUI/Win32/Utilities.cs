// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace WPFUI.Win32;

internal class Utilities
{
    private static readonly Version _osVersion = Environment.OSVersion.Version;

    internal static bool IsOSVistaOrNewer
    {
        get { return _osVersion >= new Version(6, 0); }
    }

    internal static bool IsOSWindows7OrNewer
    {
        get { return _osVersion >= new Version(6, 1); }
    }

    internal static bool IsOSWindows8OrNewer
    {
        get { return _osVersion >= new Version(6, 2); }
    }

    internal static bool IsCompositionEnabled
    {
        get
        {
            throw new NotImplementedException();
            //if (!IsOSVistaOrNewer)
            //{
            //    return false;
            //}

            //Int32 isDesktopCompositionEnabled = 0;
            //UnsafeNativeMethods.HRESULT.Check(UnsafeNativeMethods.DwmIsCompositionEnabled(out isDesktopCompositionEnabled));
            //return isDesktopCompositionEnabled != 0;
        }
    }

    internal static void SafeDispose<T>(ref T disposable) where T : IDisposable
    {
        // Dispose can safely be called on an object multiple times.
        IDisposable t = disposable;
        disposable = default(T);
        if (null != t)
        {
            t.Dispose();
        }
    }

    internal static void SafeRelease<T>(ref T comObject) where T : class
    {
        T t = comObject;
        comObject = default(T);
        if (null != t)
        {
            Debug.Assert(Marshal.IsComObject(t));
            Marshal.ReleaseComObject(t);
        }
    }
}
