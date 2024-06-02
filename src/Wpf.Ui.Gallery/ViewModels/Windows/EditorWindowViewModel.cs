// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

namespace Wpf.Ui.Gallery.ViewModels.Windows;

public partial class EditorWindowViewModel : ViewModel
{
    [ObservableProperty]
    private bool _isWordWrapEnbaled = false;

    [ObservableProperty]
    private bool _isStatusBarVisible = true;

    [ObservableProperty]
    private int _progress = 70;

    [ObservableProperty]
    private string _currentlyOpenedFile = string.Empty;

    [ObservableProperty]
    private Visibility _statusBarVisibility = Visibility.Visible;

    [RelayCommand]
    public void OnStatusBarAction(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return;
        }
    }
}
