using System.Windows;
using System.Windows.Controls;

namespace Wpf.Ui.Gallery.Views.Pages.Samples;

public partial class MultilevelNavigationSamplePage3 : Page
{
    private readonly INavigationService _navigationService;

    public MultilevelNavigationSamplePage3(INavigationService navigationService)
    {
        _navigationService = navigationService;
        InitializeComponent();
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        _navigationService.NavigateWithHierarchy(typeof(MultilevelNavigationSamplePage1));
    }
}
