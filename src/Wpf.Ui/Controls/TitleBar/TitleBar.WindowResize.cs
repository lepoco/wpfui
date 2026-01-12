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
    private int _borderX;
    private int _borderY;

    private bool _borderXCached;
    private bool _borderYCached;

    private bool _systemParamsSubscribed;

    private IntPtr GetWindowBorderHitTestResult(IntPtr hwnd, IntPtr lParam, bool isMouseOverHeaderContent)
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

        // Prioritize resize area detection for top-left and top-right corners.
        // Use a small fixed physical pixel area to avoid making the resize cursor appear too early.
        // (The WM_NCHITTEST coordinates are in physical screen pixels.)
        const int cornerResizeSize = 4;

        // Return HTTOPLEFT if within the cornerResizeSize square area of the top-left corner
        bool isInTopLeftCorner =
            x >= windowRect.left &&
            x < windowRect.left + cornerResizeSize &&
            y >= windowRect.top &&
            y < windowRect.top + cornerResizeSize;

        if (isInTopLeftCorner)
        {
            return (IntPtr)PInvoke.HTTOPLEFT;
        }

        // Return HTTOPRIGHT if within the cornerResizeSize square area of the top-right corner
        bool isInTopRightCorner =
            x >= windowRect.right - cornerResizeSize &&
            x <= windowRect.right &&
            y >= windowRect.top &&
            y < windowRect.top + cornerResizeSize;

        // Return HTTOPRIGHT if within the cornerResizeSize square area of the top-right corner
        // This ensures resize detection takes precedence over button hit testing
        if (isInTopRightCorner)
        {
            return (IntPtr)PInvoke.HTTOPRIGHT;
        }

        // If the pointer is over interactive title-bar content (Header/TrailingContent),
        // don't report resize hit-test results (except for the corner zones above).
        // This prevents custom buttons (commonly hosted in TrailingContent) from becoming
        // "resizable" on hover.
        if (isMouseOverHeaderContent)
        {
            return (IntPtr)PInvoke.HTNOWHERE;
        }

        // Get the width of the title bar button area (total width of minimize, maximize/restore, and close buttons)
        // SM_CXSIZE is the width of minimize/maximize buttons, typically about 46 pixels
        // With 3 buttons, the total is approximately 138 pixels (button width × 3)
        int buttonAreaWidth;
        try
        {
            int buttonWidth = PInvoke.GetSystemMetrics(SYSTEM_METRICS_INDEX.SM_CXSIZE);

            // Total width of 3 buttons (minimize, maximize/restore, close)
            // Add some margin as the close button may be slightly larger
            buttonAreaWidth = (buttonWidth * 3) + 2;
        }
        catch
        {
            // Fallback: use default value
            buttonAreaWidth = 138;
        }

        uint hit = 0u;

#pragma warning disable
        if (x < windowRect.left + _borderX)
            hit |= 0b0001u; // left
        
        bool isInTitleBar = y < windowRect.top + _borderY;
        bool isInButtonArea = x >= windowRect.right - buttonAreaWidth;
        
        // Right edge resize detection
        // When on the title bar, allow resizing on the left side of the button area as well
        if (isInTitleBar)
        {
            // When in the title bar, perform resize detection on the left side of the button area (top-right corner resize area)
            // Left side of button area and within the right edge resize area
            // Keep this area small (fixed pixels) so it doesn't feel like the buttons are "stealing" mouse space.
            const int titleBarRightResizeSize = cornerResizeSize;
            if (
                x >= windowRect.right - buttonAreaWidth - titleBarRightResizeSize
                && x < windowRect.right - buttonAreaWidth
            )
            {
                // Resize area on the left side of button area
                hit |= 0b0010u; // right
            }
            else if (x >= windowRect.right - _borderX && !isInButtonArea)
            {
                // Right edge resize area outside button area
                hit |= 0b0010u; // right
            }
        }
        else
        {
            // For areas outside the title bar, perform normal right edge resize detection
            if (x >= windowRect.right - _borderX)
                hit |= 0b0010u; // right
        }
        
        if (y < windowRect.top + _borderY)
            hit |= 0b0100u; // top
        if (y >= windowRect.bottom - _borderY)
            hit |= 0b1000u; // bottom
#pragma warning restore

        return hit switch
        {
            0b0101u => (IntPtr)PInvoke.HTTOPLEFT, // top    + left  (0b0100 | 0b0001)
            0b0110u => (IntPtr)PInvoke.HTTOPRIGHT, // top    + right (0b0100 | 0b0010)
            0b1001u => (IntPtr)PInvoke.HTBOTTOMLEFT, // bottom + left  (0b1000 | 0b0001)
            0b1010u => (IntPtr)PInvoke.HTBOTTOMRIGHT, // bottom + right (0b1000 | 0b0010)
            0b0100u => (IntPtr)PInvoke.HTTOP, // top
            0b0001u => (IntPtr)PInvoke.HTLEFT, // left
            0b1000u => (IntPtr)PInvoke.HTBOTTOM, // bottom
            0b0010u => (IntPtr)PInvoke.HTRIGHT, // right

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
