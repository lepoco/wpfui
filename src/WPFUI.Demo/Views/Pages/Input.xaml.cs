// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Collections.Generic;
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
    }
    private void InitializeData()
    {
        _data = new InputViewData();
        _data.AutoSuggestCollection = new string[]
        {
            "Abcd",
            "Adcde",
            "Bcde",
            "Bcdef"
        };

        DataContext = _data;
    }
}
