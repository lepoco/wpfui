// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;

namespace WPFUI.Common
{
    /// <summary>
    /// Set of static methods to operate on <see cref="Icon"/> and <see cref="IconFilled"/>.
    /// </summary>
    public static class Glyph
    {
        /// <summary>
        /// If the icon is not found in some places, this one will be displayed.
        /// </summary>
        public const Icon DefaultIcon = Icon.Heart28;

        /// <summary>
        /// If the filled icon is not found in some places, this one will be displayed.
        /// </summary>
        public const IconFilled DefaultFilledIcon = IconFilled.Heart28;

        /// <summary>
        /// Finds icon based on name.
        /// </summary>
        /// <param name="name">Name of the icon.</param>
        public static Common.Icon Parse(string name)
        {
            if (String.IsNullOrEmpty(name))
                return DefaultIcon;

            return (Common.Icon)Enum.Parse(typeof(Common.Icon), name);
        }

        /// <summary>
        /// Finds icon based on name.
        /// </summary>
        /// <param name="name">Name of the icon.</param>
        public static Common.IconFilled ParseFilled(string name)
        {
            if (String.IsNullOrEmpty(name))
                return DefaultFilledIcon;

            return (Common.IconFilled)Enum.Parse(typeof(Common.IconFilled), name);
        }
    }
}
