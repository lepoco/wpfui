// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski, Ch0pstix and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Windows;
using System.Windows.Interop;

using Wpf.Ui.Controls.Window;

namespace Wpf.Ui.Appearance;

// https://github.com/lepoco/wpfui/issues/55

/// <summary>
/// Automatically updates the application background if the system theme or color is changed.
/// <para><see cref="Watcher"/> settings work globally and cannot be changed for each <see cref="System.Windows.Window"/>.</para>
/// </summary>
public static class Watcher
{
    /// <summary>
    /// Gets or sets the background effect for the window uses custom <see cref="WindowBackdropType"/>.
    /// </summary>
    public static WindowBackdropType BackgroundEffect { get; set; } = WindowBackdropType.None;

    /// <summary>
    /// Gets or sets a value indicating whether to update the accent colors when the theme changes uses <see cref="Accent"/>.
    /// </summary>
    public static bool UpdateAccents { get; set; } = false;

    /// <summary>
    /// Gets or sets a value indicating whether to force the background effect even if it is not supported by the system.
    /// </summary>
    public static bool ForceBackground { get; set; } = false;

    /// <summary>
    /// Gets or sets the HwndSource object that represents the window handle.
    /// </summary>
    public static HwndSource hWndSource { get; set; } = null!;

    /// <summary>
    /// Gets or sets a value indicating whether the window has a hook to receive messages from the system.
    /// </summary>
    public static bool HasHook { get; set; }

    //public static void Register(Application app, WindowBackdropType backgroundEffect = WindowBackdropType.Mica,
    //    bool updateAccents = true)
    //{
    // //TO DO
    //}

    /// <summary>
    /// Watches the <see cref="Window"/> and applies the background effect and theme according to the system theme.
    /// </summary>
    /// <param name="window">The window that will be updated.</param>
    /// <param name="backgroundEffect">Background effect to be applied when changing the theme.</param>
    /// <param name="updateAccents">If <see langword="true"/>, the accents will be updated when the change is detected.</param>
    /// <param name="forceBackground">If <see langword="true"/>, bypasses the app's theme compatibility check and tries to force the change of a background effect.</param>
    public static void Watch(Window window, WindowBackdropType backgroundEffect = WindowBackdropType.Mica,
        bool updateAccents = true, bool forceBackground = false)
    {
        if (window == null)
            return;

        BackgroundEffect = backgroundEffect;
        ForceBackground = forceBackground;
        UpdateAccents = updateAccents;

        if (window.IsLoaded)
        {
            // Get the handle from the window
            IntPtr hwnd =
                (hwnd = new WindowInteropHelper(window).Handle) == IntPtr.Zero
                    ? throw new InvalidOperationException("Could not get window handle.")
                    : hwnd;

            // Initialize a new instance with the window handle
            Watch(hwnd);

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
            Watch(hwnd);
        };
    }

    /// <summary>
    /// Unwatches the window and removes the hook to receive messages from the system.
    /// </summary>
    public static void UnWatch()
    {
        if (HasHook)
        {
            hWndSource.RemoveHook(WndProc);
            HasHook = false;
        }
    }

    /// <summary>
    /// Watches the window handle and adds a hook to receive messages from the system.
    /// </summary>
    /// <param name="hWnd"></param>
    private static void Watch(IntPtr hWnd)
    {
        if (!HasHook)
        {
            hWndSource = HwndSource.FromHwnd(hWnd);
            hWndSource.AddHook(WndProc);
            HasHook = true;
        }

        // Updates themes on initialization if the current system theme is different from the app's.
        UpdateThemes(systemTheme: SystemTheme.GetTheme());
    }

    /// <summary>
    /// Listens to system messages on the application windows.
    /// </summary>
    private static IntPtr WndProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
    {
        if (msg == (int)Interop.User32.WM.WININICHANGE)
        {
            UpdateThemes(systemTheme: SystemTheme.GetTheme());
        }

        return IntPtr.Zero;
    }

    /// <summary>
    /// Updates the themes according to the system theme and applies them to the window.
    /// </summary>
    /// <param name="systemTheme"></param>
    private static void UpdateThemes(SystemThemeType systemTheme)
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
