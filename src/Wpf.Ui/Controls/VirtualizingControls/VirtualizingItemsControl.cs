// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Based on VirtualizingWrapPanel created by S. Bäumlisberger licensed under MIT license.
// https://github.com/sbaeumlisberger/VirtualizingWrapPanel
// Copyright (C) S. Bäumlisberger, Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.ComponentModel;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;

namespace Wpf.Ui.Controls.VirtualizingControls;

/// <summary>
/// Virtualized <see cref="ItemsControl"/>.
/// <para>Based on <see href="https://github.com/sbaeumlisberger/VirtualizingWrapPanel"/>.</para>
/// </summary>
[ToolboxItem(true)]
[ToolboxBitmap(typeof(VirtualizingItemsControl), "VirtualizingItemsControl.bmp")]
public class VirtualizingItemsControl : System.Windows.Controls.ItemsControl
{
    /// <summary>
    /// Property for <see cref="CacheLengthUnit"/>.
    /// </summary>
    public static readonly DependencyProperty CacheLengthUnitProperty =
        DependencyProperty.Register(nameof(CacheLengthUnit), typeof(VirtualizationCacheLengthUnit), typeof(VirtualizingItemsControl),
            new FrameworkPropertyMetadata(VirtualizationCacheLengthUnit.Page));

    /// <summary>
    /// Gets or sets the cache length unit.
    /// </summary>
    public VirtualizationCacheLengthUnit CacheLengthUnit
    {
        get => VirtualizingPanel.GetCacheLengthUnit(this);
        set
        {
            SetValue(CacheLengthUnitProperty, value);
            VirtualizingPanel.SetCacheLengthUnit(this, value);
        }
    }

    /// <summary>
    /// Constructor that initialize the <see cref="VirtualizingPanel"/>.
    /// </summary>
    public VirtualizingItemsControl()
    {
        VirtualizingPanel.SetCacheLengthUnit(this, CacheLengthUnit);
        VirtualizingPanel.SetCacheLength(this, new VirtualizationCacheLength(1));
        VirtualizingPanel.SetIsVirtualizingWhenGrouping(this, true);
    }
}

