using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Wpf.Ui.Gallery.ViewModels.Pages.Collections;

public partial class ListBoxViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<string> _listBoxItems;

    public ListBoxViewModel()
    {
        _listBoxItems = new ObservableCollection<string>
        {
            "Arial",
            "Comic Sans MS",
            "Courier New",
            "Segoe UI",
            "Times New Roman"
        };
    }
}
