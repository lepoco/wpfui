// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Microsoft.Win32;

namespace Wpf.Ui.Gallery.ViewModels.Pages.OpSystem;

public partial class FilePickerViewModel : ObservableObject
{
    [ObservableProperty]
    private Visibility _openedFilePathVisibility = Visibility.Collapsed;

    [ObservableProperty]
    private string _openedFilePath = String.Empty;

    [ObservableProperty]
    private Visibility _openedPicturePathVisibility = Visibility.Collapsed;

    [ObservableProperty]
    private string _openedPicturePath = String.Empty;

    [ObservableProperty]
    private Visibility _openedMultiplePathVisibility = Visibility.Collapsed;

    [ObservableProperty]
    private string _openedMultiplePath = String.Empty;

    [ObservableProperty]
    private Visibility _openedFolderPathVisibility = Visibility.Collapsed;

    [ObservableProperty]
    private string _openedFolderPath = String.Empty;

    [RelayCommand]
    public void OnOpenFile()
    {
        OpenedFilePathVisibility = Visibility.Collapsed;

        OpenFileDialog openFileDialog = new()
        {
            InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            Filter = "All files (*.*)|*.*"
        };

        if (openFileDialog.ShowDialog() != true)
        {
            return;
        }

        if (!File.Exists(openFileDialog.FileName))
        {
            return;
        }

        OpenedFilePath = openFileDialog.FileName;
        OpenedFilePathVisibility = Visibility.Visible;
    }

    [RelayCommand]
    public void OnOpenPicture()
    {
        OpenedPicturePathVisibility = Visibility.Collapsed;

        OpenFileDialog openFileDialog = new()
        {
            InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
            Filter = "Image files (*.bmp;*.jpg;*.jpeg;*.png)|*.bmp;*.jpg;*.jpeg;*.png|All files (*.*)|*.*"
        };

        if (openFileDialog.ShowDialog() != true)
        {
            return;
        }

        if (!File.Exists(openFileDialog.FileName))
        {
            return;
        }

        OpenedPicturePath = openFileDialog.FileName;
        OpenedPicturePathVisibility = Visibility.Visible;
    }

    [RelayCommand]
    public void OnOpenMultiple()
    {
        OpenedMultiplePathVisibility = Visibility.Collapsed;

        OpenFileDialog openFileDialog = new()
        {
            Multiselect = true,
            InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            Filter = "All files (*.*)|*.*"
        };

        if (openFileDialog.ShowDialog() != true)
        {
            return;
        }

        if (openFileDialog.FileNames.Length == 0)
        {
            return;
        }

        var fileNames = openFileDialog.FileNames;

        OpenedMultiplePath = String.Join("\n", fileNames);
        OpenedMultiplePathVisibility = Visibility.Visible;
    }

    [RelayCommand]
    public void OnOpenFolder()
    {
#if NET8_0_OR_GREATER
        OpenedFolderPathVisibility = Visibility.Collapsed;

        OpenFolderDialog openFolderDialog = new OpenFolderDialog()
        { 
            Multiselect = true,
            InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        };

        if (openFolderDialog.ShowDialog() != true)
        {
            return;
        }

        if (openFolderDialog.FileNames.Length == 0)
        {
            return;
        }

        OpenedFolderPath = String.Join("\n", openFolderDialog.FileNames);
        OpenedFolderPathVisibility = Visibility.Visible;
#else
        OpenedFolderPath = "OpenFolderDialog requires .NET 8 or newer";
        OpenedFolderPathVisibility = Visibility.Visible;
#endif
    }
}
