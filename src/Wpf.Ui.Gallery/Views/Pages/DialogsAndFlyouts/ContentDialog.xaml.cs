using System.Windows.Controls;
using Wpf.Ui.Gallery.ViewModels.Pages.DialogsAndFlyouts;

namespace Wpf.Ui.Gallery.Views.Pages.DialogsAndFlyouts;

public partial class ContentDialogPage : Page
{
    public ContentDialogPage()
    {
        InitializeComponent();
        DataContext = new ContentDialogViewModel();
    }
}
