// Based on Windows UI Library
// Copyright(c) Microsoft Corporation.All rights reserved.

// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.


using System;
using System.Windows;
using Wpf.Ui.Controls.Interfaces;

namespace Wpf.Ui.Controls.Navigation;

/// <summary>
/// Breadcrumb control allows you to keep track and maintain awareness of your locations within <see cref="INavigationView"/>.
/// </summary>
public class NavigationViewBreadcrumb : System.Windows.Controls.Control
{
    /// <summary>
    /// Property for <see cref="NavigationView"/>.
    /// </summary>
    public static readonly DependencyProperty NavigationViewProperty = DependencyProperty.Register(nameof(NavigationView),
        typeof(INavigationView), typeof(NavigationViewBreadcrumb),
        new PropertyMetadata(((NavigationViewBreadcrumb)null!), OnNavigationViewPropertyChanged));

    /// <summary>
    /// Property for <see cref="Text"/>.
    /// </summary>
    public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof(Text),
        typeof(string), typeof(NavigationViewBreadcrumb),
        new PropertyMetadata(String.Empty));


    /// <summary>
    /// Pinned navigation control.
    /// </summary>
    public INavigationView NavigationView
    {
        get => (INavigationView)GetValue(NavigationViewProperty);
        set => SetValue(NavigationViewProperty, value);
    }

    /// <summary>
    /// Primary breadcrumb navigation element.
    /// </summary>
    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public NavigationViewBreadcrumb()
    {
        if (NavigationView != null)
            UpdateRenderedItems();
    }

    /// <summary>
    /// This virtual method is called when the <see cref="NavigationView"/> is changed.
    /// </summary>
    protected virtual void OnNavigationViewChanged()
    {
        if (NavigationView == null)
            return;

        NavigationView.SelectionChanged -= OnNavigationViewSelectionChanged;
        NavigationView.SelectionChanged += OnNavigationViewSelectionChanged;
    }

    /// <summary>
    /// This virtual method is called when the <see cref="INavigationView.SelectedItem"/> of the <see cref="NavigationView"/> is changed.
    /// </summary>
    protected virtual void OnNavigationViewSelectionChanged(object sender, RoutedEventArgs e)
    {
        UpdateRenderedItems();
    }

    private static void OnNavigationViewPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not NavigationViewBreadcrumb breadcrumb)
            return;

        breadcrumb.OnNavigationViewChanged();
    }

    private void UpdateRenderedItems()
    {
        if (NavigationView.SelectedItem is not NavigationViewItem navigationViewItem)
        {
            Text = String.Empty;

            return;
        }

        // TODO: Multilevel
        Text = navigationViewItem.Content?.ToString() ?? String.Empty;
    }
}
