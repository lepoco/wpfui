// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Wpf.Ui.Extensions;
using Wpf.Ui.Interop;

namespace Wpf.Ui.Appearance;

/// <summary>
/// Allows updating the accents used by controls in the application by swapping dynamic resources.
/// </summary>
/// <example>
/// <code lang="csharp">
/// ApplicationAccentColorManager.Apply(
///     Color.FromArgb(0xFF, 0xEE, 0x00, 0xBB),
///     ApplicationTheme.Dark,
///     false
/// );
/// </code>
/// <code lang="csharp">
/// ApplicationAccentColorManager.Apply(
///     ApplicationAccentColorManager.GetColorizationColor(),
///     ApplicationTheme.Dark,
///     false
/// );
/// </code>
/// </example>
public static class ApplicationAccentColorManager
{
    /// <summary>
    /// The maximum value of the background HSV brightness after which the text on the accent will be turned dark.
    /// </summary>
    private const double BackgroundBrightnessThresholdValue = 80d;

    /// <summary>
    /// Gets the SystemAccentColor.
    /// </summary>
    public static Color SystemAccent
    {
        get
        {
            var resource = UiApplication.Current.Resources["SystemAccentColor"];

            if (resource is Color color)
            {
                return color;
            }

            return Colors.Transparent;
        }
    }

    /// <summary>
    /// Gets the <see cref="Brush"/> of the SystemAccentColor.
    /// </summary>
    public static Brush SystemAccentBrush => new SolidColorBrush(SystemAccent);

    /// <summary>
    /// Gets the SystemAccentColorPrimary.
    /// </summary>
    public static Color PrimaryAccent
    {
        get
        {
            var resource = UiApplication.Current.Resources["SystemAccentColorPrimary"];

            if (resource is Color color)
            {
                return color;
            }

            return Colors.Transparent;
        }
    }

    /// <summary>
    /// Gets the <see cref="Brush"/> of the SystemAccentColorPrimary.
    /// </summary>
    public static Brush PrimaryAccentBrush => new SolidColorBrush(PrimaryAccent);

    /// <summary>
    /// Gets the SystemAccentColorSecondary.
    /// </summary>
    public static Color SecondaryAccent
    {
        get
        {
            var resource = UiApplication.Current.Resources["SystemAccentColorSecondary"];

            if (resource is Color color)
            {
                return color;
            }

            return Colors.Transparent;
        }
    }

    /// <summary>
    /// Gets the <see cref="Brush"/> of the SystemAccentColorSecondary.
    /// </summary>
    public static Brush SecondaryAccentBrush => new SolidColorBrush(SecondaryAccent);

    /// <summary>
    /// Gets the SystemAccentColorTertiary.
    /// </summary>
    public static Color TertiaryAccent
    {
        get
        {
            var resource = UiApplication.Current.Resources["SystemAccentColorTertiary"];

            if (resource is Color color)
            {
                return color;
            }

            return Colors.Transparent;
        }
    }

    /// <summary>
    /// Gets the <see cref="Brush"/> of the SystemAccentColorTertiary.
    /// </summary>
    public static Brush TertiaryAccentBrush => new SolidColorBrush(TertiaryAccent);

    /// <summary>
    /// Changes the color accents of the application based on the color entered.
    /// </summary>
    /// <param name="systemAccent">Primary accent color.</param>
    /// <param name="applicationTheme">If <see cref="ApplicationTheme.Dark"/>, the colors will be different.</param>
    /// <param name="systemGlassColor">If the color is taken from the Glass Color System, its brightness will be increased with the help of the operations on HSV space.</param>
    public static void Apply(
        Color systemAccent,
        ApplicationTheme applicationTheme = ApplicationTheme.Light,
        bool systemGlassColor = false
    )
    {
        if (systemGlassColor)
        {
            // WindowGlassColor is little darker than accent color
            systemAccent = systemAccent.UpdateBrightness(6f);
        }

        Color primaryAccent;
        Color secondaryAccent;
        Color tertiaryAccent;

        if (applicationTheme == ApplicationTheme.Dark)
        {
            primaryAccent = systemAccent.Update(15f, -12f);
            secondaryAccent = systemAccent.Update(30f, -24f);
            tertiaryAccent = systemAccent.Update(45f, -36f);
        }
        else
        {
            primaryAccent = systemAccent.UpdateBrightness(-5f);
            secondaryAccent = systemAccent.UpdateBrightness(-10f);
            tertiaryAccent = systemAccent.UpdateBrightness(-15f);
        }

        UpdateColorResources(systemAccent, primaryAccent, secondaryAccent, tertiaryAccent);
    }

    /// <summary>
    /// Changes the color accents of the application based on the entered colors.
    /// </summary>
    /// <param name="systemAccent">Primary color.</param>
    /// <param name="primaryAccent">Alternative light or dark color.</param>
    /// <param name="secondaryAccent">Second alternative light or dark color (most used).</param>
    /// <param name="tertiaryAccent">Third alternative light or dark color.</param>
    public static void Apply(
        Color systemAccent,
        Color primaryAccent,
        Color secondaryAccent,
        Color tertiaryAccent
    )
    {
        UpdateColorResources(systemAccent, primaryAccent, secondaryAccent, tertiaryAccent);
    }

    /// <summary>
    /// Applies system accent color to the application.
    /// </summary>
    public static void ApplySystemAccent()
    {
        Apply(GetColorizationColor(), ApplicationThemeManager.GetAppTheme());
    }

    /// <summary>
    /// Gets current Desktop Window Manager colorization color.
    /// <para>It should be the color defined in the system Personalization.</para>
    /// </summary>
    public static Color GetColorizationColor()
    {
        return UnsafeNativeMethods.GetDwmColor();
    }

    /// <summary>
    /// Updates application resources.
    /// </summary>
    private static void UpdateColorResources(
        Color systemAccent,
        Color primaryAccent,
        Color secondaryAccent,
        Color tertiaryAccent
    )
    {
#if DEBUG
        System.Diagnostics.Debug.WriteLine("INFO | SystemAccentColor: " + systemAccent, "Wpf.Ui.Accent");
        System.Diagnostics.Debug.WriteLine(
            "INFO | SystemAccentColorPrimary: " + primaryAccent,
            "Wpf.Ui.Accent"
        );
        System.Diagnostics.Debug.WriteLine(
            "INFO | SystemAccentColorSecondary: " + secondaryAccent,
            "Wpf.Ui.Accent"
        );
        System.Diagnostics.Debug.WriteLine(
            "INFO | SystemAccentColorTertiary: " + tertiaryAccent,
            "Wpf.Ui.Accent"
        );
#endif

        if (secondaryAccent.GetBrightness() > BackgroundBrightnessThresholdValue)
        {
#if DEBUG
            System.Diagnostics.Debug.WriteLine("INFO | Text on accent is DARK", "Wpf.Ui.Accent");
#endif
            UiApplication.Current.Resources["TextOnAccentFillColorPrimary"] = Color.FromArgb(
                0xFF,
                0x00,
                0x00,
                0x00
            );
            UiApplication.Current.Resources["TextOnAccentFillColorSecondary"] = Color.FromArgb(
                0x80,
                0x00,
                0x00,
                0x00
            );
            UiApplication.Current.Resources["TextOnAccentFillColorDisabled"] = Color.FromArgb(
                0x77,
                0x00,
                0x00,
                0x00
            );
            UiApplication.Current.Resources["TextOnAccentFillColorSelectedText"] = Color.FromArgb(
                0x00,
                0x00,
                0x00,
                0x00
            );
            UiApplication.Current.Resources["AccentTextFillColorDisabled"] = Color.FromArgb(
                0x5D,
                0x00,
                0x00,
                0x00
            );
        }
        else
        {
#if DEBUG
            System.Diagnostics.Debug.WriteLine("INFO | Text on accent is LIGHT", "Wpf.Ui.Accent");
#endif
            UiApplication.Current.Resources["TextOnAccentFillColorPrimary"] = Color.FromArgb(
                0xFF,
                0xFF,
                0xFF,
                0xFF
            );
            UiApplication.Current.Resources["TextOnAccentFillColorSecondary"] = Color.FromArgb(
                0x80,
                0xFF,
                0xFF,
                0xFF
            );
            UiApplication.Current.Resources["TextOnAccentFillColorDisabled"] = Color.FromArgb(
                0x87,
                0xFF,
                0xFF,
                0xFF
            );
            UiApplication.Current.Resources["TextOnAccentFillColorSelectedText"] = Color.FromArgb(
                0xFF,
                0xFF,
                0xFF,
                0xFF
            );
            UiApplication.Current.Resources["AccentTextFillColorDisabled"] = Color.FromArgb(
                0x5D,
                0xFF,
                0xFF,
                0xFF
            );
        }

        UiApplication.Current.Resources["SystemAccentColor"] = systemAccent;
        UiApplication.Current.Resources["SystemAccentColorPrimary"] = primaryAccent;
        UiApplication.Current.Resources["SystemAccentColorSecondary"] = secondaryAccent;
        UiApplication.Current.Resources["SystemAccentColorTertiary"] = tertiaryAccent;

        UiApplication.Current.Resources["SystemAccentBrush"] = secondaryAccent.ToBrush();
        UiApplication.Current.Resources["SystemFillColorAttentionBrush"] = secondaryAccent.ToBrush();
        UiApplication.Current.Resources["AccentTextFillColorPrimaryBrush"] = tertiaryAccent.ToBrush();
        UiApplication.Current.Resources["AccentTextFillColorSecondaryBrush"] = tertiaryAccent.ToBrush();
        UiApplication.Current.Resources["AccentTextFillColorTertiaryBrush"] = secondaryAccent.ToBrush();
        UiApplication.Current.Resources["AccentFillColorSelectedTextBackgroundBrush"] =
            systemAccent.ToBrush();
        UiApplication.Current.Resources["AccentFillColorDefaultBrush"] = secondaryAccent.ToBrush();

        UiApplication.Current.Resources["AccentFillColorSecondaryBrush"] = secondaryAccent.ToBrush(0.9);
        UiApplication.Current.Resources["AccentFillColorTertiaryBrush"] = secondaryAccent.ToBrush(0.8);
    }
}
