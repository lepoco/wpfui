// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows.Markup;

using Wpf.Ui.Controls;

namespace Wpf.Ui.Markup;

/// <summary>
/// Markup extension that creates a VectorIcon from Geometry or path data.
/// </summary>
[ContentProperty(nameof(Data))]
[MarkupExtensionReturnType(typeof(VectorIcon))]
public class VectorIconExtension : MarkupExtension
{
    public VectorIconExtension() { }

    public VectorIconExtension(Geometry data)
    {
        Data = data;
    }

    public VectorIconExtension(string pathData)
    {
        Data = Geometry.Parse(pathData);
    }

    /// <summary>
    /// The geometry used to draw the icon.
    /// </summary>
    [ConstructorArgument("data")]
    public Geometry? Data { get; set; }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return Data is null
            ? new VectorIcon()
            : new VectorIcon { Data = Data };
    }
}
