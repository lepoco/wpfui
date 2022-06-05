// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

namespace WPFUI.Services;

/// <summary>
/// Additional data passed through the <see cref="System.Windows.Controls.Frame.Navigate"/> method.
/// </summary>
internal struct NavigationServiceExtraData
{
    /// <summary>
    /// Current page id.
    /// </summary>
    public int PageId { get; set; }

    /// <summary>
    /// Whether we should use the cache.
    /// </summary>
    public bool Cache { get; set; }

    /// <summary>
    /// Additional <see cref="System.Windows.FrameworkElement.DataContext"/>.
    /// </summary>
    public object DataContext { get; set; }
}
