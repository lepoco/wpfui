// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.
//
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
//
// NOTE: This date time helper assumes it is working in a Gregorian calendar
// If we ever support non Gregorian calendars this class would need to be redesigned

using System.Diagnostics;
using System.Windows.Controls;

namespace Wpf.Ui.Controls;

internal static class DateTimeHelper
{
    private static readonly System.Globalization.Calendar Cal = new GregorianCalendar();

    public static DateTime? AddDays(DateTime time, int days)
    {
        try
        {
            return Cal.AddDays(time, days);
        }
        catch (System.ArgumentException)
        {
            return null;
        }
    }

    public static DateTime? AddMonths(DateTime time, int months)
    {
        try
        {
            return Cal.AddMonths(time, months);
        }
        catch (System.ArgumentException)
        {
            return null;
        }
    }

    public static DateTime? AddYears(DateTime time, int years)
    {
        try
        {
            return Cal.AddYears(time, years);
        }
        catch (System.ArgumentException)
        {
            return null;
        }
    }

    public static DateTime? SetYear(DateTime date, int year)
    {
        return DateTimeHelper.AddYears(date, year - date.Year);
    }

    public static DateTime? SetYearMonth(DateTime date, DateTime yearMonth)
    {
        DateTime? target = SetYear(date, yearMonth.Year);
        if (target.HasValue)
        {
            target = DateTimeHelper.AddMonths(target.Value, yearMonth.Month - date.Month);
        }

        return target;
    }

    public static int CompareDays(DateTime dt1, DateTime dt2)
    {
        return DateTime.Compare(DiscardTime(dt1), DiscardTime(dt2));
    }

    public static int CompareYearMonth(DateTime dt1, DateTime dt2)
    {
        return ((dt1.Year - dt2.Year) * 12) + (dt1.Month - dt2.Month);
    }

    public static int DecadeOfDate(DateTime date)
    {
        return date.Year - (date.Year % 10);
    }

    public static DateTime DiscardDayTime(DateTime d)
    {
        return new DateTime(d.Year, d.Month, 1, 0, 0, 0);
    }

    public static DateTime DiscardTime(DateTime d)
    {
        return d.Date;
    }

    public static int EndOfDecade(DateTime date)
    {
        return DecadeOfDate(date) + 9;
    }

    public static DateTimeFormatInfo GetCurrentDateFormat()
    {
        return GetDateFormat(CultureInfo.CurrentCulture);
    }

    internal static DateTimeFormatInfo GetDateFormat(CultureInfo culture)
    {
        if (culture.Calendar is GregorianCalendar)
        {
            return culture.DateTimeFormat;
        }

        GregorianCalendar? foundCal = null;
        foreach (System.Globalization.Calendar cal in culture.OptionalCalendars)
        {
            if (cal is GregorianCalendar gregorianCalendar)
            {
                // Return the first Gregorian calendar with CalendarType == Localized
                // Otherwise return the first Gregorian calendar
                foundCal ??= gregorianCalendar;

                if (gregorianCalendar.CalendarType == GregorianCalendarTypes.Localized)
                {
                    foundCal = gregorianCalendar;
                    break;
                }
            }
        }

        DateTimeFormatInfo dtfi;
        if (foundCal == null)
        {
            // if there are no GregorianCalendars in the OptionalCalendars list, use the invariant dtfi
            dtfi = ((CultureInfo)CultureInfo.InvariantCulture.Clone()).DateTimeFormat;
            dtfi.Calendar = new GregorianCalendar();
        }
        else
        {
            dtfi = ((CultureInfo)culture.Clone()).DateTimeFormat;
            dtfi.Calendar = foundCal;
        }

        return dtfi;
    }

    // returns true if the date is included in the range
    public static bool InRange(DateTime date, CalendarDateRange range)
    {
        return InRange(date, range.Start, range.End);
    }

    // returns true if the date is included in the range
    public static bool InRange(DateTime date, DateTime start, DateTime end)
    {
        Debug.Assert(
            DateTime.Compare(start, end) < 1,
            "Start date must be less than or equal to the end date."
        );

        if (CompareDays(date, start) > -1 && CompareDays(date, end) < 1)
        {
            return true;
        }

        return false;
    }

    public static string ToDayString(DateTime? date, CultureInfo culture)
    {
        var result = string.Empty;
        DateTimeFormatInfo format = GetDateFormat(culture);

        if (date.HasValue && format is not null)
        {
            result = date.Value.Day.ToString(format);
        }

        return result;
    }

    public static string ToYearMonthPatternString(DateTime? date, CultureInfo culture)
    {
        var result = string.Empty;
        DateTimeFormatInfo format = GetDateFormat(culture);

        if (date.HasValue && format != null)
        {
            result = date.Value.ToString(format.YearMonthPattern, format);
        }

        return result;
    }

    public static string ToYearString(DateTime? date, CultureInfo culture)
    {
        var result = string.Empty;
        DateTimeFormatInfo format = GetDateFormat(culture);

        if (date.HasValue && format != null)
        {
            result = date.Value.Year.ToString(format);
        }

        return result;
    }

    public static string ToAbbreviatedMonthString(DateTime? date, CultureInfo culture)
    {
        var result = string.Empty;
        DateTimeFormatInfo format = GetDateFormat(culture);

        if (date.HasValue && format is not null)
        {
            string[] monthNames = format.AbbreviatedMonthNames;

            if (monthNames is not null && monthNames.Length > 0)
            {
                result = monthNames[(date.Value.Month - 1) % monthNames.Length];
            }
        }

        return result;
    }

    public static string ToLongDateString(DateTime? date, CultureInfo culture)
    {
        var result = string.Empty;
        DateTimeFormatInfo format = GetDateFormat(culture);

        if (date.HasValue && format is not null)
        {
            result = date.Value.Date.ToString(format.LongDatePattern, format);
        }

        return result;
    }
}
