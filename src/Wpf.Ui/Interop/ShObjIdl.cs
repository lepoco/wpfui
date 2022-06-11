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

namespace Wpf.Ui.Interop;

/// <summary>
/// Exposes methods that enumerate the contents of a view and receive notification from callback upon enumeration completion.
/// </summary>
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
internal static class ShObjIdl
{
    /// <summary>
    /// THUMBBUTTON flags.  THBF_*
    /// </summary>
    [Flags]
    public enum THUMBBUTTONFLAGS
    {
        THBF_ENABLED = 0,
        THBF_DISABLED = 0x1,
        THBF_DISMISSONCLICK = 0x2,
        THBF_NOBACKGROUND = 0x4,
        THBF_HIDDEN = 0x8,
        THBF_NONINTERACTIVE = 0x10
    }

    /// <summary>
    /// THUMBBUTTON mask.  THB_*
    /// </summary>
    [Flags]
    public enum THUMBBUTTONMASK
    {
        THB_BITMAP = 0x1,
        THB_ICON = 0x2,
        THB_TOOLTIP = 0x4,
        THB_FLAGS = 0x8
    }

    /// <summary>
    /// TBPF_*
    /// </summary>
    [Flags]
    public enum TBPFLAG
    {
        TBPF_NOPROGRESS = 0,
        TBPF_INDETERMINATE = 0x1,
        TBPF_NORMAL = 0x2,
        TBPF_ERROR = 0x4,
        TBPF_PAUSED = 0x8
    }

    /// <summary>
    /// STPF_*
    /// </summary>
    [Flags]
    public enum STPFLAG
    {
        STPF_NONE = 0,
        STPF_USEAPPTHUMBNAILALWAYS = 0x1,
        STPF_USEAPPTHUMBNAILWHENACTIVE = 0x2,
        STPF_USEAPPPEEKALWAYS = 0x4,
        STPF_USEAPPPEEKWHENACTIVE = 0x8
    }

    /// <summary>
    /// EBO_*
    /// </summary>
    public enum EXPLORER_BROWSER_OPTIONS
    {
        EBO_NONE = 0,
        EBO_NAVIGATEONCE = 0x1,
        EBO_SHOWFRAMES = 0x2,
        EBO_ALWAYSNAVIGATE = 0x4,
        EBO_NOTRAVELLOG = 0x8,
        EBO_NOWRAPPERWINDOW = 0x10,
        EBO_HTMLSHAREPOINTVIEW = 0x20,
        EBO_NOBORDER = 0x40,
        EBO_NOPERSISTVIEWSTATE = 0x80
    }

    /// <summary>
    /// EBF_*
    /// </summary>
    public enum EXPLORER_BROWSER_FILL_FLAGS
    {
        EBF_NONE = 0,
        EBF_SELECTFROMDATAOBJECT = 0x100,
        EBF_NODROPTARGET = 0x200
    }

    /// <summary>
    /// THUMBBUTTON
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 8, CharSet = CharSet.Unicode)]
    public struct THUMBBUTTON
    {
        /// <summary>
        /// WPARAM value for a THUMBBUTTON being clicked.
        /// </summary>
        public const int THBN_CLICKED = 0x1800;

        public THUMBBUTTONMASK dwMask;
        public uint iId;
        public uint iBitmap;
        public IntPtr hIcon;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        public string szTip;

        public THUMBBUTTONFLAGS dwFlags;
    }

    /// <summary>
    /// Class DECLSPEC_UUID("56FDF344-FD6D-11d0-958A-006097C9A090")
    /// </summary>
    [Guid("56FDF344-FD6D-11d0-958A-006097C9A090")]
    [ClassInterface(ClassInterfaceType.None)]
    [ComImport]
    public class CTaskbarList
    {
    }

    /// <summary>
    /// Class DECLSPEC_UUID("9ac9fbe1-e0a2-4ad6-b4ee-e212013ea917")
    /// </summary>
    [Guid("9ac9fbe1-e0a2-4ad6-b4ee-e212013ea917")]
    [ClassInterface(ClassInterfaceType.None)]
    [ComImport]
    public class ShellItem
    {
    }

    /// <summary>
    /// MIDL_INTERFACE("c43dc798-95d1-4bea-9030-bb99e2983a1a")
    /// ITaskbarList4 : public ITaskbarList3
    /// </summary>
    [ComImport]
    [Guid("c43dc798-95d1-4bea-9030-bb99e2983a1a")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface ITaskbarList4
    {
        // ITaskbarList
        [PreserveSig]
        void HrInit();
        [PreserveSig]
        void AddTab(IntPtr hwnd);
        [PreserveSig]
        void DeleteTab(IntPtr hwnd);
        [PreserveSig]
        void ActivateTab(IntPtr hwnd);
        [PreserveSig]
        void SetActiveAlt(IntPtr hwnd);

        // ITaskbarList2
        [PreserveSig]
        void MarkFullscreenWindow(
            IntPtr hwnd,
            [MarshalAs(UnmanagedType.Bool)] bool fFullscreen);

        // ITaskbarList3
        [PreserveSig]
        void SetProgressValue(IntPtr hwnd, UInt64 ullCompleted, UInt64 ullTotal);
        [PreserveSig]
        void SetProgressState(IntPtr hwnd, TBPFLAG tbpFlags);
        [PreserveSig]
        void RegisterTab(IntPtr hwndTab, IntPtr hwndMDI);
        [PreserveSig]
        void UnregisterTab(IntPtr hwndTab);
        [PreserveSig]
        void SetTabOrder(IntPtr hwndTab, IntPtr hwndInsertBefore);
        [PreserveSig]
        void SetTabActive(IntPtr hwndTab, IntPtr hwndInsertBefore, uint dwReserved);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="cButtons"></param>
        /// <param name="pButtons"></param>
        /// <returns>HRESULT</returns>
        [PreserveSig]
        int ThumbBarAddButtons(
            IntPtr hwnd,
            uint cButtons,
            [MarshalAs(UnmanagedType.LPArray)] THUMBBUTTON[] pButtons);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="cButtons"></param>
        /// <param name="pButtons"></param>
        /// <returns>HRESULT</returns>
        [PreserveSig]
        int ThumbBarUpdateButtons(
            IntPtr hwnd,
            uint cButtons,
            [MarshalAs(UnmanagedType.LPArray)] THUMBBUTTON[] pButtons);
        [PreserveSig]
        void ThumbBarSetImageList(IntPtr hWnd, IntPtr himl);
        [PreserveSig]
        void SetOverlayIcon(
          IntPtr hwnd,
          IntPtr hIcon,
          [MarshalAs(UnmanagedType.LPWStr)] string pszDescription);
        [PreserveSig]
        void SetThumbnailTooltip(
            IntPtr hwnd,
            [MarshalAs(UnmanagedType.LPWStr)] string pszTip);
        [PreserveSig]
        void SetThumbnailClip(
            IntPtr hwnd,
            IntPtr prcClip);

        // ITaskbarList4
        void SetTabProperties(IntPtr hwndTab, STPFLAG stpFlags);
    }
}
