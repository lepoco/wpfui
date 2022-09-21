// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

namespace Wpf.Ui.Styles.Controls
{
    public partial class InfoBar
    {
        private void CloseButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (sender is not System.Windows.Controls.Button button) return;

            if (button.TemplatedParent is Wpf.Ui.Controls.InfoBar infoBar)
                infoBar.IsOpen = false;
        }
    }
}
