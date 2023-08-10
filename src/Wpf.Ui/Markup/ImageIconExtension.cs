// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Windows.Markup;
using System.Windows.Media;
using Wpf.Ui.Controls;

namespace Wpf.Ui.Markup;

/// <summary>
/// Custom <see cref="MarkupExtension"/> which can provide <see cref="ImageIcon"/>.
/// </summary>
/// <example>
/// <code lang="xml">
/// &lt;ui:Button
///     Appearance="Primary"
///     Content="WPF UI button with font icon"
///     Icon="{ui:ImageIcon '/my-icon.png'}" /&gt;
/// </code>
/// <code lang="xml">
/// &lt;ui:Button Icon="{ui:ImageIcon 'pack://application:,,,/Assets/wpfui.png'}" /&gt;
/// </code>
/// <code lang="xml">
/// &lt;ui:Hyperlink Icon="{ui:ImageIcon 'pack://application:,,,/Assets/wpfui.png'}" /&gt;
/// </code>
/// <code lang="xml">
/// &lt;ui:TitleBar Icon="{ui:ImageIcon 'pack://application:,,,/Assets/wpfui.png'}" /&gt;
/// </code>
/// </example>
[ContentProperty(nameof(Source))]
[MarkupExtensionReturnType(typeof(ImageIcon))]
public class ImageIconExtension : MarkupExtension
{
    public ImageIconExtension(ImageSource? source)
    {
        Source = source;
    }

    [ConstructorArgument("source")]
    public ImageSource? Source { get; set; }

    public double Width { get; set; } = 16D;

    public double Height { get; set; } = 16D;

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        var imageIcon = new ImageIcon { Source = Source, Width = Width, Height = Height };

        return imageIcon;
    }
}
