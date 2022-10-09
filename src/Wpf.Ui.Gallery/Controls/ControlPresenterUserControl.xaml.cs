// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Windows;

namespace Wpf.Ui.Gallery.Controls;

public partial class ControlPresenterUserControl
{
    public static readonly DependencyProperty ControlContentProperty =
        DependencyProperty.Register(
            nameof(ControlContent), typeof(object), typeof(ControlPresenterUserControl),
            new FrameworkPropertyMetadata(null!));

    public static readonly DependencyProperty HeaderTextProperty =
        DependencyProperty.Register(
            nameof(HeaderText), typeof(string), typeof(ControlPresenterUserControl),
            new FrameworkPropertyMetadata(String.Empty));

    public static readonly DependencyProperty CodeTextProperty =
        DependencyProperty.Register(
            nameof(CodeText), typeof(string), typeof(ControlPresenterUserControl),
            new FrameworkPropertyMetadata(String.Empty));
    public object ControlContent
    {
        get => GetValue(ControlContentProperty);
        set => SetValue(ControlContentProperty, value);
    }

    public string HeaderText
    {
        get => (string)GetValue(HeaderTextProperty);
        set => SetValue(HeaderTextProperty, value);
    }

    public string CodeText
    {
        get => (string)GetValue(HeaderTextProperty);
        set => SetValue(HeaderTextProperty, value);
    }

    public ControlPresenterUserControl()
    {
        InitializeComponent();
    }
}
