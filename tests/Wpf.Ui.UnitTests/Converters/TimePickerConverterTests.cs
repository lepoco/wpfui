// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Globalization;
using Wpf.Ui.Controls;
using Wpf.Ui.Converters;

namespace Wpf.Ui.UnitTests.Converters;

public sealed class TimePickerConverterTests
{
    private static readonly CultureInfo Culture = CultureInfo.InvariantCulture;

    [Fact]
    public void HourConverter_ReturnsMidnightAs12In12HourMode()
    {
        var converter = new TimeSpanToHourMultiConverter();

        var result = converter.Convert(
            new object[] { TimeSpan.Zero, ClockIdentifier.Clock12Hour },
            typeof(string),
            null!,
            Culture
        );

        Assert.Equal("12", result);
    }

    [Fact]
    public void HourConverter_ReturnsMidnightAs00In24HourMode()
    {
        var converter = new TimeSpanToHourMultiConverter();

        var result = converter.Convert(
            new object[] { TimeSpan.Zero, ClockIdentifier.Clock24Hour },
            typeof(string),
            null!,
            Culture
        );

        Assert.Equal("00", result);
    }

    [Fact]
    public void AmPmConverter_ReturnsAmForMidnight()
    {
        var converter = new TimeSpanToAmPmStringConverter();

        var result = converter.Convert(TimeSpan.Zero, typeof(string), null!, Culture);

        Assert.Equal("AM", result);
    }

    [Fact]
    public void MinuteConverter_PadsSingleDigitMinutes()
    {
        var converter = new TimeSpanToMinuteStringConverter();

        var result = converter.Convert(TimeSpan.FromMinutes(5), typeof(string), null!, Culture);

        Assert.Equal("05", result);
    }
}