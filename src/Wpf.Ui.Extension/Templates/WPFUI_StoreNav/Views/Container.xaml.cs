using System.Windows;

namespace $safeprojectname$.Views
{
    /// <summary>
    /// Interaction logic for Container.xaml
    /// </summary>
    public partial class Container : Wpf.Ui.Controls.UiWindow
    {
        public ViewModels.ContainerViewModel ViewModel
        {
            get;
        }

        public Container()
        {
            ViewModel = new ViewModels.ContainerViewModel();
            DataContext = this;

            InitializeComponent();
        }
    }
}