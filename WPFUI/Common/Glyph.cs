// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;

namespace WPFUI.Common
{
    public class Glyph
    {
        private const Icon DefaultIcon = Icon.Heart28;

        /// <summary>
        /// Converts <see cref="Icon"/> to <see langword="char"/> based on the ID, if <see langword="null"/> or error, returns <see cref="Glyph.Play16"/>
        /// </summary>
        public static char ToGlyph(Common.Icon? icon)
        {
            if (icon == null)
                return ToChar(DefaultIcon);

            return ToChar(icon);
        }

        /// <summary>
        /// Converts <see cref="Icon"/> to <see langword="string"/> based on the ID, if <see langword="null"/> or error, returns <see cref="Glyph.Play16"/>
        /// </summary>
        public static string ToString(Common.Icon? icon)
        {
            return Glyph.ToGlyph(icon).ToString();
        }

        public static Common.Icon Parse(string name)
        {
            return (Common.Icon) Enum.Parse(typeof(Common.Icon), name);
        }

        private static char ToChar(Common.Icon? icon)
        {
            if (icon == null)
                icon = DefaultIcon;

            return Convert.ToChar(icon);
        }
    }
}
