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
                @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Themes",
                "CurrentTheme",
                "aero.theme"
            ) as string
            ?? string.Empty;

        var appUsesLightTheme = Registry.GetValue(
            @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Themes\Personalize",
            "AppsUseLightTheme",
            1
        );

        if (!string.IsNullOrEmpty(currentTheme))
        {
            currentTheme = currentTheme.ToLower().Trim();

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
        }

        return appUsesLightTheme is 1 ? SystemTheme.Light : SystemTheme.Dark;
    }
}