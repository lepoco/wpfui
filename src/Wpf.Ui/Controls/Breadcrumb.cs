// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
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

        if (DesignerProperties.GetIsInDesignMode(this))
            return;

        Loaded += (_, _) =>
        {
            Guard.IsNotNull(Navigation, nameof(Navigation));

            Navigation.Navigated += OnNavigated;
            Navigation.NavigationStack.CollectionChanged += NavigationStackOnCollectionChanged;
        };

        Unloaded += (_, _) =>
        {
            Navigation.Navigated -= OnNavigated;
            Navigation.NavigationStack.CollectionChanged -= NavigationStackOnCollectionChanged;
        };
    }

    protected virtual void OnNavigated(INavigation sender, RoutedNavigationEventArgs e)
    {
#if DEBUG
        System.Diagnostics.Debug.WriteLine($"INFO | {typeof(Breadcrumb)} builded, current nav: {Navigation.GetType()}", "Wpf.Ui.Breadcrumb");
#endif
    }

    private void NavigationStackOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
            {
                foreach (NavigationItem item in e.NewItems!)
                {
                    BreadcrumbItems.Add(BreadcrumbItem.Create(item, _onClickCommand));
                }

                break;
            }
            case NotifyCollectionChangedAction.Remove:
                BreadcrumbItems.RemoveAt(e.OldStartingIndex);
                break;
            case NotifyCollectionChangedAction.Replace:
                var replaceItem = (INavigationItem) e.NewItems![0];
                BreadcrumbItems[0] = BreadcrumbItem.Create(replaceItem, _onClickCommand);
                break;
            default:
                return;
        }

        if (BreadcrumbItems.Count > 1)
            BreadcrumbItems[BreadcrumbItems.Count - 2].IsActive = false;

        BreadcrumbItems[BreadcrumbItems.Count - 1].IsActive = true;
    }

    private void OnClick(object obj)
    {
        var pageTag = (string)obj;

        Navigation.NavigateTo(pageTag);
    }
}
