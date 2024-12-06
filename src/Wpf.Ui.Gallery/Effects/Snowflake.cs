// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows.Shapes;

namespace Wpf.Ui.Gallery.Effects;

/// <summary>
/// Snowflake data model
/// </summary>
internal class SnowFlake
{
    private Ellipse? _shape;
    private double _x;
    private double _y;
    private double _size;
    private double _speed;
    private double _opacity;
    private double _velX;
    private double _velY;
    private double _stepSize;
    private double _step;
    private double _angle;
    private TranslateTransform? _transform;

    /// <summary>
    /// Gets or sets shape of the snowflake
    /// </summary>
    public Ellipse? Shape
    {
        get => _shape;
        set => _shape = value;
    }

    /// <summary>Gets or sets x position</summary>
    public double X
    {
        get => _x;
        set => _x = value;
    }

    /// <summary>Gets or sets Y position</summary>
    public double Y
    {
        get => _y;
        set => _y = value;
    }

    /// <summary>Gets or sets Size</summary>
    public double Size
    {
        get => _size;
        set => _size = value;
    }

    /// <summary>Gets or sets Falling speed</summary>
    public double Speed
    {
        get => _speed;
        set => _speed = value;
    }

    /// <summary>Gets or sets Opacity</summary>
    public double Opacity
    {
        get => _opacity;
        set => _opacity = value;
    }

    /// <summary>Gets or sets Horizontal velocity</summary>
    public double VelX
    {
        get => _velX;
        set => _velX = value;
    }

    /// <summary>Gets or sets Vertical velocity</summary>
    public double VelY
    {
        get => _velY;
        set => _velY = value;
    }

    /// <summary>Gets or sets Step size</summary>
    public double StepSize
    {
        get => _stepSize;
        set => _stepSize = value;
    }

    /// <summary>Gets or sets Step</summary>
    public double Step
    {
        get => _step;
        set => _step = value;
    }

    /// <summary>Gets or sets Angle</summary>
    public double Angle
    {
        get => _angle;
        set => _angle = value;
    }

    /// <summary>Gets or sets 2D coordinate transformation</summary>
    public TranslateTransform? Transform
    {
        get => _transform;
        set => _transform = value;
    }
}