// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;

namespace WPFUI.Common;

/// <summary>
/// Facilitates the management of support for the functionality of the operating system.
/// </summary>
internal class Windows
{
    /// <summary>
    /// Indicates whether the operating system is of this version or higher.
    /// </summary>
    public static bool Is(WindowsRelease release)
    {
        return Environment.OSVersion.Version.Build >= (int)release;
    }

    /// <summary>
    /// Indicates whether the operating system is lower than the specified version.
    /// </summary>
    public static bool IsBelow(WindowsRelease release)
    {
        return Environment.OSVersion.Version.Build < (int)release;
    }

    /// <summary>
    /// Indicates whether the operating system is NT or newer.
    /// </summary>
    public static bool IsNt()
    {
        return Environment.OSVersion.Platform == PlatformID.Win32NT;
    }
}
