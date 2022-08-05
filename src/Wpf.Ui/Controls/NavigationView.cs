// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

#nullable enable
using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Controls.Interfaces;
using Wpf.Ui.Controls.Navigation;

namespace Wpf.Ui.Controls;

/// <summary>
/// Ready navigation that includes a <see cref="INavigation"/> control, <see cref="System.Windows.Controls.Frame"/> and <see cref="Wpf.Ui.Controls.Breadcrumb"/>.
/// </summary>
[TemplatePart(Name = "PART_BackButton", Type = typeof(NavigationBackButton))]
[TemplatePart(Name = "PART_Breadcrumb", Type = typeof(Breadcrumb))]
[TemplatePart(Name = "PART_Frame", Type = typeof(Frame))]
public class NavigationView : System.Windows.Controls.Control
{
    public static readonly DependencyProperty NavigationProperty = DependencyProperty.Register(nameof(Navigation),
        typeof(INavigation), typeof(NavigationView),
        new PropertyMetadata(null));

    public static readonly DependencyProperty BreadcrumbMarginProperty = DependencyProperty.Register(nameof(BreadcrumbMargin),
        typeof(Thickness), typeof(NavigationView),
        new PropertyMetadata(new Thickness(18, 18, 18, 18)));

    public static readonly DependencyProperty FrameMarginProperty = DependencyProperty.Register(nameof(FrameMargin),
        typeof(Thickness), typeof(NavigationView),
        new PropertyMetadata(new Thickness(0, 0, 0, 0)));

    public static readonly DependencyProperty FramePaddingProperty = DependencyProperty.Register(nameof(FramePadding),
        typeof(Thickness), typeof(NavigationView),
        new PropertyMetadata(new Thickness(0, 0, 0, 0)));

    public static readonly DependencyProperty IsBackButtonVisibleProperty = DependencyProperty.Register(nameof(IsBackButtonVisible),
        typeof(bool), typeof(NavigationView),
        new PropertyMetadata(false));

    public static readonly DependencyProperty IsBreadcrumbVisibleProperty = DependencyProperty.Register(nameof(IsBreadcrumbVisible),
        typeof(bool), typeof(NavigationView),
        new PropertyMetadata(false));

    public static readonly DependencyProperty ContentProperty = DependencyProperty.Register(nameof(Content),
        typeof(object), typeof(NavigationView),
        new PropertyMetadata(null));

    public INavigation Navigation
    {
        get => (INavigation)GetValue(NavigationProperty);
        set => SetValue(NavigationProperty, value);
    }

    public Thickness BreadcrumbMargin
    {
        get => (Thickness)GetValue(BreadcrumbMarginProperty);
        set => SetValue(BreadcrumbMarginProperty, value);
    }

    public Thickness FrameMargin
    {
        get => (Thickness)GetValue(FrameMarginProperty);
        set => SetValue(FrameMarginProperty, value);
    }

    public Thickness FramePadding
    {
        get => (Thickness)GetValue(FramePaddingProperty);
        set => SetValue(FramePaddingProperty, value);
    }

    public bool IsBackButtonVisible
    {
        get => (bool)GetValue(IsBackButtonVisibleProperty);
        set => SetValue(IsBackButtonVisibleProperty, value);
    }

    public bool IsBreadcrumbVisible
    {
        get => (bool)GetValue(IsBreadcrumbVisibleProperty);
        set => SetValue(IsBreadcrumbVisibleProperty, value);
    }

    public object? Content
    {
        get => GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
    }

    /// <summary>
    /// Back button
    /// </summary>
    public NavigationBackButton BackButton { get; private set; } = null!;

    /// <summary>
    /// Navigation frame
    /// </summary>
    public Frame Frame { get; protected set; } = null!;

    /// <summary>
    /// Breadcrumb
    /// </summary>
    public Breadcrumb Breadcrumb { get; private set; } = null!;

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        BackButton = (NavigationBackButton)GetTemplateChild("PART_BackButton")!;
        Frame = (Frame)GetTemplateChild("PART_Frame")!;
        Breadcrumb = (Breadcrumb)GetTemplateChild("PART_Breadcrumb")!;

        Navigation.Frame = Frame;
    }
}
