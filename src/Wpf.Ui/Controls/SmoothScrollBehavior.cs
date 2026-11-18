// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Input;
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

        public FrameworkElement? SourceElement { get; set; }
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
        new PropertyMetadata(300.0)
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

                ScrollViewer? sv = FindScrollViewer(element);
                if (sv != null)
                {
                    DetachScrollViewer(sv);
                }
            }
        }
    }

    private static void OnElementLoaded(object sender, RoutedEventArgs e)
    {
        if (sender is not FrameworkElement element)
        {
            return;
        }

        VirtualizingPanel.SetScrollUnit(element, ScrollUnit.Pixel);

        element.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Loaded, () =>
        {
            ScrollViewer? scrollViewer = FindScrollViewer(element);
            if (scrollViewer == null)
            {
                return;
            }

            VirtualizingPanel.SetScrollUnit(scrollViewer, ScrollUnit.Pixel);

            VirtualizingPanel? panel = FindVisualChild<VirtualizingPanel>(scrollViewer);
            if (panel != null)
            {
                VirtualizingPanel.SetScrollUnit(panel, ScrollUnit.Pixel);
            }

            ScrollData data = _scrollDataTable.GetOrCreateValue(scrollViewer);
            data.SourceElement = element;

            AttachScrollViewer(scrollViewer);
        });
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

        if (_scrollDataTable.TryGetValue(scrollViewer, out ScrollData? data))
        {
            if (data.SourceElement != null)
            {
                VirtualizingPanel.SetScrollUnit(data.SourceElement, ScrollUnit.Item);
            }

            VirtualizingPanel.SetScrollUnit(scrollViewer, ScrollUnit.Item);

            VirtualizingPanel? panel = FindVisualChild<VirtualizingPanel>(scrollViewer);
            if (panel != null)
            {
                VirtualizingPanel.SetScrollUnit(panel, ScrollUnit.Item);
            }
        }

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

        double duration = GetDuration(scrollViewer); // giữ default 250ms, hoặc set XAML Duration="220"

        DependencyProperty property = isVertical ? AnimatedVerticalOffsetProperty : AnimatedHorizontalOffsetProperty;

        double currentAnimatedValue = isVertical
            ? GetAnimatedVerticalOffset(scrollViewer)
            : GetAnimatedHorizontalOffset(scrollViewer);

        double fromValue = data.IsAnimating && currentAnimatedValue > 0
            ? currentAnimatedValue
            : (isVertical ? scrollViewer.VerticalOffset : scrollViewer.HorizontalOffset);

        scrollViewer.BeginAnimation(property, null);

        var animation = new DoubleAnimation
        {
            From = fromValue,
            To = toValue,
            Duration = TimeSpan.FromMilliseconds(duration),
            EasingFunction = new ExponentialEase { EasingMode = EasingMode.EaseOut, Exponent = 2.5 }
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

            DependencyObject? parent = null;
            if (element is Visual or System.Windows.Media.Media3D.Visual3D)
            {
                parent = VisualTreeHelper.GetParent(element);
            }

            parent ??= LogicalTreeHelper.GetParent(element);

            element = parent;
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

    private static T? FindVisualChild<T>(DependencyObject parent)
        where T : DependencyObject
    {
        for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
        {
            DependencyObject child = VisualTreeHelper.GetChild(parent, i);

            if (child is T t)
            {
                return t;
            }

            T? result = FindVisualChild<T>(child);
            if (result != null)
            {
                return result;
            }
        }

        return null;
    }
}
