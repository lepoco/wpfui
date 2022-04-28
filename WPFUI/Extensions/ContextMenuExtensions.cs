using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using WPFUI.Appearance;

namespace WPFUI.Extensions
{
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
            if (sender is not ContextMenu contextMenu)
                return;

            var source = PresentationSource.FromVisual(contextMenu) as HwndSource;

            if (source == null)
                return;

            if (Theme.IsMatchedDark())
                Background.ApplyDarkMode(source.Handle);

            // Needs more work with the Popup service

            //if (Background.Apply(source.Handle, BackgroundType.Mica))
            //    contextMenu.Background = Brushes.Transparent;
        }
    }
}
