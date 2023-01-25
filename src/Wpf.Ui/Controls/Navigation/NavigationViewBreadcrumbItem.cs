namespace Wpf.Ui.Controls.Navigation;

internal class NavigationViewBreadcrumbItem
{
    public NavigationViewBreadcrumbItem(INavigationViewItem  item)
    {
        Content = item.Content;
        PageId = item.Id;
    }

    public object Content { get; }
    public object PageId { get; }
}
