using System.Windows;

namespace $safeprojectname$.Views;

/// <summary>
/// Interaction logic for Container.xaml
/// </summary>
public partial class Container : Window
{
    public Container()
    {
        InitializeComponent();

        Wpf.Ui.Appearance.Background.Apply(
            this,
            Wpf.Ui.Appearance.BackgroundType.Mica);

        Wpf.Ui.Appearance.Accent.Apply(
            Wpf.Ui.Appearance.Accent.GetColorizationColor(),
            Wpf.Ui.Appearance.ThemeType.Dark);
    }
}
