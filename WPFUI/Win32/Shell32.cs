// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace WPFUI.Win32
{
    /// <summary>
    /// Shell32 Namespace
    /// </summary>
    internal static class Shell32
    {
        /// <summary>
        /// Contains information that the system needs to display notifications in the notification area. Used by <see cref="Shell_NotifyIcon"/>.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public class NOTIFYICONDATA
        {
            public int cbSize = Marshal.SizeOf(typeof(NOTIFYICONDATA));
            public IntPtr hWnd;
            public int uID;
            public UFlags uFlags;
            public int uCallbackMessage;
            public IntPtr hIcon;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x80)] public string szTip;
            public int dwState;
            public int dwStateMask;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x100)] public string szInfo;
            public int uTimeoutOrVersion;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x40)] public string szInfoTitle;
            public int dwInfoFlags;
        }

        /// <summary>
        /// Sends a message to the taskbar's status area.
        /// </summary>
        [SecurityCritical, DllImport("shell32.dll", CharSet = CharSet.Auto)]
        public static extern int Shell_NotifyIcon(int message, NOTIFYICONDATA pnid);
    }
}
