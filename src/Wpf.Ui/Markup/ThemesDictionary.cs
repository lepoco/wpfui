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
[Localizability(LocalizationCategory.Ignore)]
[Ambient]
[UsableDuringInitialization(true)]
public class ThemesDictionary : ResourceDictionary
{
    /// <summary>
    /// Sets the default application theme.
    /// </summary>
    public ThemeType Theme
    {
        set
        {
            var themeName = value switch
            {
                ThemeType.Dark => "Dark",
                ThemeType.HighContrast => "HighContrast",
                _ => "Light"
            };

            Source = new Uri($"{AppearanceData.LibraryThemeDictionariesUri}{themeName}.xaml", UriKind.Absolute);
        }
    }
}

