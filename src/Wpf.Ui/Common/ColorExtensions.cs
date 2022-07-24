// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
//
// Code from https://github.com/microsoft/microsoft-ui-xaml/
//

using System.Windows.Media;

namespace Wpf.Ui.Common;

public static class ColorExtensions
{
    public static string GetHexCode(this Color color, bool includeAlpha)
    {
        string alphaAsString = color.A.ToString("X2");
        
        string hexCode = "#";
        if (includeAlpha)
        {
            hexCode += alphaAsString;
        }

        hexCode += color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");

        return hexCode;
    }

    public static HsvColor ToHsvColor(this Color color)
    {
        return HsvColor.FromColor(color);
    }
}
