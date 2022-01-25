using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WPFUI.Common
{
    /// <summary>
    /// Provides access to various DPI-related methods.
    /// </summary>
    internal static class Dpi
    {
        // TODO: look into utilizing preprocessor symbols for more functionality
        // ----
        // There is an opportunity to check against NET46 if we can use
        // VisualTreeHelper in this class. We are currently not utilizing
        // it because it is not available in .NET Framework 4.6 (available
        // starting 4.6.2). For now, there is no need to overcomplicate this
        // solution for some infrequent DPI calculations. However, if this
        // becomes more central to various implementations, we may want to
        // look into fleshing it out a bit further.
        // ----
        // Reference: https://docs.microsoft.com/en-us/dotnet/standard/frameworks

        /// <summary>
        /// Gets the horizontal DPI value from <see cref="SystemParameters"/>.
        /// </summary>
        /// <returns>The horizontal DPI value from <see cref="SystemParameters"/>. If the property cannot be accessed, the default value 96 is returned.</returns>
        internal static int SystemDpiX()
        {
            var dpiProperty = typeof(SystemParameters).GetProperty("DpiX", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            if (dpiProperty == null)
                return 96;

            return (int)dpiProperty.GetValue(null, null);
        }

        /// <summary>
        /// Calculates the horizontal DPI scale factor based on the DpiX <see cref="SystemParameters"/> property value.
        /// </summary>
        /// <returns>The horizontal DPI scale factor.</returns>
        internal static double SystemDpiXScale()
        {
            return SystemDpiX() / 96.0;
        }

        /// <summary>
        /// Gets the vertical DPI value from <see cref="SystemParameters"/>.
        /// </summary>
        /// <returns>The vertical DPI value from <see cref="SystemParameters"/>. If the property cannot be accessed, the default value 96 is returned.</returns>
        internal static int SystemDpiY()
        {
            var dpiProperty = typeof(SystemParameters).GetProperty("Dpi", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            if (dpiProperty == null)
                return 96;

            return (int)dpiProperty.GetValue(null, null);
        }

        /// <summary>
        /// Calculates the vertical DPI scale factor based on the "Dpi" <see cref="SystemParameters"/> property value.
        /// </summary>
        /// <returns>The vertical DPI scale factor.</returns>
        internal static double SystemDpiYScale()
        {
            return SystemDpiY() / 96.0;
        }
    }
}
