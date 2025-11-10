// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Windows.Win32.Graphics.Dwm;

namespace Wpf.Ui.Extensions;

internal static class PInvokeExtensions
{
    extension(DWMWINDOWATTRIBUTE attr)
    {
        /// <summary>
        /// Gets the undocumented window attribute for enabling Mica effect on a window.
        /// </summary>
        public static DWMWINDOWATTRIBUTE DWMWA_MICA_EFFECT => (DWMWINDOWATTRIBUTE)1029;

        /// <summary>
        /// Gets the window attribute used to enable immersive dark mode prior to Windows 11.
        /// </summary>
        public static DWMWINDOWATTRIBUTE DMWA_USE_IMMERSIVE_DARK_MODE_OLD => DWMWINDOWATTRIBUTE.DWMWA_USE_IMMERSIVE_DARK_MODE - 1;
    }
}