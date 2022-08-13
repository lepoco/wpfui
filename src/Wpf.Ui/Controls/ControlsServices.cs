using System;

namespace Wpf.Ui.Controls
{
    /// <summary>
    /// Used to initialize the library controls with static values.
    /// </summary>
    public static class ControlsServices
    {
#if NET48_OR_GREATER || NETCOREAPP3_0_OR_GREATER
        internal static IServiceProvider ControlsServiceProvider { get; private set; }

        /// <summary>
        /// Accepts a ServiceProvider for configuring dependency injection.
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void Initialize(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
                throw new ArgumentNullException(nameof(serviceProvider));

            ControlsServiceProvider = serviceProvider;
        }
#endif
    }
}
