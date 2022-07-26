// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
//
// Code from https://github.com/microsoft/microsoft-ui-xaml/
//

using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using Colourful;

using SystemColor = System.Drawing.Color;

namespace Wpf.Ui.Common;

public static class ColorHelpers
{
    private static readonly CIEDE2000ColorDifference _colorDifference = new();
    private static readonly IColorConverter<RGBColor, LabColor> _rgbToLabConverter =
        new ConverterBuilder().FromRGB().ToLab(Illuminants.D65).Build();

    private static readonly (SystemColor Color, string Name)[] _namedColors =
    {
        (SystemColor.Aqua, "Aqua"), (SystemColor.Black, "Black"), (SystemColor.Blue, "Blue"),
        (SystemColor.FromArgb(102, 153, 204), "Blue gray"), (SystemColor.Brown, "Brown"),
        (SystemColor.DarkBlue, "Dark blue"), (SystemColor.DarkGray, "Dark gray"), (SystemColor.DarkGreen, "Dark green"),
        (SystemColor.FromArgb(48, 25, 52), "Dark purple"), (SystemColor.DarkRed, "Dark red"),
        (SystemColor.FromArgb(246, 190, 0), "Dark yellow"), (SystemColor.Gold, "Gold"), (SystemColor.Gray, "Gray"),
        (SystemColor.Green, "Green"), (SystemColor.Indigo, "Indigo"), (SystemColor.Lavender, "Lavender"),
        (SystemColor.LightBlue, "Light blue"), (SystemColor.LightGreen, "Light green"),
        (SystemColor.FromArgb(254, 216, 177), "Light orange"),
        (SystemColor.FromArgb(175, 238, 238), "Light turquoise"), (SystemColor.LightYellow, "Light yellow"),
        (SystemColor.Lime, "Lime"), (SystemColor.Olive, "Olive"), (SystemColor.Orange, "Orange"), (SystemColor.Pink, "Pink"),
        (SystemColor.Plum, "Plum"), (SystemColor.Red, "Red"), (SystemColor.FromArgb(255, 0, 128), "Rose"),
        (SystemColor.SkyBlue, "Sky blue"), (SystemColor.Teal, "Teal"), (SystemColor.Turquoise, "Turquoise"),
        (SystemColor.White, "White"), (SystemColor.Yellow, "Yellow"),
    };

    private static readonly (LabColor Color, string Name)[] _namedLabColors = Array.ConvertAll(_namedColors, t =>
        (_rgbToLabConverter.Convert(new RGBColor(t.Color)), t.Name));

    // A "kind" of replacement for the "GetColorName" method used in WinUI
    internal static (Color Color, string Name) GetNearestNamedColorTuple(Color color)
    {
        var rgbColor = new RGBColor(color.R / 255D, color.G / 255D, color.B / 255D);
        var labInputColor = _rgbToLabConverter.Convert(rgbColor);

        int nearestNamedColorIndex = 0;
        var nearestNamedColorTuple = _namedLabColors[0];
        var labNamedColor = nearestNamedColorTuple.Color;

        var nearestNamedColorDeltaE = _colorDifference.ComputeDifference(labInputColor, labNamedColor);
        for (int i = 1; i < _namedLabColors.Length; ++i)
        {
            labNamedColor = _namedLabColors[i].Color;
            var deltaE = _colorDifference.ComputeDifference(labInputColor, labNamedColor);

            if (deltaE < nearestNamedColorDeltaE)
            {
                nearestNamedColorDeltaE = deltaE;
                nearestNamedColorIndex = i;
            }
        }

        var nearestColorTuple = _namedColors[nearestNamedColorIndex];
        return (Color.FromRgb(nearestColorTuple.Color.R, nearestColorTuple.Color.G, nearestColorTuple.Color.B), nearestColorTuple.Name);
    }

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

    public static Color GetNearestNamedColor(Color color)
    {
        return GetNearestNamedColorTuple(color).Color;
    }

    public static string GetNearestNamedColorName(Color color)
    {
        return GetNearestNamedColorTuple(color).Name;
    }

    // Ported from Microsoft.UI.Xaml.Controls
    internal const int CheckerSize = 4;
    
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

    // TODO: Multiple calls to GetNearestNamedColorName, not so good
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
        string originalColorName = GetNearestNamedColorName(originalHsv.ToColor());
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
                //throw winrt::hresult_error(E_FAIL);
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
                    newColorName = GetNearestNamedColorName(newHsv.ToColor());
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
                    newColorName = GetNearestNamedColorName(newHsv.ToColor());
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

            newColorName = GetNearestNamedColorName(newHsv.ToColor());
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
                    //startValue = &(startHsv.h);
                    //currentValue = &(currentHsv.h);
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
                    //throw winrt::hresult_error(E_FAIL);
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

                currentColorName = GetNearestNamedColorName(currentHsv.ToColor());
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
