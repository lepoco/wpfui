// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

namespace WPFUI.Win32;

/// <summary>
/// The return value of the DefWindowProc function is one of the following values, indicating the position of the cursor hot spot.
/// <para><see href="https://docs.microsoft.com/en-us/windows/win32/inputdev/wm-nchittest"/></para>
/// </summary>
internal enum HT
{
    /// <summary>
    /// On the screen background or on a dividing line between windows.
    /// </summary>
    NOWHERE = 0,

    /// <summary>
    /// In a client area.
    /// </summary>
    CLIENT = 1,

    /// <summary>
    /// In a title bar.
    /// </summary>
    CAPTION = 2,

    /// <summary>
    /// In a window menu or in a Close button in a child window.
    /// </summary>
    SYSMENU = 3,

    /// <summary>
    /// In a size box (same as HTSIZE).
    /// </summary>
    GROWBOX = 4,
    //SIZE = 4,

    /// <summary>
    /// In a menu.
    /// </summary>
    MENU = 5,

    /// <summary>
    /// In a horizontal scroll bar.
    /// </summary>
    HSCROLL = 6,

    /// <summary>
    /// In the vertical scroll bar.
    /// </summary>
    VSCROLL = 7,

    /// <summary>
    /// In a Minimize button.
    /// </summary>
    MINBUTTON = 8,

    /// <summary>
    /// In a Maximize button.
    /// </summary>
    MAXBUTTON = 9,
    // ZOOM = 9,

    /// <summary>
    /// In the left border of a resizable window (the user can click the mouse to resize the window horizontally).
    /// </summary>
    LEFT = 10,

    /// <summary>
    /// In the right border of a resizable window (the user can click the mouse to resize the window horizontally).
    /// </summary>
    RIGHT = 11,

    /// <summary>
    /// In the upper-horizontal border of a window.
    /// </summary>
    TOP = 12,
}
