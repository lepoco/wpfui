using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.Input;
using Wpf.Ui.Mvvm.Contracts;

namespace Wpf.Ui.Demo.ViewModels;

public class BreadcrumbPagesViewModel
{
    public RelayCommand<string> OnClickCommand { get; }
    public ICommand OnNavigateBackCommand { get; }

    private readonly INavigationService _navigationService;

    public BreadcrumbPagesViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;

        OnClickCommand = new RelayCommand<string>(OnClick);
        OnNavigateBackCommand = new RelayCommand(OnNavigateBack);
    }

    private void OnClick(string pageTag)
    {
        _navigationService.NavigateTo($"/{pageTag}", true, this);
    }

    private void OnNavigateBack()
    {
        _navigationService.NavigateTo("..");
    }
}
