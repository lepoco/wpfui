using System.Windows;

namespace Wpf.Ui.Controls.Navigation;

public partial class NavigationView
{
    public static readonly DependencyProperty HeaderContentProperty = 
        DependencyProperty.RegisterAttached(
            "HeaderContent",
            typeof(object),
            typeof(FrameworkElement),
            new FrameworkPropertyMetadata(null)
        );

    public static object? GetHeaderContent(FrameworkElement target) => target.GetValue(HeaderContentProperty);
    public static void SetHeaderContent(FrameworkElement target, object headerContent) => target.SetValue(HeaderContentProperty, headerContent);
}
