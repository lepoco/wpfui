using RepeatButton = System.Windows.Controls.Primitives.RepeatButton;

namespace Wpf.Ui.Controls;

[TemplatePart(Name = "PART_UpButton", Type = typeof(RepeatButton))]
[TemplatePart(Name = "PART_DownButton", Type = typeof(RepeatButton))]
public class NumericUpDown : System.Windows.Controls.Control
{
    static NumericUpDown()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(NumericUpDown), new FrameworkPropertyMetadata(typeof(NumericUpDown)));

        BorderThicknessProperty.OverrideMetadata(typeof(NumericUpDown), new FrameworkPropertyMetadata(new Thickness(1)));
    }

    public NumericUpDown()
    {
        Loaded += (s, e) => UpdateDisplayValue();
    }

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

    private double CoerceMyValue(double val)
    {
        double clampedValue = Math.Max(MinValue, Math.Min(MaxValue, val));
        double roundedValue = Math.Round(clampedValue, Decimals);

        return roundedValue;
    }

    protected virtual void UpdateDisplayValue()
    {
        CultureInfo culture = CultureInfo.CurrentCulture;
        string format = "F" + Decimals;
        string displayValue = Value.ToString(format, culture);
        SetValue(DisplayValuePropertyKey, displayValue);
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        if (GetTemplateChild("PART_DownButton") is RepeatButton downButton)
        {
            downButton.Click += (sender, e) =>
            {
                Value -= Step;
            };
        }

        if (GetTemplateChild("PART_UpButton") is RepeatButton upButton)
        {
            upButton.Click += (sender, e) =>
            {
                Value += Step;
            };
        }
    }
}