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
/// NavigationViewのリサイズハンドルコントロール
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
    /// リサイズ可能な最小幅
    /// </summary>
    public static readonly DependencyProperty MinWidthProperty = DependencyProperty.Register(
        nameof(MinWidth),
        typeof(double),
        typeof(NavigationViewResizeHandle),
        new FrameworkPropertyMetadata(200.0)
    );

    /// <summary>
    /// リサイズ可能な最大幅
    /// </summary>
    public static readonly DependencyProperty MaxWidthProperty = DependencyProperty.Register(
        nameof(MaxWidth),
        typeof(double),
        typeof(NavigationViewResizeHandle),
        new FrameworkPropertyMetadata(500.0)
    );

    /// <summary>
    /// 現在の幅
    /// </summary>
    public static readonly DependencyProperty CurrentWidthProperty = DependencyProperty.Register(
        nameof(CurrentWidth),
        typeof(double),
        typeof(NavigationViewResizeHandle),
        new FrameworkPropertyMetadata(320.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
    );

    /// <summary>
    /// リサイズ可能な最小幅を取得または設定します
    /// </summary>
    public double MinWidth
    {
        get => (double)GetValue(MinWidthProperty);
        set => SetValue(MinWidthProperty, value);
    }

    /// <summary>
    /// リサイズ可能な最大幅を取得または設定します
    /// </summary>
    public double MaxWidth
    {
        get => (double)GetValue(MaxWidthProperty);
        set => SetValue(MaxWidthProperty, value);
    }

    /// <summary>
    /// 現在の幅を取得または設定します
    /// </summary>
    public double CurrentWidth
    {
        get => (double)GetValue(CurrentWidthProperty);
        set => SetValue(CurrentWidthProperty, value);
    }

    /// <summary>
    /// マウスダウン中の状態を取得します
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
            IsPressed = true;
            // 親要素（NavigationView）に対する位置を取得
            _startPoint = e.GetPosition(this.Parent as FrameworkElement ?? this);
            _startWidth = CurrentWidth;
            _lastWidth = _startWidth;
            CaptureMouse();
            e.Handled = true;
        }

        base.OnMouseLeftButtonDown(e);
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        if (_isResizing)
        {
            // 親要素（NavigationView）に対する位置を取得
            Point currentPoint = e.GetPosition(this.Parent as FrameworkElement ?? this);
            double deltaX = currentPoint.X - _startPoint.X;
            double newWidth = _startWidth + deltaX;

            // 最小・最大幅の制限を適用
            newWidth = Math.Max(MinWidth, Math.Min(MaxWidth, newWidth));

            // 幅の変更が最小単位（1px）以上の場合のみ更新
            if (Math.Abs(newWidth - _lastWidth) >= 1.0)
            {
                SetCurrentValue(CurrentWidthProperty, newWidth);
                _lastWidth = newWidth;
            }
            
            e.Handled = true;
        }
        else
        {
            // マウスカーソルをリサイズカーソルに変更
            SetCurrentValue(CursorProperty, Cursors.SizeWE);
        }

        base.OnMouseMove(e);
    }

    protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
    {
        if (_isResizing)
        {
            _isResizing = false;
            IsPressed = false;
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
        IsPressed = false;
        base.OnLostMouseCapture(e);
    }
}

