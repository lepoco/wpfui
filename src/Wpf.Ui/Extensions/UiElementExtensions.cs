// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

namespace Wpf.Ui.Extensions;

internal static class UiElementExtensions
{
    /// <summary>
    /// Do not call it outside of NCHITTEST, NCLBUTTONUP, NCLBUTTONDOWN messages!
    /// </summary>
    /// <returns><see langword="true"/> if mouse is over the element. <see langword="false"/> otherwise.</returns>
    public static bool IsMouseOverElement(this UIElement element, IntPtr lParam)
    {
        // This method will be invoked very often and must be as simple as possible.
        if (lParam == IntPtr.Zero)
        {
            return false;
        }

        try
        {
            var mousePosScreen = new Point(Get_X_LParam(lParam), Get_Y_LParam(lParam));
            var bounds = new Rect(default, element.RenderSize);

            Point mousePosRelative = element.PointFromScreen(mousePosScreen);

            return bounds.Contains(mousePosRelative) && element.IsHitTestVisible;
        }
        catch
        {
            return false;
        }
    }

    private static int Get_X_LParam(IntPtr lParam)
    {
        return (short)(lParam.ToInt32() & 0xFFFF);
    }

    private static int Get_Y_LParam(IntPtr lParam)
    {
        return (short)(lParam.ToInt32() >> 16);
    }
}
