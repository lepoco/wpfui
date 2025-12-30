// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows.Controls;

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

/// <summary>
/// Provides <c>Orientation</c> property for Separator
/// </summary>
/// <example>
/// <code lang="xml">
/// &lt;Separator controls:SeparatorAttached.Orientation="Vertical" /&gt;
/// </code>
/// </example>
public static class SeparatorAttached
{
    /// <summary>
    /// Resource key for <see cref="Separator"/> horizontal margin.
    /// </summary>
    public const string HorizontalMarginKey = "SeparatorHorizontalMargin";

    /// <summary>
    /// Resource key for <see cref="Separator"/> vertical margin.
    /// </summary>
    public const string VerticalMarginKey = "SeparatorVerticalMargin";

    public static readonly DependencyProperty OrientationProperty =
        DependencyProperty.RegisterAttached(
            "Orientation",
            typeof(Orientation),
            typeof(SeparatorAttached),
            new FrameworkPropertyMetadata(Orientation.Horizontal, OnOrientationChanged)
        );

    /// <summary>Helper for getting <see cref="OrientationProperty"/> from <paramref name="obj"/>.</summary>
    /// <param name="obj"><see cref="DependencyObject"/> to read <see cref="OrientationProperty"/> from.</param>
    /// <returns>Orientation property value.</returns>
    [AttachedPropertyBrowsableForType(typeof(Separator))]
    public static Orientation GetOrientation(DependencyObject obj)
    {
        return (Orientation)obj.GetValue(OrientationProperty);
    }

    /// <summary>Helper for setting <see cref="OrientationProperty"/> on <paramref name="obj"/>.</summary>
    /// <param name="obj"><see cref="DependencyObject"/> to set <see cref="OrientationProperty"/> on.</param>
    /// <param name="value">Orientation property value.</param>
    public static void SetOrientation(DependencyObject obj, Orientation value)
    {
        obj.SetValue(OrientationProperty, value);
    }

    private static void OnOrientationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not FrameworkElement element)
        {
            return;
        }

        Thickness currentMargin = element.Margin;
        Thickness? horizontalMargin = element.TryFindResource(HorizontalMarginKey) as Thickness?;
        Thickness? verticalMargin = element.TryFindResource(VerticalMarginKey) as Thickness?;
        Orientation newOrientation = (Orientation)e.NewValue;

        // Only swap margin if it matches one of our default values
        if (newOrientation == Orientation.Vertical && currentMargin == horizontalMargin)
        {
            element.SetResourceReference(FrameworkElement.MarginProperty, VerticalMarginKey);
        }
        else if (newOrientation == Orientation.Horizontal && currentMargin == verticalMargin)
        {
            element.SetResourceReference(FrameworkElement.MarginProperty, HorizontalMarginKey);
        }
    }
}