// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

namespace WPFUI.Unmanaged
{
    /// <summary>
    /// The following are the window styles. After the window has been created, these styles cannot be modified, except as noted.
    /// </summary>
    internal enum WStyles
    {
        /// <summary>
        /// The window has a maximize button. Cannot be combined with the WS_EX_CONTEXTHELP style. The WS_SYSMENU style must also be specified.
        /// </summary>
        WS_MAXIMIZEBOX = 0x10000,

        /// <summary>
        /// The window has a minimize button. Cannot be combined with the WS_EX_CONTEXTHELP style. The WS_SYSMENU style must also be specified.
        /// </summary>
        WS_MINIMIZEBOX = 0x20000,

        /// <summary>
        /// The window is an overlapped window. Same as the WS_TILEDWINDOW style.
        /// </summary>
        WS_SIZEBOX = 0x40000,

        /// <summary>
        /// The window has a window menu on its title bar. The WS_CAPTION style must also be specified.
        /// </summary>
        WS_SYSMENU = 0x80000
    }
}
