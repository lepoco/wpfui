using System.Windows;
using Wpf.Ui.Controls;
using Wpf.Ui.Gallery.Views.Windows;

namespace Wpf.Ui.Gallery.ViewModels.Pages.DialogsAndFlyouts;

public partial class ContentDialogViewModel : ObservableObject
{
    [ObservableProperty]
    private string _dialogResultText = string.Empty;

    [RelayCommand]
    private async void OnShowDialog(object content)
    { 
        var window = (MainWindow)Application.Current.MainWindow!;
        using var dialog = new ContentDialog(window.RootDialog)
        {
            Title = "Save your work?",
            Content = content,
            PrimaryButtonText = "Save",
            SecondaryButtonText = "Don't Save",
            CloseButtonText = "Cancel",
            DefaultButton = ContentDialogButton.Primary,
        };

        var result = await dialog.ShowAsync();

        DialogResultText = result switch
        {
            ContentDialogResult.Primary => "User saved their work",
            ContentDialogResult.Secondary => "User did not save their work",
            _ => "User cancelled the dialog"
        };
    }
}
