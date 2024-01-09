// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows.Controls.Primitives;
using System.Windows.Markup;

namespace Wpf.Ui.Controls;

[DefaultProperty(nameof(Content))]
[ContentProperty(nameof(Content))]
public class Gauge : RangeBase
{
    /// <summary>Identifies the <see cref="StartAngle"/> dependency property.</summary>
    public static readonly DependencyProperty StartAngleProperty = DependencyProperty.Register(
        nameof(StartAngle),
        typeof(double),
        typeof(Gauge),
        new FrameworkPropertyMetadata(0D, FrameworkPropertyMetadataOptions.AffectsRender)
    );

    /// <summary>Identifies the <see cref="EndAngle"/> dependency property.</summary>
    public static readonly DependencyProperty EndAngleProperty = DependencyProperty.Register(
        nameof(EndAngle),
        typeof(double),
        typeof(Gauge),
        new FrameworkPropertyMetadata(0D, FrameworkPropertyMetadataOptions.AffectsRender)
    );

    /// <summary>Identifies the <see cref="Thickness"/> dependency property.</summary>
    public static readonly DependencyProperty ThicknessProperty = DependencyProperty.Register(
        nameof(Thickness),
        typeof(double),
        typeof(Gauge),
        new FrameworkPropertyMetadata(10D, FrameworkPropertyMetadataOptions.AffectsRender)
    );

    /// <summary>Identifies the <see cref="Content"/> dependency property.</summary>
    public static readonly DependencyProperty ContentProperty = DependencyProperty.Register(
        nameof(Content),
        typeof(object),
        typeof(Gauge),
        new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender)
    );

    /// <summary>Identifies the <see cref="Header"/> dependency property.</summary>
    public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
        nameof(Header),
        typeof(object),
        typeof(Gauge),
        new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender)
    );

    /// <summary>Identifies the <see cref="StartLineCap"/> dependency property.</summary>
    public static readonly DependencyProperty StartLineCapProperty = DependencyProperty.Register(
        nameof(StartLineCap),
        typeof(PenLineCap),
        typeof(Gauge),
        new FrameworkPropertyMetadata(PenLineCap.Flat, FrameworkPropertyMetadataOptions.AffectsRender)
    );

    /// <summary>Identifies the <see cref="EndLineCap"/> dependency property.</summary>
    public static readonly DependencyProperty EndLineCapProperty = DependencyProperty.Register(
        nameof(EndLineCap),
        typeof(PenLineCap),
        typeof(Gauge),
        new FrameworkPropertyMetadata(PenLineCap.Flat, FrameworkPropertyMetadataOptions.AffectsRender)
    );

    /// <summary>Identifies the <see cref="DashCap"/> dependency property.</summary>
    public static readonly DependencyProperty DashCapProperty = DependencyProperty.Register(
        nameof(DashCap),
        typeof(PenLineCap),
        typeof(Gauge),
        new FrameworkPropertyMetadata(PenLineCap.Flat, FrameworkPropertyMetadataOptions.AffectsRender)
    );

    /// <summary>Identifies the <see cref="DashArray"/> dependency property.</summary>
    public static readonly DependencyProperty DashArrayProperty = DependencyProperty.Register(
        nameof(DashArray),
        typeof(DoubleCollection),
        typeof(Gauge),
        new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender)
    );

    /// <summary>Identifies the <see cref="DashOffset"/> dependency property.</summary>
    public static readonly DependencyProperty DashOffsetProperty = DependencyProperty.Register(
        nameof(DashOffset),
        typeof(double),
        typeof(Gauge),
        new FrameworkPropertyMetadata(0D, FrameworkPropertyMetadataOptions.AffectsRender)
    );

    /// <summary>Identifies the <see cref="LineJoin"/> dependency property.</summary>
    public static readonly DependencyProperty LineJoinProperty = DependencyProperty.Register(
        nameof(LineJoin),
        typeof(PenLineJoin),
        typeof(Gauge),
        new FrameworkPropertyMetadata(PenLineJoin.Miter, FrameworkPropertyMetadataOptions.AffectsRender)
    );

    /// <summary>Identifies the <see cref="MiterLimit"/> dependency property.</summary>
    public static readonly DependencyProperty MiterLimitProperty = DependencyProperty.Register(
        nameof(MiterLimit),
        typeof(double),
        typeof(Gauge),
        new FrameworkPropertyMetadata(0D, FrameworkPropertyMetadataOptions.AffectsRender)
    );

    /// <summary>Identifies the <see cref="Indicator"/> dependency property.</summary>
    public static readonly DependencyProperty IndicatorProperty = DependencyProperty.Register(
        nameof(Indicator),
        typeof(Brush),
        typeof(Gauge),
        new FrameworkPropertyMetadata(Brushes.Transparent, FrameworkPropertyMetadataOptions.AffectsRender)
    );

    /// <summary>Identifies the <see cref="IndicatorStartLineCap"/> dependency property.</summary>
    public static readonly DependencyProperty IndicatorStartLineCapProperty = DependencyProperty.Register(
        nameof(IndicatorStartLineCap),
        typeof(PenLineCap),
        typeof(Gauge),
        new FrameworkPropertyMetadata(PenLineCap.Flat, FrameworkPropertyMetadataOptions.AffectsRender)
    );

    /// <summary>Identifies the <see cref="IndicatorEndLineCap"/> dependency property.</summary>
    public static readonly DependencyProperty IndicatorEndLineCapProperty = DependencyProperty.Register(
        nameof(IndicatorEndLineCap),
        typeof(PenLineCap),
        typeof(Gauge),
        new FrameworkPropertyMetadata(PenLineCap.Flat, FrameworkPropertyMetadataOptions.AffectsRender)
    );

    /// <summary>Identifies the <see cref="IndicatorDashCap"/> dependency property.</summary>
    public static readonly DependencyProperty IndicatorDashCapProperty = DependencyProperty.Register(
        nameof(IndicatorDashCap),
        typeof(PenLineCap),
        typeof(Gauge),
        new FrameworkPropertyMetadata(PenLineCap.Flat, FrameworkPropertyMetadataOptions.AffectsRender)
    );

    /// <summary>Identifies the <see cref="IndicatorDashArray"/> dependency property.</summary>
    public static readonly DependencyProperty IndicatorDashArrayProperty = DependencyProperty.Register(
        nameof(IndicatorDashArray),
        typeof(DoubleCollection),
        typeof(Gauge),
        new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender)
    );

    /// <summary>Identifies the <see cref="IndicatorDashOffset"/> dependency property.</summary>
    public static readonly DependencyProperty IndicatorDashOffsetProperty = DependencyProperty.Register(
        nameof(IndicatorDashOffset),
        typeof(double),
        typeof(Gauge),
        new FrameworkPropertyMetadata(0D, FrameworkPropertyMetadataOptions.AffectsRender)
    );

    /// <summary>Identifies the <see cref="IndicatorLineJoin"/> dependency property.</summary>
    public static readonly DependencyProperty IndicatorLineJoinProperty = DependencyProperty.Register(
        nameof(IndicatorLineJoin),
        typeof(PenLineJoin),
        typeof(Gauge),
        new FrameworkPropertyMetadata(PenLineJoin.Miter, FrameworkPropertyMetadataOptions.AffectsRender)
    );

    /// <summary>Identifies the <see cref="IndicatorMiterLimit"/> dependency property.</summary>
    public static readonly DependencyProperty IndicatorMiterLimitProperty = DependencyProperty.Register(
        nameof(IndicatorMiterLimit),
        typeof(double),
        typeof(Gauge),
        new FrameworkPropertyMetadata(0D, FrameworkPropertyMetadataOptions.AffectsRender)
    );

    /// <summary>
    /// Gets or sets the initial angle from which the gauge will be drawn.
    /// </summary>
    [Category("Common")]
    [Description("Gets or sets the initial angle from which the gauge will be drawn.")]
    [DefaultValue(0D)]
    [TypeConverter(typeof(LengthConverter))]
    public double StartAngle
    {
        get => (double)GetValue(StartAngleProperty);
        set => SetValue(StartAngleProperty, value);
    }

    /// <summary>
    /// Gets or sets the final angle from which the gauge will be drawn.
    /// </summary>
    [Category("Common")]
    [Description("Gets or sets the final angle from which the gauge will be drawn.")]
    [DefaultValue(0D)]
    [TypeConverter(typeof(LengthConverter))]
    public double EndAngle
    {
        get => (double)GetValue(EndAngleProperty);
        set => SetValue(EndAngleProperty, value);
    }

    /// <summary>
    /// Gets or sets the thickness that will be used to draw the gauge.
    /// </summary>
    [Category("Common")]
    [Description("Gets or sets the thickness that will be used to draw the gauge.")]
    [DefaultValue(10D)]
    [TypeConverter(typeof(LengthConverter))]
    public double Thickness
    {
        get => (double)GetValue(ThicknessProperty);
        set => SetValue(ThicknessProperty, value);
    }

    /// <summary>
    /// Gets or sets the content within the gauge.
    /// </summary>
    [Category("Common")]
    [Description("Gets or sets the content within the gauge.")]
    [DefaultValue(null)]
    public object? Content
    {
        get => GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
    }

    /// <summary>
    /// Gets or sets the header within the gauge.
    /// </summary>
    [Category("Common")]
    [Description("Gets or sets the header within the gauge.")]
    [DefaultValue(null)]
    public object? Header
    {
        get => GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }

    /// <summary>
    /// Gets or sets the <see cref="PenLineCap"/> to use at the start of the bar.
    /// </summary>
    [Category("GaugeBar")]
    [Description("Gets or sets the PenLineCap to use at the start of the bar.")]
    [DefaultValue(PenLineCap.Flat)]
    public PenLineCap StartLineCap
    {
        get => (PenLineCap)GetValue(StartLineCapProperty);
        set => SetValue(StartLineCapProperty, value);
    }

    /// <summary>
    /// Gets or sets the <see cref="PenLineCap"/> to use at the end of the bar.
    /// </summary>
    [Category("GaugeBar")]
    [Description("Gets or sets the PenLineCap to use at the end of the bar.")]
    [DefaultValue(PenLineCap.Flat)]
    public PenLineCap EndLineCap
    {
        get => (PenLineCap)GetValue(EndLineCapProperty);
        set => SetValue(EndLineCapProperty, value);
    }

    /// <summary>
    /// Gets or sets the <see cref="PenLineCap"/> to use for the dashes of the bar.
    /// </summary>
    [Category("GaugeBar")]
    [Description("Gets or sets the PenLineCap to use for the dashes of the bar.")]
    [DefaultValue(PenLineCap.Flat)]
    public PenLineCap DashCap
    {
        get => (PenLineCap)GetValue(DashCapProperty);
        set => SetValue(DashCapProperty, value);
    }

    /// <summary>
    /// Gets or sets the sheme of dashes and gaps that will be used within the bar.
    /// </summary>
    [Category("GaugeBar")]
    [Description("Gets or sets the sheme of dashes and gaps that will be used within the bar.")]
    [DefaultValue(null)]
    [TypeConverter(typeof(DoubleCollectionConverter))]
    public DoubleCollection? DashArray
    {
        get => (DoubleCollection?)GetValue(DashArrayProperty);
        set => SetValue(DashArrayProperty, value);
    }

    /// <summary>
    /// Gets or sets the distance within the dash pattern where a dash begins.
    /// </summary>
    [Category("GaugeBar")]
    [Description("Gets or sets the distance within the dash pattern where a dash begins.")]
    [DefaultValue(0D)]
    [TypeConverter(typeof(LengthConverter))]
    public double DashOffset
    {
        get => (double)GetValue(DashOffsetProperty);
        set => SetValue(DashOffsetProperty, value);
    }

    /// <summary>
    /// Gets or sets the shape that joins two lines or segments of the bar.
    /// </summary>
    [Category("GaugeBar")]
    [Description("Gets or sets the shape that joins two lines or segments of the bar.")]
    [DefaultValue(PenLineJoin.Miter)]
    public PenLineJoin LineJoin
    {
        get => (PenLineJoin)GetValue(LineJoinProperty);
        set => SetValue(LineJoinProperty, value);
    }

    /// <summary>
    /// Gets or sets the limit on the ratio of the miter length to half the thickness of the bar.
    /// </summary>
    [Category("GaugeBar")]
    [Description("Gets or sets the limit on the ratio of the miter length to half the thickness of the bar.")]
    [DefaultValue(0D)]
    [TypeConverter(typeof(LengthConverter))]
    public double MiterLimit
    {
        get => (double)GetValue(MiterLimitProperty);
        set => SetValue(MiterLimitProperty, value);
    }

    /// <summary>
    /// Gets or sets the brush that will be used to draw the indicator.
    /// </summary>
    [Description("Gets or sets the brush that will be used to draw the indicator.")]
    public Brush Indicator
    {
        get => (Brush)GetValue(IndicatorProperty);
        set => SetValue(IndicatorProperty, value);
    }

    /// <summary>
    /// Gets or sets the <see cref="PenLineCap"/> to use at the start of the indicator.
    /// </summary>
    [Category("GaugeIndicator")]
    [Description("Gets or sets the PenLineCap to use at the start of the indicator.")]
    [DefaultValue(PenLineCap.Flat)]
    public PenLineCap IndicatorStartLineCap
    {
        get => (PenLineCap)GetValue(IndicatorStartLineCapProperty);
        set => SetValue(IndicatorStartLineCapProperty, value);
    }

    /// <summary>
    /// Gets or sets the <see cref="PenLineCap"/> to use at the end of the indicator.
    /// </summary>
    [Category("GaugeIndicator")]
    [Description("Gets or sets the PenLineCap to use at the end of the indicator.")]
    [DefaultValue(PenLineCap.Flat)]
    public PenLineCap IndicatorEndLineCap
    {
        get => (PenLineCap)GetValue(IndicatorEndLineCapProperty);
        set => SetValue(IndicatorEndLineCapProperty, value);
    }

    /// <summary>
    /// Gets or sets the <see cref="PenLineCap"/> to use for the dashes of the indicator.
    /// </summary>
    [Category("GaugeIndicator")]
    [Description("Gets or sets the PenLineCap to use for the dashes of the indicator.")]
    [DefaultValue(PenLineCap.Flat)]
    public PenLineCap IndicatorDashCap
    {
        get => (PenLineCap)GetValue(IndicatorDashCapProperty);
        set => SetValue(IndicatorDashCapProperty, value);
    }

    /// <summary>
    /// Gets or sets the sheme of dashes and gaps that will be used within the indicator.
    /// </summary>
    [Category("GaugeIndicator")]
    [Description("Gets or sets the sheme of dashes and gaps that will be used within the indicator.")]
    [DefaultValue(null)]
    [TypeConverter(typeof(DoubleCollectionConverter))]
    public DoubleCollection? IndicatorDashArray
    {
        get => (DoubleCollection?)GetValue(IndicatorDashArrayProperty);
        set => SetValue(IndicatorDashArrayProperty, value);
    }

    /// <summary>
    /// Gets or sets the distance within the dash pattern where a dash begins.
    /// </summary>
    [Category("GaugeIndicator")]
    [Description("Gets or sets the distance within the dash pattern where a dash begins.")]
    [DefaultValue(0D)]
    [TypeConverter(typeof(LengthConverter))]
    public double IndicatorDashOffset
    {
        get => (double)GetValue(IndicatorDashOffsetProperty);
        set => SetValue(IndicatorDashOffsetProperty, value);
    }

    /// <summary>
    /// Gets or sets the shape that joins two lines or segments of the indicator.
    /// </summary>
    [Category("GaugeIndicator")]
    [Description("Gets or sets the shape that joins two lines or segments of the indicator.")]
    [DefaultValue(PenLineJoin.Miter)]
    public PenLineJoin IndicatorLineJoin
    {
        get => (PenLineJoin)GetValue(IndicatorLineJoinProperty);
        set => SetValue(IndicatorLineJoinProperty, value);
    }

    /// <summary>
    /// Gets or sets the limit on the ratio of the miter length to half the thickness of the indicator.
    /// </summary>
    [Category("GaugeIndicator")]
    [Description("Gets or sets the limit on the ratio of the miter length to half the thickness of the indicator.")]
    [DefaultValue(0D)]
    [TypeConverter(typeof(LengthConverter))]
    public double IndicatorMiterLimit
    {
        get => (double)GetValue(IndicatorMiterLimitProperty);
        set => SetValue(IndicatorMiterLimitProperty, value);
    }

    /// <summary>Initializes static members of the <see cref="Gauge"/> class. Overrides default properties. </summary>
    static Gauge()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(Gauge), new FrameworkPropertyMetadata(typeof(Gauge)));

        _ = MinimumProperty.AddOwner(typeof(Gauge), new FrameworkPropertyMetadata(0D, FrameworkPropertyMetadataOptions.AffectsRender));
        _ = MaximumProperty.AddOwner(typeof(Gauge), new FrameworkPropertyMetadata(100D, FrameworkPropertyMetadataOptions.AffectsRender));
        _ = ValueProperty.AddOwner(typeof(Gauge), new FrameworkPropertyMetadata(0D, FrameworkPropertyMetadataOptions.AffectsRender));

        _ = ForegroundProperty.AddOwner(typeof(Gauge), new FrameworkPropertyMetadata(Brushes.Transparent, FrameworkPropertyMetadataOptions.AffectsRender));
        _ = BackgroundProperty.AddOwner(typeof(Gauge), new FrameworkPropertyMetadata(Brushes.Transparent, FrameworkPropertyMetadataOptions.AffectsRender));
    }

    /// <inheritdoc/>
    protected override void OnRender(DrawingContext drawingContext)
    {
        // Background
        Pen backgroundPen = new()
        {
            Brush = Background,
            Thickness = Math.Abs(Thickness),

            StartLineCap = StartLineCap,
            EndLineCap = EndLineCap,
            DashCap = DashCap,

            LineJoin = LineJoin,
            MiterLimit = MiterLimit,
        };

        if (DashArray is not null)
        {
            backgroundPen.DashStyle = new DashStyle(DashArray, DashOffset);
        }

        drawingContext.DrawGeometry(Background, backgroundPen, GetArcGeometry(EndAngle));

        // Foreground
        Pen gaugePen = new()
        {
            Brush = Indicator,
            Thickness = Math.Abs(Thickness),

            StartLineCap = IndicatorStartLineCap,
            EndLineCap = IndicatorEndLineCap,
            DashCap = IndicatorDashCap,

            LineJoin = IndicatorLineJoin,
            MiterLimit = IndicatorMiterLimit,
        };

        if (IndicatorDashArray is not null)
        {
            gaugePen.DashStyle = new DashStyle(IndicatorDashArray, IndicatorDashOffset);
        }

        drawingContext.DrawGeometry(Indicator, gaugePen, GetArcGeometry(GetAngleForValue()));

        base.OnRender(drawingContext);
    }

    /// <summary>
    /// Gets the geometry of the gauge.
    /// </summary>
    /// <remarks>Based on the <see cref="Arc.GetDefiningGeometry"/> method.</remarks>
    /// <param name="endAngle">The angle to draw the arc to.</param>
    /// <returns>The geometry of the gauge.</returns>
    private StreamGeometry GetArcGeometry(double endAngle)
    {
        StreamGeometry streamGeometry = new();
        using StreamGeometryContext geometryContext = streamGeometry.Open();

        geometryContext.BeginFigure(
            startPoint: GetPointAtAngle(Math.Min(StartAngle, endAngle)),
            isFilled: false,
            isClosed: false);

        geometryContext.ArcTo(
            point: GetPointAtAngle(Math.Max(StartAngle, endAngle)),
            size: new Size(Math.Max(0, (RenderSize.Width - Thickness) / 2), Math.Max(0, (RenderSize.Height - Thickness) / 2)),
            rotationAngle: 0,
            isLargeArc: Math.Abs(endAngle - StartAngle) > 180,
            sweepDirection: SweepDirection.Counterclockwise,
            isStroked: true,
            isSmoothJoin: false);

        streamGeometry.Transform = new TranslateTransform(Thickness / 2, Thickness / 2);

        return streamGeometry;
    }

    /// <summary>
    /// Gets the point at the given angle.
    /// </summary>
    /// <remarks>Based on the<see cref="Arc.PointAtAngle(Double)"/> method.</remarks>
    /// <param name="angle">The angle to get the point at.</param>
    /// <returns>The point at the given angle.</returns>
    protected Point GetPointAtAngle(double angle)
    {
        var radAngle = angle * (Math.PI / 180);
        var xRadius = (RenderSize.Width - Thickness) / 2;
        var yRadius = (RenderSize.Height - Thickness) / 2;

        return new(xRadius + (xRadius * Math.Cos(radAngle)), yRadius - (yRadius * Math.Sin(radAngle)));
    }

    /// <summary>
    /// Gets the angle for the current value.
    /// </summary>
    /// <returns>The angle for the current value.</returns>
    protected double GetAngleForValue()
    {
        var endAngle = Math.Abs(EndAngle);
        var minValue = Math.Abs(Minimum);
        var normalizedValue = (Math.Abs(Value) - minValue) / (Math.Abs(Maximum) - minValue);

        return ((1 - normalizedValue) * (Math.Abs(StartAngle) + endAngle)) - endAngle;
    }
}
