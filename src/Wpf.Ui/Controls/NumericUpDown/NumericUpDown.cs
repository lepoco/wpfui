using System.Windows.Controls;
using System.Windows.Input;
using RepeatButton = System.Windows.Controls.Primitives.RepeatButton;

namespace Wpf.Ui.Controls;

[TemplatePart(Name = "PART_UpButton", Type = typeof(RepeatButton))]
[TemplatePart(Name = "PART_DownButton", Type = typeof(RepeatButton))]
[TemplatePart(Name = "PART_TextBox", Type = typeof(TextBox))]
public class NumericUpDown : System.Windows.Controls.Control
{
    static NumericUpDown()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(NumericUpDown), new FrameworkPropertyMetadata(typeof(NumericUpDown)));

        Control.BorderThicknessProperty.OverrideMetadata(typeof(NumericUpDown), new FrameworkPropertyMetadata(new Thickness(1), OnBorderThicknessChanged));
    }

    public NumericUpDown()
    {
        Loaded += (s, e) =>
        {
            UpdateDisplayValue();
            UpdateTopButtonCornerRadius();
            UpdateBottomButtonCornerRadius();
        };
    }

    private static void OnBorderThicknessChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is NumericUpDown self)
        {
            self.UpdateTopButtonCornerRadius();
            self.UpdateBottomButtonCornerRadius();
        }
    }

    /// <summary>
    /// Gets or sets the value of the control.
    /// </summary>
    public double Value
    {
        get => (double)GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    /// <summary>Identifies the <see cref="Value"/> dependency property.</summary>
    public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof(Value), typeof(double), typeof(NumericUpDown), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnValueChanged, CoerceValue));

    private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is NumericUpDown self)
        {
            self.OnValueChanged(e);
        }
    }

    protected virtual void OnValueChanged(DependencyPropertyChangedEventArgs e)
    {
        UpdateDisplayValue();
    }

    private static object CoerceValue(DependencyObject d, object baseValue)
    {
        if (d is NumericUpDown self && baseValue is double doubleval)
        {
            return self.CoerceMyValue(doubleval);
        }

        return 0.0;
    }

    public string DisplayValue
    {
        get => (string)GetValue(DisplayValueProperty);
    }

    /// <summary>Identifies the <see cref="DisplayValue"/> dependency property.</summary>
    protected static readonly DependencyPropertyKey DisplayValuePropertyKey = DependencyProperty.RegisterReadOnly(nameof(DisplayValue), typeof(string), typeof(NumericUpDown), new FrameworkPropertyMetadata(default(string)));

    /// <summary>Identifies the <see cref="DisplayValue"/> dependency property.</summary>
    public static readonly DependencyProperty DisplayValueProperty = DisplayValuePropertyKey.DependencyProperty;

    public double Step
    {
        get => (double)GetValue(StepProperty);
        set => SetValue(StepProperty, value);
    }

    /// <summary>Identifies the <see cref="Step"/> dependency property.</summary>
    public static readonly DependencyProperty StepProperty = DependencyProperty.Register(nameof(Step), typeof(double), typeof(NumericUpDown), new PropertyMetadata(0.1d));

    public int Decimals
    {
        get => (int)GetValue(DecimalsProperty);
        set => SetValue(DecimalsProperty, value);
    }

    /// <summary>Identifies the <see cref="Decimals"/> dependency property.</summary>
    public static readonly DependencyProperty DecimalsProperty = DependencyProperty.Register(nameof(Decimals), typeof(int), typeof(NumericUpDown), new PropertyMetadata(2, OnDecimalsChanged));

    private static void OnDecimalsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is NumericUpDown self)
        {
            self.CoerceValue(ValueProperty);
        }
    }

    public double MaxValue
    {
        get => (double)GetValue(MaxValueProperty);
        set => SetValue(MaxValueProperty, value);
    }

    /// <summary>Identifies the <see cref="MaxValue"/> dependency property.</summary>
    public static readonly DependencyProperty MaxValueProperty = DependencyProperty.Register(nameof(MaxValue), typeof(double), typeof(NumericUpDown), new PropertyMetadata(100.0, OnMaxValueChanged));

    private static void OnMaxValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is NumericUpDown self)
        {
            if (self.MinValue > self.MaxValue)
            {
                self.MinValue = self.MaxValue;
            }

            self.CoerceValue(ValueProperty);
        }
    }

    public double MinValue
    {
        get => (double)GetValue(MinValueProperty);
        set => SetValue(MinValueProperty, value);
    }

    /// <summary>Identifies the <see cref="MinValue"/> dependency property.</summary>
    public static readonly DependencyProperty MinValueProperty = DependencyProperty.Register(nameof(MinValue), typeof(double), typeof(NumericUpDown), new PropertyMetadata(0.0, OnMinValueChanged));

    private static void OnMinValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is NumericUpDown self)
        {
            if (self.MaxValue < self.MinValue)
            {
                self.MaxValue = self.MinValue;
            }

            self.CoerceValue(ValueProperty);
        }
    }

    /// <summary>
    /// gets or sets the corner radius of the control.
    /// </summary>
    public CornerRadius CornerRadius
    {
        get => (CornerRadius)GetValue(CornerRadiusProperty);
        set => SetValue(CornerRadiusProperty, value);
    }

    /// <summary>Identifies the <see cref="CornerRadius"/> dependency property.</summary>
    public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(NumericUpDown), new FrameworkPropertyMetadata(new CornerRadius(4), OnCornerRadiusChanged));

    private static void OnCornerRadiusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is NumericUpDown self)
        {
            self.UpdateTopButtonCornerRadius();
            self.UpdateBottomButtonCornerRadius();
        }
    }

    internal CornerRadius TopButtonCornerRadius
    {
        get => (CornerRadius)GetValue(TopButtonCornerRadiusProperty);
    }

    private static readonly DependencyPropertyKey TopButtonCornerRadiusPropertyKey = DependencyProperty.RegisterReadOnly(nameof(TopButtonCornerRadius), typeof(CornerRadius), typeof(NumericUpDown), new FrameworkPropertyMetadata(default(CornerRadius)));

    /// <summary>Identifies the <see cref="TopButtonCornerRadius"/> dependency property.</summary>
    internal static readonly DependencyProperty TopButtonCornerRadiusProperty = TopButtonCornerRadiusPropertyKey.DependencyProperty;

    protected void UpdateTopButtonCornerRadius()
    {
        double topButtonCornerRadiusTopRight = Math.Max(0, CornerRadius.TopRight - (0.5 * BorderThickness.Right));
        CornerRadius topButtonCornerRadius = ButtonAlignment == NumericUpDownButtonAlignment.Vertical
            ? new(0, topButtonCornerRadiusTopRight, 0, 0)
            : new(0, 0, 0, 0);
        SetValue(TopButtonCornerRadiusPropertyKey, topButtonCornerRadius);
    }

    internal CornerRadius BottomButtonCornerRadius
    {
        get => (CornerRadius)GetValue(BottomButtonCornerRadiusProperty);
    }

    private static readonly DependencyPropertyKey BottomButtonCornerRadiusPropertyKey = DependencyProperty.RegisterReadOnly(nameof(BottomButtonCornerRadius), typeof(CornerRadius), typeof(NumericUpDown), new FrameworkPropertyMetadata(default(CornerRadius)));

    /// <summary>Identifies the <see cref="BottomButtonCornerRadius"/> dependency property.</summary>
    internal static readonly DependencyProperty BottomButtonCornerRadiusProperty = BottomButtonCornerRadiusPropertyKey.DependencyProperty;

    protected void UpdateBottomButtonCornerRadius()
    {
        double bottomButtonCornerRadiusBottomRight = Math.Max(0, CornerRadius.TopRight - (0.5 * BorderThickness.Right));
        CornerRadius bottomButtonCornerRadius = ButtonAlignment == NumericUpDownButtonAlignment.Vertical
            ? new(0, 0, bottomButtonCornerRadiusBottomRight, 0)
            : new(0, bottomButtonCornerRadiusBottomRight, bottomButtonCornerRadiusBottomRight, 0);
        SetValue(BottomButtonCornerRadiusPropertyKey, bottomButtonCornerRadius);
    }

    /// <summary>
    /// Gets or sets the width of the up and down buttons.
    /// </summary>
    public double ButtonWidth
    {
        get => (double)GetValue(ButtonWidthProperty);
        set => SetValue(ButtonWidthProperty, value);
    }

    /// <summary>Identifies the <see cref="ButtonWidth"/> dependency property.</summary>
    public static readonly DependencyProperty ButtonWidthProperty = DependencyProperty.Register(nameof(ButtonWidth), typeof(double), typeof(NumericUpDown), new FrameworkPropertyMetadata(22.0));

    /// <summary>
    /// gets or sets the alignment of the buttons.
    /// </summary>
    public NumericUpDownButtonAlignment ButtonAlignment
    {
        get => (NumericUpDownButtonAlignment)GetValue(ButtonAlignmentProperty);
        set => SetValue(ButtonAlignmentProperty, value);
    }

    /// <summary>Identifies the <see cref="ButtonAlignment"/> dependency property.</summary>
    public static readonly DependencyProperty ButtonAlignmentProperty = DependencyProperty.Register(nameof(ButtonAlignment), typeof(NumericUpDownButtonAlignment), typeof(NumericUpDown), new FrameworkPropertyMetadata(default(NumericUpDownButtonAlignment), OnButtonAlignmentChanged));

    private static void OnButtonAlignmentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is NumericUpDown self)
        {
            self.UpdateTopButtonCornerRadius();
            self.UpdateBottomButtonCornerRadius();
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the control automatically wraps the value to the Minimum or Maximum when the value exceeds the range.
    /// </summary>
    public bool Wrap
    {
        get => (bool)GetValue(WrapProperty);
        set => SetValue(WrapProperty, value);
    }

    /// <summary>Identifies the <see cref="Wrap"/> dependency property.</summary>
    public static readonly DependencyProperty WrapProperty = DependencyProperty.Register(nameof(Wrap), typeof(bool), typeof(NumericUpDown), new FrameworkPropertyMetadata(false));

    /// <summary>
    /// gets or sets a value indicating whether the control is read-only.
    /// </summary>
    public bool IsReadOnly
    {
        get => (bool)GetValue(IsReadOnlyProperty);
        set => SetValue(IsReadOnlyProperty, value);
    }

    /// <summary>Identifies the <see cref="IsReadOnly"/> dependency property.</summary>
    public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register(nameof(IsReadOnly), typeof(bool), typeof(NumericUpDown), new FrameworkPropertyMetadata(false));

    /// <summary>
    /// gets or sets the format of the display value.
    /// </summary>
    public string Format
    {
        get => (string)GetValue(FormatProperty);
        set => SetValue(FormatProperty, value);
    }

    /// <summary>Identifies the <see cref="Format"/> dependency property.</summary>
    public static readonly DependencyProperty FormatProperty = DependencyProperty.Register(nameof(Format), typeof(string), typeof(NumericUpDown), new FrameworkPropertyMetadata(string.Empty));

    private double CoerceMyValue(double val)
    {
        double clampedValue = Math.Max(MinValue, Math.Min(MaxValue, val));
        double roundedValue = Math.Round(clampedValue, Decimals);

        return roundedValue;
    }

    protected virtual void UpdateDisplayValue()
    {
        CultureInfo culture = CultureInfo.CurrentCulture;
        string format = string.IsNullOrEmpty(Format)
            ? "F" + Decimals
            : Format;
        string displayValue = Value.ToString(format, culture);
        SetValue(DisplayValuePropertyKey, displayValue);
    }

    private string _userInput = string.Empty;

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        if (GetTemplateChild("PART_DownButton") is NumericUpDownButton downButton)
        {
            downButton.Click += (sender, e) =>
            {
                double newValue = Value - Step;
                newValue = Wrap && newValue < MinValue ? MaxValue : newValue;
                SetCurrentValue(ValueProperty, newValue);
            };
        }

        if (GetTemplateChild("PART_UpButton") is NumericUpDownButton upButton)
        {
            upButton.Click += (sender, e) =>
            {
                double newValue = Value + Step;
                newValue = Wrap && newValue > MaxValue ? MinValue : newValue;
                SetCurrentValue(ValueProperty, newValue);
            };
        }

        if (GetTemplateChild("PART_TextBox") is System.Windows.Controls.TextBox textBox)
        {
            textBox.GotFocus += (sender, e) =>
            {
                if (!IsReadOnly)
                {
                    SetValue(DisplayValuePropertyKey, string.Empty);
                    this._userInput = string.Empty;
                }
            };

            textBox.TextChanged += (sender, e) =>
            {
                this._userInput = textBox.Text;
            };

            textBox.KeyDown += (sender, e) =>
            {
                if (e.Key == Key.Enter && !IsReadOnly)
                {
                    ProcessUserInput();
                    _ = Focus();
                }
            };

            textBox.LostFocus += (sender, e) =>
            {
                if (!IsReadOnly)
                {
                    ProcessUserInput();
                }
            };
        }
    }

    private void ProcessUserInput()
    {
        if (double.TryParse(this._userInput, NumberStyles.Any, CultureInfo.CurrentCulture, out double parsedVal))
        {
            if (parsedVal != Value)
            {
                SetCurrentValue(ValueProperty, parsedVal);
            }
            else
            {
                ForceUpdateDisplayValue();
            }
        }
        else
        {
            ForceUpdateDisplayValue();
        }

        this._userInput = string.Empty;
    }

    private void ForceUpdateDisplayValue()
    {
        SetValue(DisplayValuePropertyKey, string.Empty);
        UpdateDisplayValue();
    }
}