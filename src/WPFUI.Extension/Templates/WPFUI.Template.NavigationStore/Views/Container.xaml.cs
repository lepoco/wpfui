using System.Windows;

namespace WPFUI.Template.NavigationStore.Views;

/// <summary>
/// Interaction logic for Container.xaml
/// </summary>
public partial class Container : Window
{
    public Container()
    {
        InitializeComponent();

        WPFUI.Appearance.Background.Apply(
            this,
            WPFUI.Appearance.BackgroundType.Mica);

        WPFUI.Appearance.Accent.Apply(
            WPFUI.Appearance.Accent.GetColorizationColor(),
            WPFUI.Appearance.ThemeType.Dark);
    }
}
