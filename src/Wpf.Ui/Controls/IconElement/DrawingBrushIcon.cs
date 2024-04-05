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

    protected Border? Border;

    protected override UIElement InitializeChildren()
    {
        Border = new Border()
        {
            Background = Icon,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch,
        };

        Grid grid = new Grid();
        grid.Children.Add(Border);
        return grid;
    }
}
