// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.


using System.Windows.Controls;

namespace Wpf.Ui.Controls;

/// <summary>
/// Represents an icon that uses an DrawingBrush as its content.
/// </summary>
public class DrawingBrushIcon : IconElement
{
    /// <summary>
    /// Gets or sets <see cref="Icon"/>
    /// </summary>
    public DrawingBrush Icon
    {
        get { return (DrawingBrush)GetValue(IconProperty); }
        set { SetValue(IconProperty, value); }
    }

    /// <summary>Identifies the <see cref="Icon"/> dependency property.</summary>
    public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
        nameof(Icon),
        typeof(DrawingBrush),
        typeof(DrawingBrushIcon),
        new PropertyMetadata(default(DrawingBrush), OnIconChanged));

    private static void OnIconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var self = (DrawingBrushIcon)d;
        if (self.Border is null)
            return;

        self.Border.Background = e.NewValue as DrawingBrush;
    }

    /// <summary>
    /// Gets or sets <see cref="Size"/>
    /// </summary>
    public double Size
    {
        get { return (double)GetValue(SizeProperty); }
        set { SetValue(SizeProperty, value); }
    }

    /// <summary>Identifies the <see cref="Size"/> dependency property.</summary>
    public static readonly DependencyProperty SizeProperty = DependencyProperty.Register(
        nameof(Size),
        typeof(double),
        typeof(DrawingBrushIcon),
        new PropertyMetadata(16.0, OnIconSizeChanged));

    private static void OnIconSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var self = (DrawingBrushIcon)d;
        if (self.Border is null)
        {
            return;
        }

        if (double.TryParse(e.NewValue?.ToString(), out double dblValue))
        {
            self.Border.Width = dblValue;
            self.Border.Height = dblValue;
        }
    }

    protected Border? Border;

    protected override UIElement InitializeChildren()
    {
        Border = new Border()
        {
            HorizontalAlignment = HorizontalAlignment.Stretch,
            Background = Icon,
            Width = Size,
            Height = Size
        };

        Viewbox viewbox = new Viewbox();
        viewbox.Child = Border;

        return viewbox;
    }
}
