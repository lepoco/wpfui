// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Runtime.InteropServices.ComTypes;

namespace Wpf.Ui.Tray.Interop;

// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
#pragma warning disable SA1307 // Accessible fields should begin with upper-case letter
#pragma warning disable SA1401 // Fields should be private
#pragma warning disable CA1060 // Move pinvokes to native methods class

/// <summary>
/// The Windows UI provides users with access to a wide variety of objects necessary to run applications and manage the operating system.
/// </summary>
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

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public class NOTIFYICONDATA
    {
        /// <summary>
        /// <para>Type: <b>DWORD</b> The size of this structure, in bytes.</para>
        /// <para><see href="https://learn.microsoft.com/windows/win32/api/shellapi/ns-shellapi-notifyicondataw#members">Read more on learn.microsoft.com</see>.</para>
        /// </summary>
        public int cbSize = Marshal.SizeOf(typeof(NOTIFYICONDATA));

        /// <summary>
        /// <para>Type: <b>HWND</b> A handle to the window that receives notifications associated with an icon in the notification area.</para>
        /// <para><see href="https://learn.microsoft.com/windows/win32/api/shellapi/ns-shellapi-notifyicondataw#members">Read more on learn.microsoft.com</see>.</para>
        /// </summary>
        public IntPtr hWnd;

        /// <summary>
        /// <para>Type: <b>UINT</b> The application-defined identifier of the taskbar icon. The Shell uses either (<b>hWnd</b> plus <b>uID</b>) or <b>guidItem</b> to identify which icon to operate on when <a href="https://docs.microsoft.com/windows/desktop/api/shellapi/nf-shellapi-shell_notifyicona">Shell_NotifyIcon</a> is invoked. You can have multiple icons associated with a single <b>hWnd</b> by assigning each a different <b>uID</b>. If <b>guidItem</b> is specified, <b>uID</b> is ignored.</para>
        /// <para><see href="https://learn.microsoft.com/windows/win32/api/shellapi/ns-shellapi-notifyicondataw#members">Read more on learn.microsoft.com</see>.</para>
        /// </summary>
        public int uID;

        /// <summary>
        /// Flags that either indicate which of the other members of the structure contain valid data or provide additional information to the tooltip as to how it should display.
        /// </summary>
        public NIF uFlags;

        /// <summary>
        /// <para>Type: <b>UINT</b> An application-defined message identifier. The system uses this identifier to send notification messages to the window identified in <b>hWnd</b>. These notification messages are sent when a mouse event or hover occurs in the bounding rectangle of the icon, when the icon is selected or activated with the keyboard, or when those actions occur in the balloon notification.</para>
        /// <para>When the <b>uVersion</b> member is either 0 or NOTIFYICON_VERSION, the <i>wParam</i> parameter of the message contains the identifier of the taskbar icon in which the event occurred. This identifier can be 32 bits in length. The <i>lParam</i> parameter holds the mouse or keyboard message associated with the event. For example, when the pointer moves over a taskbar icon, <i>lParam</i> is set to <a href="https://docs.microsoft.com/windows/desktop/inputdev/wm-mousemove">WM_MOUSEMOVE</a>. When the <b>uVersion</b> member is NOTIFYICON_VERSION_4, applications continue to receive notification events in the form of application-defined messages through the <b>uCallbackMessage</b> member, but the interpretation of the <i>lParam</i> and <i>wParam</i> parameters of that message is changed as follows: </para>
        /// <para>This doc was truncated.</para>
        /// <para><see href="https://learn.microsoft.com/windows/win32/api/shellapi/ns-shellapi-notifyicondataw#members">Read more on learn.microsoft.com</see>.</para>
        /// </summary>
        public int uCallbackMessage;

        /// <summary>
        /// <para>Type: <b>HICON</b> A handle to the icon to be added, modified, or deleted. Windows XP and later support icons of up to 32 BPP. If only a 16x16 pixel icon is provided, it is scaled to a larger size in a system set to a high dpi value. This can lead to an unattractive result. It is recommended that you provide both a 16x16 pixel icon and a 32x32 icon in your resource file. Use <a href="https://docs.microsoft.com/windows/desktop/api/commctrl/nf-commctrl-loadiconmetric">LoadIconMetric</a> to ensure that the correct icon is loaded and scaled appropriately. See Remarks for a code example.</para>
        /// <para><see href="https://learn.microsoft.com/windows/win32/api/shellapi/ns-shellapi-notifyicondataw#members">Read more on learn.microsoft.com</see>.</para>
        /// </summary>
        public IntPtr hIcon;

        /// <summary>
        /// <para>Type: <b>TCHAR[64]</b> A null-terminated string that specifies the text for a standard tooltip. It can have a maximum of 64 characters, including the terminating null character.</para>
        /// <para>For Windows 2000 and later, <b>szTip</b> can have a maximum of 128 characters, including the terminating null character.</para>
        /// <para><see href="https://learn.microsoft.com/windows/win32/api/shellapi/ns-shellapi-notifyicondataw#members">Read more on learn.microsoft.com</see>.</para>
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x80)] // 128
        public string? szTip;

        /// <summary>
        /// The state of the icon.  There are two flags that can be set independently.
        /// NIS_HIDDEN = 1.  The icon is hidden.
        /// NIS_SHAREDICON = 2.  The icon is shared.
        /// </summary>
        public uint dwState;

        public uint dwStateMask;

        /// <summary>
        /// <para>Type: <b>TCHAR[256]</b> <b>Windows 2000 and later</b>. A null-terminated string that specifies the text to display in a balloon notification. It can have a maximum of 256 characters, including the terminating null character, but should be restricted to 200 characters in English to accommodate localization. To remove the balloon notification from the UI, either delete the icon (with <a href="https://docs.microsoft.com/windows/desktop/api/shellapi/nf-shellapi-shell_notifyicona">NIM_DELETE</a>) or set the <b>NIF_INFO</b> flag in <b>uFlags</b> and set <b>szInfo</b> to an empty string.</para>
        /// <para><see href="https://learn.microsoft.com/windows/win32/api/shellapi/ns-shellapi-notifyicondataw#members">Read more on learn.microsoft.com</see>.</para>
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x100)] // 256
        public string? szInfo;

        /// <summary>
        /// Prior to Vista this was a union of uTimeout and uVersion.  As of Vista, uTimeout has been deprecated.
        /// </summary>
        public uint uVersion; // Used with Shell_NotifyIcon flag NIM_SETVERSION.

        /// <summary>
        /// <para>Type: <b>TCHAR[64]</b> <b>Windows 2000 and later</b>. A null-terminated string that specifies a title for a balloon notification. This title appears in a larger font immediately above the text. It can have a maximum of 64 characters, including the terminating null character, but should be restricted to 48 characters in English to accommodate localization.</para>
        /// <para><see href="https://learn.microsoft.com/windows/win32/api/shellapi/ns-shellapi-notifyicondataw#members">Read more on learn.microsoft.com</see>.</para>
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x40)] // 64
        public string? szInfoTitle;

        /// <summary>
        /// <para>Type: <b>DWORD</b> <b>Windows 2000 and later</b>. Flags that can be set to modify the behavior and appearance of a balloon notification. The icon is placed to the left of the title. If the <b>szInfoTitle</b> member is zero-length, the icon is not shown.</para>
        /// <para><see href="https://learn.microsoft.com/windows/win32/api/shellapi/ns-shellapi-notifyicondataw#members">Read more on learn.microsoft.com</see>.</para>
        /// </summary>
        public uint dwInfoFlags;

        /// <summary>
        /// <para>Type: <b>GUID</b> <b>Windows XP and later</b>.</para>
        /// <para></para>
        /// <para>This doc was truncated.</para>
        /// <para><see href="https://learn.microsoft.com/windows/win32/api/shellapi/ns-shellapi-notifyicondataw#members">Read more on learn.microsoft.com</see>.</para>
        /// </summary>
        public Guid guidItem;

        /// <summary>
        /// <para>Type: <b>HICON</b> <b>Windows Vista and later</b>. The handle of a customized notification icon provided by the application that should be used independently of the notification area icon. If this member is non-NULL and the NIIF_USER flag is set in the <b>dwInfoFlags</b> member, this icon is used as the notification icon. If this member is <b>NULL</b>, the legacy behavior is carried out.</para>
        /// <para><see href="https://learn.microsoft.com/windows/win32/api/shellapi/ns-shellapi-notifyicondataw#members">Read more on learn.microsoft.com</see>.</para>
        /// </summary>
        public IntPtr hBalloonIcon;
    }

    [DllImport(Libraries.Shell32, PreserveSig = false)]
    public static extern void SHGetItemFromDataObject(
        IDataObject pdtobj,
        DOGIF dwFlags,
        [In] ref Guid riid,
        [Out, MarshalAs(UnmanagedType.Interface)] out object ppv
    );

    [DllImport(Libraries.Shell32)]
    public static extern int SHCreateItemFromParsingName(
        [MarshalAs(UnmanagedType.LPWStr)] string pszPath,
        IBindCtx pbc,
        [In] ref Guid riid,
        [Out, MarshalAs(UnmanagedType.Interface)] out object ppv
    );

    [DllImport(Libraries.Shell32, CharSet = CharSet.Unicode)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool Shell_NotifyIcon([In] NIM dwMessage, [In] NOTIFYICONDATA lpdata);

    /// <summary>
    /// Sets the User Model AppID for the current process, enabling Windows to retrieve this ID
    /// </summary>
    /// <param name="AppID">The string ID to be assigned</param>
    [DllImport(Libraries.Shell32, PreserveSig = false)]
    public static extern void SetCurrentProcessExplicitAppUserModelID(
        [MarshalAs(UnmanagedType.LPWStr)] string AppID
    );

    /// <summary>
    /// Retrieves the User Model AppID that has been explicitly set for the current process via SetCurrentProcessExplicitAppUserModelID
    /// </summary>
    /// <param name="AppID">Out parameter that receives the string ID.</param>
    /// <returns>An HRESULT indicating success (S_OK) or failure of the operation. If the function fails, the returned AppID is null.</returns>
    [DllImport(Libraries.Shell32)]
    public static extern int GetCurrentProcessExplicitAppUserModelID(
        [Out, MarshalAs(UnmanagedType.LPWStr)] out string AppID
    );

    /// <summary>Destroys an icon and frees any memory the icon occupied.</summary>
    /// <param name="hIcon">
    /// <para>Type: <b>HICON</b> A handle to the icon to be destroyed. The icon must not be in use.</para>
    /// <para><see href="https://learn.microsoft.com/windows/win32/api/winuser/nf-winuser-destroyicon#parameters">Read more on learn.microsoft.com</see>.</para>
    /// </param>
    /// <returns>
    /// <para>Type: <b>BOOL</b> If the function succeeds, the return value is nonzero. If the function fails, the return value is zero. To get extended error information, call <a href="https://docs.microsoft.com/windows/desktop/api/errhandlingapi/nf-errhandlingapi-getlasterror">GetLastError</a>.</para>
    /// </returns>
    /// <remarks>
    /// <para>It is only necessary to call <b>DestroyIcon</b> for icons and cursors created with the following functions: <a href="https://docs.microsoft.com/windows/desktop/api/winuser/nf-winuser-createiconfromresourceex">CreateIconFromResourceEx</a> (if called without the <b>LR_SHARED</b> flag), <a href="https://docs.microsoft.com/windows/desktop/api/winuser/nf-winuser-createiconindirect">CreateIconIndirect</a>, and <a href="https://docs.microsoft.com/windows/desktop/api/winuser/nf-winuser-copyicon">CopyIcon</a>. Do not use this function to destroy a shared icon. A shared icon is valid as long as the module from which it was loaded remains in memory. The following functions obtain a shared icon. </para>
    /// <para>This doc was truncated.</para>
    /// <para><see href="https://learn.microsoft.com/windows/win32/api/winuser/nf-winuser-destroyicon#">Read more on learn.microsoft.com</see>.</para>
    /// </remarks>
    [DllImport(Libraries.User32, SetLastError = true)]
    public static extern bool DestroyIcon(IntPtr hIcon);
}

#pragma warning restore SA1307 // Accessible fields should begin with upper-case letter
#pragma warning restore SA1401 // Fields should be private
#pragma warning restore CA1060 // Move pinvokes to native methods class