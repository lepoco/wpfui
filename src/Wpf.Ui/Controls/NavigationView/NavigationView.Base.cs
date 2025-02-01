// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

/* Based on Windows UI Library https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.controls.navigationview?view=winrt-22621 */

using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Windows.Input;

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

/// <summary>
/// Represents a container that enables navigation of app content. It has a header, a view for the main content, and a menu pane for navigation commands.
/// </summary>
public partial class NavigationView : System.Windows.Controls.Control, INavigationView
{
    /// <summary>
    /// Initializes static members of the <see cref="NavigationView"/> class and overrides default property metadata.
    /// </summary>
    static NavigationView()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(NavigationView),
            new FrameworkPropertyMetadata(typeof(NavigationView))
        );
        MarginProperty.OverrideMetadata(
            typeof(NavigationView),
            new FrameworkPropertyMetadata(new Thickness(0, 0, 0, 0))
        );
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NavigationView"/> class.
    /// </summary>
    public NavigationView()
    {
        NavigationParent = this;

        Loaded += OnLoaded;
        Unloaded += OnUnloaded;
        SizeChanged += OnSizeChanged;

        // Initialize MenuItems collection
        var menuItems = new ObservableCollection<object>();
        menuItems.CollectionChanged += OnMenuItems_CollectionChanged;
        SetValue(MenuItemsPropertyKey, menuItems);

        var footerMenuItems = new ObservableCollection<object>();
        footerMenuItems.CollectionChanged += OnMenuItems_CollectionChanged;
        SetValue(FooterMenuItemsPropertyKey, footerMenuItems);
    }

    /// <inheritdoc/>
    public INavigationViewItem? SelectedItem { get; protected set; }

    protected Dictionary<string, INavigationViewItem> PageIdOrTargetTagNavigationViewsDictionary { get; } =
        [];

    protected Dictionary<Type, INavigationViewItem> PageTypeNavigationViewsDictionary { get; } = [];

    private readonly ObservableCollection<string> _autoSuggestBoxItems = [];
    private readonly ObservableCollection<NavigationViewBreadcrumbItem> _breadcrumbBarItems = [];

    private static readonly Thickness TitleBarPaneOpenMarginDefault = new(35, 0, 0, 0);
    private static readonly Thickness TitleBarPaneCompactMarginDefault = new(55, 0, 0, 0);
    private static readonly Thickness AutoSuggestBoxMarginDefault = new(8, 8, 8, 16);
    private static readonly Thickness FrameMarginDefault = new(0, 50, 0, 0);

    protected static void UpdateVisualState(NavigationView navigationView)
    {
        // Skip display modes that don't have multiple states
        if (
            navigationView.PaneDisplayMode
            is NavigationViewPaneDisplayMode.LeftFluent
                or NavigationViewPaneDisplayMode.Top
                or NavigationViewPaneDisplayMode.Bottom
        )
        {
            return;
        }

        _ = VisualStateManager.GoToState(
            navigationView,
            navigationView.IsPaneOpen ? "PaneOpen" : "PaneCompact",
            true
        );
    }

    /// <inheritdoc />
    protected override void OnInitialized(EventArgs e)
    {
        base.OnInitialized(e);

        NavigationStack.CollectionChanged += NavigationStackOnCollectionChanged;

        InvalidateArrange();
        InvalidateVisual();
        UpdateLayout();

        UpdateAutoSuggestBoxSuggestions();

        AddItemsToDictionaries();
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        // TODO: Refresh
        UpdateVisualState((NavigationView)sender);
    }

    /// <summary>
    /// This virtual method is called when this element is detached form a loaded tree.
    /// </summary>
    protected virtual void OnUnloaded(object sender, RoutedEventArgs e)
    {
        Loaded -= OnLoaded;
        Unloaded -= OnUnloaded;
        SizeChanged -= OnSizeChanged;

        NavigationStack.CollectionChanged -= NavigationStackOnCollectionChanged;

        PageIdOrTargetTagNavigationViewsDictionary.Clear();
        PageTypeNavigationViewsDictionary.Clear();

        ClearJournal();

        if (AutoSuggestBox is not null)
        {
            AutoSuggestBox.SuggestionChosen -= AutoSuggestBoxOnSuggestionChosen;
            AutoSuggestBox.QuerySubmitted -= AutoSuggestBoxOnQuerySubmitted;
        }

        if (Header is BreadcrumbBar breadcrumbBar)
        {
            breadcrumbBar.ItemClicked -= BreadcrumbBarOnItemClicked;
        }

        if (ToggleButton is not null)
        {
            ToggleButton.Click -= OnToggleButtonClick;
        }

        if (BackButton is not null)
        {
            BackButton.Click -= OnToggleButtonClick;
        }

        if (AutoSuggestBoxSymbolButton is not null)
        {
            AutoSuggestBoxSymbolButton.Click -= AutoSuggestBoxSymbolButtonOnClick;
        }
    }

    protected override void OnMouseDown(MouseButtonEventArgs e)
    {
        // Back button
        if (e.ChangedButton is MouseButton.XButton1)
        {
            _ = GoBack();
            e.Handled = true;
        }

        base.OnMouseDown(e);
    }

    /// <summary>
    /// This virtual method is called when ActualWidth or ActualHeight (or both) changed.
    /// </summary>
    protected virtual void OnSizeChanged(object sender, SizeChangedEventArgs e)
    {
        // TODO: Update reveal
    }

    /// <summary>
    /// This virtual method is called when <see cref="BackButton"/> is clicked.
    /// </summary>
    protected virtual void OnBackButtonClick(object sender, RoutedEventArgs e)
    {
        _ = GoBack();
    }

    /// <summary>
    /// This virtual method is called when <see cref="ToggleButton"/> is clicked.
    /// </summary>
    protected virtual void OnToggleButtonClick(object sender, RoutedEventArgs e)
    {
        SetCurrentValue(IsPaneOpenProperty, !IsPaneOpen);
    }

    /// <summary>
    /// This virtual method is called when <see cref="AutoSuggestBoxSymbolButton"/> is clicked.
    /// </summary>
    protected virtual void AutoSuggestBoxSymbolButtonOnClick(object sender, RoutedEventArgs e)
    {
        SetCurrentValue(IsPaneOpenProperty, !IsPaneOpen);
        _ = AutoSuggestBox?.Focus();
    }

    /// <summary>
    /// This virtual method is called when <see cref="PaneDisplayMode"/> is changed.
    /// </summary>
    protected virtual void OnPaneDisplayModeChanged()
    {
        switch (PaneDisplayMode)
        {
            case NavigationViewPaneDisplayMode.LeftFluent:
                SetCurrentValue(IsBackButtonVisibleProperty, NavigationViewBackButtonVisible.Collapsed);
                SetCurrentValue(IsPaneToggleVisibleProperty, false);
                break;
        }
    }

    /// <summary>
    /// This virtual method is called when <see cref="ItemTemplate"/> is changed.
    /// </summary>
    protected virtual void OnItemTemplateChanged()
    {
        UpdateMenuItemsTemplate();
    }

    internal void ToggleAllExpands()
    {
        // TODO: When shift clicked on navigationviewitem
    }

    internal void OnNavigationViewItemClick(NavigationViewItem navigationViewItem)
    {
        OnItemInvoked();

        _ = NavigateInternal(navigationViewItem);
    }

    protected virtual void BreadcrumbBarOnItemClicked(
        BreadcrumbBar sender,
        BreadcrumbBarItemClickedEventArgs e
    )
    {
        var item = (NavigationViewBreadcrumbItem)e.Item;
        _ = Navigate(item.PageId);
    }

    private void UpdateAutoSuggestBoxSuggestions()
    {
        if (AutoSuggestBox == null)
        {
            return;
        }

        _autoSuggestBoxItems.Clear();

        AddItemsToAutoSuggestBoxItems();
    }

    /// <summary>
    /// Navigate to the page after its name is selected in <see cref="AutoSuggestBox"/>.
    /// </summary>
    private void AutoSuggestBoxOnSuggestionChosen(
        AutoSuggestBox sender,
        AutoSuggestBoxSuggestionChosenEventArgs args
    )
    {
        if (sender.IsSuggestionListOpen)
        {
            return;
        }

        if (args.SelectedItem is not string selectedSuggestBoxItem)
        {
            return;
        }

        if (NavigateToMenuItemFromAutoSuggestBox(MenuItems, selectedSuggestBoxItem))
        {
            return;
        }

        _ = NavigateToMenuItemFromAutoSuggestBox(FooterMenuItems, selectedSuggestBoxItem);
    }

    private void AutoSuggestBoxOnQuerySubmitted(
        AutoSuggestBox sender,
        AutoSuggestBoxQuerySubmittedEventArgs args
    )
    {
        var suggestions = new List<string>();
        var querySplit = args.QueryText.Split(' ');

        foreach (var item in _autoSuggestBoxItems)
        {
            bool isMatch = true;

            foreach (string queryToken in querySplit)
            {
                if (item.IndexOf(queryToken, StringComparison.CurrentCultureIgnoreCase) < 0)
                {
                    isMatch = false;
                }
            }

            if (isMatch)
            {
                suggestions.Add(item);
            }
        }

        if (suggestions.Count <= 0)
        {
            return;
        }

        var element = suggestions.First();

        if (NavigateToMenuItemFromAutoSuggestBox(MenuItems, element))
        {
            return;
        }

        _ = NavigateToMenuItemFromAutoSuggestBox(FooterMenuItems, element);
    }

    protected virtual void AddItemsToDictionaries(IEnumerable list)
    {
        foreach (NavigationViewItem singleNavigationViewItem in list.OfType<NavigationViewItem>())
        {
            if (!PageIdOrTargetTagNavigationViewsDictionary.ContainsKey(singleNavigationViewItem.Id))
            {
                PageIdOrTargetTagNavigationViewsDictionary.Add(
                    singleNavigationViewItem.Id,
                    singleNavigationViewItem
                );
            }

            if (
                !PageIdOrTargetTagNavigationViewsDictionary.ContainsKey(
                    singleNavigationViewItem.TargetPageTag
                )
            )
            {
                PageIdOrTargetTagNavigationViewsDictionary.Add(
                    singleNavigationViewItem.TargetPageTag,
                    singleNavigationViewItem
                );
            }

            if (
                singleNavigationViewItem.TargetPageType != null
                && !PageTypeNavigationViewsDictionary.ContainsKey(singleNavigationViewItem.TargetPageType)
            )
            {
                PageTypeNavigationViewsDictionary.Add(
                    singleNavigationViewItem.TargetPageType,
                    singleNavigationViewItem
                );
            }

            singleNavigationViewItem.IsMenuElement = true;

            if (singleNavigationViewItem.HasMenuItems)
            {
                AddItemsToDictionaries(singleNavigationViewItem.MenuItems);
            }
        }
    }

    protected virtual void AddItemsToDictionaries()
    {
        AddItemsToDictionaries(MenuItems);
        AddItemsToDictionaries(FooterMenuItems);
    }

    protected virtual void AddItemsToAutoSuggestBoxItems(IEnumerable list)
    {
        foreach (NavigationViewItem singleNavigationViewItem in list.OfType<NavigationViewItem>())
        {
            if (
                singleNavigationViewItem is { Content: string content, TargetPageType: { } }
                && !string.IsNullOrWhiteSpace(content)
            )
            {
                _autoSuggestBoxItems.Add(content);
            }

            if (singleNavigationViewItem.HasMenuItems)
            {
                AddItemsToAutoSuggestBoxItems(singleNavigationViewItem.MenuItems);
            }
        }
    }

    protected virtual void AddItemsToAutoSuggestBoxItems()
    {
        AddItemsToAutoSuggestBoxItems(MenuItems);
        AddItemsToAutoSuggestBoxItems(FooterMenuItems);
    }

    protected virtual bool NavigateToMenuItemFromAutoSuggestBox(
        IEnumerable list,
        string selectedSuggestBoxItem
    )
    {
        foreach (NavigationViewItem singleNavigationViewItem in list.OfType<NavigationViewItem>())
        {
            if (singleNavigationViewItem.Content is string content && content == selectedSuggestBoxItem)
            {
                _ = NavigateInternal(singleNavigationViewItem);
                singleNavigationViewItem.BringIntoView();
                _ = singleNavigationViewItem.Focus(); // TODO: Element or content?

                return true;
            }

            if (
                NavigateToMenuItemFromAutoSuggestBox(
                    singleNavigationViewItem.MenuItems,
                    selectedSuggestBoxItem
                )
            )
            {
                return true;
            }
        }

        return false;
    }

    protected virtual void UpdateMenuItemsTemplate(IEnumerable list)
    {
        if (ItemTemplate == null)
        {
            return;
        }

        foreach (var item in list)
        {
            if (item is NavigationViewItem singleNavigationViewItem)
            {
                singleNavigationViewItem.Template = ItemTemplate;
            }
        }
    }

    protected virtual void UpdateMenuItemsTemplate()
    {
        UpdateMenuItemsTemplate(MenuItems);
        UpdateMenuItemsTemplate(FooterMenuItems);
    }

    protected virtual void CloseNavigationViewItemMenus()
    {
        if (Journal.Count <= 0 || IsPaneOpen)
        {
            return;
        }

        DeactivateMenuItems(MenuItems);
        DeactivateMenuItems(FooterMenuItems);

        INavigationViewItem currentItem = PageIdOrTargetTagNavigationViewsDictionary[Journal[^1]];
        if (currentItem.NavigationViewItemParent is null)
        {
            currentItem.Activate(this);
            return;
        }

        currentItem.Deactivate(this);
        currentItem.NavigationViewItemParent?.Activate(this);
    }

    protected void DeactivateMenuItems(IEnumerable list)
    {
        foreach (var item in list)
        {
            if (item is NavigationViewItem singleNavigationViewItem)
            {
                singleNavigationViewItem.Deactivate(this);
            }
        }
    }

    [DebuggerStepThrough]
    private void NavigationStackOnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                _breadcrumbBarItems.Add(
                    new NavigationViewBreadcrumbItem((INavigationViewItem)e.NewItems![0]!)
                );
                break;
            case NotifyCollectionChangedAction.Remove:
                _breadcrumbBarItems.RemoveAt(e.OldStartingIndex);
                break;
            case NotifyCollectionChangedAction.Replace:
                _breadcrumbBarItems[0] = new NavigationViewBreadcrumbItem(
                    (INavigationViewItem)e.NewItems![0]!
                );
                break;
            case NotifyCollectionChangedAction.Move:
                break;
            case NotifyCollectionChangedAction.Reset:
                _breadcrumbBarItems.Clear();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(e), e.Action, $"Unsupported action: {e.Action}");
        }
    }
}
