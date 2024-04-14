// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

namespace Wpf.Ui.Gallery.Helpers;

internal sealed class NameToPageTypeConverter
{
    private static readonly Type[] PageTypes = Assembly
        .GetExecutingAssembly()
        .GetTypes()
        .Where(t => t.Namespace?.StartsWith("Wpf.Ui.Gallery.Views.Pages") ?? false)
        .ToArray();

    public static Type? Convert(string pageName)
    {
        pageName = pageName.Trim().ToLower() + "page";

        return PageTypes.FirstOrDefault(singlePageType =>
            singlePageType.Name.Equals(pageName, StringComparison.CurrentCultureIgnoreCase)
        );
    }
}
