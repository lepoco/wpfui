using System;

namespace Wpf.Ui
{
    /// <summary>
    /// Used to initialize the library with static values.
    /// </summary>
    public static class WpfUi
    {
#if NET48_OR_GREATER || NETCOREAPP3_0_OR_GREATER
        internal static IServiceProvider ServiceProvider { get; private set; }

        /// <summary>
        /// Accepts a ServiceProvider for configuring dependency injection.
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void Initialize(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
                throw new ArgumentNullException(nameof(serviceProvider));

            ServiceProvider = serviceProvider;
        }
#endif
    }
}
