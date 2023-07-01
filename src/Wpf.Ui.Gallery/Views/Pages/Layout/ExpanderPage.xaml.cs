using Wpf.Ui.Controls;
using Wpf.Ui.Gallery.ControlsLookup;
using Wpf.Ui.Gallery.ViewModels.Pages.Layout;

namespace Wpf.Ui.Gallery.Views.Pages.Layout;

[GalleryPage("Expander control.", SymbolRegular.Code24)]
public partial class ExpanderPage : INavigableView<ExpanderViewModel>
{
    public ExpanderPage(ExpanderViewModel viewModel)
    {
        InitializeComponent();
        ViewModel = viewModel;
    }

    public ExpanderViewModel ViewModel { get; }
}
