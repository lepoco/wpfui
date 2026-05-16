// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows.Shell;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.WindowsAndMessaging;
using RECT = Windows.Win32.Foundation.RECT;

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
///   <see cref="PInvoke.GetSystemMetrics"/>;
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
    /// <summary>
    /// Bit flags that represent which window border edges the cursor is currently over.
    /// </summary>
    [Flags]
    private enum BorderHitEdges : uint
    {
        /// <summary>No border edge is hit.</summary>
        None = 0,
        /// <summary>The left border edge is hit.</summary>
        Left = 1 << 0,
        /// <summary>The right border edge is hit.</summary>
        Right = 1 << 1,
        /// <summary>The top border edge is hit.</summary>
        Top = 1 << 2,
        /// <summary>The bottom border edge is hit.</summary>
        Bottom = 1 << 3,
    }

    private int _borderX;
    private int _borderY;

    private bool _borderXCached;
    private bool _borderYCached;

    private bool _systemParamsSubscribed;

    private IntPtr GetWindowBorderHitTestResult(IntPtr hwnd, IntPtr lParam)
    {
        if (!PInvoke.GetWindowRect(new HWND(hwnd), out RECT windowRect))
        {
            return (IntPtr)PInvoke.HTNOWHERE;
        }

        if (!_borderXCached || !_borderYCached)
        {
            ComputeAndCacheBorderSizes(hwnd);
        }

        long lp = lParam.ToInt64();

        int x = (short)(lp & 0xFFFF);
        int y = (short)((lp >> 16) & 0xFFFF);

        BorderHitEdges hit = BorderHitEdges.None;

        if (x < windowRect.left + _borderX)
            hit |= BorderHitEdges.Left;
        if (x >= windowRect.right - _borderX)
            hit |= BorderHitEdges.Right;
        if (y < windowRect.top + _borderY)
            hit |= BorderHitEdges.Top;
        if (y >= windowRect.bottom - _borderY)
            hit |= BorderHitEdges.Bottom;

        if (hit == (BorderHitEdges.Top | BorderHitEdges.Right))
        {
            const int cornerWidth = 1;
            if (x < windowRect.right - cornerWidth)
            {
                hit = BorderHitEdges.Top;
            }
        }

        return hit switch
        {
            BorderHitEdges.Top | BorderHitEdges.Left => (IntPtr)PInvoke.HTTOPLEFT,
            BorderHitEdges.Top | BorderHitEdges.Right => (IntPtr)PInvoke.HTTOPRIGHT,
            BorderHitEdges.Bottom | BorderHitEdges.Left => (IntPtr)PInvoke.HTBOTTOMLEFT,
            BorderHitEdges.Bottom | BorderHitEdges.Right => (IntPtr)PInvoke.HTBOTTOMRIGHT,
            BorderHitEdges.Top => (IntPtr)PInvoke.HTTOP,
            BorderHitEdges.Left => (IntPtr)PInvoke.HTLEFT,
            BorderHitEdges.Bottom => (IntPtr)PInvoke.HTBOTTOM,
            BorderHitEdges.Right => (IntPtr)PInvoke.HTRIGHT,

            // no match = HTNOWHERE (stop processing)
            _ => (IntPtr)PInvoke.HTNOWHERE,
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

            if (
                TryComputeFromPresentationSource(win, dipBorderX, dipBorderY, out borderX, out borderY)
                || TryComputeFromDpiApi(hwnd, dipBorderX, dipBorderY, out borderX, out borderY)
                || TryGetFromSystemMetrics(out borderX, out borderY)
            )
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
    private static bool TryComputeFromPresentationSource(
        Window? win,
        double dipBorderX,
        double dipBorderY,
        out int borderX,
        out int borderY
    )
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
    private static bool TryComputeFromDpiApi(
        IntPtr hwnd,
        double dipBorderX,
        double dipBorderY,
        out int borderX,
        out int borderY
    )
    {
        try
        {
            uint dpi = PInvoke.GetDpiForWindow(new HWND(hwnd));
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
            int sx =
                PInvoke.GetSystemMetrics(SYSTEM_METRICS_INDEX.SM_CXSIZEFRAME)
                + PInvoke.GetSystemMetrics(SYSTEM_METRICS_INDEX.SM_CXPADDEDBORDER);
            int sy =
                PInvoke.GetSystemMetrics(SYSTEM_METRICS_INDEX.SM_CYSIZEFRAME)
                + PInvoke.GetSystemMetrics(SYSTEM_METRICS_INDEX.SM_CXPADDEDBORDER);

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
