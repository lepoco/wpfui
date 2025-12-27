// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

/// <summary>
/// Internal theming infrastructure for <see cref="System.Windows.Controls.TextBlock"/>.
///
/// This helper provides attached properties used as internal resource-reference
/// holders, allowing the WPF resource system to resolve and cache themed brushes
/// without invoking <c>TryFindResource</c> from dependency property metadata
/// callbacks.
///
/// These properties are implementation details and are not intended to be used
/// or set directly by consumers.
/// </summary>
internal static class TextBlockTheming
{
    /// <summary>
    /// Attached property to hold a resource reference to a Brush that should be
    /// used as the default Foreground when the control's Foreground is not set.
    /// </summary>
    public static readonly DependencyProperty FallbackForegroundProperty =
        DependencyProperty.RegisterAttached(
            "FallbackForeground",
            typeof(Brush),
            typeof(TextBlockTheming),
            new PropertyMetadata(
                null,
                static (d, e) =>
                {
                    // When the attached resource ref resolves/changes, coerce Foreground so the new value is used.
                    if (d is System.Windows.Controls.TextBlock tb)
                    {
                        tb.CoerceValue(System.Windows.Controls.TextBlock.ForegroundProperty);
                    }
                }
            )
        );

    /// <summary>
    /// Helper for setting <see cref="FallbackForegroundProperty"/> on <paramref name="element"/>.
    /// </summary>
    /// <param name="element">
    /// <see cref="DependencyObject"/> to set <see cref="FallbackForegroundProperty"/> on.
    /// </param>
    /// <param name="value">
    /// FallbackForeground property value.
    /// </param>
    public static void SetFallbackForeground(DependencyObject element, Brush? value)
    {
        element.SetValue(FallbackForegroundProperty, value);
    }

    /// <summary>
    /// Helper for getting <see cref="FallbackForegroundProperty"/> from <paramref name="element"/>.
    /// </summary>
    /// <param name="element">
    /// <see cref="DependencyObject"/> to read <see cref="FallbackForegroundProperty"/> from.
    /// </param>
    /// <returns>
    /// FallbackForeground property value.
    /// </returns>
    [AttachedPropertyBrowsableForType(typeof(System.Windows.Controls.TextBlock))]
    public static Brush? GetFallbackForeground(DependencyObject element)
    {
        return (Brush?)element.GetValue(FallbackForegroundProperty);
    }

    /// <summary>
    /// A Puppet dependency property that is used to hold the foreground resource resolved from the Appearance property.
    /// Used only as a resolution proxy, so no accessors are provided."
    /// </summary>
    public static readonly DependencyProperty AppearanceForegroundProxyProperty =
        DependencyProperty.RegisterAttached(
            "AppearanceForegroundProxy",
            typeof(Brush),
            typeof(TextBlockTheming),
            new PropertyMetadata(
                null,
                static (d, e) =>
                {
                    if (d is System.Windows.Controls.TextBlock tb)
                    {
                        tb.CoerceValue(System.Windows.Controls.TextBlock.ForegroundProperty);
                    }
                }
            )
        );

    /// <summary>
    /// Private Puppet dependency property used to hold a ResourceReference to a FontTypographyPreset.
    /// </summary>
    public static readonly DependencyProperty FontTypographyProxyProperty =
        DependencyProperty.RegisterAttached(
            "FontTypographyProxy",
            typeof(FontTypographyPreset),
            typeof(TextBlockTheming),
            new PropertyMetadata(
                null,
                static (d, e) =>
                {
                    if (d is System.Windows.Controls.TextBlock tb)
                    {
                        tb.CoerceValue(System.Windows.Controls.TextBlock.FontSizeProperty);
                        tb.CoerceValue(System.Windows.Controls.TextBlock.FontWeightProperty);
                    }
                }
            )
        );
}