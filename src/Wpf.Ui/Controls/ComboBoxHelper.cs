// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows.Controls;

namespace Wpf.Ui.Controls;

/// <summary>
/// Used to extend the stock WPF ComboBox control. 
/// </summary>
public static class ComboBoxHelper
{
    public static readonly DependencyProperty PlaceholderProperty =
        DependencyProperty.RegisterAttached(
            "Placeholder",
            typeof(object),
            typeof(ComboBoxHelper),
            new PropertyMetadata(null));

    public static object GetPlaceholder(ComboBox control) => control.GetValue(PlaceholderProperty);

    public static void SetPlaceholder(ComboBox control, object value) => control.SetValue(PlaceholderProperty, value);
}
