// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;

namespace WPFUI.Background
{
    public class Acrylic
    {
        public static void Apply(object element)
        {
            // TODO: Implement acrylic
        }

        /// <summary>
        /// Checks if the current operating system supports Acrylic background.
        /// </summary>
        /// <returns><see langword="true"/> if Windows 11 or above.</returns>
        public static bool IsSupported()
        {
            return Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version.Build > 18000;
        }
    }
}
