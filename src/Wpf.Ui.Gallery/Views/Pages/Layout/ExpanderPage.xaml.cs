using Wpf.Ui.Controls;
using Wpf.Ui.Gallery.ViewModels.Pages.Layout;

namespace Wpf.Ui.Gallery.Views.Pages.Layout
{
    /// <summary>
    /// Interaction logic for Expander.xaml
    /// </summary>
    public partial class ExpanderPage : INavigableView<ExpanderViewModel>
    {
        public ExpanderPage(ExpanderViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
        }

        public ExpanderViewModel ViewModel { get; }
    }
}
