// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Reflection;

namespace Wpf.Ui.Controls;

/// <summary>
/// Changes readonly field value of <see cref="SystemParameters.MenuDropAlignment"/> to false.
/// </summary>
public partial class MenuLoader : ResourceDictionary
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MenuLoader"/> class.
    /// </summary>
    /// <remarks>
    /// Sets menu alignment on initialization.
    /// </remarks>
    public MenuLoader()
    {
        MenuLoader.Initialize();
    }

    private static void Initialize()
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
