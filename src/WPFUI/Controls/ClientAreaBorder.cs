// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

#nullable enable
using System;
using System.Windows;
using System.Windows.Shell;
using WPFUI.Common;

namespace WPFUI.Controls;

/// <summary>
/// If you use <see cref="WindowChrome"/> to extend the UI elements to the non-client area, you can include this container in the template of <see cref="Window"/> so that the content inside automatically fills the client area.
/// Using this container can let you get rid of various margin adaptations done in Setter/Trigger of the style of <see cref="Window"/> when the window state changes.
/// </summary>
public class ClientAreaBorder : System.Windows.Controls.Border
{
    private const int SM_CXFRAME = 32;
    private const int SM_CYFRAME = 33;
    private const int SM_CXPADDEDBORDER = 92;

    private Window? _oldWindow;
    private static Thickness? _paddedBorderThickness;
    private static Thickness? _resizeFrameBorderThickness;
    private static Thickness? _windowChromeNonClientFrameThickness;

    /// <inheritdoc />
    protected override void OnVisualParentChanged(DependencyObject oldParent)
    {
        base.OnVisualParentChanged(oldParent);

        if (_oldWindow is { } oldWindow)
        {
            oldWindow.StateChanged -= Window_StateChanged;
        }

        var newWindow = (Window?)Window.GetWindow(this);
        if (newWindow is not null)
        {
            newWindow.StateChanged -= Window_StateChanged;
            newWindow.StateChanged += Window_StateChanged;
        }

        _oldWindow = newWindow;
    }

    private void Window_StateChanged(object? sender, EventArgs e)
    {
        var window = (Window)sender!;
        Padding = window.WindowState switch
        {
            WindowState.Maximized => WindowChromeNonClientFrameThickness,
            _ => default,
        };
    }

    /// <summary>
    /// Get the system <see cref="SM_CXPADDEDBORDER"/> value in WPF units.
    /// </summary>
    public Thickness PaddedBorderThickness
    {
        get
        {
            if (_paddedBorderThickness is null)
            {
                var paddedBorder = Interop.User32.GetSystemMetrics(
                    Interop.User32.SM.CXPADDEDBORDER);

                var (factorX, factorY) = GetDpi();
                var frameSize = new Size(paddedBorder, paddedBorder);
                var frameSizeInDips = new Size(frameSize.Width / factorX, frameSize.Height / factorY);
                _paddedBorderThickness = new Thickness(frameSizeInDips.Width, frameSizeInDips.Height,
                    frameSizeInDips.Width, frameSizeInDips.Height);
            }

            return _paddedBorderThickness.Value;
        }
    }

    /// <summary>
    /// Get the system <see cref="SM_CXFRAME"/> and <see cref="SM_CYFRAME"/> values in WPF units.
    /// </summary>
    public Thickness ResizeFrameBorderThickness => _resizeFrameBorderThickness ??= new Thickness(
        SystemParameters.ResizeFrameVerticalBorderWidth,
        SystemParameters.ResizeFrameHorizontalBorderHeight,
        SystemParameters.ResizeFrameVerticalBorderWidth,
        SystemParameters.ResizeFrameHorizontalBorderHeight);

    /// <summary>
    /// If you use a <see cref="WindowChrome"/> to extend the client area of a window to the non-client area, you need to handle the edge margin issue when the window is maximized.
    /// Use this property to get the correct margin value when the window is maximized, so that when the window is maximized, the client area can completely cover the screen client area by no less than a single pixel at any DPI.
    /// The<see cref="GetSystemMetrics"/> method cannot obtain this value directly.
    /// </summary>
    public Thickness WindowChromeNonClientFrameThickness => _windowChromeNonClientFrameThickness ??= new Thickness(
        ResizeFrameBorderThickness.Left + PaddedBorderThickness.Left,
        ResizeFrameBorderThickness.Top + PaddedBorderThickness.Top,
        ResizeFrameBorderThickness.Right + PaddedBorderThickness.Right,
        ResizeFrameBorderThickness.Bottom + PaddedBorderThickness.Bottom);

    private (double factorX, double factorY) GetDpi() => PresentationSource.FromVisual(this) is { } source
        ? (source.CompositionTarget.TransformToDevice.M11,
            source.CompositionTarget.TransformToDevice.M22)
        : (DpiHelper.SystemDpiXScale(), DpiHelper.SystemDpiYScale());
}
