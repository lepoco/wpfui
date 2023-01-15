using System.Windows;

namespace Wpf.Ui.Controls;

public class BreadcrumbBarItem : System.Windows.Controls.ContentControl
{
    public static readonly DependencyProperty SymbolIconFontSizeProperty =
        DependencyProperty.Register(nameof(SymbolIconFontSize), typeof(double), typeof(BreadcrumbBarItem),
            new PropertyMetadata(18.0));

    public static readonly DependencyProperty SymbolIconFontWeightProperty =
        DependencyProperty.Register(nameof(SymbolIconFontWeight), typeof(FontWeight), typeof(BreadcrumbBarItem),
            new PropertyMetadata(FontWeights.DemiBold));

    public static readonly DependencyProperty SymbolIconSymbolProperty =
        DependencyProperty.Register(nameof(SymbolIconSymbol), typeof(Common.SymbolRegular), typeof(BreadcrumbBarItem),
            new PropertyMetadata(Common.SymbolRegular.ChevronRight24));

    public static readonly DependencyProperty SymbolIconMarginProperty =
        DependencyProperty.Register(nameof(SymbolIconMargin), typeof(Thickness), typeof(BreadcrumbBarItem),
            new PropertyMetadata(new Thickness(10, 0, 10, 0)));

    public static readonly DependencyProperty IsLastProperty =
        DependencyProperty.Register(nameof(IsLast), typeof(bool), typeof(BreadcrumbBarItem),
            new PropertyMetadata(false));

    public double SymbolIconFontSize
    {
        get => (double)GetValue(SymbolIconFontSizeProperty);
        set => SetValue(SymbolIconFontSizeProperty, value);
    }

    public FontWeight SymbolIconFontWeight
    {
        get => (FontWeight)GetValue(SymbolIconFontWeightProperty);
        set => SetValue(SymbolIconFontWeightProperty, value);
    }

    public Common.SymbolRegular SymbolIconSymbol
    {
        get => (Common.SymbolRegular)GetValue(SymbolIconSymbolProperty);
        set => SetValue(SymbolIconSymbolProperty, value);
    }

    public Thickness SymbolIconMargin
    {
        get => (Thickness)GetValue(SymbolIconMarginProperty);
        set => SetValue(SymbolIconMarginProperty, value);
    }

    public bool IsLast
    {
        get => (bool)GetValue(IsLastProperty);
        set => SetValue(IsLastProperty, value);
    }
}
