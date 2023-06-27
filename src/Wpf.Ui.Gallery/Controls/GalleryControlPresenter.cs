// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Windows;

namespace Wpf.Ui.Gallery.Controls;

public class GalleryControlPresenter : System.Windows.Controls.ContentControl
{
    /// <summary>
    /// Property for <see cref="HeaderText"/>.
    /// </summary>
    public static readonly DependencyProperty HeaderTextProperty = DependencyProperty.Register(
        nameof(HeaderText),
        typeof(string),
        typeof(GalleryControlPresenter),
        new PropertyMetadata(String.Empty)
    );

    /// <summary>
    /// Property for <see cref="CodeText"/>.
    /// </summary>
    public static readonly DependencyProperty CodeTextProperty = DependencyProperty.Register(
        nameof(CodeText),
        typeof(string),
        typeof(GalleryControlPresenter),
        new PropertyMetadata(String.Empty)
    );

    public string HeaderText
    {
        get => (string)GetValue(HeaderTextProperty);
        set => SetValue(HeaderTextProperty, value);
    }

    public string CodeText
    {
        get => (string)GetValue(CodeTextProperty);
        set => SetValue(CodeTextProperty, value);
    }
}
