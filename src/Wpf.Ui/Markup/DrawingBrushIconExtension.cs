// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows.Markup;
using Wpf.Ui.Controls;

namespace Wpf.Ui.Markup;

[ContentProperty(nameof(Icon))]
[MarkupExtensionReturnType(typeof(DrawingBrushIcon))]
public class DrawingBrushIconExtension : MarkupExtension
{
    public DrawingBrushIconExtension(DrawingBrush icon)
    {
        Icon = icon;
    }


    [ConstructorArgument("icon")]
    public DrawingBrush Icon { get; set; }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        var drawingBrushIcon = new DrawingBrushIcon { Icon = Icon };

        return drawingBrushIcon;
    }
}
