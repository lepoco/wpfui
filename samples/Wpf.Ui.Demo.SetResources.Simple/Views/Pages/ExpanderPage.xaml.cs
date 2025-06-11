// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Data;
using Wpf.Ui.Demo.SetResources.Simple.Models;

namespace Wpf.Ui.Demo.SetResources.Simple.Views.Pages;

public partial class ExpanderPage
{
    public ObservableCollection<DataGroup> GroupCollection { get; private set; } = [];

    public ExpanderPage()
    {
        App.ApplyTheme(this);

        InitializeData();
        InitializeComponent();

        ICollectionView collectionView = CollectionViewSource.GetDefaultView(GroupCollection);
        collectionView.GroupDescriptions.Add(new PropertyGroupDescription(nameof(DataGroup.GroupName)));

        Group.ItemsSource = collectionView;
    }

    private void InitializeData()
    {
        GroupCollection.Add(new DataGroup(false, "Audi", "Auto"));
        GroupCollection.Add(new DataGroup(false, "Samsung S24 Ultra", "Phone"));
        GroupCollection.Add(new DataGroup(true, "Apple iPhone 16 Pro", "Phone"));
        GroupCollection.Add(new DataGroup(true, "Bugatti", "Auto"));
        GroupCollection.Add(new DataGroup(false, "Lamborghini", "Auto"));
    }
}