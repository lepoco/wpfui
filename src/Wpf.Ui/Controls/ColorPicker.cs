// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
//
// Code from https://github.com/microsoft/microsoft-ui-xaml/
//

using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Shapes;

using Wpf.Ui.Common;

using WindowsTextBox = System.Windows.Controls.TextBox;

namespace Wpf.Ui.Controls;

public class ColorPicker : Control
{
    #region Fields
    private bool _textEntryGridOpened;
    private bool _isFocusedTextBoxValid;
    private bool _updatingColor;
    private bool _updatingControls;

    private CancellationTokenSource _alphaSliderCheckeredBackgroundCancellationTokenSource;
    private CancellationTokenSource _colorPreviewRectangleCheckeredBackgroundCancellationTokenSource;

    private Color _currentColor = Color.FromRgb(255, 255, 255);

    private HsvColor _currentHsvColor = new HsvColor(0, 1, 1, 255);

    private ImageBrush _alphaSliderCheckeredBackgroundImageBrush;

    private LinearGradientBrush _alphaSliderGradientBrush;
    private LinearGradientBrush _thirdDimensionSliderGradientBrush;

    private SolidColorBrush _checkerColorBrush;

    private string _currentColorHexCode = "#FFFFFFFF";
    private string _previousString = "";

    #region Template parts
    private ButtonBase _moreButton;

    private ColorPickerSlider _alphaSlider;
    private ColorPickerSlider _thirdDimensionSlider;

    private ComboBox _colorRepresentationComboBox;

    private ComboBoxItem _rgbComboBoxItem;
    private ComboBoxItem _hsvComboBoxItem;

    private ColorSpectrum _colorSpectrum;

    private Grid _colorPreviewRectangleGrid;

    private ImageBrush _colorPreviewRectangleCheckeredBackgroundImageBrush;

    private Rectangle _alphaSliderBackgroundRectangle;
    private Rectangle _colorPreviewRectangle;
    private Rectangle _previousColorRectangle;

    private TextBlock _alphaLabel;
    private TextBlock _blueLabel;
    private TextBlock _greenLabel;
    private TextBlock _hueLabel;
    private TextBlock _moreButtonLabel;
    private TextBlock _redLabel;
    private TextBlock _saturationLabel;
    private TextBlock _valueLabel;

    private WindowsTextBox _alphaTextBox;
    private WindowsTextBox _blueTextBox;
    private WindowsTextBox _greenTextBox;
    private WindowsTextBox _hexTextBox;
    private WindowsTextBox _hueTextBox;
    private WindowsTextBox _redTextBox;
    private WindowsTextBox _saturationTextBox;
    private WindowsTextBox _valueTextBox;
    #endregion
    #endregion

    #region Dependency properties
    internal static readonly DependencyProperty IsTextEntryGridVisibleProperty =
        DependencyProperty.Register(nameof(IsTextEntryGridVisible), typeof(bool), typeof(ColorPicker),
                                    new PropertyMetadata(true));

    internal static readonly DependencyProperty PreviousColorVisibilityProperty =
    DependencyProperty.Register(nameof(PreviousColorVisibility), typeof(Visibility), typeof(ColorPicker),
                                new PropertyMetadata(Visibility.Visible));

    public static readonly DependencyProperty IsAlphaEnabledProperty =
        DependencyProperty.Register(nameof(IsAlphaEnabled), typeof(bool), typeof(ColorPicker),
                                    new PropertyMetadata(false));

    public static readonly DependencyProperty IsAlphaSliderVisibleProperty =
        DependencyProperty.Register(nameof(IsAlphaSliderVisible), typeof(bool), typeof(ColorPicker),
                                    new PropertyMetadata(true));

    public static readonly DependencyProperty IsAlphaTextInputVisibleProperty =
        DependencyProperty.Register(nameof(IsAlphaTextInputVisible), typeof(bool), typeof(ColorPicker),
                                    new PropertyMetadata(true));

    public static readonly DependencyProperty IsColorChannelTextInputVisibleProperty =
        DependencyProperty.Register(nameof(IsColorChannelTextInputVisible), typeof(bool), typeof(ColorPicker),
                                    new PropertyMetadata(true));

    public static readonly DependencyProperty IsColorPreviewVisibleProperty =
        DependencyProperty.Register(nameof(IsColorPreviewVisible), typeof(bool), typeof(ColorPicker),
                                    new PropertyMetadata(true));

    public static readonly DependencyProperty IsColorSliderVisibleProperty =
        DependencyProperty.Register(nameof(IsColorSliderVisible), typeof(bool), typeof(ColorPicker),
                                    new PropertyMetadata(true));

    public static readonly DependencyProperty IsColorSpectrumVisibleProperty =
        DependencyProperty.Register(nameof(IsColorSpectrumVisible), typeof(bool), typeof(ColorPicker),
                                    new PropertyMetadata(true));

    public static readonly DependencyProperty IsHexInputVisibleProperty =
        DependencyProperty.Register(nameof(IsHexInputVisible), typeof(bool), typeof(ColorPicker),
                                    new PropertyMetadata(true));

    public static readonly DependencyProperty IsMoreButtonVisibleProperty =
        DependencyProperty.Register(nameof(IsMoreButtonVisible), typeof(bool), typeof(ColorPicker),
                                    new PropertyMetadata(false));

    public static readonly DependencyProperty ColorProperty =
        DependencyProperty.Register(nameof(Color), typeof(Color), typeof(ColorPicker),
                                    new PropertyMetadata(Color.FromArgb(255, 255, 255, 255)));

    public static readonly DependencyProperty PreviousColorProperty =
        DependencyProperty.Register(nameof(PreviousColor), typeof(Color?), typeof(ColorPicker),
                                    new PropertyMetadata(defaultValue: null));

    public static readonly DependencyProperty ColorSpectrumComponentsProperty =
        DependencyProperty.Register(nameof(ColorSpectrumComponents), typeof(ColorSpectrumComponents), typeof(ColorPicker),
                                    new PropertyMetadata(ColorSpectrumComponents.HueSaturation));

    public static readonly DependencyProperty ColorSpectrumShapeProperty =
        DependencyProperty.Register(nameof(ColorSpectrumShape), typeof(ColorSpectrumShape), typeof(ColorPicker),
                                    new PropertyMetadata(ColorSpectrumShape.Box));

    public static readonly DependencyProperty CornerRadiusProperty =
        DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(ColorPicker),
                                    new PropertyMetadata(new CornerRadius()));

    public static readonly DependencyProperty MaxHueProperty =
        DependencyProperty.Register(nameof(MaxHue), typeof(int), typeof(ColorPicker),
                                    new PropertyMetadata(359));

    public static readonly DependencyProperty MaxSaturationProperty =
        DependencyProperty.Register(nameof(MaxSaturation), typeof(int), typeof(ColorPicker),
                                    new PropertyMetadata(100));

    public static readonly DependencyProperty MaxValueProperty =
        DependencyProperty.Register(nameof(MaxValue), typeof(int), typeof(ColorPicker),
                                    new PropertyMetadata(100));

    public static readonly DependencyProperty MinHueProperty =
        DependencyProperty.Register(nameof(MinHue), typeof(int), typeof(ColorPicker),
                                    new PropertyMetadata(default(int)));

    public static readonly DependencyProperty MinSaturationProperty =
        DependencyProperty.Register(nameof(MinSaturation), typeof(int), typeof(ColorPicker),
                                    new PropertyMetadata(default(int)));

    public static readonly DependencyProperty MinValueProperty =
        DependencyProperty.Register(nameof(MinValue), typeof(int), typeof(ColorPicker),
                                    new PropertyMetadata(default(int)));

    public static readonly DependencyProperty OrientationProperty =
        DependencyProperty.Register(nameof(Orientation), typeof(Orientation), typeof(ColorPicker),
                                    new PropertyMetadata(Orientation.Vertical)); 
    #endregion

    public event ColorChangedEventHandler<ColorPicker> ColorChanged;

    #region Properties
    internal bool IsTextEntryGridVisible
    {
        get => (bool)GetValue(IsTextEntryGridVisibleProperty);
        set => SetValue(IsTextEntryGridVisibleProperty, value);
    }

    internal Visibility PreviousColorVisibility
    {
        get => (Visibility)GetValue(PreviousColorVisibilityProperty);
        set => SetValue(PreviousColorVisibilityProperty, value);
    }

    public HsvColor CurrentHsvColor => _currentHsvColor;

    public bool IsAlphaEnabled
    {
        get => (bool)GetValue(IsAlphaEnabledProperty);
        set => SetValue(IsAlphaEnabledProperty, value);
    }

    public bool IsAlphaSliderVisible
    {
        get => (bool)GetValue(IsAlphaSliderVisibleProperty);
        set => SetValue(IsAlphaSliderVisibleProperty, value);
    }

    public bool IsAlphaTextInputVisible
    {
        get => (bool)GetValue(IsAlphaTextInputVisibleProperty);
        set => SetValue(IsAlphaTextInputVisibleProperty, value);
    }

    public bool IsColorChannelTextInputVisible
    {
        get => (bool)GetValue(IsColorChannelTextInputVisibleProperty);
        set => SetValue(IsColorChannelTextInputVisibleProperty, value);
    }

    public bool IsColorPreviewVisible
    {
        get => (bool)GetValue(IsColorPreviewVisibleProperty);
        set => SetValue(IsColorPreviewVisibleProperty, value);
    }

    public bool IsColorSliderVisible
    {
        get => (bool)GetValue(IsColorSliderVisibleProperty);
        set => SetValue(IsColorSliderVisibleProperty, value);
    }

    public bool IsColorSpectrumVisible
    {
        get => (bool)GetValue(IsColorSpectrumVisibleProperty);
        set => SetValue(IsColorSpectrumVisibleProperty, value);
    }

    public bool IsHexInputVisible
    {
        get => (bool)GetValue(IsHexInputVisibleProperty);
        set => SetValue(IsHexInputVisibleProperty, value);
    }

    public bool IsMoreButtonVisible
    {
        get => (bool)GetValue(IsMoreButtonVisibleProperty);
        set => SetValue(IsMoreButtonVisibleProperty, value);
    }

    public Color Color
    {
        get => (Color)GetValue(ColorProperty);
        set => SetValue(ColorProperty, value);
    }

    public Color? PreviousColor
    {
        get => (Color?)GetValue(PreviousColorProperty);
        set => SetValue(PreviousColorProperty, value);
    }

    public CornerRadius CornerRadius
    {
        get => (CornerRadius)GetValue(CornerRadiusProperty);
        set => SetValue(CornerRadiusProperty, value);
    }

    public ColorSpectrumComponents ColorSpectrumComponents
    {
        get => (ColorSpectrumComponents)GetValue(ColorSpectrumComponentsProperty);
        set => SetValue(ColorSpectrumComponentsProperty, value);
    }

    public ColorSpectrumShape ColorSpectrumShape
    {
        get => (ColorSpectrumShape)GetValue(ColorSpectrumShapeProperty);
        set => SetValue(ColorSpectrumShapeProperty, value);
    }

    public int MaxHue
    {
        get => (int)GetValue(MaxHueProperty);
        set => SetValue(MaxHueProperty, value);
    }

    public int MaxSaturation
    {
        get => (int)GetValue(MaxSaturationProperty);
        set => SetValue(MaxSaturationProperty, value);
    }

    public int MaxValue
    {
        get => (int)GetValue(MaxValueProperty);
        set => SetValue(MaxValueProperty, value);
    }

    public int MinHue
    {
        get => (int)GetValue(MinHueProperty);
        set => SetValue(MinHueProperty, value);
    }

    public int MinSaturation
    {
        get => (int)GetValue(MinSaturationProperty);
        set => SetValue(MinSaturationProperty, value);
    }

    public int MinValue
    {
        get => (int)GetValue(MinValueProperty);
        set => SetValue(MinValueProperty, value);
    }

    public Orientation Orientation
    {
        get => (Orientation)GetValue(OrientationProperty);
        set => SetValue(OrientationProperty, value);
    } 
    #endregion

    public ColorPicker()
    {
        Unloaded += ColorPicker_Unloaded;
    }    

    #region Methods
    #region Private
    #region Dependendy property changes event handlers
    private void OnColorChanged(DependencyPropertyChangedEventArgs args)
    {
        // If we're in the process of internally updating the color, then we don't want to respond to the Color property changing,
        // aside from raising the ColorChanged event.
        if (!_updatingColor)
        {
            Color color = Color;

            _currentColor = color;
            _currentHsvColor = _currentColor.ToHsvColor();
            _currentColorHexCode = GetCurrentColorHexCode();

            UpdateColorControls(ColorUpdateReason.ColorPropertyChanged);
        }

        Color oldColor = (Color)args.OldValue;
        Color newColor = (Color)args.NewValue;

        if (oldColor != newColor)
        {
            ColorChanged?.Invoke(this, new ColorChangedEventArgs(newColor, oldColor));
        }
    }

    private void OnColorSpectrumComponentsChanged(DependencyPropertyChangedEventArgs args)
    {
        UpdateThirdDimensionSlider();
        SetThirdDimensionSliderChannel();
    }

    private void OnIsAlphaEnabledChanged(DependencyPropertyChangedEventArgs args)
    {
        _currentColorHexCode = GetCurrentColorHexCode();

        if (_hexTextBox != null)
        {
            _updatingControls = true;
            _hexTextBox.Text = _currentColorHexCode;
            _updatingControls = false;
        }


        OnPartVisibilityChanged(args);
    }
    
    private void OnMinMaxHueChanged(DependencyPropertyChangedEventArgs args)
    {
        int minHue = MinHue;
        int maxHue = MaxHue;

        if (minHue < 0 || minHue > 359)
        {
            throw new ArgumentException("MinHue must be between 0 and 359", nameof(MinHue));
        }
        else if (maxHue < 0 || maxHue > 359)
        {
            throw new ArgumentException("MaxHue must be between 0 and 359", nameof(MaxHue));
        }

        _currentHsvColor.Hue = Math.Max(minHue, Math.Min(_currentHsvColor.Hue, maxHue));

        UpdateColor(_currentHsvColor, ColorUpdateReason.ColorPropertyChanged);
        UpdateThirdDimensionSlider();
    }
    
    private void OnMinMaxSaturationChanged(DependencyPropertyChangedEventArgs args)
    {
        int minSaturation = MinSaturation;
        int maxSaturation = MaxSaturation;

        if (minSaturation < 0 || minSaturation > 100)
        {
            throw new ArgumentException("MinSaturation must be between 0 and 100", nameof(MinSaturation));
        }
        else if (maxSaturation < 0 || maxSaturation > 100)
        {
            throw new ArgumentException("MaxSaturation must be between 0 and 100", nameof(MaxSaturation));
        }

        _currentHsvColor.Saturation = Math.Max(minSaturation / 100, Math.Min(_currentHsvColor.Saturation, maxSaturation / 100));

        UpdateColor(_currentHsvColor, ColorUpdateReason.ColorPropertyChanged);
        UpdateThirdDimensionSlider();
    }
    private void OnMinMaxValueChanged(DependencyPropertyChangedEventArgs args)
    {
        int minValue = MinValue;
        int maxValue = MaxValue;

        if (minValue < 0 || minValue > 100)
        {
            throw new ArgumentException("MinValue must be between 0 and 100", nameof(MinValue));
        }
        else if (maxValue < 0 || maxValue > 100)
        {
            throw new ArgumentException("MaxValue must be between 0 and 100", nameof(MaxValue));
        }

        _currentHsvColor.Value = Math.Max(minValue / 100, Math.Min(_currentHsvColor.Value, maxValue / 100));

        UpdateColor(_currentHsvColor, ColorUpdateReason.ColorPropertyChanged);
        UpdateThirdDimensionSlider();
    }

    private void OnOrientationChanged(DependencyPropertyChangedEventArgs args)
    {
        UpdateVisualState(true);
    }

    private void OnPartVisibilityChanged(DependencyPropertyChangedEventArgs args)
    {
        UpdateVisualState(true /* useTransitions */);
    }
    
    private void OnPreviousColorChanged(DependencyPropertyChangedEventArgs args)
    {
        UpdatePreviousColorRectangle();
        UpdateVisualState(true /* useTransitions */);
    }
    #endregion

    #region Class changes event handlers
    private void ColorPicker_Unloaded(object sender, RoutedEventArgs args)
    {
        // If we're in the middle of creating image bitmaps while being unloaded,
        // we'll want to synchronously cancel it so we don't have any asynchronous actions
        // lingering beyond our lifetime.
        _alphaSliderCheckeredBackgroundCancellationTokenSource?.Cancel();
        _colorPreviewRectangleCheckeredBackgroundCancellationTokenSource?.Cancel();
    }

    private void OnCheckerColorChanged(object sender, EventArgs args)
    {
        CreateColorPreviewCheckeredBackground();
        CreateAlphaSliderCheckeredBackground();
    }
    #endregion

    #region Size changes event handlers
    private void OnAlphaSliderBackgroundRectangleSizeChanged(object sender, SizeChangedEventArgs args)
    {
        CreateAlphaSliderCheckeredBackground();
    }

    private void OnColorPreviewRectangleGridSizeChanged(object sender, SizeChangedEventArgs args)
    {
        CreateColorPreviewCheckeredBackground();
    }
    #endregion

    #region ColorSpectrum changes event handlers
    private void OnColorSpectrumColorChanged(ColorSpectrum sender, ColorChangedEventArgs args)
    {
        // If we're updating controls, then this is being raised in response to that,
        // so we'll ignore it.
        if (_updatingControls)
        {
            return;
        }

        var hsvColor = sender.HsvColor;
        hsvColor.Alpha = GetAlphaFromTextBox();

        UpdateColor(hsvColor, ColorUpdateReason.ColorSpectrumColorChanged);
    }

    private void OnColorSpectrumSizeChanged(object sender, SizeChangedEventArgs args)
    {
        // Since the ColorPicker is arranged vertically, the ColorSpectrum's height can be whatever we want it to be -
        // the width is the limiting factor.  Since we want it to always be a square, we'll set its height to whatever its width is.
        if (args.NewSize.Width != args.PreviousSize.Width)
        {
            _colorSpectrum.Height = args.NewSize.Width;
        }
    }
    #endregion

    #region Slider changes event handlers
    private void OnAlphaSliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> args)
    {
        // If we're in the process of updating controls in response to a color change,
        // then we don't want to do anything in response to a control being updated,
        // since otherwise we'll get into an infinite loop of updating.
        if (_updatingControls)
        {
            return;
        }

        UpdateColor((byte)(_alphaSlider.Value / 100 * 255), ColorUpdateReason.AlphaSliderChanged);
    }

    private void OnThirdDimensionSliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> args)
    {
        // If we're in the process of updating controls in response to a color change,
        // then we don't want to do anything in response to a control being updated,
        // since otherwise we'll get into an infinite loop of updating.
        if (_updatingControls)
        {
            return;
        }

        var components = ColorSpectrumComponents;

        byte alpha = _currentHsvColor.Alpha;
        double hue = _currentHsvColor.Hue;
        double saturation = _currentHsvColor.Saturation;
        double value = _currentHsvColor.Value;

        switch (components)
        {
            case ColorSpectrumComponents.HueValue:
            case ColorSpectrumComponents.ValueHue:
                saturation = _thirdDimensionSlider.Value / 100.0;
                break;

            case ColorSpectrumComponents.HueSaturation:
            case ColorSpectrumComponents.SaturationHue:
                value = _thirdDimensionSlider.Value / 100.0;
                break;

            case ColorSpectrumComponents.ValueSaturation:
            case ColorSpectrumComponents.SaturationValue:
                hue = _thirdDimensionSlider.Value;
                break;
        }

        UpdateColor(new HsvColor(hue, saturation, value, alpha), ColorUpdateReason.ThirdDimensionSliderChanged);
    }
    #endregion

    #region More button changes event handlers
    private void OnMoreButtonChecked(object sender, RoutedEventArgs args)
    {
        _textEntryGridOpened = true;
        UpdateMoreButton();
    }

    private void OnMoreButtonClicked(object sender, RoutedEventArgs args)
    {
        _textEntryGridOpened = !_textEntryGridOpened;
        UpdateMoreButton();
    }

    private void OnMoreButtonUnchecked(object sender, RoutedEventArgs args)
    {
        _textEntryGridOpened = false;
        UpdateMoreButton();
    }
    #endregion

    #region Color representation ComboBox event handler
    private void OnColorRepresentationComboBoxSelectionChanged(object sender, SelectionChangedEventArgs args)
    {
        UpdateVisualState(true);
    }
    #endregion

    #region General TextBox event handlers
    private void OnTextBoxGotFocus(object sender, RoutedEventArgs args)
    {
        var textBox = (WindowsTextBox)sender;

        _isFocusedTextBoxValid = true;
        _previousString = textBox.Text;

        if (textBox == _hexTextBox)
        {
            textBox.SelectionStart = textBox.Text.Length;
        }
    }

    private void OnTextBoxLostFocus(object sender, RoutedEventArgs args)
    {
        var textBox = (WindowsTextBox)sender;

        // When a text box loses focus, we want to check whether its contents were valid.
        // If they weren't, then we'll roll back its contents to their last valid value.
        if (!_isFocusedTextBoxValid)
        {
            textBox.Text = _previousString;
        }

        // Now that we know that no text box is currently being edited, we'll update all of the color controls
        // in order to clear away any invalid values currently in any text box.
        UpdateColorControls(ColorUpdateReason.ColorPropertyChanged);
    }
    #endregion

    #region RGB TextBox event handlers
    private void OnRgbTextChanged(object sender, TextChangedEventArgs args)
    {
        // If we're in the process of updating controls in response to a color change,
        // then we don't want to do anything in response to a control being updated,
        // since otherwise we'll get into an infinite loop of updating.
        if (_updatingControls)
        {
            return;
        }

        var textBox = (WindowsTextBox)sender;

        // We'll respond to the text change if the user has entered a valid value.
        // Otherwise, we'll do nothing except mark the text box's contents as invalid.
        if (!int.TryParse(textBox.Text, out var componentValue) || componentValue < 0 || componentValue > 255)
        {
            _isFocusedTextBoxValid = false;
        }
        else
        {
            _isFocusedTextBoxValid = true;
            UpdateColor(ApplyConstraintsToColor(GetRgbColorFromTextBoxes()), ColorUpdateReason.RgbTextBoxChanged);
        }
    }
    #endregion

    #region HSV TextBox event handlers
    private void OnHueTextChanged(object sender, TextChangedEventArgs args)
    {
        // If we're in the process of updating controls in response to a color change,
        // then we don't want to do anything in response to a control being updated,
        // since otherwise we'll get into an infinite loop of updating.
        if (_updatingControls)
        {
            return;
        }

        var textBox = (WindowsTextBox)sender;

        // We'll respond to the text change if the user has entered a valid value.
        // Otherwise, we'll do nothing except mark the text box's contents as invalid.
        if (!int.TryParse(textBox.Text, out var componentValue) || componentValue < MinHue || componentValue > MaxHue)
        {
            _isFocusedTextBoxValid = false;
        }
        else
        {
            _isFocusedTextBoxValid = true;
            UpdateColor(GetHsvColorFromTextBoxes(), ColorUpdateReason.RgbTextBoxChanged);
        }
    }

    private void OnSaturationTextChanged(object sender, TextChangedEventArgs args)
    {
        // If we're in the process of updating controls in response to a color change,
        // then we don't want to do anything in response to a control being updated,
        // since otherwise we'll get into an infinite loop of updating.
        if (_updatingControls)
        {
            return;
        }

        var textBox = (WindowsTextBox)sender;

        // We'll respond to the text change if the user has entered a valid value.
        // Otherwise, we'll do nothing except mark the text box's contents as invalid.
        if (!int.TryParse(textBox.Text, out var componentValue) || componentValue < MinSaturation || componentValue > MaxSaturation)
        {
            _isFocusedTextBoxValid = false;
        }
        else
        {
            _isFocusedTextBoxValid = true;
            UpdateColor(GetHsvColorFromTextBoxes(), ColorUpdateReason.RgbTextBoxChanged);
        }
    }

    private void OnValueTextChanged(object sender, TextChangedEventArgs args)
    {
        // If we're in the process of updating controls in response to a color change,
        // then we don't want to do anything in response to a control being updated,
        // since otherwise we'll get into an infinite loop of updating.
        if (_updatingControls)
        {
            return;
        }

        var textBox = (WindowsTextBox)sender;

        // We'll respond to the text change if the user has entered a valid value.
        // Otherwise, we'll do nothing except mark the text box's contents as invalid.
        if (!int.TryParse(textBox.Text, out var componentValue) || componentValue < MinValue || componentValue > MaxValue)
        {
            _isFocusedTextBoxValid = false;
        }
        else
        {
            _isFocusedTextBoxValid = true;
            UpdateColor(GetHsvColorFromTextBoxes(), ColorUpdateReason.RgbTextBoxChanged);
        }
    }
    #endregion

    #region Alpha TextBox event handler
    private void OnAlphaTextChanged(object sender, TextChangedEventArgs args)
    {
        // If we're in the process of updating controls in response to a color change,
        // then we don't want to do anything in response to a control being updated,
        // since otherwise we'll get into an infinite loop of updating.
        if (_updatingControls)
        {
            return;
        }

        if (_alphaTextBox != null)
        {
            // If the user hasn't entered a %, we'll do that for them, keeping the cursor
            // where it was before.
            int cursorPosition = _alphaTextBox.SelectionStart + _alphaTextBox.SelectionLength;

            var text = _alphaTextBox.Text;
            if (text == string.Empty || !text.EndsWith("%"))
            {
                _alphaTextBox.Text = text + '%';
                _alphaTextBox.SelectionStart = cursorPosition;
            }

            // We'll respond to the text change if the user has entered a valid value.
            // Otherwise, we'll do nothing except mark the text box's contents as invalid.
            //                   _alphaTextBox.Text.Substring(0, _alphaTextBox.Text.Length - 1);
            string alphaPercentageString = _alphaTextBox.Text.Substring(0, _alphaTextBox.Text.Length - 1);
            
            if (!int.TryParse(alphaPercentageString, out int alphaValue) || alphaValue < 0 || alphaValue > 100)
            {
                _isFocusedTextBoxValid = false;
            }
            else
            {
                _isFocusedTextBoxValid = true;
                UpdateColor(GetAlphaFromTextBox(), ColorUpdateReason.AlphaTextBoxChanged);
            }
        }
    }
    #endregion

    #region Hex TextBox event handler
    private void OnHexTextChanged(object sender, TextChangedEventArgs args)
    {
        // If we're in the process of updating controls in response to a color change,
        // then we don't want to do anything in response to a control being updated,
        // since otherwise we'll get into an infinite loop of updating.
        if (_updatingControls)
        {
            return;
        }

        // If the user hasn't entered a #, we'll do that for them, keeping the cursor
        // where it was before.
        var text = _hexTextBox.Text;
        if (text == string.Empty || !text.StartsWith("#"))
        {
            _hexTextBox.Text = '#' + text;
            _hexTextBox.SelectionStart = _hexTextBox.Text.Length;
        }

        // We'll respond to the text change if the user has entered a valid value.
        // Otherwise, we'll do nothing except mark the text box's contents as invalid.
        //auto[rgbValue, alphaValue] = [this, isAlphaEnabled]() {
        //    return isAlphaEnabled ?
        //        HexToRgba(m_hexTextBox.get().Text()) :
        //        std::make_tuple(HexToRgb(m_hexTextBox.get().Text()), 1.0);
        //} ();

        //if (IsAlphaEnabled)
        //{

        _isFocusedTextBoxValid = ColorHelpers.TryGetColorFromHexCode(text, out var color);

        if (_isFocusedTextBoxValid)
        {
            UpdateColor(ApplyConstraintsToColor(color), ColorUpdateReason.HexTextBoxChanged);
        }
    }
    #endregion

    #region Helper functions
    private static void AddGradientStop(LinearGradientBrush brush, double offset, HsvColor hsvColor)
    {
        var gradientStop = new GradientStop(hsvColor.ToColor(), offset);
        brush.GradientStops.Add(gradientStop);
    }

    private byte GetAlphaFromTextBox()
    {
        var alphaPercentageString = _alphaTextBox.Text;
        alphaPercentageString = alphaPercentageString.Substring(0, alphaPercentageString.Length - 1);

        return (byte)(int.Parse(alphaPercentageString) / 100D * 255);
    }

    private Color ApplyConstraintsToColor(Color color)
    {
        double minHue = MinHue;
        double maxHue = MaxHue;
        double minSaturation = MinSaturation / 100.0;
        double maxSaturation = MaxSaturation / 100.0;
        double minValue = MinValue / 100.0;
        double maxValue = MaxValue / 100.0;

        HsvColor hsvColor = color.ToHsvColor();

        hsvColor.Hue = Math.Min(Math.Max(hsvColor.Hue, minHue), maxHue);
        hsvColor.Saturation = Math.Min(Math.Max(hsvColor.Saturation, minSaturation), maxSaturation);
        hsvColor.Value = Math.Min(Math.Max(hsvColor.Value, minValue), maxValue);

        return hsvColor.ToColor();
    }

    private Color GetCheckerColor()
    {
        Color checkerColor;

        if (_checkerColorBrush != null)
        {
            checkerColor = _checkerColorBrush.Color;
        }
        else
        {
            // TODO: Find a replacement for this resource
            //checkerColor = (Color)Application.Current.Resources["SystemListLowColor"];
            checkerColor = Colors.Gray;
        }

        return checkerColor;
    }

    private Color GetRgbColorFromTextBoxes()
    {
        return Color.FromArgb(GetAlphaFromTextBox(), byte.Parse(_redTextBox.Text),
                              byte.Parse(_greenTextBox.Text), byte.Parse(_blueTextBox.Text));
    }

    private HsvColor GetHsvColorFromTextBoxes()
    {
        return new HsvColor(int.Parse(_hueTextBox.Text), int.Parse(_saturationTextBox.Text) / 100.0, int.Parse(_valueTextBox.Text) / 100.0,
                            GetAlphaFromTextBox());
    }

    private string GetCurrentColorHexCode()
    {
        var currentColorHexCode = _currentColor.GetHexCode(IsAlphaEnabled);

        //                                          : '#' + currentColorHexCode.Substring(3, currentColorHexCode.Length - 3);
        return /*IsAlphaEnabled ? */currentColorHexCode /*: '#' + currentColorHexCode[3..]*/;
    }

    private void CreateAlphaSliderCheckeredBackground()
    {
        if (DesignerProperties.GetIsInDesignMode(this))
        {
            return;
        }

        if (_alphaSliderBackgroundRectangle != null && _alphaSliderCheckeredBackgroundImageBrush != null)
        {
            int width = (int)Math.Round(_alphaSliderBackgroundRectangle.ActualWidth);
            int height = (int)Math.Round(_alphaSliderBackgroundRectangle.ActualHeight);

            if (_alphaSliderCheckeredBackgroundCancellationTokenSource != null)
            {
                _alphaSliderCheckeredBackgroundCancellationTokenSource.Cancel();
            }

            _alphaSliderCheckeredBackgroundCancellationTokenSource = new CancellationTokenSource();
            var alphaSliderCheckeredBackgroundTask = ColorHelpers.CreateCheckeredBackgroundAsync(width, height, GetCheckerColor(),
                _alphaSliderCheckeredBackgroundCancellationTokenSource.Token);

            alphaSliderCheckeredBackgroundTask.ContinueWith(t =>
            {
                var checkeredBackgroundBitmap = ColorHelpers.CreateBitmapFromPixelData(width, height, t.Result);

                Dispatcher.Invoke(() => _alphaSliderCheckeredBackgroundImageBrush.ImageSource = checkeredBackgroundBitmap);
            });
            //CreateCheckeredBackgroundAsync(width, height, GetCheckerColor(), bgraCheckeredPixelData,
            //                               _alphaSliderCheckeredBackgroundBitmapAction, m_dispatcherHelper,

            //    [strongThis](winrt::WriteableBitmap checkeredBackgroundSoftwareBitmap)
            //    {
            //    strongThis->m_alphaSliderCheckeredBackgroundImageBrush.get().ImageSource(checkeredBackgroundSoftwareBitmap);
            //});
        }
    }

    private void CreateColorPreviewCheckeredBackground()
    {
        if (DesignerProperties.GetIsInDesignMode(this))
        {
            return;
        }

        if (_colorPreviewRectangleGrid != null && _colorPreviewRectangleCheckeredBackgroundImageBrush != null)
        {
            int width = (int)Math.Round(_colorPreviewRectangleGrid.ActualWidth);
            int height = (int)Math.Round(_colorPreviewRectangleGrid.ActualHeight);

            if (_colorPreviewRectangleCheckeredBackgroundCancellationTokenSource != null)
            {
                _colorPreviewRectangleCheckeredBackgroundCancellationTokenSource.Cancel();
            }

            _colorPreviewRectangleCheckeredBackgroundCancellationTokenSource = new CancellationTokenSource();
            var colorPreviewRectangleCheckeredBackgroundTask = ColorHelpers.CreateCheckeredBackgroundAsync(width, height,
                GetCheckerColor(), _colorPreviewRectangleCheckeredBackgroundCancellationTokenSource.Token);

            colorPreviewRectangleCheckeredBackgroundTask.ContinueWith(t =>
            {
                var checkeredBackgroundBitmap = ColorHelpers.CreateBitmapFromPixelData(width, height, t.Result);

                Dispatcher.Invoke(() => _colorPreviewRectangleCheckeredBackgroundImageBrush.ImageSource = checkeredBackgroundBitmap);
            });
            //CreateCheckeredBackgroundAsync(width, height, GetCheckerColor(), bgraCheckeredPixelData,
            //    m_createColorPreviewRectangleCheckeredBackgroundBitmapAction,
            //    m_dispatcherHelper,

            //    [strongThis](winrt::WriteableBitmap checkeredBackgroundSoftwareBitmap)
            //    {
            //    strongThis->m_colorPreviewRectangleCheckeredBackgroundImageBrush.get().ImageSource(checkeredBackgroundSoftwareBitmap);
            //});
        }
    }

    private void InitializeColor()
    {
        var color = Color;

        _currentColor = color;
        _currentHsvColor = _currentColor.ToHsvColor();
        _currentColorHexCode = GetCurrentColorHexCode();

        SetColorAndUpdateControls(ColorUpdateReason.InitializingColor);
    }

    private void SetColorAndUpdateControls(ColorUpdateReason reason)
    {
        _updatingColor = true;

        Color = _currentColor;
        UpdateColorControls(reason);

        _updatingColor = false;
    }

    private void SetThirdDimensionSliderChannel()
    {
        if (_thirdDimensionSlider != null)
        {
            switch (ColorSpectrumComponents)
            {
                case ColorSpectrumComponents.ValueSaturation:
                case ColorSpectrumComponents.SaturationValue:
                    _thirdDimensionSlider.ColorChannel = ColorPickerHsvChannel.Hue;
                    //winrt::AutomationProperties::SetName(thirdDimensionSlider, ResourceAccessor::GetLocalizedStringResource(SR_AutomationNameHueSlider));
                    break;

                case ColorSpectrumComponents.HueValue:
                case ColorSpectrumComponents.ValueHue:
                    _thirdDimensionSlider.ColorChannel = ColorPickerHsvChannel.Saturation;
                    //winrt::AutomationProperties::SetName(thirdDimensionSlider, ResourceAccessor::GetLocalizedStringResource(SR_AutomationNameSaturationSlider));
                    break;

                case ColorSpectrumComponents.HueSaturation:
                case ColorSpectrumComponents.SaturationHue:
                    _thirdDimensionSlider.ColorChannel = ColorPickerHsvChannel.Value;
                    //AutomationProperties.SetName(thirdDimensionSlider, ResourceAccessor::GetLocalizedStringResource(SR_AutomationNameValueSlider));
                    break;
            }
        }
    }

    private void UpdateAlphaSlider()
    {
        if (_alphaSlider == null || _alphaSliderGradientBrush == null)
        {
            return;
        }

        // Since the slider changes only one color dimension, we can use a LinearGradientBrush
        // for its background instead of needing to manually set pixels ourselves.
        // We'll have the gradient go between the minimum and maximum values in the case where
        // the slider handles saturation or value, or in the case where it handles hue,
        // we'll have it go between red, yellow, green, cyan, blue, and purple, in that order.
        _alphaSliderGradientBrush.GradientStops.Clear();

        _alphaSlider.Minimum = 0;
        _alphaSlider.Maximum = 100;
        _alphaSlider.Value = _currentColor.A / 255D * 100;

        ColorPicker.AddGradientStop(_alphaSliderGradientBrush, 0.0, _currentHsvColor);
        ColorPicker.AddGradientStop(_alphaSliderGradientBrush, 1.0, _currentHsvColor);
    }

    private void UpdateColor(byte alpha, ColorUpdateReason reason)
    {
        _currentColor.A = alpha;
        _currentHsvColor.Alpha = alpha;
        _currentColorHexCode = GetCurrentColorHexCode();

        SetColorAndUpdateControls(reason);
    }

    private void UpdateColor(Color color, ColorUpdateReason reason)
    {
        _currentColor = color;
        _currentHsvColor = _currentColor.ToHsvColor();
        _currentColorHexCode = GetCurrentColorHexCode();

        SetColorAndUpdateControls(reason);
    }

    private void UpdateColor(HsvColor hsvColor, ColorUpdateReason reason)
    {
        _currentHsvColor = hsvColor;
        _currentColor = hsvColor.ToColor();
        _currentColorHexCode = GetCurrentColorHexCode();

        SetColorAndUpdateControls(reason);
    }

    private void UpdateColorControls(ColorUpdateReason reason)
    {
        // If we're updating the controls internally, we don't want to execute any of the controls'
        // event handlers, because that would then update the color, which would update the color controls,
        // and then we'd be in an infinite loop.
        _updatingControls = true;

        // We pass in the reason why we're updating the color controls because
        // we don't want to re-update any control that was the cause of this update.
        // For example, if a user selected a color on the ColorSpectrum, then we
        // don't want to update the ColorSpectrum's color based on this change.
        if (reason != ColorUpdateReason.ColorSpectrumColorChanged && _colorSpectrum != null)
        {
            _colorSpectrum.HsvColor = _currentHsvColor;
        }

        if (_colorPreviewRectangle != null)
        {
            var color = Color;

            _colorPreviewRectangle.Fill = new SolidColorBrush(color);
        }


        if (reason != ColorUpdateReason.ThirdDimensionSliderChanged && _thirdDimensionSlider != null)
        {
            UpdateThirdDimensionSlider();
        }

        if (reason != ColorUpdateReason.AlphaSliderChanged && _alphaSlider != null)
        {
            UpdateAlphaSlider();
        }

        /*if (SharedHelpers::IsRS2OrHigher())
        {
            // A reentrancy bug with setting TextBox.Text was fixed in RS2,
            // so we can just directly set the TextBoxes' Text property there.
            updateTextBoxes();
        }
        else*/ if (!DesignerProperties.GetIsInDesignMode(this))
        {
            // Otherwise, we need to post this to the dispatcher to avoid that reentrancy bug.
            // TODO: Not sure is this applies for WPF, but let's invoke it asynchronously anyway
            Dispatcher.BeginInvoke(() =>
            {
                _updatingControls = true;
                updateTextBoxes();
                _updatingControls = false;
            });
            //m_dispatcherHelper.RunAsync([strongThis, updateTextBoxes]()
            //{
            //    strongThis->m_updatingControls = true;
            //    updateTextBoxes();
            //    strongThis->m_updatingControls = false;
            //});
        }

        _updatingControls = false;

        #region "updateTextBoxes" local function
        void updateTextBoxes()
        {
            if (reason != ColorUpdateReason.RgbTextBoxChanged)
            {
                if (_redTextBox != null)
                {
                    _redTextBox.Text = _currentColor.R.ToString();
                }

                if (_greenTextBox != null)
                {
                    _greenTextBox.Text = _currentColor.G.ToString();
                }

                if (_blueTextBox != null)
                {
                    _blueTextBox.Text = _currentColor.B.ToString();
                }
            }

            if (reason != ColorUpdateReason.HsvTextBoxChanged)
            {
                if (_hueTextBox != null)
                {
                    _hueTextBox.Text = ((int)Math.Round(_currentHsvColor.Hue)).ToString();
                }

                if (_saturationTextBox != null)
                {
                    _saturationTextBox.Text = ((int)Math.Round(_currentHsvColor.Saturation * 100)).ToString();
                }

                if (_valueTextBox != null)
                {
                    _valueTextBox.Text = ((int)Math.Round(_currentHsvColor.Value * 100)).ToString();
                }
            }


            if (reason != ColorUpdateReason.AlphaTextBoxChanged)
            {
                if (_alphaTextBox != null)
                {
                    _alphaTextBox.Text = ((int)Math.Round((double)_currentColor.A / 255 * 100)).ToString() + '%';
                }
            }

            if (reason != ColorUpdateReason.HexTextBoxChanged)
            {
                if (_hexTextBox != null)
                {
                    _hexTextBox.Text = _currentColorHexCode;
                }
            }

        }; 
        #endregion
    }

    private void UpdateMoreButton()
    {
        if (_moreButtonLabel != null)
        {
            _moreButtonLabel.Text = _textEntryGridOpened ? "More" : "Less";
            //moreButtonLabel.Text(ResourceAccessor::GetLocalizedStringResource(m_textEntryGridOpened ? SR_TextMoreButtonLabelExpanded : SR_TextMoreButtonLabelCollapsed));
        }
        else if (_moreButton != null)
        {
            _moreButton.Content = _textEntryGridOpened ? "More" : "Less";
            //winrt::AutomationProperties::SetName(moreButton, ResourceAccessor::GetLocalizedStringResource(m_textEntryGridOpened ? SR_AutomationNameMoreButtonExpanded : SR_AutomationNameMoreButtonCollapsed));
        }


        UpdateVisualState(true /* useTransitions */);
    }

    private void UpdatePreviousColorRectangle()
    {
        if (_previousColorRectangle != null)
        {
            if (PreviousColor is Color previousColor)
            {
                _previousColorRectangle.Fill = new SolidColorBrush(previousColor);
            }
            else
            {
                _previousColorRectangle.Fill = null;
            }
        }
    }

    private void UpdateThirdDimensionSlider()
    {
        if (_thirdDimensionSlider == null || _thirdDimensionSliderGradientBrush == null)
        {
            return;
        }

        // Since the slider changes only one color dimension, we can use a LinearGradientBrush
        // for its background instead of needing to manually set pixels ourselves.
        // We'll have the gradient go between the minimum and maximum values in the case where
        // the slider handles saturation or value, or in the case where it handles hue,
        // we'll have it go between red, yellow, green, cyan, blue, and purple, in that order.
        _thirdDimensionSliderGradientBrush.GradientStops.Clear();

        switch (ColorSpectrumComponents)
        {
            case ColorSpectrumComponents.HueValue:
            case ColorSpectrumComponents.ValueHue:
                {
                    int minSaturation = MinSaturation;
                    int maxSaturation = MaxSaturation;

                    _thirdDimensionSlider.Minimum = minSaturation;
                    _thirdDimensionSlider.Maximum = maxSaturation;
                    _thirdDimensionSlider.Value = _currentHsvColor.Saturation * 100;

                    // If MinSaturation >= MaxSaturation, then by convention MinSaturation is the only value
                    // that the slider can take.
                    if (minSaturation >= maxSaturation)
                    {
                        maxSaturation = minSaturation;
                    }

                    // AddGradientStop(_thirdDimensionSliderGradientBrush, 0.0, { m_currentHsv.h, minSaturation / 100.0, 1.0 }, 1.0);
                    ColorPicker.AddGradientStop(_thirdDimensionSliderGradientBrush, 0.0,
                                    new HsvColor(_currentHsvColor.Hue, minSaturation / 100.0, 1.0, 255));
                    ColorPicker.AddGradientStop(_thirdDimensionSliderGradientBrush, 1.0,
                                    new HsvColor(_currentHsvColor.Hue, maxSaturation / 100.0, 1.0, 255));
                }
                break;

            case ColorSpectrumComponents.HueSaturation:
            case ColorSpectrumComponents.SaturationHue:
                {
                    int minValue = MinValue;
                    int maxValue = MaxValue;

                    _thirdDimensionSlider.Minimum = minValue;
                    _thirdDimensionSlider.Maximum = maxValue;
                    _thirdDimensionSlider.Value = _currentHsvColor.Value * 100;

                    // If MinValue >= MaxValue, then by convention MinValue is the only value
                    // that the slider can take.
                    if (minValue >= maxValue)
                    {
                        maxValue = minValue;
                    }

                    ColorPicker.AddGradientStop(_thirdDimensionSliderGradientBrush, 0.0,
                                    new HsvColor(_currentHsvColor.Hue, _currentHsvColor.Saturation, minValue, 255));
                    ColorPicker.AddGradientStop(_thirdDimensionSliderGradientBrush, 1.0,
                                    new HsvColor(_currentHsvColor.Hue, _currentHsvColor.Saturation, maxValue, 255));
                }
                break;

            case ColorSpectrumComponents.ValueSaturation:
            case ColorSpectrumComponents.SaturationValue:
                {
                    int minHue = MinHue;
                    int maxHue = MaxHue;

                    _thirdDimensionSlider.Minimum = minHue;
                    _thirdDimensionSlider.Maximum = maxHue;
                    _thirdDimensionSlider.Value = _currentHsvColor.Hue;

                    // If MinHue >= MaxHue, then by convention MinHue is the only value
                    // that the slider can take.
                    if (minHue >= maxHue)
                    {
                        maxHue = minHue;
                    }

                    double minOffset = minHue / 359.0;
                    double maxOffset = maxHue / 359.0;

                    // With unclamped hue values, we have six different gradient stops, corresponding to red, yellow, green, cyan, blue, and purple.
                    // However, with clamped hue values, we may not need all of those gradient stops.
                    // We know we need a gradient stop at the start and end corresponding to the min and max values for hue,
                    // and then in the middle, we'll add any gradient stops corresponding to the hue of those six pure colors that exist
                    // between the min and max hue.
                    ColorPicker.AddGradientStop(_thirdDimensionSliderGradientBrush, 0.0,
                                    new HsvColor(minHue, 1.0, 1.0, 255));

                    for (int sextant = 1; sextant <= 5; sextant++)
                    {
                        double offset = sextant / 6.0;

                        if (minOffset < offset && maxOffset > offset)
                        {
                            ColorPicker.AddGradientStop(_thirdDimensionSliderGradientBrush, (offset - minOffset) / (maxOffset - minOffset),
                                            new HsvColor(60.0 * sextant, 1.0, 1.0, 255));
                        }
                    }

                    ColorPicker.AddGradientStop(_thirdDimensionSliderGradientBrush, 1.0, new HsvColor(maxHue, 1.0, 1.0, 255));
                }
                break;
        }
    }

    private void UpdateVisualState(bool useTransitions)
    {
        IsTextEntryGridVisible = !IsMoreButtonVisible || _textEntryGridOpened || Orientation != Orientation.Vertical;
        PreviousColorVisibility = PreviousColor != null ? Visibility.Visible : Visibility.Collapsed;
    }
    #endregion
    #endregion

    #region Protected
    #region Overrides
    protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs args)
    {
        base.OnPropertyChanged(args);

        DependencyProperty property = args.Property;

        if (property == ColorProperty)
        {
            OnColorChanged(args);
        }
        else if (property == PreviousColorProperty)
        {
            OnPreviousColorChanged(args);
        }
        else if (property == IsAlphaEnabledProperty)
        {
            OnIsAlphaEnabledChanged(args);
        }
        else if (property == IsColorSpectrumVisibleProperty || property == IsColorPreviewVisibleProperty ||
                 property == IsColorSliderVisibleProperty || property == IsAlphaSliderVisibleProperty ||
                 property == IsMoreButtonVisibleProperty || property == IsColorChannelTextInputVisibleProperty ||
                 property == IsAlphaTextInputVisibleProperty || property == IsHexInputVisibleProperty)
        {
            OnPartVisibilityChanged(args);
        }
        else if (property == MinHueProperty || property == MaxHueProperty)
        {
            OnMinMaxHueChanged(args);
        }
        else if (property == MinSaturationProperty || property == MaxSaturationProperty)
        {
            OnMinMaxSaturationChanged(args);
        }
        else if (property == MinValueProperty || property == MaxValueProperty)
        {
            OnMinMaxValueChanged(args);
        }
        else if (property == ColorSpectrumComponentsProperty)
        {
            OnColorSpectrumComponentsChanged(args);
        }
        else if (property == OrientationProperty)
        {
            OnOrientationChanged(args);
        }
    }
    #endregion
    #endregion

    #region Public
    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        _colorSpectrum = (ColorSpectrum)GetTemplateChild("ColorSpectrum");

        _colorPreviewRectangleGrid = (Grid)GetTemplateChild("ColorPreviewRectangleGrid");
        _colorPreviewRectangle = (Rectangle)GetTemplateChild("ColorPreviewRectangle");
        _previousColorRectangle = (Rectangle)GetTemplateChild("PreviousColorRectangle");
        _colorPreviewRectangleCheckeredBackgroundImageBrush = (ImageBrush)GetTemplateChild("ColorPreviewRectangleCheckeredBackgroundImageBrush");

        _thirdDimensionSlider = (ColorPickerSlider)GetTemplateChild("ThirdDimensionSlider");
        _thirdDimensionSliderGradientBrush = (LinearGradientBrush)GetTemplateChild("ThirdDimensionSliderGradientBrush");

        _alphaSlider = (ColorPickerSlider)GetTemplateChild("AlphaSlider");
        _alphaSliderGradientBrush = (LinearGradientBrush)GetTemplateChild("AlphaSliderGradientBrush");
        _alphaSliderBackgroundRectangle = (Rectangle)GetTemplateChild("AlphaSliderBackgroundRectangle");
        _alphaSliderCheckeredBackgroundImageBrush = (ImageBrush)GetTemplateChild("AlphaSliderCheckeredBackgroundImageBrush");

        _moreButton = (ButtonBase)GetTemplateChild("MoreButton");

        _colorRepresentationComboBox = (ComboBox)GetTemplateChild("ColorRepresentationComboBox");

        _redTextBox = (WindowsTextBox)GetTemplateChild("RedTextBox");
        _greenTextBox = (WindowsTextBox)GetTemplateChild("GreenTextBox");
        _blueTextBox = (WindowsTextBox)GetTemplateChild("BlueTextBox");
        _hueTextBox = (WindowsTextBox)GetTemplateChild("HueTextBox");
        _saturationTextBox = (WindowsTextBox)GetTemplateChild("SaturationTextBox");
        _valueTextBox = (WindowsTextBox)GetTemplateChild("ValueTextBox");
        _alphaTextBox = (WindowsTextBox)GetTemplateChild("AlphaTextBox");
        _hexTextBox = (WindowsTextBox)GetTemplateChild("HexTextBox");

        _rgbComboBoxItem = (ComboBoxItem)GetTemplateChild("RGBComboBoxItem");
        _hsvComboBoxItem = (ComboBoxItem)GetTemplateChild("HSVComboBoxItem");
        _redLabel = (TextBlock)GetTemplateChild("RedLabel");
        _greenLabel = (TextBlock)GetTemplateChild("GreenLabel");
        _blueLabel = (TextBlock)GetTemplateChild("BlueLabel");
        _hueLabel = (TextBlock)GetTemplateChild("HueLabel");
        _saturationLabel = (TextBlock)GetTemplateChild("SaturationLabel");
        _valueLabel = (TextBlock)GetTemplateChild("ValueLabel");
        _alphaLabel = (TextBlock)GetTemplateChild("AlphaLabel");

        _checkerColorBrush = (SolidColorBrush)GetTemplateChild("CheckerColorBrush");

        if (_colorSpectrum != null)
        {
            _colorSpectrum.ColorChanged += OnColorSpectrumColorChanged;
            _colorSpectrum.SizeChanged += OnColorSpectrumSizeChanged;

            //winrt::AutomationProperties::SetName(colorSpectrum, ResourceAccessor::GetLocalizedStringResource(SR_AutomationNameColorSpectrum));
        }

        if (_colorPreviewRectangleGrid != null)
        {
            _colorPreviewRectangleGrid.SizeChanged += OnColorPreviewRectangleGridSizeChanged;
        }

        if (_thirdDimensionSlider != null)
        {
            _thirdDimensionSlider.ValueChanged += OnThirdDimensionSliderValueChanged;
            SetThirdDimensionSliderChannel();
        }

        if (_alphaSlider != null)
        {
            _alphaSlider.ValueChanged += OnAlphaSliderValueChanged;
            _alphaSlider.ColorChannel = ColorPickerHsvChannel.Alpha;

            //winrt::AutomationProperties::SetName(alphaSlider, ResourceAccessor::GetLocalizedStringResource(SR_AutomationNameAlphaSlider));
        }

        if (_alphaSliderBackgroundRectangle != null)
        {
            _alphaSliderBackgroundRectangle.SizeChanged += OnAlphaSliderBackgroundRectangleSizeChanged;
        }

        if (_moreButton != null)
        {
            if (_moreButton is ToggleButton moreButtonAsToggleButton)
            {
                moreButtonAsToggleButton.IsChecked = true;
                moreButtonAsToggleButton.Checked += OnMoreButtonChecked;
                moreButtonAsToggleButton.Unchecked += OnMoreButtonUnchecked;
            }
            else
            {
                _moreButton.Click += OnMoreButtonClicked;
                _moreButton.Content = "More";
            }

            //winrt::AutomationProperties::SetName(moreButton, ResourceAccessor::GetLocalizedStringResource(SR_AutomationNameMoreButtonCollapsed));
            //winrt::AutomationProperties::SetHelpText(moreButton, ResourceAccessor::GetLocalizedStringResource(SR_HelpTextMoreButton));

            _moreButtonLabel = (TextBlock)GetTemplateChild("MoreButtonLabel");

            if (_moreButtonLabel != null)
            {
                _moreButtonLabel.Text = "More";
                //_moreButtonLabel.Text = ResourceAccessor::GetLocalizedStringResource(SR_TextMoreButtonLabelCollapsed));
            }
        }

        if (_colorRepresentationComboBox != null)
        {
            _colorRepresentationComboBox.SelectionChanged += OnColorRepresentationComboBoxSelectionChanged;

            //winrt::AutomationProperties::SetName(colorRepresentationComboBox, ResourceAccessor::GetLocalizedStringResource(SR_AutomationNameColorModelComboBox));
        }

        if (_redTextBox != null)
        {
            _redTextBox.TextChanged += OnRgbTextChanged;
            _redTextBox.GotFocus += OnTextBoxGotFocus;
            _redTextBox.LostFocus += OnTextBoxLostFocus;

            //winrt::AutomationProperties::SetName(redTextBox, ResourceAccessor::GetLocalizedStringResource(SR_AutomationNameRedTextBox));
        }

        if (_greenTextBox != null)
        {
            _greenTextBox.TextChanged += OnRgbTextChanged;
            _greenTextBox.GotFocus += OnTextBoxGotFocus;
            _greenTextBox.LostFocus += OnTextBoxLostFocus;

            //winrt::AutomationProperties::SetName(greenTextBox, ResourceAccessor::GetLocalizedStringResource(SR_AutomationNameGreenTextBox));
        }

        if (_blueTextBox != null)
        {
            _blueTextBox.TextChanged += OnRgbTextChanged;
            _blueTextBox.GotFocus += OnTextBoxGotFocus;
            _blueTextBox.LostFocus += OnTextBoxLostFocus;

            //winrt::AutomationProperties::SetName(blueTextBox, ResourceAccessor::GetLocalizedStringResource(SR_AutomationNameBlueTextBox));
        }

        if (_hueTextBox != null)
        {
            _hueTextBox.TextChanged += OnHueTextChanged;
            _hueTextBox.GotFocus += OnTextBoxGotFocus;
            _hueTextBox.LostFocus += OnTextBoxLostFocus;

            //winrt::AutomationProperties::SetName(hueTextBox, ResourceAccessor::GetLocalizedStringResource(SR_AutomationNameHueTextBox));
        }

        if (_saturationTextBox != null)
        {
            _saturationTextBox.TextChanged += OnSaturationTextChanged;
            _saturationTextBox.GotFocus += OnTextBoxGotFocus;
            _saturationTextBox.LostFocus += OnTextBoxLostFocus;

            //winrt::AutomationProperties::SetName(saturationTextBox, ResourceAccessor::GetLocalizedStringResource(SR_AutomationNameSaturationTextBox));
        }

        if (_valueTextBox != null)
        {
            _valueTextBox.TextChanged += OnValueTextChanged;
            _valueTextBox.GotFocus += OnTextBoxGotFocus;
            _valueTextBox.LostFocus += OnTextBoxLostFocus;

            //winrt::AutomationProperties::SetName(valueTextBox, ResourceAccessor::GetLocalizedStringResource(SR_AutomationNameValueTextBox));
        }

        if (_alphaTextBox != null)
        {
            _alphaTextBox.TextChanged += OnAlphaTextChanged;
            _alphaTextBox.GotFocus += OnTextBoxGotFocus;
            _alphaTextBox.LostFocus += OnTextBoxLostFocus;

            //winrt::AutomationProperties::SetName(alphaTextBox, ResourceAccessor::GetLocalizedStringResource(SR_AutomationNameAlphaTextBox));
        }

        if (_hexTextBox != null)
        {
            _hexTextBox.TextChanged += OnHexTextChanged;
            _hexTextBox.GotFocus += OnTextBoxGotFocus;
            _hexTextBox.LostFocus += OnTextBoxLostFocus;

            //winrt::AutomationProperties::SetName(hexTextBox, ResourceAccessor::GetLocalizedStringResource(SR_AutomationNameHexTextBox));
        }

        if (_rgbComboBoxItem != null)
        {
            _rgbComboBoxItem.Content = "RGB";
            //_rgbComboBoxItem.Content = ResourceAccessor.GetLocalizedStringResource(SR_ContentRGBComboBoxItem);
        }

        if (_hsvComboBoxItem != null)
        {
            _hsvComboBoxItem.Content = "HSV";
            //_hsvComboBoxItem.Content = ResourceAccessor.GetLocalizedStringResource(SR_ContentHSVComboBoxItem);
        }

        if (_redLabel != null)
        {
            _redLabel.Text = "Red";
            //_redLabel.Text = ResourceAccessor.GetLocalizedStringResource(SR_TextRedLabel);
        }

        if (_greenLabel != null)
        {
            _greenLabel.Text = "Green";
            //greenLabel.Text(ResourceAccessor::GetLocalizedStringResource(SR_TextGreenLabel));
        }

        if (_blueLabel != null)
        {
            _blueLabel.Text = "Blue";
            //blueLabel.Text(ResourceAccessor::GetLocalizedStringResource(SR_TextBlueLabel));
        }

        if (_hueLabel != null)
        {
            _hueLabel.Text = "Hue";
            //hueLabel.Text(ResourceAccessor::GetLocalizedStringResource(SR_TextHueLabel));
        }

        if (_saturationLabel != null)
        {
            _saturationLabel.Text = "Saturation";
            //saturationLabel.Text(ResourceAccessor::GetLocalizedStringResource(SR_TextSaturationLabel));
        }

        if (_valueLabel != null)
        {
            _valueLabel.Text = "Value";
            //valueLabel.Text(ResourceAccessor::GetLocalizedStringResource(SR_TextValueLabel));
        }

        if (_alphaLabel != null)
        {
            _alphaLabel.Text = "Alpha";
            //alphaLabel.Text(ResourceAccessor::GetLocalizedStringResource(SR_TextAlphaLabel));
        }

        if (_checkerColorBrush != null)
        {
            // TODO: I'm pretty sure this won't work
            //checkerColorBrush.RegisterPropertyChangedCallback(winrt::SolidColorBrush::ColorProperty(), { this, &ColorPicker::OnCheckerColorChanged });
            _checkerColorBrush.Changed += OnCheckerColorChanged;
        }

        CreateColorPreviewCheckeredBackground();
        CreateAlphaSliderCheckeredBackground();
        UpdateVisualState(false /* useTransitions */);
        InitializeColor();
        UpdatePreviousColorRectangle();
    }
    #endregion
    #endregion
}
