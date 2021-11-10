// Show Progress on Windows Taskbar Icon
// Copyright (C) 2014 Sean Sexton
// https://wpf.2000things.com/2014/03/19/1032-show-progress-on-windows-taskbar-icon/

using System;

namespace WPFUI.Taskbar
{
    [Flags]
    internal enum ThumbButtonOptions
    {
        Enabled = 0x00000000,
        Disabled = 0x00000001,
        DismissOnClick = 0x00000002,
        NoBackground = 0x00000004,
        Hidden = 0x00000008,
        NonInteractive = 0x00000010
    }
}
