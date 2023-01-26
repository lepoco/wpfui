namespace Wpf.Ui.Gallery.ViewModels.Pages.Samples;

public partial class MultilevelNavigationSample
{
    public MultilevelNavigationSample(INavigationService navigationService)
    {
        _navigationService = navigationService;
    }

    private readonly INavigationService _navigationService;

    [RelayCommand]
    private void NavigateForward(Type type)
    {
        _navigationService.NavigateWithHierarchy(type);
    }

    [RelayCommand]
    private void NavigateBack()
    {
        _navigationService.GoBack();
    }
}
