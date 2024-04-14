// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows.Data;
using Wpf.Ui.Controls;

namespace Wpf.Ui.Converters;

/// <summary>
/// Converts an <see cref="IconSourceElement"/> to an <see cref="IconElement"/>.
/// </summary>
public class IconSourceElementConverter : IValueConverter
{
    /// <summary>
    /// Converts a value to an <see cref="IconElement"/>.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <param name="targetType">The type of the binding target property.</param>
    /// <param name="parameter">The converter parameter.</param>
    /// <param name="culture">The culture to use in the converter.</param>
    /// <returns>The converted <see cref="IconElement"/>.</returns>
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is IconSourceElement iconSourceElement)
        {
            return iconSourceElement.CreateIconElement();
        }

        return value;
    }

    /// <summary>
    /// Converts an <see cref="IconElement"/> back to an IconSourceElement.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <param name="targetType">The type of the binding target property.</param>
    /// <param name="parameter">The converter parameter.</param>
    /// <param name="culture">The culture to use in the converter.</param>
    /// <returns>The converted IconSourceElement.</returns>
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}