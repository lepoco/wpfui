// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Demo.Models.Icons;

namespace Wpf.Ui.Demo.ViewModels;

public partial class IconsViewModel : Wpf.Ui.Mvvm.ViewModelBase, INavigationAware
{
    private bool _dataInitialized = false;

    [ObservableProperty] private List<DisplayableIcon> _iconsCollection;

    [ObservableProperty] private IEnumerable<DisplayableIcon> _filteredIconsCollection;

    [ObservableProperty] private IEnumerable<string> _iconNames;

    [ObservableProperty] private Wpf.Ui.Common.SymbolRegular _selectedSymbol;

    [ObservableProperty] private string _selectedSymbolName;

    [ObservableProperty] private string _selectedSymbolCharacter;

    [ObservableProperty] private string _codeBlock;

    [ObservableProperty] private string _searchText;

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

    protected override void OnViewCommand(object parameter = null)
    {
        if (parameter is int)
            UpdateSymbolData((int)parameter);
    }

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);

        if (e.PropertyName == nameof(SearchText))
            UpdateSearchResults(SearchText);
    }

    public void OnNavigatedTo()
    {
        if (!_dataInitialized)
            InitializeData();
    }

    public void OnNavigatedFrom()
    {
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
