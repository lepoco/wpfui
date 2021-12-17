// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace WPFUI.Controls
{
    // TODO: Refactor
    // This control is in a very early stage of development. Validations, increments, pasting and especially masks that are not implemented at all should be refined.

    /// <summary>
    /// Text field for entering numbers with the possibility of specifying pattern.
    /// </summary>
    public class NumberBox : System.Windows.Controls.TextBox
    {
        internal Regex PatternRegex;

        /// <summary>
        /// Property for <see cref="Value"/>.
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof(Value),
            typeof(double), typeof(NumberBox), new PropertyMetadata(0.0, Value_PropertyChanged));

        /// <summary>
        /// Property for <see cref="Step"/>.
        /// </summary>
        public static readonly DependencyProperty StepProperty = DependencyProperty.Register(nameof(Step),
            typeof(double), typeof(NumberBox), new PropertyMetadata(1.0));

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
            typeof(int), typeof(NumberBox), new PropertyMetadata(2, DecimalPlaces_PropertyChanged));

        /// <summary>
        /// Property for <see cref="Mask"/>.
        /// </summary>
        public static readonly DependencyProperty MaskProperty = DependencyProperty.Register(nameof(Mask),
            typeof(string), typeof(NumberBox), new PropertyMetadata(String.Empty));

        /// <summary>
        /// Property for <see cref="Placeholder"/>.
        /// </summary>
        public static readonly DependencyProperty PlaceholderProperty = DependencyProperty.Register(nameof(Placeholder),
            typeof(string), typeof(NumberBox), new PropertyMetadata(String.Empty));

        /// <summary>
        /// Property for <see cref="PlaceholderVisible"/>.
        /// </summary>
        public static readonly DependencyProperty PlaceholderVisibleProperty = DependencyProperty.Register(nameof(PlaceholderVisible),
            typeof(bool), typeof(NumberBox), new PropertyMetadata(true));

        /// <summary>
        /// Property for <see cref="ControlsVisible"/>.
        /// </summary>
        public static readonly DependencyProperty ControlsVisibleProperty = DependencyProperty.Register(nameof(ControlsVisible),
            typeof(bool), typeof(NumberBox), new PropertyMetadata(true));

        /// <summary>
        /// Property for <see cref="IntegersOnly"/>.
        /// </summary>
        public static readonly DependencyProperty IntegersOnlyProperty = DependencyProperty.Register(nameof(IntegersOnly),
            typeof(bool), typeof(NumberBox), new PropertyMetadata(false, IntegersOnly_PropertyChanged));

        /// <summary>
        /// Property for <see cref="ButtonCommand"/>.
        /// </summary>
        public static readonly DependencyProperty ButtonCommandProperty =
            DependencyProperty.Register(nameof(NumberBox),
                typeof(Common.RelayCommand), typeof(TitleBar), new PropertyMetadata(null));

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
        /// Gets or sets numbers pattern.
        /// </summary>
        public string Placeholder
        {
            get => (string)GetValue(PlaceholderProperty);
            set => SetValue(PlaceholderProperty, value);
        }

        /// <summary>
        /// Gets or sets value determining whether to display the placeholder.
        /// </summary>
        public bool PlaceholderVisible
        {
            get => (bool)GetValue(PlaceholderVisibleProperty);
            set => SetValue(PlaceholderVisibleProperty, value);
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
        /// Command triggered after clicking the control button.
        /// </summary>
        public Common.RelayCommand ButtonCommand => (Common.RelayCommand)GetValue(ButtonCommandProperty);

        /// <summary>
        /// Creates new instance of <see cref="NumberBox"/> and defines default events for validating provided numbers.
        /// </summary>
        public NumberBox()
        {
            SetValue(ButtonCommandProperty, new Common.RelayCommand(o => Button_Click(this, o)));

            PatternRegex = IntegersOnly ? new("[^0-9]+") : new("[^0-9.,]+");

            PreviewTextInput += NumberBox_PreviewTextInput;
            TextChanged += NumberBox_TextChanged;
            KeyUp += NumberBox_KeyUp;

            DataObject.AddPastingHandler(this, PastingHandler);
        }

        private static void DecimalPlaces_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not NumberBox control) return;

            if (control.DecimalPlaces < 0)
            {
                control.DecimalPlaces = 0;
            }

            if (control.DecimalPlaces > 4)
            {
                control.DecimalPlaces = 4;
            }
        }

        private static void Value_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not NumberBox control) return;

            if (!String.IsNullOrEmpty(control.Text))
            {
                return;
            }

            if (control.Value % 1 != 0 && !control.IntegersOnly)
            {
                control.Text = control.Value.ToString("F" + control.DecimalPlaces, CultureInfo.InvariantCulture);
            }
            else
            {
                control.Text = control.Value.ToString("F0", CultureInfo.InvariantCulture);
            }
        }

        private static void IntegersOnly_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not NumberBox control) return;

            control.PatternRegex = control.IntegersOnly ? new("[^0-9]+") : new("[^0-9.,]+");
        }

        private void Button_Click(object sender, object parameter)
        {
            string command = parameter as string;

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

        private void NumberBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = Validate(e.Text);
        }

        private void NumberBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (sender is not NumberBox control) return;

            if (PlaceholderVisible && control.Text.Length > 0)
            {
                PlaceholderVisible = false;
            }

            if (!PlaceholderVisible && control.Text.Length < 1)
            {
                PlaceholderVisible = true;
            }

            Double.TryParse(control.Text, out double number);

            Value = number;
        }

        private void NumberBox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Up)
            {
                IncrementValue();
            }

            if (e.Key == Key.Down)
            {
                DecrementValue();
            }
        }

        private void PastingHandler(object sender, DataObjectPastingEventArgs e)
        {
            if (sender is not NumberBox control) return;

            string clipboardText = (string)e.DataObject.GetData(typeof(string));

            if (Validate(clipboardText))
            {
                e.CancelCommand();
            }
        }

        private void IncrementValue()
        {
            string currentText = Text;

            if (String.IsNullOrEmpty(currentText))
            {
                Text = Step.ToString("F0");

                return;
            }

            Double.TryParse(currentText, out double number);

            if (number + Step > Max)
            {
                return;
            }

            if ((currentText.Contains(".") || currentText.Contains(",")) && !IntegersOnly)
            {
                Text = (number + Step).ToString("F" + DecimalPlaces, CultureInfo.InvariantCulture);
            }
            else
            {
                Text = (number + Step).ToString("F0", CultureInfo.InvariantCulture);
            }
        }

        private void DecrementValue()
        {
            string currentText = Text;

            if (String.IsNullOrEmpty(currentText))
            {
                Text = "-" + Step.ToString("F0");

                return;
            }

            Double.TryParse(currentText, out double number);

            if (number - Step < Min)
            {
                return;
            }

            if ((currentText.Contains(".") || currentText.Contains(",")) && !IntegersOnly)
            {
                Text = (number - Step).ToString("F" + DecimalPlaces, CultureInfo.InvariantCulture);
            }
            else
            {
                Text = (number - Step).ToString("F0", CultureInfo.InvariantCulture);
            }
        }

        private bool Validate(string input)
        {
            if (String.IsNullOrEmpty(input))
            {
                return true;
            }

            if (input.StartsWith(".") || input.EndsWith("."))
            {
                return false;
            }

            if (input.StartsWith(",") || input.EndsWith(","))
            {
                return false;
            }

            if (!PatternRegex.IsMatch(input))
            {
                return false;
            }

            return true;
        }

        private string Format(string currentInput, string newInput)
        {
            // TODO: Format text according to MaskProperty

            return currentInput;
        }
    }
}
