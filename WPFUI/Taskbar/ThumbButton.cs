// Show Progress on Windows Taskbar Icon
// Copyright (C) 2014 Sean Sexton
// https://wpf.2000things.com/2014/03/19/1032-show-progress-on-windows-taskbar-icon/

using System;
using System.Runtime.InteropServices;

namespace WPFUI.Taskbar
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    internal struct ThumbButton
    {
        /// <summary>
        /// WPARAM value for a THUMBBUTTON being clicked.
        /// </summary>
        internal const int Clicked = 0x1800;

        [MarshalAs(UnmanagedType.U4)]
        internal ThumbButtonMask Mask;
        internal uint Id;
        internal uint Bitmap;
        internal IntPtr Icon;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        internal string Tip;
        [MarshalAs(UnmanagedType.U4)]
        internal ThumbButtonOptions Flags;
    }
}
