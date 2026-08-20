// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Shapes;

using Wpf.Ui.Markup;

namespace Wpf.Ui.Controls;

/// <summary>
/// Displays vector path data as an IconElement.
/// </summary>
public class VectorIcon : IconElement
{
    private Path? _fgPath;
    private Path? _bgPath;

    public VectorIcon() { }

    public VectorIcon(string pathData, Brush? brush = null)
        : this()
    {
        Data = Geometry.Parse(pathData);
        Foreground = brush ?? (Brush)UiApplication.Current.Resources[ThemeResource.TextFillColorPrimaryBrush];
    }

    /// <summary>
    /// The vector path geometry being rendered.
    /// </summary>
    [TypeConverter(typeof(GeometryConverter))]
    public Geometry Data
    {
        get => (Geometry)GetValue(DataProperty);
        set => SetValue(DataProperty, value);
    }

    /// <summary>Identifies the <see cref="Data"/> dependency property.</summary>
    public static readonly DependencyProperty DataProperty =
        DependencyProperty.Register(
            nameof(Data),
            typeof(Geometry),
            typeof(VectorIcon),
            new FrameworkPropertyMetadata(
                Geometry.Empty,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnDataChanged
            )
        );

    /// <summary>
    /// Determines how the icon scales inside the available bounds.
    /// </summary>
    public Stretch Stretch
    {
        get => (Stretch)GetValue(StretchProperty);
        set => SetValue(StretchProperty, value);
    }

    /// <summary>Identifies the <see cref="Stretch"/> dependency property.</summary>
    public static readonly DependencyProperty StretchProperty =
        DependencyProperty.Register(
            nameof(Stretch),
            typeof(Stretch),
            typeof(VectorIcon),
            new FrameworkPropertyMetadata(
                Stretch.Uniform,
                FrameworkPropertyMetadataOptions.AffectsRender
            )
        );

    /// <inheritdoc cref="Control.Background"/>
    [Bindable(true)]
    [Category("Brush")]
    public Brush Background
    {
        get => (Brush)GetValue(BackgroundProperty);
        set => SetValue(BackgroundProperty, value);
    }

    /// <summary>Identifies the <see cref="Background"/> dependency property.</summary>
    public static readonly DependencyProperty BackgroundProperty = TextElement.BackgroundProperty.AddOwner(
        typeof(VectorIcon),
        new FrameworkPropertyMetadata(
            Brushes.Transparent,
            FrameworkPropertyMetadataOptions.Inherits,
            static (d, args) => ((VectorIcon)d).OnBackgroundChanged(args)
        )
    );

    // Called when Foreground changes
    protected override void OnForegroundChanged(DependencyPropertyChangedEventArgs args)
    {
        _fgPath?.SetCurrentValue(Shape.FillProperty, Foreground);
    }

    // Called when Background changes
    protected void OnBackgroundChanged(DependencyPropertyChangedEventArgs args)
    {
        _bgPath?.SetCurrentValue(Shape.FillProperty, Background);
    }

    private static void OnDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not VectorIcon icon)
        {
            return;
        }

        var g = (Geometry)e.NewValue;
        icon._bgPath?.SetCurrentValue(Path.DataProperty, ToPathGeometryWithFillRule(g, FillRule.Nonzero));
        icon._fgPath?.SetCurrentValue(Path.DataProperty, ToPathGeometryWithFillRule(g, FillRule.EvenOdd));
    }

    /// <summary>
    /// Called once to create the inner visual.
    /// </summary>
    protected override UIElement InitializeChildren()
    {
        _bgPath = new Path
        {
            Stretch = Stretch,
            Fill = Background,
            Data = ToPathGeometryWithFillRule(Data, FillRule.Nonzero), // solid silhouette
            SnapsToDevicePixels = true
        };

        _fgPath = new Path
        {
            Stretch = Stretch,
            Fill = Foreground,
            Data = ToPathGeometryWithFillRule(Data, FillRule.EvenOdd), // preserves holes
            SnapsToDevicePixels = true
        };

        var grid = new Grid { SnapsToDevicePixels = true };
        grid.Children.Add(_bgPath);
        grid.Children.Add(_fgPath);
        return grid;
    }

    private static PathGeometry ToPathGeometryWithFillRule(Geometry geometry, FillRule fillRule)
    {
        // Converts *any* Geometry into a PathGeometry you can control.
        var pg = PathGeometry.CreateFromGeometry(geometry);
        pg.FillRule = fillRule;

        // Optional: freezing is nice if you won't mutate it further
        if (pg.CanFreeze)
        {
            pg.Freeze();
        }

        return pg;
    }
}