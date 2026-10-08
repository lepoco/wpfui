// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Collections.ObjectModel;

namespace Wpf.Ui.Gallery.ViewModels.Pages.Navigation;

public partial class TabViewViewModel : ViewModel
{
    private int _nextTabNumber = 3;

    public ObservableCollection<TabDocument> Documents { get; } =
    [
        new("Welcome", "This tab cannot be closed.", false),
        new("Document 2", "Drag a tab header to reorder the documents."),
    ];

    [ObservableProperty]
    private TabDocument? _selectedDocument;

    public TabViewViewModel()
    {
        SelectedDocument = Documents[0];
    }

    public TabDocument CreateDocument()
    {
        int number = _nextTabNumber++;
        return new TabDocument($"Document {number}", $"Content of document {number}.");
    }
}
