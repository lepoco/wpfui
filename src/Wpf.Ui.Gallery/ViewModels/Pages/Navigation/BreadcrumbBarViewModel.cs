// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Collections.ObjectModel;
using Wpf.Ui.Gallery.Models;

namespace Wpf.Ui.Gallery.ViewModels.Pages.Navigation;

public partial class BreadcrumbBarViewModel : ObservableObject
{
    private readonly IEnumerable<Folder> _baseFoldersCollection = new Folder[]
    {
        new ("Home"),
        new ("Folder1"),
        new ("Folder2"),
        new ("Folder3"),
    };

    [ObservableProperty]
    private ObservableCollection<string> _strings = new ObservableCollection<string>
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

    [ObservableProperty]
    private ObservableCollection<Folder> _folders = new ObservableCollection<Folder> { };

    public BreadcrumbBarViewModel()
    {
        ResetFoldersCollection();
    }

    [RelayCommand]
    private void OnStringSelected(object item)
    {
    }

    [RelayCommand]
    private void OnFolderSelected(object item)
    {
        if (item is not Folder selectedFolder)
            return;

        var index = Folders.IndexOf(selectedFolder);

        Folders.Clear();

        var counter = 0;
        foreach (var folder in _baseFoldersCollection)
        {
            if (counter++ > index)
                break;

            Folders.Add(folder);
        }
    }

    [RelayCommand]
    private void OnResetFolders()
    {
        ResetFoldersCollection();
    }

    private void ResetFoldersCollection()
    {
        Folders.Clear();

        foreach (var folder in _baseFoldersCollection)
            Folders.Add(folder);
    }
}
