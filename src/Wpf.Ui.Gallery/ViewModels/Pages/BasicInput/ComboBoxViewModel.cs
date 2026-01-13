// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

namespace Wpf.Ui.Gallery.ViewModels.Pages.BasicInput;

public partial class ComboBoxViewModel : ViewModel
{
    [ObservableProperty]
    private ObservableCollection<string> _comboBoxFontFamilies =
    [
        "Arial",
        "Comic Sans MS",
        "Segoe UI",
        "Times New Roman",
    ];

    [ObservableProperty]
    private ObservableCollection<int> _comboBoxFontSizes =
    [
        8,
        9,
        10,
        11,
        12,
        14,
        16,
        18,
        20,
        24,
        28,
        36,
        48,
        72,
    ];

    [ObservableProperty]
    private ObservableCollection<GroupedComboBoxItem> _groupedItems =
    [
        new("Fruits", "Apple"),
        new("Fruits", "Banana"),
        new("Fruits", "Orange"),
        new("Fruits", "Mango"),
        new("Fruits", "Pineapple"),
        new("Fruits", "Strawberry"),
        new("Fruits", "Grapes"),
        new("Fruits", "Watermelon"),
        new("Vegetables", "Carrot"),
        new("Vegetables", "Broccoli"),
        new("Vegetables", "Spinach"),
        new("Vegetables", "Tomato"),
        new("Vegetables", "Cucumber"),
        new("Vegetables", "Lettuce"),
        new("Vegetables", "Pepper"),
        new("Vegetables", "Onion"),
        new("Dairy", "Milk"),
        new("Dairy", "Cheese"),
        new("Dairy", "Yogurt"),
        new("Dairy", "Butter"),
        new("Dairy", "Cream"),
        new("Dairy", "Ice Cream"),
        new("Meat", "Chicken"),
        new("Meat", "Beef"),
        new("Meat", "Pork"),
        new("Meat", "Fish"),
        new("Meat", "Turkey"),
        new("Meat", "Lamb"),
        new("Grains", "Rice"),
        new("Grains", "Bread"),
        new("Grains", "Pasta"),
        new("Grains", "Oats"),
        new("Grains", "Quinoa"),
    ];
}

public record GroupedComboBoxItem(string Category, string Name);
