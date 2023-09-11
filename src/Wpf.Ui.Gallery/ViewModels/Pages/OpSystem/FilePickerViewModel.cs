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

    [ObservableProperty]
    private string _fileToSaveName = String.Empty;

    [ObservableProperty]
    private string _fileToSaveContents = String.Empty;

    [ObservableProperty]
    private Visibility _savedFileNoticeVisibility = Visibility.Collapsed;

    [ObservableProperty]
    private string _savedFileNotice = String.Empty;

    [RelayCommand]
    public void OnOpenFile()
    {
        OpenedFilePathVisibility = Visibility.Collapsed;

        OpenFileDialog openFileDialog =
            new()
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

        OpenFileDialog openFileDialog =
            new()
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

        OpenFileDialog openFileDialog =
            new()
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

        OpenFolderDialog openFolderDialog =
            new()
            {
                Multiselect = true,
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            };

        if (openFolderDialog.ShowDialog() != true)
        {
            return;
        }

        if (openFolderDialog.FolderNames.Length == 0)
        {
            return;
        }

        OpenedFolderPath = String.Join("\n", openFolderDialog.FolderNames);
        OpenedFolderPathVisibility = Visibility.Visible;
#else
        OpenedFolderPath = "OpenFolderDialog requires .NET 8 or newer";
        OpenedFolderPathVisibility = Visibility.Visible;
#endif
    }

    [RelayCommand]
    public async Task OnSaveFile(CancellationToken cancellation)
    {
        SavedFileNoticeVisibility = Visibility.Collapsed;

        SaveFileDialog saveFileDialog =
            new()
            {
                Filter = "Text Files (*.txt)|*.txt",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            };

        if (!String.IsNullOrEmpty(FileToSaveName))
        {
            var invalidChars = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());

            saveFileDialog.FileName = String
                .Join("_", FileToSaveName.Split(invalidChars.ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
                .Trim();
        }

        if (saveFileDialog.ShowDialog() != true)
        {
            return;
        }

        if (File.Exists(saveFileDialog.FileName))
        {
            // Protect the user from accidental writes
            return;
        }

        try
        {
            await File.WriteAllTextAsync(saveFileDialog.FileName, FileToSaveContents, cancellation);
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);

            return;
        }

        SavedFileNoticeVisibility = Visibility.Visible;
        SavedFileNotice = $"File {saveFileDialog.FileName} was saved.";
    }
}
