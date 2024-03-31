// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows.Controls;
using Wpf.Ui.Appearance;
using Wpf.Ui.Interop;

namespace Wpf.Ui.Extensions;

/// <summary>
/// Extensions for the <see cref="ContextMenu"/>.
/// </summary>
internal static class ContextMenuExtensions
{
    /// <summary>
    /// Tries to apply Mica effect to the <see cref="ContextMenu"/>.
    /// </summary>
    public static void ApplyMica(this ContextMenu contextMenu)
    {
        contextMenu.Opened += ContextMenuOnOpened;
    }

    private static void ContextMenuOnOpened(object sender, RoutedEventArgs e)
    {
        if (sender is not ContextMenu contextMenu
            || PresentationSource.FromVisual(contextMenu) is not HwndSource source)
        {
            return;
        }

        if (ApplicationThemeManager.GetAppTheme() == ApplicationTheme.Dark)
        {
            UnsafeNativeMethods.ApplyWindowDarkMode(source.Handle);
        }

        // TODO: Needs more work with the Popup service

        /*if (Background.Apply(source.Handle, BackgroundType.Mica))
            contextMenu.Background = Brushes.Transparent;*/
    }
}
