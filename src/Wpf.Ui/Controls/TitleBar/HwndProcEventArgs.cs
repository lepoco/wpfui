// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

public class HwndProcEventArgs : EventArgs
{
    public bool Handled { get; set; }

    public IntPtr? ReturnValue { get; set; }

    public bool IsMouseOverDetectedHeaderContent { get; }

    public IntPtr HWND { get; }

    public int Message { get; }

    public IntPtr WParam { get; }

    public IntPtr LParam { get; }

    internal HwndProcEventArgs(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, bool isMouseOverDetectedHeaderContent)
    {
        HWND = hwnd;
        Message = msg;
        WParam = wParam;
        LParam = lParam;
        IsMouseOverDetectedHeaderContent = isMouseOverDetectedHeaderContent;
    }
}
