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
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows;

// ReSharper disable InconsistentNaming

namespace Wpf.Ui.Interop;

/// <summary>
/// USER procedure declarations, constant definitions and macros.
/// </summary>
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
internal static class User32
{
    /// <summary>
    /// SetWindowPos options
    /// </summary>
    [Flags]
    public enum SWP
    {
        ASYNCWINDOWPOS = 0x4000,
        DEFERERASE = 0x2000,
        DRAWFRAME = 0x0020,
        FRAMECHANGED = 0x0020,
        HIDEWINDOW = 0x0080,
        NOACTIVATE = 0x0010,
        NOCOPYBITS = 0x0100,
        NOMOVE = 0x0002,
        NOOWNERZORDER = 0x0200,
        NOREDRAW = 0x0008,
        NOREPOSITION = 0x0200,
        NOSENDCHANGING = 0x0400,
        NOSIZE = 0x0001,
        NOZORDER = 0x0004,
        SHOWWINDOW = 0x0040,
    }

    /// <summary>
    /// EnableMenuItem uEnable values, MF_*
    /// </summary>
    [Flags]
    public enum MF : uint
    {
        /// <summary>
        /// Possible return value for EnableMenuItem
        /// </summary>
        DOES_NOT_EXIST = unchecked((uint)-1),
        ENABLED = 0,
        BYCOMMAND = 0,
        GRAYED = 1,
        DISABLED = 2,
    }

    /// <summary>
    /// Menu item element.
    /// </summary>
    public enum SC
    {
        SIZE = 0xF000,
        MOVE = 0xF010,
        MINIMIZE = 0xF020,
        MAXIMIZE = 0xF030,
        NEXTWINDOW = 0xF040,
        PREVWINDOW = 0xF050,
        CLOSE = 0xF060,
        VSCROLL = 0xF070,
        HSCROLL = 0xF080,
        MOUSEMENU = 0xF090,
        KEYMENU = 0xF100,
        ARRANGE = 0xF110,
        RESTORE = 0xF120,
        TASKLIST = 0xF130,
        SCREENSAVE = 0xF140,
        HOTKEY = 0xF150,
        DEFAULT = 0xF160,
        MONITORPOWER = 0xF170,
        CONTEXTHELP = 0xF180,
        SEPARATOR = 0xF00F,
        /// <summary>
        /// SCF_ISSECURE
        /// </summary>
        F_ISSECURE = 0x00000001,
        ICON = MINIMIZE,
        ZOOM = MAXIMIZE,
    }

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
        HTHELP = 21
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
        DWLP_DLGPROC = 0x4
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
        WCA_LAST = 30
    }

    [Flags]
    public enum ACCENT_FLAGS
    {
        DrawLeftBorder = 0x20,
        DrawTopBorder = 0x40,
        DrawRightBorder = 0x80,
        DrawBottomBorder = 0x100,
        DrawAllBorders = DrawLeftBorder | DrawTopBorder | DrawRightBorder | DrawBottomBorder
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
        ACCENT_INVALID_STATE = 5
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
    /// CS_*
    /// </summary>
    [Flags]
    public enum CS : uint
    {
        VREDRAW = 0x0001,
        HREDRAW = 0x0002,
        DBLCLKS = 0x0008,
        OWNDC = 0x0020,
        CLASSDC = 0x0040,
        PARENTDC = 0x0080,
        NOCLOSE = 0x0200,
        SAVEBITS = 0x0800,
        BYTEALIGNCLIENT = 0x1000,
        BYTEALIGNWINDOW = 0x2000,
        GLOBALCLASS = 0x4000,
        IME = 0x00010000,
        DROPSHADOW = 0x00020000
    }

    /// <summary>
    /// MSGFLT_*. New in Vista. Realiased in Windows 7.
    /// </summary>
    public enum MSGFLT
    {
        // Win7 versions of this enum:

        /// <summary>
        /// Resets the window message filter for hWnd to the default. Any message allowed globally or process-wide will get through, but any message not included in those two categories, and which comes from a lower privileged process, will be blocked.
        /// </summary>
        RESET = 0,

        /// <summary>
        /// Allows the message through the filter. This enables the message to be received by hWnd, regardless of the source of the message, even it comes from a lower privileged process.
        /// </summary>
        ALLOW = 1,

        /// <summary>
        /// Blocks the message to be delivered to hWnd if it comes from a lower privileged process, unless the message is allowed process-wide by using the ChangeWindowMessageFilter function or globally.
        /// </summary>
        DISALLOW = 2,

        // Vista versions of this enum:
        // ADD = 1,
        // REMOVE = 2,
    }

    /// <summary>
    /// MSGFLTINFO.
    /// </summary>
    public enum MSGFLTINFO
    {
        NONE = 0,
        ALREADYALLOWED_FORWND = 1,
        ALREADYDISALLOWED_FORWND = 2,
        ALLOWED_HIGHER = 3,
    }

    /// <summary>
    /// Win7 only.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CHANGEFILTERSTRUCT
    {
        public uint cbSize;
        public MSGFLTINFO ExtStatus;
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
        SETTINGCHANGE = 0x001A,
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
        //WM_TABLET_MAXOFFSET = 0x20,

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

        #region Windows 7

        DWMSENDICONICTHUMBNAIL = 0x0323,
        DWMSENDICONICLIVEPREVIEWBITMAP = 0x0326,

        #endregion

        USER = 0x0400,

        /// <summary>
        /// This is the hard-coded message value used by WinForms for Shell_NotifyIcon.
        /// It's relatively safe to reuse.
        /// </summary>
        TRAYMOUSEMESSAGE = 0x800, //WM_USER + 1024
        APP = 0x8000,
    }

    /// <summary>
    /// WindowStyle values, WS_*
    /// </summary>
    [Flags]
    public enum WS : long
    {
        OVERLAPPED = 0x00000000,
        POPUP = 0x80000000,
        CHILD = 0x40000000,
        MINIMIZE = 0x20000000,
        VISIBLE = 0x10000000,
        DISABLED = 0x08000000,
        CLIPSIBLINGS = 0x04000000,
        CLIPCHILDREN = 0x02000000,
        MAXIMIZE = 0x01000000,
        BORDER = 0x00800000,
        DLGFRAME = 0x00400000,
        VSCROLL = 0x00200000,
        HSCROLL = 0x00100000,
        SYSMENU = 0x00080000,
        THICKFRAME = 0x00040000,
        GROUP = 0x00020000,
        TABSTOP = 0x00010000,

        MINIMIZEBOX = 0x00020000,
        MAXIMIZEBOX = 0x00010000,

        CAPTION = BORDER | DLGFRAME,
        TILED = OVERLAPPED,
        ICONIC = MINIMIZE,
        SIZEBOX = THICKFRAME,
        TILEDWINDOW = OVERLAPPEDWINDOW,

        OVERLAPPEDWINDOW = OVERLAPPED | CAPTION | SYSMENU | THICKFRAME | MINIMIZEBOX | MAXIMIZEBOX,
        POPUPWINDOW = POPUP | BORDER | SYSMENU,
        CHILDWINDOW = CHILD,
    }

    /// <summary>
    /// Window style extended values, WS_EX_*
    /// </summary>
    [Flags]
    public enum WS_EX : long
    {
        NONE = 0,
        DLGMODALFRAME = 0x00000001,
        NOPARENTNOTIFY = 0x00000004,
        TOPMOST = 0x00000008,
        ACCEPTFILES = 0x00000010,
        TRANSPARENT = 0x00000020,
        MDICHILD = 0x00000040,
        TOOLWINDOW = 0x00000080,
        WINDOWEDGE = 0x00000100,
        CLIENTEDGE = 0x00000200,
        CONTEXTHELP = 0x00000400,
        RIGHT = 0x00001000,
        LEFT = 0x00000000,
        RTLREADING = 0x00002000,
        LTRREADING = 0x00000000,
        LEFTSCROLLBAR = 0x00004000,
        RIGHTSCROLLBAR = 0x00000000,
        CONTROLPARENT = 0x00010000,
        STATICEDGE = 0x00020000,
        APPWINDOW = 0x00040000,
        LAYERED = 0x00080000,
        NOINHERITLAYOUT = 0x00100000, // Disable inheritence of mirroring by children
        LAYOUTRTL = 0x00400000, // Right to left mirroring
        COMPOSITED = 0x02000000,
        NOACTIVATE = 0x08000000,
        OVERLAPPEDWINDOW = (WINDOWEDGE | CLIENTEDGE),
        PALETTEWINDOW = (WINDOWEDGE | TOOLWINDOW | TOPMOST),
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
    /// ShowWindow options
    /// </summary>
    public enum SW
    {
        HIDE = 0,
        SHOWNORMAL = 1,
        NORMAL = 1,
        SHOWMINIMIZED = 2,
        SHOWMAXIMIZED = 3,
        MAXIMIZE = 3,
        SHOWNOACTIVATE = 4,
        SHOW = 5,
        MINIMIZE = 6,
        SHOWMINNOACTIVE = 7,
        SHOWNA = 8,
        RESTORE = 9,
        SHOWDEFAULT = 10,
        FORCEMINIMIZE = 11,
    }

    [StructLayout(LayoutKind.Sequential)]
    public class WINDOWPLACEMENT
    {
        public int length = Marshal.SizeOf(typeof(WINDOWPLACEMENT));
        public int flags;
        public SW showCmd;
        public WinDef.POINT ptMinPosition;
        public WinDef.POINT ptMaxPosition;
        public WinDef.RECT rcNormalPosition;
    }

    /// <summary>
    /// Contains window class information. It is used with the <see cref="RegisterClassEx"/> and GetClassInfoEx functions.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct WNDCLASSEX
    {
        /// <summary>
        /// The size, in bytes, of this structure. Set this member to sizeof(WNDCLASSEX). Be sure to set this member before calling the GetClassInfoEx function.
        /// </summary>
        public int cbSize;

        /// <summary>
        /// The class style(s). This member can be any combination of the Class Styles.
        /// </summary>
        public CS style;

        /// <summary>
        /// A pointer to the window procedure. You must use the CallWindowProc function to call the window procedure. For more information, see WindowProc.
        /// </summary>
        public WndProc lpfnWndProc;

        /// <summary>
        /// The number of extra bytes to allocate following the window-class structure. The system initializes the bytes to zero.
        /// </summary>
        public int cbClsExtra;

        /// <summary>
        /// The number of extra bytes to allocate following the window instance. The system initializes the bytes to zero. If an application uses WNDCLASSEX to register a dialog box created by using the CLASS directive in the resource file, it must set this member to DLGWINDOWEXTRA.
        /// </summary>
        public int cbWndExtra;

        /// <summary>
        /// A handle to the instance that contains the window procedure for the class.
        /// </summary>
        public IntPtr hInstance;

        /// <summary>
        /// A handle to the class icon. This member must be a handle to an icon resource. If this member is NULL, the system provides a default icon.
        /// </summary>
        public IntPtr hIcon;

        /// <summary>
        /// A handle to the class cursor. This member must be a handle to a cursor resource. If this member is NULL, an application must explicitly set the cursor shape whenever the mouse moves into the application's window.
        /// </summary>
        public IntPtr hCursor;

        /// <summary>
        /// A handle to the class background brush. This member can be a handle to the brush to be used for painting the background, or it can be a color value.
        /// </summary>
        public IntPtr hbrBackground;

        /// <summary>
        /// Pointer to a null-terminated character string that specifies the resource name of the class menu, as the name appears in the resource file. If you use an integer to identify the menu, use the MAKEINTRESOURCE macro. If this member is NULL, windows belonging to this class have no default menu.
        /// </summary>
        [MarshalAs(UnmanagedType.LPWStr)] public string lpszMenuName;

        /// <summary>
        /// A pointer to a null-terminated string or is an atom. If this parameter is an atom, it must be a class atom created by a previous call to the RegisterClass or RegisterClassEx function. The atom must be in the low-order word of lpszClassName; the high-order word must be zero.
        /// </summary>
        [MarshalAs(UnmanagedType.LPWStr)] public string lpszClassName;

        /// <summary>
        /// A handle to a small icon that is associated with the window class. If this member is NULL, the system searches the icon resource specified by the hIcon member for an icon of the appropriate size to use as the small icon.
        /// </summary>
        public IntPtr hIconSm;
    }

    /// <summary>
    /// Delegate declaration that matches native WndProc signatures.
    /// </summary>
    public delegate IntPtr WndProc(IntPtr hWnd, WM uMsg, IntPtr wParam, IntPtr lParam);

    /// <summary>
    /// Delegate declaration that matches native WndProc signatures.
    /// </summary>
    public delegate IntPtr WndProcHook(IntPtr hWnd, WM uMsg, IntPtr wParam, IntPtr lParam, ref bool handled);

    /// <summary>
    /// Delegate declaration that matches managed WndProc signatures.
    /// </summary>
    public delegate IntPtr MessageHandler(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled);

    /// <summary>
    /// The ReleaseDC function releases a device context (DC), freeing it for use by other applications.
    /// The effect of the ReleaseDC function depends on the type of DC. It frees only common and window DCs. It has no effect on class or private DCs.
    /// </summary>
    /// <param name="hWnd">A handle to the window whose DC is to be released.</param>
    /// <param name="hDC">A handle to the DC to be released.</param>
    /// <returns>The return value indicates whether the DC was released. If the DC was released, the return value is 1. If the DC was not released, the return value is zero.</returns>
    [DllImport(Libraries.User32, CharSet = CharSet.Auto)]
    public static extern int ReleaseDC([In] IntPtr hWnd, [In] IntPtr hDC);

    /// <summary>
    /// Calculates the required size of the window rectangle, based on the desired size of the client rectangle.
    /// The window rectangle can then be passed to the CreateWindowEx function to create a window whose client area is the desired size.
    /// </summary>
    /// <param name="lpRect">A pointer to a RECT structure that contains the coordinates of the top-left and bottom-right corners of the desired client area.</param>
    /// <param name="dwStyle">The window style of the window whose required size is to be calculated. Note that you cannot specify the WS_OVERLAPPED style.</param>
    /// <param name="bMenu">Indicates whether the window has a menu.</param>
    /// <param name="dwExStyle">The extended window style of the window whose required size is to be calculated.</param>
    /// <returns>If the function succeeds, the return value is nonzero.</returns>
    [DllImport(Libraries.User32, CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool AdjustWindowRectEx([In] ref Rect lpRect, [In] WS dwStyle,
        [In][MarshalAs(UnmanagedType.Bool)] bool bMenu, [In] WS_EX dwExStyle);

    /// <summary>
    /// [Using the ChangeWindowMessageFilter function is not recommended, as it has process-wide scope. Instead, use the ChangeWindowMessageFilterEx function to control access to specific windows as needed. ChangeWindowMessageFilter may not be supported in future versions of Windows.
    /// <para>Adds or removes a message from the User Interface Privilege Isolation(UIPI) message filter.</para>
    /// </summary>
    /// <param name="message">The message to add to or remove from the filter.</param>
    /// <param name="dwFlag">The action to be performed. One of the following values.</param>
    /// <returns><see langword="true"/> if successful; otherwise, <see langword="false"/>. To get extended error information, call <see cref="Kernel32.GetLastError"/>.</returns>
    [DllImport(Libraries.User32, CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool ChangeWindowMessageFilter([In] WM message, [In] MSGFLT dwFlag);

    /// <summary>
    /// Modifies the User Interface Privilege Isolation (UIPI) message filter for a specified window.
    /// </summary>
    /// <param name="hWnd">A handle to the window whose UIPI message filter is to be modified.</param>
    /// <param name="message">The message that the message filter allows through or blocks.</param>
    /// <param name="action">The action to be performed.</param>
    /// <param name="pChangeFilterStruct">Optional pointer to a <see cref="CHANGEFILTERSTRUCT"/> structure.</param>
    /// <returns>If the function succeeds, it returns <see langword="true"/>; otherwise, it returns <see langword="false"/>. To get extended error information, call <see cref="Kernel32.GetLastError"/>.</returns>
    [DllImport(Libraries.User32, CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool ChangeWindowMessageFilterEx([In] IntPtr hWnd, [In] WM message, [In] MSGFLT action,
        [In, Out, Optional] ref CHANGEFILTERSTRUCT pChangeFilterStruct);

    /// <summary>
    /// Places (posts) a message in the message queue associated with the thread that created the specified window and returns without waiting for the thread to process the message.
    /// <para>Unicode declaration for <see cref="PostMessage"/></para>
    /// </summary>
    /// <param name="hWnd">A handle to the window whose window procedure is to receive the message.</param>
    /// <param name="Msg">The message to be posted.</param>
    /// <param name="wParam">Additional message-specific information.</param>
    /// <param name="lParam">Additional message-specific information.</param>
    /// <returns>If the function succeeds, the return value is nonzero.</returns>
    [DllImport(Libraries.User32, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool PostMessageW([In, Optional] IntPtr hWnd, [In] WM Msg, [In] IntPtr wParam,
        [In] IntPtr lParam);

    /// <summary>
    /// Places (posts) a message in the message queue associated with the thread that created the specified window and returns without waiting for the thread to process the message.
    /// <para>ANSI declaration for <see cref="PostMessage"/></para>
    /// </summary>
    /// <param name="hWnd">A handle to the window whose window procedure is to receive the message.</param>
    /// <param name="Msg">The message to be posted.</param>
    /// <param name="wParam">Additional message-specific information.</param>
    /// <param name="lParam">Additional message-specific information.</param>
    /// <returns>If the function succeeds, the return value is nonzero.</returns>
    [DllImport(Libraries.User32, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool PostMessageA([In, Optional] IntPtr hWnd, [In] WM Msg, [In] IntPtr wParam,
        [In] IntPtr lParam);

    /// <summary>
    /// Places (posts) a message in the message queue associated with the thread that created the specified window and returns without waiting for the thread to process the message.
    /// </summary>
    /// <param name="hWnd">A handle to the window whose window procedure is to receive the message.</param>
    /// <param name="Msg">The message to be posted.</param>
    /// <param name="wParam">Additional message-specific information.</param>
    /// <param name="lParam">Additional message-specific information.</param>
    /// <returns>If the function succeeds, the return value is nonzero.</returns>
    [DllImport(Libraries.User32, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool PostMessage([In, Optional] IntPtr hWnd, [In] WM Msg, [In] IntPtr wParam,
        [In] IntPtr lParam);

    /// <summary>
    /// Sends the specified message to a window or windows. The SendMessage function calls the window procedure for the specified window and does not return until the window procedure has processed the message.
    /// </summary>
    /// <param name="hWnd">A handle to the window whose window procedure will receive the message.</param>
    /// <param name="wMsg">The message to be sent.</param>
    /// <param name="wParam">Additional message-specific information.</param>
    /// <param name="lParam">Additional message-specific information.</param>
    /// <returns>The return value specifies the result of the message processing; it depends on the message sent.</returns>
    [DllImport(Libraries.User32, CharSet = CharSet.Auto)]
    public static extern int SendMessage([In] IntPtr hWnd, [In] WM wMsg, [In] IntPtr wParam, [In] IntPtr lParam);

    /// <summary>
    /// Creates an overlapped, pop-up, or child window with an extended window style; otherwise,
    /// this function is identical to the CreateWindow function. For more information about
    /// creating a window and for full descriptions of the other parameters of CreateWindowEx, see CreateWindow.
    /// </summary>
    /// <param name="dwExStyle">The extended window style of the window being created.</param>
    /// <param name="lpClassName">A null-terminated string or a class atom created by a previous call to the RegisterClass or RegisterClassEx function.</param>
    /// <param name="lpWindowName">The window name. If the window style specifies a title bar, the window title pointed to by lpWindowName is displayed in the title bar.</param>
    /// <param name="dwStyle">The style of the window being created. This parameter can be a combination of the window style values, plus the control styles indicated in the Remarks section.</param>
    /// <param name="x">The initial horizontal position of the window. For an overlapped or pop-up window, the x parameter is the initial x-coordinate of the window's upper-left corner, in screen coordinates.</param>
    /// <param name="y">The initial vertical position of the window. For an overlapped or pop-up window, the y parameter is the initial y-coordinate of the window's upper-left corner, in screen coordinates.</param>
    /// <param name="nWidth">The width, in device units, of the window. For overlapped windows, nWidth is the window's width, in screen coordinates, or CW_USEDEFAULT.</param>
    /// <param name="nHeight">The height, in device units, of the window. For overlapped windows, nHeight is the window's height, in screen coordinates. If the nWidth parameter is set to CW_USEDEFAULT, the system ignores nHeight.</param>
    /// <param name="hWndParent">A handle to the parent or owner window of the window being created. To create a child window or an owned window, supply a valid window handle. This parameter is optional for pop-up windows.</param>
    /// <param name="hMenu">A handle to a menu, or specifies a child-window identifier, depending on the window style. For an overlapped or pop-up window, hMenu identifies the menu to be used with the window; it can be NULL if the class menu is to be used.</param>
    /// <param name="hInstance">A handle to the instance of the module to be associated with the window.</param>
    /// <param name="lpParam">Pointer to a value to be passed to the window through the CREATESTRUCT structure (lpCreateParams member) pointed to by the lParam param of the WM_CREATE message. This message is sent to the created window by this function before it returns.</param>
    /// <returns>If the function succeeds, the return value is a handle to the new window.</returns>
    [DllImport(Libraries.User32, SetLastError = true, CharSet = CharSet.Unicode)]
    public static extern IntPtr CreateWindowExW(
        [In] WS_EX dwExStyle,
        [In, Optional] [MarshalAs(UnmanagedType.LPWStr)]
        string lpClassName,
        [In, Optional] [MarshalAs(UnmanagedType.LPWStr)]
        string lpWindowName,
        [In] WS dwStyle,
        [In] int x,
        [In] int y,
        [In] int nWidth,
        [In] int nHeight,
        [In, Optional] IntPtr hWndParent,
        [In, Optional] IntPtr hMenu,
        [In, Optional] IntPtr hInstance,
        [In, Optional] IntPtr lpParam);

    /// <summary>
    /// Creates an overlapped, pop-up, or child window with an extended window style; otherwise,
    /// this function is identical to the CreateWindow function. For more information about
    /// creating a window and for full descriptions of the other parameters of CreateWindowEx, see CreateWindow.
    /// </summary>
    /// <param name="dwExStyle">The extended window style of the window being created.</param>
    /// <param name="lpClassName">A null-terminated string or a class atom created by a previous call to the RegisterClass or RegisterClassEx function.</param>
    /// <param name="lpWindowName">The window name. If the window style specifies a title bar, the window title pointed to by lpWindowName is displayed in the title bar.</param>
    /// <param name="dwStyle">The style of the window being created. This parameter can be a combination of the window style values, plus the control styles indicated in the Remarks section.</param>
    /// <param name="x">The initial horizontal position of the window. For an overlapped or pop-up window, the x parameter is the initial x-coordinate of the window's upper-left corner, in screen coordinates.</param>
    /// <param name="y">The initial vertical position of the window. For an overlapped or pop-up window, the y parameter is the initial y-coordinate of the window's upper-left corner, in screen coordinates.</param>
    /// <param name="nWidth">The width, in device units, of the window. For overlapped windows, nWidth is the window's width, in screen coordinates, or CW_USEDEFAULT.</param>
    /// <param name="nHeight">The height, in device units, of the window. For overlapped windows, nHeight is the window's height, in screen coordinates. If the nWidth parameter is set to CW_USEDEFAULT, the system ignores nHeight.</param>
    /// <param name="hWndParent">A handle to the parent or owner window of the window being created. To create a child window or an owned window, supply a valid window handle. This parameter is optional for pop-up windows.</param>
    /// <param name="hMenu">A handle to a menu, or specifies a child-window identifier, depending on the window style. For an overlapped or pop-up window, hMenu identifies the menu to be used with the window; it can be NULL if the class menu is to be used.</param>
    /// <param name="hInstance">A handle to the instance of the module to be associated with the window.</param>
    /// <param name="lpParam">Pointer to a value to be passed to the window through the CREATESTRUCT structure (lpCreateParams member) pointed to by the lParam param of the WM_CREATE message. This message is sent to the created window by this function before it returns.</param>
    /// <returns>If the function succeeds, the return value is a handle to the new window.</returns>
    public static IntPtr CreateWindowEx(
        [In] WS_EX dwExStyle,
        [In] string lpClassName,
        [In] string lpWindowName,
        [In] WS dwStyle,
        [In] int x,
        [In] int y,
        [In] int nWidth,
        [In] int nHeight,
        [In, Optional] IntPtr hWndParent,
        [In, Optional] IntPtr hMenu,
        [In, Optional] IntPtr hInstance,
        [In, Optional] IntPtr lpParam)
    {
        IntPtr ret = CreateWindowExW(dwExStyle, lpClassName, lpWindowName, dwStyle, x, y, nWidth, nHeight, hWndParent,
            hMenu, hInstance, lpParam);
        if (IntPtr.Zero == ret)
        {
            throw new Exception("Unable to create a window");
            // HRESULT.ThrowLastError();
        }

        return ret;
    }

    /// <summary>
    /// Registers a window class for subsequent use in calls to the CreateWindow or CreateWindowEx function.
    /// <para>Unicode declaration for <see cref="RegisterClassEx"/></para>
    /// </summary>
    /// <param name="lpwcx">A pointer to a <see cref="WNDCLASSEX"/> structure. You must fill the structure with the appropriate class attributes before passing it to the function.</param>
    /// <returns>If the function succeeds, the return value is a class atom that uniquely identifies the class being registered.</returns>
    [DllImport(Libraries.User32, SetLastError = true, CharSet = CharSet.Unicode)]
    public static extern short RegisterClassExW([In] ref WNDCLASSEX lpwcx);

    /// <summary>
    /// Registers a window class for subsequent use in calls to the CreateWindow or CreateWindowEx function.
    /// <para>ANSI declaration for <see cref="RegisterClassEx"/></para>
    /// </summary>
    /// <param name="lpwcx">A pointer to a <see cref="WNDCLASSEX"/> structure. You must fill the structure with the appropriate class attributes before passing it to the function.</param>
    /// <returns>If the function succeeds, the return value is a class atom that uniquely identifies the class being registered.</returns>
    [DllImport(Libraries.User32, SetLastError = true)]
    public static extern short RegisterClassExA([In] ref WNDCLASSEX lpwcx);

    /// <summary>
    /// Registers a window class for subsequent use in calls to the CreateWindow or CreateWindowEx function.
    /// </summary>
    /// <param name="lpwcx">A pointer to a <see cref="WNDCLASSEX"/> structure. You must fill the structure with the appropriate class attributes before passing it to the function.</param>
    /// <returns>If the function succeeds, the return value is a class atom that uniquely identifies the class being registered.</returns>
    [DllImport(Libraries.User32, SetLastError = true)]
    public static extern short RegisterClassEx([In] ref WNDCLASSEX lpwcx);

    /// <summary>
    /// Calls the default window procedure to provide default processing for any window messages that an application does not process.
    /// This function ensures that every message is processed. DefWindowProc is called with the same parameters received by the window procedure.
    /// <para>Unicode declaration for <see cref="DefWindowProc"/></para>
    /// </summary>
    /// <param name="hWnd">A handle to the window procedure that received the message.</param>
    /// <param name="Msg">The message.</param>
    /// <param name="wParam">Additional message information. The content of this parameter depends on the value of the Msg parameter.</param>
    /// <param name="lParam">Additional message information. The content of this parameter depends on the value of the Msg parameter.</param>
    /// <returns>The return value is the result of the message processing and depends on the message.</returns>
    [DllImport(Libraries.User32, CharSet = CharSet.Unicode)]
    public static extern IntPtr DefWindowProcW([In] IntPtr hWnd, [In] WM Msg, [In] IntPtr wParam, [In] IntPtr lParam);

    /// <summary>
    /// Calls the default window procedure to provide default processing for any window messages that an application does not process.
    /// This function ensures that every message is processed. DefWindowProc is called with the same parameters received by the window procedure.
    /// <para>ANSI declaration for <see cref="DefWindowProc"/></para>
    /// </summary>
    /// <param name="hWnd">A handle to the window procedure that received the message.</param>
    /// <param name="Msg">The message.</param>
    /// <param name="wParam">Additional message information. The content of this parameter depends on the value of the Msg parameter.</param>
    /// <param name="lParam">Additional message information. The content of this parameter depends on the value of the Msg parameter.</param>
    /// <returns>The return value is the result of the message processing and depends on the message.</returns>
    [DllImport(Libraries.User32, CharSet = CharSet.Auto)]
    public static extern IntPtr DefWindowProcA([In] IntPtr hWnd, [In] WM Msg, [In] IntPtr wParam, [In] IntPtr lParam);

    /// <summary>
    /// Calls the default window procedure to provide default processing for any window messages that an application does not process.
    /// This function ensures that every message is processed. DefWindowProc is called with the same parameters received by the window procedure.
    /// </summary>
    /// <param name="hWnd">A handle to the window procedure that received the message.</param>
    /// <param name="Msg">The message.</param>
    /// <param name="wParam">Additional message information. The content of this parameter depends on the value of the Msg parameter.</param>
    /// <param name="lParam">Additional message information. The content of this parameter depends on the value of the Msg parameter.</param>
    /// <returns>The return value is the result of the message processing and depends on the message.</returns>
    [DllImport(Libraries.User32, CharSet = CharSet.Auto)]
    public static extern IntPtr DefWindowProc([In] IntPtr hWnd, [In] WM Msg, [In] IntPtr wParam, [In] IntPtr lParam);

    /// <summary>
    /// Retrieves information about the specified window. The function also retrieves the 32-bit (DWORD) value at the specified offset into the extra window memory.
    /// <para>If you are retrieving a pointer or a handle, this function has been superseded by the <see cref="GetWindowLongPtr"/> function.</para>
    /// <para>Unicode declaration for <see cref="GetWindowLong"/></para>
    /// </summary>
    /// <param name="hWnd">A handle to the window and, indirectly, the class to which the window belongs.</param>
    /// <param name="nIndex">The zero-based offset to the value to be retrieved.</param>
    /// <returns>If the function succeeds, the return value is the requested value.</returns>
    [DllImport(Libraries.User32, CharSet = CharSet.Unicode)]
    public static extern long GetWindowLongW([In] IntPtr hWnd, [In] int nIndex);

    /// <summary>
    /// Retrieves information about the specified window. The function also retrieves the 32-bit (DWORD) value at the specified offset into the extra window memory.
    /// <para>If you are retrieving a pointer or a handle, this function has been superseded by the <see cref="GetWindowLongPtr"/> function.</para>
    /// <para>ANSI declaration for <see cref="GetWindowLong"/></para>
    /// </summary>
    /// <param name="hWnd">A handle to the window and, indirectly, the class to which the window belongs.</param>
    /// <param name="nIndex">The zero-based offset to the value to be retrieved.</param>
    /// <returns>If the function succeeds, the return value is the requested value.</returns>
    [DllImport(Libraries.User32, CharSet = CharSet.Auto)]
    public static extern long GetWindowLongA([In] IntPtr hWnd, [In] int nIndex);

    /// <summary>
    /// Retrieves information about the specified window. The function also retrieves the 32-bit (DWORD) value at the specified offset into the extra window memory.
    /// <para>If you are retrieving a pointer or a handle, this function has been superseded by the <see cref="GetWindowLongPtr"/> function.</para>
    /// </summary>
    /// <param name="hWnd">A handle to the window and, indirectly, the class to which the window belongs.</param>
    /// <param name="nIndex">The zero-based offset to the value to be retrieved.</param>
    /// <returns>If the function succeeds, the return value is the requested value.</returns>
    [DllImport(Libraries.User32, CharSet = CharSet.Auto)]
    public static extern long GetWindowLong([In] IntPtr hWnd, [In] int nIndex);

    /// <summary>
    /// Retrieves information about the specified window. The function also retrieves the 32-bit (DWORD) value at the specified offset into the extra window memory.
    /// <para>If you are retrieving a pointer or a handle, this function has been superseded by the <see cref="GetWindowLongPtr"/> function.</para>
    /// </summary>
    /// <param name="hWnd">A handle to the window and, indirectly, the class to which the window belongs.</param>
    /// <param name="nIndex">The zero-based offset to the value to be retrieved.</param>
    /// <returns>If the function succeeds, the return value is the requested value.</returns>
    [DllImport(Libraries.User32, CharSet = CharSet.Auto)]
    public static extern long GetWindowLong([In] IntPtr hWnd, [In] GWL nIndex);

    /// <summary>
    /// Retrieves information about the specified window. The function also retrieves the value at a specified offset into the extra window memory.
    /// <para>Unicode declaration for <see cref="GetWindowLongPtr"/></para>
    /// </summary>
    /// <param name="hWnd">A handle to the window and, indirectly, the class to which the window belongs.</param>
    /// <param name="nIndex">The zero-based offset to the value to be retrieved.</param>
    /// <returns>If the function succeeds, the return value is the requested value.</returns>
    [DllImport(Libraries.User32, CharSet = CharSet.Auto)]
    public static extern IntPtr GetWindowLongPtrW([In] IntPtr hWnd, [In] int nIndex);

    /// <summary>
    /// Retrieves information about the specified window. The function also retrieves the value at a specified offset into the extra window memory.
    /// <para>ANSI declaration for <see cref="GetWindowLongPtr"/></para>
    /// </summary>
    /// <param name="hWnd">A handle to the window and, indirectly, the class to which the window belongs.</param>
    /// <param name="nIndex">The zero-based offset to the value to be retrieved.</param>
    /// <returns>If the function succeeds, the return value is the requested value.</returns>
    [DllImport(Libraries.User32, CharSet = CharSet.Auto)]
    public static extern IntPtr GetWindowLongPtrA([In] IntPtr hWnd, [In] int nIndex);

    /// <summary>
    /// Retrieves information about the specified window. The function also retrieves the value at a specified offset into the extra window memory.
    /// </summary>
    /// <param name="hWnd">A handle to the window and, indirectly, the class to which the window belongs.</param>
    /// <param name="nIndex">The zero-based offset to the value to be retrieved.</param>
    /// <returns>If the function succeeds, the return value is the requested value.</returns>
    [DllImport(Libraries.User32, CharSet = CharSet.Auto)]
    public static extern IntPtr GetWindowLongPtr([In] IntPtr hWnd, [In] int nIndex);

    /// <summary>
    /// Changes an attribute of the specified window. The function also sets the 32-bit (long) value at the specified offset into the extra window memory.
    /// <para>Note: This function has been superseded by the <see cref="SetWindowLongPtr"/> function. To write code that is compatible with both 32-bit and 64-bit versions of Windows, use the SetWindowLongPtr function.</para>
    /// <para>Unicode declaration for <see cref="GetWindowLongPtr"/></para>
    /// </summary>
    /// <param name="hWnd">A handle to the window and, indirectly, the class to which the window belongs.</param>
    /// <param name="nIndex">The zero-based offset to the value to be set. Valid values are in the range zero through the number of bytes of extra window memory, minus the size of an integer.</param>
    /// <param name="dwNewLong">The replacement value.</param>
    /// <returns>If the function succeeds, the return value is the previous value of the specified 32-bit integer.</returns>
    [DllImport(Libraries.User32, CharSet = CharSet.Unicode)]
    public static extern long SetWindowLongW([In] IntPtr hWnd, [In] int nIndex, [In] long dwNewLong);

    /// <summary>
    /// Changes an attribute of the specified window. The function also sets the 32-bit (long) value at the specified offset into the extra window memory.
    /// <para>Note: This function has been superseded by the <see cref="SetWindowLongPtr"/> function. To write code that is compatible with both 32-bit and 64-bit versions of Windows, use the SetWindowLongPtr function.</para>
    /// <para>ANSI declaration for <see cref="GetWindowLongPtr"/></para>
    /// </summary>
    /// <param name="hWnd">A handle to the window and, indirectly, the class to which the window belongs.</param>
    /// <param name="nIndex">The zero-based offset to the value to be set. Valid values are in the range zero through the number of bytes of extra window memory, minus the size of an integer.</param>
    /// <param name="dwNewLong">The replacement value.</param>
    /// <returns>If the function succeeds, the return value is the previous value of the specified 32-bit integer.</returns>
    [DllImport(Libraries.User32, CharSet = CharSet.Auto)]
    public static extern long SetWindowLongA([In] IntPtr hWnd, [In] int nIndex, [In] long dwNewLong);

    /// <summary>
    /// Changes an attribute of the specified window. The function also sets the 32-bit (long) value at the specified offset into the extra window memory.
    /// <para>Note: This function has been superseded by the <see cref="SetWindowLongPtr"/> function. To write code that is compatible with both 32-bit and 64-bit versions of Windows, use the SetWindowLongPtr function.</para>
    /// </summary>
    /// <param name="hWnd">A handle to the window and, indirectly, the class to which the window belongs.</param>
    /// <param name="nIndex">The zero-based offset to the value to be set. Valid values are in the range zero through the number of bytes of extra window memory, minus the size of an integer.</param>
    /// <param name="dwNewLong">The replacement value.</param>
    /// <returns>If the function succeeds, the return value is the previous value of the specified 32-bit integer.</returns>
    [DllImport(Libraries.User32, CharSet = CharSet.Auto)]
    public static extern long SetWindowLong([In] IntPtr hWnd, [In] int nIndex, [In] long dwNewLong);

    /// <summary>
    /// Changes an attribute of the specified window. The function also sets the 32-bit (long) value at the specified offset into the extra window memory.
    /// <para>Note: This function has been superseded by the <see cref="SetWindowLongPtr"/> function. To write code that is compatible with both 32-bit and 64-bit versions of Windows, use the SetWindowLongPtr function.</para>
    /// <para>ANSI declaration for <see cref="GetWindowLongPtr"/></para>
    /// </summary>
    /// <param name="hWnd">A handle to the window and, indirectly, the class to which the window belongs.</param>
    /// <param name="nIndex">The zero-based offset to the value to be set. Valid values are in the range zero through the number of bytes of extra window memory, minus the size of an integer.</param>
    /// <param name="dwNewLong">The replacement value.</param>
    /// <returns>If the function succeeds, the return value is the previous value of the specified 32-bit integer.</returns>
    [DllImport(Libraries.User32, CharSet = CharSet.Auto)]
    public static extern long SetWindowLong([In] IntPtr hWnd, [In] GWL nIndex, [In] long dwNewLong);

    /// <summary>
    /// Changes an attribute of the specified window. The function also sets the 32-bit (long) value at the specified offset into the extra window memory.
    /// <para>Note: This function has been superseded by the <see cref="SetWindowLongPtr"/> function. To write code that is compatible with both 32-bit and 64-bit versions of Windows, use the SetWindowLongPtr function.</para>
    /// <para>ANSI declaration for <see cref="GetWindowLongPtr"/></para>
    /// </summary>
    /// <param name="hWnd">A handle to the window and, indirectly, the class to which the window belongs.</param>
    /// <param name="nIndex">The zero-based offset to the value to be set. Valid values are in the range zero through the number of bytes of extra window memory, minus the size of an integer.</param>
    /// <param name="dwNewLong">New window style.</param>
    /// <returns>If the function succeeds, the return value is the previous value of the specified 32-bit integer.</returns>
    [DllImport(Libraries.User32, CharSet = CharSet.Auto)]
    public static extern long SetWindowLong([In] IntPtr hWnd, [In] GWL nIndex, [In] WS dwNewLong);

    /// <summary>
    /// Changes an attribute of the specified window. The function also sets a value at the specified offset in the extra window memory.
    /// <para>Unicode declaration for <see cref="SetWindowLongPtr"/></para>
    /// </summary>
    /// <param name="hWnd">A handle to the window and, indirectly, the class to which the window belongs.</param>
    /// <param name="nIndex">The zero-based offset to the value to be set.</param>
    /// <param name="dwNewLong">The replacement value.</param>
    /// <returns>If the function succeeds, the return value is the previous value of the specified offset.</returns>
    [DllImport(Libraries.User32, CharSet = CharSet.Auto)]
    public static extern IntPtr SetWindowLongPtrW([In] IntPtr hWnd, [In] int nIndex, [In] IntPtr dwNewLong);

    /// <summary>
    /// Changes an attribute of the specified window. The function also sets a value at the specified offset in the extra window memory.
    /// <para>ANSI declaration for <see cref="SetWindowLongPtr"/></para>
    /// </summary>
    /// <param name="hWnd">A handle to the window and, indirectly, the class to which the window belongs.</param>
    /// <param name="nIndex">The zero-based offset to the value to be set.</param>
    /// <param name="dwNewLong">The replacement value.</param>
    /// <returns>If the function succeeds, the return value is the previous value of the specified offset.</returns>
    [DllImport(Libraries.User32, CharSet = CharSet.Auto)]
    public static extern IntPtr SetWindowLongPtrA([In] IntPtr hWnd, [In] int nIndex, [In] IntPtr dwNewLong);

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
    /// Destroys an icon and frees any memory the icon occupied.
    /// </summary>
    /// <param name="handle">A handle to the icon to be destroyed. The icon must not be in use.</param>
    /// <returns>If the function succeeds, the return value is nonzero.</returns>
    [DllImport(Libraries.User32)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool DestroyIcon([In] IntPtr handle);

    /// <summary>
    /// Determines whether the specified window handle identifies an existing window.
    /// </summary>
    /// <param name="hWnd">A handle to the window to be tested.</param>
    /// <returns>If the window handle identifies an existing window, the return value is nonzero.</returns>
    [DllImport(Libraries.User32, CharSet = CharSet.Auto)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool IsWindow([In] IntPtr hWnd);

    /// <summary>
    /// Destroys the specified window. The function sends WM_DESTROY and WM_NCDESTROY messages to the window to deactivate it and remove the keyboard focus from it.
    /// </summary>
    /// <param name="hWnd">A handle to the window to be destroyed.</param>
    /// <returns>If the function succeeds, the return value is nonzero.</returns>
    [DllImport(Libraries.User32, CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool DestroyWindow([In] IntPtr hWnd);

    /// <summary>
    /// Retrieves the show state and the restored, minimized, and maximized positions of the specified window.
    /// </summary>
    /// <param name="hWnd">A handle to the window.</param>
    /// <param name="lpwndpl">A pointer to the <see cref="WINDOWPLACEMENT"/> structure that receives the show state and position information. Before calling GetWindowPlacement, set the length member to sizeof(WINDOWPLACEMENT). GetWindowPlacement fails if lpwndpl-> length is not set correctly.</param>
    /// <returns>If the function succeeds, the return value is nonzero.</returns>
    [DllImport(Libraries.User32, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool GetWindowPlacement([In] IntPtr hWnd, [In] WINDOWPLACEMENT lpwndpl);

    /// <summary>
    /// Retrieves the dimensions of the bounding rectangle of the specified window. The dimensions are given in screen coordinates that are relative to the upper-left corner of the screen.
    /// </summary>
    /// <param name="hWnd">A handle to the window.</param>
    /// <param name="lpRect">A pointer to a RECT structure that receives the screen coordinates of the upper-left and lower-right corners of the window.</param>
    /// <returns>If the function succeeds, the return value is nonzero.</returns>
    [DllImport(Libraries.User32, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool GetWindowRect([In] IntPtr hWnd, [Out] out Rect lpRect);

    /// <summary>
    /// Determines the visibility state of the specified window.
    /// </summary>
    /// <param name="hWnd">A handle to the window to be tested.</param>
    /// <returns>If the specified window, its parent window, its parent's parent window, and so forth, have the WS_VISIBLE style, the return value is nonzero. Otherwise, the return value is zero.</returns>
    [DllImport(Libraries.User32)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool IsWindowVisible([In] IntPtr hWnd);

    /// <summary>
    /// Determines whether the specified window is enabled for mouse and keyboard input.
    /// </summary>
    /// <param name="hWnd">A handle to the window to be tested.</param>
    /// <returns>If the window is enabled, the return value is nonzero.</returns>
    [DllImport(Libraries.User32, ExactSpelling = true)]
    internal static extern bool IsWindowEnabled(IntPtr hWnd);

    /// <summary>
    /// The MonitorFromWindow function retrieves a handle to the display monitor that has the largest area of intersection with the bounding rectangle of a specified window.
    /// </summary>
    /// <param name="hWnd">A handle to the window of interest.</param>
    /// <param name="dwFlags">Determines the function's return value if the window does not intersect any display monitor.</param>
    /// <returns>If the window intersects one or more display monitor rectangles, the return value is an HMONITOR handle to the display monitor that has the largest area of intersection with the window.</returns>
    [DllImport(Libraries.User32)]
    public static extern IntPtr MonitorFromWindow(IntPtr hWnd, uint dwFlags);

    /// <summary>
    /// Retrieves the specified system metric or system configuration setting.
    /// Note that all dimensions retrieved by GetSystemMetrics are in pixels.
    /// </summary>
    /// <param name="nIndex">The system metric or configuration setting to be retrieved. This parameter can be one of the <see cref="SM"/> values.
    /// Note that all SM_CX* values are widths and all SM_CY* values are heights. Also note that all settings designed to return Boolean data represent <see langword="true"/> as any nonzero value, and <see langword="false"/> as a zero value.</param>
    /// <returns>If the function succeeds, the return value is the requested system metric or configuration setting.</returns>
    [DllImport(Libraries.User32)]
    public static extern int GetSystemMetrics([In] SM nIndex);

    /// <summary>
    /// Defines a new window message that is guaranteed to be unique throughout the system. The message value can be used when sending or posting messages.
    /// <para>Unicode declaration for <see cref="RegisterWindowMessage"/></para>
    /// </summary>
    /// <param name="lpString">The message to be registered.</param>
    /// <returns>If the message is successfully registered, the return value is a message identifier in the range 0xC000 through 0xFFFF.</returns>
    [DllImport(Libraries.User32, SetLastError = true, CharSet = CharSet.Unicode)]
    public static extern uint RegisterWindowMessageW([MarshalAs(UnmanagedType.LPWStr)] string lpString);

    /// <summary>
    /// Defines a new window message that is guaranteed to be unique throughout the system. The message value can be used when sending or posting messages.
    /// <para>ANSI declaration for <see cref="RegisterWindowMessage"/></para>
    /// </summary>
    /// <param name="lpString">The message to be registered.</param>
    /// <returns>If the message is successfully registered, the return value is a message identifier in the range 0xC000 through 0xFFFF.</returns>
    [DllImport(Libraries.User32, SetLastError = true, CharSet = CharSet.Auto)]
    public static extern uint RegisterWindowMessageA([MarshalAs(UnmanagedType.LPWStr)] string lpString);

    /// <summary>
    /// Defines a new window message that is guaranteed to be unique throughout the system. The message value can be used when sending or posting messages.
    /// </summary>
    /// <param name="lpString">The message to be registered.</param>
    /// <returns>If the message is successfully registered, the return value is a message identifier in the range 0xC000 through 0xFFFF.</returns>
    [DllImport(Libraries.User32, SetLastError = true, CharSet = CharSet.Auto)]
    public static extern uint RegisterWindowMessage([MarshalAs(UnmanagedType.LPWStr)] string lpString);

    /// <summary>
    /// Activates a window. The window must be attached to the calling thread's message queue.
    /// </summary>
    /// <param name="hWnd">A handle to the top-level window to be activated.</param>
    /// <returns>If the function succeeds, the return value is the handle to the window that was previously active.</returns>
    [DllImport(Libraries.User32, SetLastError = true)]
    public static extern IntPtr SetActiveWindow(IntPtr hWnd);

    /// <summary>
    /// Brings the thread that created the specified window into the foreground and activates the window.
    /// Keyboard input is directed to the window, and various visual cues are changed for the user.
    /// The system assigns a slightly higher priority to the thread that created the foreground window than it does to other threads.
    /// </summary>
    /// <param name="hWnd">A handle to the window that should be activated and brought to the foreground.</param>
    /// <returns>If the window was brought to the foreground, the return value is nonzero.</returns>
    [DllImport(Libraries.User32, CharSet = CharSet.Auto)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool SetForegroundWindow(IntPtr hWnd);

    /// <summary>
    /// Retrieves the position of the mouse cursor, in screen coordinates.
    /// </summary>
    /// <param name="lpPoint">A pointer to a <see cref="WinDef.POINT"/> structure that receives the screen coordinates of the cursor.</param>
    /// <returns>Returns nonzero if successful or zero otherwise. To get extended error information, call <see cref="Kernel32.GetLastError"/>.</returns>
    [DllImport(Libraries.User32, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool GetCursorPos([Out] out WinDef.POINT lpPoint);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="rcDst"></param>
    /// <param name="rc1"></param>
    /// <param name="rc2"></param>
    /// <returns></returns>
    [DllImport(Libraries.User32)]
    public static extern bool UnionRect(out WinDef.RECT rcDst, ref WinDef.RECT rc1, ref WinDef.RECT rc2);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="rcDest"></param>
    /// <param name="rc1"></param>
    /// <param name="rc2"></param>
    /// <returns></returns>
    [DllImport(Libraries.User32, SetLastError = true)]
    public static extern bool IntersectRect(ref WinDef.RECT rcDest, ref WinDef.RECT rc1, ref WinDef.RECT rc2);

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [DllImport(Libraries.User32)]
    public static extern IntPtr GetShellWindow();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="nVirtKey"></param>
    /// <param name="nMapType"></param>
    /// <returns></returns>
    [DllImport(Libraries.User32, CharSet = CharSet.Unicode)]
    public static extern int MapVirtualKey(int nVirtKey, int nMapType);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="nIndex"></param>
    /// <returns></returns>
    [DllImport(Libraries.User32)]
    public static extern int GetSysColor(int nIndex);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="hWnd"></param>
    /// <param name="bRevert"></param>
    /// <returns></returns>
    [DllImport(Libraries.User32)]
    public static extern IntPtr GetSystemMenu([In] IntPtr hWnd, [In][MarshalAs(UnmanagedType.Bool)] bool bRevert);

    [DllImport(Libraries.User32, EntryPoint = "EnableMenuItem")]
    private static extern int _EnableMenuItem([In] IntPtr hMenu, [In] SC uIDEnableItem, [In] MF uEnable);

    /// <summary>
    /// Enables, disables, or grays the specified menu item.
    /// </summary>
    /// <param name="hMenu">A handle to the menu.</param>
    /// <param name="uIDEnableItem">The menu item to be enabled, disabled, or grayed, as determined by the uEnable parameter.</param>
    /// <param name="uEnable">Controls the interpretation of the uIDEnableItem parameter and indicate whether the menu item is enabled, disabled, or grayed.</param>
    /// <returns>The return value specifies the previous state of the menu item (it is either MF_DISABLED, MF_ENABLED, or MF_GRAYED). If the menu item does not exist, the return value is -1 (<see cref="MF.DOES_NOT_EXIST"/>).</returns>
    public static MF EnableMenuItem([In] IntPtr hMenu, [In] SC uIDEnableItem, [In] MF uEnable)
    {
        // Returns the previous state of the menu item, or -1 if the menu item does not exist.
        int iRet = _EnableMenuItem(hMenu, uIDEnableItem, uEnable);
        return (MF)iRet;
    }

    [DllImport(Libraries.User32, EntryPoint = "SetWindowRgn", SetLastError = true)]
    private static extern int _SetWindowRgn([In] IntPtr hWnd, [In] IntPtr hRgn, [In][MarshalAs(UnmanagedType.Bool)] bool bRedraw);

    /// <summary>
    /// The SetWindowRgn function sets the window region of a window. The window region determines the area within the window where the system permits drawing. The system does not display any portion of a window that lies outside of the window region.
    /// </summary>
    /// <param name="hWnd">A handle to the window whose window region is to be set.</param>
    /// <param name="hRgn">A handle to a region. The function sets the window region of the window to this region.</param>
    /// <param name="bRedraw">Specifies whether the system redraws the window after setting the window region. If bRedraw is <see langword="true"/>, the system does so; otherwise, it does not.</param>
    /// <exception cref="Win32Exception">Native method returned HRESULT.</exception>
    public static void SetWindowRgn([In] IntPtr hWnd, [In] IntPtr hRgn, [In] bool bRedraw)
    {
        int err = _SetWindowRgn(hWnd, hRgn, bRedraw);

        if (0 == err)
        {
            throw new Win32Exception();
        }
    }

    [DllImport(Libraries.User32, EntryPoint = "SetWindowPos", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool _SetWindowPos([In] IntPtr hWnd, [In, Optional] IntPtr hWndInsertAfter, [In] int x, [In] int y, [In] int cx, [In] int cy, [In] SWP uFlags);

    /// <summary>
    /// Changes the size, position, and Z order of a child, pop-up, or top-level window. These windows are ordered according to their appearance on the screen. The topmost window receives the highest rank and is the first window in the Z order.
    /// </summary>
    /// <param name="hWnd">A handle to the window.</param>
    /// <param name="hWndInsertAfter">A handle to the window to precede the positioned window in the Z order.</param>
    /// <param name="x">The new position of the left side of the window, in client coordinates.</param>
    /// <param name="y">The new position of the top of the window, in client coordinates.</param>
    /// <param name="cx">The new width of the window, in pixels.</param>
    /// <param name="cy">The new height of the window, in pixels.</param>
    /// <param name="uFlags">The window sizing and positioning flags.</param>
    /// <returns>If the function succeeds, the return value is nonzero.</returns>
    public static bool SetWindowPos([In] IntPtr hWnd, [In, Optional] IntPtr hWndInsertAfter, [In] int x, [In] int y, [In] int cx, [In] int cy, [In] SWP uFlags)
    {
        if (!_SetWindowPos(hWnd, hWndInsertAfter, x, y, cx, cy, uFlags))
        {
            // If this fails it's never worth taking down the process.  Let the caller deal with the error if they want.
            return false;
        }

        return true;
    }

    /// <summary>
    /// Sets the process-default DPI awareness to system-DPI awareness. This is equivalent to calling SetProcessDpiAwarenessContext with a DPI_AWARENESS_CONTEXT value of DPI_AWARENESS_CONTEXT_SYSTEM_AWARE.
    /// </summary>
    [DllImport(Libraries.User32)]
    public static extern void SetProcessDPIAware();

    /// <summary>
    /// Sets various information regarding DWM window attributes.
    /// </summary>
    [DllImport(Libraries.User32, CharSet = CharSet.Auto)]
    public static extern int SetWindowCompositionAttribute([In] IntPtr hWnd, [In, Out] ref WINCOMPATTRDATA data);

    /// <summary>
    /// Sets various information regarding DWM window attributes.
    /// </summary>
    [DllImport(Libraries.User32, CharSet = CharSet.Auto)]
    public static extern int GetWindowCompositionAttribute([In] IntPtr hWnd, [In, Out] ref WINCOMPATTRDATA data);

    /// <summary>
    /// Returns the dots per inch (dpi) value for the specified window.
    /// </summary>
    /// <param name="hWnd">The window that you want to get information about.</param>
    /// <returns>The DPI for the window, which depends on the DPI_AWARENESS of the window.</returns>
    [DllImport(Libraries.User32, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Winapi)]
    public static extern uint GetDpiForWindow([In] IntPtr hWnd);

    /// <summary>
    /// Returns the dots per inch (dpi) value for the specified window.
    /// </summary>
    /// <param name="hwnd">The window that you want to get information about.</param>
    /// <returns>The DPI for the window, which depends on the DPI_AWARENESS of the window.</returns>
    [DllImport(Libraries.User32, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Winapi)]
    public static extern uint GetDpiForWindow([In] HandleRef hwnd);
}
