// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.ComponentModel;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Shell;
using Wpf.Ui.Interop;
using Wpf.Ui.Interop.WinDef;

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

/// <summary>
/// Provides optional window-resize related hit-testing support for the <see cref="TitleBar"/> control.
/// </summary>
/// <remarks>
/// This partial class implements logic to return appropriate WM_NCHITTEST results (for example
/// <c>HTLEFT</c>, <c>HTBOTTOMRIGHT</c>, etc.) when the mouse is positioned over the window edges
/// or corners. This enables intuitive resizing behavior when the user drags the window borders.
///
/// Key points:
/// - The implementation prefers the <see cref="System.Windows.Shell.WindowChrome.ResizeBorderThickness"/>
///   value (expressed in device-independent units) when available and translates it into physical pixels;
/// - If WindowChrome or DPI information is not available, the code falls back to system metrics via
///   <see cref="Wpf.Ui.Interop.User32.GetSystemMetrics"/>;
/// - Because <c>WM_NCHITTEST</c> is raised frequently, computed border pixel sizes are cached to
///   reduce overhead; the cache is invalidated when DPI or relevant system parameters change;
/// - This component only augments the <see cref="TitleBar"/> control's non-client hit-testing to
///   improve resize behavior and does not alter the window style or system behavior itself.
///
/// Splitting this functionality into a partial class keeps the resize-related responsibilities
/// clearly separated from other TitleBar UI and interaction logic.
/// </remarks>
public partial class TitleBar
{
    private int _borderX;
    private int _borderY;

    private bool _borderXCached;
    private bool _borderYCached;

    private bool _systemParamsSubscribed;

    private IntPtr GetWindowBorderHitTestResult(IntPtr hwnd, IntPtr lParam)
    {
        if (!User32.GetWindowRect(hwnd, out RECT windowRect))
        {
            return (IntPtr)User32.WM_NCHITTEST.HTNOWHERE;
        }

        if (!_borderXCached || !_borderYCached)
        {
            ComputeAndCacheBorderSizes(hwnd);
        }

        long lp = lParam.ToInt64();

        int x = (short)(lp & 0xFFFF);
        int y = (short)((lp >> 16) & 0xFFFF);

        uint hit = 0u;

#pragma warning disable
        if (x <  windowRect.Left   + _borderX) hit |= 0b0001u; // left
        if (x >= windowRect.Right  - _borderX) hit |= 0b0010u; // right
        if (y <  windowRect.Top    + _borderY) hit |= 0b0100u; // top
        if (y >= windowRect.Bottom - _borderY) hit |= 0b1000u; // bottom
#pragma warning restore

        return hit switch
        {
            0b0101u => (IntPtr)User32.WM_NCHITTEST.HTTOPLEFT,     // top    + left  (0b0100 | 0b0001)
            0b0110u => (IntPtr)User32.WM_NCHITTEST.HTTOPRIGHT,    // top    + right (0b0100 | 0b0010)
            0b1001u => (IntPtr)User32.WM_NCHITTEST.HTBOTTOMLEFT,  // bottom + left  (0b1000 | 0b0001)
            0b1010u => (IntPtr)User32.WM_NCHITTEST.HTBOTTOMRIGHT, // bottom + right (0b1000 | 0b0010)
            0b0100u => (IntPtr)User32.WM_NCHITTEST.HTTOP,         // top
            0b0001u => (IntPtr)User32.WM_NCHITTEST.HTLEFT,        // left
            0b1000u => (IntPtr)User32.WM_NCHITTEST.HTBOTTOM,      // bottom
            0b0010u => (IntPtr)User32.WM_NCHITTEST.HTRIGHT,       // right

            // no match = HTNOWHERE (stop processing)
            _ => (IntPtr)User32.WM_NCHITTEST.HTNOWHERE
        };
    }

    private void SubscribeToSystemParameters()
    {
        if (_systemParamsSubscribed)
        {
            return;
        }

        SystemParameters.StaticPropertyChanged += OnSystemParametersChanged;
        _systemParamsSubscribed = true;
    }

    private void UnsubscribeToSystemParameters()
    {
        if (!_systemParamsSubscribed)
        {
            return;
        }

        SystemParameters.StaticPropertyChanged -= OnSystemParametersChanged;
        _systemParamsSubscribed = false;
    }

    private void OnSystemParametersChanged(object? sender, PropertyChangedEventArgs e)
    {
        InvalidateBorderCache();
    }

    private void InvalidateBorderCache()
    {
        _borderXCached = false;
        _borderYCached = false;
    }

    private void ComputeAndCacheBorderSizes(IntPtr hwnd)
    {
        try
        {
            double dipBorderX;
            double dipBorderY;

            Window? win = null;
            try
            {
                var src = HwndSource.FromHwnd(hwnd);
                if (src?.RootVisual is DependencyObject dep)
                {
                    win = Window.GetWindow(dep);
                }
            }
            catch
            {
                // ignored
            }

            // FluentWindow uses WindowChrome - get border from it first
            WindowChrome? chrome = win is null ? null : WindowChrome.GetWindowChrome(win);

            if (chrome is not null)
            {
                dipBorderX = Math.Max(chrome.ResizeBorderThickness.Left, chrome.ResizeBorderThickness.Right);
                dipBorderY = Math.Max(chrome.ResizeBorderThickness.Top, chrome.ResizeBorderThickness.Bottom);
            }
            else
            {
                dipBorderX = SystemParameters.WindowResizeBorderThickness.Left;
                dipBorderY = SystemParameters.WindowResizeBorderThickness.Top;
            }

            int borderX;
            int borderY;

            if (TryComputeFromPresentationSource(win, dipBorderX, dipBorderY, out borderX, out borderY) ||
                TryComputeFromDpiApi(hwnd, dipBorderX, dipBorderY, out borderX, out borderY) ||
                TryGetFromSystemMetrics(out borderX, out borderY))
            {
                _borderX = borderX;
                _borderY = borderY;
            }
            else
            {
                _borderX = 4;
                _borderY = 4;
            }
        }
        catch
        {
            _borderX = 4;
            _borderY = 4;
        }

        _borderXCached = true;
        _borderYCached = true;
    }

    // Try to compute border sizes from PresentationSource (per-monitor DPI-aware path).
    private static bool TryComputeFromPresentationSource(Window? win, double dipBorderX, double dipBorderY, out int borderX, out int borderY)
    {
        if (win is not null)
        {
            try
            {
                PresentationSource? source = PresentationSource.FromVisual(win);

                if (source?.CompositionTarget is not null)
                {
                    Matrix m = source.CompositionTarget.TransformToDevice;

                    borderX = Math.Max(2, (int)Math.Ceiling(dipBorderX * m.M11));
                    borderY = Math.Max(2, (int)Math.Ceiling(dipBorderY * m.M22));

                    return true;
                }
            }
            catch
            {
                // ignored
            }
        }

        borderX = 0;
        borderY = 0;

        return false;
    }

    // Try to compute border sizes using GetDpiForWindow (if available).
    private static bool TryComputeFromDpiApi(IntPtr hwnd, double dipBorderX, double dipBorderY, out int borderX, out int borderY)
    {
        try
        {
            uint dpi = User32.GetDpiForWindow(hwnd);
            if (dpi == 0)
            {
                dpi = 96;
            }

            double scale = dpi / 96.0;

            borderX = Math.Max(2, (int)Math.Ceiling(dipBorderX * scale));
            borderY = Math.Max(2, (int)Math.Ceiling(dipBorderY * scale));

            return true;
        }
        catch
        {
            borderX = 0;
            borderY = 0;

            return false;
        }
    }

    // Try to compute border sizes using GetSystemMetrics as a safe fallback.
    private static bool TryGetFromSystemMetrics(out int borderX, out int borderY)
    {
        try
        {
            int sx = User32.GetSystemMetrics(User32.SM.CXSIZEFRAME) + User32.GetSystemMetrics(User32.SM.CXPADDEDBORDER);
            int sy = User32.GetSystemMetrics(User32.SM.CYSIZEFRAME) + User32.GetSystemMetrics(User32.SM.CXPADDEDBORDER);

            borderX = Math.Max(2, sx);
            borderY = Math.Max(2, sy);

            return true;
        }
        catch
        {
            borderX = 0;
            borderY = 0;

            return false;
        }
    }
}
