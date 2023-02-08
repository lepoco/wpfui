// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows;
using System.Windows.Input;

namespace Wpf.Ui.Controls.NumberBoxControl;

// TODO: Mask (with placeholder); Clipboard paste;
// TODO: Constant decimals when formatting. Although this can actually be done with NumberFormatter.
// TODO: Disable expression by default
// TODO: Fix when exiting with TAB, try to organize the OnTextChanged
// TODO: Lock to digit characters only by property

/// <summary>
/// Represents a control that can be used to display and edit numbers.
/// </summary>
[ToolboxItem(true)]
[ToolboxBitmap(typeof(NumberBox), "NumberBox.bmp")]
public class NumberBox : Wpf.Ui.Controls.TextBox
{
    private bool _valueUpdating = false;

    private bool _textUpdating = false;

    /// <summary>
    /// Property for <see cref="Value"/>.
    /// </summary>
    public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof(Value),
        typeof(double?), typeof(NumberBox), new PropertyMetadata((double?)null, OnValuePropertyChanged));

    /// <summary>
    /// Property for <see cref="MaxDecimalPlaces"/>.
    /// </summary>
    public static readonly DependencyProperty MaxDecimalPlacesProperty = DependencyProperty.Register(nameof(MaxDecimalPlaces),
        typeof(int), typeof(NumberBox), new PropertyMetadata(6));

    /// <summary>
    /// Property for <see cref="SmallChange"/>.
    /// </summary>
    public static readonly DependencyProperty SmallChangeProperty = DependencyProperty.Register(nameof(SmallChange),
        typeof(double), typeof(NumberBox), new PropertyMetadata(1.0d));

    /// <summary>
    /// Property for <see cref="LargeChange"/>.
    /// </summary>
    public static readonly DependencyProperty LargeChangeProperty = DependencyProperty.Register(nameof(LargeChange),
        typeof(double), typeof(NumberBox), new PropertyMetadata(10.0d));

    /// <summary>
    /// Property for <see cref="Maximum"/>.
    /// </summary>
    public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(nameof(Maximum),
        typeof(double), typeof(NumberBox), new PropertyMetadata(Double.MaxValue));

    /// <summary>
    /// Property for <see cref="Minimum"/>.
    /// </summary>
    public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(nameof(Minimum),
        typeof(double), typeof(NumberBox), new PropertyMetadata(Double.MinValue));

    /// <summary>
    /// Property for <see cref="AcceptsExpression"/>.
    /// </summary>
    public static readonly DependencyProperty AcceptsExpressionProperty = DependencyProperty.Register(nameof(AcceptsExpression),
        typeof(bool), typeof(NumberBox), new PropertyMetadata(true));

    /// <summary>
    /// Property for <see cref="SpinButtonPlacementMode"/>.
    /// </summary>
    public static readonly DependencyProperty SpinButtonPlacementModeProperty = DependencyProperty.Register(nameof(SpinButtonPlacementMode),
        typeof(NumberBoxSpinButtonPlacementMode), typeof(NumberBox), new PropertyMetadata(NumberBoxSpinButtonPlacementMode.Inline));

    /// <summary>
    /// Property for <see cref="ValidationMode"/>.
    /// </summary>
    public static readonly DependencyProperty ValidationModeProperty = DependencyProperty.Register(nameof(ValidationMode),
        typeof(NumberBoxValidationMode), typeof(NumberBox), new PropertyMetadata(NumberBoxValidationMode.InvalidInputOverwritten));

    /// <summary>
    /// Property for <see cref="NumberFormatter"/>.
    /// </summary>
    public static readonly DependencyProperty NumberFormatterProperty = DependencyProperty.Register(nameof(NumberFormatter),
        typeof(INumberFormatter), typeof(NumberBox), new PropertyMetadata(null, OnNumberFormatterPropertyChanged));

    /// <summary>
    /// Routed event for <see cref="ValueChanged"/>.
    /// </summary>
    public static readonly RoutedEvent ValueChangedEvent = EventManager.RegisterRoutedEvent(
        nameof(ValueChanged), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(NumberBox));

    /// <summary>
    /// Gets or sets the numeric value of a <see cref="NumberBox"/>.
    /// </summary>
    public double? Value
    {
        get => (double?)GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    /// <summary>
    /// Gets or sets the number of decimal places to be rounded when converting from Text to Value.
    /// </summary>
    public int MaxDecimalPlaces
    {
        get => (int)GetValue(MaxDecimalPlacesProperty);
        set => SetValue(MaxDecimalPlacesProperty, value);
    }

    /// <summary>
    /// Gets or sets the value that is added to or subtracted from <see cref="Value"/> when a small change is made, such as with an arrow key or scrolling.
    /// </summary>
    public double SmallChange
    {
        get => (double)GetValue(SmallChangeProperty);
        set => SetValue(SmallChangeProperty, value);
    }

    /// <summary>
    /// Gets or sets the value that is added to or subtracted from <see cref="Value"/> when a large change is made, such as with the PageUP and PageDown keys.
    /// </summary>
    public double LargeChange
    {
        get => (double)GetValue(LargeChangeProperty);
        set => SetValue(LargeChangeProperty, value);
    }

    /// <summary>
    /// Gets or sets the numerical maximum for <see cref="Value"/>.
    /// </summary>
    public double Maximum
    {
        get => (double)GetValue(MaximumProperty);
        set => SetValue(MaximumProperty, value);
    }

    /// <summary>
    /// Gets or sets the numerical minimum for <see cref="Value"/>.
    /// </summary>
    public double Minimum
    {
        get => (double)GetValue(MinimumProperty);
        set => SetValue(MinimumProperty, value);
    }

    /// <summary>
    /// Toggles whether the control will accept and evaluate a basic formulaic expression entered as input.
    /// </summary>
    public bool AcceptsExpression
    {
        get => (bool)GetValue(AcceptsExpressionProperty);
        set => SetValue(AcceptsExpressionProperty, value);
    }

    public INumberFormatter NumberFormatter
    {
        get => (INumberFormatter)GetValue(NumberFormatterProperty);
        set => SetValue(NumberFormatterProperty, value);
    }

    /// <summary>
    /// Gets or sets a value that indicates the placement of buttons used to increment or decrement the <see cref="Value"/> property.
    /// </summary>
    public NumberBoxSpinButtonPlacementMode SpinButtonPlacementMode
    {
        get => (NumberBoxSpinButtonPlacementMode)GetValue(SpinButtonPlacementModeProperty);
        set => SetValue(SpinButtonPlacementModeProperty, value);
    }

    /// <summary>
    /// Gets or sets the input validation behavior to invoke when invalid input is entered.
    /// </summary>
    public NumberBoxValidationMode ValidationMode
    {
        get => (NumberBoxValidationMode)GetValue(ValidationModeProperty);
        set => SetValue(ValidationModeProperty, value);
    }

    /// <summary>
    /// Occurs after the user triggers evaluation of new input by pressing the Enter key, clicking a spin button, or by changing focus.
    /// </summary>
    public event RoutedEventHandler ValueChanged
    {
        add => AddHandler(ValueChangedEvent, value);
        remove => RemoveHandler(ValueChangedEvent, value);
    }

    static NumberBox()
    {
        AcceptsReturnProperty.OverrideMetadata(typeof(PasswordBox), new FrameworkPropertyMetadata(false));
        MaxLinesProperty.OverrideMetadata(typeof(PasswordBox), new FrameworkPropertyMetadata(1));
        MinLinesProperty.OverrideMetadata(typeof(PasswordBox), new FrameworkPropertyMetadata(1));
    }

    /// <inheritdoc />
    public NumberBox() : base()
    {
        NumberFormatter ??= GetRegionalSettingsAwareDecimalFormatter();

        DataObject.AddPastingHandler(this, OnClipboardPaste);
    }

    /// <inheritdoc />
    protected override void OnKeyUp(KeyEventArgs e)
    {
        base.OnKeyUp(e);

        switch (e.Key)
        {
            case Key.PageUp:
                StepValue(LargeChange);
                break;
            case Key.PageDown:
                StepValue(-LargeChange);
                break;
            case Key.Up:
                StepValue(SmallChange);
                break;
            case Key.Down:
                StepValue(-SmallChange);
                break;
            case Key.Enter:
                if (TextWrapping != TextWrapping.Wrap)
                {
                    ValidateInput();
                    MoveCaretToTextEnd();
                }
                break;
        }
    }

    /// <inheritdoc />
    protected override void OnTemplateButtonClick(string parameter)
    {
#if DEBUG
        System.Diagnostics.Debug.WriteLine($"INFO: {typeof(NumberBox)} button clicked with param: {parameter}", "Wpf.Ui.NumberBox");
#endif

        switch (parameter)
        {
            case "clear":
                OnClearButtonClick();

                break;
            case "increment":
                StepValue(SmallChange);

                break;
            case "decrement":
                StepValue(-SmallChange);

                break;
        }

        // NOTE: Focus looks and works well with mouse and Clear button. But it sucks for spin buttons
        Focus();
    }

    /// <inheritdoc />
    protected override void OnLostFocus(RoutedEventArgs e)
    {
        base.OnLostFocus(e);

        ValidateInput();
    }

    /// <inheritdoc />
    //protected override void OnTextChanged(System.Windows.Controls.TextChangedEventArgs e)
    //{
    //    base.OnTextChanged(e);

    //    //if (new string[] { ",", ".", " " }.Any(s => Text.EndsWith(s)))
    //    //    return;

    //    //if (!_textUpdating)
    //    //    UpdateValueToText();
    //}

    /// <inheritdoc />
    protected override void OnTemplateChanged(System.Windows.Controls.ControlTemplate oldTemplate, System.Windows.Controls.ControlTemplate newTemplate)
    {
        base.OnTemplateChanged(oldTemplate, newTemplate);

        // If Text has been set, but Value hasn't, update Value based on Text.
        if (String.IsNullOrEmpty(Text) && Value != null)
            UpdateValueToText();
        else
            UpdateTextToValue();
    }

    /// <summary>
    /// Is called when <see cref="Value"/> in this <see cref="NumberBox"/> changes.
    /// </summary>
    protected virtual void OnValueChanged(DependencyObject d, double? oldValue)
    {
        if (_valueUpdating)
            return;

        _valueUpdating = true;

        var newValue = Value;

        if (newValue > Maximum)
            Value = Maximum;

        if (newValue < Minimum)
            Value = Minimum;

        if (newValue != oldValue)
            RaiseEvent(new RoutedEventArgs(ValueChangedEvent));

        UpdateTextToValue();

        _valueUpdating = false;
    }

    /// <summary>
    /// Is called when something is pasted in this <see cref="NumberBox"/>.
    /// </summary>
    protected virtual void OnClipboardPaste(object sender, DataObjectPastingEventArgs e)
    {
        // TODO: Fix clipboard
        if (sender is not NumberBox numberBox)
            return;

        ValidateInput();
    }

    private void StepValue(double? change)
    {
#if DEBUG
        System.Diagnostics.Debug.WriteLine($"INFO: {typeof(NumberBox)} {nameof(StepValue)} raised, change {change}", "Wpf.Ui.NumberBox");
#endif

        // Before adjusting the value, validate the contents of the textbox so we don't override it.
        ValidateInput();

        var newValue = Value ?? 0;

        if (change != null)
            newValue += change ?? 0d;

        Value = newValue;

        MoveCaretToTextEnd();
    }

    private void UpdateTextToValue()
    {
        _textUpdating = true;

        var newText = String.Empty;
        // text = value

        if (Value != null)
            newText = NumberFormatter.FormatDouble(
                Math.Round((double)Value, MaxDecimalPlaces));

        Text = newText;

        _textUpdating = false;
    }

    private void UpdateValueToText()
    {
        ValidateInput();
    }

    private void ValidateInput()
    {
        var text = Text.Trim();

        if (String.IsNullOrEmpty(text))
        {
            Value = null;

            return;
        }

        var numberParser = NumberFormatter as INumberParser;
        var value = numberParser!.ParseDouble(text);

        if (value == null || Value == value)
        {
            UpdateTextToValue();

            return;
        }

        if (value > Maximum)
            value = Maximum;

        if (value < Minimum)
            value = Minimum;

        Value = value;
    }

    private void MoveCaretToTextEnd()
    {
        CaretIndex = Text.Length;
    }

    private INumberFormatter GetRegionalSettingsAwareDecimalFormatter()
    {
        return new ValidateNumberFormatter();
    }

    private static void OnValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not NumberBox numberBox)
            return;

        numberBox.OnValueChanged(d, (double?)e.OldValue);
    }

    private static void OnNumberFormatterPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (e.NewValue is not INumberParser)
            throw new ArgumentException($"{nameof(NumberFormatter)} must implement {typeof(INumberParser)}", nameof(NumberFormatter));
    }
}
