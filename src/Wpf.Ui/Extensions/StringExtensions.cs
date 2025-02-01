// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

#if NETFRAMEWORK
using System.Diagnostics.Contracts;

namespace Wpf.Ui.Extensions;

internal static class StringExtensions
{
    /// <summary>
    ///     Returns a value indicating whether a specified string occurs within this string, using the specified comparison rules.
    /// </summary>
    /// <param name="source">Source string.</param>
    /// <param name="value">The string to seek.</param>
    /// <param name="comparison">One of the enumeration values that specifies the rules to use in the comparison.</param>
    /// <returns>true if the value parameter occurs within this string, or if value is the empty string (""); otherwise, false.</returns>
    [Pure]
    public static bool Contains(this string source, string value, StringComparison comparison)
    {
        return source.IndexOf(value, comparison) >= 0;
    }
}
#endif