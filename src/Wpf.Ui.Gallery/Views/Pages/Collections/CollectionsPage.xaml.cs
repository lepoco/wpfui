// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Wpf.Ui.Controls;
using Wpf.Ui.Gallery.Effects;
using Wpf.Ui.Gallery.ViewModels.Pages.Collections;

namespace Wpf.Ui.Gallery.Views.Pages.Collections;

public partial class CollectionsPage : INavigableView<CollectionsViewModel>
{
    private readonly INavigationService _navigationService;
    private SnowflakeEffect? _snowflake;

    public CollectionsViewModel ViewModel { get; }

    public CollectionsPage(CollectionsViewModel viewModel, INavigationService navigationService)
    {
        ViewModel = viewModel;
        DataContext = this;
        _navigationService = navigationService;

        InitializeComponent();
        Loaded += HandleLoaded;
        Unloaded += HandleUnloaded;
    }

    private void HandleLoaded(object sender, RoutedEventArgs e)
    {
        INavigationView? navigationControl = _navigationService.GetNavigationControl();
        if (navigationControl?.BreadcrumbBar != null &&
            navigationControl.BreadcrumbBar.Visibility != Visibility.Collapsed)
        {
            navigationControl.BreadcrumbBar.SetCurrentValue(VisibilityProperty, Visibility.Collapsed);
        }

        INavigationViewItem? selectedItem = navigationControl?.SelectedItem;
        if (selectedItem != null)
        {
            string? newTitle = selectedItem.Content?.ToString();
            if (MainTitle.Text != newTitle)
            {
                MainTitle.SetCurrentValue(System.Windows.Controls.TextBlock.TextProperty, newTitle);
            }

            if (selectedItem.Icon is SymbolIcon selectedIcon &&
                MainSymbolIcon.Symbol != selectedIcon.Symbol)
            {
                MainSymbolIcon.SetCurrentValue(SymbolIcon.SymbolProperty, selectedIcon.Symbol);
            }
        }

        _snowflake ??= new(MainCanvas);
        _snowflake.Start();
    }

    private void HandleUnloaded(object sender, RoutedEventArgs e)
    {
        INavigationView? navigationControl = _navigationService.GetNavigationControl();
        if (navigationControl?.BreadcrumbBar != null &&
            navigationControl.BreadcrumbBar.Visibility != Visibility.Visible)
        {
            navigationControl.BreadcrumbBar.SetCurrentValue(VisibilityProperty, Visibility.Visible);
        }

        _snowflake?.Stop();
        _snowflake = null;
        Loaded -= HandleLoaded;
        Unloaded -= HandleUnloaded;
    }
}
