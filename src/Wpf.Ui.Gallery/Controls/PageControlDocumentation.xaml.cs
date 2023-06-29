using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Controls;

namespace Wpf.Ui.Gallery.Controls;

public class PageControlDocumentation : Control
{
    public static readonly DependencyProperty NavigationViewProperty = DependencyProperty.Register(
        nameof(NavigationView),
        typeof(INavigationView),
        typeof(PageControlDocumentation),
        new FrameworkPropertyMetadata(null)
    );

    public INavigationView? NavigationView
    {
        get => (INavigationView)GetValue(NavigationViewProperty);
        set => SetValue(NavigationViewProperty, value);
    }

    public PageControlDocumentation()
    {
        Loaded += static (sender, _) => ((PageControlDocumentation)sender).OnLoaded();
        Unloaded += static (sender, _) => ((PageControlDocumentation)sender).OnUnloaded();
    }

    private void OnLoaded()
    {
        if (NavigationView is null)
            throw new ArgumentNullException(nameof(NavigationView));

        NavigationView.Navigating += NavigationViewOnNavigating;
    }

    private void OnUnloaded()
    {
        NavigationView!.Navigating += NavigationViewOnNavigating;
    }

    private void NavigationViewOnNavigating(NavigationView sender, NavigatingCancelEventArgs args)
    {
        
    }
}
