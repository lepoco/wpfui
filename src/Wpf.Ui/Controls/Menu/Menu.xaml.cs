// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Reflection;

namespace Wpf.Ui.Styles.Controls;

/// <summary>
/// Extension to the menu.
/// </summary>
public partial class Menu : ResourceDictionary
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Menu"/> class.
    /// </summary>
    /// <remarks>
    /// Sets menu alignment on initialization.
    /// </remarks>
    public Menu()
    {
        Initialize();
    }

    private void Initialize()
    {
        if (!SystemParameters.MenuDropAlignment)
        {
            return;
        }

        FieldInfo? fieldInfo = typeof(SystemParameters).GetField(
            "_menuDropAlignment",
            BindingFlags.NonPublic | BindingFlags.Static
        );

        fieldInfo?.SetValue(null, false);
    }
}
