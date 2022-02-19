// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

// https://docs.microsoft.com/en-us/fluent-ui/web-components/components/progress-ring

using System.Windows;

namespace WPFUI.Controls
{
    /// <summary>
    /// Rotating loading ring.
    /// </summary>
    public class ProgressRing : System.Windows.Controls.Control
    {
        /// <summary>
        /// Property for <see cref="Progress"/>.
        /// </summary>
        public static readonly DependencyProperty ProgressProperty = DependencyProperty.Register(nameof(Progress),
            typeof(double), typeof(ProgressRing),
            new PropertyMetadata(50d, PropertyChangedCallback));

        /// <summary>
        /// Property for <see cref="Thickness"/>.
        /// </summary>
        public static readonly DependencyProperty ThicknessProperty = DependencyProperty.Register(nameof(Thickness),
            typeof(double), typeof(ProgressRing),
            new PropertyMetadata(12d));

        /// <summary>
        /// Property for <see cref="EngAngle"/>.
        /// </summary>
        public static readonly DependencyProperty EngAngleProperty = DependencyProperty.Register(nameof(EngAngle),
            typeof(double), typeof(ProgressRing),
            new PropertyMetadata(180.0d));

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

        /// <summary>
        /// Gets or sets the <see cref="Arc.EndAngle"/>.
        /// </summary>
        public double EngAngle
        {
            get => (double)GetValue(EngAngleProperty);
            set => SetValue(EngAngleProperty, value);
        }

        /// <summary>
        /// Re-draws <see cref="Arc.EndAngle"/> depending on <see cref="Progress"/>.
        /// </summary>
        protected void UpdateProgressAngle()
        {
            EngAngle = (Progress / 100) * 360;
        }

        /// <summary>
        /// Validates the entered <see cref="Progress"/> and redraws the <see cref="Arc"/>.
        /// </summary>
        protected static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not ProgressRing control) return;

            if (control.Progress > 100)
            {
                control.Progress = 100;

                return;
            }

            if (control.Progress < 0)
            {
                control.Progress = 0;

                return;
            }

            control.UpdateProgressAngle();
        }
    }
}