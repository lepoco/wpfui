// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows.Controls;
using System.Windows.Shapes;

namespace Wpf.Ui.Gallery.Effects;

/// <summary>
/// Snow effect where the snowflakes are blown away by the mouse
/// </summary>
internal class SnowflakeEffect
{
    private readonly Canvas _canvas; // Canvas for displaying snowflakes
    private readonly Random _random = new(); // Random number generator
    private readonly List<SnowFlake> _snowFlakes = []; // Stores all snowflake objects
    private readonly int _flakeCount; // Number of snowflakes
    private double mX = -100; // Mouse X-coordinate, default value -100
    private double mY = -100; // Mouse Y-coordinate, default value -100

    /// <summary>
    /// Initializes a new instance of the <see cref="SnowflakeEffect"/> class.
    /// </summary>
    /// <param name="canvas">The canvas where the effect is applied.</param>
    /// <param name="flakeCount">The number of snowflakes.</param>
    public SnowflakeEffect(Canvas canvas, int flakeCount = 188)
    {
        _canvas = canvas;
        _flakeCount = flakeCount;
        InitSnowFlakes();

        if (_canvas.Parent is FrameworkElement parentElement)
        {
            parentElement.MouseMove += OnMouseMove;
            parentElement.SizeChanged += OnSizeChanged;
        }
    }

    /// <summary>
    /// Starts displaying the snowflake effect
    /// </summary>
    public void Start()
    {
        CompositionTarget.Rendering += UpdateSnowFlakes;
    }

    /// <summary>
    /// Stops displaying the snowflake effect and cleans up resources
    /// </summary>
    public void Stop()
    {
        CompositionTarget.Rendering -= UpdateSnowFlakes;
        ClearSnowFlakes();

        if (_canvas.Parent is FrameworkElement parentElement)
        {
            parentElement.MouseMove -= OnMouseMove;
            parentElement.SizeChanged -= OnSizeChanged;
        }

        _canvas.Children.Clear();
    }

    /// <summary>
    /// Initializes snowflake objects
    /// </summary>
    private void InitSnowFlakes()
    {
        for (int i = 0; i < _flakeCount; i++)
        {
            CreateSnowFlake();
        }
    }

    /// <summary>
    /// Creates a single snowflake and adds it to the canvas
    /// </summary>
    private void CreateSnowFlake()
    {
        double size = (_random.NextDouble() * 3) + 2; // Snowflake size
        double speed = (_random.NextDouble() * 1) + 0.5; // Falling speed
        double opacity = (_random.NextDouble() * 0.5) + 0.3; // Opacity
        double x = _random.NextDouble() * _canvas.ActualWidth; // Initial X position
        double y = _random.NextDouble() * _canvas.ActualHeight; // Initial Y position

        Ellipse flakeShape = new()
        {
            Width = size,
            Height = size,
            Fill = new SolidColorBrush(Color.FromArgb((byte)(opacity * 255), 255, 255, 255)),
        };

        TranslateTransform transform = new(x, y);
        flakeShape.RenderTransform = transform;

        _ = _canvas.Children.Add(flakeShape);

        SnowFlake flake = new()
        {
            Shape = flakeShape,
            X = x,
            Y = y,
            Size = size,
            Speed = speed,
            Opacity = opacity,
            VelX = 0,
            VelY = speed,
            StepSize = _random.NextDouble() / 30 * 1,
            Step = 0,
            Angle = 180,
            Transform = transform,
        };

        _snowFlakes.Add(flake);
    }

    /// <summary>
    /// Updates the position of snowflakes to respond to mouse movements
    /// </summary>
    private void UpdateSnowFlakes(object? sender, EventArgs e)
    {
        if (_canvas.ActualWidth == 0 || _canvas.ActualHeight == 0)
        {
            return;
        }

        foreach (SnowFlake flake in _snowFlakes)
        {
            double x = mX;
            double y = mY;
            double minDist = 150;
            double x2 = flake.X;
            double y2 = flake.Y;

            double dist = Math.Sqrt(((x2 - x) * (x2 - x)) + ((y2 - y) * (y2 - y)));

            if (dist < minDist)
            {
                double force = minDist / (dist * dist);
                double xcomp = (x - x2) / dist;
                double ycomp = (y - y2) / dist;
                double deltaV = force / 2;

                flake.VelX -= deltaV * xcomp;
                flake.VelY -= deltaV * ycomp;
            }
            else
            {
                flake.VelX *= 0.98;
                if (flake.VelY <= flake.Speed)
                {
                    flake.VelY = flake.Speed;
                }

                flake.VelX += Math.Cos(flake.Step += 0.05) * flake.StepSize;
            }

            flake.Y += flake.VelY;
            flake.X += flake.VelX;

            if (flake.Y >= _canvas.ActualHeight || flake.Y <= 0)
            {
                ResetFlake(flake);
            }

            if (flake.X >= _canvas.ActualWidth || flake.X <= 0)
            {
                ResetFlake(flake);
            }

            flake.Transform!.SetCurrentValue(TranslateTransform.XProperty, flake.X);
            flake.Transform!.SetCurrentValue(TranslateTransform.YProperty, flake.Y);
        }
    }

    /// <summary>
    /// Resets the position and properties of a snowflake when it moves out of view
    /// </summary>
    private void ResetFlake(SnowFlake flake)
    {
        flake.X = _random.NextDouble() * _canvas.ActualWidth;
        flake.Y = 0;
        flake.Size = (_random.NextDouble() * 3) + 2;
        flake.Speed = (_random.NextDouble() * 1) + 0.5;
        flake.VelY = flake.Speed;
        flake.VelX = 0;
        flake.Opacity = (_random.NextDouble() * 0.5) + 0.3;

        if (flake.Shape == null)
        {
            return;
        }

        flake.Shape.SetCurrentValue(FrameworkElement.WidthProperty, flake.Size);
        flake.Shape.SetCurrentValue(FrameworkElement.HeightProperty, flake.Size);
        flake.Shape.SetCurrentValue(
            Shape.FillProperty,
            new SolidColorBrush(Color.FromArgb((byte)(flake.Opacity * 255), 255, 255, 255))
        );
    }

    /// <summary>
    /// Cleans up all snowflakes, used when stopping the effect
    /// </summary>
    private void ClearSnowFlakes()
    {
        foreach (SnowFlake flake in _snowFlakes)
        {
            _canvas.Children.Remove(flake.Shape);
        }

        _snowFlakes.Clear();
    }

    /// <summary>
    /// Mouse move event handler, updates mouse position
    /// </summary>
    private void OnMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
    {
        Point position = e.GetPosition(_canvas);
        mX = position.X;
        mY = position.Y;
    }

    /// <summary>
    /// Canvas size change event handler, updates canvas dimensions
    /// </summary>
    private void OnSizeChanged(object sender, SizeChangedEventArgs e)
    {
        _canvas.SetCurrentValue(FrameworkElement.WidthProperty, e.NewSize.Width);
        _canvas.SetCurrentValue(FrameworkElement.HeightProperty, e.NewSize.Height);
    }
}
