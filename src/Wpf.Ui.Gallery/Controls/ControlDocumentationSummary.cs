// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Windows;

namespace Wpf.Ui.Gallery.Controls;

public class ControlDocumentationSummary : System.Windows.Controls.Control
{
    /// <summary>
    /// Property for <see cref="XamlUrl"/>.
    /// </summary>
    public static readonly DependencyProperty XamlUrlProperty =
        DependencyProperty.Register(nameof(XamlUrl),
            typeof(string), typeof(ControlDocumentationSummary), new PropertyMetadata(String.Empty));

    /// <summary>
    /// Property for <see cref="CsharpUrl"/>.
    /// </summary>
    public static readonly DependencyProperty CsharpUrlProperty =
        DependencyProperty.Register(nameof(CsharpUrl),
            typeof(string), typeof(ControlDocumentationSummary), new PropertyMetadata(String.Empty));

    public string XamlUrl
    {
        get => (string)GetValue(XamlUrlProperty);
        set => SetValue(XamlUrlProperty, value);
    }

    public string CsharpUrl
    {
        get => (string)GetValue(CsharpUrlProperty);
        set => SetValue(CsharpUrlProperty, value);
    }
}
