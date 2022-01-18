// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Windows.Media;

namespace WPFUI.Common
{
    /// <summary>
    /// Simple color manipulations.
    /// </summary>
    internal static class ColorManipulation
    {
        /// <summary>
        /// Changes the color brightness (HSV) and saturation by the entered amount.
        /// </summary>
        /// <param name="color">Color to change.</param>
        /// <param name="brightness">The value of the brightness change from <see langword="100"/> to <see langword="-100"/>.</param>
        /// <param name="saturation">The value of the saturation change from <see langword="100"/> to <see langword="-100"/>.</param>
        public static Color Change(Color color, float brightness, float saturation)
        {
            if (brightness > 100f || brightness < -100f)
            {
                throw new ArgumentOutOfRangeException(nameof(brightness));
            }

            if (saturation > 100f || saturation < -100f)
            {
                throw new ArgumentOutOfRangeException(nameof(saturation));
            }

            (float hue, float rawSaturation, float rawBrightness) = ToHsv(color.R, color.G, color.B);

            (int red, int green, int blue) = FromHsvToRgb(hue, ToPercentage(rawSaturation + saturation),
                ToPercentage(rawBrightness + brightness));

            return Color.FromArgb(
                color.A,
                ToColorByte(red),
                ToColorByte(green),
                ToColorByte(blue)
            );
        }

        /// <summary>
        /// Changes the color brightness (HSV) by the entered amount.
        /// </summary>
        /// <param name="color">Color to change.</param>
        /// <param name="brightness">The value of the brightness change from <see langword="100"/> to <see langword="-100"/>.</param>
        public static Color ChangeBrightness(Color color, float brightness)
        {
            if (brightness > 100f || brightness < -100f)
            {
                throw new ArgumentOutOfRangeException(nameof(brightness));
            }

            (float hue, float saturation, float rawBrightness) = ToHsv(color.R, color.G, color.B);

            (int red, int green, int blue) = FromHsvToRgb(hue, saturation, ToPercentage(rawBrightness + brightness));

            return Color.FromArgb(
                color.A,
                ToColorByte(red),
                ToColorByte(green),
                ToColorByte(blue)
            );
        }

        /// <summary>
        /// Changes the color luminance (HSL) by the entered amount.
        /// </summary>
        /// <param name="color">Color to change.</param>
        /// <param name="luminance">The value of the luminance change from <see langword="100"/> to <see langword="-100"/>.</param>
        public static Color ChangeLuminance(Color color, float luminance)
        {
            if (luminance > 100f || luminance < -100f)
            {
                throw new ArgumentOutOfRangeException(nameof(luminance));
            }

            (float hue, float saturation, float rawLuminance) = ToHsl(color.R, color.G, color.B);

            (int red, int green, int blue) = FromHslToRgb(hue, saturation, ToPercentage(rawLuminance + luminance));

            return Color.FromArgb(
                color.A,
                ToColorByte(red),
                ToColorByte(green),
                ToColorByte(blue)
            );
        }

        /// <summary>
        /// Changes the color saturation by the entered amount.
        /// </summary>
        /// <param name="color">Color to change.</param>
        /// <param name="saturation">The value of the saturation change from <see langword="100"/> to <see langword="-100"/>.</param>
        public static Color ChangeSaturation(Color color, float saturation)
        {
            if (saturation > 100f || saturation < -100f)
            {
                throw new ArgumentOutOfRangeException(nameof(saturation));
            }

            (float hue, float rawSaturation, float brightness) = ToHsl(color.R, color.G, color.B);


            (int red, int green, int blue) = FromHslToRgb(hue, ToPercentage(rawSaturation + saturation), brightness);

            return Color.FromArgb(
                color.A,
                ToColorByte(red),
                ToColorByte(green),
                ToColorByte(blue)
            );
        }

        /// <summary>
        /// HSL representation models the way different paints mix together to create colour in the real world,
        /// with the lightness dimension resembling the varying amounts of black or white paint in the mixture.
        /// </summary>
        /// <param name="red"></param>
        /// <param name="green"></param>
        /// <param name="blue"></param>
        /// <returns><see langword="float"/> hue, <see langword="float"/> saturation, <see langword="float"/> lightness</returns>
        public static (float, float, float) ToHsl(int red, int green, int blue)
        {
            float max = Math.Max(red, Math.Max(green, blue)) / (float)byte.MaxValue;
            float min = Math.Min(red, Math.Min(green, blue)) / (float)byte.MaxValue;

            float hue = 0f;
            float saturation = 0f;
            float lightness = (max + min) / 2;

            if (max != min)
            {
                if (max == red)
                    hue = (green / (float)byte.MaxValue - blue / (float)byte.MaxValue) / (max - min);
                else if (max == green)
                    hue = 2f + (blue / (float)byte.MaxValue - red / (float)byte.MaxValue) / (max - min);
                else
                    hue = 4f + (red / (float)byte.MaxValue - green / (float)byte.MaxValue) / (max - min);

                if (hue < 0)
                    hue += 360;

                if (lightness <= 0.5)
                    saturation = ((max - min) / (max + min));
                else
                    saturation = ((max - min) / (2f - max - min));
            }

            return (hue * 60f, saturation * 100f, lightness * 100f);
        }

        /// <summary>
        /// HSV representation models how colors appear under light.
        /// </summary>
        /// <returns><see langword="float"/> hue, <see langword="float"/> saturation, <see langword="float"/> brightness</returns>
        public static (float, float, float) ToHsv(int red, int green, int blue)
        {
            float max = Math.Max(red, Math.Max(green, blue)) / (float)byte.MaxValue;
            float min = Math.Min(red, Math.Min(green, blue)) / (float)byte.MaxValue;

            float hue = 0f;
            float saturation = 0f;
            float luminance = (max + min) / 2f;

            if (max != min)
            {
                if (max == red)
                    hue = (green / (float)byte.MaxValue - blue / (float)byte.MaxValue) / (max - min);
                else if (max == green)
                    hue = 2f + (blue / (float)byte.MaxValue - red / (float)byte.MaxValue) / (max - min);
                else
                    hue = 4f + (red / (float)byte.MaxValue - green / (float)byte.MaxValue) / (max - min);

                if (hue < 0)
                    hue += 360;

                if (luminance <= 0.5)
                    saturation = ((max - min) / (max + min));
                else
                    saturation = ((max - min) / (2f - max - min));
            }

            return (hue * 60f, saturation * 100f, max * 100f);
        }

        /// <summary>
        /// Converts the color values stored as HSL to RGB.
        /// </summary>
        public static (int, int, int) FromHslToRgb(float hue, float saturation, float lightness)
        {
            if (AlmostEquals(saturation, 0, 0.01f))
            {
                int color = (int)(lightness * byte.MaxValue);

                return (color, color, color);
            }

            lightness /= 100f;
            saturation /= 100f;

            float hueAngle = hue / 360f;

            return (
                CalcHslChannel(hueAngle + 0.333333333f, saturation, lightness),
                CalcHslChannel(hueAngle, saturation, lightness),
                CalcHslChannel(hueAngle - 0.333333333f, saturation, lightness)
            );
        }

        /// <summary>
        /// Converts the color values stored as HSV (HSB) to RGB.
        /// </summary>
        public static (int, int, int) FromHsvToRgb(float hue, float saturation, float brightness)
        {
            int red = 0, green = 0, blue = 0;

            if (AlmostEquals(saturation, 0, 0.01f))
            {
                red = green = blue = (int)(((brightness / 100f) * (float)byte.MaxValue) + 0.5f);

                return (red, green, blue);
            }

            hue /= 360f;
            brightness /= 100f;
            saturation /= 100f;

            float hueAngle = (hue - (float)Math.Floor(hue)) * 6.0f;
            float f = hueAngle - (float)Math.Floor(hueAngle);

            float p = brightness * (1.0f - saturation);
            float q = brightness * (1.0f - saturation * f);
            float t = brightness * (1.0f - (saturation * (1.0f - f)));

            switch ((int)hueAngle)
            {
                case 0:
                    red = (int)(brightness * 255.0f + 0.5f);
                    green = (int)(t * 255.0f + 0.5f);
                    blue = (int)(p * 255.0f + 0.5f);

                    break;
                case 1:
                    red = (int)(q * 255.0f + 0.5f);
                    green = (int)(brightness * 255.0f + 0.5f);
                    blue = (int)(p * 255.0f + 0.5f);

                    break;
                case 2:
                    red = (int)(p * 255.0f + 0.5f);
                    green = (int)(brightness * 255.0f + 0.5f);
                    blue = (int)(t * 255.0f + 0.5f);

                    break;
                case 3:
                    red = (int)(p * 255.0f + 0.5f);
                    green = (int)(q * 255.0f + 0.5f);
                    blue = (int)(brightness * 255.0f + 0.5f);

                    break;
                case 4:
                    red = (int)(t * 255.0f + 0.5f);
                    green = (int)(p * 255.0f + 0.5f);
                    blue = (int)(brightness * 255.0f + 0.5f);

                    break;
                case 5:
                    red = (int)(brightness * 255.0f + 0.5f);
                    green = (int)(p * 255.0f + 0.5f);
                    blue = (int)(q * 255.0f + 0.5f);

                    break;
            }

            return (red, green, blue);
        }

        /// <summary>
        /// Calculates the color component for HSL.
        /// </summary>
        private static int CalcHslChannel(float color, float saturation, float lightness)
        {
            float num1, num2;

            if (color > 1)
                color -= 1f;

            if (color < 0)
                color += 1f;

            if (lightness < 0.5f)
                num1 = lightness * (1f + saturation);
            else
                num1 = lightness + saturation - lightness * saturation;

            num2 = 2f * lightness - num1;

            if (color * 6f < 1)
                return (int)((num2 + (num1 - num2) * 6f * color) * (float)byte.MaxValue);

            if (color * 2f < 1)
                return (int)(num1 * (float)byte.MaxValue);

            if (color * 3f < 2)
                return (int)((num2 + (num1 - num2) * (0.666666666f - color) * 6f) * (float)byte.MaxValue);

            return (int)(num2 * (float)byte.MaxValue);
        }

        /// <summary>
        /// Whether the floating point number is about the same.
        /// </summary>
        private static bool AlmostEquals(float numberOne, float numberTwo, float precision = 0)
        {
            if (precision <= 0)
                precision = Single.Epsilon;

            return numberOne >= (numberTwo - precision) && numberOne <= (numberTwo + precision);
        }

        /// <summary>
        /// Absolute percentage.
        /// </summary>
        private static float ToPercentage(float value)
        {
            if (value > 100f)
                return 100f;


            if (value < 0f)
                return 0f;

            return value;
        }

        /// <summary>
        /// Absolute byte.
        /// </summary>
        private static byte ToColorByte(int value)
        {
            if (value > byte.MaxValue)
                value = byte.MaxValue;


            if (value < byte.MinValue)
                value = byte.MinValue;

            return Convert.ToByte(value);
        }
    }
}