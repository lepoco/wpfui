// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows.Markup;

namespace Wpf.Ui.Animations;

/// <summary>
/// A XAML markup extension that resolves an <see cref="AnimationDuration"/> key
/// to a concrete <see cref="Duration"/> value at parse time.
/// This allows Storyboard animations to reference configurable durations
/// without requiring <c>DynamicResource</c> (which cannot be frozen).
/// </summary>
/// <example>
/// <code lang="xml">
/// &lt;!-- Positional syntax --&gt;
/// Duration="{markup:AnimationDuration Slow}"
///
/// &lt;!-- Named property syntax --&gt;
/// Duration="{markup:AnimationDuration Duration=Slow}"
/// </code>
/// </example>
public class AnimationDurationExtension : MarkupExtension
{
    /// <summary>
    /// Gets or sets the <see cref="AnimationDuration"/> key to resolve.
    /// </summary>
    public AnimationDuration Duration { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="AnimationDurationExtension"/> class.
    /// </summary>
    public AnimationDurationExtension() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="AnimationDurationExtension"/> class.
    /// </summary>
    public AnimationDurationExtension(AnimationDuration animationDuration)
    {
        Duration = animationDuration;
    }

    /// <summary>
    /// Returns the <see cref="System.Windows.Duration"/> associated with the current
    /// <see cref="Duration"/> key, including any user-defined override.
    /// </summary>
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return AnimationDurationsDictionary.Get(Duration);
    }
}

/// <summary>
/// Defines the speed categories available for control animations.
/// Each key maps to a <see cref="Duration"/> value held in
/// <see cref="AnimationDurationsDictionary"/>.
/// </summary>
public enum AnimationDuration
{
    /// <summary>
    /// A longer duration intended for prominent or decorative transitions.
    /// </summary>
    SlowDuration,

    /// <summary>
    /// The standard duration for most interactive feedback animations.
    /// </summary>
    NormalDuration,

    /// <summary>
    /// A shorter duration for rapid state changes.
    /// </summary>
    FastDuration,
}

/// <summary>
/// A static registry that maps each <see cref="AnimationDuration"/> key to a
/// <see cref="Duration"/> value. Defaults are provided out of the box and can be
/// overridden at application startup via <see cref="Set"/> or through the
/// corresponding properties on <see cref="ControlsDictionary"/>
/// (e.g. <c>&lt;ui:ControlsDictionary SlowDuration="0:0:0.9" /&gt;</c>).
/// Overrides must be applied before the control dictionaries are loaded,
/// because <see cref="AnimationDurationExtension"/> resolves values at XAML parse time.
/// </summary>
public static class AnimationDurationsDictionary
{
    private static readonly Dictionary<AnimationDuration, Duration> Defaults = new()
    {
        [AnimationDuration.SlowDuration] = new Duration(TimeSpan.FromMilliseconds(333)),
        [AnimationDuration.NormalDuration] = new Duration(TimeSpan.FromMilliseconds(167)),
        [AnimationDuration.FastDuration] = new Duration(TimeSpan.FromMilliseconds(80)),
    };

    private static readonly Dictionary<AnimationDuration, Duration> Overrides = [];

    /// <summary>
    /// Returns the <see cref="Duration"/> for the specified <paramref name="key"/>,
    /// using the user-defined override if one exists, otherwise the built-in default.
    /// </summary>
    /// <param name="key">The animation speed category to look up.</param>
    public static Duration Get(AnimationDuration key)
    {
        return Overrides.TryGetValue(key, out Duration value)
            ? value
            : Defaults[key];
    }

    /// <summary>
    /// Overrides the <see cref="Duration"/> for the specified <paramref name="key"/>.
    /// Must be called before the WPF UI control dictionaries are parsed.
    /// </summary>
    /// <param name="key">The animation speed category to override.</param>
    /// <param name="value">The desired duration.</param>
    public static void Set(AnimationDuration key, TimeSpan value)
    {
        Overrides[key] = new Duration(value);
    }
}