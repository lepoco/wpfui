// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Wpf.Ui.Controls;

/// <summary>
/// Attached behavior to add smooth scrolling to any ScrollViewer
/// </summary>
public static class SmoothScrollBehavior
{
    private class ScrollData
    {
        public double LastVerticalOffset { get; set; }

        public double LastHorizontalOffset { get; set; }

        public bool IsAnimating { get; set; }
    }

    private static readonly ConditionalWeakTable<ScrollViewer, ScrollData> _scrollDataTable = new();

    public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached(
        "IsEnabled",
        typeof(bool),
        typeof(SmoothScrollBehavior),
        new PropertyMetadata(false, OnIsEnabledChanged)
    );

    public static readonly DependencyProperty DurationProperty = DependencyProperty.RegisterAttached(
        "Duration",
        typeof(double),
        typeof(SmoothScrollBehavior),
        new PropertyMetadata(250.0)
    );

    public static readonly DependencyProperty MultiplierProperty = DependencyProperty.RegisterAttached(
        "Multiplier",
        typeof(double),
        typeof(SmoothScrollBehavior),
        new PropertyMetadata(1.0)
    );

    public static readonly DependencyProperty AnimatedVerticalOffsetProperty = DependencyProperty.RegisterAttached(
        "AnimatedVerticalOffset",
        typeof(double),
        typeof(SmoothScrollBehavior),
        new PropertyMetadata(0.0, OnAnimatedVerticalOffsetChanged)
    );

    public static readonly DependencyProperty AnimatedHorizontalOffsetProperty = DependencyProperty.RegisterAttached(
        "AnimatedHorizontalOffset",
        typeof(double),
        typeof(SmoothScrollBehavior),
        new PropertyMetadata(0.0, OnAnimatedHorizontalOffsetChanged)
    );

    public static readonly DependencyProperty IsAnimatingProperty = DependencyProperty.RegisterAttached(
        "IsAnimating",
        typeof(bool),
        typeof(SmoothScrollBehavior),
        new PropertyMetadata(false)
    );

    public static bool GetIsEnabled(DependencyObject obj) => (bool)obj.GetValue(IsEnabledProperty);

    public static void SetIsEnabled(DependencyObject obj, bool value) => obj.SetValue(IsEnabledProperty, value);

    public static double GetDuration(DependencyObject obj) => (double)obj.GetValue(DurationProperty);

    public static void SetDuration(DependencyObject obj, double value) => obj.SetValue(DurationProperty, value);

    public static double GetMultiplier(DependencyObject obj) => (double)obj.GetValue(MultiplierProperty);

    public static void SetMultiplier(DependencyObject obj, double value) => obj.SetValue(MultiplierProperty, value);

    private static double GetAnimatedVerticalOffset(DependencyObject obj) => (double)obj.GetValue(AnimatedVerticalOffsetProperty);

    private static void SetAnimatedVerticalOffset(DependencyObject obj, double value) => obj.SetValue(AnimatedVerticalOffsetProperty, value);

    private static double GetAnimatedHorizontalOffset(DependencyObject obj) => (double)obj.GetValue(AnimatedHorizontalOffsetProperty);

    private static void SetAnimatedHorizontalOffset(DependencyObject obj, double value) => obj.SetValue(AnimatedHorizontalOffsetProperty, value);

    private static void OnIsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is ScrollViewer scrollViewer)
        {
            if ((bool)e.NewValue)
            {
                AttachScrollViewer(scrollViewer);
            }
            else
            {
                DetachScrollViewer(scrollViewer);
            }
        }
        else if (d is FrameworkElement element)
        {
            if ((bool)e.NewValue)
            {
                element.Loaded += OnElementLoaded;
            }
            else
            {
                element.Loaded -= OnElementLoaded;
            }
        }
    }

    private static void OnElementLoaded(object sender, RoutedEventArgs e)
    {
        if (sender is FrameworkElement element)
        {
            ScrollViewer? scrollViewer = FindScrollViewer(element);

            if (scrollViewer != null)
            {
                AttachScrollViewer(scrollViewer);
            }
        }
    }

    private static void AttachScrollViewer(ScrollViewer scrollViewer)
    {
        ScrollData data = _scrollDataTable.GetOrCreateValue(scrollViewer);

        data.LastVerticalOffset = scrollViewer.VerticalOffset;
        data.LastHorizontalOffset = scrollViewer.HorizontalOffset;

        scrollViewer.PreviewMouseWheel += ScrollViewer_PreviewMouseWheel;
        scrollViewer.ScrollChanged += ScrollViewer_ScrollChanged;
    }

    private static void DetachScrollViewer(ScrollViewer scrollViewer)
    {
        scrollViewer.PreviewMouseWheel -= ScrollViewer_PreviewMouseWheel;
        scrollViewer.ScrollChanged -= ScrollViewer_ScrollChanged;

        _ = _scrollDataTable.Remove(scrollViewer);
    }

    private static void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
    {
        if (sender is not ScrollViewer scrollViewer)
        {
            return;
        }

        if (!_scrollDataTable.TryGetValue(scrollViewer, out ScrollData? data))
        {
            return;
        }

        // Check if scrolling inside nested scrollviewer
        if (IsNestedScrollViewer(e.OriginalSource as DependencyObject, scrollViewer))
        {
            return;
        }

        bool isHorizontal = Keyboard.Modifiers == ModifierKeys.Shift;
        double multiplier = GetMultiplier(scrollViewer);

        if (isHorizontal)
        {
            if (scrollViewer.ScrollableWidth <= 0)
            {
                return;
            }

            e.Handled = true;

            double wheelChange = e.Delta * multiplier;
            double newOffset = data.LastHorizontalOffset - wheelChange;
            newOffset = Math.Max(0, Math.Min(scrollViewer.ScrollableWidth, newOffset));

            if (Math.Abs(newOffset - data.LastHorizontalOffset) < 0.1)
            {
                return;
            }

            scrollViewer.ScrollToHorizontalOffset(data.LastHorizontalOffset);
            AnimateScroll(scrollViewer, newOffset, false);
            data.LastHorizontalOffset = newOffset;
        }
        else
        {
            if (scrollViewer.ScrollableHeight <= 0)
            {
                return;
            }

            double wheelChange = e.Delta * multiplier;
            double newOffset = data.LastVerticalOffset - wheelChange;

            // Check boundary for parent scrolling
            if ((newOffset < 0 && wheelChange < 0) || (newOffset > scrollViewer.ScrollableHeight && wheelChange > 0))
            {
                return;
            }

            e.Handled = true;

            newOffset = Math.Max(0, Math.Min(scrollViewer.ScrollableHeight, newOffset));

            if (Math.Abs(newOffset - data.LastVerticalOffset) < 0.1)
            {
                return;
            }

            scrollViewer.ScrollToVerticalOffset(data.LastVerticalOffset);
            AnimateScroll(scrollViewer, newOffset, true);
            data.LastVerticalOffset = newOffset;
        }
    }

    private static void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
    {
        if (sender is not ScrollViewer scrollViewer)
        {
            return;
        }

        if (!_scrollDataTable.TryGetValue(scrollViewer, out ScrollData? data))
        {
            return;
        }

        // Update last offsets only when not animating
        if (!data.IsAnimating)
        {
            data.LastVerticalOffset = scrollViewer.VerticalOffset;
            data.LastHorizontalOffset = scrollViewer.HorizontalOffset;
        }
    }

    private static void AnimateScroll(ScrollViewer scrollViewer, double toValue, bool isVertical)
    {
        if (!_scrollDataTable.TryGetValue(scrollViewer, out ScrollData? data))
        {
            return;
        }

        data.IsAnimating = true;

        double duration = GetDuration(scrollViewer);

        DependencyProperty property = isVertical ? AnimatedVerticalOffsetProperty : AnimatedHorizontalOffsetProperty;

        double fromValue = isVertical ? scrollViewer.VerticalOffset : scrollViewer.HorizontalOffset;

        scrollViewer.BeginAnimation(property, null);

        var animation = new DoubleAnimation
        {
            From = fromValue,
            To = toValue,
            Duration = TimeSpan.FromMilliseconds(duration),
            EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
        };

        animation.Completed += (s, e) => { data.IsAnimating = false; };

        scrollViewer.BeginAnimation(property, animation);
    }

    private static void OnAnimatedVerticalOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is ScrollViewer scrollViewer)
        {
            scrollViewer.ScrollToVerticalOffset((double)e.NewValue);
        }
    }

    private static void OnAnimatedHorizontalOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is ScrollViewer scrollViewer)
        {
            scrollViewer.ScrollToHorizontalOffset((double)e.NewValue);
        }
    }

    private static bool IsNestedScrollViewer(DependencyObject? element, ScrollViewer parentScrollViewer)
    {
        if (element == null)
        {
            return false;
        }

        while (element != null && element != parentScrollViewer)
        {
            if (element is ScrollViewer sv && sv != parentScrollViewer)
            {
                return sv.ScrollableHeight > 0 || sv.ScrollableWidth > 0;
            }

            element = VisualTreeHelper.GetParent(element);
        }

        return false;
    }

    private static ScrollViewer? FindScrollViewer(DependencyObject element)
    {
        if (element is ScrollViewer sv)
        {
            return sv;
        }

        for (int i = 0; i < VisualTreeHelper.GetChildrenCount(element); i++)
        {
            DependencyObject child = VisualTreeHelper.GetChild(element, i);
            ScrollViewer? result = FindScrollViewer(child);
            if (result != null)
            {
                return result;
            }
        }

        return null;
    }
}