// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using WPFUI.Win32;

namespace WPFUI.Appearance
{
    /// <summary>
    /// Lets you apply background effects to <see cref="Window"/> or <c>hWnd</c> by its <see cref="IntPtr"/>.
    /// </summary>
    public static class Background
    {
        /// <summary>
        /// Checks if the current <see cref="Environment.OSVersion"/> supports selected <see cref="BackgroundType"/>.
        /// </summary>
        /// <param name="type">Background type to check.</param>
        /// <returns><see langword="true"/> if <see cref="BackgroundType"/> is supported.</returns>
        public static bool IsSupported(BackgroundType type)
        {
            if (Environment.OSVersion.Platform != PlatformID.Win32NT)
                return false;

            return type switch
            {
                BackgroundType.Auto => Environment.OSVersion.Version.Build >= 22523 // Insider with new API
                ,
                BackgroundType.Tabbed => Environment.OSVersion.Version.Build >= 22523 // Insider with new API
                ,
                BackgroundType.Mica => Environment.OSVersion.Version.Build >= 20000 // Since W11
                ,
                BackgroundType.Acrylic => Environment.OSVersion.Version.Build >= 7601 // NT 6.1
                ,
                _ => false
            };
        }

        /// <summary>
        /// Applies selected background effect to <see cref="Window"/> when is rendered.
        /// </summary>
        /// <param name="window">Window to apply effect.</param>
        /// <param name="type">Background type.</param>
        public static bool Apply(Window window, BackgroundType type)
        {
            if (!IsSupported(type) || !Theme.IsAppMatchesSystem())
                return false;

            // TODO: On rendered

            window.Loaded += (sender, args) =>
            {
                window.Background = Brushes.Transparent;

                PresentationSource.FromVisual(window)!.ContentRendered += (o, args) =>
                {
                    var windowHandle = new WindowInteropHelper(window).Handle;

                    if (windowHandle == IntPtr.Zero) return;

                    Apply(windowHandle, type);
                };
            };

            return true;
        }

        /// <summary>
        /// Applies selected background effect to <c>hWnd</c> by it's pointer.
        /// </summary>
        /// <param name="handle">Pointer to the window handle.</param>
        /// <param name="type">Background type.</param>
        public static bool Apply(IntPtr handle, BackgroundType type)
        {
            if (!IsSupported(type) || !Theme.IsAppMatchesSystem())
                return false;

            if (handle == IntPtr.Zero)
                return false;

            if (!AppearanceData.Handlers.Contains(handle))
                AppearanceData.Handlers.Add(handle);

            if (Theme.IsMatchedDark())
                ApplyDarkMode(handle);

            switch (type)
            {
                case BackgroundType.Auto:
                    return TryApplyAuto(handle);
                case BackgroundType.Mica:
                    return TryApplyMica(handle);
                case BackgroundType.Acrylic:
                    return TryApplyAcrylic(handle);
                case BackgroundType.Tabbed:
                    return TryApplyTabbed(handle);
                default:
                    return false;
            }
        }

        /// <summary>
        /// Tries to remove background effects if they have been applied to the <see cref="Window"/>.
        /// </summary>
        /// <param name="window">The window from which the effect should be removed.</param>
        public static void Remove(Window window)
        {
            var windowHandle = new WindowInteropHelper(window).Handle;

            if (windowHandle == IntPtr.Zero) return;

            Remove(windowHandle);
        }

        /// <summary>
        /// Tries to remove all effects if they have been applied to the <c>hWnd</c>.
        /// </summary>
        /// <param name="handle">Pointer to the window handle.</param>
        public static void Remove(IntPtr handle)
        {
            if (handle == IntPtr.Zero) return;

            int pvAttribute = (int)Dwmapi.PvAttribute.Disable;
            int backdropPvAttribute = (int)Dwmapi.DWMSBT.DWMSBT_DISABLE;

            Dwmapi.DwmSetWindowAttribute(handle, Dwmapi.DWMWINDOWATTRIBUTE.DWMWA_USE_IMMERSIVE_DARK_MODE,
                ref pvAttribute,
                Marshal.SizeOf(typeof(int)));

            Dwmapi.DwmSetWindowAttribute(handle, Dwmapi.DWMWINDOWATTRIBUTE.DWMWA_MICA_EFFECT, ref pvAttribute,
                Marshal.SizeOf(typeof(int)));

            Dwmapi.DwmSetWindowAttribute(handle, Dwmapi.DWMWINDOWATTRIBUTE.DWMWA_SYSTEMBACKDROP_TYPE,
                ref backdropPvAttribute,
                Marshal.SizeOf(typeof(int)));

            for (int i = 0; i < AppearanceData.Handlers.Count; i++)
            {
                if (AppearanceData.Handlers[i] != handle) continue;
                AppearanceData.Handlers.RemoveAt(i);

                break;
            }
        }

        /// <summary>
        /// Tries to inform the operating system that this window uses dark mode.
        /// </summary>
        /// <param name="window">Window to apply effect.</param>
        public static void ApplyDarkMode(Window window)
        {
            var windowHandle = new WindowInteropHelper(window).Handle;

            if (windowHandle == IntPtr.Zero) return;

            ApplyDarkMode(windowHandle);
        }

        /// <summary>
        /// Tries to inform the operating system that this <c>hWnd</c> uses dark mode.
        /// </summary>
        /// <param name="handle">Pointer to the window handle.</param>
        public static void ApplyDarkMode(IntPtr handle)
        {
            if (handle == IntPtr.Zero) return;

            int pvAttribute = (int)Dwmapi.PvAttribute.Enable;

            Dwmapi.DwmSetWindowAttribute(handle, Dwmapi.DWMWINDOWATTRIBUTE.DWMWA_USE_IMMERSIVE_DARK_MODE,
                ref pvAttribute,
                Marshal.SizeOf(typeof(int)));
        }

        /// <summary>
        /// Tries to clear the dark theme usage information.
        /// </summary>
        /// <param name="window">Window to remove effect.</param>
        public static void RemoveDarkMode(Window window)
        {
            var windowHandle = new WindowInteropHelper(window).Handle;

            if (windowHandle == IntPtr.Zero) return;

            RemoveDarkMode(windowHandle);
        }

        /// <summary>
        /// Tries to clear the dark theme usage information.
        /// </summary>
        /// <param name="handle">Pointer to the window handle.</param>
        public static void RemoveDarkMode(IntPtr handle)
        {
            if (handle == IntPtr.Zero) return;

            int pvAttribute = (int)Dwmapi.PvAttribute.Disable;

            Dwmapi.DwmSetWindowAttribute(handle, Dwmapi.DWMWINDOWATTRIBUTE.DWMWA_USE_IMMERSIVE_DARK_MODE,
                ref pvAttribute,
                Marshal.SizeOf(typeof(int)));
        }

        /// <summary>
        /// Tries to remove default TitleBar from <c>hWnd</c>.
        /// </summary>
        /// <param name="handle">Pointer to the window handle.</param>
        /// <returns><see langowrd="false"/> is problem occurs.</returns>
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

        private static bool TryApplyAuto(IntPtr handle)
        {
#if DEBUG
            System.Diagnostics.Debug.WriteLine($"INFO | {typeof(Background)} tries to apply {BackgroundType.Auto} effect to: {handle}", "WPFUI.Background");
#endif
            if (Environment.OSVersion.Platform != PlatformID.Win32NT || Environment.OSVersion.Version.Build < 22523)
                return false;

            if (!RemoveTitleBar(handle))
                return false;

            int backdropPvAttribute = (int)Dwmapi.DWMSBT.DWMSBT_AUTO;

            Dwmapi.DwmSetWindowAttribute(handle, Dwmapi.DWMWINDOWATTRIBUTE.DWMWA_SYSTEMBACKDROP_TYPE,
                ref backdropPvAttribute,
                Marshal.SizeOf(typeof(int)));

            if (!AppearanceData.Handlers.Contains(handle))
                AppearanceData.Handlers.Add(handle);

            return true;
        }

        private static bool TryApplyMica(IntPtr handle)
        {
#if DEBUG
            System.Diagnostics.Debug.WriteLine($"INFO | {typeof(Background)} tries to apply {BackgroundType.Mica} effect to: {handle}", "WPFUI.Background");
#endif
            if (Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version.Build >= 22523)
            {
                if (!RemoveTitleBar(handle))
                    return false;

                int backdropPvAttribute = (int)Dwmapi.DWMSBT.DWMSBT_MAINWINDOW;

                Dwmapi.DwmSetWindowAttribute(handle, Dwmapi.DWMWINDOWATTRIBUTE.DWMWA_SYSTEMBACKDROP_TYPE,
                    ref backdropPvAttribute,
                    Marshal.SizeOf(typeof(int)));

                if (!AppearanceData.Handlers.Contains(handle))
                    AppearanceData.Handlers.Add(handle);

                return true;
            }

            if (Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version.Build >= 20000)
            {
                if (!RemoveTitleBar(handle))
                    return false;

                int backdropPvAttribute = (int)Dwmapi.PvAttribute.Enable;

                Dwmapi.DwmSetWindowAttribute(handle, Dwmapi.DWMWINDOWATTRIBUTE.DWMWA_MICA_EFFECT,
                    ref backdropPvAttribute,
                    Marshal.SizeOf(typeof(int)));

                if (!AppearanceData.Handlers.Contains(handle))
                    AppearanceData.Handlers.Add(handle);

                return true;
            }

            return false;
        }

        private static bool TryApplyTabbed(IntPtr handle)
        {
#if DEBUG
            System.Diagnostics.Debug.WriteLine($"INFO | {typeof(Background)} tries to apply {BackgroundType.Tabbed} effect to: {handle}", "WPFUI.Background");
#endif
            if (Environment.OSVersion.Platform != PlatformID.Win32NT && Environment.OSVersion.Version.Build < 22523)
                return false;

            if (!RemoveTitleBar(handle))
                return false;

            int backdropPvAttribute = (int)Dwmapi.DWMSBT.DWMSBT_TABBEDWINDOW;

            Dwmapi.DwmSetWindowAttribute(handle, Dwmapi.DWMWINDOWATTRIBUTE.DWMWA_SYSTEMBACKDROP_TYPE,
                ref backdropPvAttribute,
                Marshal.SizeOf(typeof(int)));

            if (!AppearanceData.Handlers.Contains(handle))
                AppearanceData.Handlers.Add(handle);

            return true;
        }

        private static bool TryApplyAcrylic(IntPtr handle)
        {
#if DEBUG
            System.Diagnostics.Debug.WriteLine($"INFO | {typeof(Background)} tries to apply {BackgroundType.Acrylic} effect to: {handle}", "WPFUI.Background");
#endif
            if (Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version.Build >= 22523)
            {
                if (!RemoveTitleBar(handle))
                    return false;

                int backdropPvAttribute = (int)Dwmapi.DWMSBT.DWMSBT_TRANSIENTWINDOW;

                Dwmapi.DwmSetWindowAttribute(handle, Dwmapi.DWMWINDOWATTRIBUTE.DWMWA_SYSTEMBACKDROP_TYPE,
                    ref backdropPvAttribute,
                    Marshal.SizeOf(typeof(int)));

                if (!AppearanceData.Handlers.Contains(handle))
                    AppearanceData.Handlers.Add(handle);

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
    }
}