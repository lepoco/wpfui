// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Gallery.Models;

namespace Wpf.Ui.Gallery.ViewModels.Pages.Icons;

public partial class IconsViewModel : ObservableObject, INavigationAware
{
    private bool _isInitialized = false;

    private string _autoSuggestBoxText = String.Empty;

    [ObservableProperty]
    private ICollection<DisplayableIcon> _iconsCollection = new List<DisplayableIcon>();

    [ObservableProperty]
    private ICollection<DisplayableIcon> _filteredIconsCollection = new DisplayableIcon[] { };

    [ObservableProperty]
    private ICollection<string> _iconNames = new string[] { };

    public string AutoSuggestBoxText
    {
        get => _autoSuggestBoxText;
        set
        {
            SetProperty<string>(ref _autoSuggestBoxText, value);
            UpdateSearchResults(value);
        }
    }

    public void OnNavigatedTo()
    {
        if (!_isInitialized)
            InitializeViewModel();
    }

    public void OnNavigatedFrom()
    { }

    [RelayCommand]
    public void OnIconSelected(int parameter)
    {
        UpdateSymbolData(parameter);
    }

    private void InitializeViewModel()
    {
        Task.Run(() =>
        {
            var id = 0;
            var names = Enum.GetNames(typeof(Common.SymbolRegular));
            var icons = new List<DisplayableIcon>();

            names = names.OrderBy(n => n).ToArray();

            foreach (string iconName in names)
            {
                var icon = Common.Glyph.Parse(iconName);

                icons.Add(new DisplayableIcon
                {
                    Id = id++,
                    Name = iconName,
                    Icon = icon,
                    Symbol = ((char)icon).ToString(),
                    Code = ((int)icon).ToString("X4")
                });
            }

            IconsCollection = icons;
            FilteredIconsCollection = icons;
            IconNames = icons.Select(icon => icon.Name).ToArray();

            if (icons.Count > 4)
                UpdateSymbolData(4);

            _isInitialized = true;
        });
    }

    private void UpdateSymbolData(int symbolId)
    {
        if (IconsCollection.Count - 1 < symbolId)
            return;

        //SelectedSymbol = IconsCollection[symbolId].Icon;
        //SelectedSymbolCharacter = "\\u" + IconsCollection[symbolId].Code;
        //SelectedSymbolName = IconsCollection[symbolId].Name;
        //CodeBlock = "<ui:SymbolIcon Symbol=\"" + IconsCollection[symbolId].Name + "\"/>";
    }

    private void UpdateSearchResults(string searchedText)
    {
        Task.Run(() =>
        {
            if (String.IsNullOrEmpty(searchedText))
            {
                FilteredIconsCollection = IconsCollection;

                return true;
            }

            var formattedText = searchedText.ToLower().Trim();

            FilteredIconsCollection = IconsCollection
                .Where(icon => icon.Name.ToLower().Contains(formattedText)).ToArray();

            return true;
        });
    }
}
