// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Windows;
using System.Windows.Media;
using Microsoft.Win32;

namespace Wpf.Ui.Appearance;

internal static class SystemTheme
{
    /// <summary>
    /// Gets the current main color of the system.
    /// </summary>
    /// <returns></returns>
    public static Color GlassColor => SystemParameters.WindowGlassColor;

    /// <summary>
    /// Determines whether the system is currently set to hight contrast mode.
    /// </summary>
    /// <returns><see langword="true"/> if <see cref="SystemParameters.HighContrast"/>.</returns>
    public static bool HighContrast => SystemParameters.HighContrast;

    /// <summary>
    /// Gets currently set system theme based on <see cref="Registry"/> value.
    /// </summary>
    public static SystemThemeType GetTheme()
    {
        var currentTheme =
            Registry.GetValue("HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Themes",
                "CurrentTheme", "aero.theme") as string ?? String.Empty;

        if (String.IsNullOrEmpty(currentTheme))
            return SystemThemeType.Unknown;

        currentTheme = currentTheme.ToLower().Trim();

        // This may be changed in the next versions, check the Insider previews

        if (currentTheme.Contains("basic.theme"))
            return SystemThemeType.Light;

        if (currentTheme.Contains("aero.theme"))
            return SystemThemeType.Light;

        if (currentTheme.Contains("dark.theme"))
            return SystemThemeType.Dark;

        if (currentTheme.Contains("themea.theme"))
            return SystemThemeType.Glow;

        if (currentTheme.Contains("themeb.theme"))
            return SystemThemeType.CapturedMotion;

        if (currentTheme.Contains("themec.theme"))
            return SystemThemeType.Sunrise;

        if (currentTheme.Contains("themed.theme"))
            return SystemThemeType.Flow;

        //if (currentTheme.Contains("custom.theme"))
        //    return ; custom can be light or dark

        var rawAppsUseLightTheme = Registry.GetValue(
        "HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Themes\\Personalize",
        "AppsUseLightTheme", 1) ?? 1;

        if (rawAppsUseLightTheme is int and 0)
            return SystemThemeType.Dark;

        var rawSystemUsesLightTheme = Registry.GetValue(
            "HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Themes\\Personalize",
            "SystemUsesLightTheme", 1) ?? 1;

        return rawSystemUsesLightTheme is int and 0 ? SystemThemeType.Dark : SystemThemeType.Light;
    }
}
