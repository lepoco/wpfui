using Wpf.Ui.Controls.Navigation;
using Wpf.Ui.Gallery.ViewModels.Pages.Samples;

namespace Wpf.Ui.Gallery.Views.Pages.Samples;

public partial class MultilevelNavigationSamplePage1 : INavigableView<MultilevelNavigationSample>
{
    public MultilevelNavigationSamplePage1(MultilevelNavigationSample viewModel)
    {
        ViewModel = viewModel;
        DataContext = viewModel;

        InitializeComponent();
    }

    public MultilevelNavigationSample ViewModel { get; }
}
