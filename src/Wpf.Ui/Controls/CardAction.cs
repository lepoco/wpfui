// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.ComponentModel;
using System.Drawing;
using System.Windows;
using Wpf.Ui.Controls.IconElements;
using Wpf.Ui.Converters;

namespace Wpf.Ui.Controls;

/// <summary>
/// Inherited from the <see cref="System.Windows.Controls.Primitives.ButtonBase"/> interactive card styled according to Fluent Design.
/// </summary>
//#if NETFRAMEWORK
//    [ToolboxBitmap(typeof(Button))]
//#endif
[ToolboxItem(true)]
[ToolboxBitmap(typeof(CardAction), "CardAction.bmp")]
public class CardAction : System.Windows.Controls.Primitives.ButtonBase
{
    /// <summary>
    /// Property for <see cref="IsChevronVisible"/>.
    /// </summary>
    public static readonly DependencyProperty IsChevronVisibleProperty = DependencyProperty.Register(nameof(IsChevronVisible),
        typeof(bool), typeof(CardAction), new PropertyMetadata(true));

    /// <summary>
    /// Property for <see cref="Icon"/>.
    /// </summary>
    public static readonly DependencyProperty IconProperty = DependencyProperty.Register(nameof(Icon),
        typeof(IconElement), typeof(CardAction),
        new PropertyMetadata(null, null, IconSourceElementConverter.ConvertToIconElement));

    /// <summary>
    /// Gets or sets information whether to display the chevron icon on the right side of the card.
    /// </summary>
    [Bindable(true), Category("Appearance")]
    public bool IsChevronVisible
    {
        get => (bool)GetValue(IsChevronVisibleProperty);
        set => SetValue(IsChevronVisibleProperty, value);
    }

    /// <summary>
    /// Gets or sets displayed <see cref="IconElement"/>.
    /// </summary>
    [Bindable(true), Category("Appearance")]
    public IconElement? Icon
    {
        get => (IconElement)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }
}
