using System;
using System.Collections.Generic;

namespace Wpf.Ui.Controls.Navigation;

internal class NavigationCache
{
    private IDictionary<Type, object?> _entires = new Dictionary<Type, object>();

    public object? Remember(Type? entryType, NavigationCacheMode cacheMode, Func<object?> generate)
    {
        if (entryType == null)
        {
            return null;
        }

        if (cacheMode == NavigationCacheMode.Disabled)
        {
            return generate.Invoke();
        }

        if (!_entires.TryGetValue(entryType, out object? value))
        {
            value = generate.Invoke();

            _entires.Add(entryType, value);
        }

        return value;
    }
}
