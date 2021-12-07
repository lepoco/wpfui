// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Interop;
using WPFUI.Unmanaged;

namespace WPFUI.Common
{
    internal class SnapLayout
    {
        private const int HTMAXBUTTON = 9;

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

        private IntPtr HwndSourceHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            Input mouseNotification = (Input)msg;

            switch (mouseNotification)
            {
                case Input.WM_NCMOUSEMOVE:

                    //_button.RaiseEvent(new MouseEventArgs(null, (int)DateTime.Now.Subtract(new DateTime(1970, 1, 1)).TotalSeconds));

                    //_button.IsMouseOver = true;
                    handled = true;
                    break;

                case Input.WM_NCMOUSELEAVE:
                    //_button.RaiseEvent(new RoutedEventArgs(Button.MouseLeaveEvent, _button));

                    handled = true;
                    break;

                case Input.WM_NCLBUTTONDOWN:
                    if (IsOverButton(wParam, lParam))
                    {
                        RaiseButtonClick();

                        handled = true;
                    }

                    System.Diagnostics.Debug.WriteLine("Not over button");

                    break;

                case Input.WM_NCHITTEST:
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

            //return User32.DefWindowProc(hwnd, msg, wParam, lParam);
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