// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Collections.Generic;
using System.Windows;
using WPFUI.Common;

namespace WPFUI.Demo.Views.Pages;

public class InputViewData : ViewData
{
    private IEnumerable<string> _autoSuggestCollection = new string[] { };
    public IEnumerable<string> AutoSuggestCollection
    {
        get => _autoSuggestCollection;
        set => UpdateProperty(ref _autoSuggestCollection, value, nameof(AutoSuggestCollection));
    }

    private IEnumerable<string> _comboCollection = new string[] { };
    public IEnumerable<string> ComboCollection
    {
        get => _comboCollection;
        set => UpdateProperty(ref _comboCollection, value, nameof(ComboCollection));
    }
}

/// <summary>
/// Interaction logic for Input.xaml
/// </summary>
public partial class Input
{
    private InputViewData _data;
    public Input()
    {
        InitializeComponent();
        InitializeData();

        Loaded += OnLoaded;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        RootPanel.ScrollOwner = ScrollHost;
    }

    private void InitializeData()
    {
        _data = new InputViewData();
        _data.AutoSuggestCollection = new[]
        {
            "Blossoms",
            "Bloodmoss",
            "Blowbill",
            "Bryonia",
            "Buckthorn",
            "Celandine",
            "Cortinarius",
            "Crow's Eye",
            "Fools Parsley Leaves",
            "Ginatia Petals",
            "Han",
            "Hellebore Petals",
            "Honeysuckle",
            "Hop Umbels",
            "Hornwart",
            "Longrube",
            "Mandrake Root",
            "Moleyarrow",
            "Nostrix",
            "Pigskin Puffball",
            "Pringrape",
            "Ranogrin",
            "Ribleaf",
            "Sewant Mushrooms",
            "Verbena",
            "White Myrtle",
            "Wolfsbane"
        };
        _data.ComboCollection = new[]
        {
            "Blossoms",
            "Bloodmoss",
            "Blowbill",
            "Bryonia",
            "Buckthorn",
            "Celandine",
            "Cortinarius",
            "Crow's Eye",
            "Fools Parsley Leaves",
            "Ginatia Petals",
            "Han",
        };

        DataContext = _data;
    }
}
