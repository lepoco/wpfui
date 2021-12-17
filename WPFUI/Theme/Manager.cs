// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;

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
        public static Style Current => GetAppTheme();

        /// <summary>
        /// Gets currently set system theme based on <see cref="Registry"/> value.
        /// </summary>
        /// <returns>Currently set system theme <see cref="Style"/>.</returns>
        public static Style System => GetSystemTheme();

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
        public static void SetSystemTheme()
        {
            Switch(GetSystemTheme());
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
        public static void Switch(Style theme)
        {
            Collection<ResourceDictionary> applicationDictionaries = Application.Current.Resources.MergedDictionaries;
            if (applicationDictionaries.Count == 0)
                return;

            if (Background.Mica.IsSupported() && Background.Mica.IsApplied)
            {
                Background.Mica.Remove();
            }

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

                        UpdateApplicationBackground();

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

                        UpdateApplicationBackground();

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
            ChangeAccentColor(SystemParameters.WindowGlassColor);
        }

        /// <summary>
        /// Forces change to application background. Required if Mica effect was previously applied.
        /// </summary>
        private static void UpdateApplicationBackground()
        {
            if (Application.Current.MainWindow == null)
            {
                return;
            }

            var backgroundColor = Application.Current.Resources["ApplicationBackgroundColor"];

            if (backgroundColor == null || backgroundColor.GetType() != typeof(Color))
            {
                return;
            }

            // TODO: If the application opens other windows and the theme is changed in the middle, the effect will not be applied and may cause visual glitches.

            Application.Current.MainWindow.Background = new SolidColorBrush((Color)backgroundColor);
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

        private static void ChangeAccentColor(Color accentColor)
        {
            Color accentColor2 = accentColor;
            Color accentColor3 = accentColor;

            switch (Current)
            {
                case Style.Dark:
                    accentColor2 = Color.Multiply(accentColor2, 2);
                    accentColor2 = Color.Multiply(accentColor2, 4);

                    break;

                case Style.Light:
                    accentColor2 = Color.Multiply(accentColor2, (float)0.6);
                    accentColor2 = Color.Multiply(accentColor2, (float)0.4);

                    break;

                case Style.Glow:
                    accentColor = Color.FromRgb(201, 146, 210);
                    accentColor2 = Color.FromRgb(219, 128, 229);
                    accentColor3 = Color.FromRgb(219, 128, 229);

                    break;

                case Style.CapturedMotion:
                    accentColor = Color.FromRgb(223, 119, 94);
                    accentColor2 = Color.FromRgb(240, 129, 102);
                    accentColor3 = Color.FromRgb(240, 129, 102);

                    break;

                case Style.Sunrise:
                    accentColor = Color.FromRgb(52, 117, 135);
                    accentColor2 = Color.FromRgb(32, 101, 123);
                    accentColor3 = Color.FromRgb(32, 101, 123);

                    break;

                case Style.Flow:
                    accentColor = Color.FromRgb(96, 108, 121);
                    accentColor2 = Color.FromRgb(76, 95, 107);
                    accentColor3 = Color.FromRgb(76, 95, 107);

                    break;
            }
#if DEBUG
            Debug.WriteLine("System accent color is: " + accentColor);
            Debug.WriteLine("System accentColor2 color is: " + accentColor2);
#endif

            Application.Current.Resources["SystemAccentColor"] = accentColor;
            Application.Current.Resources["SystemAccentColorLight2"] = accentColor2;
            Application.Current.Resources["SystemAccentColorLight3"] = accentColor3;
        }
    }
}