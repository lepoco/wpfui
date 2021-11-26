// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using WPFUI.Theme;
using WPFUI.Unmanaged;
using Style = WPFUI.Theme.Style;

namespace WPFUI.Background
{
    /// <summary>
    /// Contains static handlers for applying background Mica effects from Windows 11.
    /// </summary>
    public class Mica
    {
        private static int pvTrueAttribute = 0x01;

        private static int pvFalseAttribute = 0x00;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="visualElement"></param>
        public static void Apply(object visualElement)
        {
            // TODO: Allow mica for controls

            var window = visualElement as Window;

            if (window == null)
            {
                throw new Exception("Only windows can have the Mica effect applied.");
            }

            window.Loaded += OnWindowLoaded;
        }

        /// <summary>
        /// Event handler triggered after the window is loaded that applies the <see cref="Mica"/> effect.
        /// </summary>
        /// <param name="sender">The window whose background is to be set.</param>
        /// <param name="e"><see cref="RoutedEventArgs"/></param>
        public static void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            var window = sender as Window;

            if (window == null)
            {
                throw new Exception("Only windows can have the Mica effect applied.");
            }

            window.Background = Brushes.Transparent;

            //_windowHandle = new WindowInteropHelper(this).Handle;

            PresentationSource.FromVisual(window)!.ContentRendered += OnContentRendered;
        }

        /// <summary>
        /// Checks if the current operating system supports Mica.
        /// </summary>
        /// <returns><see langword="true"/> if Windows 11 or above.</returns>
        public static bool IsSupported()
        {
            return Environment.OSVersion.Version.Build > 20000;
        }

        private static void OnContentRendered(object sender, EventArgs e)
        {
            Style currentTheme = Manager.GetSystemTheme();

            if (currentTheme == Style.Unknown)
            {
                currentTheme = Style.Dark;
            }

            SetMicaAttribute(((HwndSource)sender).Handle, currentTheme);
        }

        private static void SetMicaAttribute(IntPtr handle, Style theme)
        {
            if (handle == IntPtr.Zero)
            {
                return;
            }

            // Hide default TitleBar
            // https://stackoverflow.com/questions/743906/how-to-hide-close-button-in-wpf-window
            try
            {
                User32.SetWindowLong(handle, -16, User32.GetWindowLong(handle, -16) & ~0x80000);
            }
            catch (Exception e)
            {
#if DEBUG
                Console.WriteLine(e);
#endif
            }

            if (theme == Style.Dark || theme == Style.Glow || theme == Style.CapturedMotion)
            {
                Dwmapi.DwmSetWindowAttribute(handle, DWMWINDOWATTRIBUTE.DWMWA_USE_IMMERSIVE_DARK_MODE, ref pvTrueAttribute,
                    Marshal.SizeOf(typeof(int)));
            }
            else
            {
                Dwmapi.DwmSetWindowAttribute(handle, DWMWINDOWATTRIBUTE.DWMWA_USE_IMMERSIVE_DARK_MODE, ref pvFalseAttribute,
                    Marshal.SizeOf(typeof(int)));
            }

            Dwmapi.DwmSetWindowAttribute(handle, DWMWINDOWATTRIBUTE.DWMWA_MICA_EFFECT, ref pvTrueAttribute,
                Marshal.SizeOf(typeof(int)));
        }
    }
}