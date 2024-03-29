// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Microsoft.Win32;

namespace Wpf.Ui.Appearance;

/// <summary>
/// Provides information about Windows system themes.
/// </summary>
/// <example>
/// <code lang="csharp">
/// var currentWindowTheme = SystemThemeManager.GetCachedSystemTheme();
/// </code>
/// <code lang="csharp">
/// SystemThemeManager.UpdateSystemThemeCache();
/// var currentWindowTheme = SystemThemeManager.GetCachedSystemTheme();
/// </code>
/// </example>
public static class SystemThemeManager
{
    private static SystemTheme _cachedTheme = SystemTheme.Unknown;

    /// <summary>
    /// Gets the Windows glass color.
    /// </summary>
    public static Color GlassColor => SystemParameters.WindowGlassColor;

    /// <summary>
    /// Gets a value indicating whether the system is currently using the high contrast theme.
    /// </summary>
    public static bool HighContrast => SystemParameters.HighContrast;

    /// <summary>
    /// Returns the Windows theme retrieved from the registry. If it has not been cached before, invokes the <see cref="UpdateSystemThemeCache"/> and then returns the currently obtained theme.
    /// </summary>
    /// <returns>Currently cached Windows theme.</returns>
    public static SystemTheme GetCachedSystemTheme()
    {
        if (_cachedTheme != SystemTheme.Unknown)
        {
            return _cachedTheme;
        }

        UpdateSystemThemeCache();

        return _cachedTheme;
    }

    /// <summary>
    /// Refreshes the currently saved system theme.
    /// </summary>
    public static void UpdateSystemThemeCache()
    {
        _cachedTheme = GetCurrentSystemTheme();
    }

    private static SystemTheme GetCurrentSystemTheme()
    {
        var currentTheme =
            Registry.GetValue(
                "HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Themes",
                "CurrentTheme",
                "aero.theme"
            ) as string
            ?? String.Empty;

        if (!String.IsNullOrEmpty(currentTheme))
        {
            currentTheme = currentTheme.ToLower().Trim();

            // This may be changed in the next versions, check the Insider previews
            if (currentTheme.Contains("basic.theme"))
            {
                return SystemTheme.Light;
            }

            if (currentTheme.Contains("aero.theme"))
            {
                return SystemTheme.Light;
            }

            if (currentTheme.Contains("dark.theme"))
            {
                return SystemTheme.Dark;
            }

            if (currentTheme.Contains("hcblack.theme"))
            {
                return SystemTheme.HCBlack;
            }

            if (currentTheme.Contains("hcwhite.theme"))
            {
                return SystemTheme.HCWhite;
            }

            if (currentTheme.Contains("hc1.theme"))
            {
                return SystemTheme.HC1;
            }

            if (currentTheme.Contains("hc2.theme"))
            {
                return SystemTheme.HC2;
            }

            if (currentTheme.Contains("themea.theme"))
            {
                return SystemTheme.Glow;
            }

            if (currentTheme.Contains("themeb.theme"))
            {
                return SystemTheme.CapturedMotion;
            }

            if (currentTheme.Contains("themec.theme"))
            {
                return SystemTheme.Sunrise;
            }

            if (currentTheme.Contains("themed.theme"))
            {
                return SystemTheme.Flow;
            }
        }

        /*if (currentTheme.Contains("custom.theme"))
            return ; custom can be light or dark*/
        var rawAppsUseLightTheme = Registry.GetValue(
            "HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Themes\\Personalize",
            "AppsUseLightTheme",
            1
        );

        if (rawAppsUseLightTheme is 0)
        {
            return SystemTheme.Dark;
        }
        else if (rawAppsUseLightTheme is 1)
        {
            return SystemTheme.Light;
        }

        var rawSystemUsesLightTheme =
            Registry.GetValue(
                "HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Themes\\Personalize",
                "SystemUsesLightTheme",
                1
            ) ?? 1;

        return rawSystemUsesLightTheme is 0 ? SystemTheme.Dark : SystemTheme.Light;
    }
}
