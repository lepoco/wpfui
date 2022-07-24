// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
//
// Code from https://github.com/microsoft/microsoft-ui-xaml/
//

using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using Wpf.ModernColorPicker.Common;
using Wpf.Ui.Common;

namespace Wpf.Ui.Controls;

public class ColorSpectrum : Control
{
    #region Fields
    private bool _isPointerOver;
    private bool _isPointerPressed;
    private bool _shouldShowLargeSelection;
    private bool _updatingColor;
    private bool _updatingHsvColor;

    private CancellationTokenSource _imageBitmapCreationCancellationTokenSource;

    private Color _oldColor;

    private ColorSpectrumShape _shapeFromLastBitmapCreation;
    private ColorSpectrumComponents _componentsFromLastBitmapCreation;

    private double _imageHeightFromLastBitmapCreation;
    private double _imageWidthFromLastBitmapCreation;

    private HsvColor[] _hsvValues;

    private int _maxHueFromLastBitmapCreation;
    private int _maxSaturationFromLastBitmapCreation;
    private int _maxValueFromLastBitmapCreation;
    private int _minHueFromLastBitmapCreation;
    private int _minSaturationFromLastBitmapCreation;
    private int _minValueFromLastBitmapCreation;

    private HsvColor _oldHsvColor;

    private WriteableBitmap _hueBlueBitmap;
    private WriteableBitmap _hueCyanBitmap;
    private WriteableBitmap _hueGreenBitmap;
    private WriteableBitmap _huePurpleBitmap;
    private WriteableBitmap _hueRedBitmap;
    private WriteableBitmap _hueYellowBitmap;
    private WriteableBitmap _saturationMinimumBitmap;
    private WriteableBitmap _saturationMaximumBitmap;
    private WriteableBitmap _valueBitmap;

    #region Template parts
    private Ellipse _spectrumEllipse;
    private Ellipse _spectrumOverlayEllipse;
    private FrameworkElement _inputTarget;
    private Grid _layoutRoot;
    private Grid _sizingGrid;
    private Panel _selectionEllipsePanel;
    private Rectangle _spectrumRectangle;
    private Rectangle _spectrumOverlayRectangle;
    private ToolTip _colorNameToolTip;
    #endregion
    #endregion

    #region Dependency properties
    internal static readonly DependencyProperty SelectionEllipseShouldBeLightProperty =
    DependencyProperty.Register(nameof(SelectionEllipseShouldBeLight), typeof(bool), typeof(ColorSpectrum),
                                new PropertyMetadata(false));

    internal static readonly DependencyProperty ShouldShowLargeSelectionProperty =
    DependencyProperty.Register(nameof(ShouldShowLargeSelection), typeof(bool), typeof(ColorSpectrum),
                                new PropertyMetadata(false));

    public static readonly DependencyProperty ColorProperty =
        DependencyProperty.Register(nameof(Color), typeof(Color), typeof(ColorSpectrum),
                                    new PropertyMetadata(Color.FromArgb(255, 255, 255, 255)));

    public static readonly DependencyProperty ComponentsProperty =
        DependencyProperty.Register(nameof(Components), typeof(ColorSpectrumComponents), typeof(ColorSpectrum),
                                    new PropertyMetadata(ColorSpectrumComponents.HueSaturation));

    public static readonly DependencyProperty ShapeProperty =
        DependencyProperty.Register(nameof(Shape), typeof(ColorSpectrumShape), typeof(ColorSpectrum),
                                    new PropertyMetadata(ColorSpectrumShape.Box));

    public static readonly DependencyProperty CornerRadiusProperty =
        DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(ColorSpectrum),
                                    new PropertyMetadata(new CornerRadius()));

    public static readonly DependencyProperty HsvColorProperty =
        DependencyProperty.Register(nameof(HsvColor), typeof(HsvColor), typeof(ColorSpectrum),
                                    new PropertyMetadata(new HsvColor(0, 0, 1, 255)));

    public static readonly DependencyProperty MaxHueProperty =
        DependencyProperty.Register(nameof(MaxHue), typeof(int), typeof(ColorSpectrum),
                                    new PropertyMetadata(359));

    public static readonly DependencyProperty MaxSaturationProperty =
        DependencyProperty.Register(nameof(MaxSaturation), typeof(int), typeof(ColorSpectrum),
                                    new PropertyMetadata(100));

    public static readonly DependencyProperty MaxValueProperty =
        DependencyProperty.Register(nameof(MaxValue), typeof(int), typeof(ColorSpectrum),
                                    new PropertyMetadata(100));

    public static readonly DependencyProperty MinHueProperty =
        DependencyProperty.Register(nameof(MinHue), typeof(int), typeof(ColorSpectrum),
                                    new PropertyMetadata(default(int)));

    public static readonly DependencyProperty MinSaturationProperty =
        DependencyProperty.Register(nameof(MinSaturation), typeof(int), typeof(ColorSpectrum),
                                    new PropertyMetadata(default(int)));

    public static readonly DependencyProperty MinValueProperty =
        DependencyProperty.Register(nameof(MinValue), typeof(int), typeof(ColorSpectrum),
                                    new PropertyMetadata(default(int))); 
    #endregion

    public event ColorChangedEventHandler<ColorSpectrum> ColorChanged;

    #region Properties
    internal bool SelectionEllipseShouldBeLight
    {
        get => (bool)GetValue(SelectionEllipseShouldBeLightProperty);
        set => SetValue(SelectionEllipseShouldBeLightProperty, value);
    }

    internal bool ShouldShowLargeSelection
    {
        get => (bool)GetValue(ShouldShowLargeSelectionProperty);
        set => SetValue(ShouldShowLargeSelectionProperty, value);
    }

    public Color Color
    {
        get => (Color)GetValue(ColorProperty);
        set => SetValue(ColorProperty, value);
    }

    public ColorSpectrumComponents Components
    {
        get => (ColorSpectrumComponents)GetValue(ComponentsProperty);
        set => SetValue(ComponentsProperty, value);
    }

    public ColorSpectrumShape Shape
    {
        get => (ColorSpectrumShape)GetValue(ShapeProperty);
        set => SetValue(ShapeProperty, value);
    }

    public CornerRadius CornerRadius
    {
        get => (CornerRadius)GetValue(CornerRadiusProperty);
        set => SetValue(CornerRadiusProperty, value);
    }

    public HsvColor HsvColor
    {
        get => (HsvColor)GetValue(HsvColorProperty);
        set => SetValue(HsvColorProperty, value);
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
    #endregion

    public ColorSpectrum()
    {
        _hsvValues = Array.Empty<HsvColor>();
        _updatingColor = false;
        _updatingHsvColor = false;
        _isPointerOver = false;
        _isPointerPressed = false;
        _shouldShowLargeSelection = false;

        _shapeFromLastBitmapCreation = Shape;
        _componentsFromLastBitmapCreation = Components;
        _imageWidthFromLastBitmapCreation = 0;
        _imageHeightFromLastBitmapCreation = 0;
        _minHueFromLastBitmapCreation = MinHue;
        _maxHueFromLastBitmapCreation = MaxHue;
        _minSaturationFromLastBitmapCreation = MinSaturation;
        _maxSaturationFromLastBitmapCreation = MaxSaturation;
        _minValueFromLastBitmapCreation = MinValue;
        _maxValueFromLastBitmapCreation = MaxValue;
    }

    #region Methods
    #region Private
    #region Dependency property changes event handlers
    private void OnColorChanged(DependencyPropertyChangedEventArgs args)
    {
        // If we're in the process of internally updating the color, then we don't want to respond to the Color property changing.
        if (!_updatingColor)
        {
            Color color = Color;

            _updatingHsvColor = true;
            HsvColor = color.ToHsvColor();
            //HsvColor(winrt::float4{ static_cast<float>(newHsv.h), static_cast<float>(newHsv.s), static_cast<float>(newHsv.v), static_cast<float>(color.A / 255.0) });
            _updatingHsvColor = false;

            UpdateEllipse();
            UpdateBitmapSources();
        }

        _oldColor = (Color)args.OldValue;
    }

    private void OnHsvColorChanged(DependencyPropertyChangedEventArgs args)
    {
        // If we're in the process of internally updating the HSV color, then we don't want to respond to the HsvColor property changing.
        if (!_updatingHsvColor)
        {
            SetColor();
        }

        _oldHsvColor = (HsvColor)args.OldValue;
    }

    private void OnMinMaxHueChanged(DependencyPropertyChangedEventArgs args)
    {
        int minHue = MinHue;
        int maxHue = MaxHue;

        if (minHue < 0 || minHue > 359)
        {
            throw new ArgumentException("MinHue must be between 0 and 359");
        }
        else if (maxHue < 0 || maxHue > 359)
        {
            throw new ArgumentException("MaxHue must be between 0 and 359");
        }

        ColorSpectrumComponents components = Components;

        // If hue is one of the axes in the spectrum bitmap, then we'll need to regenerate it
        // if the maximum or minimum value has changed.
        if (components != ColorSpectrumComponents.SaturationValue && components != ColorSpectrumComponents.ValueSaturation)
        {
            CreateBitmapsAndColorMap();
        }
    }

    private void OnMinMaxSaturationChanged(DependencyPropertyChangedEventArgs args)
    {
        int minSaturation = MinSaturation;
        int maxSaturation = MaxSaturation;

        if (minSaturation < 0 || minSaturation > 100)
        {
            throw new ArgumentException("MinSaturation must be between 0 and 100");
        }
        else if (maxSaturation < 0 || maxSaturation > 100)
        {
            throw new ArgumentException("MaxSaturation must be between 0 and 100");
        }

        ColorSpectrumComponents components = Components;

        // If value is one of the axes in the spectrum bitmap, then we'll need to regenerate it
        // if the maximum or minimum value has changed.
        if (components != ColorSpectrumComponents.HueValue && components != ColorSpectrumComponents.ValueHue)
        {
            CreateBitmapsAndColorMap();
        }
    }

    private void OnMinMaxValueChanged(DependencyPropertyChangedEventArgs args)
    {
        int minValue = MinValue;
        int maxValue = MaxValue;

        if (minValue < 0 || minValue > 100)
        {
            throw new ArgumentException("MinValue must be between 0 and 100");
        }
        else if (maxValue < 0 || maxValue > 100)
        {
            throw new ArgumentException("MaxValue must be between 0 and 100");
        }

        ColorSpectrumComponents components = Components;

        // If value is one of the axes in the spectrum bitmap, then we'll need to regenerate it
        // if the maximum or minimum value has changed.
        if (components != ColorSpectrumComponents.HueSaturation && components != ColorSpectrumComponents.SaturationHue)
        {
            CreateBitmapsAndColorMap();
        }
    }

    private void OnShapeChanged(DependencyPropertyChangedEventArgs args)
    {
        CreateBitmapsAndColorMap();
    }

    private void OnComponentsChanged(DependencyPropertyChangedEventArgs args)
    {
        CreateBitmapsAndColorMap();
    }
    #endregion

    #region Class changes event handlers
    private void OnUnloaded(object sender, RoutedEventArgs args)
    {
        // If we're in the middle of creating an image bitmap while being unloaded,
        // we'll want to synchronously cancel it so we don't have any asynchronous actions
        // lingering beyond our lifetime.
        _imageBitmapCreationCancellationTokenSource.Cancel();
    }
    #endregion

    #region Template part changes event handlers
    private void OnInputTargetPointerDown(object sender, RoutedEventArgs args, Point pointerPosition)
    {
        //Focus(winrt::FocusState::Pointer);
        Focus();

        _isPointerPressed = true;
        UpdateColorFromPoint(pointerPosition);
        UpdateVisualState(true /* useTransitions*/);
        UpdateEllipse();
        UpdateShouldShowLargeSelectionProperty();

        args.Handled = true;
    }

    private void OnInputTargetPointerEnter(object sender, RoutedEventArgs args)
    {
        _isPointerOver = true;
        UpdateVisualState(true /* useTransitions*/);
        args.Handled = true;
    }

    private void OnInputTargetPointerLeave(object sender, RoutedEventArgs args)
    {
        _isPointerOver = false;
        UpdateVisualState(true /* useTransitions*/);
        args.Handled = true;
    }

    private void OnInputTargetPointerMove(object sender, RoutedEventArgs args, Point pointerPosition)
    {
        if (!_isPointerPressed)
        {
            return;
        }

        UpdateColorFromPoint(pointerPosition);
        args.Handled = true;
    }

    private void OnInputTargetPointerUp(object sender, RoutedEventArgs args)
    {
        _isPointerPressed = false;
        _inputTarget.ReleaseMouseCapture();
        UpdateVisualState(true /* useTransitions*/);
        UpdateEllipse();
        UpdateShouldShowLargeSelectionProperty();

        args.Handled = true;
    }

    private void OnInputTargetMouseDown(object sender, MouseEventArgs args)
    {
        args.MouseDevice.Capture(_inputTarget);
        OnInputTargetPointerDown(sender, args, args.GetPosition(_inputTarget));
    }

    private void OnInputTargetMouseEnter(object sender, MouseEventArgs args)
    {
        OnInputTargetPointerEnter(sender, args);
    }

    private void OnInputTargetMouseLeave(object sender, MouseEventArgs args)
    {
        OnInputTargetPointerLeave(sender, args);
    }

    private void OnInputTargetMouseMove(object sender, MouseEventArgs args)
    {
        OnInputTargetPointerMove(sender, args, args.GetPosition(_inputTarget));
    }

    private void OnInputTargetMouseUp(object sender, MouseButtonEventArgs args)
    {
        OnInputTargetPointerUp(sender, args);
    }

    private void OnInputTargetStylusDown(object sender, StylusEventArgs args)
    {
        _shouldShowLargeSelection = true;
        args.StylusDevice.Capture(_inputTarget);
        OnInputTargetPointerDown(sender, args, args.GetPosition(_inputTarget));
    }

    private void OnInputTargetStylusEnter(object sender, StylusEventArgs args)
    {
        OnInputTargetPointerEnter(sender, args);
    }

    private void OnInputTargetStylusLeave(object sender, StylusEventArgs args)
    {
        OnInputTargetPointerLeave(sender, args);
    }

    private void OnInputTargetStylusMove(object sender, StylusEventArgs args)
    {
        OnInputTargetPointerMove(sender, args, args.GetPosition(_inputTarget));
    }

    private void OnInputTargetStylusUp(object sender, StylusEventArgs args)
    {
        _shouldShowLargeSelection = false;
        OnInputTargetPointerUp(sender, args);
    }

    private void OnInputTargetTouchDown(object sender, TouchEventArgs args)
    {
        _shouldShowLargeSelection = true;
        args.TouchDevice.Capture(_inputTarget);
        OnInputTargetPointerDown(sender, args, args.GetTouchPoint(_inputTarget).Position);
    }

    private void OnInputTargetTouchEnter(object sender, TouchEventArgs args)
    {
        OnInputTargetPointerEnter(sender, args);
    }

    private void OnInputTargetTouchLeave(object sender, TouchEventArgs args)
    {
        OnInputTargetPointerLeave(sender, args);
    }

    private void OnInputTargetTouchMove(object sender, TouchEventArgs args)
    {
        OnInputTargetPointerMove(sender, args, args.GetTouchPoint(_inputTarget).Position);
    }

    private void OnInputTargetTouchUp(object sender, TouchEventArgs args)
    {
        _shouldShowLargeSelection = false;
        OnInputTargetPointerUp(sender, args);
    }

    private void OnLayoutRootSizeChanged(object sender, SizeChangedEventArgs args)
    {
        // We want ColorSpectrum to always be a square, so we'll take the smaller of the dimensions
        // and size the sizing grid to that.
        CreateBitmapsAndColorMap();
    }

    private void OnSelectionEllipseFlowDirectionChanged(DependencyObject sender, DependencyProperty property)
    {
        UpdateEllipse();
    }
    #endregion

    private void CreateBitmapsAndColorMap()
    {
        if (_layoutRoot == null || _sizingGrid == null || _inputTarget == null || _spectrumRectangle == null ||
            _spectrumEllipse == null || _spectrumOverlayRectangle == null || _spectrumOverlayEllipse == null ||
            DesignerProperties.GetIsInDesignMode(this))
        {
            return;
        }

        double minDimension = Math.Min(_layoutRoot.ActualWidth, _layoutRoot.ActualHeight);

        if (minDimension == 0)
        {
            return;
        }

        _sizingGrid.Width = minDimension;
        _sizingGrid.Height = minDimension;

        if (_sizingGrid.Clip is RectangleGeometry rectangleGeometry)
        {
            rectangleGeometry = rectangleGeometry.Clone();
            rectangleGeometry.Rect = new Rect(0, 0, minDimension, minDimension);

            if (rectangleGeometry.CanFreeze)
            {
                rectangleGeometry.Freeze();
            }
            _sizingGrid.Clip = rectangleGeometry;
        }

        _inputTarget.Width = minDimension;
        _inputTarget.Height = minDimension;
        _spectrumRectangle.Width = minDimension;
        _spectrumRectangle.Height = minDimension;
        _spectrumEllipse.Width = minDimension;
        _spectrumEllipse.Height = minDimension;
        _spectrumOverlayRectangle.Width = minDimension;
        _spectrumOverlayRectangle.Height = minDimension;
        _spectrumOverlayEllipse.Width = minDimension;
        _spectrumOverlayEllipse.Height = minDimension;

        HsvColor hsvColor = HsvColor;
        int minHue = MinHue;
        int maxHue = MaxHue;
        int minSaturation = MinSaturation;
        int maxSaturation = MaxSaturation;
        int minValue = MinValue;
        int maxValue = MaxValue;
        ColorSpectrumShape shape = Shape;
        ColorSpectrumComponents components = Components;

        // If min >= max, then by convention, min is the only number that a property can have.
        if (minHue >= maxHue)
        {
            maxHue = minHue;
        }

        if (minSaturation >= maxSaturation)
        {
            maxSaturation = minSaturation;
        }

        if (minValue >= maxValue)
        {
            maxValue = minValue;
        }

        HsvColor hsv = hsvColor;

        var pixelCount = (int)(Math.Round(minDimension) * Math.Round(minDimension));
        var pixelDataSize = pixelCount * 4;

        // The middle 4 are only needed and used in the case of hue as the third dimension.
        // Saturation and luminosity need only a min and max.
        byte[] bgraMinPixelData = new byte[pixelDataSize];
        byte[] bgraMaxPixelData = new byte[pixelDataSize];
        byte[] bgraMiddle1PixelData = Array.Empty<byte>();
        byte[] bgraMiddle2PixelData = Array.Empty<byte>();
        byte[] bgraMiddle3PixelData = Array.Empty<byte>();
        byte[] bgraMiddle4PixelData = Array.Empty<byte>();
        HsvColor[] newHsvValues = new HsvColor[pixelCount];

        // We'll only save pixel data for the middle bitmaps if our third dimension is hue.
        if (components == ColorSpectrumComponents.ValueSaturation || components == ColorSpectrumComponents.SaturationValue)
        {
            bgraMiddle1PixelData = new byte[pixelDataSize];
            bgraMiddle2PixelData = new byte[pixelDataSize];
            bgraMiddle3PixelData = new byte[pixelDataSize];
            bgraMiddle4PixelData = new byte[pixelDataSize];
        }

        int minDimensionInt = (int)Math.Round(minDimension);

        if (_imageBitmapCreationCancellationTokenSource != null)
        {
            _imageBitmapCreationCancellationTokenSource.Cancel();
        }

        _imageBitmapCreationCancellationTokenSource = new CancellationTokenSource();
        var imageBitmapCreationTask = new Task(obj =>
        {
            // As the user perceives it, every time the third dimension not represented in the ColorSpectrum changes,
            // the ColorSpectrum will visually change to accommodate that value.  For example, if the ColorSpectrum handles hue and luminosity,
            // and the saturation externally goes from 1.0 to 0.5, then the ColorSpectrum will visually change to look more washed out
            // to represent that third dimension's new value.
            // Internally, however, we don't want to regenerate the ColorSpectrum bitmap every single time this happens, since that's very expensive.
            // In order to make it so that we don't have to, we implement an optimization where, rather than having only one bitmap,
            // we instead have multiple that we blend together using opacity to create the effect that we want.
            // In the case where the third dimension is saturation or luminosity, we only need two: one bitmap at the minimum value
            // of the third dimension, and one bitmap at the maximum.  Then we set the second's opacity at whatever the value of
            // the third dimension is - e.g., a saturation of 0.5 implies an opacity of 50%.
            // In the case where the third dimension is hue, we need six: one bitmap corresponding to red, yellow, green, cyan, blue, and purple.
            // We'll then blend between whichever colors our hue exists between - e.g., an orange color would use red and yellow with an opacity of 50%.
            // This optimization does incur slightly more startup time initially since we have to generate multiple bitmaps at once instead of only one,
            // but the running time savings after that are *huge* when we can just set an opacity instead of generating a brand new bitmap.
            CancellationToken cancellationToken = (CancellationToken)obj;

            int pixelDataBuffersIndex = 0;
            int newHsvValuesArrayIndex = 0;

            if (shape == ColorSpectrumShape.Box)
            {
                for (int x = minDimensionInt - 1; x >= 0; --x)
                {
                    for (int y = minDimensionInt - 1; y >= 0; --y)
                    {
                        if (cancellationToken.IsCancellationRequested)
                        {
                            break;
                        }

                        FillPixelForBox(x, y, hsv, minDimensionInt, components, minHue, maxHue, minSaturation, maxSaturation, minValue,
                                        maxValue, bgraMinPixelData, bgraMiddle1PixelData, bgraMiddle2PixelData, bgraMiddle3PixelData,
                                        bgraMiddle4PixelData, bgraMaxPixelData, newHsvValues, pixelDataBuffersIndex,
                                        newHsvValuesArrayIndex);

                        pixelDataBuffersIndex += 4;
                        newHsvValuesArrayIndex++;
                    }
                }
            }
            else
            {
                for (int y = 0; y < minDimensionInt; ++y)
                {
                    for (int x = 0; x < minDimensionInt; ++x)
                    {
                        if (cancellationToken.IsCancellationRequested)
                        {
                            break;
                        }

                        FillPixelForRing(x, y, minDimensionInt / 2.0, hsv, components, minHue, maxHue, minSaturation, maxSaturation,
                                            minValue, maxValue, bgraMinPixelData, bgraMiddle1PixelData, bgraMiddle2PixelData,
                                            bgraMiddle3PixelData, bgraMiddle4PixelData, bgraMaxPixelData, newHsvValues,
                                            pixelDataBuffersIndex, newHsvValuesArrayIndex);

                        pixelDataBuffersIndex += 4;
                        newHsvValuesArrayIndex++;
                    }
                }
            }
        }, _imageBitmapCreationCancellationTokenSource.Token);

        imageBitmapCreationTask.Start();
        imageBitmapCreationTask.ContinueWith(t =>
        {
            if (t.Status != TaskStatus.RanToCompletion)
            {
                return;
            }

            _imageBitmapCreationCancellationTokenSource = null;

            //strongThis->m_dispatcherHelper.RunAsync(

            //    [strongThis, minDimension, bgraMinPixelData, bgraMiddle1PixelData, bgraMiddle2PixelData, bgraMiddle3PixelData, bgraMiddle4PixelData, bgraMaxPixelData, newHsvValues]()
            //{
            int pixelWidth = (int)Math.Round(minDimension);
            int pixelHeight = (int)Math.Round(minDimension);

            //if (SharedHelpers::IsRS2OrHigher())
            //{
            //    winrt::LoadedImageSurface minSurface = CreateSurfaceFromPixelData(pixelWidth, pixelHeight, bgraMinPixelData);
            //    winrt::LoadedImageSurface maxSurface = CreateSurfaceFromPixelData(pixelWidth, pixelHeight, bgraMaxPixelData);

            //    switch (components)
            //    {
            //        case winrt::ColorSpectrumComponents::HueValue:
            //        case winrt::ColorSpectrumComponents::ValueHue:
            //            strongThis->m_saturationMinimumSurface = minSurface;
            //            strongThis->m_saturationMaximumSurface = maxSurface;
            //            break;
            //        case winrt::ColorSpectrumComponents::HueSaturation:
            //        case winrt::ColorSpectrumComponents::SaturationHue:
            //            strongThis->m_valueSurface = maxSurface;
            //            break;
            //        case winrt::ColorSpectrumComponents::ValueSaturation:
            //        case winrt::ColorSpectrumComponents::SaturationValue:
            //            strongThis->m_hueRedSurface = minSurface;
            //            strongThis->m_hueYellowSurface = CreateSurfaceFromPixelData(pixelWidth, pixelHeight, bgraMiddle1PixelData);
            //            strongThis->m_hueGreenSurface = CreateSurfaceFromPixelData(pixelWidth, pixelHeight, bgraMiddle2PixelData);
            //            strongThis->m_hueCyanSurface = CreateSurfaceFromPixelData(pixelWidth, pixelHeight, bgraMiddle3PixelData);
            //            strongThis->m_hueBlueSurface = CreateSurfaceFromPixelData(pixelWidth, pixelHeight, bgraMiddle4PixelData);
            //            strongThis->m_huePurpleSurface = maxSurface;
            //            break;
            //    }
            //}
            //else
            //{
            WriteableBitmap minBitmap = ColorHelpers.CreateBitmapFromPixelData(pixelWidth, pixelHeight, bgraMinPixelData);
            WriteableBitmap maxBitmap = ColorHelpers.CreateBitmapFromPixelData(pixelWidth, pixelHeight, bgraMaxPixelData);

            switch (components)
            {
                case ColorSpectrumComponents.HueValue:
                case ColorSpectrumComponents.ValueHue:
                    _saturationMinimumBitmap = minBitmap;
                    _saturationMaximumBitmap = maxBitmap;
                    break;
                case ColorSpectrumComponents.HueSaturation:
                case ColorSpectrumComponents.SaturationHue:
                    _valueBitmap = maxBitmap;
                    break;
                case ColorSpectrumComponents.ValueSaturation:
                case ColorSpectrumComponents.SaturationValue:
                    _hueRedBitmap = minBitmap;
                    _hueYellowBitmap = ColorHelpers.CreateBitmapFromPixelData(pixelWidth, pixelHeight, bgraMiddle1PixelData);
                    _hueGreenBitmap = ColorHelpers.CreateBitmapFromPixelData(pixelWidth, pixelHeight, bgraMiddle2PixelData);
                    _hueCyanBitmap = ColorHelpers.CreateBitmapFromPixelData(pixelWidth, pixelHeight, bgraMiddle3PixelData);
                    _hueBlueBitmap = ColorHelpers.CreateBitmapFromPixelData(pixelWidth, pixelHeight, bgraMiddle4PixelData);
                    _huePurpleBitmap = maxBitmap;
                    break;
            }
            //}

            _hsvValues = newHsvValues;

            Dispatcher.Invoke(() =>
            {
                _shapeFromLastBitmapCreation = Shape;
                _componentsFromLastBitmapCreation = Components;
                _imageWidthFromLastBitmapCreation = minDimension;
                _imageHeightFromLastBitmapCreation = minDimension;
                _minHueFromLastBitmapCreation = MinHue;
                _maxHueFromLastBitmapCreation = MaxHue;
                _minSaturationFromLastBitmapCreation = MinSaturation;
                _maxSaturationFromLastBitmapCreation = MaxSaturation;
                _minValueFromLastBitmapCreation = MinValue;
                _maxValueFromLastBitmapCreation = MaxValue;

                UpdateBitmapSources();
                UpdateEllipse();
            });
            //});
        });
    }

    private void FillPixelForBox(double x, double y, HsvColor baseHsv, double minDimension, ColorSpectrumComponents components,
                                 double minHue, double maxHue, double minSaturation, double maxSaturation, double minValue,
                                 double maxValue, byte[] bgraMinPixelData, byte[] bgraMiddle1PixelData, byte[] bgraMiddle2PixelData,
                                 byte[] bgraMiddle3PixelData, byte[] bgraMiddle4PixelData, byte[] bgraMaxPixelData,
                                 HsvColor[] newHsvValues, int pixelDataBuffersIndex, int newHsvValuesArrayIndex)
    {
        minSaturation /= 100.0;
        maxSaturation /= 100.0;
        minValue /= 100.0;
        maxValue /= 100.0;

        HsvColor hsvMin = baseHsv;
        HsvColor hsvMiddle1 = baseHsv;
        HsvColor hsvMiddle2 = baseHsv;
        HsvColor hsvMiddle3 = baseHsv;
        HsvColor hsvMiddle4 = baseHsv;
        HsvColor hsvMax = baseHsv;

        double xPercent = (minDimension - 1 - x) / (minDimension - 1);
        double yPercent = (minDimension - 1 - y) / (minDimension - 1);

        switch (components)
        {
            case ColorSpectrumComponents.HueValue:
                hsvMin.Hue = hsvMiddle1.Hue = hsvMiddle2.Hue = hsvMiddle3.Hue = hsvMiddle4.Hue = hsvMax.Hue = minHue + yPercent * (maxHue - minHue);
                hsvMin.Value = hsvMiddle1.Value = hsvMiddle2.Value = hsvMiddle3.Value = hsvMiddle4.Value = hsvMax.Value = minValue + xPercent * (maxValue - minValue);
                hsvMin.Saturation = 0;
                hsvMax.Saturation = 1;
                break;

            case ColorSpectrumComponents.HueSaturation:
                hsvMin.Hue = hsvMiddle1.Hue = hsvMiddle2.Hue = hsvMiddle3.Hue = hsvMiddle4.Hue = hsvMax.Hue = minHue + yPercent * (maxHue - minHue);
                hsvMin.Saturation = hsvMiddle1.Saturation = hsvMiddle2.Saturation = hsvMiddle3.Saturation = hsvMiddle4.Saturation = hsvMax.Saturation = minSaturation + xPercent * (maxSaturation - minSaturation);
                hsvMin.Value = 0;
                hsvMax.Value = 1;
                break;

            case ColorSpectrumComponents.ValueHue:
                hsvMin.Value = hsvMiddle1.Value = hsvMiddle2.Value = hsvMiddle3.Value = hsvMiddle4.Value = hsvMax.Value = minValue + yPercent * (maxValue - minValue);
                hsvMin.Hue = hsvMiddle1.Hue = hsvMiddle2.Hue = hsvMiddle3.Hue = hsvMiddle4.Hue = hsvMax.Hue = minHue + xPercent * (maxHue - minHue);
                hsvMin.Saturation = 0;
                hsvMax.Saturation = 1;
                break;

            case ColorSpectrumComponents.ValueSaturation:
                hsvMin.Value = hsvMiddle1.Value = hsvMiddle2.Value = hsvMiddle3.Value = hsvMiddle4.Value = hsvMax.Value = minValue + yPercent * (maxValue - minValue);
                hsvMin.Saturation = hsvMiddle1.Saturation = hsvMiddle2.Saturation = hsvMiddle3.Saturation = hsvMiddle4.Saturation = hsvMax.Saturation = minSaturation + xPercent * (maxSaturation - minSaturation);
                hsvMin.Hue = 0;
                hsvMiddle1.Hue = 60;
                hsvMiddle2.Hue = 120;
                hsvMiddle3.Hue = 180;
                hsvMiddle4.Hue = 240;
                hsvMax.Hue = 300;
                break;

            case ColorSpectrumComponents.SaturationHue:
                hsvMin.Saturation = hsvMiddle1.Saturation = hsvMiddle2.Saturation = hsvMiddle3.Saturation = hsvMiddle4.Saturation = hsvMax.Saturation = minSaturation + yPercent * (maxSaturation - minSaturation);
                hsvMin.Hue = hsvMiddle1.Hue = hsvMiddle2.Hue = hsvMiddle3.Hue = hsvMiddle4.Hue = hsvMax.Hue = minHue + xPercent * (maxHue - minHue);
                hsvMin.Value = 0;
                hsvMax.Value = 1;
                break;

            case ColorSpectrumComponents.SaturationValue:
                hsvMin.Saturation = hsvMiddle1.Saturation = hsvMiddle2.Saturation = hsvMiddle3.Saturation = hsvMiddle4.Saturation = hsvMax.Saturation = minSaturation + yPercent * (maxSaturation - minSaturation);
                hsvMin.Value = hsvMiddle1.Value = hsvMiddle2.Value = hsvMiddle3.Value = hsvMiddle4.Value = hsvMax.Value = minValue + xPercent * (maxValue - minValue);
                hsvMin.Hue = 0;
                hsvMiddle1.Hue = 60;
                hsvMiddle2.Hue = 120;
                hsvMiddle3.Hue = 180;
                hsvMiddle4.Hue = 240;
                hsvMax.Hue = 300;
                break;
        }

        // If saturation is an axis in the spectrum with hue, or value is an axis, then we want
        // that axis to go from maximum at the top to minimum at the bottom,
        // or maximum at the outside to minimum at the inside in the case of the ring configuration,
        // so we'll invert the number before assigning the HSL value to the array.
        // Otherwise, we'll have a very narrow section in the middle that actually has meaningful hue
        // in the case of the ring configuration.
        if (components == ColorSpectrumComponents.HueSaturation || components == ColorSpectrumComponents.SaturationHue)
        {
            hsvMin.Saturation = maxSaturation - hsvMin.Saturation + minSaturation;
            hsvMiddle1.Saturation = maxSaturation - hsvMiddle1.Saturation + minSaturation;
            hsvMiddle2.Saturation = maxSaturation - hsvMiddle2.Saturation + minSaturation;
            hsvMiddle3.Saturation = maxSaturation - hsvMiddle3.Saturation + minSaturation;
            hsvMiddle4.Saturation = maxSaturation - hsvMiddle4.Saturation + minSaturation;
            hsvMax.Saturation = maxSaturation - hsvMax.Saturation + minSaturation;
        }
        else
        {
            hsvMin.Value = maxValue - hsvMin.Value + minValue;
            hsvMiddle1.Value = maxValue - hsvMiddle1.Value + minValue;
            hsvMiddle2.Value = maxValue - hsvMiddle2.Value + minValue;
            hsvMiddle3.Value = maxValue - hsvMiddle3.Value + minValue;
            hsvMiddle4.Value = maxValue - hsvMiddle4.Value + minValue;
            hsvMax.Value = maxValue - hsvMax.Value + minValue;
        }

        newHsvValues[newHsvValuesArrayIndex] = hsvMin;

        Color minColor = hsvMin.ToColor();
        bgraMinPixelData[pixelDataBuffersIndex] = minColor.B; // b
        bgraMinPixelData[pixelDataBuffersIndex + 1] = minColor.G; // g
        bgraMinPixelData[pixelDataBuffersIndex + 2] = minColor.R; // r
        bgraMinPixelData[pixelDataBuffersIndex + 3] = 255; // a - ignored

        // We'll only save pixel data for the middle bitmaps if our third dimension is hue.
        if (components == ColorSpectrumComponents.ValueSaturation || components == ColorSpectrumComponents.SaturationValue)
        {
            Color middle1Color = hsvMiddle1.ToColor();
            bgraMiddle1PixelData[pixelDataBuffersIndex] = middle1Color.B; // b
            bgraMiddle1PixelData[pixelDataBuffersIndex + 1] = middle1Color.G; // g
            bgraMiddle1PixelData[pixelDataBuffersIndex + 2] = middle1Color.R; // r
            bgraMiddle1PixelData[pixelDataBuffersIndex + 3] = 255; // a - ignored

            Color middle2Color = hsvMiddle2.ToColor();
            bgraMiddle2PixelData[pixelDataBuffersIndex] = middle2Color.B; // b
            bgraMiddle2PixelData[pixelDataBuffersIndex + 1] = middle2Color.G; // g
            bgraMiddle2PixelData[pixelDataBuffersIndex + 2] = middle2Color.R; // r
            bgraMiddle2PixelData[pixelDataBuffersIndex + 3] = 255; // a - ignored

            Color middle3Color = hsvMiddle3.ToColor();
            bgraMiddle3PixelData[pixelDataBuffersIndex] = middle3Color.B; // b
            bgraMiddle3PixelData[pixelDataBuffersIndex + 1] = middle3Color.G; // g
            bgraMiddle3PixelData[pixelDataBuffersIndex + 2] = middle3Color.R; // r
            bgraMiddle3PixelData[pixelDataBuffersIndex + 3] = 255; // a - ignored

            Color middle4Color = hsvMiddle4.ToColor();
            bgraMiddle4PixelData[pixelDataBuffersIndex] = middle4Color.B; // b
            bgraMiddle4PixelData[pixelDataBuffersIndex + 1] = middle4Color.G; // g
            bgraMiddle4PixelData[pixelDataBuffersIndex + 2] = middle4Color.R; // r
            bgraMiddle4PixelData[pixelDataBuffersIndex + 3] = 255; // a - ignored
        }

        Color maxColor = hsvMax.ToColor();
        bgraMaxPixelData[pixelDataBuffersIndex] = maxColor.B; // b
        bgraMaxPixelData[pixelDataBuffersIndex + 1] = maxColor.G; // g
        bgraMaxPixelData[pixelDataBuffersIndex + 2] = maxColor.R; // r
        bgraMaxPixelData[pixelDataBuffersIndex + 3] = 255; // a - ignored
    }

    private void FillPixelForRing(double x, double y, double radius, HsvColor baseHsv, ColorSpectrumComponents components,
                                  double minHue, double maxHue, double minSaturation, double maxSaturation, double minValue,
                                  double maxValue, byte[] bgraMinPixelData, byte[] bgraMiddle1PixelData, byte[] bgraMiddle2PixelData,
                                  byte[] bgraMiddle3PixelData, byte[] bgraMiddle4PixelData, byte[] bgraMaxPixelData,
                                  HsvColor[] newHsvValues, int pixelDataBuffersIndex, int newHsvValuesArrayIndex)
    {
        minSaturation /= 100.0;
        maxSaturation /= 100.0;
        minValue /= 100.0;
        maxValue /= 100.0;

        double distanceFromRadius = Math.Sqrt(Math.Pow(x - radius, 2) + Math.Pow(y - radius, 2));

        double xToUse = x;
        double yToUse = y;

        // If we're outside the ring, then we want the pixel to appear as blank.
        // However, to avoid issues with rounding errors, we'll act as though this point
        // is on the edge of the ring for the purposes of returning an HSL value.
        // That way, hittesting on the edges will always return the correct value.
        if (distanceFromRadius > radius)
        {
            xToUse = (radius / distanceFromRadius) * (x - radius) + radius;
            yToUse = (radius / distanceFromRadius) * (y - radius) + radius;
            distanceFromRadius = radius;
        }

        HsvColor hsvMin = baseHsv;
        HsvColor hsvMiddle1 = baseHsv;
        HsvColor hsvMiddle2 = baseHsv;
        HsvColor hsvMiddle3 = baseHsv;
        HsvColor hsvMiddle4 = baseHsv;
        HsvColor hsvMax = baseHsv;

        double r = 1 - distanceFromRadius / radius;

        double theta = Math.Atan2(radius - yToUse, radius - xToUse) * 180.0 / Math.PI;
        theta += 180.0;
        theta = Math.Floor(theta);

        while (theta > 360)
        {
            theta -= 360;
        }

        double thetaPercent = theta / 360;

        switch (components)
        {
            case ColorSpectrumComponents.HueValue:
                hsvMin.Hue = hsvMiddle1.Hue = hsvMiddle2.Hue = hsvMiddle3.Hue = hsvMiddle4.Hue = hsvMax.Hue = minHue + thetaPercent * (maxHue - minHue);
                hsvMin.Value = hsvMiddle1.Value = hsvMiddle2.Value = hsvMiddle3.Value = hsvMiddle4.Value = hsvMax.Value = minValue + r * (maxValue - minValue);
                hsvMin.Saturation = 0;
                hsvMax.Saturation = 1;
                break;

            case ColorSpectrumComponents.HueSaturation:
                hsvMin.Hue = hsvMiddle1.Hue = hsvMiddle2.Hue = hsvMiddle3.Hue = hsvMiddle4.Hue = hsvMax.Hue = minHue + thetaPercent * (maxHue - minHue);
                hsvMin.Saturation = hsvMiddle1.Saturation = hsvMiddle2.Saturation = hsvMiddle3.Saturation = hsvMiddle4.Saturation = hsvMax.Saturation = minSaturation + r * (maxSaturation - minSaturation);
                hsvMin.Value = 0;
                hsvMax.Value = 1;
                break;

            case ColorSpectrumComponents.ValueHue:
                hsvMin.Value = hsvMiddle1.Value = hsvMiddle2.Value = hsvMiddle3.Value = hsvMiddle4.Value = hsvMax.Value = minValue + thetaPercent * (maxValue - minValue);
                hsvMin.Hue = hsvMiddle1.Hue = hsvMiddle2.Hue = hsvMiddle3.Hue = hsvMiddle4.Hue = hsvMax.Hue = minHue + r * (maxHue - minHue);
                hsvMin.Saturation = 0;
                hsvMax.Saturation = 1;
                break;

            case ColorSpectrumComponents.ValueSaturation:
                hsvMin.Value = hsvMiddle1.Value = hsvMiddle2.Value = hsvMiddle3.Value = hsvMiddle4.Value = hsvMax.Value = minValue + thetaPercent * (maxValue - minValue);
                hsvMin.Saturation = hsvMiddle1.Saturation = hsvMiddle2.Saturation = hsvMiddle3.Saturation = hsvMiddle4.Saturation = hsvMax.Saturation = minSaturation + r * (maxSaturation - minSaturation);
                hsvMin.Hue = 0;
                hsvMiddle1.Hue = 60;
                hsvMiddle2.Hue = 120;
                hsvMiddle3.Hue = 180;
                hsvMiddle4.Hue = 240;
                hsvMax.Hue = 300;
                break;

            case ColorSpectrumComponents.SaturationHue:
                hsvMin.Saturation = hsvMiddle1.Saturation = hsvMiddle2.Saturation = hsvMiddle3.Saturation = hsvMiddle4.Saturation = hsvMax.Saturation = minSaturation + thetaPercent * (maxSaturation - minSaturation);
                hsvMin.Hue = hsvMiddle1.Hue = hsvMiddle2.Hue = hsvMiddle3.Hue = hsvMiddle4.Hue = hsvMax.Hue = minHue + r * (maxHue - minHue);
                hsvMin.Value = 0;
                hsvMax.Value = 1;
                break;

            case ColorSpectrumComponents.SaturationValue:
                hsvMin.Saturation = hsvMiddle1.Saturation = hsvMiddle2.Saturation = hsvMiddle3.Saturation = hsvMiddle4.Saturation = hsvMax.Saturation = minSaturation + thetaPercent * (maxSaturation - minSaturation);
                hsvMin.Value = hsvMiddle1.Value = hsvMiddle2.Value = hsvMiddle3.Value = hsvMiddle4.Value = hsvMax.Value = minValue + r * (maxValue - minValue);
                hsvMin.Hue = 0;
                hsvMiddle1.Hue = 60;
                hsvMiddle2.Hue = 120;
                hsvMiddle3.Hue = 180;
                hsvMiddle4.Hue = 240;
                hsvMax.Hue = 300;
                break;
        }

        // If saturation is an axis in the spectrum with hue, or value is an axis, then we want
        // that axis to go from maximum at the top to minimum at the bottom,
        // or maximum at the outside to minimum at the inside in the case of the ring configuration,
        // so we'll invert the number before assigning the HSL value to the array.
        // Otherwise, we'll have a very narrow section in the middle that actually has meaningful hue
        // in the case of the ring configuration.
        if (components == ColorSpectrumComponents.HueSaturation || components == ColorSpectrumComponents.SaturationHue)
        {
            hsvMin.Saturation = maxSaturation - hsvMin.Saturation + minSaturation;
            hsvMiddle1.Saturation = maxSaturation - hsvMiddle1.Saturation + minSaturation;
            hsvMiddle2.Saturation = maxSaturation - hsvMiddle2.Saturation + minSaturation;
            hsvMiddle3.Saturation = maxSaturation - hsvMiddle3.Saturation + minSaturation;
            hsvMiddle4.Saturation = maxSaturation - hsvMiddle4.Saturation + minSaturation;
            hsvMax.Saturation = maxSaturation - hsvMax.Saturation + minSaturation;
        }
        else
        {
            hsvMin.Value = maxValue - hsvMin.Value + minValue;
            hsvMiddle1.Value = maxValue - hsvMiddle1.Value + minValue;
            hsvMiddle2.Value = maxValue - hsvMiddle2.Value + minValue;
            hsvMiddle3.Value = maxValue - hsvMiddle3.Value + minValue;
            hsvMiddle4.Value = maxValue - hsvMiddle4.Value + minValue;
            hsvMax.Value = maxValue - hsvMax.Value + minValue;
        }

        newHsvValues[newHsvValuesArrayIndex] = hsvMin;

        Color minColor = hsvMin.ToColor();
        bgraMinPixelData[pixelDataBuffersIndex] = minColor.B; // b
        bgraMinPixelData[pixelDataBuffersIndex + 1] = minColor.G; // g
        bgraMinPixelData[pixelDataBuffersIndex + 2] = minColor.R; // r
        bgraMinPixelData[pixelDataBuffersIndex + 3] = 255; // a - ignored

        // We'll only save pixel data for the middle bitmaps if our third dimension is hue.
        if (components == ColorSpectrumComponents.ValueSaturation || components == ColorSpectrumComponents.SaturationValue)
        {
            Color middle1Color = hsvMiddle1.ToColor();
            bgraMiddle1PixelData[pixelDataBuffersIndex] = middle1Color.B; // b
            bgraMiddle1PixelData[pixelDataBuffersIndex + 1] = middle1Color.G; // g
            bgraMiddle1PixelData[pixelDataBuffersIndex + 2] = middle1Color.R; // r
            bgraMiddle1PixelData[pixelDataBuffersIndex + 3] = 255; // a - ignored

            Color middle2Color = hsvMiddle2.ToColor();
            bgraMiddle2PixelData[pixelDataBuffersIndex] = middle2Color.B; // b
            bgraMiddle2PixelData[pixelDataBuffersIndex + 1] = middle2Color.G; // g
            bgraMiddle2PixelData[pixelDataBuffersIndex + 2] = middle2Color.R; // r
            bgraMiddle2PixelData[pixelDataBuffersIndex + 3] = 255; // a - ignored

            Color middle3Color = hsvMiddle3.ToColor();
            bgraMiddle3PixelData[pixelDataBuffersIndex] = middle3Color.B; // b
            bgraMiddle3PixelData[pixelDataBuffersIndex + 1] = middle3Color.G; // g
            bgraMiddle3PixelData[pixelDataBuffersIndex + 2] = middle3Color.R; // r
            bgraMiddle3PixelData[pixelDataBuffersIndex + 3] = 255; // a - ignored

            Color middle4Color = hsvMiddle4.ToColor();
            bgraMiddle4PixelData[pixelDataBuffersIndex] = middle4Color.B; // b
            bgraMiddle4PixelData[pixelDataBuffersIndex + 1] = middle4Color.G; // g
            bgraMiddle4PixelData[pixelDataBuffersIndex + 2] = middle4Color.R; // r
            bgraMiddle4PixelData[pixelDataBuffersIndex + 3] = 255; // a - ignored
        }

        Color maxColor = hsvMax.ToColor();
        bgraMaxPixelData[pixelDataBuffersIndex] = maxColor.B; // b
        bgraMaxPixelData[pixelDataBuffersIndex + 1] = maxColor.G; // g
        bgraMaxPixelData[pixelDataBuffersIndex + 2] = maxColor.R; // r
        bgraMaxPixelData[pixelDataBuffersIndex + 3] = 255; // a - ignored
    }

    private void SetColor()
    {
        HsvColor hsvColor = HsvColor;

        _updatingColor = true;
        Color = hsvColor.ToColor();

        _updatingColor = false;

        UpdateEllipse();
        UpdateBitmapSources();
        RaiseColorChanged();
    }

    private void UpdateBitmapSources()
    {
        if (_spectrumOverlayRectangle == null || _spectrumOverlayEllipse == null)
        {
            return;
        }

        var hsvColor = HsvColor;
        ColorSpectrumComponents components = Components;

        // We'll set the base image and the overlay image based on which component is our third dimension.
        // If it's saturation or luminosity, then the base image is that dimension at its minimum value,
        // while the overlay image is that dimension at its maximum value.
        // If it's hue, then we'll figure out where in the color wheel we are, and then use the two
        // colors on either side of our position as our base image and overlay image.
        // For example, if our hue is orange, then the base image would be red and the overlay image yellow.
        switch (components)
        {
            case ColorSpectrumComponents.HueValue:
            case ColorSpectrumComponents.ValueHue:
                {
                    //if (SharedHelpers::IsRS2OrHigher())
                    //{
                    //    if (!m_saturationMinimumSurface ||
                    //        !m_saturationMaximumSurface)
                    //    {
                    //        return;
                    //    }

                    //    winrt::SpectrumBrush spectrumBrush{ winrt::make<SpectrumBrush>() };

                    //    spectrumBrush.MinSurface(m_saturationMinimumSurface);
                    //    spectrumBrush.MaxSurface(m_saturationMaximumSurface);
                    //    spectrumBrush.MaxSurfaceOpacity(hsv::GetSaturation(hsvColor));
                    //    spectrumRectangle.Fill(spectrumBrush);
                    //    spectrumEllipse.Fill(spectrumBrush);
                    //}
                    //else
                    //{
                    if (_saturationMinimumBitmap == null || _saturationMaximumBitmap == null)
                    {
                        return;
                    }

                    ImageBrush spectrumBrush = new ImageBrush(_saturationMinimumBitmap);
                    ImageBrush spectrumOverlayBrush = new ImageBrush(_saturationMaximumBitmap);

                    _spectrumOverlayRectangle.Opacity = hsvColor.Saturation;
                    _spectrumOverlayEllipse.Opacity = hsvColor.Saturation;
                    _spectrumRectangle.Fill = spectrumBrush;
                    _spectrumEllipse.Fill = spectrumBrush;
                    _spectrumOverlayRectangle.Fill = spectrumOverlayBrush;
                    _spectrumOverlayRectangle.Fill = spectrumOverlayBrush;
                    //}
                    break;
                }
            case ColorSpectrumComponents.HueSaturation:
            case ColorSpectrumComponents.SaturationHue:
                {
                    //if (SharedHelpers::IsRS2OrHigher())
                    //{
                    //    if (!m_valueSurface)
                    //    {
                    //        return;
                    //    }

                    //    winrt::SpectrumBrush spectrumBrush{ winrt::make<SpectrumBrush>() };

                    //    spectrumBrush.MinSurface(m_valueSurface);
                    //    spectrumBrush.MaxSurface(m_valueSurface);
                    //    spectrumBrush.MaxSurfaceOpacity(1);
                    //    spectrumRectangle.Fill(spectrumBrush);
                    //    spectrumEllipse.Fill(spectrumBrush);
                    //}
                    //else
                    //{
                    if (_valueBitmap == null)
                    {
                        return;
                    }

                    ImageBrush spectrumBrush = new ImageBrush(_valueBitmap);
                    ImageBrush spectrumOverlayBrush = new ImageBrush(_valueBitmap);

                    _spectrumOverlayRectangle.Opacity = 1;
                    _spectrumOverlayEllipse.Opacity = 1;
                    _spectrumRectangle.Fill = spectrumBrush;
                    _spectrumEllipse.Fill = spectrumBrush;
                    _spectrumOverlayRectangle.Fill = spectrumOverlayBrush;
                    //_spectrumOverlayRectangle.Fill = spectrumOverlayBrush; Original code was duplicated, must be this next line instead
                    _spectrumOverlayEllipse.Fill = spectrumOverlayBrush;
                    //}
                    break;
                }

            case ColorSpectrumComponents.ValueSaturation:
            case ColorSpectrumComponents.SaturationValue:
                {
                    //if (SharedHelpers::IsRS2OrHigher())
                    //{
                    //    if (!m_hueRedSurface ||
                    //        !m_hueYellowSurface ||
                    //        !m_hueGreenSurface ||
                    //        !m_hueCyanSurface ||
                    //        !m_hueBlueSurface ||
                    //        !m_huePurpleSurface)
                    //    {
                    //        return;
                    //    }

                    //    winrt::SpectrumBrush spectrumBrush{ winrt::make<SpectrumBrush>() };

                    //    const double sextant = hsv::GetHue(hsvColor) / 60.0;

                    //    if (sextant < 1)
                    //    {
                    //        spectrumBrush.MinSurface(m_hueRedSurface);
                    //        spectrumBrush.MaxSurface(m_hueYellowSurface);
                    //    }
                    //    else if (sextant >= 1 && sextant < 2)
                    //    {
                    //        spectrumBrush.MinSurface(m_hueYellowSurface);
                    //        spectrumBrush.MaxSurface(m_hueGreenSurface);
                    //    }
                    //    else if (sextant >= 2 && sextant < 3)
                    //    {
                    //        spectrumBrush.MinSurface(m_hueGreenSurface);
                    //        spectrumBrush.MaxSurface(m_hueCyanSurface);
                    //    }
                    //    else if (sextant >= 3 && sextant < 4)
                    //    {
                    //        spectrumBrush.MinSurface(m_hueCyanSurface);
                    //        spectrumBrush.MaxSurface(m_hueBlueSurface);
                    //    }
                    //    else if (sextant >= 4 && sextant < 5)
                    //    {
                    //        spectrumBrush.MinSurface(m_hueBlueSurface);
                    //        spectrumBrush.MaxSurface(m_huePurpleSurface);
                    //    }
                    //    else
                    //    {
                    //        spectrumBrush.MinSurface(m_huePurpleSurface);
                    //        spectrumBrush.MaxSurface(m_hueRedSurface);
                    //    }

                    //    spectrumBrush.MaxSurfaceOpacity(sextant - static_cast<int>(sextant));
                    //    spectrumRectangle.Fill(spectrumBrush);
                    //    spectrumEllipse.Fill(spectrumBrush);
                    //}
                    //else
                    //{
                    if (_hueRedBitmap == null || _hueYellowBitmap == null || _hueGreenBitmap == null || _hueCyanBitmap == null ||
                        _hueBlueBitmap == null || _huePurpleBitmap == null)
                    {
                        return;
                    }

                    ImageBrush spectrumBrush = new ImageBrush();
                    ImageBrush spectrumOverlayBrush = new ImageBrush();

                    double sextant = hsvColor.Hue / 60.0;

                    if (sextant < 1)
                    {
                        spectrumBrush.ImageSource = _hueRedBitmap;
                        spectrumOverlayBrush.ImageSource = _hueYellowBitmap;
                    }
                    else if (sextant >= 1 && sextant < 2)
                    {
                        spectrumBrush.ImageSource = _hueYellowBitmap;
                        spectrumOverlayBrush.ImageSource = _hueGreenBitmap;
                    }
                    else if (sextant >= 2 && sextant < 3)
                    {
                        spectrumBrush.ImageSource = _hueGreenBitmap;
                        spectrumOverlayBrush.ImageSource = _hueCyanBitmap;
                    }
                    else if (sextant >= 3 && sextant < 4)
                    {
                        spectrumBrush.ImageSource = _hueCyanBitmap;
                        spectrumOverlayBrush.ImageSource = _hueBlueBitmap;
                    }
                    else if (sextant >= 4 && sextant < 5)
                    {
                        spectrumBrush.ImageSource = _hueBlueBitmap;
                        spectrumOverlayBrush.ImageSource = _huePurpleBitmap;
                    }
                    else
                    {
                        spectrumBrush.ImageSource = _huePurpleBitmap;
                        spectrumOverlayBrush.ImageSource = _hueRedBitmap;
                    }

                    _spectrumOverlayRectangle.Opacity = sextant - (int)sextant;
                    _spectrumOverlayEllipse.Opacity = sextant - (int)sextant;
                    _spectrumRectangle.Fill = spectrumBrush;
                    _spectrumEllipse.Fill = spectrumBrush;
                    _spectrumOverlayRectangle.Fill = spectrumOverlayBrush;
                    _spectrumOverlayRectangle.Fill = spectrumOverlayBrush;

                    //}
                    break;
                }
        }
    }

    private void UpdateColor(HsvColor newHsv)
    {
        _updatingColor = true;
        _updatingHsvColor = true;

        Color newColor = newHsv.ToColor();

        Color = newColor;
        HsvColor = newHsv;

        UpdateEllipse();
        UpdateSelectionEllipseShouldBeLightProperty();
        UpdateVisualState(true /* useTransitions */);

        _updatingHsvColor = false;
        _updatingColor = false;

        RaiseColorChanged();
    }

    private void UpdateColorFromPoint(Point point)
    {
        // If we haven't initialized our HSV value array yet, then we should just ignore any user input -
        // we don't yet know what to do with it.
        if (_hsvValues.Length == 0)
        {
            return;
        }

        double xPosition = point.X;
        double yPosition = point.Y;
        double radius = Math.Min(_imageWidthFromLastBitmapCreation, _imageHeightFromLastBitmapCreation) / 2;
        double distanceFromRadius = Math.Sqrt(Math.Pow(xPosition - radius, 2) + Math.Pow(yPosition - radius, 2));

        var shape = Shape;

        // If the point is outside the circle, we should bring it back into the circle.
        if (distanceFromRadius > radius && shape == ColorSpectrumShape.Ring)
        {
            xPosition = (radius / distanceFromRadius) * (xPosition - radius) + radius;
            yPosition = (radius / distanceFromRadius) * (yPosition - radius) + radius;
        }

        // Now we need to find the index into the array of HSL values at each point in the spectrum m_image.
        int x = (int)Math.Round(xPosition);
        int y = (int)Math.Round(yPosition);
        int width = (int)Math.Round(_imageWidthFromLastBitmapCreation);

        if (x < 0)
        {
            x = 0;
        }
        else if (x >= _imageWidthFromLastBitmapCreation)
        {
            x = ((int)Math.Round(_imageWidthFromLastBitmapCreation)) - 1;
        }

        if (y < 0)
        {
            y = 0;
        }
        else if (y >= _imageHeightFromLastBitmapCreation)
        {
            y = ((int)Math.Round(_imageHeightFromLastBitmapCreation)) - 1;
        }

        // The gradient image contains two dimensions of HSL information, but not the third.
        // We should keep the third where it already was.
        HsvColor hsvAtPoint = _hsvValues[y * width + x];

        var components = Components;
        var hsvColor = HsvColor;

        switch (components)
        {
            case ColorSpectrumComponents.HueValue:
            case ColorSpectrumComponents.ValueHue:
                hsvAtPoint.Saturation = hsvColor.Saturation;
                break;

            case ColorSpectrumComponents.HueSaturation:
            case ColorSpectrumComponents.SaturationHue:
                hsvAtPoint.Value = hsvColor.Value;
                break;

            case ColorSpectrumComponents.ValueSaturation:
            case ColorSpectrumComponents.SaturationValue:
                hsvAtPoint.Hue = hsvColor.Hue;
                break;
        }

        UpdateColor(hsvAtPoint);
    }

    private void UpdateEllipse()
    {
        if (_selectionEllipsePanel == null)
        {
            return;
        }

        // If we don't have an image size yet, we shouldn't be showing the ellipse.
        if (_imageWidthFromLastBitmapCreation == 0 || _imageHeightFromLastBitmapCreation == 0)
        {
            _selectionEllipsePanel.Visibility = Visibility.Collapsed;
            return;
        }
        else
        {
            _selectionEllipsePanel.Visibility = Visibility.Visible;
        }

        double xPosition;
        double yPosition;

        HsvColor hsvColor = HsvColor;

        hsvColor.Hue = MathHelpers.Clamp(hsvColor.Hue, _minHueFromLastBitmapCreation, _maxHueFromLastBitmapCreation);

        hsvColor.Saturation = MathHelpers.Clamp(hsvColor.Saturation, _minSaturationFromLastBitmapCreation,
                                                _maxSaturationFromLastBitmapCreation);

        hsvColor.Value = MathHelpers.Clamp(hsvColor.Value, _minValueFromLastBitmapCreation, _maxValueFromLastBitmapCreation);

        if (_shapeFromLastBitmapCreation == ColorSpectrumShape.Box)
        {
            double xPercent = 0;
            double yPercent = 0;

            double hPercent = (hsvColor.Hue - _minHueFromLastBitmapCreation) /
                              (_maxHueFromLastBitmapCreation - _minHueFromLastBitmapCreation);

            double sPercent = (hsvColor.Saturation * 100.0 - _minSaturationFromLastBitmapCreation) /
                              (_maxSaturationFromLastBitmapCreation - _minSaturationFromLastBitmapCreation);

            double vPercent = (hsvColor.Value * 100.0 - _minValueFromLastBitmapCreation) /
                              (_maxValueFromLastBitmapCreation - _minValueFromLastBitmapCreation);

            // In the case where saturation was an axis in the spectrum with hue, or value is an axis, full stop,
            // we inverted the direction of that axis in order to put more hue on the outside of the ring,
            // so we need to do similarly here when positioning the ellipse.
            if (_componentsFromLastBitmapCreation == ColorSpectrumComponents.HueSaturation ||
                _componentsFromLastBitmapCreation == ColorSpectrumComponents.SaturationHue)
            {
                sPercent = 1 - sPercent;
            }
            else
            {
                vPercent = 1 - vPercent;
            }

            switch (_componentsFromLastBitmapCreation)
            {
                case ColorSpectrumComponents.HueValue:
                    xPercent = hPercent;
                    yPercent = vPercent;
                    break;

                case ColorSpectrumComponents.HueSaturation:
                    xPercent = hPercent;
                    yPercent = sPercent;
                    break;

                case ColorSpectrumComponents.ValueHue:
                    xPercent = vPercent;
                    yPercent = hPercent;
                    break;

                case ColorSpectrumComponents.ValueSaturation:
                    xPercent = vPercent;
                    yPercent = sPercent;
                    break;

                case ColorSpectrumComponents.SaturationHue:
                    xPercent = sPercent;
                    yPercent = hPercent;
                    break;

                case ColorSpectrumComponents.SaturationValue:
                    xPercent = sPercent;
                    yPercent = vPercent;
                    break;
            }

            xPosition = _imageWidthFromLastBitmapCreation * xPercent;
            yPosition = _imageHeightFromLastBitmapCreation * yPercent;
        }
        else
        {
            double thetaValue = 0;
            double rValue = 0;

            double hueThetaValue = _maxHueFromLastBitmapCreation != _minHueFromLastBitmapCreation
                ? 360 * (hsvColor.Hue - _minHueFromLastBitmapCreation) / (_maxHueFromLastBitmapCreation - _minHueFromLastBitmapCreation)
                : 0;
            double saturationThetaValue =
                _maxSaturationFromLastBitmapCreation != _minSaturationFromLastBitmapCreation
                ? 360 * (hsvColor.Saturation * 100.0 - _minSaturationFromLastBitmapCreation) / (_maxSaturationFromLastBitmapCreation - _minSaturationFromLastBitmapCreation)
                : 0;
            double valueThetaValue =
                _maxValueFromLastBitmapCreation != _minValueFromLastBitmapCreation
                ? 360 * (hsvColor.Value * 100.0 - _minValueFromLastBitmapCreation) / (_maxValueFromLastBitmapCreation - _minValueFromLastBitmapCreation)
                : 0;
            double hueRValue = _maxHueFromLastBitmapCreation != _minHueFromLastBitmapCreation
                ? (hsvColor.Hue - _minHueFromLastBitmapCreation) / (_maxHueFromLastBitmapCreation - _minHueFromLastBitmapCreation) - 1
                : 0;
            double saturationRValue = _maxSaturationFromLastBitmapCreation != _minSaturationFromLastBitmapCreation
                ? (hsvColor.Saturation * 100.0 - _minSaturationFromLastBitmapCreation) / (_maxSaturationFromLastBitmapCreation - _minSaturationFromLastBitmapCreation) - 1
                : 0;
            double valueRValue = _maxValueFromLastBitmapCreation != _minValueFromLastBitmapCreation
                ? (hsvColor.Value * 100.0 - _minValueFromLastBitmapCreation) / (_maxValueFromLastBitmapCreation - _minValueFromLastBitmapCreation) - 1
                : 0;

            // In the case where saturation was an axis in the spectrum with hue, or value is an axis, full stop,
            // we inverted the direction of that axis in order to put more hue on the outside of the ring,
            // so we need to do similarly here when positioning the ellipse.
            if (_componentsFromLastBitmapCreation == ColorSpectrumComponents.HueSaturation || _componentsFromLastBitmapCreation == ColorSpectrumComponents.SaturationHue)
            {
                saturationThetaValue = 360 - saturationThetaValue;
                saturationRValue = -saturationRValue - 1;
            }
            else
            {
                valueThetaValue = 360 - valueThetaValue;
                valueRValue = -valueRValue - 1;
            }

            switch (_componentsFromLastBitmapCreation)
            {
                case ColorSpectrumComponents.HueValue:
                    thetaValue = hueThetaValue;
                    rValue = valueRValue;
                    break;

                case ColorSpectrumComponents.HueSaturation:
                    thetaValue = hueThetaValue;
                    rValue = saturationRValue;
                    break;

                case ColorSpectrumComponents.ValueHue:
                    thetaValue = valueThetaValue;
                    rValue = hueRValue;
                    break;

                case ColorSpectrumComponents.ValueSaturation:
                    thetaValue = valueThetaValue;
                    rValue = saturationRValue;
                    break;

                case ColorSpectrumComponents.SaturationHue:
                    thetaValue = saturationThetaValue;
                    rValue = hueRValue;
                    break;

                case ColorSpectrumComponents.SaturationValue:
                    thetaValue = saturationThetaValue;
                    rValue = valueRValue;
                    break;
            }

            double radius = Math.Min(_imageWidthFromLastBitmapCreation, _imageHeightFromLastBitmapCreation) / 2;

            xPosition = (Math.Cos((thetaValue * Math.PI / 180) + Math.PI) * radius * rValue) + radius;
            yPosition = (Math.Sin((thetaValue * Math.PI / 180) + Math.PI) * radius * rValue) + radius;
        }

        Canvas.SetLeft(_selectionEllipsePanel, xPosition - (_selectionEllipsePanel.Width / 2));
        Canvas.SetTop(_selectionEllipsePanel, yPosition - (_selectionEllipsePanel.Height / 2));

        var rect = new Rect(xPosition - (_colorNameToolTip.ActualWidth / 2), yPosition - (_colorNameToolTip.ActualHeight / 2), 0, 0);
        _colorNameToolTip.PlacementRectangle = rect;

        UpdateVisualState(true /* useTransitions */);
    }

    private void UpdateSelectionEllipseShouldBeLightProperty()
    {
        // The selection ellipse should be light if and only if the chosen color
        // contrasts more with black than it does with white.
        // To find how much something contrasts with white, we use the equation
        // for relative luminance, which is given by
        //
        // L = 0.2126 * Rg + 0.7152 * Gg + 0.0722 * Bg
        //
        // where Xg = { X/3294 if X <= 10, (R/269 + 0.0513)^2.4 otherwise }
        //
        // If L is closer to 1, then the color is closer to white; if it is closer to 0,
        // then the color is closer to black.  This is based on the fact that the human
        // eye perceives green to be much brighter than red, which in turn is perceived to be
        // brighter than blue.
        //
        // If the third dimension is value, then we won't be updating the spectrum's displayed colors,
        // so in that case we should use a value of 1 when considering the backdrop
        // for the selection ellipse.
        Color displayedColor;

        if (Components == ColorSpectrumComponents.HueSaturation || Components == ColorSpectrumComponents.SaturationHue)
        {
            var hsvColor = HsvColor;
            hsvColor.Value = 1;
            displayedColor = hsvColor.ToColor();
        }
        else
        {
            displayedColor = Color;
        }

        double rg = displayedColor.R <= 10 ? displayedColor.R / 3294.0 : Math.Pow(displayedColor.R / 269.0 + 0.0513, 2.4);
        double gg = displayedColor.G <= 10 ? displayedColor.G / 3294.0 : Math.Pow(displayedColor.G / 269.0 + 0.0513, 2.4);
        double bg = displayedColor.B <= 10 ? displayedColor.B / 3294.0 : Math.Pow(displayedColor.B / 269.0 + 0.0513, 2.4);

        SelectionEllipseShouldBeLight = 0.2126 * rg + 0.7152 * gg + 0.0722 * bg <= 0.5;
    }

    private void UpdateShouldShowLargeSelectionProperty()
    {
        ShouldShowLargeSelection = _shouldShowLargeSelection;
    }

    private void UpdateVisualState(bool useTransitions)
    {
        //if (_isPointerPressed)
        //{
        //    VisualStateManager.GoToState(thisAsControl, m_shouldShowLargeSelection ? L"PressedLarge" : L"Pressed", useTransitions);
        //}
        //else if (m_isPointerOver)
        //{
        //    winrt::VisualStateManager::GoToState(thisAsControl, L"PointerOver", useTransitions);
        //}
        //else
        //{
        //    winrt::VisualStateManager::GoToState(thisAsControl, L"Normal", useTransitions);
        //}

        //VisualStateManager.GoToState(this, _shapeFromLastBitmapCreation == ColorSpectrumShape.Box ? "BoxSelected" : "RingSelected", useTransitions);
        //winrt::VisualStateManager::GoToState(thisAsControl, SelectionEllipseShouldBeLight() ? L"SelectionEllipseLight" : L"SelectionEllipseDark", useTransitions);

        //if (IsEnabled() && FocusState() != winrt::FocusState::Unfocused)
        //{
        //    if (FocusState() == winrt::FocusState::Pointer)
        //    {
        //        winrt::VisualStateManager::GoToState(thisAsControl, L"PointerFocused", useTransitions);
        //    }
        //    else
        //    {
        //        winrt::VisualStateManager::GoToState(thisAsControl, L"Focused", useTransitions);
        //    }
        //}
        //else
        //{
        //    winrt::VisualStateManager::GoToState(thisAsControl, L"Unfocused", useTransitions);
        //}
    }
    #endregion

    #region Protected
    protected override void OnGotFocus(RoutedEventArgs args)
    {
        base.OnGotFocus(args);

        // We only want to bother with the color name tool tip if we can provide color names.
        if (_colorNameToolTip != null)
        {
            //if (DownlevelHelper::ToDisplayNameExists())
            //{
            _colorNameToolTip.IsOpen = true;
            //}
        }

        UpdateVisualState(true /* useTransitions */);
    }

    protected override void OnKeyDown(KeyEventArgs args)
    {
        if (args.Key != Key.Left && args.Key != Key.Right && args.Key != Key.Up && args.Key != Key.Down)
        {
            base.OnKeyDown(args);
            return;
        }

        bool isControlDown = Keyboard.Modifiers == ModifierKeys.Control;
        ColorPickerHsvChannel incrementChannel = ColorPickerHsvChannel.Hue;

        bool isSaturationValue = false;

        if (args.Key == Key.Left || args.Key == Key.Right)
        {
            switch (Components)
            {
                case ColorSpectrumComponents.HueSaturation:
                case ColorSpectrumComponents.HueValue:
                    incrementChannel = ColorPickerHsvChannel.Hue;
                    break;

                case ColorSpectrumComponents.SaturationValue:
                    isSaturationValue = true;
                    goto case ColorSpectrumComponents.SaturationHue; // fallthrough is explicitly wanted

                case ColorSpectrumComponents.SaturationHue:
                    incrementChannel = ColorPickerHsvChannel.Saturation;
                    break;

                case ColorSpectrumComponents.ValueHue:
                case ColorSpectrumComponents.ValueSaturation:
                    incrementChannel = ColorPickerHsvChannel.Value;
                    break;
            }
        }
        else if (args.Key == Key.Up || args.Key == Key.Down)
        {
            switch (Components)
            {
                case ColorSpectrumComponents.SaturationHue:
                case ColorSpectrumComponents.ValueHue:
                    incrementChannel = ColorPickerHsvChannel.Hue;
                    break;

                case ColorSpectrumComponents.HueSaturation:
                case ColorSpectrumComponents.ValueSaturation:
                    incrementChannel = ColorPickerHsvChannel.Saturation;
                    break;

                case ColorSpectrumComponents.SaturationValue:
                    isSaturationValue = true;
                    goto case ColorSpectrumComponents.HueValue; // fallthrough is explicitly wanted

                case ColorSpectrumComponents.HueValue:
                    incrementChannel = ColorPickerHsvChannel.Value;
                    break;
            }
        }

        double minBound = 0;
        double maxBound = 0;

        switch (incrementChannel)
        {
            case ColorPickerHsvChannel.Hue:
                minBound = MinHue;
                maxBound = MaxHue;
                break;

            case ColorPickerHsvChannel.Saturation:
                minBound = MinSaturation;
                maxBound = MaxSaturation;
                break;

            case ColorPickerHsvChannel.Value:
                minBound = MinValue;
                maxBound = MaxValue;
                break;
        }

        // The order of saturation and value in the spectrum is reversed - the max value is at the bottom while the min value is at the top -
        // so we want left and up to be lower for hue, but higher for saturation and value.
        // This will ensure that the icon always moves in the direction of the key press.
        IncrementDirection direction = (incrementChannel == ColorPickerHsvChannel.Hue && (args.Key == Key.Left || args.Key == Key.Up)) ||
                                       (incrementChannel != ColorPickerHsvChannel.Hue && (args.Key == Key.Right || args.Key == Key.Down))
                                       ? IncrementDirection.Lower
                                       : IncrementDirection.Higher;

        // Image is flipped in RightToLeft, so we need to invert direction in that case.
        // The combination saturation and value is also flipped, so we need to invert in that case too.
        // If both are false, we don't need to invert.
        // If both are true, we would invert twice, so not invert at all.
        if (FlowDirection == FlowDirection.RightToLeft != isSaturationValue && (args.Key == Key.Left || args.Key == Key.Right))
        {
            if (direction == IncrementDirection.Higher)
            {
                direction = IncrementDirection.Lower;
            }
            else
            {
                direction = IncrementDirection.Higher;
            }
        }

        IncrementAmount amount = isControlDown ? IncrementAmount.Large : IncrementAmount.Small;

        UpdateColor(ColorHelpers.IncrementColorChannel(HsvColor, incrementChannel, direction, amount, true /* shouldWrap */,
                                                       minBound, maxBound));
        args.Handled = true;
    }

    protected override void OnLostFocus(RoutedEventArgs args)
    {
        base.OnLostFocus(args);

        if (_colorNameToolTip != null)
        {
            _colorNameToolTip.IsOpen = false;
        }

        UpdateVisualState(true);
    }

    protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs args)
    {
        base.OnPropertyChanged(args);

        DependencyProperty property = args.Property;

        if (property == ColorProperty)
        {
            OnColorChanged(args);
        }
        else if (property == HsvColorProperty)
        {
            OnHsvColorChanged(args);
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
        else if (property == ShapeProperty)
        {
            OnShapeChanged(args);
        }
        else if (property == ComponentsProperty)
        {
            OnComponentsChanged(args);
        }
    }

    #endregion

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        _layoutRoot = (Grid)GetTemplateChild("LayoutRoot");
        _sizingGrid = (Grid)GetTemplateChild("SizingGrid");
        _spectrumRectangle = (Rectangle)GetTemplateChild("SpectrumRectangle");
        _spectrumEllipse = (Ellipse)GetTemplateChild("SpectrumEllipse");
        _spectrumOverlayRectangle = (Rectangle)GetTemplateChild("SpectrumOverlayRectangle");
        _spectrumOverlayEllipse = (Ellipse)GetTemplateChild("SpectrumOverlayEllipse");
        _inputTarget = (FrameworkElement)GetTemplateChild("InputTarget");
        _selectionEllipsePanel = (Panel)GetTemplateChild("SelectionEllipsePanel");
        _colorNameToolTip = (ToolTip)GetTemplateChild("ColorNameToolTip");

        _layoutRoot.SizeChanged += OnLayoutRootSizeChanged;

        _inputTarget.MouseDown += OnInputTargetMouseDown;
        _inputTarget.MouseEnter += OnInputTargetMouseEnter;
        _inputTarget.MouseLeave += OnInputTargetMouseLeave;
        _inputTarget.MouseMove += OnInputTargetMouseMove;
        _inputTarget.MouseUp += OnInputTargetMouseUp;

        _inputTarget.StylusDown += OnInputTargetStylusDown;
        _inputTarget.StylusEnter += OnInputTargetStylusEnter;
        _inputTarget.StylusLeave += OnInputTargetStylusLeave;
        _inputTarget.StylusMove += OnInputTargetStylusMove;
        _inputTarget.StylusUp += OnInputTargetStylusUp;

        _inputTarget.TouchDown += OnInputTargetTouchDown;
        _inputTarget.TouchEnter += OnInputTargetTouchEnter;
        _inputTarget.TouchLeave += OnInputTargetTouchLeave;
        _inputTarget.TouchMove += OnInputTargetTouchMove;
        _inputTarget.TouchUp += OnInputTargetTouchUp;

        if (_colorNameToolTip != null)
        {
            _colorNameToolTip.Content = ColorHelpers.GetNearestNamedColorName(Color);
        }

        //_selectionEllipsePanel.RegisterPropertyChangedCallback(winrt::FrameworkElement::FlowDirectionProperty(), { this, &ColorSpectrum::OnSelectionEllipseFlowDirectionChanged });

        // If we haven't yet created our bitmaps, do so now.
        if (_hsvValues.Length == 0)
        {
            CreateBitmapsAndColorMap();
        }

        UpdateEllipse();
        UpdateVisualState(false /* useTransitions */);
    }

    // TODO: Where was this needed?
    //public Rect GetBoundingRectangle()
    //{
    //    Rect localRect = new Rect(0, 0, _inputTarget.ActualWidth, _inputTarget.ActualHeight);

    //    var globalBounds = TransformToVisual(this/* NOTE: Check this replacement: nullptr */).TransformBounds(localRect);
    //    return SharedHelpers.ConvertDipsToPhysical(this, globalBounds);
    //}

    public void RaiseColorChanged()
    {
        Color newColor = Color;
        bool colorChanged = !_oldColor.Equals(newColor);
        bool areBothColorsBlack = (_oldColor.R == newColor.R && newColor.R == 0) || (_oldColor.G == newColor.G && newColor.G == 0) ||
                                  (_oldColor.B == newColor.B && newColor.B == 0);

        if (colorChanged || areBothColorsBlack)
        {
            var colorChangedEventArgs = new ColorChangedEventArgs(newColor, _oldColor);

            ColorChanged?.Invoke(this, colorChangedEventArgs);

            if (_colorNameToolTip != null)
            {
                // TODO: We need a faster way to get the color name, this causes some flickering when
                // repositioning the selection ellipse and its tooltip
                _colorNameToolTip.Content = ColorHelpers.GetNearestNamedColorName(newColor);
            }
        }
    } 
    #endregion
}
