using System.Windows;

namespace $safeprojectname$.Views;

/// <summary>
/// Interaction logic for Container.xaml
/// </summary>
public partial class Container : Wpf.Ui.Controls.UiWindow
{
    public Container()
    {
        InitializeComponent();

        Wpf.Ui.Appearance.Accent.ApplySystemAccent();
    }
}
