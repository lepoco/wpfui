// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using WPFUI.Background;
using WPFUI.Common;

namespace WPFUI.Theme
{
    /// <summary>
    /// Allows to manage available color themes from the library.
    /// </summary>
    public static class Manager
    {
        private const string LibraryNamespace = "wpfui;";

        private const string LibraryUri = "pack://application:,,,/WPFUI;component/Styles/Theme/";

        /// <summary>
        /// Gets the contents of the merged dictionaries in <see cref="Application.Resources"/> and verifies currently set theme.
        /// </summary>
        /// <returns>Currently set app theme <see cref="Style"/>.</returns>
        public static Style CurrentTheme => GetAppTheme();

        /// <summary>
        /// Gets currently set system theme based on <see cref="Registry"/> value.
        /// </summary>
        /// <returns>Currently set system theme <see cref="Style"/>.</returns>
        public static Style SystemTheme => GetSystemTheme();

        /// <summary>
        /// Indicates whether the application is in dark mode.
        /// </summary>
        public static bool IsDark => CurrentTheme == Style.Dark;

        /// <summary>
        /// Determines whether the system is currently set to hight contrast mode.
        /// </summary>
        /// <returns><see langword="true"/> if <see cref="SystemParameters.HighContrast"/>.</returns>
        public static bool IsHighContrast()
        {
            return SystemParameters.HighContrast;
        }

        /// <summary>
        /// Gets the current system theme and tries to set it as the application theme using <see cref="Manager.Switch"/>.
        /// </summary>
        public static void SetSystemTheme(bool useMica = false, bool updateAccent = true)
        {
            Switch(GetSystemTheme(), useMica, updateAccent);
        }

        /// <summary>
        /// Gets currently set system theme based on <see cref="Registry"/> value.
        /// </summary>
        public static Style GetSystemTheme()
        {
            string currentTheme =
                Registry.GetValue("HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Themes",
                    "CurrentTheme", "aero.theme") as string;

            if (string.IsNullOrEmpty(currentTheme))
            {
                return Style.Unknown;
            }

            currentTheme = currentTheme.ToLower().Trim();

            if (currentTheme.Contains("aero.theme"))
            {
                return Style.Light;
            }

            if (currentTheme.Contains("dark.theme"))
            {
                return Style.Dark;
            }

            if (currentTheme.Contains("themea.theme"))
            {
                return Style.Glow;
            }

            if (currentTheme.Contains("themeb.theme"))
            {
                return Style.CapturedMotion;
            }

            if (currentTheme.Contains("themec.theme"))
            {
                return Style.Sunrise;
            }

            if (currentTheme.Contains("themed.theme"))
            {
                return Style.Flow;
            }

            if (currentTheme.Contains("custom.theme"))
            {
                //eturn ColorTheme.Flow; custom can be light or dark
                //SystemParameters.WindowGlassBrush
            }

            int appsUseLightTheme = (int)Registry.GetValue(
                "HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Themes\\Personalize",
                "AppsUseLightTheme", 1);

            if (appsUseLightTheme == 0)
            {
                return Style.Dark;
            }

            int systemUsesLightTheme = (int)Registry.GetValue(
                "HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Themes\\Personalize",
                "SystemUsesLightTheme", 1);

            if (systemUsesLightTheme == 0)
            {
                return Style.Dark;
            }

            return Style.Light;
        }

        /// <summary>
        /// Gets currently set app theme based on <see cref="ResourceDictionary"/> value.
        /// </summary>
        public static Style GetAppTheme()
        {
            Collection<ResourceDictionary> applicationDictionaries =
                Application.Current.Resources.MergedDictionaries;

            if (applicationDictionaries.Count == 0)
            {
                return Style.Unknown;
            }

            Style returnTheme;

            for (int i = 0; i < applicationDictionaries.Count; i++)
            {
                returnTheme = CheckDictionarySource(applicationDictionaries[i]);

                if (returnTheme != Style.Unknown)
                    return returnTheme;

                for (int j = 0; j < applicationDictionaries[i].MergedDictionaries.Count; j++)
                {
                    returnTheme = CheckDictionarySource(applicationDictionaries[i].MergedDictionaries[j]);

                    if (returnTheme != Style.Unknown)
                        return returnTheme;
                }
            }

            return Style.Unknown;
        }

        /// <summary>
        /// Changes the currently set <see cref="ResourceDictionary"/> with theme in assembly App.xaml.
        /// </summary>
        public static void Switch(Style theme, bool useMica = false, bool updateAccent = false)
        {
            Collection<ResourceDictionary> applicationDictionaries = Application.Current.Resources.MergedDictionaries;
            if (applicationDictionaries.Count == 0)
                return;

            string sourceUri;

            for (int i = 0; i < applicationDictionaries.Count; i++)
            {
                if (applicationDictionaries[i].Source != null)
                {
                    sourceUri = applicationDictionaries[i].Source.ToString().ToLower().Trim();

                    if (sourceUri.Contains(LibraryNamespace) && sourceUri.Contains("theme"))
                    {
                        applicationDictionaries[i] = new ResourceDictionary()
                        { Source = new Uri(LibraryUri + GetThemeName(theme) + ".xaml", UriKind.Absolute) };

                        UpdateApplicationBackground(theme, useMica);


                        if (updateAccent)
                        {
                            SetSystemAccent();
                        }

                        return;
                    }
                }

                for (int j = 0; j < applicationDictionaries[i].MergedDictionaries.Count; j++)
                {
                    if (applicationDictionaries[i].MergedDictionaries[j].Source != null)
                    {
                        sourceUri = applicationDictionaries[i].MergedDictionaries[j].Source.ToString().ToLower().Trim();

                        if (!sourceUri.Contains(LibraryNamespace) || !sourceUri.Contains("theme"))
                        {
                            continue;
                        }

                        applicationDictionaries[i].MergedDictionaries[j] = new ResourceDictionary()
                        { Source = new Uri(LibraryUri + GetThemeName(theme) + ".xaml", UriKind.Absolute) };

                        UpdateApplicationBackground(theme, useMica);


                        if (updateAccent)
                        {
                            SetSystemAccent();
                        }

                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Changes the accent color in the application to those proposed by the system.
        /// </summary>
        public static void SetSystemAccent()
        {
            ChangeAccentColor(SystemParameters.WindowGlassColor, true);
        }


        /// <summary>
        /// Checks if the currently set system theme is compatible with the application's theme. If not, the backdrop should not be set as it causes strange behavior.
        /// </summary>
        /// <returns><see langword="true"/> if the system theme is similar to the app's theme.</returns>
        public static bool IsSystemThemeCompatible()
        {
            return IsMatchedDark() || IsMatchedLight();
        }

        /// <summary>
        /// Gets information about whether the application and the system have dark mode.
        /// </summary>
        public static bool IsMatchedDark()
        {
            Style appTheme = CurrentTheme;
            Style systemTheme = SystemTheme;

            return appTheme == Style.Dark && (systemTheme == Theme.Style.Dark || systemTheme == Theme.Style.Glow || systemTheme == Style.CapturedMotion);
        }

        /// <summary>
        /// Gets information about whether the application and the system have dark mode.
        /// </summary>
        public static bool IsMatchedLight()
        {
            Style appTheme = CurrentTheme;
            Style systemTheme = SystemTheme;

            return appTheme == Style.Light && (systemTheme == Style.Light || systemTheme == Style.Flow || systemTheme == Style.Sunrise);
        }

        /// <summary>
        /// Forces change to application background. Required if Mica effect was previously applied.
        /// </summary>
        private static void UpdateApplicationBackground(Style theme, bool useMica = true)
        {
            var mainWindow = Application.Current.MainWindow;

            if (mainWindow == null)
            {
                return;
            }

            // TODO: If the application opens other windows and the theme is changed in the middle, the effect will not be applied and may cause visual glitches.

            IntPtr mainWindowHandle = new WindowInteropHelper(mainWindow).Handle;

            WPFUI.Background.Manager.Remove(mainWindowHandle);

            var backgroundColor = Application.Current.Resources["ApplicationBackgroundColor"];

            if (backgroundColor != null)
            {
                mainWindow.Background = new SolidColorBrush((Color)backgroundColor);
            }

            if (theme == Style.Dark)
            {
                WPFUI.Background.Manager.ApplyDarkMode(mainWindowHandle);
            }
            else
            {
                WPFUI.Background.Manager.RemoveDarkMode(mainWindowHandle);
            }

            if (useMica && WPFUI.Background.Manager.IsSupported(BackgroundType.Mica) && IsSystemThemeCompatible())
            {
                mainWindow.Background = Brushes.Transparent;
                WPFUI.Background.Manager.Apply(WPFUI.Background.BackgroundType.Mica, mainWindowHandle);
            }
        }

        private static string GetThemeName(Style theme)
        {
            return theme switch
            {
                Style.Dark => "Dark",
                Style.Glow => "Dark",
                Style.CapturedMotion => "Dark",
                Style.Sunrise => "Light",
                Style.Flow => "Light",
                _ => "Light"
            };
        }

        private static Style GetThemeFromName(string themeName)
        {
            themeName = themeName.ToLower().Trim();

            if (themeName.Contains("light"))
            {
                return Style.Light;
            }

            if (themeName.Contains("dark"))
            {
                return Style.Dark;
            }

            if (themeName.Contains("glow"))
            {
                return Style.Dark;
            }

            if (themeName.Contains("capturedmotion"))
            {
                return Style.Dark;
            }

            if (themeName.Contains("sunrise"))
            {
                return Style.Light;
            }

            if (themeName.Contains("flow"))
            {
                return Style.Light;
            }

            return Style.Unknown;
        }

        private static Style CheckDictionarySource(ResourceDictionary dictionary)
        {
            string sourceUri = dictionary.Source.ToString().ToLower().Trim();

            if (sourceUri.Contains(LibraryNamespace) && sourceUri.Contains("theme"))
            {
                return GetThemeFromName(sourceUri);
            }

            return Style.Unknown;
        }

        private static void ChangeAccentColor(Color accentColor, bool system = false)
        {
            // TODO: Sometimes the colors disappear, see why

            if (system)
            {
#if DEBUG
                System.Diagnostics.Debug.WriteLine("Base SystemAccentColor: " + accentColor);
                System.Diagnostics.Debug.WriteLine("Increased SystemAccentColor: " + ColorManipulation.ChangeBrightness(accentColor, 6f));
#endif
                // WindowGlassColor is little darker than accent color
                accentColor = ColorManipulation.ChangeBrightness(accentColor, 6f);
            }

            Application.Current.Resources["SystemAccentColor"] = accentColor;

            if (IsDark)
            {
#if DEBUG
                System.Diagnostics.Debug.WriteLine("SystemAccentColorLight2: " + ColorManipulation.Change(accentColor, 18f, -30));
#endif
                Application.Current.Resources["SystemAccentColorLight1"] = ColorManipulation.Change(accentColor, 9f, -15);
                Application.Current.Resources["SystemAccentColorLight2"] = ColorManipulation.Change(accentColor, 18f, -30);
                Application.Current.Resources["SystemAccentColorLight3"] = ColorManipulation.Change(accentColor, 27f, -45);
            }
            else
            {
#if DEBUG
                System.Diagnostics.Debug.WriteLine("SystemAccentColorLight2: " + ColorManipulation.Change(accentColor, 40f, -16));
#endif
                Application.Current.Resources["SystemAccentColorLight1"] = ColorManipulation.Change(accentColor, -9f, -15);
                Application.Current.Resources["SystemAccentColorLight2"] = ColorManipulation.Change(accentColor, -18f, -30);
                Application.Current.Resources["SystemAccentColorLight3"] = ColorManipulation.Change(accentColor, -27f, -45);
            }
        }
    }
}