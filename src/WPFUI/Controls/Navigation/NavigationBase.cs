// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WPFUI.Common;
using WPFUI.Controls.Interfaces;
using WPFUI.Mvvm.Contracts;
using WPFUI.Mvvm.Interfaces;

namespace WPFUI.Controls.Navigation;

/// <summary>
/// Base implementation for the navigation view.
/// </summary>
public abstract class NavigationBase : System.Windows.Controls.Control, INavigation
{
    /// <summary>
    /// Service used for navigation purposes.
    /// </summary>
    private readonly WPFUI.Services.NavigationService _navigationService;

    /// <summary>
    /// Property for <see cref="Items"/>.
    /// </summary>
    public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register(nameof(Items),
        typeof(ObservableCollection<INavigationControl>), typeof(NavigationBase),
        new PropertyMetadata((ObservableCollection<INavigationControl>)null, OnItemsChanged));

    /// <summary>
    /// Property for <see cref="Footer"/>.
    /// </summary>
    public static readonly DependencyProperty FooterProperty = DependencyProperty.Register(nameof(Footer),
        typeof(ObservableCollection<INavigationControl>), typeof(NavigationBase),
        new PropertyMetadata((ObservableCollection<INavigationControl>)null, OnFooterChanged));

    /// <summary>
    /// Property for <see cref="Frame"/>.
    /// </summary>
    public static readonly DependencyProperty FrameProperty = DependencyProperty.Register(nameof(Frame),
        typeof(Frame), typeof(NavigationBase),
        new PropertyMetadata((Frame)null, OnFrameChanged));

    /// <summary>
    /// Property for <see cref="TransitionDuration"/>.
    /// </summary>
    public static readonly DependencyProperty TransitionDurationProperty = DependencyProperty.Register(
        nameof(TransitionDuration),
        typeof(int), typeof(NavigationBase),
        new PropertyMetadata(300, OnTransitionDurationChanged));

    /// <summary>
    /// Property for <see cref="TransitionType"/>.
    /// </summary>
    public static readonly DependencyProperty TransitionTypeProperty = DependencyProperty.Register(
        nameof(TransitionType),
        typeof(Services.TransitionType), typeof(NavigationBase),
        new PropertyMetadata(Services.TransitionType.FadeInWithSlide, OnTransitionTypeChanged));

    /// <summary>
    /// Property for <see cref="SelectedPageIndex"/>.
    /// </summary>
    public static readonly DependencyProperty SelectedPageIndexProperty = DependencyProperty.Register(
        nameof(SelectedPageIndex),
        typeof(int), typeof(NavigationBase),
        new PropertyMetadata(0));

    /// <summary>
    /// Property for <see cref="Precache"/>.
    /// </summary>
    public static readonly DependencyProperty PrecacheProperty = DependencyProperty.Register(
        nameof(Precache),
        typeof(bool), typeof(NavigationBase),
        new PropertyMetadata(false));

    /// <summary>
    /// Attached property for <see cref="INavigationItem"/>'s to get its parent.
    /// </summary>
    internal static readonly DependencyProperty NavigationParentProperty = DependencyProperty.RegisterAttached(
        nameof(NavigationParent), typeof(INavigation), typeof(NavigationBase),
        new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));

    /// <inheritdoc/>
    public ObservableCollection<INavigationControl> Items
    {
        get => GetValue(ItemsProperty) as ObservableCollection<INavigationControl>;
        set => SetValue(ItemsProperty, value);
    }

    /// <inheritdoc/>
    public ObservableCollection<INavigationControl> Footer
    {
        get => GetValue(FooterProperty) as ObservableCollection<INavigationControl>;
        set => SetValue(FooterProperty, value);
    }

    /// <inheritdoc/>
    public int TransitionDuration
    {
        get => (int)GetValue(TransitionDurationProperty);
        set => SetValue(TransitionDurationProperty, value);
    }

    /// <inheritdoc/>
    public Services.TransitionType TransitionType
    {
        get => (Services.TransitionType)GetValue(TransitionTypeProperty);
        set => SetValue(TransitionTypeProperty, value);
    }

    /// <inheritdoc/>
    public Frame Frame
    {
        get => GetValue(FrameProperty) as Frame;
        set => SetValue(FrameProperty, value);
    }

    /// <inheritdoc/>
    public int SelectedPageIndex
    {
        get => (int)GetValue(SelectedPageIndexProperty);
        set => SetValue(SelectedPageIndexProperty, value);
    }

    /// <inheritdoc/>
    public bool Precache
    {
        get => (bool)GetValue(PrecacheProperty);
        set => SetValue(PrecacheProperty, value);
    }

    internal INavigation NavigationParent
    {
        get => (INavigation)GetValue(NavigationParentProperty);
        private set => SetValue(NavigationParentProperty, value);
    }

    #region Events

    /// <summary>
    /// Event triggered when <see cref="NavigationBase"/> navigate to page.
    /// </summary>
    public static readonly RoutedEvent NavigatedEvent = EventManager.RegisterRoutedEvent(nameof(Navigated),
        RoutingStrategy.Bubble, typeof(RoutedNavigationEvent), typeof(NavigationBase));

    /// <inheritdoc/>
    public event RoutedNavigationEvent Navigated
    {
        add => AddHandler(NavigatedEvent, value);
        remove => RemoveHandler(NavigatedEvent, value);
    }

    /// <summary>
    /// Event triggered when <see cref="NavigationBase"/> navigate to the next page.
    /// </summary>
    public static readonly RoutedEvent NavigatedForwardEvent =
        EventManager.RegisterRoutedEvent(nameof(NavigatedForward), RoutingStrategy.Bubble,
            typeof(RoutedNavigationEvent), typeof(NavigationBase));

    /// <inheritdoc/>
    public event RoutedNavigationEvent NavigatedForward
    {
        add => AddHandler(NavigatedForwardEvent, value);
        remove => RemoveHandler(NavigatedForwardEvent, value);
    }

    /// <summary>
    /// Event triggered when <see cref="NavigationBase"/> navigate to the previous page.
    /// </summary>
    public static readonly RoutedEvent NavigatedBackwardEvent =
        EventManager.RegisterRoutedEvent(nameof(NavigatedBackward), RoutingStrategy.Bubble,
            typeof(RoutedNavigationEvent), typeof(NavigationBase));

    /// <inheritdoc/>
    public event RoutedNavigationEvent NavigatedBackward
    {
        add => AddHandler(NavigatedBackwardEvent, value);
        remove => RemoveHandler(NavigatedBackwardEvent, value);
    }

    #endregion

    /// <inheritdoc/>
    public IPageService PageService
    {
        get => _navigationService.GetService();
        set => _navigationService.SetService(value);
    }

    /// <inheritdoc/>
    public int PreviousPageIndex => _navigationService?.GetPreviousId() ?? 0;

    /// <inheritdoc/>
    public INavigationItem Current { get; internal set; }

    /// <summary>
    /// Navigation history containing pages tags.
    /// </summary>
    public List<string> History { get; internal set; }

    /// <summary>
    /// Static constructor overriding default properties.
    /// </summary>
    static NavigationBase()
    {
        KeyboardNavigation.DirectionalNavigationProperty.OverrideMetadata(
            typeof(NavigationBase),
            new FrameworkPropertyMetadata(KeyboardNavigationMode.Contained));

        KeyboardNavigation.TabNavigationProperty.OverrideMetadata(
            typeof(NavigationBase),
            new FrameworkPropertyMetadata(KeyboardNavigationMode.Once));
    }

    /// <summary>
    /// Prepares base navigation properties.
    /// </summary>
    protected NavigationBase()
    {
        _navigationService = new WPFUI.Services.NavigationService();
        _navigationService.TransitionDuration = TransitionDuration;
        _navigationService.TransitionType = TransitionType;

        _navigationService.SetFrame(Frame);

        InitializeBase();
    }

    /// <summary>
    /// Initializes internal properties.
    /// </summary>
    private void InitializeBase()
    {
        // Let the NavigationItem children be able to get me.
        NavigationParent = this;

        Current = (INavigationItem)null;
        History = new List<string>();

        // Prepare individual collections for this navigation
        Items ??= new ObservableCollection<INavigationControl>();
        Footer ??= new ObservableCollection<INavigationControl>();

        // Loaded does not have override
        Loaded += OnLoaded;
    }

    /// <inheritdoc/>
    public bool Navigate(Type pageType)
    {
        return Navigate(pageType, null);
    }

    /// <inheritdoc/>
    public bool Navigate(Type pageType, object dataContext)
    {
        if (!_navigationService.Navigate(pageType, dataContext))
            return false;

        SelectedPageIndex = _navigationService.GetCurrentId();

        UpdateItems();

        OnNavigated();

        if (_navigationService.GetCurrentId() > _navigationService.GetPreviousId())
            OnNavigatedForward();
        else
            OnNavigatedBackward();

        return true;
    }

    /// <inheritdoc/>
    public bool Navigate(string pageTag)
    {
        return Navigate(pageTag, null);
    }

    /// <inheritdoc/>
    public bool Navigate(string pageTag, object dataContext)
    {
        if (!_navigationService.Navigate(pageTag, dataContext))
            return false;

        SelectedPageIndex = _navigationService.GetCurrentId();

        UpdateItems();

        OnNavigated();

        if (_navigationService.GetCurrentId() > _navigationService.GetPreviousId())
            OnNavigatedForward();
        else
            OnNavigatedBackward();

        return true;
    }


    /// <inheritdoc/>
    public bool Navigate(int pageId)
    {
        return Navigate(pageId, null);
    }

    /// <inheritdoc/>
    public bool Navigate(int pageId, object dataContext)
    {
        if (!_navigationService.Navigate(pageId, dataContext))
            return false;

        SelectedPageIndex = _navigationService.GetCurrentId();

        UpdateItems();

        OnNavigated();

        if (_navigationService.GetCurrentId() > _navigationService.GetPreviousId())
            OnNavigatedForward();
        else
            OnNavigatedBackward();

        return true;
    }

    /// <inheritdoc/>
    public bool NavigateExternal(object frameworkElement)
    {
        return NavigateExternal(frameworkElement, null);
    }

    /// <inheritdoc/>
    public bool NavigateExternal(object frameworkElement, object dataContext)
    {
        if (!_navigationService.NavigateExternal(frameworkElement, dataContext))
            return false;

        SelectedPageIndex = _navigationService.GetCurrentId();

        UpdateItems();

        OnNavigated();

        if (_navigationService.GetCurrentId() > _navigationService.GetPreviousId())
            OnNavigatedForward();
        else
            OnNavigatedBackward();

        return true;
    }

    /// <inheritdoc/>
    public bool NavigateExternal(Uri absolutePageUri)
    {
        return NavigateExternal(absolutePageUri, null);
    }

    /// <inheritdoc/>
    public bool NavigateExternal(Uri absolutePageUri, object dataContext)
    {
        if (!_navigationService.NavigateExternal(absolutePageUri, dataContext))
            return false;

        SelectedPageIndex = _navigationService.GetCurrentId();

        OnNavigated();

        if (_navigationService.GetCurrentId() > _navigationService.GetPreviousId())
            OnNavigatedForward();
        else
            OnNavigatedBackward();

        return true;
    }

    /// <inheritdoc/>
    public void SetCurrentContext(object dataContext)
    {
        if (Frame?.Content is not FrameworkElement)
            return;

        ((FrameworkElement)Frame.Content).DataContext = dataContext;

        if (dataContext is IViewModel)
            ((IViewModel)dataContext).OnMounted(((FrameworkElement)Frame.Content));
    }

    /// <inheritdoc/>
    public bool SetContext(string pageTag, object dataContext)
    {
        return _navigationService.SetContext(pageTag, dataContext);
    }

    /// <inheritdoc/>
    public bool SetContext(int pageId, object dataContext)
    {
        return _navigationService.SetContext(pageId, dataContext);
    }

    /// <inheritdoc/>
    public void Flush()
    {
        Items.Clear();
        Footer.Clear();

        Current = (INavigationItem)null;
    }

    /// <inheritdoc/>
    public void ClearCache()
    {
        _navigationService.ClearCache();
    }

    /// <summary>
    /// Updates <see cref="Current"/> property and modifies Active attribute of navigation items.
    /// </summary>
    private void UpdateItems()
    {
        var currentTag = _navigationService.GetCurrentTag();

        foreach (var singleNavigationControl in Items)
        {
            if (singleNavigationControl is not INavigationItem)
                continue;

            if (((INavigationItem)singleNavigationControl).PageTag == currentTag)
            {
                ((INavigationItem)singleNavigationControl).IsActive = true;
                Current = (INavigationItem)singleNavigationControl;
            }
            else
            {
                ((INavigationItem)singleNavigationControl).IsActive = false;
            }
        }

        foreach (var singleNavigationControl in Footer)
        {
            if (singleNavigationControl is not INavigationItem)
                continue;

            if (((INavigationItem)singleNavigationControl).PageTag == currentTag)
            {
                ((INavigationItem)singleNavigationControl).IsActive = true;
                Current = (INavigationItem)singleNavigationControl;
            }
            else
            {
                ((INavigationItem)singleNavigationControl).IsActive = false;
            }
        }
    }

    /// <summary>
    /// This virtual method is called when <see cref="INavigation"/> is loaded.
    /// </summary>
    protected virtual async void OnLoaded(object sender, RoutedEventArgs e)
    {
        _navigationService.UpdateItems(Items, Footer);

        if (PageService == null && Frame != null && SelectedPageIndex > -1)
            Navigate(SelectedPageIndex);

        // If we are using the MVVM model, do not use the cache.
        if (Precache)
        {
            if (PageService != null)
                throw new InvalidOperationException("The cache cannot be used if you are using IPageService.");

            // TODO: Precache
            //await PrecacheInstances();
        }
    }

    /// <inheritdoc/>
    protected override void OnKeyDown(KeyEventArgs e)
    {
        // We handle Left/Up/Right/Down keys for keyboard navigation only,
        // so no modifiers are needed.
        if (Keyboard.Modifiers is not ModifierKeys.None)
            return;

        // For most cases, this method do nothing because it does not receive focus by default.
        // But if someone set focus to it, the key handling can move the focus to its navigation children.
        switch (e.Key)
        {
            // We use Direction Left/Up/Right/Down instead of Previous/Next to make sure
            // that the KeyboardNavigation.DirectionalNavigation property works correctly.
            case Key.Left:
                MoveFocus(this, FocusNavigationDirection.Left);
                e.Handled = true;
                break;

            case Key.Up:
                MoveFocus(this, FocusNavigationDirection.Up);
                e.Handled = true;
                break;

            case Key.Right:
                MoveFocus(this, FocusNavigationDirection.Right);
                e.Handled = true;
                break;

            case Key.Down:
                MoveFocus(this, FocusNavigationDirection.Down);
                e.Handled = true;
                break;
        }

        if (!e.Handled)
            base.OnKeyDown(e);

        static void MoveFocus(FrameworkElement element, FocusNavigationDirection direction)
        {
            var request = new TraversalRequest(direction);
            element.MoveFocus(request);
        }
    }

    /// <summary>
    /// This virtual method is called during any navigation and it raises the <see cref="Navigated"/> <see langword="event"/>.
    /// </summary>
    protected virtual void OnNavigated()
    {
        var newEvent = new RoutedNavigationEventArgs(NavigatedEvent, this, Current);
        RaiseEvent(newEvent);
    }

    /// <summary>
    /// This virtual method is called during forward navigation and it raises the <see cref="NavigatedForward"/> <see langword="event"/>.
    /// </summary>
    protected virtual void OnNavigatedForward()
    {
        var newEvent = new RoutedNavigationEventArgs(NavigatedForwardEvent, this, Current);
        RaiseEvent(newEvent);
    }

    /// <summary>
    /// This virtual method is called during backward navigation and it raises the <see cref="NavigatedBackward"/> <see langword="event"/>.
    /// </summary>
    protected virtual void OnNavigatedBackward()
    {
        var newEvent = new RoutedNavigationEventArgs(NavigatedBackwardEvent, this, Current);
        RaiseEvent(newEvent);
    }

    /// <summary>
    /// This virtual method is called when one of the navigation items is clicked.
    /// </summary>
    protected virtual void OnNavigationItemClicked(object sender, RoutedEventArgs e)
    {
        if (sender is not INavigationItem navigationItem)
            return;

        if (navigationItem.AbsolutePageSource == null && navigationItem.PageType == null)
            return;

        if (PageService == null)
        {
            Navigate(navigationItem.PageTag);

            return;
        }

        if (navigationItem.PageType == null)
            throw new InvalidOperationException("When navigating through the IPageService, the navigated page type must be defined the INavigationItem.PageType.");

        Navigate(navigationItem.PageType);
    }

    /// <summary>
    /// This virtual method is called when something is added, deleted or changed in <see cref="Items"/> or <see cref="Footer"/>.
    /// </summary>
    protected virtual void OnNavigationCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        if (IsLoaded)
            _navigationService.UpdateItems(Items, Footer);

        if (e.NewItems != null)
            foreach (var addedItem in e.NewItems)
                if (addedItem is INavigationItem)
                {
                    ((INavigationItem)addedItem).Click -= OnNavigationItemClicked; // Unsafe - Remove duplicates
                    ((INavigationItem)addedItem).Click += OnNavigationItemClicked;
                }

        if (e.OldItems == null)
            return;

        foreach (var deletedItem in e.OldItems)
            ((INavigationItem)deletedItem).Click -= OnNavigationItemClicked;
    }

    /// <summary>
    /// Triggered when <see cref="ItemsProperty"/> is changed.
    /// </summary>
    private static void OnItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not NavigationBase navigationBase)
            return;

        if (e.NewValue is not ObservableCollection<INavigationControl> itemsCollection)
            return;

        itemsCollection.CollectionChanged += navigationBase.OnNavigationCollectionChanged;
    }

    /// <summary>
    /// Triggered when <see cref="FooterProperty"/> is changed.
    /// </summary>
    private static void OnFooterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not NavigationBase navigationBase)
            return;

        if (e.NewValue is not ObservableCollection<INavigationControl> itemsCollection)
            return;

        itemsCollection.CollectionChanged += navigationBase.OnNavigationCollectionChanged;
    }

    /// <summary>
    /// This virtual method is called when one of the navigation items is clicked.
    /// </summary>
    protected virtual void OnFrameChanged(Frame frame)
    {
        _navigationService.SetFrame(frame);
    }

    /// <summary>
    /// Triggered when <see cref="FrameProperty"/> is changed.
    /// </summary>
    private static void OnFrameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not NavigationBase navigationBase || e.NewValue is not Frame frame)
            return;

        navigationBase.OnFrameChanged(frame);
    }

    private static void OnTransitionDurationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not NavigationBase navigation)
            return;

        navigation._navigationService.TransitionDuration = (int)e.NewValue;
    }

    private static void OnTransitionTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not NavigationBase navigation)
            return;

        navigation._navigationService.TransitionType = (Services.TransitionType)e.NewValue;
    }

    /// <summary>
    /// Gets the <see cref="Navigation"/> parent view for its <see cref="NavigationItem"/> children.
    /// </summary>
    /// <param name="navigationItem"></param>
    /// <returns></returns>
    internal static NavigationBase? GetNavigationParent<T>(T navigationItem)
        where T : DependencyObject, INavigationItem
    {
        return (NavigationBase?)navigationItem.GetValue(NavigationParentProperty);
    }
}
