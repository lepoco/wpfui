using Wpf.Ui.Controls.Navigation;
using Wpf.Ui.Gallery.ViewModels.Pages.Samples;

namespace Wpf.Ui.Gallery.Views.Pages.Navigation;

public partial class MultilevelNavigationPage : INavigableView<MultilevelNavigationSample>
{
    public MultilevelNavigationPage(MultilevelNavigationSample viewModel)
    {
        ViewModel = viewModel;
        DataContext = viewModel;

        InitializeComponent();
    }

    public MultilevelNavigationSample ViewModel { get; }
}
