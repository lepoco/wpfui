// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows.Markup;

namespace Wpf.Ui.Markup;

/// <summary>
/// Provides a dictionary implementation that contains <c>WPF UI</c> controls resources used by components and other elements of a WPF application.
/// </summary>
/// <example>
/// <code lang="xml">
/// &lt;Application
///     xmlns:ui="http://schemas.lepo.co/wpfui/2022/xaml"&gt;
///     &lt;Application.Resources&gt;
///         &lt;ResourceDictionary&gt;
///             &lt;ResourceDictionary.MergedDictionaries&gt;
///                 &lt;ui:ControlsDictionary /&gt;
///             &lt;/ResourceDictionary.MergedDictionaries&gt;
///         &lt;/ResourceDictionary&gt;
///     &lt;/Application.Resources&gt;
/// &lt;/Application&gt;
/// </code>
/// </example>
[Localizability(LocalizationCategory.Ignore)]
[Ambient]
[UsableDuringInitialization(true)]
public class ControlsDictionary : ResourceDictionary
{
    private const string DictionaryUri = "pack://application:,,,/Wpf.Ui;component/Resources/Wpf.Ui.xaml";

    /// <summary>
    /// Initializes a new instance of the <see cref="ControlsDictionary"/> class.
    /// Default constructor defining <see cref="ResourceDictionary.Source"/> of the <c>WPF UI</c> controls dictionary.
    /// </summary>
    public ControlsDictionary()
    {
        Source = new Uri(DictionaryUri, UriKind.Absolute);
    }
}
