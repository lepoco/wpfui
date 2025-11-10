// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

/* This Source Code is partially based on reverse engineering of the Windows Operating System,
   and is intended for use on Windows systems only.
   This Source Code is partially based on the source code provided by the .NET Foundation.

   NOTE:
   I split unmanaged code stuff into the NativeMethods library.
   If you have suggestions for the code below, please submit your changes there.
   https://github.com/lepoco/nativemethods */

using System.Runtime.InteropServices;

namespace Wpf.Ui.Interop;

// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
#pragma warning disable SA1300 // Element should begin with upper-case letter
#pragma warning disable SA1307 // Accessible fields should begin with upper-case letter
#pragma warning disable SA1401 // Fields should be private
#pragma warning disable CA1060 // Move pinvokes to native methods class

/// <summary>
/// USER procedure declarations, constant definitions and macros.
/// </summary>
internal static class User32
{
    /// <summary>
    /// WM_NCHITTEST and MOUSEHOOKSTRUCT Mouse Position Codes
    /// </summary>
    public enum WM_NCHITTEST
    {
        /// <summary>
        /// Hit test returned error.
        /// </summary>
        HTERROR = unchecked(-2),

        /// <summary>
        /// Hit test returned transparent.
        /// </summary>
        HTTRANSPARENT = unchecked(-1),

        /// <summary>
        /// On the screen background or on a dividing line between windows.
        /// </summary>
        HTNOWHERE = 0,

        /// <summary>
        /// In a client area.
        /// </summary>
        HTCLIENT = 1,

        /// <summary>
        /// In a title bar.
        /// </summary>
        HTCAPTION = 2,

        /// <summary>
        /// In a window menu or in a Close button in a child window.
        /// </summary>
        HTSYSMENU = 3,

        /// <summary>
        /// In a size box (same as HTSIZE).
        /// </summary>
        HTGROWBOX = 4,
        HTSIZE = HTGROWBOX,

        /// <summary>
        /// In a menu.
        /// </summary>
        HTMENU = 5,

        /// <summary>
        /// In a horizontal scroll bar.
        /// </summary>
        HTHSCROLL = 6,

        /// <summary>
        /// In the vertical scroll bar.
        /// </summary>
        HTVSCROLL = 7,

        /// <summary>
        /// In a Minimize button.
        /// </summary>
        HTMINBUTTON = 8,

        /// <summary>
        /// In a Maximize button.
        /// </summary>
        HTMAXBUTTON = 9,

        // ZOOM = 9,

        /// <summary>
        /// In the left border of a resizable window (the user can click the mouse to resize the window horizontally).
        /// </summary>
        HTLEFT = 10,

        /// <summary>
        /// In the right border of a resizable window (the user can click the mouse to resize the window horizontally).
        /// </summary>
        HTRIGHT = 11,

        /// <summary>
        /// In the upper-horizontal border of a window.
        /// </summary>
        HTTOP = 12,

        // From 10.0.22000.0\um\WinUser.h
        HTTOPLEFT = 13,
        HTTOPRIGHT = 14,
        HTBOTTOM = 15,
        HTBOTTOMLEFT = 16,
        HTBOTTOMRIGHT = 17,
        HTBORDER = 18,
        HTREDUCE = HTMINBUTTON,
        HTZOOM = HTMAXBUTTON,
        HTSIZEFIRST = HTLEFT,
        HTSIZELAST = HTBOTTOMRIGHT,
        HTOBJECT = 19,
        HTCLOSE = 20,
        HTHELP = 21,
    }

    /// <summary>
    /// Window long flags.
    /// <para><see href="https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-setwindowlonga"/></para>
    /// </summary>
    [Flags]
    public enum GWL
    {
        /// <summary>
        /// Sets a new extended window style.
        /// </summary>
        GWL_EXSTYLE = -20,

        /// <summary>
        /// Sets a new application instance handle.
        /// </summary>
        GWLP_HINSTANCE = -6,

        /// <summary>
        /// Sets a new hWnd parent.
        /// </summary>
        GWLP_HWNDPARENT = -8,

        /// <summary>
        /// Sets a new identifier of the child window. The window cannot be a top-level window.
        /// </summary>
        GWL_ID = -12,

        /// <summary>
        /// Sets a new window style.
        /// </summary>
        GWL_STYLE = -16,

        /// <summary>
        /// Sets the user data associated with the window.
        /// This data is intended for use by the application that created the window. Its value is initially zero.
        /// </summary>
        GWL_USERDATA = -21,

        /// <summary>
        /// Sets a new address for the window procedure.
        /// You cannot change this attribute if the window does not belong to the same process as the calling thread.
        /// </summary>
        GWL_WNDPROC = -4,

        /// <summary>
        /// Sets new extra information that is private to the application, such as handles or pointers.
        /// </summary>
        DWLP_USER = 0x8,

        /// <summary>
        /// Sets the return value of a message processed in the dialog box procedure.
        /// </summary>
        DWLP_MSGRESULT = 0x0,

        /// <summary>
        /// Sets the new address of the dialog box procedure.
        /// </summary>
        DWLP_DLGPROC = 0x4,
    }

    /// <summary>
    /// Window composition attributes.
    /// </summary>
    public enum WCA
    {
        WCA_UNDEFINED = 0,
        WCA_NCRENDERING_ENABLED = 1,
        WCA_NCRENDERING_POLICY = 2,
        WCA_TRANSITIONS_FORCEDISABLED = 3,
        WCA_ALLOW_NCPAINT = 4,
        WCA_CAPTION_BUTTON_BOUNDS = 5,
        WCA_NONCLIENT_RTL_LAYOUT = 6,
        WCA_FORCE_ICONIC_REPRESENTATION = 7,
        WCA_EXTENDED_FRAME_BOUNDS = 8,
        WCA_HAS_ICONIC_BITMAP = 9,
        WCA_THEME_ATTRIBUTES = 10,
        WCA_NCRENDERING_EXILED = 11,
        WCA_NCADORNMENTINFO = 12,
        WCA_EXCLUDED_FROM_LIVEPREVIEW = 13,
        WCA_VIDEO_OVERLAY_ACTIVE = 14,
        WCA_FORCE_ACTIVEWINDOW_APPEARANCE = 15,
        WCA_DISALLOW_PEEK = 16,
        WCA_CLOAK = 17,
        WCA_CLOAKED = 18,
        WCA_ACCENT_POLICY = 19,
        WCA_FREEZE_REPRESENTATION = 20,
        WCA_EVER_UNCLOAKED = 21,
        WCA_VISUAL_OWNER = 22,
        WCA_HOLOGRAPHIC = 23,
        WCA_EXCLUDED_FROM_DDA = 24,
        WCA_PASSIVEUPDATEMODE = 25,
        WCA_USEDARKMODECOLORS = 26,
        WCA_CORNER_STYLE = 27,
        WCA_PART_COLOR = 28,
        WCA_DISABLE_MOVESIZE_FEEDBACK = 29,
        WCA_LAST = 30,
    }

    /// <summary>
    /// DWM window accent state.
    /// </summary>
    public enum ACCENT_STATE
    {
        ACCENT_DISABLED = 0,
        ACCENT_ENABLE_GRADIENT = 1,
        ACCENT_ENABLE_TRANSPARENTGRADIENT = 2,
        ACCENT_ENABLE_BLURBEHIND = 3,
        ACCENT_ENABLE_ACRYLICBLURBEHIND = 4,
        ACCENT_INVALID_STATE = 5,
    }

    /// <summary>
    /// WCA window accent policy.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ACCENT_POLICY
    {
        public ACCENT_STATE nAccentState;
        public uint nFlags;
        public uint nColor;
        public uint nAnimationId;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct WINCOMPATTRDATA
    {
        public WCA Attribute;
        public IntPtr Data;
        public int SizeOfData;
    }

    /// <summary>
    /// Window message values, WM_*
    /// </summary>
    public enum WM
    {
        NULL = 0x0000,
        CREATE = 0x0001,
        DESTROY = 0x0002,
        MOVE = 0x0003,
        SIZE = 0x0005,
        ACTIVATE = 0x0006,
        SETFOCUS = 0x0007,
        KILLFOCUS = 0x0008,
        ENABLE = 0x000A,
        SETREDRAW = 0x000B,
        SETTEXT = 0x000C,
        GETTEXT = 0x000D,
        GETTEXTLENGTH = 0x000E,
        PAINT = 0x000F,
        CLOSE = 0x0010,
        QUERYENDSESSION = 0x0011,
        QUIT = 0x0012,
        QUERYOPEN = 0x0013,
        ERASEBKGND = 0x0014,
        SYSCOLORCHANGE = 0x0015,
        SHOWWINDOW = 0x0018,
        CTLCOLOR = 0x0019,
        WININICHANGE = 0x001A,
        SETTINGCHANGE = WININICHANGE,
        ACTIVATEAPP = 0x001C,
        SETCURSOR = 0x0020,
        MOUSEACTIVATE = 0x0021,
        CHILDACTIVATE = 0x0022,
        QUEUESYNC = 0x0023,
        GETMINMAXINFO = 0x0024,

        WINDOWPOSCHANGING = 0x0046,
        WINDOWPOSCHANGED = 0x0047,

        CONTEXTMENU = 0x007B,
        STYLECHANGING = 0x007C,
        STYLECHANGED = 0x007D,
        DISPLAYCHANGE = 0x007E,
        GETICON = 0x007F,
        SETICON = 0x0080,
        NCCREATE = 0x0081,
        NCDESTROY = 0x0082,
        NCCALCSIZE = 0x0083,
        NCHITTEST = 0x0084,
        NCPAINT = 0x0085,
        NCACTIVATE = 0x0086,
        GETDLGCODE = 0x0087,
        SYNCPAINT = 0x0088,
        NCMOUSEMOVE = 0x00A0,
        NCLBUTTONDOWN = 0x00A1,
        NCLBUTTONUP = 0x00A2,
        NCLBUTTONDBLCLK = 0x00A3,
        NCRBUTTONDOWN = 0x00A4,
        NCRBUTTONUP = 0x00A5,
        NCRBUTTONDBLCLK = 0x00A6,
        NCMBUTTONDOWN = 0x00A7,
        NCMBUTTONUP = 0x00A8,
        NCMBUTTONDBLCLK = 0x00A9,

        SYSKEYDOWN = 0x0104,
        SYSKEYUP = 0x0105,
        SYSCHAR = 0x0106,
        SYSDEADCHAR = 0x0107,
        COMMAND = 0x0111,
        SYSCOMMAND = 0x0112,

        MOUSEMOVE = 0x0200,
        LBUTTONDOWN = 0x0201,
        LBUTTONUP = 0x0202,
        LBUTTONDBLCLK = 0x0203,
        RBUTTONDOWN = 0x0204,
        RBUTTONUP = 0x0205,
        RBUTTONDBLCLK = 0x0206,
        MBUTTONDOWN = 0x0207,
        MBUTTONUP = 0x0208,
        MBUTTONDBLCLK = 0x0209,
        MOUSEWHEEL = 0x020A,
        XBUTTONDOWN = 0x020B,
        XBUTTONUP = 0x020C,
        XBUTTONDBLCLK = 0x020D,
        MOUSEHWHEEL = 0x020E,
        PARENTNOTIFY = 0x0210,

        CAPTURECHANGED = 0x0215,
        POWERBROADCAST = 0x0218,
        DEVICECHANGE = 0x0219,

        ENTERSIZEMOVE = 0x0231,
        EXITSIZEMOVE = 0x0232,

        IME_SETCONTEXT = 0x0281,
        IME_NOTIFY = 0x0282,
        IME_CONTROL = 0x0283,
        IME_COMPOSITIONFULL = 0x0284,
        IME_SELECT = 0x0285,
        IME_CHAR = 0x0286,
        IME_REQUEST = 0x0288,
        IME_KEYDOWN = 0x0290,
        IME_KEYUP = 0x0291,

        NCMOUSELEAVE = 0x02A2,

        TABLET_DEFBASE = 0x02C0,

        /// <summary>
        /// The WM_DPICHANGED message is sent when the DPI of the window has changed.
        /// </summary>
        /// <remarks>
        /// <para>**Supported clients:** Windows 8.1+ (Desktop apps)</para>
        /// <para>**Supported servers:** Windows Server 2012 R2+ (Desktop apps)</para>
        /// </remarks>
        DPICHANGED = 0x02E0,

        // WM_TABLET_MAXOFFSET = 0x20,
        TABLET_ADDED = TABLET_DEFBASE + 8,
        TABLET_DELETED = TABLET_DEFBASE + 9,
        TABLET_FLICK = TABLET_DEFBASE + 11,
        TABLET_QUERYSYSTEMGESTURESTATUS = TABLET_DEFBASE + 12,

        CUT = 0x0300,
        COPY = 0x0301,
        PASTE = 0x0302,
        CLEAR = 0x0303,
        UNDO = 0x0304,
        RENDERFORMAT = 0x0305,
        RENDERALLFORMATS = 0x0306,
        DESTROYCLIPBOARD = 0x0307,
        DRAWCLIPBOARD = 0x0308,
        PAINTCLIPBOARD = 0x0309,
        VSCROLLCLIPBOARD = 0x030A,
        SIZECLIPBOARD = 0x030B,
        ASKCBFORMATNAME = 0x030C,
        CHANGECBCHAIN = 0x030D,
        HSCROLLCLIPBOARD = 0x030E,
        QUERYNEWPALETTE = 0x030F,
        PALETTEISCHANGING = 0x0310,
        PALETTECHANGED = 0x0311,
        HOTKEY = 0x0312,
        PRINT = 0x0317,
        PRINTCLIENT = 0x0318,
        APPCOMMAND = 0x0319,
        THEMECHANGED = 0x031A,

        DWMCOMPOSITIONCHANGED = 0x031E,
        DWMNCRENDERINGCHANGED = 0x031F,
        DWMCOLORIZATIONCOLORCHANGED = 0x0320,
        DWMWINDOWMAXIMIZEDCHANGE = 0x0321,

        GETTITLEBARINFOEX = 0x033F,

        DWMSENDICONICTHUMBNAIL = 0x0323,
        DWMSENDICONICLIVEPREVIEWBITMAP = 0x0326,

        USER = 0x0400,

        /// <summary>
        /// This is the hard-coded message value used by WinForms for Shell_NotifyIcon.
        /// It's relatively safe to reuse.
        /// </summary>
        TRAYMOUSEMESSAGE = 0x800, // WM_USER + 1024
        APP = 0x8000,
    }

    /// <summary>
    /// SystemMetrics.  SM_*
    /// </summary>
    public enum SM
    {
        CXSCREEN = 0,
        CYSCREEN = 1,
        CXVSCROLL = 2,
        CYHSCROLL = 3,
        CYCAPTION = 4,
        CXBORDER = 5,
        CYBORDER = 6,
        CXFIXEDFRAME = 7,
        CYFIXEDFRAME = 8,
        CYVTHUMB = 9,
        CXHTHUMB = 10,
        CXICON = 11,
        CYICON = 12,
        CXCURSOR = 13,
        CYCURSOR = 14,
        CYMENU = 15,
        CXFULLSCREEN = 16,
        CYFULLSCREEN = 17,
        CYKANJIWINDOW = 18,
        MOUSEPRESENT = 19,
        CYVSCROLL = 20,
        CXHSCROLL = 21,
        DEBUG = 22,
        SWAPBUTTON = 23,
        CXMIN = 28,
        CYMIN = 29,
        CXSIZE = 30,
        CYSIZE = 31,
        CXFRAME = 32,
        CXSIZEFRAME = CXFRAME,
        CYFRAME = 33,
        CYSIZEFRAME = CYFRAME,
        CXMINTRACK = 34,
        CYMINTRACK = 35,
        CXDOUBLECLK = 36,
        CYDOUBLECLK = 37,
        CXICONSPACING = 38,
        CYICONSPACING = 39,
        MENUDROPALIGNMENT = 40,
        PENWINDOWS = 41,
        DBCSENABLED = 42,
        CMOUSEBUTTONS = 43,
        SECURE = 44,
        CXEDGE = 45,
        CYEDGE = 46,
        CXMINSPACING = 47,
        CYMINSPACING = 48,
        CXSMICON = 49,
        CYSMICON = 50,
        CYSMCAPTION = 51,
        CXSMSIZE = 52,
        CYSMSIZE = 53,
        CXMENUSIZE = 54,
        CYMENUSIZE = 55,
        ARRANGE = 56,
        CXMINIMIZED = 57,
        CYMINIMIZED = 58,
        CXMAXTRACK = 59,
        CYMAXTRACK = 60,
        CXMAXIMIZED = 61,
        CYMAXIMIZED = 62,
        NETWORK = 63,
        CLEANBOOT = 67,
        CXDRAG = 68,
        CYDRAG = 69,
        SHOWSOUNDS = 70,
        CXMENUCHECK = 71,
        CYMENUCHECK = 72,
        SLOWMACHINE = 73,
        MIDEASTENABLED = 74,
        MOUSEWHEELPRESENT = 75,
        XVIRTUALSCREEN = 76,
        YVIRTUALSCREEN = 77,
        CXVIRTUALSCREEN = 78,
        CYVIRTUALSCREEN = 79,
        CMONITORS = 80,
        SAMEDISPLAYFORMAT = 81,
        IMMENABLED = 82,
        CXFOCUSBORDER = 83,
        CYFOCUSBORDER = 84,
        TABLETPC = 86,
        MEDIACENTER = 87,
        CXPADDEDBORDER = 92,
        REMOTESESSION = 0x1000,
        REMOTECONTROL = 0x2001,
    }

    /// <summary>
    /// Delegate declaration that matches native WndProc signatures.
    /// </summary>
    public delegate IntPtr WndProc(IntPtr hWnd, WM uMsg, IntPtr wParam, IntPtr lParam);

    /// <summary>
    /// Changes an attribute of the specified window. The function also sets a value at the specified offset in the extra window memory.
    /// </summary>
    /// <param name="hWnd">A handle to the window and, indirectly, the class to which the window belongs.</param>
    /// <param name="nIndex">The zero-based offset to the value to be set.</param>
    /// <param name="dwNewLong">The replacement value.</param>
    /// <returns>If the function succeeds, the return value is the previous value of the specified offset.</returns>
    [DllImport(Libraries.User32, CharSet = CharSet.Auto)]
    public static extern IntPtr SetWindowLongPtr([In] IntPtr hWnd, [In] int nIndex, [In] IntPtr dwNewLong);

    /// <summary>
    /// Determines whether the specified window handle identifies an existing window.
    /// </summary>
    /// <param name="hWnd">A handle to the window to be tested.</param>
    /// <returns>If the window handle identifies an existing window, the return value is nonzero.</returns>
    [DllImport(Libraries.User32, CharSet = CharSet.Auto)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool IsWindow([In] IntPtr hWnd);

    /// <summary>
    /// Retrieves the dimensions of the bounding rectangle of the specified window. The dimensions are given in screen coordinates that are relative to the upper-left corner of the screen.
    /// </summary>
    /// <param name="hWnd">A handle to the window.</param>
    /// <param name="lpRect">A pointer to a RECT structure that receives the screen coordinates of the upper-left and lower-right corners of the window.</param>
    /// <returns>If the function succeeds, the return value is nonzero.</returns>
    [DllImport(Libraries.User32, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool GetWindowRect([In] IntPtr hWnd, [Out] out WinDef.RECT lpRect);

    /// <summary>
    /// Retrieves the specified system metric or system configuration setting.
    /// Note that all dimensions retrieved by GetSystemMetrics are in pixels.
    /// </summary>
    /// <param name="nIndex">The system metric or configuration setting to be retrieved. This parameter can be one of the <see cref="SM"/> values.
    /// Note that all SM_CX* values are widths and all SM_CY* values are heights. Also note that all settings designed to return Boolean data represent <see langword="true"/> as any nonzero value, and <see langword="false"/> as a zero value.</param>
    /// <returns>If the function succeeds, the return value is the requested system metric or configuration setting.</returns>
    [DllImport(Libraries.User32)]
    public static extern int GetSystemMetrics([In] SM nIndex);

    [DllImport(Libraries.User32, EntryPoint = "SetWindowRgn", SetLastError = true)]
    private static extern int _SetWindowRgn(
        [In] IntPtr hWnd,
        [In] IntPtr hRgn,
        [In] [MarshalAs(UnmanagedType.Bool)] bool bRedraw
    );

    /// <summary>
    /// The SetWindowRgn function sets the window region of a window. The window region determines the area within the window where the system permits drawing. The system does not display any portion of a window that lies outside of the window region.
    /// </summary>
    /// <param name="hWnd">A handle to the window whose window region is to be set.</param>
    /// <param name="hRgn">A handle to a region. The function sets the window region of the window to this region.</param>
    /// <param name="bRedraw">Specifies whether the system redraws the window after setting the window region. If bRedraw is <see langword="true"/>, the system does so; otherwise, it does not.</param>
    /// <exception cref="Win32Exception">Native method returned HRESULT.</exception>
    public static void SetWindowRgn([In] IntPtr hWnd, [In] IntPtr hRgn, [In] bool bRedraw)
    {
        var err = _SetWindowRgn(hWnd, hRgn, bRedraw);

        if (err == 0)
        {
            throw new Win32Exception();
        }
    }

    /// <summary>
    /// Sets various information regarding DWM window attributes.
    /// </summary>
    [DllImport(Libraries.User32, CharSet = CharSet.Auto)]
    public static extern int SetWindowCompositionAttribute(
        [In] IntPtr hWnd,
        [In, Out] ref WINCOMPATTRDATA data
    );
}

#pragma warning restore SA1300 // Element should begin with upper-case letter
#pragma warning restore SA1307 // Accessible fields should begin with upper-case letter
#pragma warning restore SA1401 // Fields should be private
#pragma warning restore CA1060 // Move pinvokes to native methods class
