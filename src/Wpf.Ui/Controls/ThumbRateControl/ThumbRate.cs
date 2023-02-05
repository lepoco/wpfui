// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.ComponentModel;
using System.Drawing;
using System.Windows;
using Wpf.Ui.Common;

namespace Wpf.Ui.Controls.ThumbRateControl;

/// <summary>
/// Allows to rate positively or negatively by clicking on one of the thumbs.
/// </summary>
[ToolboxItem(true)]
[ToolboxBitmap(typeof(ThumbRate), "ThumbRate.bmp")]
public class ThumbRate : System.Windows.Controls.Control
{
    /// <summary>
    /// Property for <see cref="State"/>.
    /// </summary>
    public static readonly DependencyProperty StateProperty = DependencyProperty.Register(nameof(State),
        typeof(ThumbRateState), typeof(ThumbRate),
        new PropertyMetadata(ThumbRateState.None, OnStateChanged));

    /// <summary>
    /// Event property for <see cref="StateChanged"/>.
    /// </summary>
    public static readonly RoutedEvent StateChangedEvent = EventManager.RegisterRoutedEvent(nameof(StateChanged),
        RoutingStrategy.Bubble, typeof(TypedEventHandler<ThumbRate, RoutedEventArgs>), typeof(ThumbRate));

    /// <summary>
    /// Occurs when <see cref="State"/> is changed.
    /// </summary>
    public event TypedEventHandler<ThumbRate, RoutedEventArgs> StateChanged
    {
        add => AddHandler(StateChangedEvent, value);
        remove => RemoveHandler(StateChangedEvent, value);
    }

    /// <summary>
    /// Property for <see cref="TemplateButtonCommand"/>.
    /// </summary>
    public static readonly DependencyProperty TemplateButtonCommandProperty =
        DependencyProperty.Register(nameof(TemplateButtonCommand),
            typeof(IRelayCommand), typeof(ThumbRate), new PropertyMetadata(null));

    /// <summary>
    /// Gets or sets the value determining the current state of the control.
    /// </summary>
    public ThumbRateState State
    {
        get => (ThumbRateState)GetValue(StateProperty);
        set => SetValue(StateProperty, value);
    }

    /// <summary>
    /// Command triggered after clicking the button.
    /// </summary>
    public IRelayCommand TemplateButtonCommand => (IRelayCommand)GetValue(TemplateButtonCommandProperty);

    /// <summary>
    /// Creates new instance and attaches <see cref="TemplateButtonCommand"/>.
    /// </summary>
    public ThumbRate()
    {
        SetValue(TemplateButtonCommandProperty, new RelayCommand<ThumbRateState>(OnTemplateButtonClick));
    }

    /// <summary>
    /// Triggered by clicking a button in the control template.
    /// </summary>
    protected virtual void OnTemplateButtonClick(ThumbRateState parameter)
    {
        if (State == parameter)
        {
            State = ThumbRateState.None;
            return;
        }

        State = parameter;
    }

    /// <summary>
    /// This virtual method is called when <see cref="State"/> is changed.
    /// </summary>
    protected virtual void OnStateChanged(ThumbRateState previousState, ThumbRateState currentState)
    {
        RaiseEvent(new RoutedEventArgs(StateChangedEvent, this));
    }

    private static void OnStateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not ThumbRate thumbRate)
            return;

        thumbRate.OnStateChanged((ThumbRateState)e.OldValue, (ThumbRateState)e.NewValue);
    }
}
