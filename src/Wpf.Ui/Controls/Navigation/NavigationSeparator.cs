// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.ComponentModel;
using System.Drawing;
using Wpf.Ui.Controls.Interfaces;

namespace Wpf.Ui.Controls.Navigation;

[ToolboxItem(true)]
[ToolboxBitmap(typeof(NavigationSeparator), "NavigationSeparator.bmp")]
public class NavigationSeparator : System.Windows.Controls.Separator, INavigationControl
{
}
