// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

namespace Wpf.Ui.Controls.Navigation;

internal class NavigationViewBreadcrumbItem
{
    public NavigationViewBreadcrumbItem(INavigationViewItem  item)
    {
        Content = item.Content;
        PageId = item.Id;
    }

    public object Content { get; }
    public string PageId { get; }
}
