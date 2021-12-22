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
using WPFUI.Win32;

namespace WPFUI.Background
{
    /// <summary>
    /// Lets you apply background effects to <see cref="Window"/> or any HWND by its <see cref="IntPtr"/>.
    /// </summary>
    public static class Manager
    {
        /// <summary>
        /// Collection of pointers.
        /// </summary>
        private static List<IntPtr> _handlers = new();

        /// <summary>
        /// Gets list of <see cref="IntPtr"/> for affected windows.
        /// </summary>
        public static List<IntPtr> Handlers => _handlers;

        /// <summary>
        /// Indicates whether the <see cref="Manager"/> has been used in the <see cref="Application"/>.
        /// </summary>
        public static bool IsUsed => _handlers?.Count > 0;

        /// <summary>
        /// Tries to apply the latest background effect to the window if possible.
        /// </summary>
        /// <param name="window">Window to affect.</param>
        /// <param name="fallbackOlderStyles">If the newest backdrop effect is not available, try using an older one, such as Acrylic.</param>
        public static void Apply(Window window, bool fallbackOlderStyles = false)
        {
            //if (!Theme.Manager.IsSystemThemeCompatible())
            //{
            //    return;
            //}

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
        /// <param name="window">Window to apply effect.</param>
        public static void Apply(BackgroundType type, Window window)
        {
            switch (type)
            {
                case BackgroundType.Mica:
                    window.Loaded += ApplyMica_OnWindowLoaded;

                    break;

                case BackgroundType.Auto:
                    window.Loaded += ApplyAuto_OnWindowLoaded;

                    break;

                case BackgroundType.Tabbed:
                    window.Loaded += ApplyTabbed_OnWindowLoaded;

                    break;

                case BackgroundType.Acrylic:
                    window.Loaded += ApplyAcrylic_OnWindowLoaded;

                    break;
            }
        }

        /// <summary>
        /// Applies selected background effect to Window by it's <see cref="IntPtr"/>.
        /// </summary>
        /// <param name="type">Backdrop type.</param>
        /// <param name="handle">Pointer to Window handle.</param>
        /// <param name="ignoreTitleBar">Whether to inform try to delete default TitleBar.</param>
        public static bool Apply(BackgroundType type, IntPtr handle, bool ignoreTitleBar = false)
        {
            if (handle == IntPtr.Zero)
            {
                return true;
            }

            if (!_handlers.Contains(handle))
            {
                _handlers.Add(handle);
            }

            switch (type)
            {
                case BackgroundType.Mica:
                    return ApplyMica(handle, ignoreTitleBar);

                case BackgroundType.Acrylic:
                    return ApplyAcrylic(handle, ignoreTitleBar);

                case BackgroundType.Tabbed:
                    return ApplyTabbed(handle, ignoreTitleBar);

                case BackgroundType.Auto:
                    return ApplyAuto(handle, ignoreTitleBar);
            }

            return false;
        }

        /// <summary>
        /// Informs the system that the <see cref="Window"/> is currently in dark mode.
        /// </summary>
        /// <param name="window">Window to be affected.</param>
        public static bool ApplyDarkMode(Window window)
        {
            return ApplyDarkMode(new WindowInteropHelper(window).Handle);
        }

        /// <summary>
        /// Informs the system that the HWND is currently in dark mode.
        /// </summary>
        /// <param name="handle">Pointer to Window handle.</param>
        public static bool ApplyDarkMode(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                return false;
            }

            int pvAttribute = (int)Dwmapi.PvAttribute.Enable;

            Dwmapi.DwmSetWindowAttribute(handle, Dwmapi.DWMWINDOWATTRIBUTE.DWMWA_USE_IMMERSIVE_DARK_MODE, ref pvAttribute,
                Marshal.SizeOf(typeof(int)));

            if (!_handlers.Contains(handle))
            {
                _handlers.Add(handle);
            }

            return true;
        }

        /// <summary>
        /// Removes the immersive dark mode attribute from <see cref="Window"/>.
        /// </summary>
        /// <param name="window">Window to be affected.</param>
        public static bool RemoveDarkMode(Window window)
        {
            return RemoveDarkMode(new WindowInteropHelper(window).Handle);
        }

        /// <summary>
        /// Removes the immersive dark mode attribute from HWND.
        /// </summary>
        /// <param name="handle">Pointer to Window handle.</param>
        public static bool RemoveDarkMode(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                return false;
            }

            int pvAttribute = (int)Dwmapi.PvAttribute.Disable;

            Dwmapi.DwmSetWindowAttribute(handle, Dwmapi.DWMWINDOWATTRIBUTE.DWMWA_USE_IMMERSIVE_DARK_MODE, ref pvAttribute,
                Marshal.SizeOf(typeof(int)));

            return true;
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

            int pvAttribute = (int)Dwmapi.PvAttribute.Disable;
            int backdropPvAttribute = (int)BackdropType.DWMSBT_DISABLE;

            Dwmapi.DwmSetWindowAttribute(handle, Dwmapi.DWMWINDOWATTRIBUTE.DWMWA_USE_IMMERSIVE_DARK_MODE, ref pvAttribute,
                Marshal.SizeOf(typeof(int)));

            Dwmapi.DwmSetWindowAttribute(handle, Dwmapi.DWMWINDOWATTRIBUTE.DWMWA_MICA_EFFECT, ref pvAttribute,
                Marshal.SizeOf(typeof(int)));

            Dwmapi.DwmSetWindowAttribute(handle, Dwmapi.DWMWINDOWATTRIBUTE.DWMWA_SYSTEMBACKDROP_TYPE, ref backdropPvAttribute,
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
        /// Indicates whether the specified <see cref="Window"/> has one of the effects applied.
        /// </summary>
        public static bool HasEffect(Window window)
        {
            return HasEffect(new WindowInteropHelper(window).Handle);
        }

        /// <summary>
        /// Indicates whether the specified HWND has one of the effects applied.
        /// </summary>
        public static bool HasEffect(IntPtr handle)
        {
            return _handlers.Contains(handle);
        }

        /// <summary>
        /// Checks if the current <see cref="Environment.OSVersion"/> supports selected <see cref="BackdropType"/>.
        /// </summary>
        /// <returns><see langword="true"/> if background effect is supported.</returns>
        public static bool IsSupported(BackgroundType type)
        {
            if (Environment.OSVersion.Platform != PlatformID.Win32NT)
            {
                return false;
            }

            switch (type)
            {
                case BackgroundType.Auto:
                case BackgroundType.Tabbed:
                    return Environment.OSVersion.Version.Build >= 22523; // Insider with new API

                case BackgroundType.Mica:
                    return Environment.OSVersion.Version.Build >= 20000; // Since W11

                case BackgroundType.Acrylic:
                    return Environment.OSVersion.Version.Build >= 7601; // NT 6.1

                case BackgroundType.Default:
                    return true;
            }

            return false;
        }

        private static bool ApplyAuto(IntPtr handle, bool ignoreTitleBar = false)
        {
            if (Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version.Build >= 22523)
            {
                int backdropPvAttribute = (int)BackdropType.DWMSBT_AUTO;

                Dwmapi.DwmSetWindowAttribute(handle, Dwmapi.DWMWINDOWATTRIBUTE.DWMWA_SYSTEMBACKDROP_TYPE,
                    ref backdropPvAttribute,
                    Marshal.SizeOf(typeof(int)));

                if (!_handlers.Contains(handle))
                {
                    _handlers.Add(handle);
                }

                return true;
            }

            return false;
        }

        private static bool ApplyTabbed(IntPtr handle, bool ignoreTitleBar = false)
        {
            if (Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version.Build >= 22523)
            {
                if (!ignoreTitleBar && !RemoveTitleBar(handle))
                {
                    return false;
                }

                int backdropPvAttribute = (int)BackdropType.DWMSBT_TABBEDWINDOW;

                Dwmapi.DwmSetWindowAttribute(handle, Dwmapi.DWMWINDOWATTRIBUTE.DWMWA_SYSTEMBACKDROP_TYPE,
                    ref backdropPvAttribute,
                    Marshal.SizeOf(typeof(int)));

                if (!_handlers.Contains(handle))
                {
                    _handlers.Add(handle);
                }

                return true;
            }

            return false;
        }

        private static bool ApplyMica(IntPtr handle, bool ignoreTitleBar = false)
        {
#if DEBUG
            System.Diagnostics.Debug.WriteLine("Apply Mica effect to: IntPtr " + handle);
#endif
            if (Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version.Build >= 22523)
            {
                if (!ignoreTitleBar && !RemoveTitleBar(handle))
                {
                    return false;
                }

                int backdropPvAttribute = (int)BackdropType.DWMSBT_MAINWINDOW;

                Dwmapi.DwmSetWindowAttribute(handle, Dwmapi.DWMWINDOWATTRIBUTE.DWMWA_SYSTEMBACKDROP_TYPE,
                    ref backdropPvAttribute,
                    Marshal.SizeOf(typeof(int)));

                if (!_handlers.Contains(handle))
                {
                    _handlers.Add(handle);
                }

                return true;
            }

            if (Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version.Build >= 20000)
            {
                if (!ignoreTitleBar && !RemoveTitleBar(handle))
                {
                    return false;
                }

                int backdropPvAttribute = (int)Dwmapi.PvAttribute.Enable;

                Dwmapi.DwmSetWindowAttribute(handle, Dwmapi.DWMWINDOWATTRIBUTE.DWMWA_MICA_EFFECT, ref backdropPvAttribute,
                    Marshal.SizeOf(typeof(int)));

                if (!_handlers.Contains(handle))
                {
                    _handlers.Add(handle);
                }

                return true;
            }

            return false;
        }

        private static bool ApplyAcrylic(IntPtr handle, bool ignoreTitleBar = false)
        {
            if (Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version.Build >= 22523)
            {
                if (!ignoreTitleBar && !RemoveTitleBar(handle))
                {
                    return false;
                }

                int backdropPvAttribute = (int)BackdropType.DWMSBT_TRANSIENTWINDOW;

                Dwmapi.DwmSetWindowAttribute(handle, Dwmapi.DWMWINDOWATTRIBUTE.DWMWA_SYSTEMBACKDROP_TYPE,
                    ref backdropPvAttribute,
                    Marshal.SizeOf(typeof(int)));

                if (!_handlers.Contains(handle))
                {
                    _handlers.Add(handle);
                }

                return true;
            }

            if (Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version.Build >= 7601)
            {
                //TODO: We need to set window transparency to True

                User32.ACCENT_POLICY accentPolicy = new User32.ACCENT_POLICY
                {
                    AccentState = User32.ACCENT_STATE.ACCENT_ENABLE_ACRYLICBLURBEHIND,
                    GradientColor = (0 << 24) | (0x990000 & 0xFFFFFF)
                };

                int accentStructSize = Marshal.SizeOf(accentPolicy);

                IntPtr accentPtr = Marshal.AllocHGlobal(accentStructSize);
                Marshal.StructureToPtr(accentPolicy, accentPtr, false);

                User32.WINCOMPATTRDATA data = new User32.WINCOMPATTRDATA
                {
                    Attribute = User32.WINCOMPATTR.WCA_ACCENT_POLICY,
                    SizeOfData = accentStructSize,
                    Data = accentPtr
                };

                User32.SetWindowCompositionAttribute(handle, ref data);

                Marshal.FreeHGlobal(accentPtr);

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
            catch
            {
#if DEBUG
                Console.WriteLine(e);
#endif
                return false;
            }
        }

        #region Window specific handlers

        // Each of these backdrops has its own event listener so that we can make simple modifications or corrections for each type.

        private static void ApplyMica_OnWindowLoaded(object sender, RoutedEventArgs e)
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

        private static void ApplyTabbed_OnWindowLoaded(object sender, RoutedEventArgs e)
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

        private static void ApplyAuto_OnWindowLoaded(object sender, RoutedEventArgs e)
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

        private static void ApplyAcrylic_OnWindowLoaded(object sender, RoutedEventArgs e)
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