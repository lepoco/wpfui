// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows.Media;

namespace Wpf.Ui.Common.Media;

public static class ColorExtensions
{
    public static string GetHexCode(this Color color, bool includeAlpha)
    {
        var alphaAsString = color.A.ToString("X2");

        var hexCode = "#";
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
