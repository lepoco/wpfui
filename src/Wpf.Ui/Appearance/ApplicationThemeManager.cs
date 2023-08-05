// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Windows;

using Wpf.Ui.Controls;
using Wpf.Ui.Interop;

namespace Wpf.Ui.Appearance;

/// <summary>
/// Allows to manage available color themes from the library.
/// </summary>
public static class ApplicationThemeManager
{
    /// <summary>
    /// Event triggered when the application's theme is changed.
    /// </summary>
    public static event ThemeChangedEvent? Changed;

    /// <summary>
    /// Gets a value that indicates whether the application is currently using the high contrast theme.
    /// </summary>
    /// <returns><see langword="true"/> if application uses high contrast theme.</returns>
    public static bool IsHighContrast() =>
        AppearanceData.ApplicationTheme == ApplicationTheme.HighContrast;

    /// <summary>
    /// Gets a value that indicates whether the Windows is currently using the high contrast theme.
    /// </summary>
    /// <returns><see langword="true"/> if system uses high contrast theme.</returns>
    public static bool IsSystemHighContrast() => SystemThemeManager.HighContrast;

    /// <summary>
    /// Changes the current application theme.
    /// </summary>
    /// <param name="applicationTheme">Theme to set.</param>
    /// <param name="backgroundEffect">Whether the custom background effect should be applied.</param>
    /// <param name="updateAccent">Whether the color accents should be changed.</param>
    /// <param name="forceBackground">If <see langword="true"/>, bypasses the app's theme compatibility check and tries to force the change of a background effect.</param>
    public static void Apply(
        ApplicationTheme applicationTheme,
        WindowBackdropType backgroundEffect = WindowBackdropType.Mica,
        bool updateAccent = true,
        bool forceBackground = false
    )
    {
        if (updateAccent)
        {
            ApplicationAccentColorManager.Apply(
                ApplicationAccentColorManager.GetColorizationColor(),
                applicationTheme,
                false
            );
        }

        if (applicationTheme == ApplicationTheme.Unknown)
        {
            return;
        }

        var appDictionaries = new ResourceDictionaryManager(AppearanceData.LibraryNamespace);

        var themeDictionaryName = "Light";

        switch (applicationTheme)
        {
            case ApplicationTheme.Dark:
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
            $"INFO | {typeof(ApplicationThemeManager)} tries to update theme to {themeDictionaryName} ({applicationTheme}): {isUpdated}",
            "Wpf.Ui.Theme"
        );
#endif
        if (!isUpdated)
            return;

        AppearanceData.ApplicationTheme = applicationTheme;

        Changed?.Invoke(applicationTheme, ApplicationAccentColorManager.SystemAccent);

        UpdateBackground(applicationTheme, backgroundEffect, forceBackground);
    }

    /// <summary>
    /// Gets currently set application theme.
    /// </summary>
    /// <returns><see cref="ApplicationTheme.Unknown"/> if something goes wrong.</returns>
    public static ApplicationTheme GetAppTheme()
    {
        if (AppearanceData.ApplicationTheme == ApplicationTheme.Unknown)
            FetchApplicationTheme();

        return AppearanceData.ApplicationTheme;
    }

    /// <summary>
    /// Gets currently set system theme.
    /// </summary>
    /// <returns><see cref="SystemTheme.Unknown"/> if something goes wrong.</returns>
    public static SystemTheme GetSystemTheme()
    {
        if (AppearanceData.SystemTheme == SystemTheme.Unknown)
        {
            FetchSystemTheme();
        }

        return AppearanceData.SystemTheme;
    }

    /// <summary>
    /// Gets a value that indicates whether the application is matching the system theme.
    /// </summary>
    /// <returns><see langword="true"/> if the application has the same theme as the system.</returns>
    public static bool IsAppMatchesSystem()
    {
        ApplicationTheme appApplicationTheme = GetAppTheme();
        SystemTheme sysTheme = GetSystemTheme();

        return appApplicationTheme switch
        {
            ApplicationTheme.Dark
                => sysTheme is SystemTheme.Dark or SystemTheme.CapturedMotion or SystemTheme.Glow,
            ApplicationTheme.Light
                => sysTheme is SystemTheme.Light or SystemTheme.Flow or SystemTheme.Sunrise,
            _
                => appApplicationTheme == ApplicationTheme.HighContrast
                    && SystemThemeManager.HighContrast
        };
    }

    /// <summary>
    /// Checks if the application and the operating system are currently working in a dark theme.
    /// </summary>
    public static bool IsMatchedDark()
    {
        ApplicationTheme appApplicationTheme = GetAppTheme();
        SystemTheme sysTheme = GetSystemTheme();

        if (appApplicationTheme != ApplicationTheme.Dark)
        {
            return false;
        }

        return sysTheme is SystemTheme.Dark or SystemTheme.CapturedMotion or SystemTheme.Glow;
    }

    /// <summary>
    /// Checks if the application and the operating system are currently working in a light theme.
    /// </summary>
    public static bool IsMatchedLight()
    {
        ApplicationTheme appApplicationTheme = GetAppTheme();
        SystemTheme sysTheme = GetSystemTheme();

        if (appApplicationTheme != ApplicationTheme.Light)
        {
            return false;
        }

        return sysTheme is SystemTheme.Light or SystemTheme.Flow or SystemTheme.Sunrise;
    }

    /// <summary>
    /// Tries to apply dark theme to <see cref="Window"/>.
    /// </summary>
    public static bool ApplyDarkThemeToWindow(Window window)
    {
        if (window == null)
        {
            return false;
        }

        if (window.IsLoaded)
        {
            return UnsafeNativeMethods.ApplyWindowDarkMode(window);
        }

        window.Loaded += (sender, _) => UnsafeNativeMethods.ApplyWindowDarkMode(sender as Window);

        return true;
    }

    /// <summary>
    /// Tries to remove dark theme from <see cref="Window"/>.
    /// </summary>
    public static bool RemoveDarkThemeFromWindow(Window window)
    {
        if (window == null)
        {
            return false;
        }

        if (window.IsLoaded)
        {
            return UnsafeNativeMethods.RemoveWindowDarkMode(window);
        }

        window.Loaded += (sender, _) => UnsafeNativeMethods.RemoveWindowDarkMode(sender as Window);

        return true;
    }

    /// <summary>
    /// Tries to guess the currently set application theme.
    /// </summary>
    private static void FetchApplicationTheme()
    {
        ResourceDictionaryManager appDictionaries = new(AppearanceData.LibraryNamespace);
        ResourceDictionary? themeDictionary = appDictionaries.GetDictionary("theme");

        if (themeDictionary == null)
        {
            return;
        }

        var themeUri = themeDictionary.Source.ToString().Trim().ToLower();

        if (themeUri.Contains("light"))
        {
            AppearanceData.ApplicationTheme = ApplicationTheme.Light;
        }

        if (themeUri.Contains("dark"))
        {
            AppearanceData.ApplicationTheme = ApplicationTheme.Dark;
        }

        if (themeUri.Contains("highcontrast"))
        {
            AppearanceData.ApplicationTheme = ApplicationTheme.HighContrast;
        }
    }

    /// <summary>
    /// Tries to guess the currently set system theme.
    /// </summary>
    private static void FetchSystemTheme()
    {
        AppearanceData.SystemTheme = SystemThemeManager.GetCurrentSystemTheme();
    }

    /// <summary>
    /// Forces change to application background. Required if custom background effect was previously applied.
    /// </summary>
    private static void UpdateBackground(
        ApplicationTheme applicationTheme,
        WindowBackdropType backgroundEffect = WindowBackdropType.None,
        bool forceBackground = false
    )
    {
        List<IntPtr> handles = AppearanceData.ModifiedBackgroundHandles;

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
