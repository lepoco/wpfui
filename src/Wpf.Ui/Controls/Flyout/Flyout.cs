// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls.Primitives;

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

/// <summary>
/// Represents a control that creates a pop-up window that displays information for an element in the interface.
/// </summary>
[TemplatePart(Name = "PART_Popup", Type = typeof(System.Windows.Controls.Primitives.Popup))]
public class Flyout : System.Windows.Controls.ContentControl
{
    private const string ElementPopup = "PART_Popup";

    private System.Windows.Controls.Primitives.Popup? _popup = default;

    /// <summary>
    /// Property for <see cref="IsOpen"/>.
    /// </summary>
    public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(
        nameof(IsOpen),
        typeof(bool),
        typeof(Flyout),
        new PropertyMetadata(false)
    );

    /// <summary>
    /// Property for <see cref="Placement"/>.
    /// </summary>
    public static readonly DependencyProperty PlacementProperty = DependencyProperty.Register(
        nameof(Placement),
        typeof(PlacementMode),
        typeof(Flyout),
        new PropertyMetadata(PlacementMode.Top)
    );

    /// <summary>
    /// Routed event for <see cref="Opened"/>.
    /// </summary>
    public static readonly RoutedEvent OpenedEvent = EventManager.RegisterRoutedEvent(
        nameof(Opened),
        RoutingStrategy.Bubble,
        typeof(TypedEventHandler<Flyout, RoutedEventArgs>),
        typeof(Flyout)
    );

    /// <summary>
    /// Routed event for <see cref="Closed"/>.
    /// </summary>
    public static readonly RoutedEvent ClosedEvent = EventManager.RegisterRoutedEvent(
        nameof(Closed),
        RoutingStrategy.Bubble,
        typeof(TypedEventHandler<Flyout, RoutedEventArgs>),
        typeof(Flyout)
    );

    /// <summary>
    /// Gets or sets a value indicating whether a <see cref="Flyout" /> is visible.
    /// </summary>
    public bool IsOpen
    {
        get => (bool)GetValue(IsOpenProperty);
        set => SetValue(IsOpenProperty, value);
    }

    /// <summary>
    /// Event triggered when <see cref="Flyout" /> is opened.
    /// </summary>
    public event TypedEventHandler<Flyout, RoutedEventArgs> Opened
    {
        add => AddHandler(OpenedEvent, value);
        remove => RemoveHandler(OpenedEvent, value);
    }

    /// <summary>
    /// Event triggered when <see cref="Flyout" /> is opened.
    /// </summary>
    public event TypedEventHandler<Flyout, RoutedEventArgs> Closed
    {
        add => AddHandler(ClosedEvent, value);
        remove => RemoveHandler(ClosedEvent, value);
    }

    /// <summary>
    /// Gets or sets the orientation of the <see cref="Flyout" /> control when the control opens,
    /// and specifies the behavior of the <see cref="T:System.Windows.Controls.Primitives.Popup" />
    /// control when it overlaps screen boundaries.
    /// </summary>
    [Bindable(true)]
    [Category("Layout")]
    public PlacementMode Placement
    {
        get => (PlacementMode)GetValue(PlacementProperty);
        set => SetValue(PlacementProperty, value);
    }

    /// <summary>
    /// Invoked whenever application code or an internal process,
    /// such as a rebuilding layout pass, calls the ApplyTemplate method.
    /// </summary>
    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        _popup = GetTemplateChild(ElementPopup) as System.Windows.Controls.Primitives.Popup;

        if (_popup is null)
        {
            return;
        }

        _popup.Opened -= OnPopupOpened;
        _popup.Opened += OnPopupOpened;

        _popup.Closed -= OnPopupClosed;
        _popup.Closed += OnPopupClosed;
    }

    public void Show()
    {
        if (!IsOpen)
        {
            SetCurrentValue(IsOpenProperty, true);
        }
    }

    public void Hide()
    {
        if (IsOpen)
        {
            SetCurrentValue(IsOpenProperty, false);
        }
    }

    protected virtual void OnPopupOpened(object? sender, EventArgs e)
    {
        RaiseEvent(new RoutedEventArgs(OpenedEvent, this));
    }

    protected virtual void OnPopupClosed(object? sender, EventArgs e)
    {
        Hide();
        RaiseEvent(new RoutedEventArgs(ClosedEvent, this));
    }
}
