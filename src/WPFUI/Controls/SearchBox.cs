// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;

namespace WPFUI.Controls;

/// <summary>
/// Lets look for things and other stuff.
/// </summary>
public class SearchBox : WPFUI.Controls.AutoSuggestBox
{
    /// <summary>
    /// Property override for <see cref="WPFUI.Controls.TextBox.Icon"/>.
    /// </summary>
    // Static constructor.
    static SearchBox()
    {
        FrameworkPropertyMetadata newIconMetadata = new(
            defaultValue: Common.SymbolRegular.Search24);

        IconProperty.OverrideMetadata(
            forType: typeof(SearchBox),
            typeMetadata: newIconMetadata);
    }
}
