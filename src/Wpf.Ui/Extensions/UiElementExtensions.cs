using System;
using System.Windows;

namespace Wpf.Ui.Extensions;

internal static class UiElementExtensions
{
    /// <summary>
    /// Do not call it outside of NCHITTEST, NCLBUTTONUP, NCLBUTTONDOWN messages!
    /// </summary>
    public static bool IsMouseOverElement(this UIElement element, IntPtr lParam)
    {
        // This method will be invoked very often and must be as simple as possible.

        if (lParam == IntPtr.Zero)
            return false;

        var mousePosScreen = new Point(Get_X_LParam(lParam), Get_Y_LParam(lParam));
        var bounds = new Rect(new Point(), element.RenderSize);
        var mousePosRelative = element.PointFromScreen(mousePosScreen);
        return bounds.Contains(mousePosRelative);
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
