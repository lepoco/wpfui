// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

namespace Wpf.Ui.Demo.Console.Views;

public partial class SimpleView
{
    public SimpleView()
    {
        InitializeComponent();
        this.ApplyTheme();
    }

    private void CardAction_Click(object sender, RoutedEventArgs e)
    {
        ThemeUtilities.ChangeTheme();
    }
}
