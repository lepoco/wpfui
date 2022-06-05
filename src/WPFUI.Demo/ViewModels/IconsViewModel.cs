// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPFUI.Common.Interfaces;
using WPFUI.Demo.Models.Icons;

namespace WPFUI.Demo.ViewModels;

public class IconsViewModel : WPFUI.Mvvm.ViewModelBase, INavigationAware
{
    private bool _dataInitialized = false;

    public List<DisplayableIcon> IconsCollection
    {
        get => GetValue<List<DisplayableIcon>>();
        set => SetValue(value);
    }

    public IEnumerable<DisplayableIcon> FilteredIconsCollection
    {
        get => GetValue<IEnumerable<DisplayableIcon>>();
        set => SetValue(value);
    }

    public IEnumerable<string> IconNames
    {
        get => GetValue<IEnumerable<string>>();
        set => SetValue(value);
    }

    public WPFUI.Common.SymbolRegular SelectedSymbol
    {
        get => GetStructOrDefault(WPFUI.Common.SymbolRegular.Empty);
        set => SetValue(value);
    }

    public string SelectedSymbolName
    {
        get => GetValueOrDefault(String.Empty);
        set => SetValue(value);
    }

    public string SelectedSymbolCharacter
    {
        get => GetValueOrDefault(String.Empty);
        set => SetValue(value);
    }

    public string CodeBlock
    {
        get => GetValueOrDefault(String.Empty);
        set => SetValue(value);
    }

    public string SearchText
    {
        get => GetValueOrDefault(String.Empty);
        set => SetValue(value);
    }

    private void UpdateSymbolData(int symbolId)
    {
        if (IconsCollection.Count - 1 < symbolId)
            return;

        SelectedSymbol = IconsCollection[symbolId].Icon;
        SelectedSymbolCharacter = "\\u" + IconsCollection[symbolId].Code;
        SelectedSymbolName = IconsCollection[symbolId].Name;
        CodeBlock = "<wpfui:SymbolIcon Symbol=\"" + IconsCollection[symbolId].Name + "\"/>";
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

    protected override void OnPropertyChanged(string propertyName)
    {
        base.OnPropertyChanged(propertyName);

        if (propertyName == nameof(SearchText))
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
