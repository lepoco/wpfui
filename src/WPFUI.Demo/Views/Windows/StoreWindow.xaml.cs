// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

namespace WPFUI.Demo.Views.Windows;

/// <summary>
/// Interaction logic for StoreWindow.xaml
/// </summary>
public partial class StoreWindow : WPFUI.Controls.UiWindow
{
    public StoreWindow()
    {
        InitializeComponent();

        WPFUI.Appearance.Background.Apply(
            this,
            WPFUI.Appearance.BackgroundType.Mica);
    }
}
