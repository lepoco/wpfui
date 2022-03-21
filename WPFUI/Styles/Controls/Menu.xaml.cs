using System.Reflection;
using System.Windows;

namespace WPFUI.Styles.Controls
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
            if (!SystemParameters.MenuDropAlignment) return;

            var fi = typeof(SystemParameters).GetField("_menuDropAlignment", BindingFlags.NonPublic | BindingFlags.Static);
            fi?.SetValue(null, false);
        }

    }
}
