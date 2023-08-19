// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Wpf.Ui.Controls;
using Wpf.Ui.Interop;

namespace Wpf.Ui.Appearance;

/// <summary>
/// Automatically updates the application background if the system theme or color is changed.
/// <para><see cref="SystemThemeWatcher"/> settings work globally and cannot be changed for each <see cref="System.Windows.Window"/>.</para>
/// </summary>
/// <example>
/// <code lang="csharp">
/// SystemThemeWatcher.Watch(this as System.Windows.Window);
/// SystemThemeWatcher.UnWatch(this as System.Windows.Window);
/// </code>
/// <code lang="csharp">
/// SystemThemeWatcher.Watch(
///     _serviceProvider.GetRequiredService&lt;MainWindow&gt;()
/// );
/// </code>
/// </example>
public static class SystemThemeWatcher
{
    private static readonly ICollection<ObservedWindow> _observedWindows =
        new List<ObservedWindow>();

    /// <summary>
    /// Watches the <see cref="Window"/> and applies the background effect and theme according to the system theme.
    /// </summary>
    /// <param name="window">The window that will be updated.</param>
    /// <param name="backdrop">Background effect to be applied when changing the theme.</param>
    /// <param name="updateAccents">If <see langword="true"/>, the accents will be updated when the change is detected.</param>
    /// <param name="forceBackgroundReplace">If <see langword="true"/>, bypasses the app's theme compatibility check and tries to force the change of a background effect.</param>
    public static void Watch(
        Window? window,
        WindowBackdropType backdrop = WindowBackdropType.Mica,
        bool updateAccents = true,
        bool forceBackgroundReplace = false
    )
    {
        if (window is null)
        {
            return;
        }

        if (window.IsLoaded)
        {
            ObserveLoadedWindow(window, backdrop, updateAccents, forceBackgroundReplace);
        }
        else
        {
            ObserveWindowWhenLoaded(window, backdrop, updateAccents, forceBackgroundReplace);
        }

        if (!_observedWindows.Any())
        {
#if DEBUG
            System.Diagnostics.Debug.WriteLine(
                $"INFO | {typeof(SystemThemeWatcher)} changed the app theme on initialization.",
                nameof(SystemThemeWatcher)
            );
#endif
            ApplicationThemeManager.ApplySystemTheme(updateAccents);
        }
    }

    private static void ObserveLoadedWindow(
        Window window,
        WindowBackdropType backdrop,
        bool updateAccents,
        bool forceBackgroundReplace
    )
    {
        IntPtr hWnd =
            (hWnd = new WindowInteropHelper(window).Handle) == IntPtr.Zero
                ? throw new InvalidOperationException("Could not get window handle.")
                : hWnd;

        if (hWnd == IntPtr.Zero)
        {
            throw new InvalidOperationException("Window handle cannot be empty");
        }

        ObserveLoadedHandle(
            new ObservedWindow(hWnd, backdrop, forceBackgroundReplace, updateAccents)
        );
    }

    private static void ObserveWindowWhenLoaded(
        Window window,
        WindowBackdropType backdrop,
        bool updateAccents,
        bool forceBackgroundReplace
    )
    {
        window.Loaded += (_, _) =>
        {
            IntPtr hWnd =
                (hWnd = new WindowInteropHelper(window).Handle) == IntPtr.Zero
                    ? throw new InvalidOperationException("Could not get window handle.")
                    : hWnd;

            if (hWnd == IntPtr.Zero)
            {
                throw new InvalidOperationException("Window handle cannot be empty");
            }

            ObserveLoadedHandle(
                new ObservedWindow(hWnd, backdrop, forceBackgroundReplace, updateAccents)
            );
        };
    }

    private static void ObserveLoadedHandle(ObservedWindow observedWindow)
    {
        if (!observedWindow.HasHook)
        {
#if DEBUG
            System.Diagnostics.Debug.WriteLine(
                $"INFO | {observedWindow.Handle} ({observedWindow.RootVisual?.Title}) registered as watched window.",
                nameof(SystemThemeWatcher)
            );
#endif
            observedWindow.AddHook(WndProc);
            _observedWindows.Add(observedWindow);
        }
    }

    /// <summary>
    /// Unwatches the window and removes the hook to receive messages from the system.
    /// </summary>
    public static void UnWatch(Window? window)
    {
        if (window is null)
        {
            return;
        }

        if (!window.IsLoaded)
        {
            throw new InvalidOperationException(
                "You cannot unwatch a window that is not yet loaded."
            );
        }

        IntPtr hWnd =
            (hWnd = new WindowInteropHelper(window).Handle) == IntPtr.Zero
                ? throw new InvalidOperationException("Could not get window handle.")
                : hWnd;

        ObservedWindow? observedWindow = _observedWindows.FirstOrDefault(x => x.Handle == hWnd);

        if (observedWindow is null)
        {
            return;
        }

        observedWindow.RemoveHook(WndProc);

        _ = _observedWindows.Remove(observedWindow);
    }

    /// <summary>
    /// Listens to system messages on the application windows.
    /// </summary>
    private static IntPtr WndProc(
        IntPtr hWnd,
        int msg,
        IntPtr wParam,
        IntPtr lParam,
        ref bool handled
    )
    {
        if (msg == (int)User32.WM.WININICHANGE)
        {
            UpdateObservedWindow(hWnd);
        }

        return IntPtr.Zero;
    }

    private static void UpdateObservedWindow(nint hWnd)
    {
        if (!UnsafeNativeMethods.IsValidWindow(hWnd))
        {
            return;
        }

        ObservedWindow? observedWindow = _observedWindows.FirstOrDefault(x => x.Handle == hWnd);

        if (observedWindow is null)
        {
            return;
        }

        ApplicationThemeManager.ApplySystemTheme(observedWindow.UpdateAccents);
        ApplicationTheme currentApplicationTheme = ApplicationThemeManager.GetAppTheme();

#if DEBUG
        System.Diagnostics.Debug.WriteLine(
            $"INFO | {observedWindow.Handle} ({observedWindow.RootVisual?.Title}) triggered the application theme change to {ApplicationThemeManager.GetSystemTheme()}.",
            nameof(SystemThemeWatcher)
        );
#endif

        if (observedWindow.RootVisual is not null)
        {
            WindowBackgroundManager.UpdateBackground(
                observedWindow.RootVisual,
                currentApplicationTheme,
                observedWindow.Backdrop,
                observedWindow.ForceBackgroundReplace
            );
        }
    }
}
