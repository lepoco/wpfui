// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Windows.Win32.Graphics.Dwm;

namespace Wpf.Ui.Extensions;

internal static class PInvokeExtensions
{
    extension(DWMWINDOWATTRIBUTE str)
    {
        /// <summary>
        /// Undocumented attribute for enabling Mica effect on a window.
        /// </summary>
        public static DWMWINDOWATTRIBUTE DWMWA_MICA_EFFECT => (DWMWINDOWATTRIBUTE)1029;
    }
}