// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls.Interfaces;
using Wpf.Ui.Dpi;

namespace Wpf.Ui.TitleBar;

/// <summary>
/// Brings the Snap Layout functionality from Windows 11 to a custom <see cref="Controls.TitleBar"/>.
/// </summary>
internal sealed class SnapLayout : IThemeControl
{
    /// <summary>
    /// List of snap layout buttons.
    /// </summary>
    private readonly SnapLayoutButton[] _buttons;

    /// <summary>
    /// Currently used theme.
    /// </summary>
    private ThemeType _currentTheme;

    /// <summary>
    /// Currently active hover color.
    /// </summary>
    private SolidColorBrush _currentHoverColor = Brushes.Transparent;

    /// <summary>
    /// Current theme.
    /// </summary>
    public ThemeType Theme
    {
        get => _currentTheme;
        set
        {
            _currentHoverColor = value == ThemeType.Light ? HoverColorLight : HoverColorDark;
            _currentTheme = value;
        }
    }

    /// <summary>
    /// Default background.
    /// </summary>
    public SolidColorBrush DefaultButtonBackground { get; set; } = Brushes.Transparent;

    /// <summary>
    /// Hover background when light theme.
    /// </summary>
    public SolidColorBrush HoverColorLight = Brushes.Transparent;

    /// <summary>
    /// Hover background when dark theme.
    /// </summary>
    public SolidColorBrush HoverColorDark = Brushes.Transparent;

    /// <summary>
    /// Creates new instance.
    /// </summary>
    private SnapLayout(Window window, Wpf.Ui.Controls.Button maximizeButton, Wpf.Ui.Controls.Button restoreButton)
    {
        if (window == null)
            return;

        var windowHandle = new WindowInteropHelper(window).Handle;

        if (windowHandle == IntPtr.Zero)
            return;

        var windowDpi = DpiHelper.GetWindowDpi(windowHandle);

        _buttons = new[]
        {
            new SnapLayoutButton(maximizeButton, TitleBarButton.Maximize, windowDpi.DpiScaleX),
            new SnapLayoutButton(restoreButton, TitleBarButton.Restore, windowDpi.DpiScaleX),
        };

        var windowSource = HwndSource.FromHwnd(windowHandle);

        if (windowSource != null)
            windowSource.AddHook(HwndSourceHook);
    }

    /// <summary>
    /// Determines whether the snap layout is supported.
    /// </summary>
    public static bool IsSupported()
    {
        return Win32.Utilities.IsOSWindows11OrNewer;
    }

    /// <summary>
    /// Registers the snap layout for provided buttons and window.
    /// </summary>
    public static SnapLayout Register(Window window, Wpf.Ui.Controls.Button maximizeButton, Wpf.Ui.Controls.Button restoreButton)
    {
        return new SnapLayout(window, maximizeButton, restoreButton);
    }

    /// <summary>
    /// Represents the method that handles Win32 window messages.
    /// </summary>
    /// <param name="hWnd">The window handle.</param>
    /// <param name="uMsg">The message ID.</param>
    /// <param name="wParam">The message's wParam value.</param>
    /// <param name="lParam">The message's lParam value.</param>
    /// <param name="handled">A value that indicates whether the message was handled. Set the value to <see langword="true"/> if the message was handled; otherwise, <see langword="false"/>.</param>
    /// <returns>The appropriate return value depends on the particular message. See the message documentation details for the Win32 message being handled.</returns>
    private IntPtr HwndSourceHook(IntPtr hWnd, int uMsg, IntPtr wParam, IntPtr lParam, ref bool handled)
    {
        var mouseNotification = (Interop.User32.WM)uMsg;

        switch (mouseNotification)
        {
            case Interop.User32.WM.MOVE:
                // Adjust [Size] of the buttons if the DPI is changed
                break;

            // Mouse leaves the window
            case Interop.User32.WM.NCMOUSELEAVE:
                _buttons[0].RemoveHover(DefaultButtonBackground);
                _buttons[1].RemoveHover(DefaultButtonBackground);

                break;

            // Left button clicked down
            case Interop.User32.WM.NCLBUTTONDOWN:
                if (_buttons[0].IsMouseOver(lParam))
                {
                    _buttons[0].IsClickedDown = true;

                    handled = true;
                }

                if (_buttons[1].IsMouseOver(lParam))
                {
                    _buttons[1].IsClickedDown = true;

                    handled = true;
                }

                break;

            // Left button clicked up
            case Interop.User32.WM.NCLBUTTONUP:
                if (_buttons[0].IsClickedDown && _buttons[0].IsMouseOver(lParam))
                {
                    _buttons[0].InvokeClick();

                    handled = true;

                    break;
                }

                if (_buttons[1].IsClickedDown && _buttons[1].IsMouseOver(lParam))
                {
                    _buttons[1].InvokeClick();

                    handled = true;
                }

                break;

            // Hit test, for determining whether the mouse cursor is over one of the buttons
            case Interop.User32.WM.NCHITTEST:
                if (_buttons[0].IsMouseOver(lParam))
                {
                    _buttons[0].Hover(_currentHoverColor);

                    handled = true;

                    return new IntPtr((int)Interop.User32.WM_NCHITTEST.HTMAXBUTTON);
                }

                _buttons[0].RemoveHover(DefaultButtonBackground);

                if (_buttons[1].IsMouseOver(lParam))
                {
                    _buttons[1].Hover(_currentHoverColor);

                    handled = true;

                    return new IntPtr((int)Interop.User32.WM_NCHITTEST.HTMAXBUTTON);
                }

                _buttons[1].RemoveHover(DefaultButtonBackground);

                return IntPtr.Zero;
        }

        return IntPtr.Zero;
    }
}
