// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Reflection;
using System.Windows;

namespace Wpf.Ui.Styles.Controls
{
    /// <summary>
    /// Extension to the menu.
    /// </summary>
    partial class Menu : ResourceDictionary
    {
        /// <summary>
        /// Sets menu alignment on initialization.
        /// </summary>
        public Menu() => Initialize();

        private void Initialize()
        {
            if (!SystemParameters.MenuDropAlignment)
                return;

            var fieldInfo = typeof(SystemParameters).GetField("_menuDropAlignment", BindingFlags.NonPublic | BindingFlags.Static);
            fieldInfo?.SetValue(null, false);
        }

    }
}
