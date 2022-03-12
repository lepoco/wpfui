// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;
using System.Windows.Media;
using WPFUI.Common;

namespace WPFUI.Appearance
{
    /// <summary>
    /// Lets you update the color accents of the application.
    /// </summary>
    public static class Accent
    {
        /// <summary>
        /// SystemAccentColor
        /// </summary>
        public static Color SystemAccent
        {
            get
            {
                var resource = Application.Current.Resources["SystemAccentColor"];

                if (resource is Color color)
                    return color;

                return Colors.Transparent;
            }
        }

        /// <summary>
        /// SystemAccentColorLight1.
        /// </summary>
        public static Color PrimaryAccent
        {
            get
            {
                var resource = Application.Current.Resources["SystemAccentColorLight1"];

                if (resource is Color color)
                    return color;

                return Colors.Transparent;
            }
        }

        /// <summary>
        /// SystemAccentColorLight2.
        /// </summary>
        public static Color SecondaryAccent
        {
            get
            {
                var resource = Application.Current.Resources["SystemAccentColorLight2"];

                if (resource is Color color)
                    return color;

                return Colors.Transparent;
            }
        }

        /// <summary>
        /// SystemAccentColorLight3.
        /// </summary>
        public static Color TertiaryAccent
        {
            get
            {
                var resource = Application.Current.Resources["SystemAccentColorLight3"];

                if (resource is Color color)
                    return color;

                return Colors.Transparent;
            }
        }

        /// <summary>
        /// Changes the color accents of the application based on the color entered.
        /// </summary>
        /// <param name="systemAccent">Primary accent color.</param>
        /// <param name="themeType">If <see cref="ThemeType.Dark"/>, the colors will be different.</param>
        /// <param name="systemGlassColor">If the color is taken from the Glass Color System, its brightness will be increased with the help of the operations on HSV space.</param>
        public static void Change(Color systemAccent, ThemeType themeType = ThemeType.Light,
            bool systemGlassColor = false)
        {
            if (systemGlassColor)
            {
                // WindowGlassColor is little darker than accent color
                systemAccent = systemAccent.UpdateBrightness(6f);
            }

            Color primaryAccent, secondaryAccent, tertiaryAccent;

            if (themeType == ThemeType.Dark)
            {
                primaryAccent = systemAccent.Update(9f, -15);
                secondaryAccent = systemAccent.Update(18f, -30);
                tertiaryAccent = systemAccent.Update(27f, -45);
            }
            else
            {
                primaryAccent = systemAccent.Update(-9f, -15);
                secondaryAccent = systemAccent.Update(-18f, -30);
                tertiaryAccent = systemAccent.Update(-27f, -45);
            }

            UpdateColorResources(
                systemAccent,
                primaryAccent,
                secondaryAccent,
                tertiaryAccent
            );
        }

        /// <summary>
        /// Changes the color accents of the application based on the entered colors.
        /// </summary>
        /// <param name="systemAccent">Primary color.</param>
        /// <param name="primaryAccent">Alternative light or dark color.</param>
        /// <param name="secondaryAccent">Second alternative light or dark color (most used).</param>
        /// <param name="tertiaryAccent">Third alternative light or dark color.</param>
        public static void Change(Color systemAccent, Color primaryAccent,
            Color secondaryAccent, Color tertiaryAccent)
        {
            UpdateColorResources(systemAccent, primaryAccent, secondaryAccent, tertiaryAccent);
        }

        /// <summary>
        /// Updates application resources.
        /// </summary>
        private static void UpdateColorResources(Color systemAccent, Color primaryAccent,
            Color secondaryAccent, Color tertiaryAccent)
        {
#if DEBUG
            System.Diagnostics.Debug.WriteLine("INFO | SystemAccentColor: " + systemAccent, "WPFUI.Accent");
            System.Diagnostics.Debug.WriteLine("INFO | SystemAccentColorLight1: " + primaryAccent, "WPFUI.Accent");
            System.Diagnostics.Debug.WriteLine("INFO | SystemAccentColorLight2: " + secondaryAccent, "WPFUI.Accent");
            System.Diagnostics.Debug.WriteLine("INFO | SystemAccentColorLight3: " + tertiaryAccent, "WPFUI.Accent");
#endif

            Application.Current.Resources["SystemAccentColor"] = systemAccent;
            Application.Current.Resources["SystemAccentColorLight1"] = primaryAccent;
            Application.Current.Resources["SystemAccentColorLight2"] = secondaryAccent;
            Application.Current.Resources["SystemAccentColorLight3"] = tertiaryAccent;

            Application.Current.Resources["SystemAccentBrush"] = secondaryAccent.ToBrush();
            Application.Current.Resources["SystemFillColorAttentionBrush"] = secondaryAccent.ToBrush();
            Application.Current.Resources["AccentTextFillColorPrimaryBrush"] = tertiaryAccent.ToBrush();
            Application.Current.Resources["AccentTextFillColorSecondaryBrush"] = tertiaryAccent.ToBrush();
            Application.Current.Resources["AccentTextFillColorTertiaryBrush"] = secondaryAccent.ToBrush();
            Application.Current.Resources["AccentFillColorSelectedTextBackgroundBrush"] = systemAccent.ToBrush();
            Application.Current.Resources["AccentFillColorDefaultBrush"] = secondaryAccent.ToBrush();

            Application.Current.Resources["AccentFillColorSecondaryBrush"] = secondaryAccent.ToBrush(0.9);
            Application.Current.Resources["AccentFillColorTertiaryBrush"] = secondaryAccent.ToBrush(0.8);
        }
    }
}