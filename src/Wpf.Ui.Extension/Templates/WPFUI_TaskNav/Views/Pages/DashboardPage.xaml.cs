using System.Windows.Controls;

namespace $safeprojectname$.Views.Pages
{
    /// <summary>
    /// Interaction logic for DashboardPage.xaml
    /// </summary>
    public partial class DashboardPage : Wpf.Ui.Controls.UiPage
    {
        public ViewModels.DashboardViewModel ViewModel
        {
            get;
        }

        public DashboardPage()
        {
            ViewModel = new ViewModels.DashboardViewModel();
            DataContext = this;

            InitializeComponent();
        }
    }
}