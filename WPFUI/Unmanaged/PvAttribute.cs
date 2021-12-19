// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

namespace WPFUI.Unmanaged
{
    /// <summary>
    /// Abstraction of pointer to an object containing the attribute value to set. The type of the value set depends on the value of the dwAttribute parameter.
    /// The DWMWINDOWATTRIBUTE enumeration topic indicates, in the row for each flag, what type of value you should pass a pointer to in the pvAttribute parameter.
    /// </summary>
    internal enum PvAttribute
    {
        /// <summary>
        /// Object containing the <see langowrd="false"/> attribute value to set in dwmapi.h. 
        /// </summary>
        Disable = 0x00,

        /// <summary>
        /// Object containing the <see langowrd="true"/> attribute value to set in dwmapi.h. 
        /// </summary>
        Enable = 0x01
    }
}
