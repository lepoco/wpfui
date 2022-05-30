// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPFUI.Controls;

// TODO: Handle backspace
// TODO: Handle editing
// TODO: Implement mask

/// <summary>
/// Text field for entering numbers with the possibility of specifying a pattern.
/// </summary>
public class NumberBox : WPFUI.Controls.TextBox
{
    // In both expressions, we allow the lonely characters '-', '.' and ',' so the numbers can be typed in real-time.

    /// <summary>
    /// Accepts a string of digits separated by a comma or period. Allows for a leading minus sign.
    /// </summary>
    private readonly string _decimalExpression = /* language=regex */ @"^\-?(\d+(?:[\.\,]|[\.\,]\d+)?)?$";

    /// <summary>
    /// Accepts a string of digits only. Allows for a leading minus sign.
    /// </summary>
    private readonly string _integerExpression = /* language=regex */ @"^\-?(\d+)*$";

    /// <summary>
    /// Property for <see cref="Value"/>.
    /// </summary>
    public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof(Value),
        typeof(double), typeof(NumberBox), new PropertyMetadata(0.0d));

    /// <summary>
    /// Property for <see cref="Step"/>.
    /// </summary>
    public static readonly DependencyProperty StepProperty = DependencyProperty.Register(nameof(Step),
        typeof(double), typeof(NumberBox), new PropertyMetadata(1.0d));

    /// <summary>
    /// Property for <see cref="Max"/>.
    /// </summary>
    public static readonly DependencyProperty MaxProperty = DependencyProperty.Register(nameof(Max),
        typeof(double), typeof(NumberBox), new PropertyMetadata(Double.MaxValue));

    /// <summary>
    /// Property for <see cref="Min"/>.
    /// </summary>
    public static readonly DependencyProperty MinProperty = DependencyProperty.Register(nameof(Min),
        typeof(double), typeof(NumberBox), new PropertyMetadata(Double.MinValue));

    /// <summary>
    /// Property for <see cref="DecimalPlaces"/>.
    /// </summary>
    public static readonly DependencyProperty DecimalPlacesProperty = DependencyProperty.Register(nameof(DecimalPlaces),
        typeof(int), typeof(NumberBox), new PropertyMetadata(2, OnDecimalPlacesChanged));

    /// <summary>
    /// Property for <see cref="Mask"/>.
    /// </summary>
    public static readonly DependencyProperty MaskProperty = DependencyProperty.Register(nameof(Mask),
        typeof(string), typeof(NumberBox), new PropertyMetadata(String.Empty));

    /// <summary>
    /// Property for <see cref="ControlsVisible"/>.
    /// </summary>
    public static readonly DependencyProperty ControlsVisibleProperty = DependencyProperty.Register(nameof(ControlsVisible),
        typeof(bool), typeof(NumberBox), new PropertyMetadata(true));

    /// <summary>
    /// Property for <see cref="IntegersOnly"/>.
    /// </summary>
    public static readonly DependencyProperty IntegersOnlyProperty = DependencyProperty.Register(nameof(IntegersOnly),
        typeof(bool), typeof(NumberBox), new PropertyMetadata(false));

    /// <summary>
    /// Routed event for <see cref="Incremented"/>.
    /// </summary>
    public static readonly RoutedEvent IncrementedEvent = EventManager.RegisterRoutedEvent(
        nameof(Incremented), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(NumberBox));

    /// <summary>
    /// Routed event for <see cref="Decremented"/>.
    /// </summary>
    public static readonly RoutedEvent DecrementedEvent = EventManager.RegisterRoutedEvent(
        nameof(Decremented), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(NumberBox));

    /// <summary>
    /// Property for <see cref="ButtonCommand"/>.
    /// </summary>
    public static readonly DependencyProperty ButtonCommandProperty =
        DependencyProperty.Register(nameof(ButtonCommand),
            typeof(Common.IRelayCommand), typeof(NumberBox), new PropertyMetadata(null));

    /// <summary>
    /// Current numeric value.
    /// </summary>
    public double Value
    {
        get => (double)GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    /// <summary>
    /// Gets or sets value by which the given number will be increased or decreased after pressing the button.
    /// </summary>
    public double Step
    {
        get => (double)GetValue(StepProperty);
        set => SetValue(StepProperty, value);
    }

    /// <summary>
    /// Maximum allowable value.
    /// </summary>
    public double Max
    {
        get => (double)GetValue(MaxProperty);
        set => SetValue(MaxProperty, value);
    }

    /// <summary>
    /// Minimum allowable value.
    /// </summary>
    public double Min
    {
        get => (double)GetValue(MinProperty);
        set => SetValue(MinProperty, value);
    }

    /// <summary>
    /// Number of decimal places.
    /// </summary>
    public int DecimalPlaces
    {
        get => (int)GetValue(DecimalPlacesProperty);
        set => SetValue(DecimalPlacesProperty, value);
    }

    /// <summary>
    /// Gets or sets numbers pattern.
    /// </summary>
    public string Mask
    {
        get => (string)GetValue(MaskProperty);
        set => SetValue(MaskProperty, value);
    }

    /// <summary>
    /// Gets or sets value determining whether to display the button controls.
    /// </summary>
    public bool ControlsVisible
    {
        get => (bool)GetValue(ControlsVisibleProperty);
        set => SetValue(ControlsVisibleProperty, value);
    }

    /// <summary>
    /// Gets or sets value which determines whether only integers can be entered.
    /// </summary>
    public bool IntegersOnly
    {
        get => (bool)GetValue(IntegersOnlyProperty);
        set => SetValue(IntegersOnlyProperty, value);
    }

    /// <summary>
    /// Event occurs when a value is incremented by button or arrow key.
    /// </summary>
    public event RoutedEventHandler Incremented
    {
        add => AddHandler(IncrementedEvent, value);
        remove => RemoveHandler(IncrementedEvent, value);
    }

    /// <summary>
    /// Event occurs when a value is decremented by button or arrow key.
    /// </summary>
    public event RoutedEventHandler Decremented
    {
        add => AddHandler(DecrementedEvent, value);
        remove => RemoveHandler(DecrementedEvent, value);
    }

    /// <summary>
    /// Command triggered after clicking the control button.
    /// </summary>
    protected Common.IRelayCommand ButtonCommand => (Common.IRelayCommand)GetValue(ButtonCommandProperty);

    /// <summary>
    /// Creates new instance of <see cref="NumberBox"/>.
    /// </summary>
    public NumberBox()
    {
        SetValue(ButtonCommandProperty, new Common.RelayCommand(o => OnCommandButtonClick(this, o)));

        DataObject.AddPastingHandler(this, OnClipboardPaste);

        Loaded += OnLoaded;
    }

    /// <summary>
    /// Updates <see cref="Value"/> and <see cref="System.Windows.Controls.TextBox.Text"/>.
    /// </summary>
    private void UpdateValue(double value, bool updateText)
    {
        Value = value;

        if (!updateText)
            return;

        var newText = FormatDoubleToString(value);

        Text = newText;
        CaretIndex = newText.Length;
    }

    /// <summary>
    /// Updates <see cref="Value"/> and <see cref="System.Windows.Controls.TextBox.Text"/>.
    /// </summary>
    private void UpdateValue(double value, string updateText)
    {
        Value = value;

        Text = updateText;
        CaretIndex = updateText.Length;
    }

    /// <summary>
    /// Increments current <see cref="Value"/>.
    /// </summary>
    private void IncrementValue()
    {
        var currentText = Text;
        var parsedNumber = ParseStringToDouble(currentText) + Step;

        if (String.IsNullOrWhiteSpace(currentText) || parsedNumber > Max)
        {
            UpdateValue(Max, true);

            return;
        }

        UpdateValue(parsedNumber, true);

        OnIncremented();
    }

    /// <summary>
    /// Decrements current <see cref="Value"/>.
    /// </summary>
    private void DecrementValue()
    {
        var currentText = Text;
        var parsedNumber = ParseStringToDouble(currentText) - Step;

        if (String.IsNullOrWhiteSpace(currentText) || parsedNumber < Min)
        {
            UpdateValue(Min, true);

            return;
        }

        UpdateValue(parsedNumber, true);

        OnDecremented();
    }

    /// <summary>
    /// Formats double number according to configuration.
    /// </summary>
    private string FormatDoubleToString(double number)
    {
        if (IntegersOnly || DecimalPlaces < 1)
            return number.ToString("F0", CultureInfo.InvariantCulture);

        if (DecimalPlaces < 5)
            return number.ToString($"F{DecimalPlaces}", CultureInfo.InvariantCulture);

        return number.ToString(CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Tests provided text with regular expression according to configuration.
    /// </summary>
    private bool IsNumberTextValid(string inputText)
    {
        // If the mask is used this method will not work

        var decimalPlaces = DecimalPlaces;
        var integerRegex = new Regex(_integerExpression);
        var decimalRegex = new Regex(_decimalExpression);

        if (IntegersOnly || decimalPlaces < 1)
            return integerRegex.IsMatch(inputText);

        if (!decimalRegex.IsMatch(inputText))
            return false;

        if (inputText.Contains(",") && inputText.Substring(inputText.IndexOf(",")).Length > decimalPlaces)
            return false;

        if (inputText.Contains(".") && inputText.Substring(inputText.IndexOf(".")).Length > decimalPlaces)
            return false;

        return true;
    }

    /// <summary>
    /// Tries to format provided string according to the mask.
    /// </summary>
    private string FormatWithMask(string currentInput, string newInput)
    {
        // TODO: Format text according to MaskProperty

        return currentInput;
    }

    /// <summary>
    /// Tries to parse provided string to double with invariant culture.
    /// </summary>
    private double ParseStringToDouble(string inputText)
    {
        Double.TryParse(inputText, NumberStyles.Any, CultureInfo.InvariantCulture, out double number);

        return number;
    }

    /// <summary>
    /// Occurs when controls is loaded.
    /// </summary>
    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        Text = FormatDoubleToString(Value);
    }

    /// <inheritdoc />
    protected override void OnKeyUp(KeyEventArgs e)
    {
        if (e.Key == Key.Up)
        {
            IncrementValue();

            e.Handled = true;
        }

        if (e.Key == Key.Down)
        {
            DecrementValue();

            e.Handled = true;
        }

        base.OnKeyUp(e);
    }

    /// <inheritdoc />
    protected override void OnTextChanged(TextChangedEventArgs e)
    {
        base.OnTextChanged(e);

        var currentText = Text;
        var parsedNumber = ParseStringToDouble(currentText);

        PlaceholderVisible = currentText.Length < 1;

        // TODO: Meh
        if (parsedNumber > Max)
        {
            UpdateValue(Max, true);

            return;
        }

        if (parsedNumber < Min)
        {
            UpdateValue(Min, true);

            return;
        }

        UpdateValue(parsedNumber, true);
    }

    /// <inheritdoc />
    protected override void OnPreviewTextInput(TextCompositionEventArgs e)
    {
        var newText = Text + (e.Text ?? String.Empty);

        if (!String.IsNullOrEmpty(newText))
            e.Handled = !IsNumberTextValid(newText);

        // Do not allow a leading minus sign if the min value is greater than zero.
        if (Min >= 0 && newText.StartsWith("-"))
            e.Handled = true;


        base.OnPreviewTextInput(e);
    }

    /// <summary>
    /// This virtual method is called after incrementing a value using button or arrow key.
    /// </summary>
    protected virtual void OnIncremented()
    {
        RaiseEvent(new RoutedEventArgs(IncrementedEvent, this));
    }

    /// <summary>
    /// This virtual method is called after decrementing a value using button or arrow key.
    /// </summary>
    protected virtual void OnDecremented()
    {
        RaiseEvent(new RoutedEventArgs(DecrementedEvent, this));
    }

    /// <summary>
    /// This virtual method is called after <see cref="DecimalPlaces"/> is changed.
    /// </summary>
    protected virtual void OnDecimalPlacesChanged(int decimalPlaces)
    {
        if (decimalPlaces < 0)
            DecimalPlaces = 0;
    }

    private static void OnDecimalPlacesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not NumberBox control)
            return;

        if (e.NewValue is not int newValue)
            return;

        control.OnDecimalPlacesChanged(newValue);
    }

    private void OnClipboardPaste(object sender, DataObjectPastingEventArgs e)
    {
        if (sender is not NumberBox control)
            return;

        var clipboardText = (string)e.DataObject.GetData(typeof(string));

        if (!IsNumberTextValid(clipboardText))
            e.CancelCommand();
    }

    /// <summary>
    /// Handles the increment and decrement button.
    /// </summary>
    private void OnCommandButtonClick(object sender, object parameter)
    {
        if (sender is not NumberBox)
            return;

        var command = parameter?.ToString() ?? String.Empty;

        switch (command)
        {
            case "increment":
                IncrementValue();
                break;

            case "decrement":
                DecrementValue();
                break;
        }
    }
}
