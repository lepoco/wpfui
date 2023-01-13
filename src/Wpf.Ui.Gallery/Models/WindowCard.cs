// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

namespace Wpf.Ui.Gallery.Models;

public record WindowCard
{
    public string Name { get; set; }

    public string Description { get; init; }

    public SymbolRegular Icon { get; init; }

    public string Value { get; set; }

    public WindowCard(string name, string description, SymbolRegular icon, string value)
    {
        Name = name;
        Description = description;
        Icon = icon;
        Value = value;
    }
}
