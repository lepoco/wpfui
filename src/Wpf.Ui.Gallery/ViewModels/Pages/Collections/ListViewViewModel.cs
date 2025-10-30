// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows.Controls;
using Wpf.Ui.Gallery.Models;

namespace Wpf.Ui.Gallery.ViewModels.Pages.Collections;

public partial class ListViewViewModel : ViewModel
{
    private int _listViewSelectionModeComboBoxSelectedIndex = 0;

    public int ListViewSelectionModeComboBoxSelectedIndex
    {
        get => _listViewSelectionModeComboBoxSelectedIndex;
        set
        {
            _ = SetProperty(ref _listViewSelectionModeComboBoxSelectedIndex, value);
            UpdateListViewSelectionMode(value);
        }
    }

    [ObservableProperty]
    private SelectionMode _listViewSelectionMode = SelectionMode.Single;

    [ObservableProperty]
    private ObservableCollection<Person> _basicListViewItems = GeneratePersons();

    [ObservableProperty]
    private ObservableCollection<string> _bigArray = GenerateBigArray();

    private static ObservableCollection<string> GenerateBigArray()
    {
        var result = new ObservableCollection<string>();
        for (int i = 0; i < 500; i++)
        {
            result.Add($"Item {i + 1}");
        }
        return result;
    }

    private static ObservableCollection<Person> GeneratePersons()
    {
        var random = new Random();
        var persons = new ObservableCollection<Person>();

        var names = new[]
        {
            "John",
            "Winston",
            "Adrianna",
            "Spencer",
            "Phoebe",
            "Lucas",
            "Carl",
            "Marissa",
            "Brandon",
            "Antoine",
            "Arielle",
            "Arielle",
            "Jamie",
            "Alexzander",
        };
        var surnames = new[]
        {
            "Doe",
            "Tapia",
            "Cisneros",
            "Lynch",
            "Munoz",
            "Marsh",
            "Hudson",
            "Bartlett",
            "Gregory",
            "Banks",
            "Hood",
            "Fry",
            "Carroll",
        };
        var companies = new[]
        {
            "Pineapple Inc.",
            "Macrosoft Redmond",
            "Amazing Basics Ltd",
            "Megabyte Computers Inc",
            "Roude Mics",
            "XD Projekt Red S.A.",
            "Lepo.co",
        };

        for (int i = 0; i < 50; i++)
        {
            persons.Add(
                new Person(
                    names[random.Next(0, names.Length)],
                    surnames[random.Next(0, surnames.Length)],
                    companies[random.Next(0, companies.Length)]
                )
            );
        }

        return persons;
    }

    private void UpdateListViewSelectionMode(int selectionModeIndex)
    {
        ListViewSelectionMode = selectionModeIndex switch
        {
            1 => SelectionMode.Multiple,
            2 => SelectionMode.Extended,
            _ => SelectionMode.Single,
        };
    }
}
