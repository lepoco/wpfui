using System.ComponentModel;
using System.Windows;

namespace WPFUI.Common
{
    /// <summary>
    /// Helper class for Visual Studio designer.
    /// </summary>
    internal static class Designer
    {
        private static bool _validated = false;

        private static bool _isInDesignMode = false;

        /// <summary>
        /// Indicates whether the project is currently in design mode.
        /// </summary>
        public static bool IsInDesignMode
        {
            get
            {
                ValidateDesigner();

                return _isInDesignMode;
            }
        }

        private static void ValidateDesigner()
        {
            if (_validated)
                _isInDesignMode = (bool)(DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject))?.DefaultValue ?? false);

            _validated = true;
        }
    }
}
