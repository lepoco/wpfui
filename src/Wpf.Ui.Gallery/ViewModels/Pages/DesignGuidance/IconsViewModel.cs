// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows.Controls;
using Wpf.Ui.Controls;
using Wpf.Ui.Gallery.Models;

namespace Wpf.Ui.Gallery.ViewModels.Pages.DesignGuidance;

public partial class IconsViewModel : ObservableObject, INavigationAware
{
    private int _selectedIconId = 0;

    private string _autoSuggestBoxText = string.Empty;

    [ObservableProperty]
    private SymbolRegular _selectedSymbol = SymbolRegular.Empty;

    [ObservableProperty]
    private string _selectedSymbolName = string.Empty;

    [ObservableProperty]
    private string _selectedSymbolUnicodePoint = string.Empty;

    [ObservableProperty]
    private string _selectedSymbolTextGlyph = string.Empty;

    [ObservableProperty]
    private string _selectedSymbolXaml = string.Empty;

    [ObservableProperty]
    private bool _isIconFilled = false;

    [ObservableProperty]
    private List<DisplayableIcon> _iconsCollection = [];

    [ObservableProperty]
    private List<DisplayableIcon> _filteredIconsCollection = [];

    [ObservableProperty]
    private List<string> _iconNames = [];

    public string AutoSuggestBoxText
    {
        get => _autoSuggestBoxText;
        set
        {
            _ = SetProperty(ref _autoSuggestBoxText, value);
            UpdateSearchResults(value);
        }
    }

    public IconsViewModel()
    {
        _ = Task.Run(() =>
        {
            var id = 0;
            var names = Enum.GetNames(typeof(SymbolRegular));
            var icons = new List<DisplayableIcon>();

            names = names.OrderBy(n => n).ToArray();

            foreach (string iconName in names)
            {
                SymbolRegular icon = SymbolGlyph.Parse(iconName);

                icons.Add(
                    new DisplayableIcon
                    {
                        Id = id++,
                        Name = iconName,
                        Icon = icon,
                        Symbol = ((char)icon).ToString(),
                        Code = ((int)icon).ToString("X4"),
                    }
                );
            }

            IconsCollection = icons;
            FilteredIconsCollection = icons;
            IconNames = icons.Select(icon => icon.Name).ToList();

            if (icons.Count > 4)
            {
                _selectedIconId = 4;

                UpdateSymbolData();
            }
        });
    }

    public void OnNavigatedTo() { }

    public void OnNavigatedFrom() { }

    [RelayCommand]
    public void OnIconSelected(int parameter)
    {
        _selectedIconId = parameter;

        UpdateSymbolData();
    }

    [RelayCommand]
    public void OnCheckboxChecked(object sender)
    {
        if (sender is not CheckBox checkbox)
        {
            return;
        }

        IsIconFilled = checkbox?.IsChecked ?? false;

        UpdateSymbolData();
    }

    private void UpdateSymbolData()
    {
        if (IconsCollection.Count - 1 < _selectedIconId)
        {
            return;
        }

        DisplayableIcon selectedSymbol = IconsCollection.FirstOrDefault(sym => sym.Id == _selectedIconId);

        SelectedSymbol = selectedSymbol.Icon;
        SelectedSymbolName = selectedSymbol.Name;
        SelectedSymbolUnicodePoint = selectedSymbol.Code;
        SelectedSymbolTextGlyph = $"&#x{selectedSymbol.Code};";
        SelectedSymbolXaml =
            $"<ui:SymbolIcon Symbol=\"{selectedSymbol.Name}\"{(IsIconFilled ? " Filled=\"True\"" : string.Empty)}/>";
    }

    private void UpdateSearchResults(string searchedText)
    {
        _ = Task.Run(() =>
        {
            if (string.IsNullOrEmpty(searchedText))
            {
                FilteredIconsCollection = IconsCollection;

                return true;
            }

            var formattedText = searchedText.ToLower().Trim();

            FilteredIconsCollection = IconsCollection
                .Where(icon => icon.Name.Contains(formattedText, StringComparison.OrdinalIgnoreCase))
                .ToList();

            return true;
        });
    }
}
