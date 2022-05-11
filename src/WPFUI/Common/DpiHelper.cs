// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// This Source Code is partially based on the source code provided by the .NET Foundation.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Windows;
using System.Windows.Media;

namespace WPFUI.Common;

/// <summary>
/// Provides access to various DPI-related methods.
/// </summary>
internal class DpiHelper
{
    [ThreadStatic]
    private static Matrix _transformToDevice;

    [ThreadStatic]
    private static Matrix _transformToDip;

    /// <summary>
    /// Default DPI value.
    /// </summary>
    private const double DefaultDpi = 96.0d;

    // TODO: Look into utilizing preprocessor symbols for more functionality
    // ----
    // There is an opportunity to check against NET46 if we can use
    // VisualTreeHelper in this class. We are currently not utilizing
    // it because it is not available in .NET Framework 4.6 (available
    // starting 4.6.2). For now, there is no need to overcomplicate this
    // solution for some infrequent DPI calculations. However, if this
    // becomes more central to various implementations, we may want to
    // look into fleshing it out a bit further.
    // ----
    // Reference: https://docs.microsoft.com/en-us/dotnet/standard/frameworks

    /// <summary>
    /// Gets the horizontal DPI value from <see cref="SystemParameters"/>.
    /// </summary>
    /// <returns>The horizontal DPI value from <see cref="SystemParameters"/>. If the property cannot be accessed, the default value 96 is returned.</returns>
    public static int SystemDpiX()
    {
        var dpiProperty = typeof(SystemParameters).GetProperty("DpiX",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);

        if (dpiProperty == null)
            return (int)DefaultDpi;

        return (int)dpiProperty.GetValue(null, null)!;
    }

    /// <summary>
    /// Gets the horizontal DPI scale factor based on <see cref="SystemParameters"/>.
    /// </summary>
    /// <returns>The horizontal DPI scale factor.</returns>
    public static double SystemDpiXScale()
    {
        return SystemDpiX() / DefaultDpi;
    }

    /// <summary>
    /// Gets the vertical DPI value from <see cref="SystemParameters"/>.
    /// </summary>
    /// <returns>The vertical DPI value from <see cref="SystemParameters"/>. If the property cannot be accessed, the default value 96 is returned.</returns>
    public static int SystemDpiY()
    {
        var dpiProperty = typeof(SystemParameters).GetProperty("Dpi",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);

        if (dpiProperty == null)
            return (int)DefaultDpi;

        return (int)dpiProperty.GetValue(null, null)!;
    }

    /// <summary>
    /// Gets the vertical DPI scale factor based on <see cref="SystemParameters"/>.
    /// </summary>
    /// <returns>The vertical DPI scale factor.</returns>
    public static double SystemDpiYScale()
    {
        return SystemDpiY() / DefaultDpi;
    }

    /// <summary>
    /// Convert a point in device independent pixels (1/96") to a point in the system coordinates.
    /// </summary>
    /// <param name="logicalPoint">A point in the logical coordinate system.</param>
    /// <returns>Returns the parameter converted to the system's coordinates.</returns>
    public static Point LogicalPixelsToDevice(Point logicalPoint, double dpiScaleX, double dpiScaleY)
    {
        _transformToDevice = Matrix.Identity;
        _transformToDevice.Scale(dpiScaleX, dpiScaleY);
        return _transformToDevice.Transform(logicalPoint);
    }

    /// <summary>
    /// Convert a point in system coordinates to a point in device independent pixels (1/96").
    /// </summary>
    /// <param name="logicalPoint">A point in the physical coordinate system.</param>
    /// <returns>Returns the parameter converted to the device independent coordinate system.</returns>
    public static Point DevicePixelsToLogical(Point devicePoint, double dpiScaleX, double dpiScaleY)
    {
        _transformToDip = Matrix.Identity;
        _transformToDip.Scale(1d / dpiScaleX, 1d / dpiScaleY);
        return _transformToDip.Transform(devicePoint);
    }

    public static Rect LogicalRectToDevice(Rect logicalRectangle, double dpiScaleX, double dpiScaleY)
    {
        Point topLeft = LogicalPixelsToDevice(new Point(logicalRectangle.Left, logicalRectangle.Top), dpiScaleX, dpiScaleY);
        Point bottomRight = LogicalPixelsToDevice(new Point(logicalRectangle.Right, logicalRectangle.Bottom), dpiScaleX, dpiScaleY);

        return new Rect(topLeft, bottomRight);
    }

    public static Rect DeviceRectToLogical(Rect deviceRectangle, double dpiScaleX, double dpiScaleY)
    {
        Point topLeft = DevicePixelsToLogical(new Point(deviceRectangle.Left, deviceRectangle.Top), dpiScaleX, dpiScaleY);
        Point bottomRight = DevicePixelsToLogical(new Point(deviceRectangle.Right, deviceRectangle.Bottom), dpiScaleX, dpiScaleY);

        return new Rect(topLeft, bottomRight);
    }

    public static Size LogicalSizeToDevice(Size logicalSize, double dpiScaleX, double dpiScaleY)
    {
        Point pt = LogicalPixelsToDevice(new Point(logicalSize.Width, logicalSize.Height), dpiScaleX, dpiScaleY);

        return new Size { Width = pt.X, Height = pt.Y };
    }

    public static Size DeviceSizeToLogical(Size deviceSize, double dpiScaleX, double dpiScaleY)
    {
        Point pt = DevicePixelsToLogical(new Point(deviceSize.Width, deviceSize.Height), dpiScaleX, dpiScaleY);

        return new Size(pt.X, pt.Y);
    }

    public static Thickness LogicalThicknessToDevice(Thickness logicalThickness, double dpiScaleX, double dpiScaleY)
    {
        Point topLeft = LogicalPixelsToDevice(new Point(logicalThickness.Left, logicalThickness.Top), dpiScaleX, dpiScaleY);
        Point bottomRight = LogicalPixelsToDevice(new Point(logicalThickness.Right, logicalThickness.Bottom), dpiScaleX, dpiScaleY);

        return new Thickness(topLeft.X, topLeft.Y, bottomRight.X, bottomRight.Y);
    }
}

