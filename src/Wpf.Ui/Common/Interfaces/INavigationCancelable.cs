#nullable enable
using Wpf.Ui.Controls.Interfaces;

namespace Wpf.Ui.Common.Interfaces;

/// <summary>
/// TODO
/// </summary>
public interface INavigationCancelable
{
    /// <summary>
    /// TODO
    /// </summary>
    /// <returns></returns>
    bool CouldNavigate(INavigationItem? navigationFrom);
}
