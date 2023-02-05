// Based on Windows UI Library
// Copyright(c) Microsoft Corporation.All rights reserved.

// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;

namespace Wpf.Ui.Controls.BreadcrumbControl;

/// <summary>
/// Represents an item in a <see cref="BreadcrumbBar"/> control.
/// </summary>
public class BreadcrumbBarItem : System.Windows.Controls.ContentControl
{
    /// <summary>
    /// Property for <see cref="SymbolIconFontSize"/>.
    /// </summary>
    public static readonly DependencyProperty SymbolIconFontSizeProperty =
        DependencyProperty.Register(nameof(SymbolIconFontSize), typeof(double), typeof(BreadcrumbBarItem),
            new PropertyMetadata(18.0));

    /// <summary>
    /// Property for <see cref="SymbolIconFontWeight"/>.
    /// </summary>
    public static readonly DependencyProperty SymbolIconFontWeightProperty =
        DependencyProperty.Register(nameof(SymbolIconFontWeight), typeof(FontWeight), typeof(BreadcrumbBarItem),
            new PropertyMetadata(FontWeights.DemiBold));

    /// <summary>
    /// Property for <see cref="SymbolIconSymbol"/>.
    /// </summary>
    public static readonly DependencyProperty SymbolIconSymbolProperty =
        DependencyProperty.Register(nameof(SymbolIconSymbol), typeof(Common.SymbolRegular), typeof(BreadcrumbBarItem),
            new PropertyMetadata(Common.SymbolRegular.ChevronRight24));

    /// <summary>
    /// Property for <see cref="SymbolIconMargin"/>.
    /// </summary>
    public static readonly DependencyProperty SymbolIconMarginProperty =
        DependencyProperty.Register(nameof(SymbolIconMargin), typeof(Thickness), typeof(BreadcrumbBarItem),
            new PropertyMetadata(new Thickness(10, 0, 10, 0)));

    /// <summary>
    /// Property for <see cref="IsLast"/>.
    /// </summary>
    public static readonly DependencyProperty IsLastProperty =
        DependencyProperty.Register(nameof(IsLast), typeof(bool), typeof(BreadcrumbBarItem),
            new PropertyMetadata(false));

    /// <summary>
    /// Font size of the <see cref="SymbolIconSymbol"/>.
    /// </summary>
    public double SymbolIconFontSize
    {
        get => (double)GetValue(SymbolIconFontSizeProperty);
        set => SetValue(SymbolIconFontSizeProperty, value);
    }

    /// <summary>
    /// Font weight of the <see cref="SymbolIconSymbol"/>.
    /// </summary>
    public FontWeight SymbolIconFontWeight
    {
        get => (FontWeight)GetValue(SymbolIconFontWeightProperty);
        set => SetValue(SymbolIconFontWeightProperty, value);
    }

    /// <summary>
    /// Symbol used to represent the item.
    /// </summary>
    public Common.SymbolRegular SymbolIconSymbol
    {
        get => (Common.SymbolRegular)GetValue(SymbolIconSymbolProperty);
        set => SetValue(SymbolIconSymbolProperty, value);
    }

    /// <summary>
    /// Margin of the <see cref="SymbolIconSymbol"/>.
    /// </summary>
    public Thickness SymbolIconMargin
    {
        get => (Thickness)GetValue(SymbolIconMarginProperty);
        set => SetValue(SymbolIconMarginProperty, value);
    }

    /// <summary>
    /// Whether the current item is the last one.
    /// </summary>
    public bool IsLast
    {
        get => (bool)GetValue(IsLastProperty);
        set => SetValue(IsLastProperty, value);
    }
}
