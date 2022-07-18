// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.ComponentModel;
using System.Drawing;
using System.Windows;
using Wpf.Ui.Controls.States;

namespace Wpf.Ui.Controls;

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
        RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ThumbRate));

    /// <summary>
    /// Occurs when <see cref="State"/> is changed.
    /// </summary>
    public event RoutedEventHandler StateChanged
    {
        add => AddHandler(StateChangedEvent, value);
        remove => RemoveHandler(StateChangedEvent, value);
    }

    /// <summary>
    /// Property for <see cref="TemplateButtonCommand"/>.
    /// </summary>
    public static readonly DependencyProperty TemplateButtonCommandProperty =
        DependencyProperty.Register(nameof(TemplateButtonCommand),
            typeof(Common.IRelayCommand), typeof(ThumbRate), new PropertyMetadata(null));

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
    public Common.IRelayCommand TemplateButtonCommand => (Common.IRelayCommand)GetValue(TemplateButtonCommandProperty);

    /// <summary>
    /// Creates new instance and attaches <see cref="TemplateButtonCommand"/>.
    /// </summary>
    public ThumbRate()
    {
        SetValue(TemplateButtonCommandProperty, new Common.RelayCommand(o => OnButtonClick(this, o)));
    }

    /// <summary>
    /// Triggered by clicking a button in the control template.
    /// </summary>
    /// <param name="sender">Sender of the click event.</param>
    /// <param name="parameter">Additional parameters.</param>
    protected virtual void OnButtonClick(object sender, object parameter)
    {
        if (parameter is not string)
            return;

        var param = parameter as string;

        switch (param)
        {
            case "up":
                State = State == ThumbRateState.Liked ? ThumbRateState.None : ThumbRateState.Liked;
                break;

            case "down":
                State = State == ThumbRateState.Disliked ? ThumbRateState.None : ThumbRateState.Disliked;
                break;
        }
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
