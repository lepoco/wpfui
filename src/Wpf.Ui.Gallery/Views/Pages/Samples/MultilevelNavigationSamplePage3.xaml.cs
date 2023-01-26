using Wpf.Ui.Controls.Navigation;
using Wpf.Ui.Gallery.ViewModels.Pages.Samples;

namespace Wpf.Ui.Gallery.Views.Pages.Samples;

public partial class MultilevelNavigationSamplePage3 : INavigableView<MultilevelNavigationSample>
{
    public MultilevelNavigationSamplePage3(MultilevelNavigationSample viewModel)
    {
        ViewModel = viewModel;
        DataContext = viewModel;

        InitializeComponent();
    }

    public MultilevelNavigationSample ViewModel { get; }
}
