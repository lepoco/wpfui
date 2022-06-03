// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

namespace WPFUI.Demo.Models.Icons;

public struct DisplayableIcon
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Code { get; set; }

    public string Symbol { get; set; }

    public WPFUI.Common.SymbolRegular Icon { get; set; }
}
