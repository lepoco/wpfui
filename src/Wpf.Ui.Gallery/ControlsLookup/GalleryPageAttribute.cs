// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Wpf.Ui.Controls;

namespace Wpf.Ui.Gallery.ControlsLookup;

[AttributeUsage(AttributeTargets.Class)]
internal class GalleryPageAttribute : Attribute
{
    public string Description { get; }

    public SymbolRegular Icon { get; }

    public GalleryPageAttribute(string description, SymbolRegular icon)
    {
        Description = description;
        Icon = icon;
    }
}
