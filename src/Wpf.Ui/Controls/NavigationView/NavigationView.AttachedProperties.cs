// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

/// <content>
/// Defines attached properties for <see cref="NavigationView"/>.
/// </content>
public partial class NavigationView
{
    // ============================================================
    // HeaderContent Attached Property
    // ============================================================

    /// <summary>Registers attached property NavigationView.HeaderContent</summary>
    public static readonly DependencyProperty HeaderContentProperty = DependencyProperty.RegisterAttached(
        "HeaderContent",
        typeof(object),
        typeof(NavigationView),
        new FrameworkPropertyMetadata(null, OnHeaderContentChanged)
    );

    private static void OnHeaderContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not FrameworkElement frameworkElement)
        {
            return;
        }

        GetNavigationParent(frameworkElement)?.NotifyHeaderContentChanged(frameworkElement, e.NewValue);
    }

    /// <summary>Helper for getting <see cref="HeaderContentProperty"/> from <paramref name="target"/>.</summary>
    /// <param name="target"><see cref="FrameworkElement"/> to read <see cref="HeaderContentProperty"/> from.</param>
    /// <returns>HeaderContent property value.</returns>
    [AttachedPropertyBrowsableForType(typeof(FrameworkElement))]
    public static object? GetHeaderContent(FrameworkElement target) => target.GetValue(HeaderContentProperty);

    /// <summary>Helper for setting <see cref="HeaderContentProperty"/> on <paramref name="target"/>.</summary>
    /// <param name="target"><see cref="FrameworkElement"/> to set <see cref="HeaderContentProperty"/> on.</param>
    /// <param name="headerContent">HeaderContent property value.</param>
    public static void SetHeaderContent(FrameworkElement target, object? headerContent) =>
        target.SetValue(HeaderContentProperty, headerContent);

    // ============================================================
    // IsScrollable Attached Property
    // ============================================================

    /// <summary>
    /// Identifies the <see cref="IsScrollable"/> attached property.
    /// </summary>
    public static readonly DependencyProperty IsScrollableProperty = DependencyProperty.RegisterAttached(
        "IsScrollable",
        typeof(bool),
        typeof(NavigationView),
        new FrameworkPropertyMetadata(true)
    );

    /// <summary>
    /// Gets the value of the <see cref="IsScrollableProperty"/> attached property for a specified element.
    /// </summary>
    /// <param name="element">The element from which to read the property value.</param>
    /// <returns><see langword="true"/> if the content of the page hosted in <see cref="NavigationView"/> should be scrollable; otherwise, <see langword="false"/>.</returns>
    [AttachedPropertyBrowsableForType(typeof(DependencyObject))]
    public static bool GetIsScrollable(DependencyObject element)
    {
        return (bool)element.GetValue(IsScrollableProperty);
    }

    /// <summary>
    /// Sets the value of the <see cref="IsScrollableProperty"/> attached property for a specified element.
    /// </summary>
    /// <param name="element">The element on which to set the property value.</param>
    /// <param name="value">The value to set. <see langword="false"/> to disable scrolling on the <see cref="NavigationView"/> content presenter.</param>
    public static void SetIsScrollable(DependencyObject element, bool value)
    {
        element.SetValue(IsScrollableProperty, value);
    }

    // ============================================================
    // NavigationParent Attached Property
    // ============================================================

    /// <summary>Identifies the <see cref="NavigationParent"/> dependency property.</summary>
    internal static readonly DependencyProperty NavigationParentProperty =
        DependencyProperty.RegisterAttached(
            nameof(NavigationParent),
            typeof(NavigationView),
            typeof(NavigationView),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits)
        );

    /// <summary>
    /// Gets the parent <see cref="NavigationView"/> for its <see cref="INavigationViewItem"/> children.
    /// </summary>
    internal NavigationView? NavigationParent
    {
        get => (NavigationView?)GetValue(NavigationParentProperty);
        private set => SetValue(NavigationParentProperty, value);
    }

    /// <summary>Helper for getting <see cref="NavigationParentProperty"/> from <paramref name="navigationItem"/>.</summary>
    /// <param name="navigationItem"><see cref="DependencyObject"/> to read <see cref="NavigationParentProperty"/> from.</param>
    /// <returns>NavigationParent property value.</returns>
    [AttachedPropertyBrowsableForType(typeof(DependencyObject))]
    internal static NavigationView? GetNavigationParent(DependencyObject navigationItem) =>
        navigationItem.GetValue(NavigationParentProperty) as NavigationView;
}
