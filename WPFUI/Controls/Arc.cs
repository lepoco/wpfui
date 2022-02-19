// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;
using System.Windows.Media;

namespace WPFUI.Controls
{
    /// <summary>
    /// Control that draws a symmetrical arc with rounded edges.
    /// </summary>
    public class Arc : System.Windows.Shapes.Shape
    {
        /// <summary>
        /// Property for <see cref="StartAngle"/>.
        /// </summary>
        public static readonly DependencyProperty StartAngleProperty =
            DependencyProperty.Register(nameof(StartAngle), typeof(double), typeof(Arc),
                new PropertyMetadata(0.0d, PropertyChangedCallback));

        /// <summary>
        /// Property for <see cref="EndAngle"/>.
        /// </summary>
        public static readonly DependencyProperty EndAngleProperty =
            DependencyProperty.Register(nameof(EndAngle), typeof(double), typeof(Arc),
                new PropertyMetadata(0.0d, PropertyChangedCallback));

        /// <summary>
        /// Gets or sets the initial angle from which the arc will be drawn.
        /// </summary>
        public double StartAngle
        {
            get => (double)GetValue(StartAngleProperty);
            set => SetValue(StartAngleProperty, value);
        }

        /// <summary>
        /// Gets or sets the final angle from which the arc will be drawn.
        /// </summary>
        public double EndAngle
        {
            get => (double)GetValue(EndAngleProperty);
            set => SetValue(EndAngleProperty, value);
        }

        /// <inheritdoc />
        protected override Geometry DefiningGeometry => GetDefiningGeometry();

        /// <summary>
        /// Overrides default properties.
        /// </summary>
        static Arc()
        {
            StrokeStartLineCapProperty.OverrideMetadata(
                forType: typeof(Arc),
                typeMetadata: new(
                    defaultValue: PenLineCap.Round));

            StrokeEndLineCapProperty.OverrideMetadata(
                forType: typeof(Arc),
                typeMetadata: new(
                    defaultValue: PenLineCap.Round));
        }

        /// <summary>
        /// Get the geometry that defines this shape.
        /// </summary>
        protected Geometry GetDefiningGeometry()
        {
            var geometryStream = new StreamGeometry();

            return geometryStream;
        }

        /// <summary>
        /// Event triggered when one of the key parameters is changed. Forces the geometry to be redrawn.
        /// </summary>
        protected static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not Arc control) return;

            // Force complete new layout pass
            control.InvalidateVisual();
        }
    }
}