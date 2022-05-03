// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using WPFUI.Appearance;
using WPFUI.Common;
using WPFUI.Controls.Interfaces;

namespace WPFUI.Controls;

// TODO: It's still a disgusting mix. Requirements are: preview in the designer, and the ability to add items with XAML.

/// <summary>
/// Base class for creating new navigation controls.
/// </summary>
public abstract class Navigation : Control, INavigation
{
    #region Dependencies

    /// <summary>
    /// Property for <see cref="SelectedPageIndex"/>.
    /// </summary>
    public static readonly DependencyProperty SelectedPageIndexProperty = DependencyProperty.Register(
        nameof(SelectedPageIndex),
        typeof(int), typeof(Navigation),
        new PropertyMetadata(0));

    /// <summary>
    /// Property for <see cref="PreviousPageIndex"/>.
    /// </summary>
    public static readonly DependencyProperty PreviousPageIndexProperty = DependencyProperty.Register(
        nameof(PreviousPageIndex),
        typeof(int), typeof(Navigation),
        new PropertyMetadata(0));

    /// <summary>
    /// Property for <see cref="Frame"/>.
    /// </summary>
    public static readonly DependencyProperty FrameProperty = DependencyProperty.Register(nameof(Frame),
        typeof(Frame), typeof(Navigation),
        new PropertyMetadata(null));

    /// <summary>
    /// Property for <see cref="Items"/>.
    /// </summary>
    public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register(nameof(Items),
        typeof(ObservableCollection<INavigationItem>), typeof(Navigation),
        new PropertyMetadata(default(ObservableCollection<INavigationItem>), Items_OnChanged));

    /// <summary>
    /// Property for <see cref="Footer"/>.
    /// </summary>
    public static readonly DependencyProperty FooterProperty = DependencyProperty.Register(nameof(Footer),
        typeof(ObservableCollection<INavigationItem>), typeof(Navigation),
        new PropertyMetadata(default(ObservableCollection<INavigationItem>), Footer_OnChanged));

    #endregion

    #region Public variables

    /// <inheritdoc/>
    public int SelectedPageIndex
    {
        get => (int)GetValue(SelectedPageIndexProperty);
        set => SetValue(SelectedPageIndexProperty, value);
    }

    /// <inheritdoc/>
    public int PreviousPageIndex
    {
        get => (int)GetValue(PreviousPageIndexProperty);
        set => SetValue(PreviousPageIndexProperty, value);
    }

    /// <inheritdoc/>
    public Frame Frame
    {
        get => GetValue(FrameProperty) as Frame;
        set => SetValue(FrameProperty, value);
    }

    /// <inheritdoc/>
    public ObservableCollection<INavigationItem> Items
    {
        get => GetValue(ItemsProperty) as ObservableCollection<INavigationItem>;
        set => SetValue(ItemsProperty, value);
    }

    /// <inheritdoc/>
    public ObservableCollection<INavigationItem> Footer
    {
        get => GetValue(FooterProperty) as ObservableCollection<INavigationItem>;
        set => SetValue(FooterProperty, value);
    }

    /// <inheritdoc/>
    public string PageNow { get; internal set; } = String.Empty;

    /// <summary>
    /// Namespace containing the pages.
    /// </summary>
    public string Namespace { get; set; } = String.Empty;

    /// <summary>
    /// Navigation history containing pages tags.
    /// </summary>
    public List<string> History { get; set; } = new List<string>() { };

    /// <inheritdoc/>
    public INavigationItem Current { get; internal set; } = null;

    #endregion

    #region Events

    /// <summary>
    /// Event triggered when <see cref="Navigation"/> navigate to page.
    /// </summary>
    public static readonly RoutedEvent NavigatedEvent = EventManager.RegisterRoutedEvent(nameof(Navigated),
        RoutingStrategy.Bubble, typeof(RoutedNavigationEvent), typeof(Navigation));

    /// <inheritdoc/>
    public event RoutedNavigationEvent Navigated
    {
        add => AddHandler(NavigatedEvent, value);
        remove => RemoveHandler(NavigatedEvent, value);
    }

    /// <summary>
    /// Event triggered when <see cref="Navigation"/> navigate to the next page.
    /// </summary>
    public static readonly RoutedEvent NavigatedForwardEvent =
        EventManager.RegisterRoutedEvent(nameof(NavigatedForward), RoutingStrategy.Bubble,
            typeof(RoutedNavigationEvent), typeof(Navigation));

    /// <inheritdoc/>
    public event RoutedNavigationEvent NavigatedForward
    {
        add => AddHandler(NavigatedForwardEvent, value);
        remove => RemoveHandler(NavigatedForwardEvent, value);
    }

    /// <summary>
    /// Event triggered when <see cref="Navigation"/> navigate to the previous page.
    /// </summary>
    public static readonly RoutedEvent NavigatedBackwardEvent =
        EventManager.RegisterRoutedEvent(nameof(NavigatedBackward), RoutingStrategy.Bubble,
            typeof(RoutedNavigationEvent), typeof(Navigation));

    /// <inheritdoc/>
    public event RoutedNavigationEvent NavigatedBackward
    {
        add => AddHandler(NavigatedBackwardEvent, value);
        remove => RemoveHandler(NavigatedBackwardEvent, value);
    }

    #endregion

    #region Constructors

    /// <summary>
    /// Creates new instance of <see cref="INavigation"/> and sets it's default <see cref="FrameworkElement.Loaded"/> event.
    /// </summary>
    protected Navigation()
    {
        Items = new ObservableCollection<INavigationItem>();
        Footer = new ObservableCollection<INavigationItem>();

        Loaded += Navigation_OnLoaded;
    }

    #endregion

    #region Public Methods

    /// <inheritdoc/>
    public void Flush()
    {
        Items.Clear();
        Footer.Clear();

        PageNow = String.Empty;
    }

    /// <inheritdoc/>
    public void FlushPages()
    {
        if (Items != null)
            for (var i = 0; i < Items.Count; i++)
                Items[i].Instance = null;

        if (Footer != null)
            for (var i = 0; i < Footer.Count; i++)
                Footer[i].Instance = null;
    }

    /// <inheritdoc/>
    public bool Navigate(string pageTag, bool refresh = false, object dataContext = null)
    {
        for (var i = 0; i < Items?.Count; i++)
        {
            if ((Items[i].PageTag ?? String.Empty) != pageTag)
                continue;

            return Navigate(i, refresh, dataContext);
        }

        for (var i = 0; i < Footer?.Count; i++)
        {
            if ((Footer[i].PageTag ?? String.Empty) != pageTag)
                continue;

            return Navigate((i + Items?.Count ?? 0), refresh, dataContext);
        }

        return false;
    }

    /// <inheritdoc/>
    public bool Navigate(int pageIndex, bool refresh = false, object dataContext = null)
    {
        if (Items == null || Items?.Count == 0 || Frame == null || pageIndex < 0)
            return false;

        var indexShift = pageIndex;

        for (var i = 0; i < Items.Count; i++)
        {
            if (i != indexShift || !Items[i].IsValid)
                continue;

            PreviousPageIndex = SelectedPageIndex;
            SelectedPageIndex = i;

            NavigateToElement(Items[i], refresh, dataContext);

            return true;
        }

        indexShift -= Items.Count;

        if (Footer == null || !Footer.Any() || indexShift < 0)
            return false;

        for (var i = 0; i < Footer.Count; i++)
        {
            if (i != indexShift || !Footer[i].IsValid)
                continue;

            PreviousPageIndex = SelectedPageIndex;
            SelectedPageIndex = i;

            NavigateToElement(Footer[i], refresh, dataContext);

            return true;
        }

        return false;
    }

    /// <inheritdoc/>
    public void SetCurrentContext(object dataContext)
    {
        if (Current == null || Current.Instance == null)
            return;

        Current.Instance.DataContext = dataContext;
    }

    #endregion

    #region Protected methods

    /// <summary>
    /// This virtual method is called during any navigation and it raises the <see cref="Navigated"/> <see langword="event"/>.
    /// </summary>
    protected virtual void OnNavigated()
    {
        var newEvent = new RoutedNavigationEventArgs(Navigation.NavigatedEvent, this, Current);
        RaiseEvent(newEvent);
    }

    /// <summary>
    /// This virtual method is called during forward navigation and it raises the <see cref="NavigatedForward"/> <see langword="event"/>.
    /// </summary>
    protected virtual void OnNavigatedForward()
    {
        var newEvent = new RoutedNavigationEventArgs(Navigation.NavigatedForwardEvent, this, Current);
        RaiseEvent(newEvent);
    }

    /// <summary>
    /// This virtual method is called during backward navigation and it raises the <see cref="NavigatedBackward"/> <see langword="event"/>.
    /// </summary>
    protected virtual void OnNavigatedBackward()
    {
        var newEvent = new RoutedNavigationEventArgs(Navigation.NavigatedBackwardEvent, this, Current);
        RaiseEvent(newEvent);
    }

    #endregion

    #region Private methods

    private void NavigateToElement(INavigationItem element, bool refresh, object dataContext)
    {
        var pageTag = element.PageTag ?? String.Empty;

        if (String.IsNullOrEmpty(pageTag))
            throw new InvalidOperationException($"{typeof(NavigationItem)} has to have a string Tag.");

        if (pageTag == PageNow && !refresh)
            return;

        if (element.Instance == null || refresh)
        {
            if (element.Page == null)
                throw new InvalidOperationException($"{typeof(NavigationItem)} has to have a Page type defined.");

            element.Instance = CreateInstance(element.Page);
        }

        if (element.Instance == null)
            throw new InvalidOperationException("The new page instance could not be created, something went wrong");

        if (dataContext != null)
            element.Instance.DataContext = dataContext;

        InactivateElements(pageTag);

        Current = element;
        PageNow = pageTag;

        History.Add(pageTag);
        Frame.Navigate(element.Instance);

        if (element.Instance is INavigable navigable)
            navigable?.OnNavigationRequest(this, element);

        element.IsActive = true;

        OnNavigated();

        if (SelectedPageIndex > PreviousPageIndex)
            OnNavigatedForward();
        else
            OnNavigatedBackward();
    }

    private void InactivateElements(string exceptElement)
    {
        foreach (INavigationItem singleNavItem in Items)
            if ((singleNavItem.PageTag ?? String.Empty) != exceptElement)
                singleNavItem.IsActive = false;

        foreach (INavigationItem singleNavItem in Footer)
            if ((singleNavItem.PageTag ?? String.Empty) != exceptElement)
                singleNavItem.IsActive = false;
    }

    private void LoadFirstPage()
    {
        if (SelectedPageIndex < 0 || (!Items.Any() && !Footer.Any()))
            return;

        var indexShift = SelectedPageIndex;

        for (var i = 0; i < Items?.Count; i++)
        {
            if (i != indexShift)
                continue;

            Navigate(i);

            return;
        }

        indexShift -= Items?.Count ?? 0;

        if (indexShift < 0)
            return;

        for (var i = 0; i < Footer?.Count; i++)
        {
            if (i != indexShift)
                continue;

            Navigate(i + Items?.Count ?? 0);

            return;
        }
    }

    private Page CreateInstance(Type pageType)
    {
        if (pageType.IsAssignableFrom(typeof(Page)))
            return null;

        // TODO: Creation of the page by Activator in the designer throws exception, I do not know why
        if (Designer.IsInDesignMode)
            return new Page
            {
                Content = new TextBlock
                {
                    Foreground = Theme.GetAppTheme() == ThemeType.Dark ? Brushes.Black : Brushes.White,
                    Margin = new Thickness(20, 20, 20, 20),
                    Text =
                        "Page content preview is not available in design mode.\nYou can preview the content of the page by editing it directly."
                }
            };

        return (Page)Activator.CreateInstance(pageType.Assembly.FullName, pageType.FullName ?? "")?.Unwrap();

        //return Activator.CreateInstance(pageType) as Page ?? null;
    }

    #endregion

    #region Handlers

    private void Items_OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.NewItems != null)
            foreach (var addedItem in e.NewItems)

                if (addedItem is INavigationItem)
                    ((INavigationItem)addedItem).Click += Item_OnClicked;

        if (e.OldItems == null)
            return;

        foreach (var deletedItem in e.OldItems)
            ((INavigationItem)deletedItem).Click -= Item_OnClicked;
    }

    private void Footer_OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.NewItems != null)
            foreach (var addedItem in e.NewItems)
                if (addedItem is INavigationItem)
                    ((INavigationItem)addedItem).Click += Item_OnClicked;

        if (e.OldItems == null)
            return;

        foreach (var deletedItem in e.OldItems)
            ((INavigationItem)deletedItem).Click -= Item_OnClicked;
    }

    private static void Items_OnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        // TODO: Fix Navigation direct class reference
        if (d is not Navigation navigation)
            return;

        if (navigation.Items == null)
            return;

        foreach (var navigationItem in navigation.Items)
            if (navigationItem.Page != null)
                navigationItem.Click += navigation.Item_OnClicked;

        navigation.Items.CollectionChanged += navigation.Items_OnCollectionChanged;
    }

    private static void Footer_OnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not Navigation navigation)
            return;

        if (navigation.Footer == null)
            return;

        foreach (var navigationItem in navigation.Footer)
            if (navigationItem.Page != null)
                navigationItem.Click += navigation.Item_OnClicked;

        navigation.Footer.CollectionChanged += navigation.Footer_OnCollectionChanged;
    }

    private void Item_OnClicked(object sender, RoutedEventArgs e)
    {
        if (sender is not INavigationItem item)
            return;

        if (item.Page == null)
            return;

        var pageTag = item.PageTag ?? String.Empty;

        if (String.IsNullOrEmpty(pageTag))
            return;

        Navigate(pageTag);
    }

    private void Navigation_OnLoaded(object sender, RoutedEventArgs e)
    {
        if (Frame == null)
            return;

        Frame.NavigationUIVisibility = NavigationUIVisibility.Hidden;
        Frame.Navigating += Frame_OnNavigating;

        LoadFirstPage();
    }

    private void Frame_OnNavigating(object sender, NavigatingCancelEventArgs e)
    {
        if (e.Content == null)
            return;

        Frame.NavigationService.RemoveBackEntry();

        // TODO: Nothing is more permanent than temporary fixes... However, navigate using internal methods
        if (e.NavigationMode == NavigationMode.Back)
            e.Cancel = true;
    }

    #endregion
}
