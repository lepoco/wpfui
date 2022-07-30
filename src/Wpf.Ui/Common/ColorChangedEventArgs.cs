// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Windows.Media;

namespace Wpf.Ui.Common;

public class ColorChangedEventArgs : EventArgs
{
    public Color NewColor { get; }

    public Color OldColor { get; }

    public ColorChangedEventArgs(Color newColor, Color oldColor)
    {
        NewColor = newColor;
        OldColor = oldColor;
    }
}
