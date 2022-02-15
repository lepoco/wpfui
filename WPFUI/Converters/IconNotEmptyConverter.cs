// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Windows.Data;

namespace WPFUI.Converters
{
    /// <summary>
    /// Checks if the <see cref="Common.Icon"/> is valid and not empty.
    /// </summary>
    internal class IconNotEmptyConverter : IValueConverter
    {
        /// <summary>
        /// Checks if the <see cref="Common.Icon"/> is valid and not empty.
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is Common.Icon icon)
                return icon != Common.Icon.Empty;

            return false;
        }

        /// <summary>
        /// Not Implemented.
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}