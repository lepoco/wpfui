// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using Microsoft.Win32;

namespace WPFUI
{
    /// <summary>
    /// Themes available for the WPF UI library
    /// </summary>
    public enum ColorTheme
    {
        Unknown,
        Light,
        Dark,
        Glow,
        CapturedMotion,
        Sunrise,
        Flow
    }

    public class Theme
    {
        private const string _libNamespace = "wpfui;";

        private static readonly string _wpfuiUri = "pack://application:,,,/WPFUI;component/Styles/Theme/";

        /// <summary>
        /// Gets the contents of the merged dictionaries in <see cref="Application.Resources"/> and verifies currently set theme.
        /// </summary>
        /// <returns>Currently set <see cref="ColorTheme"/></returns>
        public static ColorTheme Current
        {
            get
            {
                ColorTheme returnTheme;

                Collection<ResourceDictionary> applicationDictionaries = Application.Current.Resources.MergedDictionaries;
                if (applicationDictionaries.Count == 0)
                    return ColorTheme.Unknown;

                for (int i = 0; i < applicationDictionaries.Count; i++)
                {
                    returnTheme = CheckDicionarySource(applicationDictionaries[i]);

                    if (returnTheme != ColorTheme.Unknown)
                        return returnTheme;

                    if (applicationDictionaries[i].MergedDictionaries != null)
                    {
                        for (int j = 0; j < applicationDictionaries[i].MergedDictionaries.Count; j++)
                        {
                            returnTheme = CheckDicionarySource(applicationDictionaries[i].MergedDictionaries[j]);

                            if (returnTheme != ColorTheme.Unknown)
                                return returnTheme;
                        }
                    }
                }

                return ColorTheme.Unknown;
            }
        }

        public static void WatchSystemTheme()
        {
            new ThemeWatcher();
        }

        public static bool IsHighContrast()
        {
            return SystemParameters.HighContrast;
        }

        public static ColorTheme GetSystemTheme()
        {
            string currentTheme = (string) Registry.GetValue("HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Themes", "CurrentTheme", "aero.theme");

            currentTheme = currentTheme.ToLower().Trim();

            if (currentTheme.Contains("aero.theme"))
            {
                return ColorTheme.Light;
            }

            if (currentTheme.Contains("dark.theme"))
            {
                return ColorTheme.Dark;
            }

            if (currentTheme.Contains("themea.theme"))
            {
                return ColorTheme.Glow;
            }

            if (currentTheme.Contains("themeb.theme"))
            {
                return ColorTheme.CapturedMotion;
            }

            if (currentTheme.Contains("themec.theme"))
            {
                return ColorTheme.Sunrise;
            }

            if (currentTheme.Contains("themed.theme"))
            {
                return ColorTheme.Flow;
            }

            if (currentTheme.Contains("custom.theme"))
            {
                //eturn ColorTheme.Flow; custom can be light or dark
                //SystemParameters.WindowGlassBrush
            }

            int appsUseLightTheme = (int) Registry.GetValue(
                "HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Themes\\Personalize", "AppsUseLightTheme", 1);

            if (0 == appsUseLightTheme)
            {
                return ColorTheme.Dark;
            }

            int systemUsesLightTheme = (int) Registry.GetValue(
                "HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Themes\\Personalize", "SystemUsesLightTheme", 1);

            if (0 == systemUsesLightTheme)
            {
                return ColorTheme.Dark;
            }

            return ColorTheme.Light;
        }

        /// <summary>
        /// Changes the currently set <see cref="ColorTheme"/>, if one is set.
        /// </summary>
        public static void Switch(ColorTheme theme)
        {
            Collection<ResourceDictionary> applicationDictionaries = Application.Current.Resources.MergedDictionaries;
            if (applicationDictionaries.Count == 0)
                return;

            string sourceUri;

            for (int i = 0; i < applicationDictionaries.Count; i++)
            {
                sourceUri = applicationDictionaries[i].Source.ToString().ToLower().Trim();

                if (sourceUri.Contains(_libNamespace) && sourceUri.Contains("theme"))
                {
                    applicationDictionaries[i] = new ResourceDictionary() { Source = new Uri(_wpfuiUri + GetThemeName(theme) + ".xaml", UriKind.Absolute) };
                    return;
                }

                if (applicationDictionaries[i].MergedDictionaries != null)
                {
                    for (int j = 0; j < applicationDictionaries[i].MergedDictionaries.Count; j++)
                    {
                        sourceUri = applicationDictionaries[i].MergedDictionaries[j].Source.ToString().ToLower().Trim();

                        if (sourceUri.Contains(_libNamespace) && sourceUri.Contains("theme"))
                        {
                            applicationDictionaries[i].MergedDictionaries[j] = new ResourceDictionary() { Source = new Uri(_wpfuiUri + GetThemeName(theme) + ".xaml", UriKind.Absolute) };
                            return;
                        }
                    }
                }
            }
        }

        private static string GetThemeName(ColorTheme theme)
        {
            switch (theme)
            {
                case ColorTheme.Dark:
                    return "Dark";
                case ColorTheme.Glow:
                    return "Dark";
                case ColorTheme.CapturedMotion:
                    return "Dark";
                case ColorTheme.Sunrise:
                    return "Light";
                case ColorTheme.Flow:
                    return "Light";
            }

            return "Light";
        }

        private static ColorTheme GetThemeFromName(string themeName)
        {
            themeName = themeName.ToLower().Trim();

            if (themeName.Contains("light"))
            {
                return ColorTheme.Light;
            }
            
            if (themeName.Contains("dark"))
            {
                return ColorTheme.Dark;
            }

            if (themeName.Contains("glow"))
            {
                return ColorTheme.Dark;
            }

            if (themeName.Contains("capturedmotion"))
            {
                return ColorTheme.Dark;
            }

            if (themeName.Contains("sunrise"))
            {
                return ColorTheme.Light;
            }

            if (themeName.Contains("flow"))
            {
                return ColorTheme.Light;
            }


            return ColorTheme.Unknown;
        }

        private static ColorTheme CheckDicionarySource(ResourceDictionary dictionary)
        {
            string sourceUri = dictionary.Source.ToString().ToLower().Trim();

            if (sourceUri.Contains(_libNamespace) && sourceUri.Contains("theme"))
            {
                return GetThemeFromName(sourceUri);
            }
            else
            {
                return ColorTheme.Unknown; ;
            }
        }
    }
}
