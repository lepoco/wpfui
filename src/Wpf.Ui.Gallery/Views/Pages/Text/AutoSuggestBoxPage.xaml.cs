// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.


using System.Diagnostics;
using Wpf.Ui.Controls;

namespace Wpf.Ui.Gallery.Views.Pages.Text;

public record TestMode(string Text)
{
    public static implicit operator TestMode(string text) => new TestMode(text);
}

public partial class AutoSuggestBoxPage
{
    public AutoSuggestBoxPage()
    {
        InitializeComponent();
        AutoSuggestBox.OriginalItemsSource = _cats;
    }

    private readonly List<TestMode> _cats = new()
    {
        "Abyssinian",
        "Aegean",
        "American Bobtail",
        "American Curl",
        "American Ringtail",
        "American Shorthair",
        "American Wirehair",
        "Aphrodite Giant",
        "Arabian Mau",
        "Asian cat",
        "Asian Semi-longhair",
        "Australian Mist",
        "Balinese",
        "Bambino",
        "Bengal",
        "Birman",
        "Brazilian Shorthair",
        "British Longhair",
        "British Shorthair",
        "Burmese",
        "Burmilla",
        "California Spangled",
        "Chantilly-Tiffany",
        "Chartreux",
        "Chausie",
        "Colorpoint Shorthair",
        "Cornish Rex",
        "Cymric",
        "Cyprus",
        "Devon Rex",
        "Donskoy",
        "Dragon Li",
        "Dwelf",
        "Egyptian Mau",
        "European Shorthair",
        "Exotic Shorthair",
        "Foldex",
        "German Rex",
        "Havana Brown",
        "Highlander",
        "Himalayan",
        "Japanese Bobtail",
        "Javanese",
        "Kanaani",
        "Khao Manee",
        "Kinkalow",
        "Korat",
        "Korean Bobtail",
        "Korn Ja",
        "Kurilian Bobtail",
        "Lambkin",
        "LaPerm",
        "Lykoi",
        "Maine Coon",
        "Manx",
        "Mekong Bobtail",
        "Minskin",
        "Napoleon",
        "Munchkin",
        "Nebelung",
        "Norwegian Forest Cat",
        "Ocicat",
        "Ojos Azules",
        "Oregon Rex",
        "Persian (modern)",
        "Persian (traditional)",
        "Peterbald",
        "Pixie-bob",
        "Ragamuffin",
        "Ragdoll",
        "Raas",
        "Russian Blue",
        "Russian White",
        "Sam Sawet",
        "Savannah",
        "Scottish Fold",
        "Selkirk Rex",
        "Serengeti",
        "Serrade Petit",
        "Siamese",
        "Siberian or´Siberian Forest Cat",
        "Singapura",
        "Snowshoe",
        "Sokoke",
        "Somali",
        "Sphynx",
        "Suphalak",
        "Thai",
        "Thai Lilac",
        "Tonkinese",
        "Toyger",
        "Turkish Angora",
        "Turkish Van",
        "Turkish Vankedisi",
        "Ukrainian Levkoy",
        "Wila Krungthep",
        "York Chocolate"
    };

    private void AutoSuggestBox_OnTextChanged(NewAutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
    {
        if (args.Reason is not AutoSuggestionBoxTextChangeReason.UserInput)
            return;

        var suitableItems = new List<TestMode>();
        var splitText = args.Text.ToLower().Split(" ");

        foreach (var cat in _cats)
        {
            var found = splitText.All(key=> cat.Text.ToLower().Contains(key));

            if(found)
                suitableItems.Add(cat);
        }

        if (suitableItems.Count == 0)
            suitableItems.Add("No results found");

        sender.ItemsSource = suitableItems;
    }

    private void AutoSuggestBox_OnSuggestionChosen(NewAutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
    {
        SuggestionOutput.Text = ((TestMode)args.SelectedItem).Text;
    }
}
