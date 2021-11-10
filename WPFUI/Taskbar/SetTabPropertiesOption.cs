// Show Progress on Windows Taskbar Icon
// Copyright (C) 2014 Sean Sexton
// https://wpf.2000things.com/2014/03/19/1032-show-progress-on-windows-taskbar-icon/

namespace WPFUI.Taskbar
{
    internal enum SetTabPropertiesOption
    {
        None = 0x0,
        UseAppThumbnailAlways = 0x1,
        UseAppThumbnailWhenActive = 0x2,
        UseAppPeekAlways = 0x4,
        UseAppPeekWhenActive = 0x8
    }
}
