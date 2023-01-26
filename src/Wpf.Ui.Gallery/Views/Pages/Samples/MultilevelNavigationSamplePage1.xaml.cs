using System.Windows;
using System.Windows.Controls;

namespace Wpf.Ui.Gallery.Views.Pages.Samples;

public partial class MultilevelNavigationSamplePage1 : Page
{
    public MultilevelNavigationSamplePage1(INavigationService navigationService)
    {
        _navigationService = navigationService;
        InitializeComponent();
    }

    private readonly INavigationService _navigationService;

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        _navigationService.NavigateWithHierarchy(typeof(MultilevelNavigationSamplePage2));
    }
}
