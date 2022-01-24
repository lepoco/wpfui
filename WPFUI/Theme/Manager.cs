// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
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
        /// Determines whether the system is currently set to hight contrast mode.
        /// </summary>
        /// <returns><see langword="true"/> if <see cref="SystemParameters.HighContrast"/>.</returns>
        public static bool IsHighContrast() => SystemTheme.IsHighContrast();

        /// <summary>
        /// Indicates whether the application is in dark mode.
        /// </summary>
        public static bool IsDark() => GetCurrentTheme() == Style.Dark;

        /// <summary>
        /// Gets currently set system theme based on <see cref="Microsoft.Win32.Registry"/> value.
        /// </summary>
        /// <returns>Currently set system theme <see cref="Style"/>.</returns>
        public static Style GetSystemTheme() => SystemTheme.GetTheme();

        /// <summary>
        /// Gets the contents of the merged dictionaries in <see cref="Application.Resources"/> and verifies currently set theme.
        /// </summary>
        /// <returns>Currently set app theme <see cref="Style"/>.</returns>
        public static Style GetCurrentTheme()
        {
            var appDictionaries = new AppDictionaryFinder(LibraryNamespace);
            var themeDictionary = appDictionaries.GetDictionary("theme");

            if (themeDictionary == null)
                return Style.Unknown;

            string sourceUri = themeDictionary.Source.ToString().ToLower().Trim();

            return StyleFormat.GetInternalStyle(sourceUri);
        }

        /// <summary>
        /// Gets the current system theme and tries to set it as the application theme using <see cref="Manager.Switch"/>.
        /// </summary>
        public static void SetSystemTheme(bool useMica = false, bool updateAccent = false)
        {
            Switch(SystemTheme.GetTheme(), useMica, updateAccent);
        }

        /// <summary>
        /// Changes the currently set <see cref="ResourceDictionary"/> with theme in assembly App.xaml.
        /// </summary>
        public static void Switch(Style theme, bool useMica = false, bool updateAccent = false)
        {
            var appDictionaries = new AppDictionaryFinder(LibraryNamespace);

            if (updateAccent)
                ChangeAccentColor(SystemTheme.GetColor(), theme, true);

            bool isUpdated = appDictionaries.UpdateDictionary("theme", new Uri(LibraryUri + StyleFormat.GetInternalName(theme) + ".xaml", UriKind.Absolute));

            if (!isUpdated) return;

            UpdateApplicationBackground(theme, useMica);
        }

        /// <summary>
        /// Checks if the currently set system theme is compatible with the application's theme. If not, the backdrop should not be set as it causes strange behavior.
        /// </summary>
        /// <returns><see langword="true"/> if the system theme is similar to the app's theme.</returns>
        public static bool IsSystemThemeCompatible()
        {
            var staticStyle = GetCurrentTheme();
            var variedStyle = SystemTheme.GetTheme();

            return IsMatchedDark(staticStyle, variedStyle) || IsMatchedLight(staticStyle, variedStyle);
        }

        /// <summary>
        /// Gets information about whether the provided style and the variable style have dark mode.
        /// </summary>
        public static bool IsMatchedDark(Style staticStyle = Style.Unknown, Style variedStyle = Style.Unknown)
        {
            if (staticStyle == Style.Unknown)
                staticStyle = GetCurrentTheme();

            if (variedStyle == Style.Unknown)
                variedStyle = SystemTheme.GetTheme();

            return staticStyle == Style.Dark && variedStyle is Style.Dark or Style.Glow or Style.CapturedMotion;
        }

        /// <summary>
        /// Gets information about whether the application and the system have dark mode.
        /// </summary>
        public static bool IsMatchedLight(Style staticStyle = Style.Unknown, Style variedStyle = Style.Unknown)
        {
            if (staticStyle == Style.Unknown)
                staticStyle = GetCurrentTheme();

            if (variedStyle == Style.Unknown)
                variedStyle = SystemTheme.GetTheme();

            return staticStyle == Style.Light && variedStyle is Style.Light or Style.Flow or Style.Sunrise;
        }

        /// <summary>
        /// Forces change to application background. Required if Mica effect was previously applied.
        /// </summary>
        private static void UpdateApplicationBackground(Style theme, bool useMica = false)
        {
            var mainWindow = Application.Current.MainWindow;

            if (mainWindow == null)
                return;

            IntPtr mainWindowHandle = new WindowInteropHelper(mainWindow).Handle;

            Background.Manager.Remove(mainWindowHandle);

            var backgroundColor = Application.Current.Resources["ApplicationBackgroundColor"];

            if (backgroundColor != null)
                mainWindow.Background = new SolidColorBrush((Color)backgroundColor);

            if (theme == Style.Dark)
                Background.Manager.ApplyDarkMode(mainWindowHandle);
            else
                Background.Manager.RemoveDarkMode(mainWindowHandle);

            if (useMica && Background.Manager.IsSupported(BackgroundType.Mica) && IsSystemThemeCompatible())
            {
                mainWindow.Background = Brushes.Transparent;
                Background.Manager.Apply(BackgroundType.Mica, mainWindowHandle);
            }
        }

        private static void ChangeAccentColor(Color accentColor, Style style, bool systemGlassColor = false)
        {
            if (systemGlassColor)
            {
                // WindowGlassColor is little darker than accent color
                accentColor = ColorManipulation.ChangeBrightness(accentColor, 6f);
            }

            Color accentColorOne, accentColorTwo, accentColorThree;

            if (style == Style.Dark)
            {
                accentColorOne = ColorManipulation.Change(accentColor, 9f, -15);
                accentColorTwo = ColorManipulation.Change(accentColor, 18f, -30);
                accentColorThree = ColorManipulation.Change(accentColor, 27f, -45);
            }
            else
            {
                accentColorOne = ColorManipulation.Change(accentColor, -9f, -15);
                accentColorTwo = ColorManipulation.Change(accentColor, -18f, -30);
                accentColorThree = ColorManipulation.Change(accentColor, -27f, -45);
            }

#if DEBUG
            System.Diagnostics.Debug.WriteLine("Style: " + style);
            System.Diagnostics.Debug.WriteLine("SystemAccentColor: " + accentColor);
            System.Diagnostics.Debug.WriteLine("SystemAccentColorLight1: " + accentColorOne);
            System.Diagnostics.Debug.WriteLine("SystemAccentColorLight2: " + accentColorTwo);
            System.Diagnostics.Debug.WriteLine("SystemAccentColorLight3: " + accentColorThree);
#endif

            Application.Current.Resources["SystemAccentColor"] = accentColor;
            Application.Current.Resources["SystemAccentColorLight1"] = accentColorOne;
            Application.Current.Resources["SystemAccentColorLight2"] = accentColorTwo;
            Application.Current.Resources["SystemAccentColorLight3"] = accentColorThree;

            Application.Current.Resources["SystemAccentBrush"] = new SolidColorBrush(accentColorTwo);
            Application.Current.Resources["SystemFillColorAttentionBrush"] = new SolidColorBrush(accentColorTwo);
            Application.Current.Resources["AccentTextFillColorPrimaryBrush"] = new SolidColorBrush(accentColorThree);
            Application.Current.Resources["AccentTextFillColorSecondaryBrush"] = new SolidColorBrush(accentColorThree);
            Application.Current.Resources["AccentTextFillColorTertiaryBrush"] = new SolidColorBrush(accentColorTwo);
            Application.Current.Resources["AccentFillColorSelectedTextBackgroundBrush"] = new SolidColorBrush(accentColor);
            Application.Current.Resources["AccentFillColorDefaultBrush"] = new SolidColorBrush(accentColorTwo);

            Application.Current.Resources["AccentFillColorSecondaryBrush"] = new SolidColorBrush { Color = accentColorTwo, Opacity = 0.9 };
            Application.Current.Resources["AccentFillColorTertiaryBrush"] = new SolidColorBrush { Color = accentColorTwo, Opacity = 0.8 };
        }
    }
}