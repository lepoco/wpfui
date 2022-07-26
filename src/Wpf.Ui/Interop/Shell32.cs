// This Source Code is partially based on reverse engineering of the Windows Operating System,
// and is intended for use on Windows systems only.
// This Source Code is partially based on the source code provided by the .NET Foundation.
// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski.
// All Rights Reserved.

// NOTE
// I split unmanaged code stuff into the NativeMethods library.
// If you have suggestions for the code below, please submit your changes there.
// https://github.com/lepoco/nativemethods

using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace Wpf.Ui.Interop;

/// <summary>
/// The Windows UI provides users with access to a wide variety of objects necessary to run applications and manage the operating system.
/// </summary>
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
internal static class Shell32
{
    /// <summary>
    /// DATAOBJ_GET_ITEM_FLAGS.  DOGIF_*.
    /// </summary>
    public enum DOGIF
    {
        DEFAULT = 0x0000,
        TRAVERSE_LINK = 0x0001, // if the item is a link get the target
        NO_HDROP = 0x0002, // don't fallback and use CF_HDROP clipboard format
        NO_URL = 0x0004, // don't fallback and use URL clipboard format
        ONLY_IF_ONE = 0x0008, // only return the item if there is one item in the array
    }

    /// <summary>
    /// Shell_NotifyIcon messages.  NIM_*
    /// </summary>
    public enum NIM : uint
    {
        ADD = 0,
        MODIFY = 1,
        DELETE = 2,
        SETFOCUS = 3,
        SETVERSION = 4,
    }

    /// <summary>
    /// Shell_NotifyIcon flags.  NIF_*
    /// </summary>
    [Flags]
    public enum NIF : uint
    {
        MESSAGE = 0x0001,
        ICON = 0x0002,
        TIP = 0x0004,
        STATE = 0x0008,
        INFO = 0x0010,
        GUID = 0x0020,

        /// <summary>
        /// Vista only.
        /// </summary>
        REALTIME = 0x0040,

        /// <summary>
        /// Vista only.
        /// </summary>
        SHOWTIP = 0x0080,

        XP_MASK = MESSAGE | ICON | STATE | INFO | GUID,
        VISTA_MASK = XP_MASK | REALTIME | SHOWTIP,
    }

    [StructLayout(LayoutKind.Sequential)]
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
        public NIF uFlags;

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

        /// <summary>
        /// The state of the icon.  There are two flags that can be set independently.
        /// NIS_HIDDEN = 1.  The icon is hidden.
        /// NIS_SHAREDICON = 2.  The icon is shared.
        /// </summary>
        public uint dwState;

        public uint dwStateMask;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x100)] // 256
        public string szInfo;

        /// <summary>
        /// Prior to Vista this was a union of uTimeout and uVersion.  As of Vista, uTimeout has been deprecated.
        /// </summary>
        public uint uVersion; // Used with Shell_NotifyIcon flag NIM_SETVERSION.

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x40)] // 64
        public string szInfoTitle;

        public uint dwInfoFlags;

        public Guid guidItem;

        // Vista only
        IntPtr hBalloonIcon;
    }

    [DllImport(Libraries.Shell32, PreserveSig = false)]
    public static extern void SHGetItemFromDataObject(IDataObject pdtobj, DOGIF dwFlags, [In] ref Guid riid,
        [Out, MarshalAs(UnmanagedType.Interface)]
        out object ppv);

    [DllImport(Libraries.Shell32)]
    public static extern int SHCreateItemFromParsingName([MarshalAs(UnmanagedType.LPWStr)] string pszPath, IBindCtx pbc,
        [In] ref Guid riid, [Out, MarshalAs(UnmanagedType.Interface)] out object ppv);

    [DllImport(Libraries.Shell32)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool Shell_NotifyIcon([In] NIM dwMessage, [In] NOTIFYICONDATA lpdata);

    /// <summary>
    /// Sets the User Model AppID for the current process, enabling Windows to retrieve this ID
    /// </summary>
    /// <param name="AppID"></param>
    [DllImport(Libraries.Shell32, PreserveSig = false)]
    public static extern void SetCurrentProcessExplicitAppUserModelID([MarshalAs(UnmanagedType.LPWStr)] string AppID);

    /// <summary>
    /// Retrieves the User Model AppID that has been explicitly set for the current process via SetCurrentProcessExplicitAppUserModelID
    /// </summary>
    /// <param name="AppID"></param>
    [DllImport(Libraries.Shell32)]
    public static extern int GetCurrentProcessExplicitAppUserModelID(
        [Out, MarshalAs(UnmanagedType.LPWStr)] out string AppID);
}
