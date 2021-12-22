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
using WPFUI.Win32;

namespace WPFUI.Common
{
    /// <summary>
    /// Brings the Snap Layout functionality from Windows 11 to a custom <see cref="Controls.TitleBar"/>.
    /// </summary>
    internal sealed class SnapLayout
    {
        private double _dpiScale = 1.2; //7

        private Window _window;

        private Button _button;

        public void Register(Window window, Button button)
        {
            _window = window;
            _button = button;

            HwndSource hwnd = (HwndSource)PresentationSource.FromVisual(button);
            if (hwnd != null) hwnd.AddHook(HwndSourceHook);
        }

        public static bool IsSupported()
        {
            return Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version.Build > 20000;
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
            User32.WM mouseNotification = (User32.WM)uMsg;

            switch (mouseNotification)
            {
                case User32.WM.NCMOUSEMOVE:

                    //_button.RaiseEvent(new MouseEventArgs(null, (int)DateTime.Now.Subtract(new DateTime(1970, 1, 1)).TotalSeconds));

                    //_button.IsMouseOver = true;
                    handled = true;
                    break;

                case User32.WM.NCMOUSELEAVE:
                    //_button.RaiseEvent(new RoutedEventArgs(Button.MouseLeaveEvent, _button));

                    handled = true;
                    break;

                case User32.WM.NCLBUTTONDOWN:
                    if (IsOverButton(wParam, lParam))
                    {
                        RaiseButtonClick();

                        handled = true;
                    }

                    break;

                case User32.WM.NCHITTEST:
                    if (IsOverButton(wParam, lParam))
                    {
                        handled = true;

                        if (_window.WindowState == WindowState.Maximized)
                        {
                            return new IntPtr((int)HT.MINBUTTON);
                        }

                        return new IntPtr((int)HT.MAXBUTTON);
                    }

                    break;

                default:
                    handled = false;
                    break;
            }

            return new IntPtr((int)HT.CLIENT);
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
                {
                    return true;
                }
            }
            catch (OverflowException)
            {
                return true;
            }

            return false;
        }

        private void RaiseButtonClick()
        {
            if (new ButtonAutomationPeer(_button).GetPattern(PatternInterface.Invoke) is IInvokeProvider invokeProv)
                invokeProv?.Invoke();
        }
    }
}