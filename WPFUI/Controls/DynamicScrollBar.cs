// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;
using System.Windows.Input;

namespace WPFUI.Controls;

/// <summary>
/// Custom <see cref="System.Windows.Controls.Primitives.ScrollBar"/> with events depending on actions taken by the user.
/// </summary>
public class DynamicScrollBar : System.Windows.Controls.Primitives.ScrollBar
{
    private bool _isScrolling = false;

    private bool _isInteracted = false;

    /// <summary>
    /// Property for <see cref="IsScrolling"/>.
    /// </summary>
    public static readonly DependencyProperty IsScrollingProperty = DependencyProperty.Register(nameof(IsScrolling),
        typeof(bool), typeof(DynamicScrollBar), new PropertyMetadata(false, IsScrollingProperty_OnChange));

    /// <summary>
    /// Property for <see cref="IsInteracted"/>.
    /// </summary>
    public static readonly DependencyProperty IsInteractedProperty = DependencyProperty.Register(
        nameof(IsInteracted),
        typeof(bool), typeof(DynamicScrollBar), new PropertyMetadata(false, IsInteractedProperty_OnChange));

    /// <summary>
    /// Gets or sets information whether the user was scrolling for the last few seconds.
    /// </summary>
    public bool IsScrolling
    {
        get => (bool)GetValue(IsScrollingProperty);
        set => SetValue(IsScrollingProperty, value);
    }

    /// <summary>
    /// Informs whether the user has taken an action related to scrolling.
    /// </summary>
    public bool IsInteracted
    {
        get => (bool)GetValue(IsInteractedProperty);
        set
        {
            if ((bool)GetValue(IsInteractedProperty) != value)
                SetValue(IsInteractedProperty, value);
        }
    }

    /// <summary>
    /// Method reporting the mouse entered this element.
    /// </summary>
    protected override void OnMouseEnter(MouseEventArgs e)
    {
        base.OnMouseEnter(e);

        if (!_isInteracted)
            IsInteracted = true;
    }

    /// <summary>
    /// Method reporting the mouse leaved this element.
    /// </summary>
    protected override void OnMouseLeave(MouseEventArgs e)
    {
        base.OnMouseLeave(e);

        if (_isInteracted != _isScrolling)
            IsInteracted = _isScrolling;
    }

    private void UpdateScroll()
    {
        var shouldScroll = IsMouseOver || _isScrolling;

        if (shouldScroll != _isInteracted)
            IsInteracted = shouldScroll;
    }

    private static void IsScrollingProperty_OnChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not DynamicScrollBar bar)
            return;

        if (bar._isScrolling == bar.IsScrolling)
            return;

        bar._isScrolling = !bar._isScrolling;

        bar.UpdateScroll();
    }

    private static void IsInteractedProperty_OnChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not DynamicScrollBar bar)
            return;

        if (bar._isInteracted == bar.IsInteracted)
            return;

        bar._isInteracted = !bar._isInteracted;

        bar.UpdateScroll();
    }
}
