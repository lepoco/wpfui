// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows.Markup;
using Wpf.Ui.Animations;
using Wpf.Ui.Controls;

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

    public ControlsDictionary()
    {
        Source = new Uri(DictionaryUri, UriKind.Absolute);
        TextBlockMetadata.Initialize();
    }

    /// <summary>
    /// Gets or sets the duration used by animations marked as <see cref="AnimationDuration.SlowDuration"/>.
    /// The default value is 333 ms. Set this before the dictionary is loaded to override the default.
    /// </summary>
    /// <example>
    /// <code lang="xml">
    /// &lt;ui:ControlsDictionary SlowDuration="0:0:0.9" /&gt;
    /// </code>
    /// </example>
    public TimeSpan SlowDuration
    {
        get => AnimationDurationsDictionary.Get(AnimationDuration.SlowDuration).TimeSpan;
        set => AnimationDurationsDictionary.Set(AnimationDuration.SlowDuration, value);
    }

    /// <summary>
    /// Gets or sets the duration used by animations marked as <see cref="AnimationDuration.NormalDuration"/>.
    /// The default value is 167 ms. Set this before the dictionary is loaded to override the default.
    /// </summary>
    /// <example>
    /// <code lang="xml">
    /// &lt;ui:ControlsDictionary SlowDuration="0:0:0.9" /&gt;
    /// </code>
    /// </example>
    public TimeSpan NormalDuration
    {
        get => AnimationDurationsDictionary.Get(AnimationDuration.NormalDuration).TimeSpan;
        set => AnimationDurationsDictionary.Set(AnimationDuration.NormalDuration, value);
    }

    /// <summary>
    /// Gets or sets the duration used by animations marked as <see cref="AnimationDuration.FastDuration"/>.
    /// The default value is 80 ms. Set this before the dictionary is loaded to override the default.
    /// </summary>
    /// <example>
    /// <code lang="xml">
    /// &lt;ui:ControlsDictionary SlowDuration="0:0:0.9" /&gt;
    /// </code>
    /// </example>
    public TimeSpan FastDuration
    {
        get => AnimationDurationsDictionary.Get(AnimationDuration.FastDuration).TimeSpan;
        set => AnimationDurationsDictionary.Set(AnimationDuration.FastDuration, value);
    }
}