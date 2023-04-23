// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Wpf.Ui.Controls;

/// <summary>
/// Represents an image with additional proproties for Borders and Rounded corners
/// </summary>
public class Image : Control
{
    #region DependencyPropreties
    /// <summary>
    /// Gets/Sets the Source on this Image.
    /// The Source property is the ImageSource that holds the actual image drawn.
    /// </summary>
    public static readonly DependencyProperty SourceProperty =
        DependencyProperty.Register(nameof(Source), typeof(ImageSource), typeof(Image),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, null, null),
            null);

    /// <summary>
    /// DependencyProperty for CornerRadius property.
    /// </summary>
    public static readonly DependencyProperty CornerRadiusProperty =
        DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(Image), new PropertyMetadata(new CornerRadius(0), new PropertyChangedCallback(OnCornerRadiusChanged)));

    /// <summary>
    /// DependencyProperty for StretchDirection property.
    /// </summary>
    /// <seealso cref="Viewbox.Stretch" />
    public static readonly DependencyProperty StretchProperty = DependencyProperty.Register(
        nameof(Stretch), typeof(Stretch), typeof(Image), new FrameworkPropertyMetadata(Stretch.Uniform, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender), null);

    /// <summary>
    /// DependencyProperty for Stretch property.
    /// </summary>
    public static readonly DependencyProperty StretchDirectionProperty = DependencyProperty.Register(
        nameof(StretchDirection), typeof(StretchDirection), typeof(Image), new FrameworkPropertyMetadata(StretchDirection.Both, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender), null);

    /// <summary>
    /// DependencyPropertyKey for InnerCornerRadius property.
    /// </summary>
    public static readonly DependencyPropertyKey InnerCornerRadiusPropertyKey = DependencyProperty.RegisterReadOnly(
        nameof(InnerCornerRadius), typeof(CornerRadius), typeof(Image), new PropertyMetadata(new CornerRadius(0)));

    /// <summary>
    /// DependencyProperty for InnerCornerRadius property.
    /// </summary>
    public static readonly DependencyProperty InnerCornerRadiusProperty =
    InnerCornerRadiusPropertyKey.DependencyProperty;
    #endregion

    #region Propreties
    /// <summary>
    /// Gets/Sets the Source on this Image.
    /// The Source property is the ImageSource that holds the actual image drawn.
    /// </summary>
    public ImageSource Source
    {
        get => (ImageSource)GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }

    /// <summary>
    /// Gets/Sets the Stretch on this Image.
    /// The Stretch property determines how large the Image will be drawn.
    /// </summary>
    public Stretch Stretch
    {
        get => (Stretch)GetValue(StretchProperty);
        set => SetValue(StretchProperty, value);
    }

    /// <summary>
    /// Gets/Sets the stretch direction of the Viewbox, which determines the restrictions on
    /// scaling that are applied to the content inside the Viewbox.  For instance, this property
    /// can be used to prevent the content from being smaller than its native size or larger than
    /// its native size.
    /// </summary>
    public StretchDirection StretchDirection
    {
        get => (StretchDirection)GetValue(StretchDirectionProperty);
        set => SetValue(StretchDirectionProperty, value);
    }

    /// <summary>
    /// The CornerRadius property allows users to control the roundness of the corners independently by
    /// setting a radius value for each corner.  Radius values that are too large are scaled so that they
    /// smoothly blend from corner to corner.
    /// </summary>
    public CornerRadius CornerRadius
    {
        get => (CornerRadius)GetValue(CornerRadiusProperty);
        set => SetValue(CornerRadiusProperty, value);
    }

    /// <summary>
    /// The CornerRadius for the inner image's Mask.
    /// </summary>
    internal CornerRadius InnerCornerRadius => (CornerRadius)GetValue(InnerCornerRadiusProperty);
    #endregion

    #region Methods
    private static void OnCornerRadiusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var thickness = (Thickness)d.GetValue(BorderThicknessProperty);
        var outerRarius = (CornerRadius)e.NewValue;

        //Inner radius = Outer radius - thickenss/2
        d.SetValue(InnerCornerRadiusPropertyKey,
            new CornerRadius(
                topLeft: Math.Max(0, (int)Math.Round(outerRarius.TopLeft - thickness.Left / 2, 0)),
                topRight: Math.Max(0, (int)Math.Round(outerRarius.TopRight - thickness.Top / 2, 0)),
                bottomRight: Math.Max(0, (int)Math.Round(outerRarius.BottomRight - thickness.Right / 2, 0)),
                bottomLeft: Math.Max(0, (int)Math.Round(outerRarius.BottomLeft - thickness.Bottom / 2, 0)))
        );
    }
    #endregion
}
