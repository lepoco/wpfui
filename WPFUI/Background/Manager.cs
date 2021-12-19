// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using WPFUI.Unmanaged;

namespace WPFUI.Background
{
    /// <summary>
    /// ...
    /// </summary>
    public static class Manager
    {
        /// <summary>
        /// Collection of pointers.
        /// </summary>
        private static List<IntPtr> _handlers = new List<IntPtr>();

        /// <summary>
        /// Gets list of <see cref="IntPtr"/> for affected windows.
        /// </summary>
        public static List<IntPtr> Handlers => _handlers;

        public static bool IsUsed => _handlers?.Count > 0;

        /// <summary>
        /// Tries to apply the latest background effect to the window if possible.
        /// </summary>
        /// <param name="window">Window to affect.</param>
        /// <param name="fallbackOlderStyles">If the newest backdrop effect is not available, try using an older one, such as Acrylic.</param>
        public static void Apply(Window window, bool fallbackOlderStyles = false)
        {
            if (!IsSystemThemeCompatible())
            {
                return;
            }

            if (IsSupported(BackgroundType.Mica))
            {
                Apply(BackgroundType.Mica, window);

                return;
            }

            if (fallbackOlderStyles && IsSupported(BackgroundType.Acrylic))
            {
                Apply(BackgroundType.Acrylic, window);
            }
        }

        /// <summary>
        /// Applies selected background effect to <see cref="Window"/>.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="window"></param>
        public static void Apply(BackgroundType type, Window window)
        {
            switch (type)
            {
                case BackgroundType.Mica:
                    window.Loaded += Window_ApplyMica_OnLoaded;

                    break;

                case BackgroundType.Auto:
                    window.Loaded += Window_ApplyAuto_OnLoaded;

                    break;

                case BackgroundType.Tabbed:
                    window.Loaded += Window_ApplyTabbed_OnLoaded;

                    break;

                case BackgroundType.Acrylic:
                    window.Loaded += Window_ApplyAcrylic_OnLoaded;

                    break;
            }
        }

        /// <summary>
        /// Applies selected background effect to Window by it's <see cref="IntPtr"/>.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="handle"></param>
        /// <param name="enableImmersiveDarkMode"></param>
        public static void Apply(BackgroundType type, IntPtr handle, bool enableImmersiveDarkMode = true)
        {
            if (handle == IntPtr.Zero)
            {
                return;
            }

            if (!_handlers.Contains(handle))
            {
                _handlers.Add(handle);
            }

            if (!_handlers.Contains(handle))
            {
                _handlers.Add(handle);
            }

            switch (type)
            {
                case BackgroundType.Mica:
                    ApplyMica(handle);

                    break;

                case BackgroundType.Acrylic:
                    ApplyMica(handle);

                    break;

                case BackgroundType.Tabbed:
                    ApplyMica(handle);

                    break;

                case BackgroundType.Auto:
                    ApplyMica(handle);

                    break;
            }
        }

        /// <summary>
        /// Removes Dark Mode and Mica effects, next sets backdrop to DWMSBT_DISABLE.
        /// </summary>
        /// <param name="window"></param>
        public static void Remove(Window window)
        {
            Remove(new WindowInteropHelper(window).Handle);
        }

        /// <summary>
        /// Removes Dark Mode and Mica effects, next sets backdrop to DWMSBT_DISABLE.
        /// </summary>
        /// <param name="handle"></param>
        public static void Remove(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                return;
            }

            int pvAttribute = (int)PvAttribute.Disable;
            int backdropPvAttribute = (int)BackdropType.DWMSBT_DISABLE;

            Dwmapi.DwmSetWindowAttribute(handle, DWMWINDOWATTRIBUTE.DWMWA_USE_IMMERSIVE_DARK_MODE, ref pvAttribute,
                Marshal.SizeOf(typeof(int)));

            Dwmapi.DwmSetWindowAttribute(handle, DWMWINDOWATTRIBUTE.DWMWA_MICA_EFFECT, ref pvAttribute,
                Marshal.SizeOf(typeof(int)));

            Dwmapi.DwmSetWindowAttribute(handle, DWMWINDOWATTRIBUTE.DWMWA_SYSTEMBACKDROP_TYPE, ref backdropPvAttribute,
                Marshal.SizeOf(typeof(int)));

            for (int i = 0; i < _handlers.Count; i++)
            {
                if (_handlers[i] == handle)
                {
                    _handlers.RemoveAt(i);

                    break;
                }
            }
        }

        /// <summary>
        /// Checks if the current operating system supports selected <see cref="Type"/>.
        /// </summary>
        /// <returns><see langword="true"/> if background effect is supported.</returns>
        public static bool IsSupported(BackgroundType type)
        {
            switch (type)
            {
                case BackgroundType.Auto:
                case BackgroundType.Tabbed:
                    return Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version.Build >= 22523; // Insider with new API

                case BackgroundType.Mica:
                    return Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version.Build >= 20000; // Since W11

                case BackgroundType.Acrylic:
                    return Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version.Build >= 7601; // NT 6.1

                case BackgroundType.Default:
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Checks if the currently set system theme is compatible with the application's theme. If not, the backdrop should not be set as it causes strange behavior.
        /// </summary>
        /// <returns><see langword="true"/> if the system theme is similar to the app's theme.</returns>
        public static bool IsSystemThemeCompatible()
        {
            return Theme.Manager.IsMatchedDark() || Theme.Manager.IsMatchedLight();
        }

        private static bool ApplyDarkMode(IntPtr handle)
        {
            int pvAttribute = (int)PvAttribute.Enable;

            Dwmapi.DwmSetWindowAttribute(handle, DWMWINDOWATTRIBUTE.DWMWA_USE_IMMERSIVE_DARK_MODE, ref pvAttribute,
                Marshal.SizeOf(typeof(int)));

            if (!_handlers.Contains(handle))
            {
                _handlers.Add(handle);
            }

            return true;
        }

        private static bool ApplyAuto(IntPtr handle)
        {
            if (Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version.Build >= 22523)
            {
                int backdropPvAttribute = (int)BackdropType.DWMSBT_AUTO;

                Dwmapi.DwmSetWindowAttribute(handle, DWMWINDOWATTRIBUTE.DWMWA_SYSTEMBACKDROP_TYPE, ref backdropPvAttribute,
                    Marshal.SizeOf(typeof(int)));

                if (!_handlers.Contains(handle))
                {
                    _handlers.Add(handle);
                }

                return true;
            }

            return false;
        }

        private static bool ApplyTabbed(IntPtr handle)
        {
            if (Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version.Build >= 22523)
            {
                int backdropPvAttribute = (int)BackdropType.DWMSBT_TABBEDWINDOW;

                Dwmapi.DwmSetWindowAttribute(handle, DWMWINDOWATTRIBUTE.DWMWA_SYSTEMBACKDROP_TYPE, ref backdropPvAttribute,
                    Marshal.SizeOf(typeof(int)));

                if (!_handlers.Contains(handle))
                {
                    _handlers.Add(handle);
                }

                return true;
            }

            return false;
        }

        private static bool ApplyMica(IntPtr handle)
        {
            if (Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version.Build >= 22523)
            {
                RemoveTitleBar(handle);

                int backdropPvAttribute = (int)BackdropType.DWMSBT_MAINWINDOW;

                Dwmapi.DwmSetWindowAttribute(handle, DWMWINDOWATTRIBUTE.DWMWA_SYSTEMBACKDROP_TYPE, ref backdropPvAttribute,
                    Marshal.SizeOf(typeof(int)));

                if (!_handlers.Contains(handle))
                {
                    _handlers.Add(handle);
                }

                return true;
            }

            if (Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version.Build >= 20000)
            {
                RemoveTitleBar(handle);

                int backdropPvAttribute = (int)PvAttribute.Enable;

                Dwmapi.DwmSetWindowAttribute(handle, DWMWINDOWATTRIBUTE.DWMWA_MICA_EFFECT, ref backdropPvAttribute,
                    Marshal.SizeOf(typeof(int)));

                if (!_handlers.Contains(handle))
                {
                    _handlers.Add(handle);
                }

                return true;
            }

            return false;
        }

        private static bool ApplyAcrylic(IntPtr handle)
        {
            if (Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version.Build >= 22523)
            {
                int backdropPvAttribute = (int)BackdropType.DWMSBT_TRANSIENTWINDOW;

                Dwmapi.DwmSetWindowAttribute(handle, DWMWINDOWATTRIBUTE.DWMWA_SYSTEMBACKDROP_TYPE, ref backdropPvAttribute,
                    Marshal.SizeOf(typeof(int)));

                if (!_handlers.Contains(handle))
                {
                    _handlers.Add(handle);
                }

                return true;
            }

            if (Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version.Build >= 7601)
            {
                // TODO: Older versions acrylic effect.

                return true;
            }

            return false;
        }

        private static bool RemoveTitleBar(IntPtr handle)
        {
            // Hide default TitleBar
            // https://stackoverflow.com/questions/743906/how-to-hide-close-button-in-wpf-window
            try
            {
                User32.SetWindowLong(handle, -16, User32.GetWindowLong(handle, -16) & ~0x80000);

                return true;
            }
            catch (Exception e)
            {
#if DEBUG
                Console.WriteLine(e);
#endif
                return false;
            }
        }

        #region Window specific handlers
        // Each of these backdrops has its own event listener so that we can make simple modifications or corrections for each type.

        private static void Window_ApplyMica_OnLoaded(object sender, RoutedEventArgs e)
        {
            var window = sender as Window;

            if (window == null)
            {
                return;
            }

            window.Background = Brushes.Transparent;

            PresentationSource.FromVisual(window)!.ContentRendered += (o, args) =>
            {
                if (Theme.Manager.IsMatchedDark())
                {
                    ApplyDarkMode(((HwndSource)o)?.Handle ?? IntPtr.Zero);
                }

                ApplyMica(((HwndSource)o)?.Handle ?? IntPtr.Zero);
            };
        }

        private static void Window_ApplyTabbed_OnLoaded(object sender, RoutedEventArgs e)
        {
            var window = sender as Window;

            if (window == null)
            {
                return;
            }

            window.Background = Brushes.Transparent;

            PresentationSource.FromVisual(window)!.ContentRendered += (o, args) =>
            {
                if (Theme.Manager.IsMatchedDark())
                {
                    ApplyDarkMode(((HwndSource)o)?.Handle ?? IntPtr.Zero);
                }

                ApplyTabbed(((HwndSource)o)?.Handle ?? IntPtr.Zero);
            };
        }

        private static void Window_ApplyAuto_OnLoaded(object sender, RoutedEventArgs e)
        {
            var window = sender as Window;

            if (window == null)
            {
                return;
            }

            window.Background = Brushes.Transparent;

            PresentationSource.FromVisual(window)!.ContentRendered += (o, args) =>
            {
                if (Theme.Manager.IsMatchedDark())
                {
                    ApplyDarkMode(((HwndSource)o)?.Handle ?? IntPtr.Zero);
                }

                ApplyAuto(((HwndSource)o)?.Handle ?? IntPtr.Zero);
            };
        }

        private static void Window_ApplyAcrylic_OnLoaded(object sender, RoutedEventArgs e)
        {
            var window = sender as Window;

            if (window == null)
            {
                return;
            }

            window.Background = Brushes.Transparent;

            PresentationSource.FromVisual(window)!.ContentRendered += (o, args) =>
            {
                if (Theme.Manager.IsMatchedDark())
                {
                    ApplyDarkMode(((HwndSource)o)?.Handle ?? IntPtr.Zero);
                }

                ApplyAcrylic(((HwndSource)o)?.Handle ?? IntPtr.Zero);
            };
        }

        #endregion
    }
}
