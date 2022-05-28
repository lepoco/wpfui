// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;
using WPFUI.Common;
using WPFUI.Controls.Interfaces;

namespace WPFUI.Demo.Views.Windows;

public class ExperimentalViewData : ViewData
{
    public INavigation Navigation { get; set; } = (INavigation)null;

    private int _generalId = 0;
    public int GeneralId
    {
        get => _generalId;
        set => UpdateProperty(ref _generalId, value, nameof(GeneralId));
    }
}

/// <summary>
/// Interaction logic for ExperimentalWindow.xaml
/// </summary>
public partial class ExperimentalWindow : WPFUI.Controls.UiWindow
{
    private ExperimentalViewData _viewData;

    public ExperimentalWindow()
    {
        InitializeComponent();

        _viewData = new ExperimentalViewData();
        _viewData.GeneralId = 2;
        _viewData.Navigation = RootNavigation;

        RootNavigation.Loaded += RootNavigationOnLoaded;
    }

    private void RootNavigationOnLoaded(object sender, RoutedEventArgs e)
    {
        RootNavigation.Navigate(0, _viewData);
    }
}
