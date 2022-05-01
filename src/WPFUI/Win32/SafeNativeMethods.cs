// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace WPFUI.Win32;

internal class SafeNativeMethods
{
    public static bool IsUxThemeActive()
    {
        // TODO:
        //return SafeNativeMethodsPrivate.IsThemeActive() != 0;
        return true;
    }
}
