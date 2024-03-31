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
    public static readonly DependencyProperty HeaderContentProperty = DependencyProperty.RegisterAttached(
        "HeaderContent",
        typeof(object),
        typeof(NavigationView),
        new FrameworkPropertyMetadata(null)
    );

    /// <summary>Helper for getting <see cref="HeaderContentProperty"/> from <paramref name="target"/>.</summary>
    /// <param name="target"><see cref="FrameworkElement"/> to read <see cref="HeaderContentProperty"/> from.</param>
    /// <returns>HeaderContent property value.</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("WpfAnalyzers.DependencyProperty", "WPF0033:Add [AttachedPropertyBrowsableForType]", Justification = "Don't need to pollute all FE with this attached property")]
    public static object? GetHeaderContent(FrameworkElement target) => target.GetValue(HeaderContentProperty);

    /// <summary>Helper for setting <see cref="HeaderContentProperty"/> on <paramref name="target"/>.</summary>
    /// <param name="target"><see cref="FrameworkElement"/> to set <see cref="HeaderContentProperty"/> on.</param>
    /// <param name="headerContent">HeaderContent property value.</param>
    public static void SetHeaderContent(FrameworkElement target, object? headerContent) =>
        target.SetValue(HeaderContentProperty, headerContent);
}
