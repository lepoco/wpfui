// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski, Ch0pstix and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Windows;
using System.Windows.Interop;

namespace Wpf.Ui.Appearance;

// https://github.com/lepoco/wpfui/issues/55

/// <summary>
/// Automatically updates the application background if the system theme or color is changed.
/// <para><see cref="Watcher"/> settings work globally and cannot be changed for each <see cref="System.Windows.Window"/>.</para>
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

    /// <summary>
    /// Gets or sets a value that indicates whether the <see cref="Watcher"/> forces the background effect to be applied.
    /// </summary>
    public bool ForceBackground { get; set; } = false;

    //public static void Register(Application app, BackgroundType backgroundEffect = BackgroundType.Mica,
    //    bool updateAccents = true)
    //{
    // //TO DO
    //}

    /// <summary>
    /// Creates a new instance of <see cref="Watcher"/> and attaches the instance to the given <see cref="Window"/>.
    /// </summary>
    /// <param name="window">The window that will be updated by <see cref="Watcher"/>.</param>
    /// <param name="backgroundEffect">Background effect to be applied when changing the theme.</param>
    /// <param name="updateAccents">If <see langword="true"/>, the accents will be updated when the change is detected.</param>
    /// <param name="forceBackground">If <see langword="true"/>, bypasses the app's theme compatibility check and tries to force the change of a background effect.</param>
    public static void Watch(Window window, BackgroundType backgroundEffect = BackgroundType.Mica,
        bool updateAccents = true, bool forceBackground = false)
    {
        if (window == null)
            return;

        if (window.IsLoaded)
        {
            // Get the handle from the window
            IntPtr hwnd =
                (hwnd = new WindowInteropHelper(window).Handle) == IntPtr.Zero
                    ? throw new InvalidOperationException("Could not get window handle.")
                    : hwnd;

            // Initialize a new instance with the window handle
            var watcher = new Watcher(hwnd, backgroundEffect, updateAccents, forceBackground);

            // Updates themes on initialization if the current system theme is different from the app's.
            var currentSystemTheme = SystemTheme.GetTheme();
            watcher.UpdateThemes(currentSystemTheme);

            return;
        }

        window.Loaded += (sender, args) =>
        {
            // Get the handle from the window
            IntPtr hwnd =
                (hwnd = new WindowInteropHelper(window).Handle) == IntPtr.Zero
                    ? throw new InvalidOperationException("Could not get window handle.")
                    : hwnd;

            // Initialize a new instance with the window handle
            var watcher = new Watcher(hwnd, backgroundEffect, updateAccents, forceBackground);

            // Updates themes on initialization if the current system theme is different from the app's.
            var currentSystemTheme = SystemTheme.GetTheme();
            watcher.UpdateThemes(currentSystemTheme);
        };
    }

    /// <summary>
    /// Initializes a new instance of <see cref="Watcher"/>.
    /// </summary>
    /// <param name="hWnd">Window handle</param>
    /// <param name="backgroundEffect">Background effect to be applied when changing the theme.</param>
    /// <param name="updateAccents">If <see langword="true"/>, the accents will be updated when the change is detected.</param>
    /// <param name="forceBackground">If <see langword="true"/>, bypasses the app's theme compatibility check and tries to force the change of a background effect.</param>
    public Watcher(IntPtr hWnd, BackgroundType backgroundEffect, bool updateAccents, bool forceBackground)
    {
        var hWndSource = HwndSource.FromHwnd(hWnd);

        BackgroundEffect = backgroundEffect;
        ForceBackground = forceBackground;
        UpdateAccents = updateAccents;

        hWndSource?.AddHook(WndProc);
    }

    /// <summary>
    /// Listens to system messages on the application windows.
    /// </summary>
    private IntPtr WndProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
    {
        if (msg != (int)Interop.User32.WM.WININICHANGE)
            return IntPtr.Zero;

        var currentSystemTheme = SystemTheme.GetTheme();
        UpdateThemes(currentSystemTheme);

        return IntPtr.Zero;
    }

    private void UpdateThemes(SystemThemeType systemTheme)
    {
        AppearanceData.SystemTheme = systemTheme;

        var themeToSet = ThemeType.Light;

        if (systemTheme is SystemThemeType.Dark or SystemThemeType.CapturedMotion or SystemThemeType.Glow)
            themeToSet = ThemeType.Dark;

        Theme.Apply(themeToSet, BackgroundEffect, UpdateAccents, ForceBackground);

#if DEBUG
        System.Diagnostics.Debug.WriteLine($"INFO | {typeof(Watcher)} changed the app theme.", "Wpf.Ui.Watcher");
        System.Diagnostics.Debug.WriteLine($"INFO | Current accent: {Accent.SystemAccent}", "Wpf.Ui.Watcher");
        System.Diagnostics.Debug.WriteLine($"INFO | Current app theme: {AppearanceData.ApplicationTheme}",
            "Wpf.Ui.Watcher");
        System.Diagnostics.Debug.WriteLine($"INFO | Current system theme: {AppearanceData.SystemTheme}",
            "Wpf.Ui.Watcher");
#endif
    }
}
