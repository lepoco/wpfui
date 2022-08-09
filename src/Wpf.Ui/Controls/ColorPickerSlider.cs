// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

using Lepo.i18n;

using Wpf.Ui.Common.Media;

namespace Wpf.Ui.Controls;

public class ColorPickerSlider : Slider
{
    private ToolTip _toolTip;
    private Track _track;

    #region Dependency properties
    public static readonly DependencyProperty IsThumbToolTipEnabledProperty =
        DependencyProperty.Register(nameof(IsThumbToolTipEnabled), typeof(bool), typeof(ColorPickerSlider),
                                    new PropertyMetadata(false));

    public static readonly DependencyProperty ColorChannelProperty =
        DependencyProperty.Register(nameof(ColorChannel), typeof(ColorPickerHsvChannel), typeof(ColorPickerSlider),
                                    new PropertyMetadata(ColorPickerHsvChannel.Value));

    public static readonly DependencyProperty CornerRadiusProperty =
        DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(ColorPickerSlider),
                                    new PropertyMetadata(new CornerRadius()));

    public static readonly DependencyProperty HeaderTemplateProperty =
        DependencyProperty.Register(nameof(HeaderTemplate), typeof(DataTemplate), typeof(ColorPickerSlider));

    public static readonly DependencyProperty HeaderProperty =
        DependencyProperty.Register(nameof(Header), typeof(object), typeof(ColorPickerSlider),
                                    new PropertyMetadata(defaultValue: null));
    #endregion

    #region Properties
    public bool IsThumbToolTipEnabled
    {
        get => (bool)GetValue(IsThumbToolTipEnabledProperty);
        set => SetValue(IsThumbToolTipEnabledProperty, value);
    }

    public ColorPickerHsvChannel ColorChannel
    {
        get => (ColorPickerHsvChannel)GetValue(ColorChannelProperty);
        set => SetValue(ColorChannelProperty, value);
    }

    public CornerRadius CornerRadius
    {
        get => (CornerRadius)GetValue(CornerRadiusProperty);
        set => SetValue(CornerRadiusProperty, value);
    }

    public DataTemplate HeaderTemplate
    {
        get => (DataTemplate)GetValue(HeaderTemplateProperty);
        set => SetValue(HeaderTemplateProperty, value);
    }

    public object Header
    {
        get => GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    } 
    #endregion

    #region Methods
    #region Private
    private bool TryGetParentColorPicker(out ColorPicker parentColorPicker)
    {
        DependencyObject currentObject = this;
        ColorPicker colorPicker;

        do
        {
            currentObject = VisualTreeHelper.GetParent(currentObject);
            colorPicker = currentObject as ColorPicker;
        }
        while (colorPicker == null && currentObject != null);

        parentColorPicker = colorPicker;

        return parentColorPicker != null;
    }

    private string GetToolTipString()
    {
        int sliderValue = (int)Math.Round(Value);
        string localizedString;

        if (ColorChannel == ColorPickerHsvChannel.Alpha)
        {
            localizedString = Translator.String("toolTipStringAlphaSlider");
            return string.Format(localizedString, sliderValue);
        }
        else if (TryGetParentColorPicker(out ColorPicker parentColorPicker))
        {
            HsvColor currentHsv = parentColorPicker.CurrentHsvColor;

            switch (ColorChannel)
            {
                case ColorPickerHsvChannel.Hue:
                    currentHsv.Hue = Value;
                    localizedString = Translator.String("toolTipStringHueSliderWithColorName");
                    break;

                case ColorPickerHsvChannel.Saturation:
                    currentHsv.Saturation = Value / 100;
                    localizedString = Translator.String("toolTipStringSaturationSliderWithColorName");
                    break;

                case ColorPickerHsvChannel.Value:
                    currentHsv.Value = Value / 100;
                    localizedString = Translator.String("toolTipStringValueSliderWithColorName");
                    break;
                default:
                    throw new ArgumentException("Invalid ColorPickerHsvChannel value", nameof(ColorChannel));
            }

            return string.Format(localizedString, sliderValue, ColorHelpers.GetColorDisplayName(currentHsv.ToColor()));
        }
        else
        {
            switch (ColorChannel)
            {
                case ColorPickerHsvChannel.Hue:
                    localizedString = Translator.String("toolTipStringHueSliderWithoutColorName");
                    break;
                case ColorPickerHsvChannel.Saturation:
                    localizedString = Translator.String("toolTipStringSaturationSliderWithoutColorName");
                    break;
                case ColorPickerHsvChannel.Value:
                    localizedString = Translator.String("toolTipStringValueSliderWithoutColorName");
                    break;
                default:
                    throw new ArgumentException("Invalid ColorPickerHsvChannel value", nameof(ColorChannel));
            }

            return string.Format(localizedString, sliderValue);
        }
    }
    #endregion

    #region Protected
    protected override void OnGotFocus(RoutedEventArgs args)
    {
        base.OnGotFocus(args);

        if (_toolTip != null)
        {
            _toolTip.Content = GetToolTipString();
            _toolTip.IsEnabled = true;
            _toolTip.IsOpen = true;
        }
    }

    protected override void OnKeyDown(KeyEventArgs args)
    {
        if (args.Key != Key.Left && args.Key != Key.Right && args.Key != Key.Up && args.Key != Key.Down)
        {
            base.OnKeyDown(args);
            return;
        }

        if (!TryGetParentColorPicker(out ColorPicker parentColorPicker))
        {
            return;
        }

        bool isControlDown = Keyboard.Modifiers == ModifierKeys.Control;
        double maxBound, minBound;

        HsvColor currentHsvColor = parentColorPicker.CurrentHsvColor;
        double currentAlpha = 0;

        switch (ColorChannel)
        {
            case ColorPickerHsvChannel.Hue:
                minBound = parentColorPicker.MinHue;
                maxBound = parentColorPicker.MaxHue;
                currentHsvColor.Hue = Value;
                break;

            case ColorPickerHsvChannel.Saturation:
                minBound = parentColorPicker.MinSaturation;
                maxBound = parentColorPicker.MaxSaturation;
                currentHsvColor.Saturation = Value / 100;
                break;

            case ColorPickerHsvChannel.Value:
                minBound = parentColorPicker.MinValue;
                maxBound = parentColorPicker.MaxValue;
                currentHsvColor.Value = Value / 100;
                break;

            case ColorPickerHsvChannel.Alpha:
                minBound = 0;
                maxBound = 100;
                currentAlpha = Value / 100;
                break;

            default:
                throw new ArgumentException("Invalid ColorPickerHsvChannel value", nameof(ColorChannel));
        }

        bool shouldInvertHorizontalDirection = FlowDirection == FlowDirection.RightToLeft && !IsDirectionReversed;

        IncrementDirection direction = ((args.Key == Key.Left && !shouldInvertHorizontalDirection) ||
                                        (args.Key == Key.Right && shouldInvertHorizontalDirection) || args.Key == Key.Down)
                                       ? IncrementDirection.Lower
                                       : IncrementDirection.Higher;

        IncrementAmount amount = isControlDown ? IncrementAmount.Large : IncrementAmount.Small;

        if (ColorChannel != ColorPickerHsvChannel.Alpha)
        {
            currentHsvColor = ColorHelpers.IncrementColorChannel(currentHsvColor, ColorChannel, direction, amount,
                                                                 false, minBound, maxBound);
        }
        else
        {
            currentAlpha = ColorHelpers.IncrementAlphaChannel(currentAlpha, direction, amount, false, minBound, maxBound);
        }

        switch (ColorChannel)
        {
            case ColorPickerHsvChannel.Hue:
                Value = currentHsvColor.Hue;
                break;

            case ColorPickerHsvChannel.Saturation:
                Value = currentHsvColor.Saturation * 100;
                break;

            case ColorPickerHsvChannel.Value:
                Value = currentHsvColor.Value * 100;
                break;

            case ColorPickerHsvChannel.Alpha:
                Value = currentAlpha * 100;
                break;

            default:
                //MUX_ASSERT(false);
                Debug.Assert(false);
                break;
        }

        args.Handled = true;
    }

    protected override void OnLostFocus(RoutedEventArgs args)
    {
        base.OnLostFocus(args);

        if (_toolTip != null)
        {
            _toolTip.IsOpen = false;
        }
    }

    protected override void OnValueChanged(double oldValue, double newValue)
    {
        base.OnValueChanged(oldValue, newValue);

        if (_toolTip != null)
        {
            _toolTip.Content = GetToolTipString();

            var rect = new Rect(_track.ActualWidth * (Value / 100) - (_toolTip.ActualWidth / 2), -10, 0, 0);
            _toolTip.PlacementRectangle = rect;
        }
    }
    #endregion

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        _toolTip = (ToolTip)GetTemplateChild("ToolTip");
        _track = (Track)GetTemplateChild("PART_Track");

        if (_toolTip != null)
        {
            _toolTip.Content = GetToolTipString();
        }
    } 
    #endregion
}
