// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Windows;

using Wpf.Ui.Controls.Window;
using Wpf.Ui.Interop;

namespace Wpf.Ui.Appearance;

/// <summary>
/// Allows to manage available color themes from the library.
/// </summary>
public static class Theme
{
    /// <summary>
    /// Event triggered when the application's theme is changed.
    /// </summary>
    public static event ThemeChangedEvent? Changed;

    /// <summary>
    /// Gets a value that indicates whether the application is currently using the high contrast theme.
    /// </summary>
    /// <returns><see langword="true"/> if application uses high contrast theme.</returns>
    public static bool IsHighContrast() => AppearanceData.ApplicationTheme == ThemeType.HighContrast;

    /// <summary>
    /// Gets a value that indicates whether the Windows is currently using the high contrast theme.
    /// </summary>
    /// <returns><see langword="true"/> if system uses high contrast theme.</returns>
    public static bool IsSystemHighContrast() => SystemTheme.HighContrast;

    /// <summary>
    /// Changes the current application theme.
    /// </summary>
    /// <param name="themeType">Theme to set.</param>
    /// <param name="backgroundEffect">Whether the custom background effect should be applied.</param>
    /// <param name="updateAccent">Whether the color accents should be changed.</param>
    /// <param name="forceBackground">If <see langword="true"/>, bypasses the app's theme compatibility check and tries to force the change of a background effect.</param>
    public static void Apply(ThemeType themeType, WindowBackdropType backgroundEffect = WindowBackdropType.Mica,
        bool updateAccent = true, bool forceBackground = false)
    {
        if (updateAccent)
            Accent.Apply(
                Accent.GetColorizationColor(),
                themeType,
                false
            );

        if (themeType == ThemeType.Unknown)
            return;

        var appDictionaries = new ResourceDictionaryManager(AppearanceData.LibraryNamespace);

        var themeDictionaryName = "Light";

        switch (themeType)
        {
            case ThemeType.Dark:
                themeDictionaryName = "Dark";
                break;
        }

        var isUpdated = appDictionaries.UpdateDictionary(
            "theme",
            new Uri(
                AppearanceData.LibraryThemeDictionariesUri + themeDictionaryName + ".xaml",
                UriKind.Absolute
            )
        );

        //var wpfUiDictionary = appDictionaries.GetDictionary("wpf.ui");

        // Force reloading ALL dictionaries
        // Works but is terrible
        //var isCoreUpdated = appDictionaries.UpdateDictionary(
        //    "wpf.ui",
        //    new Uri(
        //        AppearanceData.LibraryDictionariesUri + "Wpf.Ui.xaml",
        //        UriKind.Absolute
        //    )
        //);

        //var isBrushesUpdated = appDictionaries.UpdateDictionary(
        //        "assets/brushes",
        //        new Uri(
        //            AppearanceData.LibraryDictionariesUri + "Assets/Brushes.xaml",
        //            UriKind.Absolute
        //        )
        //    );

#if DEBUG
        System.Diagnostics.Debug.WriteLine(
            $"INFO | {typeof(Theme)} tries to update theme to {themeDictionaryName} ({themeType}): {isUpdated}",
            "Wpf.Ui.Theme");
#endif
        if (!isUpdated)
            return;

        AppearanceData.ApplicationTheme = themeType;

        Changed?.Invoke(themeType, Accent.SystemAccent);

        UpdateBackground(themeType, backgroundEffect, forceBackground);
    }

    /// <summary>
    /// Gets currently set application theme.
    /// </summary>
    /// <returns><see cref="ThemeType.Unknown"/> if something goes wrong.</returns>
    public static ThemeType GetAppTheme()
    {
        if (AppearanceData.ApplicationTheme == ThemeType.Unknown)
            FetchApplicationTheme();

        return AppearanceData.ApplicationTheme;
    }

    /// <summary>
    /// Gets currently set system theme.
    /// </summary>
    /// <returns><see cref="SystemThemeType.Unknown"/> if something goes wrong.</returns>
    public static SystemThemeType GetSystemTheme()
    {
        if (AppearanceData.SystemTheme == SystemThemeType.Unknown)
            FetchSystemTheme();

        return AppearanceData.SystemTheme;
    }

    /// <summary>
    /// Gets a value that indicates whether the application is matching the system theme.
    /// </summary>
    /// <returns><see langword="true"/> if the application has the same theme as the system.</returns>
    public static bool IsAppMatchesSystem()
    {
        var appTheme = GetAppTheme();
        var sysTheme = GetSystemTheme();

        return appTheme switch
        {
            ThemeType.Dark => sysTheme is SystemThemeType.Dark or SystemThemeType.CapturedMotion
                or SystemThemeType.Glow,
            ThemeType.Light => sysTheme is SystemThemeType.Light or SystemThemeType.Flow or SystemThemeType.Sunrise,
            _ => appTheme == ThemeType.HighContrast && SystemTheme.HighContrast
        };
    }

    /// <summary>
    /// Checks if the application and the operating system are currently working in a dark theme.
    /// </summary>
    public static bool IsMatchedDark()
    {
        var appTheme = GetAppTheme();
        var sysTheme = GetSystemTheme();

        if (appTheme != ThemeType.Dark)
            return false;

        return sysTheme is SystemThemeType.Dark or SystemThemeType.CapturedMotion or SystemThemeType.Glow;
    }

    /// <summary>
    /// Checks if the application and the operating system are currently working in a light theme.
    /// </summary>
    public static bool IsMatchedLight()
    {
        var appTheme = GetAppTheme();
        var sysTheme = GetSystemTheme();

        if (appTheme != ThemeType.Light)
            return false;

        return sysTheme is SystemThemeType.Light or SystemThemeType.Flow or SystemThemeType.Sunrise;
    }

    /// <summary>
    /// Tries to apply dark theme to <see cref="Window"/>.
    /// </summary>
    public static bool ApplyDarkThemeToWindow(Window window)
    {
        if (window == null)
            return false;

        if (window.IsLoaded)
            return UnsafeNativeMethods.ApplyWindowDarkMode(window);

        window.Loaded += (sender, _) => UnsafeNativeMethods.ApplyWindowDarkMode(sender as Window);

        return true;
    }

    /// <summary>
    /// Tries to remove dark theme from <see cref="Window"/>.
    /// </summary>
    public static bool RemoveDarkThemeFromWindow(Window window)
    {
        if (window == null)
            return false;

        if (window.IsLoaded)
            return UnsafeNativeMethods.RemoveWindowDarkMode(window);

        window.Loaded += (sender, _) => UnsafeNativeMethods.RemoveWindowDarkMode(sender as Window);

        return true;
    }

    /// <summary>
    /// Tries to guess the currently set application theme.
    /// </summary>
    private static void FetchApplicationTheme()
    {
        var appDictionaries = new ResourceDictionaryManager(AppearanceData.LibraryNamespace);
        var themeDictionary = appDictionaries.GetDictionary("theme");

        if (themeDictionary == null)
            return;

        var themeUri = themeDictionary.Source.ToString().Trim().ToLower();

        if (themeUri.Contains("light"))
            AppearanceData.ApplicationTheme = ThemeType.Light;
        else if (themeUri.Contains("dark"))
            AppearanceData.ApplicationTheme = ThemeType.Dark;
        else if (themeUri.Contains("highcontrast"))
            AppearanceData.ApplicationTheme = ThemeType.HighContrast;
    }

    /// <summary>
    /// Tries to guess the currently set system theme.
    /// </summary>
    private static void FetchSystemTheme()
    {
        AppearanceData.SystemTheme = SystemTheme.GetTheme();
    }

    /// <summary>
    /// Forces change to application background. Required if custom background effect was previously applied.
    /// </summary>
    private static void UpdateBackground(ThemeType themeType,
        WindowBackdropType backgroundEffect = WindowBackdropType.None, bool forceBackground = false)
    {
        var handles = AppearanceData.ModifiedBackgroundHandles;

        foreach (var singleHandle in handles)
        {
            WindowBackdrop.ApplyBackdrop(singleHandle, backgroundEffect);
        }
        // TODO: All windows

        if (!AppearanceData.HasHandle(Application.Current.MainWindow))
        {
            WindowBackdrop.ApplyBackdrop(Application.Current.MainWindow, backgroundEffect);
            AppearanceData.AddHandle(Application.Current.MainWindow);
        }

        // Do we really neeed this?
        //if (!Win32.Utilities.IsOSWindows11OrNewer)
        //{
        //    var mainWindow = Application.Current.MainWindow;

        //    if (mainWindow == null)
        //        return;

        //    var backgroundColor = Application.Current.Resources["ApplicationBackgroundColor"];
        //    if (backgroundColor is Color color)
        //        mainWindow.Background = new SolidColorBrush(color);
        //}


        //        var mainWindow = Application.Current.MainWindow;

        //        if (mainWindow == null)
        //            return;

        //        // TODO: Do not refresh window presenter background if already applied
        //        var backgroundColor = Application.Current.Resources["ApplicationBackgroundColor"];
        //        if (backgroundColor is Color color)
        //            mainWindow.Background = new SolidColorBrush(color);

        //#if DEBUG
        //        System.Diagnostics.Debug.WriteLine($"INFO | Current background color: {backgroundColor}", "Wpf.Ui.Theme");
        //#endif

        //        var windowHandle = new WindowInteropHelper(mainWindow).Handle;

        //        if (windowHandle == IntPtr.Zero)
        //            return;

        //        Background.Remove(windowHandle);

        //        //if (!IsAppMatchesSystem() || backgroundEffect == BackgroundType.Unknown)
        //        //    return;

        //        if (backgroundEffect == BackgroundType.Unknown)
        //            return;

        //        // TODO: Improve
        //        if (Background.Apply(windowHandle, backgroundEffect, forceBackground))
        //            mainWindow.Background = Brushes.Transparent;
    }
}
