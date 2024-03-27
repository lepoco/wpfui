// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

namespace Wpf.Ui.Controls;

internal class NumericUpDownButton : System.Windows.Controls.Primitives.RepeatButton
{
    public CornerRadius CornerRadius
    {
        get => (CornerRadius)GetValue(CornerRadiusProperty);
        set => SetValue(CornerRadiusProperty, value);
    }

    public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(NumericUpDownButton), new FrameworkPropertyMetadata(default(CornerRadius)));

    public NumericUpDownButtonType NumericButtonType
    {
        get => (NumericUpDownButtonType)GetValue(NumericButtonTypeProperty);
        set => SetValue(NumericButtonTypeProperty, value);
    }

    public static readonly DependencyProperty NumericButtonTypeProperty = DependencyProperty.Register(nameof(NumericButtonType), typeof(NumericUpDownButtonType), typeof(NumericUpDownButton), new FrameworkPropertyMetadata(default(NumericUpDownButtonType)));

}
