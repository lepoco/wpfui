// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

// https://docs.microsoft.com/en-us/fluent-ui/web-components/components/progress-ring

using System.Windows;

namespace WPFUI.Controls
{
    /// <summary>
    /// Rotating loading icon.
    /// </summary>
    public class ProgressRing : System.Windows.Controls.Control
    {
        /// <summary>
        /// Property for <see cref="Progress"/>.
        /// </summary>
        public static readonly DependencyProperty ProgressProperty = DependencyProperty.Register(nameof(Progress),
            typeof(double), typeof(ProgressRing),
            new PropertyMetadata(8d));

        /// <summary>
        /// Property for <see cref="Thickness"/>.
        /// </summary>
        public static readonly DependencyProperty ThicknessProperty = DependencyProperty.Register(nameof(Thickness),
            typeof(double), typeof(ProgressRing),
            new PropertyMetadata(12d));

        /// <summary>
        /// Property for <see cref="StartPoint"/>.
        /// </summary>
        public static readonly DependencyProperty StartPointProperty = DependencyProperty.Register(nameof(StartPoint),
            typeof(Point), typeof(ProgressRing),
            new PropertyMetadata(new Point()));

        /// <summary>
        /// Property for <see cref="EndPoint"/>.
        /// </summary>
        public static readonly DependencyProperty EndPointProperty = DependencyProperty.Register(nameof(EndPoint),
            typeof(Point), typeof(ProgressRing),
            new PropertyMetadata(new Point()));

        /// <summary>
        /// Gets or sets the progress.
        /// </summary>
        public double Progress
        {
            get => (double)GetValue(ProgressProperty);
            set => SetValue(ProgressProperty, value);
        }

        /// <summary>
        /// Gets or sets the stroke thickness.
        /// </summary>
        public double Thickness
        {
            get => (double)GetValue(ThicknessProperty);
            set => SetValue(ThicknessProperty, value);
        }

        public Point StartPoint
        {
            get => (Point)GetValue(StartPointProperty);
            set => SetValue(StartPointProperty, value);
        }

        public Point EndPoint
        {
            get => (Point)GetValue(EndPointProperty);
            set => SetValue(EndPointProperty, value);
        }

        static ProgressRing()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ProgressRing),
                new FrameworkPropertyMetadata(typeof(ProgressRing)));
        }

        public ProgressRing() : base()
        {
            double y = 36;
            StartPoint = new Point(Thickness / 2, y);
        }

        // Thicknes varied by width?
        // X always (0 + Thickness /2), rotate by other property
    }
}