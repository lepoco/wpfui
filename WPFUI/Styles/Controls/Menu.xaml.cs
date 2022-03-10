using System.Reflection;
using System.Windows;

namespace WPFUI.Styles.Controls
{
    partial class Menu : ResourceDictionary
    {

        public Menu()
        {
            Initialize();
        }


        private void Initialize()
        {
            if (SystemParameters.MenuDropAlignment)
            {
                FieldInfo fi = typeof(SystemParameters).GetField("_menuDropAlignment", BindingFlags.NonPublic | BindingFlags.Static);
                fi.SetValue(null, false);
            }
        }

    }
}
