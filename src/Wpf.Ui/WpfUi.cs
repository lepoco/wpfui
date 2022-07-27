using System;

namespace Wpf.Ui
{
    public static class WpfUi
    {
#if NET48_OR_GREATER
        internal static IServiceProvider ServiceProvider { get; private set; }

        public static void Initialize(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
                throw new ArgumentNullException(nameof(serviceProvider));

            ServiceProvider = serviceProvider;
        }
#endif

#if NETCOREAPP3_0_OR_GREATER
        internal static IServiceProvider ServiceProvider { get; private set; }

        public static void Initialize(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
                throw new ArgumentNullException(nameof(serviceProvider));

            ServiceProvider = serviceProvider;
        }
#endif
    }
}
