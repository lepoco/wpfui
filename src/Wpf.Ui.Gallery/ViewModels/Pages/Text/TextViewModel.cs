// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Wpf.Ui.Common;
using Wpf.Ui.Gallery.Models;
using Wpf.Ui.Mvvm.Contracts;

namespace Wpf.Ui.Gallery.ViewModels.Pages.Text;

public partial class TextViewModel : ObservableObject
{
    private readonly INavigationService _navigationService;

    [ObservableProperty]
    private ICollection<NavigationCard> _navigationCards;

    public TextViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;

        NavigationCards = new ObservableCollection<NavigationCard>
        {
            new()
            {
                Name = "AutoSuggestBox",
                Icon = SymbolRegular.Navigation24,
                Description = "AutoSuggestBox",
                Link = "AutoSuggestBox"
            },
            new()
            {
                Name = "NumberBox",
                Icon = SymbolRegular.NavigationUnread24,
                Description = "NumberBox",
                Link = "NumberBox"
            },
            new()
            {
                Name = "PasswordBox",
                Icon = SymbolRegular.Navigation24,
                Description = "PasswordBox",
                Link = "PasswordBox"
            },
            new()
            {
                Name = "RichTextBox",
                Icon = SymbolRegular.NavigationUnread24,
                Description = "RichTextBox",
                Link = "RichTextBox"
            },
            new()
            {
                Name = "Label",
                Icon = SymbolRegular.Navigation24,
                Description = "Label",
                Link = "Label"
            },
            new()
            {
                Name = "TextBlock",
                Icon = SymbolRegular.NavigationUnread24,
                Description = "TextBlock",
                Link = "TextBlock"
            },
            new()
            {
                Name = "TextBox",
                Icon = SymbolRegular.Navigation24,
                Description = "TextBox",
                Link = "TextBox"
            },
        };
    }

    [RelayCommand]
    private void OnNavigatedTo(string parameter)
    {
#if DEBUG
        System.Diagnostics.Debug.WriteLine($"INFO | Navigate to: {parameter}");
#endif
        _navigationService.Navigate(parameter);
    }
}
