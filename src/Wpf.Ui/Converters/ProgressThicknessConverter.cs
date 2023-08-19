// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows.Data;

namespace Wpf.Ui.Converters;

internal class ProgressThicknessConverter : IValueConverter
{
    public object Convert(
        object? value,
        Type targetType,
        object? parameter,
        CultureInfo culture
    )
    {
        // TODO: It's too hardcoded, we should define better formula.

        if (value is double height)
        {
            return height / 8;
        }

        return 12.0d;
    }

    public object ConvertBack(
        object? value,
        Type targetType,
        object? parameter,
        CultureInfo culture
    )
    {
        throw new NotImplementedException();
    }
}
