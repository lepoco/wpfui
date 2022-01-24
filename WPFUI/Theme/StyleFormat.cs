// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

namespace WPFUI.Theme
{
    /// <summary>
    /// Makes it easy to format the names of the theme styles.
    /// </summary>
    internal static class StyleFormat
    {
        /// <summary>
        /// Translates <see cref="Style"/> to <see langword="string"/>.
        /// </summary>
        /// <returns>Translated style, or '<c>Light</c>' as default.</returns>
        public static string GetName(Style style)
        {
            return style switch
            {
                Style.Dark => "Dark",
                Style.Glow => "Glow",
                Style.CapturedMotion => "CapturedMotion",
                Style.Sunrise => "Sunrise",
                Style.Flow => "Flow",
                _ => "Light"
            };
        }

        /// <summary>
        /// Translates <see cref="Style"/> to <see langword="string"/>.
        /// </summary>
        /// <returns>'<c>Dark</c>' or '<c>Light</c>'.</returns>
        public static string GetInternalName(Style style)
        {
            return style switch
            {
                Style.Dark => "Dark",
                Style.Glow => "Dark",
                Style.CapturedMotion => "Dark",
                Style.Sunrise => "Light",
                Style.Flow => "Light",
                _ => "Light"
            };
        }

        /// <summary>
        /// Translates <see langword="string"/> to <see cref="Style"/>.
        /// </summary>
        /// <returns>One of <see cref="Style"/>.</returns>
        public static Style GetStyle(string styleName)
        {
            styleName = styleName.ToLower().Trim();

            if (styleName.Contains("light"))
                return Style.Light;

            if (styleName.Contains("dark"))
                return Style.Dark;

            if (styleName.Contains("glow"))
                return Style.Glow;

            if (styleName.Contains("capturedmotion"))
                return Style.CapturedMotion;

            if (styleName.Contains("sunrise"))
                return Style.Sunrise;

            if (styleName.Contains("flow"))
                return Style.Flow;

            return Style.Unknown;
        }

        /// <summary>
        /// Translates <see langword="string"/> to <see cref="Style"/>.
        /// </summary>
        /// <returns><see cref="Style.Dark"/> or <see cref="Style.Light"/>.</returns>
        public static Style GetInternalStyle(string styleName)
        {
            styleName = styleName.ToLower().Trim();

            if (styleName.Contains("light"))
                return Style.Light;

            if (styleName.Contains("dark"))
                return Style.Dark;

            if (styleName.Contains("glow"))
                return Style.Dark;

            if (styleName.Contains("capturedmotion"))
                return Style.Dark;

            if (styleName.Contains("sunrise"))
                return Style.Light;

            if (styleName.Contains("flow"))
                return Style.Light;

            return Style.Light;
        }
    }
}
