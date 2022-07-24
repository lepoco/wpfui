// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
//
// Code from https://github.com/microsoft/microsoft-ui-xaml/
//

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

using Wpf.Ui.Common;

namespace Wpf.Ui.Controls;

public class ColorPickerSlider : Slider
{
    private ToolTip _toolTip;

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

    public ColorPickerSlider()
    {
        ValueChanged += ColorPickerSlider_ValueChanged;
    }

    #region Methods
    #region Private
    #region Class changes event handlers
    private void ColorPickerSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> args)
    {
        if (_toolTip != null)
        {
            _toolTip.Content = GetToolTipString();

            // ToolTip doesn't currently provide any way to re-run its placement logic if its placement target moves,
            // so toggling IsEnabled induces it to do that without incurring any visual glitches.
            _toolTip.IsEnabled = false;
            _toolTip.IsEnabled = true;
        }

        if (TryGetParentColorPicker(out ColorPicker owningColorPicker))
        {
            Color oldColor = owningColorPicker.Color;
            HsvColor hsvColor = oldColor.ToHsvColor();
            hsvColor.Value = args.NewValue / 100.0;
            Color newColor = hsvColor.ToColor();

            //ColorPickerSliderAutomationPeer peer = winrt::FrameworkElementAutomationPeer::FromElement(*this).as< winrt::ColorPickerSliderAutomationPeer > ();
            //get_self<ColorPickerSliderAutomationPeer>(peer)->RaisePropertyChangedEvent(oldColor, newColor, static_cast<int>(round(args.OldValue())), static_cast<int>(round(args.NewValue())));
        }
    }
    #endregion

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
        //int sliderValue = (int)Math.Round(Value);

        //if (ColorChannel == ColorPickerHsvChannel.Alpha)
        //{
        //    return string.Empty;
        //    //return StringUtil::FormatString(ResourceAccessor::GetLocalizedStringResource(SR_ToolTipStringAlphaSlider), sliderValue);
        //}
        //else
        //{
        //    if (TryGetParentColorPicker(out ColorPicker parentColorPicker) /*&& DownlevelHelper::ToDisplayNameExists()*/)
        //    {
        //        HsvColor currentHsv = parentColorPicker.GetCurrentHsv();
        //        string localizedString;

        //        switch (ColorChannel)
        //        {
        //            case ColorPickerHsvChannel.Hue:
        //                currentHsv.Hue = Value;
        //                //localizedString = ResourceAccessor::GetLocalizedStringResource(SR_ToolTipStringHueSliderWithColorName);
        //                break;

        //            case ColorPickerHsvChannel.Saturation:
        //                //localizedString = ResourceAccessor::GetLocalizedStringResource(SR_ToolTipStringSaturationSliderWithColorName);
        //                currentHsv.Saturation = Value / 100;
        //                break;

        //            case ColorPickerHsvChannel.Value:
        //                //localizedString = ResourceAccessor::GetLocalizedStringResource(SR_ToolTipStringValueSliderWithColorName);
        //                currentHsv.Value = Value / 100;
        //                break;
        //            default:
        //                //throw winrt::hresult_error(E_FAIL);
        //                throw new ArgumentException("Invalid ColorPickerHsvChannel value", nameof(ColorChannel));
        //        }

        //        return string.Empty;
        //        //return StringUtil::FormatString(
        //        //    localizedString,
        //        //    sliderValue,
        //        //    winrt::ColorHelper::ToDisplayName(ColorFromRgba(HsvToRgb(currentHsv))).data());
        //    }
        //    else
        //    {
        //        string localizedString;
        //        switch (ColorChannel)
        //        {
        //            case ColorPickerHsvChannel.Hue:
        //                //localizedString = ResourceAccessor::GetLocalizedStringResource(SR_ToolTipStringHueSliderWithoutColorName);
        //                break;
        //            case winrt::ColorPickerHsvChannel::Saturation:
        //                //localizedString = ResourceAccessor::GetLocalizedStringResource(SR_ToolTipStringSaturationSliderWithoutColorName);
        //                break;
        //            case winrt::ColorPickerHsvChannel::Value:
        //                //localizedString = ResourceAccessor::GetLocalizedStringResource(SR_ToolTipStringValueSliderWithoutColorName);
        //                break;
        //            default:
        //                //throw winrt::hresult_error(E_FAIL);
        //                throw new ArgumentException("Invalid ColorPickerHsvChannel value", nameof(ColorChannel));
        //        }

        //        return string.Empty;
        //        //return StringUtil::FormatString(
        //        //    localizedString,
        //        //    sliderValue);
        //    }
        //}

        return string.Empty;
    }
    #endregion

    #region Protected
    protected override void OnGotFocus(RoutedEventArgs args)
    {
        base.OnGotFocus(args);

        _toolTip.Content = GetToolTipString();
        _toolTip.IsEnabled = true;
        _toolTip.IsOpen = true;
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

        _toolTip.IsOpen = false;
    }
    #endregion

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        _toolTip = (ToolTip)GetTemplateChild("ToolTip");

        _toolTip.Content = GetToolTipString();
    } 
    #endregion
}
