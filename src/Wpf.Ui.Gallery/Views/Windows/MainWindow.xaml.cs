using System;
using System.Windows;
using Wpf.Ui.Controls.Navigation;
using Wpf.Ui.Gallery.Services.Contracts;
using Wpf.Ui.Gallery.ViewModels.Windows;
using Wpf.Ui.Gallery.Views.Pages;
using Wpf.Ui.Mvvm.Contracts;

namespace Wpf.Ui.Gallery.Views.Windows;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : IWindow
{
    public MainWindowViewModel ViewModel { get; }

    public MainWindow(MainWindowViewModel viewModel, INavigationService navigationService,
        IServiceProvider serviceProvider, ISnackbarService snackbarService,
        IDialogService dialogService)
    {
        Appearance.Watcher.Watch(this);

        ViewModel = viewModel;
        DataContext = this;

        InitializeComponent();

        snackbarService.SetSnackbarControl(RootSnackbar);
        dialogService.SetDialogControl(RootDialog);
        navigationService.SetNavigationControl(NavigationView);

        NavigationView.SetServiceProvider(serviceProvider);
        NavigationView.Loaded += (_, _) => NavigationView.Navigate(typeof(DashboardPage));
    }

    private void OnNavigationSelectionChanged(object sender, RoutedEventArgs e)
    {
        if (sender is not Wpf.Ui.Controls.Navigation.NavigationView navigationView)
            return;

        NavigationView.HeaderVisibility = navigationView.SelectedItem?.TargetPageType != typeof(DashboardPage)
            ? Visibility.Visible
            : Visibility.Collapsed;
    }
}

