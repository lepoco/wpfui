using System.Collections.ObjectModel;
using System.Windows;
using Wpf.Ui.Controls;

namespace Wpf.Ui.Gallery.Views.Pages.Navigation;

public partial class BreadcrumbBarPage
{
    public BreadcrumbBarPage()
    {
        InitializeComponent();
        DataContext = this;

        _folders = new Folder[]
        {
            new("Home"),
            new("Folder1"),
            new("Folder2"),
            new("Folder3"),
        };

        Strings = new[]
        {
            "Home",
            "Document",
            "Design",
            "Northwind",
            "Images",
            "Folder1",
            "Folder2",
            "Folder3"
        };

        Folders = new ObservableCollection<Folder>(_folders);

        Loaded += OnLoaded;
        Unloaded += OnUnloaded;
    }

    private readonly Folder[] _folders;

    public string[] Strings { get; }
    public ObservableCollection<Folder> Folders { get; }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        BreadcrumbBar2.ItemClicked += BreadcrumbBar2OnItemClicked;
    }

    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
        Loaded -= OnLoaded;
        Unloaded -= OnUnloaded;

        BreadcrumbBar2.ItemClicked -= BreadcrumbBar2OnItemClicked;
    }

    private void BreadcrumbBar2OnItemClicked(object sender, RoutedEventArgs e)
    {
        var args = (BreadcrumbBarItemClickedEventArgs)e;

        Folders.RemoveAt(args.Index + 1);
    }

    private void ResetButton_OnClick(object sender, RoutedEventArgs e)
    {
        Folders.Clear();

        foreach (var folder in _folders)
        {
            Folders.Add(folder);
        }
    }
}

public record Folder(string Name);
