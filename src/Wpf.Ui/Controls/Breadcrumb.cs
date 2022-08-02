// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Microsoft.Toolkit.Diagnostics;
using Wpf.Ui.Common;
using Wpf.Ui.Controls.Interfaces;

namespace Wpf.Ui.Controls;

/// <summary>
/// Displays the name of the current <see cref="NavigationItem"/> and it's parents that can be navigated using <see cref="INavigation"/>.
/// </summary>
public class Breadcrumb : System.Windows.Controls.Control
{
    /// <summary>
    /// Property for <see cref="Navigation"/>.
    /// </summary>
    public static readonly DependencyProperty NavigationProperty = DependencyProperty.Register(nameof(Navigation),
        typeof(INavigation), typeof(Breadcrumb),
        new PropertyMetadata(null));

    public static readonly DependencyProperty BreadcrumbItemsProperty = DependencyProperty.Register(nameof(BreadcrumbItems),
        typeof(ObservableCollection<BreadcrumbItem>), typeof(Breadcrumb), new PropertyMetadata(null));


    /// <summary>
    /// <see cref="INavigation"/> based on which <see cref="Breadcrumb"/> displays the titles.
    /// </summary>
    public INavigation Navigation
    {
        get => GetValue(NavigationProperty) as INavigation;
        set => SetValue(NavigationProperty, value);
    }

    public ObservableCollection<BreadcrumbItem> BreadcrumbItems
    {
        get => (ObservableCollection<BreadcrumbItem>)GetValue(BreadcrumbItemsProperty);
        private set => SetValue(BreadcrumbItemsProperty, value);
    }

    private readonly ICommand _onClickCommand;

    public Breadcrumb()
    {
        BreadcrumbItems = new ObservableCollection<BreadcrumbItem>();
        _onClickCommand = new RelayCommand(OnClick);

        Loaded += (sender, args) =>
        {
            Guard.IsNotNull(Navigation, nameof(Navigation));
            Navigation.Navigated += OnNavigated;
        };

        Unloaded += (sender, args) =>
        {
            Navigation.Navigated -= OnNavigated;
        };
    }

    protected virtual void OnNavigated(INavigation sender, RoutedNavigationEventArgs e)
    {
#if DEBUG
        System.Diagnostics.Debug.WriteLine($"INFO | {typeof(Breadcrumb)} builded, current nav: {Navigation.GetType()}", "Wpf.Ui.Breadcrumb");
#endif

        //TODO This event needs some kind of optimization
        BreadcrumbItems.Clear();
        foreach (var navigationItem in e.NavigationStack)
        {
            BreadcrumbItems.Add(new BreadcrumbItem()
            {
                Text = navigationItem.Content as string ?? string.Empty,
                PageTag = navigationItem.PageTag,
                OnClickCommand = _onClickCommand
            });
        }

        BreadcrumbItems[BreadcrumbItems.Count - 1].IsActive = true;
    }

    private void OnClick(object obj)
    {
        var pageTag = (string)obj;

        Navigation.NavigateTo(pageTag);
    }
}
