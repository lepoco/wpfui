#nullable enable
using System;

namespace Wpf.Ui.Services.Internal;

internal sealed class NavigationManager
{
    public string CurrentTag { get; private set; }

    public void NavigateToByTag(string tag, object? dataContext = null)
    {

    }

    public void NavigateToByType(Type type, object? dataContext = null)
    {

    }

    public void NavigateToById(int id, object? dataContext = null)
    {

    }
}
