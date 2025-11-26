// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Wpf.Ui.Controls;

/// <summary>
/// NavigationView resize handle control
/// </summary>
public class NavigationViewResizeHandle : Control
{
    static NavigationViewResizeHandle()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(NavigationViewResizeHandle),
            new FrameworkPropertyMetadata(typeof(NavigationViewResizeHandle))
        );
    }

    /// <summary>
    /// Minimum resizable width
    /// </summary>
    public static new readonly DependencyProperty MinWidthProperty = DependencyProperty.Register(
        nameof(MinWidth),
        typeof(double),
        typeof(NavigationViewResizeHandle),
        new FrameworkPropertyMetadata(200.0)
    );

    /// <summary>
    /// Maximum resizable width
    /// </summary>
    public static new readonly DependencyProperty MaxWidthProperty = DependencyProperty.Register(
        nameof(MaxWidth),
        typeof(double),
        typeof(NavigationViewResizeHandle),
        new FrameworkPropertyMetadata(500.0)
    );

    /// <summary>
    /// Current width
    /// </summary>
    public static readonly DependencyProperty CurrentWidthProperty = DependencyProperty.Register(
        nameof(CurrentWidth),
        typeof(double),
        typeof(NavigationViewResizeHandle),
        new FrameworkPropertyMetadata(320.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
    );

    /// <summary>
    /// Gets or sets the minimum resizable width
    /// </summary>
    public new double MinWidth
    {
        get => (double)GetValue(MinWidthProperty);
        set => SetValue(MinWidthProperty, value);
    }

    /// <summary>
    /// Gets or sets the maximum resizable width
    /// </summary>
    public new double MaxWidth
    {
        get => (double)GetValue(MaxWidthProperty);
        set => SetValue(MaxWidthProperty, value);
    }

    /// <summary>
    /// Gets or sets the current width
    /// </summary>
    public double CurrentWidth
    {
        get => (double)GetValue(CurrentWidthProperty);
        set => SetValue(CurrentWidthProperty, value);
    }

    /// <summary>
    /// Gets a value indicating whether the mouse button is pressed
    /// </summary>
    public bool IsPressed
    {
        get => (bool)GetValue(IsPressedProperty);
        private set => SetValue(IsPressedProperty, value);
    }

    /// <summary>Identifies the <see cref="IsPressed"/> dependency property.</summary>
    public static readonly DependencyProperty IsPressedProperty = DependencyProperty.Register(
        nameof(IsPressed),
        typeof(bool),
        typeof(NavigationViewResizeHandle),
        new FrameworkPropertyMetadata(false)
    );

    private bool _isResizing;
    private Point _startPoint;
    private double _startWidth;
    private double _lastWidth;

    protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
    {
        if (e.ButtonState == MouseButtonState.Pressed)
        {
            _isResizing = true;
            SetCurrentValue(IsPressedProperty, true);
            // Get position relative to parent element (NavigationView)
            _startPoint = e.GetPosition(this.Parent as FrameworkElement ?? this);
            _startWidth = CurrentWidth;
            _lastWidth = _startWidth;
            _ = CaptureMouse();
            e.Handled = true;
        }

        base.OnMouseLeftButtonDown(e);
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        if (_isResizing)
        {
            // Get position relative to parent element (NavigationView)
            Point currentPoint = e.GetPosition(this.Parent as FrameworkElement ?? this);
            double deltaX = currentPoint.X - _startPoint.X;
            double newWidth = _startWidth + deltaX;

            // Apply minimum and maximum width constraints
            newWidth = Math.Max(MinWidth, Math.Min(MaxWidth, newWidth));

            // Only update if width change is at least 1px to prevent flickering
            if (Math.Abs(newWidth - _lastWidth) >= 1.0)
            {
                SetCurrentValue(CurrentWidthProperty, newWidth);
                _lastWidth = newWidth;
            }

            e.Handled = true;
        }
        else
        {
            // Change mouse cursor to resize cursor
            SetCurrentValue(CursorProperty, Cursors.SizeWE);
        }

        base.OnMouseMove(e);
    }

    protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
    {
        if (_isResizing)
        {
            _isResizing = false;
            SetCurrentValue(IsPressedProperty, false);
            ReleaseMouseCapture();
            e.Handled = true;
        }

        base.OnMouseLeftButtonUp(e);
    }

    protected override void OnMouseLeave(MouseEventArgs e)
    {
        if (!_isResizing)
        {
            SetCurrentValue(CursorProperty, Cursors.Arrow);
        }

        base.OnMouseLeave(e);
    }

    protected override void OnLostMouseCapture(MouseEventArgs e)
    {
        _isResizing = false;
        SetCurrentValue(IsPressedProperty, false);
        base.OnLostMouseCapture(e);
    }
}

