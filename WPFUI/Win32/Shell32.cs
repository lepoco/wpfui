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
        /// Flags for SetTabProperties.  STPF_*
        /// </summary>
        /// <remarks>The native enum was called STPFLAG.</remarks>
        [Flags]
        public enum STPF
        {
            NONE = 0x00000000,
            USEAPPTHUMBNAILALWAYS = 0x00000001,
            USEAPPTHUMBNAILWHENACTIVE = 0x00000002,
            USEAPPPEEKALWAYS = 0x00000004,
            USEAPPPEEKWHENACTIVE = 0x00000008,
        }

        /// <summary>
        /// THUMBBUTTON mask.  THB_*
        /// </summary>
        [Flags]
        public enum THB : uint
        {
            BITMAP = 0x0001,
            ICON = 0x0002,
            TOOLTIP = 0x0004,
            FLAGS = 0x0008,
        }

        /// <summary>
        /// THUMBBUTTON flags.  THBF_*
        /// </summary>
        [Flags]
        public enum THBF : uint
        {
            ENABLED = 0x0000,
            DISABLED = 0x0001,
            DISMISSONCLICK = 0x0002,
            NOBACKGROUND = 0x0004,
            HIDDEN = 0x0008,
            // Added post-beta
            NONINTERACTIVE = 0x0010,
        }

        [StructLayout(LayoutKind.Sequential, Pack = 8, CharSet = CharSet.Unicode)]
        internal struct THUMBBUTTON
        {
            /// <summary>
            /// WPARAM value for a THUMBBUTTON being clicked.
            /// </summary>
            public const int THBN_CLICKED = 0x1800;

            public THB dwMask;
            public uint iId;
            public uint iBitmap;
            public IntPtr hIcon;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szTip;
            public THBF dwFlags;
        }

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
