// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace WPFUI.Theme
{
    /// <summary>
    /// Allows to manage available color themes from the library.
    /// </summary>
    public class Manager
    {
        private const string LibraryNamespace = "wpfui;";

        private const string LibraryUri = "pack://application:,,,/WPFUI;component/Styles/Theme/";

        /// <summary>
        /// Gets the contents of the merged dictionaries in <see cref="Application.Resources"/> and verifies currently set theme.
        /// </summary>
        /// <returns>Currently set <see cref="Style"/></returns>
        public static Style Current
        {
            get
            {
                Style returnTheme;

                Collection<ResourceDictionary> applicationDictionaries = Application.Current.Resources.MergedDictionaries;

                if (applicationDictionaries.Count == 0)
                {
                    return Style.Unknown;
                }

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
        }

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
            string currentTheme = Registry.GetValue("HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Themes", "CurrentTheme", "aero.theme") as string;

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
                "HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Themes\\Personalize", "AppsUseLightTheme", 1);

            if (0 == appsUseLightTheme)
            {
                return Style.Dark;
            }

            int systemUsesLightTheme = (int)Registry.GetValue(
                "HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Themes\\Personalize", "SystemUsesLightTheme", 1);

            if (0 == systemUsesLightTheme)
            {
                return Style.Dark;
            }

            return Style.Light;
        }

        /// <summary>
        /// Changes the currently set <see cref="ResourceDictionary"/> with theme in assembly App.xaml.
        /// </summary>
        public static void Switch(Style theme)
        {
            Collection<ResourceDictionary> applicationDictionaries = Application.Current.Resources.MergedDictionaries;
            if (applicationDictionaries.Count == 0)
                return;

            string sourceUri;

            for (int i = 0; i < applicationDictionaries.Count; i++)
            {
                sourceUri = applicationDictionaries[i].Source.ToString().ToLower().Trim();

                if (sourceUri.Contains(LibraryNamespace) && sourceUri.Contains("theme"))
                {
                    applicationDictionaries[i] = new ResourceDictionary() { Source = new Uri(LibraryUri + GetThemeName(theme) + ".xaml", UriKind.Absolute) };

                    return;
                }

                for (int j = 0; j < applicationDictionaries[i].MergedDictionaries.Count; j++)
                {
                    sourceUri = applicationDictionaries[i].MergedDictionaries[j].Source.ToString().ToLower().Trim();

                    if (!sourceUri.Contains(LibraryNamespace) || !sourceUri.Contains("theme")) continue;
                    applicationDictionaries[i].MergedDictionaries[j] = new ResourceDictionary() { Source = new Uri(LibraryUri + GetThemeName(theme) + ".xaml", UriKind.Absolute) };

                    return;
                }
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
            else
            {
                return Style.Unknown;
            }
        }
    }
}
