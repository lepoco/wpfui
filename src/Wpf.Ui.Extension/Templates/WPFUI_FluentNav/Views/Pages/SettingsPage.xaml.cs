using System.Windows.Controls;

namespace $safeprojectname$.Views.Pages
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Wpf.Ui.Controls.UiPage
    {
        public ViewModels.SettingsViewModel ViewModel
        {
            get;
        }

        public SettingsPage()
        {
            ViewModel = new ViewModels.SettingsViewModel();
            DataContext = this;

            InitializeComponent();
        }
    }
}