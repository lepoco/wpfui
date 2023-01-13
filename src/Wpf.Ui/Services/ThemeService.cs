using System;
using System.Windows.Media;
using Wpf.Ui.Appearance;
using Wpf.Ui.Contracts;

namespace Wpf.Ui.Services;

/// <summary>
/// Lets you set the app theme.
/// </summary>
public partial class ThemeService : IThemeService
{
    /// <inheritdoc />
    public virtual ThemeType GetTheme()
        => Theme.GetAppTheme();

    /// <inheritdoc />
    public virtual SystemThemeType GetNativeSystemTheme()
        => Theme.GetSystemTheme();

    /// <inheritdoc />
    public virtual ThemeType GetSystemTheme()
    {
        var systemTheme = Theme.GetSystemTheme();

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
        if (Theme.GetAppTheme() == themeType)
            return false;

        Theme.Apply(themeType);

        return true;
    }

    /// <inheritdoc />
    public bool SetSystemAccent()
    {
        Accent.ApplySystemAccent();

        return true;
    }

    /// <inheritdoc />
    public bool SetAccent(Color accentColor)
    {
        Accent.Apply(accentColor);

        return true;
    }

    /// <inheritdoc />
    public bool SetAccent(SolidColorBrush accentSolidBrush)
    {
        var color = accentSolidBrush.Color;
        color.A = (byte)Math.Round(accentSolidBrush.Opacity * byte.MaxValue);

        Accent.Apply(color);

        return true;
    }
}
