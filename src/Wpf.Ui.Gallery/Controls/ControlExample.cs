using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Wpf.Ui.Gallery.Controls;

[ContentProperty(nameof(ExampleContent))]
public class ControlExample : Control
{
    public static readonly DependencyProperty ExampleContentProperty =
        DependencyProperty.Register(nameof(ExampleContent),
            typeof(object), typeof(ControlExample), new PropertyMetadata(null));

    public static readonly DependencyProperty XamlCodeProperty =
        DependencyProperty.Register(nameof(XamlCode),
            typeof(string), typeof(ControlExample), new PropertyMetadata(null));

    public static readonly DependencyProperty CsharpCodeProperty =
        DependencyProperty.Register(nameof(CsharpCode),
            typeof(string), typeof(ControlExample), new PropertyMetadata(null));

    public object? ExampleContent
    {
        get => GetValue(ExampleContentProperty);
        set => SetValue(ExampleContentProperty, value);
    }

    public string? XamlCode
    {
        get => (string)GetValue(XamlCodeProperty);
        set => SetValue(XamlCodeProperty, value);
    }

    public string? CsharpCode
    {
        get => (string)GetValue(CsharpCodeProperty);
        set => SetValue(CsharpCodeProperty, value);
    }
}
