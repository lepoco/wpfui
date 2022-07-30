// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using Colourful;

using Lepo.i18n;

namespace Wpf.Ui.Common;

public static class ColorHelpers
{
    private static readonly CIEDE2000ColorDifference _colorDifference = new();
    private static readonly IColorConverter<RGBColor, LabColor> _rgbToLabConverter =
        new ConverterBuilder().FromRGB().ToLab(Illuminants.D65).Build();

    internal const int CheckerSize = 4;

    private static readonly (LabColor Color, string Name)[] _namedColors =
    {
        (new LabColor(91.11, -48.09, -14.13), "aqua"), // System.Drawing.Color.Aqua
        (new LabColor(0, 0, 0), "black"), // System.Drawing.Color.Black
        (new LabColor(32.3, 79.19, -107.86), "blue"), // System.Drawing.Color.Blue
        (new LabColor(61.62, -2.82, -31.43), "blueGray"), // 102, 153, 204
        (new LabColor(89.08, -74.43, 84.82), "brightGreen"), // 102, 255, 0
        (new LabColor(37.53, 49.69, 30.54), "brown"), // System.Drawing.Color.Brown
        (new LabColor(67.3, 45.35, 47.49), "coral"), // System.Drawing.Color.Coral
        (new LabColor(14.75, 50.42, -68.68), "darkBlue"), // System.Drawing.Color.DarkBlue
        (new LabColor(69.24, 0, 0), "darkGray"), // System.Drawing.Color.DarkGray
        (new LabColor(36.2, -43.37, 41.86), "darkGreen"), // System.Drawing.Color.DarkGreen
        (new LabColor(13.06, 16.95, -13.11), "darkPurple"), // 48, 25, 52
        (new LabColor(28.09, 51, 41.29), "darkRed"),  // System.Drawing.Color.DarkRed
        (new LabColor(35.29, -22.66, -6.68), "darkTeal"), // 4, 93, 93
        (new LabColor(79.85, 7.05, 81.78), "darkYellow"), // 246, 190, 0
        (new LabColor(86.93, -1.92, 87.13), "gold"), // System.Drawing.Color.Gold
        (new LabColor(53.59, 0, 0), "gray"), // System.Drawing.Color.Gray
        (new LabColor(46.23, -51.7, 49.9), "green"), // System.Drawing.Color.Green
        (new LabColor(75.79, -4.75, -7.34), "iceBlue"), // 170, 190, 200
        (new LabColor(20.47, 51.69, -53.31), "indigo"), // System.Drawing.Color.Indigo
        (new LabColor(91.83, 3.71, -9.66), "lavender"), // System.Drawing.Color.Lavender
        (new LabColor(83.81, -10.89, -11.48), "lightBlue"), // System.Drawing.Color.LightBlue
        (new LabColor(84.56, 0, 0), "lightGray"), // System.Drawing.Color.LightGray
        (new LabColor(86.55, -46.33, 36.95), "lightGreen"), // System.Drawing.Color.LightGreen
        (new LabColor(88.62, 7.71, 24.17), "lightOrange"), // 254, 216, 177
        (new LabColor(90.06, -19.64, -6.4), "lightTurquoise"), // 175, 238, 238
        (new LabColor(99.29, -5.11, 14.84), "lightYellow"), // System.Drawing.Color.LightYellow
        (new LabColor(87.73, -86.18, 83.18), "lime"), // System.Drawing.Color.Lime
        (new LabColor(55.49, -17, 27.04), "oliveGreen"), // 120, 140, 85
        (new LabColor(74.94, 23.93, 78.95), "orange"), // System.Drawing.Color.Orange
        (new LabColor(59.2, 33.1, 63.46), "periwinkle"), // 128, 128, 255
        (new LabColor(83.59, 24.14, 3.33), "pink"), // System.Drawing.Color.Pink
        (new LabColor(73.37, 32.53, -21.99), "plum"), // System.Drawing.Color.Plum
        (new LabColor(29.78, 58.93, -36.49), "purple"), // System.Drawing.Color.Purple
        (new LabColor(53.24, 80.09, 67.2), "red"), // System.Drawing.Color.Red
        (new LabColor(54.89, 84.53, 4.08), "rose"), // 255, 0, 128
        (new LabColor(79.21, -14.84, -21.28), "skyBlue"), // System.Drawing.Color.SkyBlue
        (new LabColor(74.98, 5.02, 24.43), "tan"), // System.Drawing.Color.Tan
        (new LabColor(48.25, -28.85, -8.48), "teal"), // System.Drawing.Color.Teal
        (new LabColor(81.26, -44.08, -4.03), "turquoise"), // System.Drawing.Color.Turquoise
        (new LabColor(100, 0, 0), "white"), // System.Drawing.Color.White
        (new LabColor(97.14, -21.55, 94.48), "yellow"), // System.Drawing.Color.Yellow
    };

    public static bool TryGetColorFromHexCode(string hexCode, out Color result)
    {
        if (hexCode.StartsWith("#"))
        {
            hexCode = hexCode.Substring(1, hexCode.Length - 1);
        }

        hexCode = hexCode.PadLeft(6, '0');
        hexCode = hexCode.PadLeft(8, 'F');

        hexCode = '#' + hexCode;

        try
        {
            result = (Color)ColorConverter.ConvertFromString(hexCode);
        }
        catch
        {
            result = Colors.Transparent;
            return false;
        }

        return true;
    }

    // TODO: This is not fast enough for the color spectrum and color picker slider tooltips
    public static string GetColorDisplayName(Color color)
    {
        var rgbColor = new RGBColor(color.R / 255D, color.G / 255D, color.B / 255D);
        var labInputColor = _rgbToLabConverter.Convert(rgbColor);

        var nearestNamedColorTuple = _namedColors[0];
        var labNamedColor = nearestNamedColorTuple.Color;

        string nearestNamedColorName = nearestNamedColorTuple.Name;
        var nearestNamedColorDeltaE = _colorDifference.ComputeDifference(labInputColor, labNamedColor);
        for (int i = 1; i < _namedColors.Length; ++i)
        {
            labNamedColor = _namedColors[i].Color;
            var deltaE = _colorDifference.ComputeDifference(labInputColor, labNamedColor);

            if (deltaE < nearestNamedColorDeltaE)
            {
                nearestNamedColorDeltaE = deltaE;
                nearestNamedColorName = _namedColors[i].Name;
            }
        }

        return Translator.String("colorname." + nearestNamedColorName);
    }
    
    public static HsvColor IncrementColorChannel(HsvColor originalHsv, ColorPickerHsvChannel channel, IncrementDirection direction,
                                                 IncrementAmount amount, bool shouldWrap, double minBound, double maxBound)
    {
        HsvColor newHsv = originalHsv;

        if (amount == IncrementAmount.Small)
        {
            // In order to avoid working with small values that can incur rounding issues,
            // we'll multiple saturation and value by 100 to put them in the range of 0-100 instead of 0-1.
            newHsv.Saturation *= 100;
            newHsv.Value *= 100;

            ref double valueToIncrement = ref newHsv.Hue;
            double incrementAmount = 0;

            // If we're adding a small increment, then we'll just add or subtract 1.
            // If we're adding a large increment, then we want to snap to the next
            // or previous major value - for hue, this is every increment of 30;
            // for saturation and value, this is every increment of 10.
            switch (channel)
            {
                case ColorPickerHsvChannel.Hue:
                    //valueToIncrement = ref newHsv.Hue;
                    incrementAmount = amount == IncrementAmount.Small ? 1 : 30;
                    break;

                case ColorPickerHsvChannel.Saturation:
                    valueToIncrement = ref newHsv.Saturation;
                    incrementAmount = amount == IncrementAmount.Small ? 1 : 10;
                    break;

                case ColorPickerHsvChannel.Value:
                    valueToIncrement = newHsv.Value;
                    incrementAmount = amount == IncrementAmount.Small ? 1 : 10;
                    break;

                default:
                    throw new ArgumentException("Invalid ColorPickerHsvChannel value", nameof(channel));
            }

            double previousValue = valueToIncrement;

            valueToIncrement += (direction == IncrementDirection.Lower ? -incrementAmount : incrementAmount);

            // If the value has reached outside the bounds, we were previous at the boundary, and we should wrap,
            // then we'll place the selection on the other side of the spectrum.
            // Otherwise, we'll place it on the boundary that was exceeded.
            if (valueToIncrement < minBound)
            {
                valueToIncrement = (shouldWrap && previousValue == minBound) ? maxBound : minBound;
            }

            if (valueToIncrement > maxBound)
            {
                valueToIncrement = (shouldWrap && previousValue == maxBound) ? minBound : maxBound;
            }

            // We multiplied saturation and value by 100 previously, so now we want to put them back in the 0-1 range.
            newHsv.Saturation /= 100;
            newHsv.Value /= 100;
        }
        else
        {
            // While working with named colors, we're going to need to be working in actual HSV units,
            // so we'll divide the min bound and max bound by 100 in the case of saturation or value,
            // since we'll have received units between 0-100 and we need them within 0-1.
            if (channel == ColorPickerHsvChannel.Saturation || channel == ColorPickerHsvChannel.Value)
            {
                minBound /= 100;
                maxBound /= 100;
            }

            newHsv = FindNextNamedColor(originalHsv, channel, direction, shouldWrap, minBound, maxBound);
        }

        return newHsv;
    }

    public static HsvColor FindNextNamedColor(HsvColor originalHsv, ColorPickerHsvChannel channel, IncrementDirection direction,
                                              bool shouldWrap, double minBound, double maxBound)
    {
        // There's no easy way to directly get the next named color, so what we'll do
        // is just iterate in the direction that we want to find it until we find a color
        // in that direction that has a color name different than our current color name.
        // Once we find a new color name, then we'll iterate across that color name until
        // we find its bounds on the other side, and then select the color that is exactly
        // in the middle of that color's bounds.
        HsvColor newHsv = originalHsv;
        string originalColorName = GetColorDisplayName(originalHsv.ToColor());
        string newColorName = originalColorName;

        double originalValue = 0;
        ref double newValue = ref newHsv.Hue;
        double incrementAmount = 0;

        switch (channel)
        {
            case ColorPickerHsvChannel.Hue:
                originalValue = originalHsv.Hue;
                newValue = ref newHsv.Hue;
                incrementAmount = 1;
                break;

            case ColorPickerHsvChannel.Saturation:
                originalValue = originalHsv.Saturation;
                newValue = ref newHsv.Saturation;
                incrementAmount = 0.01;
                break;

            case ColorPickerHsvChannel.Value:
                originalValue = originalHsv.Value;
                newValue = ref newHsv.Value;
                incrementAmount = 0.01;
                break;

            default:
                throw new ArgumentException("Invalid ColorPickerHsvChannel value", nameof(channel));
        }

        bool shouldFindMidPoint = true;

        while (newColorName == originalColorName)
        {
            double previousValue = newValue;
            newValue += (direction == IncrementDirection.Lower ? -1 : 1) * incrementAmount;

            bool justWrapped = false;

            // If we've hit a boundary, then either we should wrap or we shouldn't.
            // If we should, then we'll perform that wrapping if we were previously up against
            // the boundary that we've now hit.  Otherwise, we'll stop at that boundary.
            if (newValue > maxBound)
            {
                if (shouldWrap)
                {
                    newValue = minBound;
                    justWrapped = true;
                }
                else
                {
                    newValue = maxBound;
                    shouldFindMidPoint = false;
                    newColorName = GetColorDisplayName(newHsv.ToColor());
                    break;
                }
            }
            else if (newValue < minBound)
            {
                if (shouldWrap)
                {
                    newValue = maxBound;
                    justWrapped = true;
                }
                else
                {
                    newValue = minBound;
                    shouldFindMidPoint = false;
                    newColorName = GetColorDisplayName(newHsv.ToColor());
                    break;
                }
            }

            if (!justWrapped && previousValue != originalValue &&
                Math.Sign(newValue - originalValue) != Math.Sign(previousValue - originalValue))
            {
                // If we've wrapped all the way back to the start and have failed to find a new color name,
                // then we'll just quit - there isn't a new color name that we're going to find.
                shouldFindMidPoint = false;
                break;
            }

            newColorName = GetColorDisplayName(newHsv.ToColor());
        }

        if (shouldFindMidPoint)
        {
            HsvColor startHsv = newHsv;
            HsvColor currentHsv = startHsv;
            double startEndOffset = 0;
            string currentColorName = newColorName;

            ref double startValue = ref startHsv.Hue;
            ref double currentValue = ref currentHsv.Hue;
            double wrapIncrement = 0;

            switch (channel)
            {
                case ColorPickerHsvChannel.Hue:
                    //startValue = ref startHsv.Hue;
                    //currentValue = ref currentHsv.Hue;
                    wrapIncrement = 360.0;
                    break;

                case ColorPickerHsvChannel.Saturation:
                    startValue = ref startHsv.Saturation;
                    currentValue = ref currentHsv.Saturation;
                    wrapIncrement = 1.0;
                    break;

                case ColorPickerHsvChannel.Value:
                    startValue = ref startHsv.Value;
                    currentValue = ref currentHsv.Value;
                    wrapIncrement = 1.0;
                    break;

                default:
                    throw new ArgumentException("Invalid ColorPickerHsvChannel value", nameof(channel));
            }

            while (newColorName == currentColorName)
            {
                currentValue += (direction == IncrementDirection.Lower ? -1 : 1) * incrementAmount;

                // If we've hit a boundary, then either we should wrap or we shouldn't.
                // If we should, then we'll perform that wrapping if we were previously up against
                // the boundary that we've now hit.  Otherwise, we'll stop at that boundary.
                if (currentValue > maxBound)
                {
                    if (shouldWrap)
                    {
                        currentValue = minBound;
                        startEndOffset = maxBound - minBound;
                    }
                    else
                    {
                        currentValue = maxBound;
                        break;
                    }
                }
                else if (currentValue < minBound)
                {
                    if (shouldWrap)
                    {
                        currentValue = maxBound;
                        startEndOffset = minBound - maxBound;
                    }
                    else
                    {
                        currentValue = minBound;
                        break;
                    }
                }

                currentColorName = GetColorDisplayName(currentHsv.ToColor());
            }

            newValue = (startValue + currentValue + startEndOffset) / 2;

            // Dividing by 2 may have gotten us halfway through a single step, so we'll
            // remove that half-step if it exists.
            double leftoverValue = Math.Abs(newValue);

            while (leftoverValue > incrementAmount)
            {
                leftoverValue -= incrementAmount;
            }

            newValue -= leftoverValue;

            while (newValue < minBound)
            {
                newValue += wrapIncrement;
            }

            while (newValue > maxBound)
            {
                newValue -= wrapIncrement;
            }
        }

        return newHsv;
    }

    public static double IncrementAlphaChannel(double originalAlpha, IncrementDirection direction, IncrementAmount amount,
                                               bool shouldWrap, double minBound, double maxBound)
    {
        // In order to avoid working with small values that can incur rounding issues,
        // we'll multiple alpha by 100 to put it in the range of 0-100 instead of 0-1.
        originalAlpha *= 100;

        const double smallIncrementAmount = 1;
        const double largeIncrementAmount = 10;

        if (amount == IncrementAmount.Small)
        {
            originalAlpha += (direction == IncrementDirection.Lower ? -1 : 1) * smallIncrementAmount;
        }
        else
        {
            if (direction == IncrementDirection.Lower)
            {
                originalAlpha = Math.Ceiling((originalAlpha - largeIncrementAmount) / largeIncrementAmount) * largeIncrementAmount;
            }
            else
            {
                originalAlpha = Math.Floor((originalAlpha + largeIncrementAmount) / largeIncrementAmount) * largeIncrementAmount;
            }
        }

        // If the value has reached outside the bounds and we should wrap, then we'll place the selection
        // on the other side of the spectrum.  Otherwise, we'll place it on the boundary that was exceeded.
        if (originalAlpha < minBound)
        {
            originalAlpha = shouldWrap ? maxBound : minBound;
        }

        if (originalAlpha > maxBound)
        {
            originalAlpha = shouldWrap ? minBound : maxBound;
        }

        // We multiplied alpha by 100 previously, so now we want to put it back in the 0-1 range.
        return originalAlpha / 100;
    }

    public static Task<byte[]> CreateCheckeredBackgroundAsync(int width, int height, Color checkerColor,
                                                              CancellationToken cancellationToken)
    {
        if (width == 0 || height == 0)
        {
            return Task.FromResult(Array.Empty<byte>());
        }

        var bgraCheckeredBackgroundPixelData = new byte[width * height * 4];
        int position = 0;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return Task.FromResult(bgraCheckeredBackgroundPixelData);
                }
                
                // We want the checkered pattern to alternate both vertically and horizontally.
                // In order to achieve that, we'll toggle visibility of the current pixel on or off
                // depending on both its x- and its y-position.  If x == CheckerSize, we'll turn visibility off,
                // but then if y == CheckerSize, we'll turn it back on.
                // The below is a shorthand for the above intent.
                bool pixelShouldBeBlank = (x / CheckerSize + y / CheckerSize) % 2 == 0;

                if (pixelShouldBeBlank)
                {
                    bgraCheckeredBackgroundPixelData[position++] = 0;
                    bgraCheckeredBackgroundPixelData[position++] = 0;
                    bgraCheckeredBackgroundPixelData[position++] = 0;
                    bgraCheckeredBackgroundPixelData[position++] = 0;
                }
                else
                {
                    bgraCheckeredBackgroundPixelData[position++] = (byte)(checkerColor.B * checkerColor.A / 255);
                    bgraCheckeredBackgroundPixelData[position++] = (byte)(checkerColor.G * checkerColor.A / 255);
                    bgraCheckeredBackgroundPixelData[position++] = (byte)(checkerColor.R * checkerColor.A / 255);
                    bgraCheckeredBackgroundPixelData[position++] = checkerColor.A;
                }
            }
        }

        return Task.FromResult(bgraCheckeredBackgroundPixelData);
    }

    public static WriteableBitmap CreateBitmapFromPixelData(int pixelWidth, int pixelHeight, byte[] bgraPixelData)
    {
        var bitmap = new WriteableBitmap(pixelWidth, pixelHeight, 96, 96, PixelFormats.Bgra32, null);
        bitmap.WritePixels(new Int32Rect(0, 0, pixelWidth, pixelHeight), bgraPixelData,
                           pixelWidth * 4, 0);
        // We are working with BGRA, so it's four "channels" per pixel
        bitmap.Freeze();

        return bitmap;
    }
}
