// This Source Code is partially based on the source code provided by the .NET Foundation.
// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) .NET Foundation Contributors, WPF UI Contributors, Leszek Pomianowski.
// All Rights Reserved.

using System;

namespace Wpf.Ui.Dpi;

/// <summary>
/// Stores DPI information from which a System.Windows.Media.Visual or System.Windows.UIElement
/// is rendered.
/// </summary>
public struct Dpi
{
    /// <summary>
    /// Initializes a new instance of the System.Windows.DpiScale structure.
    /// </summary>
    /// <param name="dpiScaleX">The DPI scale on the X axis.</param>
    /// <param name="dpiScaleY">The DPI scale on the Y axis.</param>
    public Dpi(double dpiScaleX, double dpiScaleY)
    {
        DpiScaleX = dpiScaleX;
        DpiScaleY = dpiScaleY;

        DpiX = (int)Math.Round(DpiHelper.DefaultDpi * dpiScaleX, MidpointRounding.AwayFromZero);
        DpiY = (int)Math.Round(DpiHelper.DefaultDpi * dpiScaleY, MidpointRounding.AwayFromZero);
    }

    /// <summary>
    /// Initializes a new instance of the System.Windows.DpiScale structure.
    /// </summary>
    /// <param name="dpiX">The DPI on the X axis.</param>
    /// <param name="dpiY">The DPI on the Y axis.</param>
    public Dpi(int dpiX, int dpiY)
    {
        DpiX = dpiX;
        DpiY = dpiY;

        DpiScaleX = dpiX / (double)DpiHelper.DefaultDpi;
        DpiScaleY = dpiY / (double)DpiHelper.DefaultDpi;
    }

    /// <summary>
    /// Gets the DPI on the X axis.
    /// </summary>
    public int DpiX { get; }

    /// <summary>
    /// Gets the DPI on the Y axis.
    /// </summary>
    public int DpiY { get; }

    /// <summary>
    /// Gets the DPI scale on the X axis.
    /// </summary>
    public double DpiScaleX { get; }

    /// <summary>
    /// Gets the DPI scale on the Y axis.
    /// </summary>
    public double DpiScaleY { get; }
}

