#nullable enable
using System.ComponentModel;
using System.Windows;
using Wpf.Ui.Controls.Interfaces;

namespace Wpf.Ui.Controls.Navigation;

/// <summary>
/// Inherited from the <see cref="Wpf.Ui.Controls.Button"/>, used to navigate backwards/>.
/// </summary>
public class NavigationBackButton : Button
{
    /// <summary>
    /// Property for <see cref="Navigation"/>.
    /// </summary>
    public static readonly DependencyProperty NavigationProperty = DependencyProperty.Register(nameof(Navigation),
        typeof(INavigation), typeof(NavigationBackButton), new PropertyMetadata(null));

    /// <summary>
    /// <see cref="INavigation"/>
    /// </summary>
    [Bindable(true), Category("Behavior")]
    public INavigation? Navigation
    {
        get => (INavigation)GetValue(NavigationProperty);
        set => SetValue(NavigationProperty, value);
    }

    public NavigationBackButton()
    {
        SetValue(CommandProperty, new Common.RelayCommand(o => Navigation?.NavigateBack(), () => Navigation is not null && Navigation.CanGoBack));
    }
}
