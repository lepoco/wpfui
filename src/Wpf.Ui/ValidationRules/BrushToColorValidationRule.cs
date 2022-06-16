// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Drawing;
using System.Globalization;
using System.Windows.Controls;

namespace Wpf.Ui.ValidationRules;

internal class BrushToColorValidationRule : ValidationRule
{
    public BrushToColorValidationRule()
    {
    }

    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        if (value is Brush)
            return ValidationResult.ValidResult;

        if (value is Color)
            return ValidationResult.ValidResult;

        return new ValidationResult(false, $"{value?.GetType()} is not {typeof(Brush)} or {typeof(Color)}.");
    }
}

