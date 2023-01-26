using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Gallery.Views.Pages.Samples;

namespace Wpf.Ui.Gallery.Views.Pages.Navigation;

public partial class MultilevelNavigationPage : Page
{
    public MultilevelNavigationPage(INavigationService navigationService)
    {
        _navigationService = navigationService;
        InitializeComponent();
    }

    private readonly INavigationService _navigationService;

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        _navigationService.NavigateWithHierarchy(typeof(MultilevelNavigationSamplePage1));
    }
}
