// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski, Ch0pstix and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Windows;
using System.Windows.Interop;

namespace WPFUI.Appearance
{
    // https://github.com/lepoco/wpfui/issues/55

    /// <summary>
    /// Automatically updates the application background if the system theme or color is changed.
    /// <para><see cref="Watcher"/> settings work globally as a singleton, they cannot be changed for each <see cref="System.Windows.Window"/>.</para>
    /// </summary>
    public sealed class Watcher
    {
        /// <summary>
        /// Gets or sets a value that indicates whether the <see cref="Watcher"/> uses custom <see cref="BackgroundType"/>.
        /// </summary>
        public BackgroundType BackgroundEffect { get; set; } = BackgroundType.Unknown;

        /// <summary>
        /// Gets or sets a value that indicates whether the <see cref="Watcher"/> uses <see cref="Accent"/>.
        /// </summary>
        public bool UpdateAccents { get; set; } = false;

        //public static void Register(Application app, BackgroundType backgroundEffect = BackgroundType.Mica,
        //    bool updateAccents = true)
        //{
        // //TO DO
        //}

        /// <summary>
        /// Creates a new instance of <see cref="Watcher"/> and attaches the instance to the given <see cref="Window"/>.
        /// </summary>
        public static Watcher Watch(Window window, BackgroundType backgroundEffect = BackgroundType.Mica,
            bool updateAccents = true)
        {
            // Get the handle from the window
            IntPtr hwnd =
                (hwnd = new WindowInteropHelper(window).Handle) == IntPtr.Zero
                    ? throw new InvalidOperationException("Could not get window handle.")
                    : hwnd;

            // Initialize a new instance with the window handle
            Watcher watcher = new(hwnd, backgroundEffect, updateAccents);

            // Updates themes on initialization if the current system theme is different from the app's.
            var currentSystemTheme = SystemTheme.GetTheme();
            watcher.UpdateThemes(currentSystemTheme);

            return watcher;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="Watcher"/>.
        /// </summary>
        /// <param name="hWnd">Window handle</param>
        /// <param name="backgroundEffect">Background effect to be applied when changing the theme.</param>
        /// <param name="updateAccents">If <see langword="true"/>, the accents will be updated when the change is detected.</param>
        public Watcher(IntPtr hWnd, BackgroundType backgroundEffect, bool updateAccents)
        {
            var hWndSource = HwndSource.FromHwnd(hWnd);

            BackgroundEffect = backgroundEffect;
            UpdateAccents = updateAccents;

            if (hWndSource == null) return;

            hWndSource.AddHook(WndProc);
        }

        /// <summary>
        /// Listens to system messages on the application windows.
        /// </summary>
        private IntPtr WndProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == (int)Win32.User32.WM.WININICHANGE)
            {
                var currentSystemTheme = SystemTheme.GetTheme();
                UpdateThemes(currentSystemTheme);
            }

            return IntPtr.Zero;
        }

        private void UpdateThemes(SystemThemeType systemTheme)
        {
            AppearanceData.SystemTheme = systemTheme;

            var themeToSet = ThemeType.Light;

            if (systemTheme is SystemThemeType.Dark or SystemThemeType.CapturedMotion or SystemThemeType.Glow)
                themeToSet = ThemeType.Dark;

            Theme.Set(themeToSet, BackgroundEffect, UpdateAccents);

#if DEBUG
            System.Diagnostics.Debug.WriteLine($"INFO | {typeof(Watcher)} changed the app theme.", "WPFUI.Watcher");
            System.Diagnostics.Debug.WriteLine($"INFO | Current accent: {Accent.SystemAccent}", "WPFUI.Watcher");
            System.Diagnostics.Debug.WriteLine($"INFO | Current app theme: {AppearanceData.ApplicationTheme}",
                "WPFUI.Watcher");
            System.Diagnostics.Debug.WriteLine($"INFO | Current system theme: {AppearanceData.SystemTheme}",
                "WPFUI.Watcher");
#endif
        }
    }
}