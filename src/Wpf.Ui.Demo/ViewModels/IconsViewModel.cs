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
using Wpf.Ui.Demo.Models.Icons;

namespace Wpf.Ui.Demo.ViewModels;

public partial class IconsViewModel : ObservableObject, INavigationAware
{
    private bool _dataInitialized = false;

    [ObservableProperty]
    private List<DisplayableIcon> _iconsCollection = new();

    [ObservableProperty]
    private IEnumerable<DisplayableIcon> _filteredIconsCollection = new DisplayableIcon[] { };

    [ObservableProperty]
    private IEnumerable<string> _iconNames = new string[] { };

    [ObservableProperty]
    private Wpf.Ui.Common.SymbolRegular _selectedSymbol = Common.SymbolRegular.Empty;

    [ObservableProperty]
    private string _selectedSymbolName = String.Empty;

    [ObservableProperty]
    private string _selectedSymbolCharacter = String.Empty;

    [ObservableProperty]
    private string _codeBlock = String.Empty;

    
    private string _searchText = String.Empty;

    public string SearchText
    {
        get => _searchText;
        set
        {
            SetProperty(ref _searchText, value);
            UpdateSearchResults(SearchText);
        }
    }

    public void OnNavigatedTo()
    {
        if (!_dataInitialized)
            InitializeData();
    }

    public void OnNavigatedFrom()
    {
    }

    [RelayCommand]
    private void OnIconSelected(int iconId)
    {
        UpdateSymbolData(iconId);
    }

    private void UpdateSymbolData(int symbolId)
    {
        if (IconsCollection.Count - 1 < symbolId)
            return;

        SelectedSymbol = IconsCollection[symbolId].Icon;
        SelectedSymbolCharacter = "\\u" + IconsCollection[symbolId].Code;
        SelectedSymbolName = IconsCollection[symbolId].Name;
        CodeBlock = "<ui:SymbolIcon Symbol=\"" + IconsCollection[symbolId].Name + "\"/>";
    }

    private void UpdateSearchResults(string searchText)
    {
        Task.Run(() =>
        {
            if (String.IsNullOrEmpty(searchText))
            {
                FilteredIconsCollection = IconsCollection;

                return true;
            }

            var formattedText = searchText.ToLower().Trim();

            FilteredIconsCollection = IconsCollection
                .Where(icon => icon.Name.ToLower().Contains(formattedText)).ToArray();

            return true;
        });
    }

    private void InitializeData()
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
        });

        _dataInitialized = true;
    }
}
