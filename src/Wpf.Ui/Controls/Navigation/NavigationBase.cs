// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

#nullable enable

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CommunityToolkit.Diagnostics;
using Wpf.Ui.Common;
using Wpf.Ui.Controls.Interfaces;
using Wpf.Ui.Mvvm.Contracts;
using Wpf.Ui.Services.Internal;

namespace Wpf.Ui.Controls.Navigation;

/// <summary>
/// Base implementation for the navigation view.
/// </summary>
public abstract class NavigationBase : System.Windows.Controls.Control, INavigation
{
    private FrameManager _frameManager = null!;
    private NavigationManager _navigationManager = null!;
    private IPageService? _pageService;
    private INavigationItem[] _items = null!;
    private bool _loaded;

    #region DependencyProperties

    /// <summary>
    /// Property for <see cref="Items"/>.
    /// </summary>
    public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register(nameof(Items),
        typeof(ObservableCollection<INavigationControl>), typeof(NavigationBase),
        new PropertyMetadata(null));

    /// <summary>
    /// Property for <see cref="Footer"/>.
    /// </summary>
    public static readonly DependencyProperty FooterProperty = DependencyProperty.Register(nameof(Footer),
        typeof(ObservableCollection<INavigationControl>), typeof(NavigationBase),
        new PropertyMetadata(null));

    /// <summary>
    /// Property for <see cref="HiddenItems"/>.
    /// </summary>
    public static readonly DependencyProperty HiddenItemsProperty = DependencyProperty.Register(nameof(HiddenItems),
        typeof(List<INavigationItem>), typeof(NavigationBase),
        new PropertyMetadata(null));

    /// <summary>
    /// Property for <see cref="Orientation"/>.
    /// </summary>
    public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(nameof(Orientation),
        typeof(Orientation), typeof(NavigationBase),
        new FrameworkPropertyMetadata(Orientation.Vertical,
            FrameworkPropertyMetadataOptions.AffectsMeasure));

    /// <summary>
    /// Property for <see cref="Frame"/>.
    /// </summary>
    public static readonly DependencyProperty FrameProperty = DependencyProperty.Register(nameof(Frame),
        typeof(Frame), typeof(NavigationBase),
        new PropertyMetadata());

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
        typeof(Animations.TransitionType), typeof(NavigationBase),
        new PropertyMetadata(Animations.TransitionType.FadeInWithSlide, OnTransitionTypeChanged));

    /// <summary>
    /// Property for <see cref="SelectedPageIndex"/>.
    /// </summary>
    public static readonly DependencyProperty SelectedPageIndexProperty = DependencyProperty.Register(
        nameof(SelectedPageIndex),
        typeof(int), typeof(NavigationBase),
        new PropertyMetadata(-1));

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

    #endregion

    #region Properties

    /// <inheritdoc/>
    public ObservableCollection<INavigationControl> Items
    {
        get => (ObservableCollection<INavigationControl>) GetValue(ItemsProperty);
        set => SetValue(ItemsProperty, value);
    }

    /// <inheritdoc/>
    public ObservableCollection<INavigationControl> Footer
    {
        get => (ObservableCollection<INavigationControl>) GetValue(FooterProperty);
        set => SetValue(FooterProperty, value);
    }

    /// <inheritdoc/>
    public List<INavigationItem> HiddenItems
    {
        get => (List<INavigationItem>) GetValue(HiddenItemsProperty);
        set => SetValue(HiddenItemsProperty, value);
    }

    /// <inheritdoc/>
    [Obsolete("Work in progress.")]
    public Orientation Orientation
    {
        get => (Orientation)GetValue(OrientationProperty);
        set => SetValue(OrientationProperty, value);
    }

    /// <inheritdoc/>
    public int TransitionDuration
    {
        get => (int)GetValue(TransitionDurationProperty);
        set => SetValue(TransitionDurationProperty, value);
    }

    /// <inheritdoc/>
    public Animations.TransitionType TransitionType
    {
        get => (Animations.TransitionType)GetValue(TransitionTypeProperty);
        set => SetValue(TransitionTypeProperty, value);
    }

    /// <inheritdoc/>
    public Frame Frame
    {
        get => (GetValue(FrameProperty) as Frame)!;
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

    #endregion

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

    #endregion

    /// <inheritdoc/>
    public bool CanGoBack => !DesignerProperties.GetIsInDesignMode(this) && _navigationManager.CanGoBack;

    public ObservableCollection<INavigationItem> NavigationStack => _navigationManager.NavigationStack;

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
        Items = new ObservableCollection<INavigationControl>();
        Footer = new ObservableCollection<INavigationControl>();
        HiddenItems = new List<INavigationItem>();

        // Let the NavigationItem children be able to get me.
        NavigationParent = this;

        if (DesignerProperties.GetIsInDesignMode(this))
            return;

        Loaded += OnLoaded;
        Unloaded += OnUnloaded;
    }

    /// <inheritdoc/>
    public void SetIPageService(IPageService pageService)
    {
        _pageService = pageService;
    }

    /// <inheritdoc/>
    public void Preload() => _navigationManager.Preload();

    /// <inheritdoc/>
    public void ClearCache() => _navigationManager.ClearCache();

    /// <inheritdoc/>
    public void NavigateTo(string pageTag, object? dataContext = null)
    {
        if (_navigationManager.NavigateTo(pageTag, dataContext))
            OnNavigated();
    }

    /// <inheritdoc/>
    public void NavigateTo(Type type, object? dataContext = null)
    {
        if (_navigationManager.NavigateTo(type, dataContext))
            OnNavigated();
    }

    /// <summary>
    /// This virtual method is called when <see cref="INavigation"/> is loaded.
    /// </summary>
    protected virtual void OnLoaded(object sender, RoutedEventArgs e)
    {
        Guard.IsNotNull(Frame, nameof(Frame));

        _items = MergeItems();

        _frameManager = new FrameManager(Frame, TransitionDuration, TransitionType);
        _navigationManager = new NavigationManager(Frame, _pageService, _items);

        if (SelectedPageIndex > -1)
        {
            _navigationManager.NavigateTo(SelectedPageIndex);
            OnNavigated();
        }

        _loaded = true;
    }

    protected virtual void OnUnloaded(object sender, RoutedEventArgs e)
    {
        Loaded -= OnLoaded;
        Unloaded -= OnUnloaded;

        _frameManager.Dispose();
        _navigationManager.Dispose();

        foreach (var item in _items)
            item.Click -= OnNavigationItemClicked;

        _items = null!;
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
        var newEvent = new RoutedNavigationEventArgs(NavigatedEvent, this, _navigationManager.NavigationFrom, NavigationStack[NavigationStack.Count - 1]);
        RaiseEvent(newEvent);
    }

    /// <summary>
    /// This virtual method is called when one of the navigation items is clicked.
    /// </summary>
    protected virtual void OnNavigationItemClicked(object sender, RoutedEventArgs e)
    {
        if (sender is not INavigationItem navigationItem)
            return;

        NavigateTo(navigationItem.PageTag);
    }

    private static void OnTransitionDurationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not NavigationBase navigation)
            return;

        if (navigation._loaded)
            navigation._frameManager.TransitionDuration = (int)e.NewValue;
    }

    private static void OnTransitionTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not NavigationBase navigation)
            return;

        if (navigation._loaded)
            navigation._frameManager.TransitionType = (Animations.TransitionType)e.NewValue;
    }

    /// <summary>
    /// Gets the <see cref="INavigation"/> parent view for its <see cref="NavigationItem"/> children.
    /// </summary>
    /// <param name="navigationItem"></param>
    /// <returns></returns>
    internal static NavigationBase? GetNavigationParent<T>(T navigationItem)
        where T : DependencyObject, INavigationItem
    {
        return (NavigationBase?)navigationItem.GetValue(NavigationParentProperty);
    }

    private INavigationItem[] MergeItems()
    {
        List<INavigationItem> buffer = new List<INavigationItem>(Items.Count);

        AddToBuffer(Items);
        AddToBuffer(Footer);
        AddToBuffer(HiddenItems, item => item.IsHidden = true);

        return buffer.ToArray();

        void AddToBuffer(IEnumerable<object> list, Action<INavigationItem>? action = null)
        {
            foreach (var addedItem in list)
            {
                if (addedItem is not INavigationItem item) continue;

                if (item.PageType is not null)
                    item.Click += OnNavigationItemClicked;

                action?.Invoke(item);
                buffer.Add(item);
            }
        }
    }
}
