// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Windows;

namespace Wpf.Ui.Gallery.Controls;

public partial class ControlSummaryUserControl
{
    public static readonly DependencyProperty XamlUrlProperty =
        DependencyProperty.Register(
            nameof(XamlUrl), typeof(string), typeof(ControlSummaryUserControl),
            new FrameworkPropertyMetadata(String.Empty));

    public static readonly DependencyProperty CsharpUrlProperty =
        DependencyProperty.Register(
            nameof(CsharpUrl), typeof(string), typeof(ControlSummaryUserControl),
            new FrameworkPropertyMetadata(String.Empty));

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

    public ControlSummaryUserControl()
    {
        InitializeComponent();
    }
}
