using System;
using System.Windows.Media;
using WPFUI.Appearance;
using WPFUI.Mvvm.Contracts;

namespace WPFUI.Mvvm.Services;

/// <summary>
/// Lets you set the app theme.
/// </summary>
public partial class ThemeService : IThemeService
{
    /// <inheritdoc />
    public virtual ThemeType GetTheme()
        => WPFUI.Appearance.Theme.GetAppTheme();

    /// <inheritdoc />
    public virtual SystemThemeType GetNativeSystemTheme()
        => WPFUI.Appearance.Theme.GetSystemTheme();

    /// <inheritdoc />
    public virtual ThemeType GetSystemTheme()
    {
        var systemTheme = WPFUI.Appearance.Theme.GetSystemTheme();

        return systemTheme switch
        {
            SystemThemeType.Light => ThemeType.Light,
            SystemThemeType.Dark => ThemeType.Dark,
            SystemThemeType.Glow => ThemeType.Dark,
            SystemThemeType.CapturedMotion => ThemeType.Dark,
            SystemThemeType.Sunrise => ThemeType.Light,
            SystemThemeType.Flow => ThemeType.Light,
            _ => ThemeType.Unknown
        };
    }

    /// <inheritdoc />
    public virtual bool SetTheme(ThemeType themeType)
    {
        if (WPFUI.Appearance.Theme.GetAppTheme() == themeType)
            return false;

        WPFUI.Appearance.Theme.Apply(themeType);

        return true;
    }

    /// <inheritdoc />
    public bool SetAccent(Color accentColor)
    {
        WPFUI.Appearance.Accent.Apply(accentColor);

        return true;
    }

    /// <inheritdoc />
    public bool SetAccent(SolidColorBrush accentSolidBrush)
    {
        var color = accentSolidBrush.Color;
        color.A = (byte)Math.Round(accentSolidBrush.Opacity * Byte.MaxValue);

        WPFUI.Appearance.Accent.Apply(color);

        return true;
    }
}
