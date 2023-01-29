using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Wpf.Ui.Controls;

public class Image : Control
{
	public static readonly DependencyProperty SourceProperty =
		DependencyProperty.Register(nameof(Source), typeof(ImageSource), typeof(Image),
			new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, null, null),
			null);

	public static readonly DependencyProperty CornerRadiusProperty =
		DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(Image), new PropertyMetadata(new CornerRadius(0), new PropertyChangedCallback(OnCornerRadiusChanged)));

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

	public static readonly DependencyProperty StretchProperty = DependencyProperty.Register(
		nameof(Stretch), typeof(Stretch), typeof(Image), new FrameworkPropertyMetadata(Stretch.Uniform, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender), null);

	public static readonly DependencyProperty StretchDirectionProperty = DependencyProperty.Register(
		nameof(StretchDirection), typeof(StretchDirection), typeof(Image), new FrameworkPropertyMetadata(StretchDirection.Both, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender), null);

	//Corner radius for inner Image
	public static readonly DependencyPropertyKey InnerCornerRadiusPropertyKey = DependencyProperty.RegisterReadOnly(
		nameof(InnerCornerRadius), typeof(CornerRadius), typeof(Image), new PropertyMetadata(new CornerRadius(0)));

	public static readonly DependencyProperty InnerCornerRadiusProperty =
		InnerCornerRadiusPropertyKey.DependencyProperty;



	//Propreties
	public ImageSource Source
	{
		get => (ImageSource)GetValue(SourceProperty);
		set => SetValue(SourceProperty, value);
	}

	public Stretch Stretch
	{
		get => (Stretch)GetValue(StretchProperty);
		set => SetValue(StretchProperty, value);
	}

	public StretchDirection StretchDirection
	{
		get => (StretchDirection)GetValue(StretchDirectionProperty);
		set => SetValue(StretchDirectionProperty, value);
	}

	public CornerRadius CornerRadius
	{
		get => (CornerRadius)GetValue(CornerRadiusProperty);
		set => SetValue(CornerRadiusProperty, value);
	}
	public CornerRadius InnerCornerRadius => (CornerRadius)GetValue(InnerCornerRadiusProperty);
}