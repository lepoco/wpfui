// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

#nullable enable

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shell;
using Wpf.Ui.Appearance;
using Wpf.Ui.Dpi;
using Wpf.Ui.Interop;
using Size = System.Windows.Size;

namespace Wpf.Ui.Controls;

/// <summary>
/// If you use <see cref="WindowChrome"/> to extend the UI elements to the non-client area, you can include this container
/// in the template of <see cref="Window"/> so that the content inside automatically fills the client area.
/// Using this container can let you get rid of various margin adaptations done in
/// Setter/Trigger of the style of <see cref="Window"/> when the window state changes.
/// </summary>
public class ClientAreaBorder : System.Windows.Controls.Border, IThemeControl
{
    private bool _borderBrushApplied = false;

    private const int SM_CXFRAME = 32;

    private const int SM_CYFRAME = 33;

    private const int SM_CXPADDEDBORDER = 92;

    private System.Windows.Window? _oldWindow;

    private static Thickness? _paddedBorderThickness;

    private static Thickness? _resizeFrameBorderThickness;

    private static Thickness? _windowChromeNonClientFrameThickness;

    public ThemeType Theme { get; set; } = ThemeType.Unknown;

    /// <summary>
    /// Get the system <see cref="SM_CXPADDEDBORDER"/> value in WPF units.
    /// </summary>
    public Thickness PaddedBorderThickness
    {
        get
        {
            if (_paddedBorderThickness is not null)
                return _paddedBorderThickness.Value;

            var paddedBorder = Interop.User32.GetSystemMetrics(
                Interop.User32.SM.CXPADDEDBORDER);

            var (factorX, factorY) = GetDpi();
            var frameSize = new Size(paddedBorder, paddedBorder);
            var frameSizeInDips = new Size(frameSize.Width / factorX, frameSize.Height / factorY);

            _paddedBorderThickness = new Thickness(frameSizeInDips.Width, frameSizeInDips.Height,
                frameSizeInDips.Width, frameSizeInDips.Height);

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
    /// The<see cref="User32.GetSystemMetrics"/> method cannot obtain this value directly.
    /// </summary>
    public Thickness WindowChromeNonClientFrameThickness => _windowChromeNonClientFrameThickness ??= new Thickness(
        ResizeFrameBorderThickness.Left + PaddedBorderThickness.Left,
        ResizeFrameBorderThickness.Top + PaddedBorderThickness.Top,
        ResizeFrameBorderThickness.Right + PaddedBorderThickness.Right,
        ResizeFrameBorderThickness.Bottom + PaddedBorderThickness.Bottom);

    public ClientAreaBorder()
    {
        Theme = Appearance.Theme.GetAppTheme();
        Appearance.Theme.Changed += OnThemeChanged;
    }

    private void OnThemeChanged(ThemeType currentTheme, Color systemAccent)
    {
        Theme = currentTheme;

        if (!_borderBrushApplied || _oldWindow == null)
            return;

        ApplyDefaultWindowBorder();
    }

    /// <inheritdoc />
    protected override void OnVisualParentChanged(DependencyObject oldParent)
    {
        base.OnVisualParentChanged(oldParent);

        if (_oldWindow is { } oldWindow)
        {
            oldWindow.StateChanged -= OnWindowStateChanged;
            oldWindow.Closing -= OnWindowClosing;
        }

        var newWindow = (System.Windows.Window?)System.Windows.Window.GetWindow(this);

        if (newWindow is not null)
        {
            newWindow.StateChanged -= OnWindowStateChanged; // Unsafe
            newWindow.StateChanged += OnWindowStateChanged;
            newWindow.Closing += OnWindowClosing;
        }

        _oldWindow = newWindow;

        ApplyDefaultWindowBorder();
    }
    private void OnWindowClosing(object? sender, CancelEventArgs e)
    {
        Appearance.Theme.Changed -= OnThemeChanged;
        if (_oldWindow != null)
            _oldWindow.Closing -= OnWindowClosing;
    }
    private void OnWindowStateChanged(object? sender, EventArgs e)
    {
        if (sender is not System.Windows.Window window)
            return;

        Padding = window.WindowState switch
        {
            WindowState.Maximized => WindowChromeNonClientFrameThickness,
            _ => default,
        };
    }

    private void ApplyDefaultWindowBorder()
    {
        if (Win32.Utilities.IsOSWindows11OrNewer || _oldWindow == null)
            return;

        _borderBrushApplied = true;

        // SystemParameters.WindowGlassBrush

        _oldWindow.BorderThickness = new Thickness(1);
        _oldWindow.BorderBrush = new SolidColorBrush(Theme == ThemeType.Light ? Color.FromArgb(0xFF, 0x7A, 0x7A, 0x7A) : Color.FromArgb(0xFF, 0x3A, 0x3A, 0x3A));
    }

    private (double factorX, double factorY) GetDpi()
    {
        if (PresentationSource.FromVisual(this) is { } source)
            return (source.CompositionTarget.TransformToDevice.M11, // Possible null reference
                source.CompositionTarget.TransformToDevice.M22);

        var systemDPi = DpiHelper.GetSystemDpi();

        return (systemDPi.DpiScaleX, systemDPi.DpiScaleY);
    }
}
