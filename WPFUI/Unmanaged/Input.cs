// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

namespace WPFUI.Unmanaged
{
    /// <summary>
    /// Mouse Input Notifications
    /// </summary>
    internal enum Input
    {
        /// <summary>
        /// Sent to a window in order to determine what part of the window corresponds to a particular screen coordinate. This can happen, for example, when the cursor moves, when a mouse button is pressed or released, or in response to a call to a function such as WindowFromPoint. If the mouse is not captured, the message is sent to the window beneath the cursor. Otherwise, the message is sent to the window that has captured the mouse.
        /// </summary>
        WM_NCHITTEST = 0x0084,

        /// <summary>
        /// Posted when the user presses the left mouse button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor.
        /// </summary>
        WM_LBUTTONDOWN = 0x0201,

        /// <summary>
        /// Posted to a window when the cursor hovers over the client area of the window for the period of time specified in a prior call to TrackMouseEvent.
        /// </summary>
        WM_MOUSEHOVER = 0x02A1,

        /// <summary>
        /// Posted to a window when the cursor leaves the client area of the window specified in a prior call to TrackMouseEvent.
        /// </summary>
        WM_MOUSELEAVE = 0x02A3,

        /// <summary>
        /// Posted to a window when the cursor is moved within the nonclient area of the window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.
        /// </summary>
        WM_NCMOUSEMOVE = 0x00A0,

        /// <summary>
        /// Posted to a window when the cursor leaves the nonclient area of the window specified in a prior call to TrackMouseEvent.
        /// </summary>
        WM_NCMOUSELEAVE = 0x02A2,

        /// <summary>
        /// Posted when the user presses the left mouse button while the cursor is within the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.
        /// </summary>
        WM_NCLBUTTONDOWN = 0x00A1
    }
}
