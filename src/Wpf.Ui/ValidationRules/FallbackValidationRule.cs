// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Globalization;
using System.Windows.Controls;

namespace Wpf.Ui.ValidationRules;

internal class FallbackValidationRule : ValidationRule
{
    public FallbackValidationRule()
    {
    }

    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        return ValidationResult.ValidResult;
    }
}
