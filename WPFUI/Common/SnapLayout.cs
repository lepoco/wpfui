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

        private int _heightAdd = 0; //7

        private int _widthAdd = 0;

        private Button _button;

        public void Register(Button button)
        {
            _button = button;

            HwndSource hwnd = (HwndSource)PresentationSource.FromVisual(button);
            if (hwnd != null) hwnd.AddHook(HwndSourceHook);
        }

        public static bool IsSupported()
        {
            return Environment.OSVersion.Version.Build > 20000;
        }

        private IntPtr HwndSourceHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            Input mouseNotification = (Input)msg;

            switch (mouseNotification)
            {
                case Input.WM_NCLBUTTONDOWN:
                    RaiseButtonClick();
                    handled = true;

                    return new IntPtr((int)HT.MAXBUTTON);
                    break;

                case Input.WM_NCMOUSEMOVE:
                    //_button.IsMouseOver = true;
                    handled = true;

                    break;

                case Input.WM_NCMOUSELEAVE:

                    //handled = true;

                    break;

                case Input.WM_NCHITTEST:
                    if (IsOverButton(wParam, lParam))
                    {
                        handled = true;
                    }

                    return new IntPtr((int)HT.MAXBUTTON);

                default:
                    handled = false;
                    break;
            }

            //return User32.DefWindowProc(hwnd, msg, wParam, lParam);
            return new IntPtr((int)HT.NOWHERE);
        }

        private bool IsOverButton(IntPtr wParam, IntPtr lParam)
        {
            try
            {
                int x = lParam.ToInt32() & 0xffff;
                int y = lParam.ToInt32() >> 16;

                Rect rect = new Rect(_button.PointToScreen(
                        new Point()),
                    new Size(_button.ActualWidth + _widthAdd, _button.ActualHeight + _heightAdd));

                if (rect.Contains(new Point(x, y)))
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
