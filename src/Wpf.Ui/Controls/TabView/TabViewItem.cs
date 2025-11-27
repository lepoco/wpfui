// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;
using System.Windows.Input;

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

/// <summary>
/// Represents a single tab within a <see cref="TabView"/>.
/// </summary>
[TemplatePart(Name = "PART_CloseButton", Type = typeof(System.Windows.Controls.Button))]
public class TabViewItem : System.Windows.Controls.TabItem
{
    /// <summary>Identifies the <see cref="IsClosable"/> dependency property.</summary>
    public static readonly DependencyProperty IsClosableProperty = DependencyProperty.Register(
        nameof(IsClosable),
        typeof(bool),
        typeof(TabViewItem),
        new PropertyMetadata(true)
    );

    /// <summary>
    /// Gets or sets a value indicating whether the tab can be closed.
    /// </summary>
    public bool IsClosable
    {
        get => (bool)GetValue(IsClosableProperty);
        set => SetValue(IsClosableProperty, value);
    }

    /// <summary>
    /// Occurs when the close button is clicked.
    /// </summary>
    public event RoutedEventHandler? CloseRequested;

    /// <summary>
    /// Raises the <see cref="CloseRequested"/> event.
    /// </summary>
    internal void OnCloseRequested()
    {
        CloseRequested?.Invoke(this, new RoutedEventArgs());
    }

    static TabViewItem()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(TabViewItem), new FrameworkPropertyMetadata(typeof(TabViewItem)));
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        if (GetTemplateChild("PART_CloseButton") is System.Windows.Controls.Button closeButton)
        {
            closeButton.Click -= OnCloseButtonClick;
            closeButton.Click += OnCloseButtonClick;
        }
    }

    private void OnCloseButtonClick(object sender, RoutedEventArgs e)
    {
        e.Handled = true;
        OnCloseRequested();
    }
}
