// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows.Input;

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

/// <summary>
/// Custom <see cref="System.Windows.Controls.Primitives.ScrollBar"/> with events depending on actions taken by the user.
/// </summary>
public class DynamicScrollBar : System.Windows.Controls.Primitives.ScrollBar
{
    private bool _isScrolling = false;

    private bool _isInteracted = false;

    private readonly EventIdentifier _interactiveIdentifier = new();

    /// <summary>Identifies the <see cref="IsScrolling"/> dependency property.</summary>
    public static readonly DependencyProperty IsScrollingProperty = DependencyProperty.Register(
        nameof(IsScrolling),
        typeof(bool),
        typeof(DynamicScrollBar),
        new PropertyMetadata(false, OnIsScrollingChanged)
    );

    /// <summary>Identifies the <see cref="IsInteracted"/> dependency property.</summary>
    public static readonly DependencyProperty IsInteractedProperty = DependencyProperty.Register(
        nameof(IsInteracted),
        typeof(bool),
        typeof(DynamicScrollBar),
        new PropertyMetadata(false, OnIsInteractedChanged)
    );

    /// <summary>Identifies the <see cref="Timeout"/> dependency property.</summary>
    public static readonly DependencyProperty TimeoutProperty = DependencyProperty.Register(
        nameof(Timeout),
        typeof(int),
        typeof(DynamicScrollBar),
        new PropertyMetadata(1000)
    );

    /// <summary>
    /// Gets or sets a value indicating whether the user was recently scrolling in the last few seconds.
    /// </summary>
    public bool IsScrolling
    {
        get => (bool)GetValue(IsScrollingProperty);
        set => SetValue(IsScrollingProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the user has taken an action related to scrolling.
    /// </summary>
    public bool IsInteracted
    {
        get => (bool)GetValue(IsInteractedProperty);
        set
        {
            if ((bool)GetValue(IsInteractedProperty) != value)
            {
                SetValue(IsInteractedProperty, value);
            }
        }
    }

    /// <summary>
    /// Gets or sets additional delay after which the <see cref="DynamicScrollBar"/> should be hidden.
    /// </summary>
    public int Timeout
    {
        get => (int)GetValue(TimeoutProperty);
        set => SetValue(TimeoutProperty, value);
    }

    /// <summary>
    /// Method reporting the mouse entered this element.
    /// </summary>
    protected override void OnMouseEnter(MouseEventArgs e)
    {
        base.OnMouseEnter(e);

        UpdateScroll().GetAwaiter();
    }

    /// <summary>
    /// Method reporting the mouse leaved this element.
    /// </summary>
    protected override void OnMouseLeave(MouseEventArgs e)
    {
        base.OnMouseLeave(e);

        UpdateScroll().GetAwaiter();
    }

    private async Task UpdateScroll()
    {
        var currentEvent = _interactiveIdentifier.GetNext();
        var shouldScroll = IsMouseOver || _isScrolling;

        if (shouldScroll == _isInteracted)
        {
            return;
        }

        if (!shouldScroll)
        {
            await Task.Delay(Timeout);
        }

        if (!_interactiveIdentifier.IsEqual(currentEvent))
        {
            return;
        }

        IsInteracted = shouldScroll;
    }

    private static void OnIsScrollingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not DynamicScrollBar bar)
        {
            return;
        }

        if (bar._isScrolling == bar.IsScrolling)
        {
            return;
        }

        bar._isScrolling = !bar._isScrolling;

        bar.UpdateScroll().GetAwaiter();
    }

    private static void OnIsInteractedChanged(
        DependencyObject d,
        DependencyPropertyChangedEventArgs e
    )
    {
        if (d is not DynamicScrollBar bar)
        {
            return;
        }

        if (bar._isInteracted == bar.IsInteracted)
        {
            return;
        }

        bar._isInteracted = !bar._isInteracted;

        bar.UpdateScroll().GetAwaiter();
    }
}
