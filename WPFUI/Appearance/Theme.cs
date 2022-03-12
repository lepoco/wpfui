// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

namespace WPFUI.Appearance
{
    /// <summary>
    /// Allows to manage available color themes from the library.
    /// </summary>
    public static class Theme
    {
        /// <summary>
        /// Event triggered when the application's theme is changed.
        /// </summary>
        public static event ThemeChangedEvent OnChange;

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
        public static void Set(ThemeType themeType, BackgroundType backgroundEffect = BackgroundType.Mica, bool updateAccent = true)
        {
            if (updateAccent)
                Accent.Change(SystemTheme.GlassColor, themeType, true);

            if (themeType == ThemeType.Unknown || themeType == AppearanceData.ApplicationTheme) return;

            var appDictionaries = new ResourceDictionaryManager(AppearanceData.LibraryNamespace);

            var themeDictionaryName = "Light";

            switch (themeType)
            {
                case ThemeType.Dark:
                    themeDictionaryName = "Dark";
                    break;
            }

            bool isUpdated = appDictionaries.UpdateDictionary(
                "theme",
                new Uri(
                    AppearanceData.LibraryThemeDictionariesUri + themeDictionaryName + ".xaml",
                    UriKind.Absolute
                )
            );

#if DEBUG
            System.Diagnostics.Debug.WriteLine(
                $"INFO | {typeof(Theme)} tries to update theme to {themeDictionaryName} ({themeType}): {isUpdated}", "WPFUI.Theme");
#endif
            if (!isUpdated) return;

            AppearanceData.ApplicationTheme = themeType;

            if (OnChange != null)
                OnChange(themeType, Accent.SystemAccent);

            UpdateBackground(themeType, backgroundEffect);
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
            if (AppearanceData.ApplicationTheme == ThemeType.Dark)
            {
                return AppearanceData.SystemTheme == SystemThemeType.Dark ||
                       AppearanceData.SystemTheme == SystemThemeType.CapturedMotion ||
                       AppearanceData.SystemTheme == SystemThemeType.Glow;
            }

            if (AppearanceData.ApplicationTheme == ThemeType.Light)
            {
                return AppearanceData.SystemTheme == SystemThemeType.Light ||
                       AppearanceData.SystemTheme == SystemThemeType.Flow ||
                       AppearanceData.SystemTheme == SystemThemeType.Sunrise;
            }

            return AppearanceData.ApplicationTheme == ThemeType.HighContrast && SystemTheme.HighContrast;
        }

        /// <summary>
        /// Checks if the application and the operating system are currently working in a dark theme.
        /// </summary>
        public static bool IsMatchedDark()
        {
            if (AppearanceData.ApplicationTheme != ThemeType.Dark)
                return false;

            return AppearanceData.SystemTheme == SystemThemeType.Dark ||
                   AppearanceData.SystemTheme == SystemThemeType.CapturedMotion ||
                   AppearanceData.SystemTheme == SystemThemeType.Glow;
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
        private static void UpdateBackground(ThemeType themeType, BackgroundType backgroundEffect = BackgroundType.Unknown)
        {
            var mainWindow = Application.Current.MainWindow;

            if (mainWindow == null)
                return;

            var backgroundColor = Application.Current.Resources["ApplicationBackgroundColor"];
            if (backgroundColor is Color color)
                mainWindow.Background = new SolidColorBrush(color);

#if DEBUG
            System.Diagnostics.Debug.WriteLine($"INFO | Current background color: {backgroundColor}", "WPFUI.Theme");
#endif

            var windowHandle = new WindowInteropHelper(mainWindow).Handle;

            if (windowHandle == IntPtr.Zero) return;

            Background.Remove(windowHandle);

            if (!IsAppMatchesSystem() || backgroundEffect == BackgroundType.Unknown) return;

            // TODO: Improve
            if (Background.Apply(windowHandle, backgroundEffect))
                mainWindow.Background = Brushes.Transparent;
        }
    }
}