// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Collections.Generic;
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
    public static class Mica
    {
        private static int _pvTrueAttribute = 0x01;

        private static int _pvFalseAttribute = 0x00;

        private static readonly List<Window> Containers = new List<Window>() { };

        /// <summary>
        /// Static singleton identifier determining whether the Mica effect has been applied.
        /// </summary>
        public static bool IsApplied { get; set; } = false;

        /// <summary>
        /// Applies a Mica effect when the <see cref="Window"/> is loaded.
        /// </summary>
        /// <param name="window">Active instance of <see cref="Window"/>.</param>
        public static void Apply(object window)
        {
            var decWindow = window as Window;

            if (decWindow == null)
            {
                throw new Exception("Only Window controls can have the Mica effect applied.");
            }

            decWindow.Loaded += OnWindowLoaded;
        }

        /// <summary>
        /// Tries to remove the Mica effect from all defined pointers.
        /// </summary>
        public static void Remove()
        {
            if (Containers == null || Containers.Count < 1)
            {
                return;
            }

            Containers.ForEach(RemoveMicaAttribute);
            Containers.Clear();
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

            Containers.Add(window);

            //_windowHandle = new WindowInteropHelper(this).Handle;

            PresentationSource.FromVisual(window)!.ContentRendered += OnContentRendered;
        }

        /// <summary>
        /// Checks if the current operating system supports Mica.
        /// </summary>
        /// <returns><see langword="true"/> if Windows 11 or above.</returns>
        public static bool IsSupported()
        {
            return Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version.Build > 20000;
        }

        /// <summary>
        /// Checks if the currently set system theme is compatible with the application's theme. If not, the Mica theme should not be set as it causes strange behavior.
        /// </summary>
        /// <returns><see langword="true"/> if the system theme is similar to the app's theme.</returns>
        public static bool IsSystemThemeCompatible()
        {
            Style appTheme = WPFUI.Theme.Manager.Current;
            Style systemTheme = WPFUI.Theme.Manager.System;

            if (appTheme == Style.Light && (systemTheme == Style.Light || systemTheme == Style.Flow || systemTheme == Style.Sunrise))
            {
                return true;
            }

            if (appTheme == Style.Dark && (systemTheme == Style.Dark || systemTheme == Style.Glow || systemTheme == Style.CapturedMotion))
            {
                return true;
            }

            return false;
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
                Dwmapi.DwmSetWindowAttribute(handle, DWMWINDOWATTRIBUTE.DWMWA_USE_IMMERSIVE_DARK_MODE, ref _pvTrueAttribute,
                    Marshal.SizeOf(typeof(int)));
            }
            else
            {
                Dwmapi.DwmSetWindowAttribute(handle, DWMWINDOWATTRIBUTE.DWMWA_USE_IMMERSIVE_DARK_MODE, ref _pvFalseAttribute,
                    Marshal.SizeOf(typeof(int)));
            }

            Dwmapi.DwmSetWindowAttribute(handle, DWMWINDOWATTRIBUTE.DWMWA_MICA_EFFECT, ref _pvTrueAttribute,
                Marshal.SizeOf(typeof(int)));

            IsApplied = true;
        }

        private static void RemoveMicaAttribute(Window window)
        {
            IntPtr handle = new WindowInteropHelper(window).Handle;

            if (handle == IntPtr.Zero)
            {
                return;
            }

            try
            {
                window.Background = (SolidColorBrush)Application.Current.Resources["ApplicationBackgroundBrush"];
            }
            catch (Exception e)
            {
#if DEBUG
                Console.WriteLine(e);
#endif
            }

            Dwmapi.DwmSetWindowAttribute(handle, DWMWINDOWATTRIBUTE.DWMWA_USE_IMMERSIVE_DARK_MODE, ref _pvFalseAttribute,
                Marshal.SizeOf(typeof(int)));

            Dwmapi.DwmSetWindowAttribute(handle, DWMWINDOWATTRIBUTE.DWMWA_MICA_EFFECT, ref _pvFalseAttribute,
                Marshal.SizeOf(typeof(int)));
        }
    }
}