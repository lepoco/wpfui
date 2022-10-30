// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Wpf.Ui.Gallery.Models;

namespace Wpf.Ui.Gallery.ViewModels.Pages.Collections;

public partial class ListViewViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<Person> _basicListViewItems;

    public ListViewViewModel()
    {
        _basicListViewItems = GeneratePersons();
    }

    private ObservableCollection<Person> GeneratePersons()
    {
        var random = new Random();
        var persons = new ObservableCollection<Person>();

        var names = new[] { "John", "Winston", "Adrianna", "Spencer", "Phoebe", "Lucas", "Carl", "Marissa", "Brandon", "Antoine", "Arielle", "Arielle", "Jamie", "Alexzander" };
        var surnames = new[] { "Doe", "Tapia", "Cisneros", "Lynch", "Munoz", "Marsh", "Hudson", "Bartlett", "Gregory", "Banks", "Hood", "Fry", "Carroll" };

        for (int i = 0; i < 50; i++)
            persons.Add(new Person(
                names[random.Next(0, names.Length)],
                surnames[random.Next(0, surnames.Length)]));

        return persons;
    }
}
