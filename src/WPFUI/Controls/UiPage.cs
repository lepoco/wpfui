// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace WPFUI.Controls;

/// <summary>
/// Extended <see cref="System.Windows.Controls.Page"/> with WPF UI features. 
/// </summary>
[TemplatePart(Name = "PART_ScrollViewer", Type = typeof(WPFUI.Controls.DynamicScrollViewer))]
public class UiPage : System.Windows.Controls.Page
{
    /// <summary>
    /// Template element represented by the <c>PART_Popup</c> name.
    /// </summary>
    private const string ElementScrollViewer = "PART_ScrollViewer";

    /// <summary>
    /// Property for <see cref="Scrollable"/>.
    /// </summary>
    public static readonly DependencyProperty ScrollableProperty = DependencyProperty.Register(nameof(Scrollable),
        typeof(bool), typeof(UiPage), new PropertyMetadata(false));

    /// <summary>
    /// Property for <see cref="ScrollHost"/>.
    /// </summary>
    public static readonly DependencyProperty ScrollHostProperty = DependencyProperty.Register(nameof(ScrollHost),
        typeof(ScrollViewer), typeof(UiPage), new PropertyMetadata((ScrollViewer)null));

    /// <summary>
    /// Gets or sets a value determining whether the content should be scrollable.
    /// <para>If set, <see cref="WPFUI.Controls.DynamicScrollViewer"/> will be added to the <see cref="System.Windows.Controls.Control.Template"/></para>
    /// </summary>
    [Bindable(true), Category("Appearance")]
    public bool Scrollable
    {
        get => (bool)GetValue(ScrollableProperty);
        set => SetValue(ScrollableProperty, value);
    }

    /// <summary>
    /// If the page is <see cref="Scrollable"/>, gets a <see cref="ScrollViewer"/> that container the <see cref="Page"/>.
    /// </summary>
    [Bindable(false)]
    public ScrollViewer ScrollHost
    {
        get
        {
            if (!Scrollable)
                throw new InvalidOperationException($"{nameof(ScrollHost)} can only be taken from a page that has {nameof(Scrollable)} set to true.");

            return (ScrollViewer)GetValue(ScrollHostProperty);
        }
        private set => SetValue(ScrollHostProperty, value);
    }

    public UiPage()
    {
        SetResourceReference(StyleProperty, typeof(UiPage));
    }

    static UiPage()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(UiPage), new FrameworkPropertyMetadata(typeof(UiPage)));
    }

    /// <summary>
    /// Invoked whenever application code or an internal process,
    /// such as a rebuilding layout pass, calls the ApplyTemplate method.
    /// </summary>
    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        var scrollHost = GetTemplateChild(ElementScrollViewer);

        if (scrollHost is ScrollViewer)
            ScrollHost = scrollHost as ScrollViewer;
    }
}
