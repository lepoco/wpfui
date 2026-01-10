// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

// This Source Code is partially based on the source code provided by the .NET Foundation.

// TODO: Mask (with placeholder); Clipboard paste;
// TODO: Constant decimals when formatting. Although this can actually be done with NumberFormatter.
// TODO: Disable expression by default
// TODO: Lock to digit characters only by property
//
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

/// <summary>
/// Represents a control that can be used to display and edit numbers.
/// </summary>
[TemplatePart(Name = PART_ClearButton, Type = typeof(Button))]
[TemplatePart(Name = PART_InlineIncrementButton, Type = typeof(RepeatButton))]
[TemplatePart(Name = PART_InlineDecrementButton, Type = typeof(RepeatButton))]
public partial class NumberBox : Wpf.Ui.Controls.TextBox
{
    // Template part names
    private const string PART_ClearButton = nameof(PART_ClearButton);
    private const string PART_InlineIncrementButton = nameof(PART_InlineIncrementButton);
    private const string PART_InlineDecrementButton = nameof(PART_InlineDecrementButton);

    private bool _valueUpdating;

    /// <summary>Identifies the <see cref="Value"/> dependency property.</summary>
    public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
        nameof(Value),
        typeof(double?),
        typeof(NumberBox),
        new FrameworkPropertyMetadata(
            null,
            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
            OnValueChanged,
            null,
            false,
            UpdateSourceTrigger.LostFocus
        )
    );

    /// <summary>Identifies the <see cref="MaxDecimalPlaces"/> dependency property.</summary>
    public static readonly DependencyProperty MaxDecimalPlacesProperty = DependencyProperty.Register(
        nameof(MaxDecimalPlaces),
        typeof(int),
        typeof(NumberBox),
        new PropertyMetadata(6)
    );

    /// <summary>Identifies the <see cref="SmallChange"/> dependency property.</summary>
    public static readonly DependencyProperty SmallChangeProperty = DependencyProperty.Register(
        nameof(SmallChange),
        typeof(double),
        typeof(NumberBox),
        new PropertyMetadata(1.0d)
    );

    /// <summary>Identifies the <see cref="LargeChange"/> dependency property.</summary>
    public static readonly DependencyProperty LargeChangeProperty = DependencyProperty.Register(
        nameof(LargeChange),
        typeof(double),
        typeof(NumberBox),
        new PropertyMetadata(10.0d)
    );

    /// <summary>Identifies the <see cref="Maximum"/> dependency property.</summary>
    public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(
        nameof(Maximum),
        typeof(double),
        typeof(NumberBox),
        new PropertyMetadata(double.MaxValue)
    );

    /// <summary>Identifies the <see cref="Minimum"/> dependency property.</summary>
    public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(
        nameof(Minimum),
        typeof(double),
        typeof(NumberBox),
        new PropertyMetadata(double.MinValue)
    );

    /// <summary>Identifies the <see cref="AcceptsExpression"/> dependency property.</summary>
    public static readonly DependencyProperty AcceptsExpressionProperty = DependencyProperty.Register(
        nameof(AcceptsExpression),
        typeof(bool),
        typeof(NumberBox),
        new PropertyMetadata(true)
    );

    /// <summary>Identifies the <see cref="SpinButtonPlacementMode"/> dependency property.</summary>
    public static readonly DependencyProperty SpinButtonPlacementModeProperty = DependencyProperty.Register(
        nameof(SpinButtonPlacementMode),
        typeof(NumberBoxSpinButtonPlacementMode),
        typeof(NumberBox),
        new PropertyMetadata(NumberBoxSpinButtonPlacementMode.Inline)
    );

    /// <summary>Identifies the <see cref="ValidationMode"/> dependency property.</summary>
    public static readonly DependencyProperty ValidationModeProperty = DependencyProperty.Register(
        nameof(ValidationMode),
        typeof(NumberBoxValidationMode),
        typeof(NumberBox),
        new PropertyMetadata(NumberBoxValidationMode.InvalidInputOverwritten)
    );

    /// <summary>Identifies the <see cref="NumberFormatter"/> dependency property.</summary>
    public static readonly DependencyProperty NumberFormatterProperty = DependencyProperty.Register(
        nameof(NumberFormatter),
        typeof(INumberFormatter),
        typeof(NumberBox),
        new PropertyMetadata(null, OnNumberFormatterChanged)
    );

    /// <summary>Identifies the <see cref="ValueChanged"/> routed event.</summary>
    public static readonly RoutedEvent ValueChangedEvent = EventManager.RegisterRoutedEvent(
        nameof(ValueChanged),
        RoutingStrategy.Bubble,
        typeof(NumberBoxValueChangedEvent),
        typeof(NumberBox)
    );

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
    /// Gets or sets a value indicating whether the control will accept and evaluate a basic formulaic expression entered as input.
    /// </summary>
    public bool AcceptsExpression
    {
        get => (bool)GetValue(AcceptsExpressionProperty);
        set => SetValue(AcceptsExpressionProperty, value);
    }

    /// <summary>
    /// Gets or sets the number formatter.
    /// </summary>
    public INumberFormatter? NumberFormatter
    {
        get => (INumberFormatter?)GetValue(NumberFormatterProperty);
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
    public event NumberBoxValueChangedEvent ValueChanged
    {
        add => AddHandler(ValueChangedEvent, value);
        remove => RemoveHandler(ValueChangedEvent, value);
    }

    static NumberBox()
    {
        AcceptsReturnProperty.OverrideMetadata(typeof(NumberBox), new FrameworkPropertyMetadata(false));
        MaxLinesProperty.OverrideMetadata(typeof(NumberBox), new FrameworkPropertyMetadata(1));
        MinLinesProperty.OverrideMetadata(typeof(NumberBox), new FrameworkPropertyMetadata(1));
    }

    public NumberBox()
        : base()
    {
        NumberFormatter ??= NumberBox.GetRegionalSettingsAwareDecimalFormatter();

        DataObject.AddPastingHandler(this, OnClipboardPaste);
    }

    /// <inheritdoc />
    protected override void OnPreviewKeyDown(KeyEventArgs e)
    {
        if (IsReadOnly)
        {
            return;
        }

        switch (e.Key)
        {
            case Key.PageUp:
                StepValue(LargeChange);
                e.Handled = true;
                break;
            case Key.PageDown:
                StepValue(-LargeChange);
                e.Handled = true;
                break;
            case Key.Up:
                StepValue(SmallChange);
                e.Handled = true;
                break;
            case Key.Down:
                StepValue(-SmallChange);
                e.Handled = true;
                break;
        }

        base.OnPreviewKeyDown(e);
    }

    /// <inheritdoc />
    protected override void OnPreviewKeyUp(KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Key.Enter:
                if (TextWrapping != TextWrapping.Wrap)
                {
                    ValidateInput();
                    MoveCaretToTextEnd();
                }

                e.Handled = true;
                break;

            case Key.Escape:
                UpdateTextToValue();
                e.Handled = true;
                break;
        }

        base.OnPreviewKeyUp(e);
    }

    /// <inheritdoc />
    protected override void OnLostFocus(RoutedEventArgs e)
    {
        base.OnLostFocus(e);

        var oldValue = Value;
        ValidateInput();

        // Update binding source if value changed
        if (!Equals(oldValue, Value))
        {
            GetBindingExpression(ValueProperty)?.UpdateSource();
        }
    }

    /*/// <inheritdoc />
    protected override void OnTextChanged(System.Windows.Controls.TextChangedEventArgs e)
    {
        base.OnTextChanged(e);

        //if (new string[] { ",", ".", " " }.Any(s => Text.EndsWith(s)))
        //    return;

        //if (!_textUpdating)
        //    UpdateValueToText();
    }*/

    /// <inheritdoc />
    public override void OnApplyTemplate()
    {
        SubscribeToButtonClickEvent<System.Windows.Controls.Button>(
            PART_ClearButton,
            () => OnClearButtonClick()
        );
        SubscribeToButtonClickEvent<RepeatButton>(PART_InlineIncrementButton, () => StepValue(SmallChange));
        SubscribeToButtonClickEvent<RepeatButton>(PART_InlineDecrementButton, () => StepValue(-SmallChange));

        // If Text has been set, but Value hasn't, update Value based on Text.
        if (string.IsNullOrEmpty(Text) && Value != null)
        {
            UpdateValueToText();
        }
        else
        {
            UpdateTextToValue();
        }

        base.OnApplyTemplate();
    }

    private void SubscribeToButtonClickEvent<TButton>(string elementName, Action action)
        where TButton : ButtonBase
    {
        if (GetTemplateChild(elementName) is TButton button)
        {
            button.Click += (s, e) =>
            {
                Debug.InfoWriteLineForButtonClick(s);
                action();
            };
        }
    }

    /// <summary>
    /// Is called when <see cref="Value"/> in this <see cref="NumberBox"/> changes.
    /// </summary>
    protected virtual void OnValueChanged(DependencyObject d, double? oldValue)
    {
        if (_valueUpdating)
        {
            return;
        }

        _valueUpdating = true;

        var newValue = Value;

        if (newValue > Maximum)
        {
            SetCurrentValue(ValueProperty, Maximum);
        }

        if (newValue < Minimum)
        {
            SetCurrentValue(ValueProperty, Minimum);
        }

        if (!Equals(newValue, oldValue))
        {
            RaiseEvent(new NumberBoxValueChangedEventArgs(oldValue, newValue, this));
        }

        UpdateTextToValue();

        _valueUpdating = false;
    }

    /// <summary>
    /// Is called when something is pasted in this <see cref="NumberBox"/>.
    /// </summary>
    protected virtual void OnClipboardPaste(object sender, DataObjectPastingEventArgs e)
    {
        // TODO: Fix clipboard
        if (sender is not NumberBox)
        {
            return;
        }

        ValidateInput();
    }

    private void StepValue(double? change)
    {
        Debug.InfoWriteLine($"{typeof(NumberBox)} {nameof(StepValue)} raised, change {change}");

        // Before adjusting the value, validate the contents of the textbox so we don't override it.
        ValidateInput();

        var newValue = Value ?? 0;

        if (change is not null)
        {
            newValue += change ?? 0d;
        }

        SetCurrentValue(ValueProperty, newValue);

        MoveCaretToTextEnd();
    }

    private void UpdateTextToValue()
    {
        var newText = string.Empty;

        if (Value is not null && NumberFormatter is not null)
        {
            newText = NumberFormatter.FormatDouble(Math.Round((double)Value, MaxDecimalPlaces));
        }

        SetCurrentValue(TextProperty, newText);
    }

    private void UpdateValueToText()
    {
        ValidateInput();
    }

    private void ValidateInput()
    {
        var text = Text.Trim();

        if (string.IsNullOrEmpty(text))
        {
            SetCurrentValue(ValueProperty, null);

            return;
        }

        var numberParser = NumberFormatter as INumberParser;
        var value = numberParser!.ParseDouble(text);

        if (value is null || Equals(Value, value))
        {
            UpdateTextToValue();

            return;
        }

        if (value > Maximum)
        {
            value = Maximum;
        }

        if (value < Minimum)
        {
            value = Minimum;
        }

        SetCurrentValue(ValueProperty, value);
    }

    private void MoveCaretToTextEnd()
    {
        CaretIndex = Text.Length;
    }

    private static INumberFormatter GetRegionalSettingsAwareDecimalFormatter()
    {
        return new ValidateNumberFormatter();
    }

    private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is NumberBox numberBox)
        {
            numberBox.OnValueChanged(d, (double?)e.OldValue);
        }
    }

    private static void OnNumberFormatterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (e.NewValue is not INumberParser)
        {
            throw new InvalidOperationException(
                $"{nameof(NumberFormatter)} must implement {typeof(INumberParser)}"
            );
        }
    }

    private static partial class Debug
    {
        public static partial void InfoWriteLine(string debugLine);

        public static partial void InfoWriteLineForButtonClick(object sender);

#if DEBUG
        public static partial void InfoWriteLine(string debugLine)
        {
            System.Diagnostics.Debug.WriteLine($"INFO: {debugLine}", "Wpf.Ui.NumberBox");
        }

        public static partial void InfoWriteLineForButtonClick(object sender)
        {
            var buttonName =
                (sender is System.Windows.Controls.Primitives.ButtonBase element)
                    ? element.Name
                    : throw new InvalidCastException(nameof(sender));

            InfoWriteLine($"{typeof(NumberBox)} {buttonName} clicked");
        }

#else
        public static partial void InfoWriteLine(string debugLine)
        {
            // Do nothing in non-DEBUG builds
        }

        public static partial void InfoWriteLineForButtonClick(object sender)
        {
            // Do nothing in non-DEBUG builds
        }

#endif // DEBUG
    }
}
