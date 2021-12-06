// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;

namespace WPFUI.Common
{
    /// <summary>
    /// Represents a set of static methods to operate on <see cref="Icon"/> and <see cref="IconFilled"/>.
    /// </summary>
    public class Glyph
    {
        private static readonly Icon _defaultIcon = Icon.Heart28;

        private static readonly IconFilled _defaultFilledIcon = IconFilled.Heart28;

        /// <summary>
        /// Replaces <see cref="Icon"/> with <see cref="IconFilled"/>.
        /// </summary>
        /// <param name="icon"></param>
        /// <returns></returns>
        public static IconFilled Swap(Icon icon)
        {
            // TODO: It is possible that the alternative icon does not exist
            return ParseFilled(icon.ToString());
        }

        /// <summary>
        /// Replaces <see cref="IconFilled"/> with <see cref="Icon"/>.
        /// </summary>
        /// <param name="icon"></param>
        /// <returns></returns>
        public static Icon Swap(IconFilled icon)
        {
            // TODO: It is possible that the alternative icon does not exist
            return Parse(icon.ToString());
        }

        /// <summary>
        /// Converts <see cref="Icon"/> to <see langword="char"/> based on the ID.
        /// </summary>
        public static char ToGlyph(Common.Icon? icon)
        {
            if (icon == null)
                return ToChar(_defaultIcon);

            return ToChar(icon);
        }

        /// <summary>
        /// Converts <see cref="IconFilled"/> to <see langword="char"/> based on the ID.
        /// </summary>
        public static char ToGlyph(Common.IconFilled? icon)
        {
            if (icon == null)
                return ToChar(_defaultFilledIcon);

            return ToChar(icon);
        }

        /// <summary>
        /// Converts <see cref="Icon"/> to <see langword="string"/> based on the ID.
        /// </summary>
        public static string ToString(Common.Icon? icon)
        {
            return Glyph.ToGlyph(icon).ToString();
        }

        /// <summary>
        /// Converts <see cref="IconFilled"/> to <see langword="string"/> based on the ID.
        /// </summary>
        public static string ToString(Common.IconFilled? icon)
        {
            return Glyph.ToGlyph(icon).ToString();
        }

        /// <summary>
        /// Finds icon based on name.
        /// </summary>
        /// <param name="name">Name of the icon.</param>
        /// <returns></returns>
        public static Common.Icon Parse(string name)
        {
            if (String.IsNullOrEmpty(name))
            {
                return _defaultIcon;
            }

            return (Common.Icon)Enum.Parse(typeof(Common.Icon), name);
        }

        /// <summary>
        /// Finds icon based on name.
        /// </summary>
        /// <param name="name">Name of the icon.</param>
        /// <returns></returns>
        public static Common.IconFilled ParseFilled(string name)
        {
            if (String.IsNullOrEmpty(name))
            {
                return _defaultFilledIcon;
            }

            return (Common.IconFilled)Enum.Parse(typeof(Common.IconFilled), name);
        }

        private static char ToChar(Common.Icon? icon)
        {
            if (icon == null)
            {
                icon = _defaultIcon;
            }

            return Convert.ToChar(icon);
        }

        private static char ToChar(Common.IconFilled? icon)
        {
            if (icon == null)
            {
                icon = _defaultFilledIcon;
            }

            return Convert.ToChar(icon);
        }
    }
}