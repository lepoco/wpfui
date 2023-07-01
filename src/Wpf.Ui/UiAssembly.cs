using System.Reflection;

namespace Wpf.Ui;

public static class UiAssembly
{
    /// <summary>
    /// Gets the WPF UI assembly.
    /// </summary>
    public static Assembly Asssembly => Assembly.GetExecutingAssembly();
}
