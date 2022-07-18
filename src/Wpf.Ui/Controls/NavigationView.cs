// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Common;
using Wpf.Ui.Controls.Interfaces;

namespace Wpf.Ui.Controls;

/// <summary>
/// Ready navigation that includes a <see cref="INavigation"/> control, <see cref="System.Windows.Controls.Frame"/> and <see cref="Wpf.Ui.Controls.Breadcrumb"/>.
/// </summary>
[TemplatePart(Name = "PART_Navigation", Type = typeof(Wpf.Ui.Controls.Navigation.NavigationBase))]
[TemplatePart(Name = "PART_Breadcrumb", Type = typeof(Wpf.Ui.Controls.Breadcrumb))]
[TemplatePart(Name = "PART_Frame", Type = typeof(System.Windows.Controls.Frame))]
public class NavigationView : System.Windows.Controls.Control
{
    /// <summary>
    /// Template element represented by the <c>PART_Popup</c> name.
    /// </summary>
    private const string ElementNavigation = "PART_Navigation";

    /// <summary>
    /// Template element represented by the <c>PART_Popup</c> name.
    /// </summary>
    private const string ElementBreadcrumb = "PART_Breadcrumb";

    /// <summary>
    /// Template element represented by the <c>PART_Popup</c> name.
    /// </summary>
    private const string ElementFrame = "PART_Frame";

    public static readonly DependencyProperty TypeProperty = DependencyProperty.Register(nameof(Type),
        typeof(NavigationType), typeof(NavigationView),
        new PropertyMetadata(NavigationType.Compact));

    /// <summary>
    /// Navigation control.
    /// </summary>
    public INavigation Navigation { get; protected set; }

    /// <summary>
    /// Navigation breadcrumb.
    /// </summary>
    public Breadcrumb Breadcrumb { get; protected set; }

    /// <summary>
    /// Navigation frame
    /// </summary>
    public Frame Frame { get; protected set; }

    public NavigationType Type
    {
        get => (NavigationType)GetValue(TypeProperty);
        set => SetValue(TypeProperty, value);
    }

    /// <inheritdoc />
    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        Navigation = GetTemplateChild(ElementNavigation) as INavigation;
        Breadcrumb = GetTemplateChild(ElementBreadcrumb) as Breadcrumb;
        Frame = GetTemplateChild(ElementFrame) as Frame;
    }
}
