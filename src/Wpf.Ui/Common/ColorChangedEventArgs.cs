// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
//
// Code from https://github.com/microsoft/microsoft-ui-xaml/
//

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
