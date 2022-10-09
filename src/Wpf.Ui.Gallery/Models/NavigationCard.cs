// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using Wpf.Ui.Common;

namespace Wpf.Ui.Gallery.Models;

public class NavigationCard
{
    public string Name { get; set; } = String.Empty;

    public SymbolRegular Icon { get; set; } = SymbolRegular.Empty;

    public string Description { get; set; } = String.Empty;

    public string Link { get; set; } = String.Empty;
}
