// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.


using System.Windows.Controls;

namespace Wpf.Ui.Controls;
public class DrawingBrushIcon : IconElement
{
    public DrawingBrush Icon
    {
        get { return (DrawingBrush)GetValue(IconProperty); }
        set { SetValue(IconProperty, value); }
    }

    public static readonly DependencyProperty IconProperty =
        DependencyProperty.Register("Icon", typeof(DrawingBrush), typeof(DrawingBrushIcon), new PropertyMetadata(default(DrawingBrush), OnIconChanged));

    private static void OnIconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var self = (DrawingBrushIcon)d;
        if (self.Border is null)
            return;

        self.Border.Background = e.NewValue as DrawingBrush;
    }

    public double Size
    {
        get { return (double)GetValue(SizeProperty); }
        set { SetValue(SizeProperty, value); }
    }

    // Using a DependencyProperty as the backing store for IconSize.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty SizeProperty =
        DependencyProperty.Register("Size", typeof(double), typeof(DrawingBrushIcon), new PropertyMetadata(16.0, OnIconSizeChanged));

    private static void OnIconSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var self = (DrawingBrushIcon)d;
        if (self.Border is null)
            return;

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
