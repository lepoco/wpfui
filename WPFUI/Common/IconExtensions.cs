using System;

namespace WPFUI.Common
{
    /// <summary>
    /// Set of extensions for the enumeration of icons to facilitate their management and replacement.
    /// </summary>
    public static class IconExtensions
    {
        /// <summary>
        /// Replaces <see cref="Icon"/> with <see cref="IconFilled"/>.
        /// </summary>
        public static IconFilled Swap(this Icon icon)
        {
            // TODO: It is possible that the alternative icon does not exist
            return Glyph.ParseFilled(icon.ToString());
        }

        /// <summary>
        /// Replaces <see cref="IconFilled"/> with <see cref="Icon"/>.
        /// </summary>
        public static Icon Swap(this IconFilled icon)
        {
            // TODO: It is possible that the alternative icon does not exist
            return Glyph.Parse(icon.ToString());
        }

        /// <summary>
        /// Converts <see cref="Icon"/> to <see langword="char"/> based on the ID.
        /// </summary>
        public static char GetGlyph(this Icon icon)
        {
            return ToChar(icon);
        }

        /// <summary>
        /// Converts <see cref="IconFilled"/> to <see langword="char"/> based on the ID.
        /// </summary>
        public static char GetGlyph(this IconFilled icon)
        {
            return ToChar(icon);
        }

        /// <summary>
        /// Converts <see cref="Icon"/> to <see langword="string"/> based on the ID.
        /// </summary>
        public static string GetString(this Common.Icon icon)
        {
            return icon.GetGlyph().ToString();
        }

        /// <summary>
        /// Converts <see cref="IconFilled"/> to <see langword="string"/> based on the ID.
        /// </summary>
        public static string GetString(this Common.IconFilled icon)
        {
            return icon.GetGlyph().ToString();
        }

        /// <summary>
        /// Converts <see cref="Icon"/> to <see langword="char"/>.
        /// </summary>
        private static char ToChar(Common.Icon? icon)
        {
            icon ??= Glyph.DefaultIcon;

            return Convert.ToChar(icon);
        }

        /// <summary>
        /// Converts <see cref="IconFilled"/> to <see langword="char"/>.
        /// </summary>
        private static char ToChar(Common.IconFilled? icon)
        {
            icon ??= Glyph.DefaultFilledIcon;

            return Convert.ToChar(icon);
        }
    }
}
