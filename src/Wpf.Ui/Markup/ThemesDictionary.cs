// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Windows;
using System.Windows.Markup;
using Wpf.Ui.Appearance;

namespace Wpf.Ui.Markup;

/// <summary>
/// Provides a dictionary implementation that contains <c>WPF UI</c> theme resources used by components and other elements of a WPF application.
/// </summary>
/// <example>
/// <code lang="xml">
/// &lt;Application
///     xmlns:ui="http://schemas.lepo.co/wpfui/2022/xaml"&gt;
///     &lt;Application.Resources&gt;
///         &lt;ResourceDictionary&gt;
///             &lt;ResourceDictionary.MergedDictionaries&gt;
///                 &lt;ui:ThemesDictionary Theme = "Dark" /&gt;
///             &lt;/ResourceDictionary.MergedDictionaries&gt;
///         &lt;/ResourceDictionary&gt;
///     &lt;/Application.Resources&gt;
/// &lt;/Application&gt;
/// </code>
/// </example>
[Localizability(LocalizationCategory.Ignore)]
[Ambient]
[UsableDuringInitialization(true)]
public class ThemesDictionary : ResourceDictionary
{
    /// <summary>
    /// Sets the default application theme.
    /// </summary>
    public ApplicationTheme Theme
    {
        set => SetSourceBasedOnSelectedTheme(value);
    }

    private void SetSourceBasedOnSelectedTheme(ApplicationTheme? selectedApplicationTheme)
    {
        var themeName = selectedApplicationTheme switch
        {
            ApplicationTheme.Dark => "Dark",
            ApplicationTheme.HighContrast => "HighContrast",
            _ => "Light"
        };

        Source = new Uri(
            $"{ApplicationThemeManager.ThemesDictionaryPath}{themeName}.xaml",
            UriKind.Absolute
        );
    }
}
