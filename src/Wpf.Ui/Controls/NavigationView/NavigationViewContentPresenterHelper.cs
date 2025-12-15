// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

/* Based on Windows UI Library */

using System.Windows.Controls;
using System.Windows.Media.Media3D;
using System.Windows.Threading;

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

/// <summary>
/// Helper attached property for pages to control whether the parent <see cref="NavigationViewContentPresenter"/>
/// should enable its internal dynamic scroll viewer.
/// </summary>
public static class NavigationViewContentPresenterHelper
{
    /// <summary>
    /// Attached property that lets page content request whether the nearest
    /// <see cref="NavigationViewContentPresenter"/> should enable its dynamic scroll viewer.
    /// When set to true/false the presenter's <c>IsDynamicScrollViewerEnabled</c> is updated accordingly.
    /// When null (not set) the presenter is left unchanged and its default behavior remains.
    /// </summary>
    /// <example>
    /// <code lang="xml">
    /// <Page xmlns:ui="http://schemas.lepo.co/wpfui/2022/xaml"
    ///       ui:NavigationViewContentPresenterHelper.EnableDynamicScrollViewer="False" />
    /// </code>
    /// </example>
    public static readonly DependencyProperty EnableDynamicScrollViewerProperty =
        DependencyProperty.RegisterAttached(
            "EnableDynamicScrollViewer",
            typeof(bool?),
            typeof(NavigationViewContentPresenterHelper),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.Inherits,
                OnEnableDynamicScrollViewerChanged
            )
        );

    [AttachedPropertyBrowsableForType(typeof(DependencyObject))]
    public static bool? GetEnableDynamicScrollViewer(DependencyObject target)
    {
        return (bool?)target.GetValue(EnableDynamicScrollViewerProperty);
    }

    public static void SetEnableDynamicScrollViewer(DependencyObject target, bool? value)
    {
        target.SetValue(EnableDynamicScrollViewerProperty, value);
    }

    private static void OnEnableDynamicScrollViewerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        // When a page sets this property, find the nearest NavigationViewContentPresenter
        // and update its IsDynamicScrollViewerEnabled accordingly.
        NavigationViewContentPresenter? presenter = FindAncestor<NavigationViewContentPresenter>(d);
        if (presenter is null)
        {
            return;
        }

        if (e.NewValue is bool newValue)
        {
            // Delay the change to avoid reentrancy caused by template reapplication.
            presenter.Dispatcher.BeginInvoke(
                new Action(() =>
                {
                    if (presenter.IsDynamicScrollViewerEnabled != newValue)
                    {
                        presenter.SetCurrentValue(NavigationViewContentPresenter.IsDynamicScrollViewerEnabledProperty, newValue);
                    }
                }),
                DispatcherPriority.Loaded
            );
        }

        // If the value is cleared (null), do not modify the presenter's configuration - maintain default behavior.
    }

    private static T? FindAncestor<T>(DependencyObject start)
        where T : DependencyObject
    {
        DependencyObject? current = start;
        while (current != null)
        {
            if (current is T t)
            {
                return t;
            }

            DependencyObject? parent = LogicalTreeHelper.GetParent(current);

            if (parent is null && (current is Visual || current is Visual3D))
            {
                parent = VisualTreeHelper.GetParent(current);
            }

            current = parent;
        }

        return null;
    }
}