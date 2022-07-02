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
/// Provides a dictionary implementation that contains <c>WPF UI</c> controls resources used by components and other elements of a WPF application.
/// </summary>
[Localizability(LocalizationCategory.Ignore)]
[Ambient]
[UsableDuringInitialization(true)]
public class ControlsDictionary : ResourceDictionary
{
    /// <summary>
    /// Default constructor defining <see cref="ResourceDictionary.Source"/> of the <c>WPF UI</c> controls dictionary.
    /// </summary>
    public ControlsDictionary()
        => Source = new Uri($"{AppearanceData.LibraryDictionariesUri}{AppearanceData.LibraryMainDictionary}.xaml", UriKind.Absolute);
}
