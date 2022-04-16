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

        [Flags]
        /// <summary>
        /// A value that specifies the action to be taken by this function. NIM_*
        /// </summary>
        public enum NIM : uint
        {
            /// <summary>
            /// Adds an icon to the status area.The icon is given an identifier in the NOTIFYICONDATA structure pointed to by lpdata—either through its uID or guidItem member.This identifier is used in subsequent calls to Shell_NotifyIcon to perform later actions on the icon.
            /// </summary>
            ADD = 0x00000000,

            /// <summary>
            /// Modifies an icon in the status area.NOTIFYICONDATA structure pointed to by lpdata uses the ID originally assigned to the icon when it was added to the notification area(NIM_ADD) to identify the icon to be modified.
            /// </summary>
            MODIFY = 0x00000001,

            /// <summary>
            /// Deletes an icon from the status area.NOTIFYICONDATA structure pointed to by lpdata uses the ID originally assigned to the icon when it was added to the notification area(NIM_ADD) to identify the icon to be deleted.
            /// </summary>
            DELETE = 0x00000002,

            /// <summary>
            /// Shell32.dll version 5.0 and later only.Returns focus to the taskbar notification area.Notification area icons should use this message when they have completed their UI operation. For example, if the icon displays a shortcut menu, but the user presses ESC to cancel it, use NIM_SETFOCUS to return focus to the notification area.
            /// </summary>
            SETFOCUS = 0x00000003,

            /// <summary>
            /// Shell32.dll version 5.0 and later only. Instructs the notification area to behave according to the version number specified in the uVersion member of the structure pointed to by lpdata. The version number specifies which members are recognized.
            /// <para>NIM_SETVERSION must be called every time a notification area icon is added (NIM_ADD). It does not need to be called with NIM_MODIFY. The version setting is not persisted once a user logs off.</para>
            /// </summary>
            SETVERSION = 0x00000004

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
            /// <summary>
            /// The size of this structure, in bytes.
            /// </summary>
            public int cbSize = Marshal.SizeOf(typeof(NOTIFYICONDATA));

            /// <summary>
            /// A handle to the window that receives notifications associated with an icon in the notification area.
            /// </summary>
            public IntPtr hWnd;

            /// <summary>
            /// The application-defined identifier of the taskbar icon. The Shell uses either (hWnd plus uID) or guidItem to identify which icon to operate on when Shell_NotifyIcon is invoked.
            /// You can have multiple icons associated with a single hWnd by assigning each a different uID. If guidItem is specified, uID is ignored.
            /// </summary>
            public int uID;

            /// <summary>
            /// Flags that either indicate which of the other members of the structure contain valid data or provide additional information to the tooltip as to how it should display.
            /// </summary>
            public UFlags uFlags;

            /// <summary>
            /// 0x00000001. The uCallbackMessage member is valid.
            /// </summary>
            public int uCallbackMessage;

            /// <summary>
            /// 0x00000002. The hIcon member is valid.
            /// </summary>
            public IntPtr hIcon;

            /// <summary>
            /// 0x00000004. The szTip member is valid.
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x80)] // 128
            public string szTip;

            public int dwState;

            public int dwStateMask;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x100)] // 256
            public string szInfo;

            public int uTimeoutOrVersion;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x40)] // 64
            public string szInfoTitle;

            public int dwInfoFlags;
        }

        /// <summary>
        /// Sends a message to the taskbar's status area.
        /// </summary>
        [SecurityCritical, DllImport("shell32.dll", CharSet = CharSet.Auto)]
        public static extern int Shell_NotifyIcon(NIM message, NOTIFYICONDATA pnid);
    }
}
