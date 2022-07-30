using Wpf.Ui.Common.Interfaces;

namespace $safeprojectname$.Views.Pages
{
    /// <summary>
    /// Interaction logic for DataView.xaml
    /// </summary>
    public partial class DataPage : INavigableView<ViewModels.DataViewModel>
    {
        public ViewModels.DataViewModel ViewModel
        {
            get;
        }

        public DataPage(ViewModels.DataViewModel viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent();
        }
    }
}
