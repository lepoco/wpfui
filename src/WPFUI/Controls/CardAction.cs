// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;
using WPFUI.Controls.Interfaces;

namespace WPFUI.Controls;

/// <summary>
/// Inherited from the <see cref="System.Windows.Controls.Primitives.ButtonBase"/> interactive card styled according to Fluent Design.
/// </summary>
//#if NETFRAMEWORK
//    [ToolboxBitmap(typeof(Button))]
//#endif
public class CardAction : System.Windows.Controls.Primitives.ButtonBase, IIconControl
{
    /// <summary>
    /// Property for <see cref="ShowChevron"/>.
    /// </summary>
    public static readonly DependencyProperty ShowChevronProperty = DependencyProperty.Register(nameof(ShowChevron),
        typeof(bool), typeof(CardAction), new PropertyMetadata(true));

    /// <summary>
    /// Property for <see cref="Icon"/>.
    /// </summary>
    public static readonly DependencyProperty IconProperty = DependencyProperty.Register(nameof(Icon),
        typeof(Common.SymbolRegular), typeof(CardAction),
        new PropertyMetadata(Common.SymbolRegular.Empty));

    /// <summary>
    /// Property for <see cref="IconFilled"/>.
    /// </summary>
    public static readonly DependencyProperty IconFilledProperty = DependencyProperty.Register(nameof(IconFilled),
        typeof(bool), typeof(CardAction), new PropertyMetadata(false));

    /// <summary>
    /// Gets or sets information whether to display the chevron icon on the right side of the card.
    /// </summary>
    public bool ShowChevron
    {
        get => (bool)GetValue(ShowChevronProperty);
        set => SetValue(ShowChevronProperty, value);
    }

    /// <inheritdoc />
    public Common.SymbolRegular Icon
    {
        get => (Common.SymbolRegular)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    /// <inheritdoc />
    public bool IconFilled
    {
        get => (bool)GetValue(IconFilledProperty);
        set => SetValue(IconFilledProperty, value);
    }
}
