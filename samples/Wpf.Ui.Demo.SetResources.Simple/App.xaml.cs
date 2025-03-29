// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;
using Wpf.Ui.Appearance;
using Wpf.Ui.Markup;

namespace Wpf.Ui.Demo.SetResources.Simple;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App
{
    public static readonly ThemesDictionary ThemesDictionary = new();
    public static readonly ControlsDictionary ControlsDictionary = new();

    public static void Apply(ApplicationTheme theme)
    {
        ThemesDictionary.Theme = theme;
    }

    public static void ApplyTheme(FrameworkElement element)
    {
        element.Resources.MergedDictionaries.Add(ThemesDictionary);
        element.Resources.MergedDictionaries.Add(ControlsDictionary);
    }
}