// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;
using Wpf.Ui.Gallery.Models;
using Wpf.Ui.Gallery.Views.Windows;

namespace Wpf.Ui.Gallery.ViewModels.Pages.Windows;

public partial class WindowsViewModel : ObservableObject
{
    [ObservableProperty]
    private IEnumerable<WindowCard> _windowCards = new WindowCard[]
    {
        new("Editor", "Sample text editor with tabbed background.", SymbolRegular.ScanText24, "editor")
    };

    [RelayCommand]
    public void OnOpenWindow(string value)
    {
        if (String.IsNullOrEmpty(value))
            return;

        switch (value)
        {
            case "editor":
                Show<EditorWindow>();
                break;
        }
    }

    private void Show<T>() where T : new()
    {
        if (!typeof(Window).IsAssignableFrom(typeof(T)))
            throw new InvalidOperationException($"The window class should be derived from {typeof(Window)}.");

        var windowInstance = new T() as Window;

        if (windowInstance == null)
            throw new InvalidOperationException("Window is not registered as service.");

        windowInstance.Owner = Application.Current.MainWindow;
        windowInstance.Show();
    }
}
