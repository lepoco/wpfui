// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;

namespace Wpf.Ui.Extensions;

/// <summary>
/// A collection of several extensions to the <see cref="DateTime"/> class.
/// </summary>
public static class DateTimeExtensions
{
    /// <summary>
    /// Gets the number of seconds that have elapsed since the Unix epoch, excluding leap seconds. The Unix epoch is 00:00:00 UTC on 1 January 1970.
    /// </summary>
    public static long GetTimestamp(this DateTime dateTime)
    {
        return (long)dateTime.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
    }

    /// <summary>
    /// Gets the number of milliseconds that have elapsed since the Unix epoch, excluding leap seconds. The Unix epoch is 00:00:00 UTC on 1 January 1970.
    /// </summary>
    public static long GetMillisTimestamp(this DateTime dateTime)
    {
        // Should be 10^-3
        return (long)dateTime.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
    }

    /// <summary>
    /// Gets the number of microseconds that have elapsed since the Unix epoch, excluding leap seconds. The Unix epoch is 00:00:00 UTC on 1 January 1970.
    /// </summary>
    public static long GetMicroTimestamp(this DateTime dateTime)
    {
        // Should be 10^-6
        return (long)dateTime.Subtract(new DateTime(1970, 1, 1)).Ticks / (TimeSpan.TicksPerMillisecond / 1000);
    }
}
