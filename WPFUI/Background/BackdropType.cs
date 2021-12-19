// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

namespace WPFUI.Background
{
    /// <summary>
    /// Collection of backdrop types.
    /// </summary>
    internal enum BackdropType
    {
        /// <summary>
        /// Automatically selects backdrop effect.
        /// </summary>
        DWMSBT_AUTO = 0,

        /// <summary>
        /// Turns off the backdrop effect.
        /// </summary>
        DWMSBT_DISABLE = 1,

        /// <summary>
        /// Sets Mica effect with generated wallpaper tint.
        /// </summary>
        DWMSBT_MAINWINDOW = 2,

        /// <summary>
        /// Sets acrlic effect.
        /// </summary>
        DWMSBT_TRANSIENTWINDOW = 3,

        /// <summary>
        /// Sets blurred wallpaper effect, like Mica without tint.
        /// </summary>
        DWMSBT_TABBEDWINDOW = 4
    }
}
