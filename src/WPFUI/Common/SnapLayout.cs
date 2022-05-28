// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using WPFUI.Appearance;
using WPFUI.Controls.Interfaces;
using Point = System.Windows.Point;
using Size = System.Windows.Size;

namespace WPFUI.Common;

/// <summary>
/// Brings the Snap Layout functionality from Windows 11 to a custom <see cref="Controls.TitleBar"/>.
/// </summary>
internal sealed class SnapLayout : IThemeControl
{
    public SolidColorBrush DefaultButtonBackground { get; set; } = Brushes.Transparent;

    public SolidColorBrush HoverColorLight = Brushes.Transparent;

    public SolidColorBrush HoverColorDark = Brushes.Transparent;
    public ThemeType Theme { get; set; } = ThemeType.Unknown;

    private bool _isButtonFocused;

    private bool _isButtonClicked;

    private double _dpiScale;

    private Button _button;

    public void Register(Button button)
    {
        _isButtonFocused = false;
        _button = button;
        _dpiScale = DpiHelper.SystemDpiYScale();

        HwndSource hwnd = (HwndSource)PresentationSource.FromVisual(button);

        if (hwnd != null)
            hwnd.AddHook(HwndSourceHook);
    }

    public static bool IsSupported()
    {
        return Win32.Utilities.IsOSWindows11OrNewer;
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
        // TODO: This whole class is one big todo

        var mouseNotification = (Interop.User32.WM)uMsg;

        switch (mouseNotification)
        {
            case Interop.User32.WM.NCLBUTTONDOWN:
                if (IsOverButton(wParam, lParam))
                {
                    _isButtonClicked = true;

                    handled = true;
                }

                break;

            case Interop.User32.WM.NCMOUSELEAVE:
                DefocusButton();

                break;

            case Interop.User32.WM.NCLBUTTONUP:
                if (!_isButtonClicked)
                    break;

                if (IsOverButton(wParam, lParam))
                    RaiseButtonClick();

                _isButtonClicked = false;
                handled = true;

                break;

            case Interop.User32.WM.NCHITTEST:
                if (IsOverButton(wParam, lParam))
                {
                    FocusButton();

                    handled = true;
                }
                else
                {
                    DefocusButton();
                }

                return new IntPtr((int)Interop.User32.WM_NCHITTEST.HTMAXBUTTON);

            default:
                handled = false;
                break;
        }

        return new IntPtr((int)Interop.User32.WM_NCHITTEST.HTCLIENT);
    }

    private void FocusButton()
    {
        if (_isButtonFocused)
            return;

        _button.Background = Theme == ThemeType.Dark ? HoverColorDark : HoverColorLight;
        _isButtonFocused = true;
    }

    private void DefocusButton()
    {
        if (!_isButtonFocused)
            return;

        _button.Background = DefaultButtonBackground;
        _isButtonFocused = false;
    }

    private bool IsOverButton(IntPtr wParam, IntPtr lParam)
    {
        try
        {
            int positionX = lParam.ToInt32() & 0xffff;
            int positionY = lParam.ToInt32() >> 16;

            Rect rect = new Rect(_button.PointToScreen(new Point()),
                new Size(_button.Width * _dpiScale, _button.Height * _dpiScale));

            if (rect.Contains(new Point(positionX, positionY)))
                return true;
        }
        catch (OverflowException)
        {
            return true; // or not to true, that is the question
        }

        return false;
    }

    private void RaiseButtonClick()
    {
        if (new ButtonAutomationPeer(_button).GetPattern(PatternInterface.Invoke) is IInvokeProvider invokeProv)
            invokeProv?.Invoke();
    }
}
