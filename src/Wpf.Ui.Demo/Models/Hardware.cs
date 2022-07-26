// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Collections.Generic;

namespace Wpf.Ui.Demo.Models;

public class Hardware
{
    public string Name { get; set; } = String.Empty;
    public double Value { get; set; } = 0d;
    public double Min { get; set; } = 0d;
    public double Max { get; set; } = 0d;

    public IEnumerable<Hardware> SubItems { get; set; } = new Hardware[] { };
}

