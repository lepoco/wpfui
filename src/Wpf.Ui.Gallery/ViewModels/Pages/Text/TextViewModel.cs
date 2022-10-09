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
                Icon = SymbolRegular.TextBulletListSquare24,
                Description = "Control with suggestions.",
                Link = "AutoSuggestBox"
            },
            new()
            {
                Name = "NumberBox",
                Icon = SymbolRegular.NumberSymbol24,
                Description = "Control for numeric input.",
                Link = "NumberBox"
            },
            new()
            {
                Name = "PasswordBox",
                Icon = SymbolRegular.Password24,
                Description = "A control for entering passwords.",
                Link = "PasswordBox"
            },
            new()
            {
                Name = "RichTextBox",
                Icon = SymbolRegular.DrawText24,
                Description = "A rich editing control.",
                Link = "RichTextBox"
            },
            new()
            {
                Name = "Label",
                Icon = SymbolRegular.TextBaseline20,
                Description = "Caption of an item.",
                Link = "Label"
            },
            new()
            {
                Name = "TextBlock",
                Icon = SymbolRegular.TextCaseLowercase24,
                Description = "Control for displaying text.",
                Link = "TextBlock"
            },
            new()
            {
                Name = "TextBox",
                Icon = SymbolRegular.TextColor24,
                Description = "Plain text field.",
                Link = "TextBox"
            }
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
