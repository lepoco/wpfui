// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Wpf.Ui.Input;

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

/// <summary>
/// Allows to rate positively or negatively by clicking on one of the thumbs.
/// </summary>
public class ThumbRate : System.Windows.Controls.Control
{
    /// <summary>Identifies the <see cref="State"/> dependency property.</summary>
    public static readonly DependencyProperty StateProperty = DependencyProperty.Register(
        nameof(State),
        typeof(ThumbRateState),
        typeof(ThumbRate),
        new PropertyMetadata(ThumbRateState.None, OnStateChanged)
    );

    /// <summary>Identifies the <see cref="StateChanged"/> routed event.</summary>
    public static readonly RoutedEvent StateChangedEvent = EventManager.RegisterRoutedEvent(
        nameof(StateChanged),
        RoutingStrategy.Bubble,
        typeof(TypedEventHandler<ThumbRate, RoutedEventArgs>),
        typeof(ThumbRate)
    );

    /// <summary>
    /// Occurs when <see cref="State"/> is changed.
    /// </summary>
    public event TypedEventHandler<ThumbRate, RoutedEventArgs> StateChanged
    {
        add => AddHandler(StateChangedEvent, value);
        remove => RemoveHandler(StateChangedEvent, value);
    }

    /// <summary>Identifies the <see cref="TemplateButtonCommand"/> dependency property.</summary>
    public static readonly DependencyProperty TemplateButtonCommandProperty = DependencyProperty.Register(
        nameof(TemplateButtonCommand),
        typeof(IRelayCommand),
        typeof(ThumbRate),
        new PropertyMetadata(null)
    );

    /// <summary>
    /// Gets or sets the value determining the current state of the control.
    /// </summary>
    public ThumbRateState State
    {
        get => (ThumbRateState)GetValue(StateProperty);
        set => SetValue(StateProperty, value);
    }

    /// <summary>
    /// Gets the command triggered when clicking the button.
    /// </summary>
    public IRelayCommand TemplateButtonCommand => (IRelayCommand)GetValue(TemplateButtonCommandProperty);

    /// <summary>
    /// Initializes a new instance of the <see cref="ThumbRate"/> class and attaches <see cref="TemplateButtonCommand"/>.
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
            SetCurrentValue(StateProperty, ThumbRateState.None);
            return;
        }

        SetCurrentValue(StateProperty, parameter);
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
        {
            return;
        }

        thumbRate.OnStateChanged((ThumbRateState)e.OldValue, (ThumbRateState)e.NewValue);
    }
}
