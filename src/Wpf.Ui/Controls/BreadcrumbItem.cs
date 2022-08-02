using System.Windows;
using System.Windows.Input;
using Wpf.Ui.Controls.Interfaces;

namespace Wpf.Ui.Controls;

public class BreadcrumbItem : System.Windows.Controls.Control
{
    public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof(Text),
        typeof(string), typeof(BreadcrumbItem), new PropertyMetadata(string.Empty));

    public static readonly DependencyProperty PageTagProperty = DependencyProperty.Register(nameof(PageTag),
        typeof(string), typeof(BreadcrumbItem), new PropertyMetadata(string.Empty));

    public static readonly DependencyProperty OnClickCommandProperty = DependencyProperty.Register(nameof(OnClickCommand),
        typeof(ICommand), typeof(BreadcrumbItem), new PropertyMetadata(null));

    public static readonly DependencyProperty IsActiveProperty = DependencyProperty.Register(nameof(IsActive),
        typeof(bool), typeof(BreadcrumbItem), new PropertyMetadata(false));

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public string PageTag
    {
        get => (string)GetValue(PageTagProperty);
        set => SetValue(PageTagProperty, value);
    }

    public ICommand OnClickCommand
    {
        get => (ICommand)GetValue(OnClickCommandProperty);
        set => SetValue(OnClickCommandProperty, value);
    }

    public bool IsActive
    {
        get => (bool)GetValue(IsActiveProperty);
        set => SetValue(IsActiveProperty, value);
    }

    public static BreadcrumbItem Create(INavigationItem item, ICommand onClickCommand) => new BreadcrumbItem()
    {
        Text = item.Content as string ?? string.Empty,
        PageTag = item.PageTag,
        OnClickCommand = onClickCommand
    };
}
