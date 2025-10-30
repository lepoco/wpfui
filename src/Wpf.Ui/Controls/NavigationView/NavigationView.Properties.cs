// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Controls;
using Wpf.Ui.Animations;

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

/// <content>
/// Defines the dependency properties and dp callbacks for <see cref="NavigationView"/> control
/// </content>
public partial class NavigationView
{
    /// <summary>Identifies the <see cref="EnableDebugMessages"/> dependency property.</summary>
    public static readonly DependencyProperty EnableDebugMessagesProperty = DependencyProperty.Register(
        nameof(EnableDebugMessages),
        typeof(bool),
        typeof(NavigationView),
        new FrameworkPropertyMetadata(false)
    );

    /// <summary>Identifies the <see cref="Header"/> dependency property.</summary>
    public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
        nameof(Header),
        typeof(object),
        typeof(NavigationView),
        new FrameworkPropertyMetadata(null)
    );

    /// <summary>Identifies the <see cref="HeaderVisibility"/> dependency property.</summary>
    public static readonly DependencyProperty HeaderVisibilityProperty = DependencyProperty.Register(
        nameof(HeaderVisibility),
        typeof(Visibility),
        typeof(NavigationView),
        new FrameworkPropertyMetadata(Visibility.Visible)
    );

    /// <summary>Identifies the <see cref="AlwaysShowHeader"/> dependency property.</summary>
    public static readonly DependencyProperty AlwaysShowHeaderProperty = DependencyProperty.Register(
        nameof(AlwaysShowHeader),
        typeof(bool),
        typeof(NavigationView),
        new FrameworkPropertyMetadata(false)
    );

    private static readonly DependencyPropertyKey MenuItemsPropertyKey = DependencyProperty.RegisterReadOnly(
        nameof(MenuItems),
        typeof(ObservableCollection<object>),
        typeof(NavigationView),
        new PropertyMetadata(null)
    );

    /// <summary>Identifies the <see cref="MenuItems"/> dependency property.</summary>
    public static readonly DependencyProperty MenuItemsProperty = MenuItemsPropertyKey.DependencyProperty;

    /// <summary>Identifies the <see cref="MenuItemsSource"/> dependency property.</summary>
    public static readonly DependencyProperty MenuItemsSourceProperty = DependencyProperty.Register(
        nameof(MenuItemsSource),
        typeof(object),
        typeof(NavigationView),
        new FrameworkPropertyMetadata(null, OnMenuItemsSourceChanged)
    );

    private static readonly DependencyPropertyKey FooterMenuItemsPropertyKey =
        DependencyProperty.RegisterReadOnly(
            nameof(FooterMenuItems),
            typeof(ObservableCollection<object>),
            typeof(NavigationView),
            new PropertyMetadata(null)
        );

    /// <summary>Identifies the <see cref="FooterMenuItems"/> dependency property.</summary>
    public static readonly DependencyProperty FooterMenuItemsProperty =
        FooterMenuItemsPropertyKey.DependencyProperty;

    /// <summary>Identifies the <see cref="FooterMenuItemsSource"/> dependency property.</summary>
    public static readonly DependencyProperty FooterMenuItemsSourceProperty = DependencyProperty.Register(
        nameof(FooterMenuItemsSource),
        typeof(object),
        typeof(NavigationView),
        new FrameworkPropertyMetadata(null, OnFooterMenuItemsSourceChanged)
    );

    /// <summary>Identifies the <see cref="ContentOverlay"/> dependency property.</summary>
    public static readonly DependencyProperty ContentOverlayProperty = DependencyProperty.Register(
        nameof(ContentOverlay),
        typeof(object),
        typeof(NavigationView),
        new FrameworkPropertyMetadata(null)
    );

    /// <summary>Identifies the <see cref="IsBackEnabled"/> dependency property.</summary>
    public static readonly DependencyProperty IsBackEnabledProperty = DependencyProperty.Register(
        nameof(IsBackEnabled),
        typeof(bool),
        typeof(NavigationView),
        new FrameworkPropertyMetadata(false)
    );

    /// <summary>Identifies the <see cref="IsBackButtonVisible"/> dependency property.</summary>
    public static readonly DependencyProperty IsBackButtonVisibleProperty = DependencyProperty.Register(
        nameof(IsBackButtonVisible),
        typeof(NavigationViewBackButtonVisible),
        typeof(NavigationView),
        new FrameworkPropertyMetadata(NavigationViewBackButtonVisible.Auto)
    );

    /// <summary>Identifies the <see cref="IsPaneToggleVisible"/> dependency property.</summary>
    public static readonly DependencyProperty IsPaneToggleVisibleProperty = DependencyProperty.Register(
        nameof(IsPaneToggleVisible),
        typeof(bool),
        typeof(NavigationView),
        new FrameworkPropertyMetadata(true)
    );

    /// <summary>Identifies the <see cref="IsPaneOpen"/> dependency property.</summary>
    public static readonly DependencyProperty IsPaneOpenProperty = DependencyProperty.Register(
        nameof(IsPaneOpen),
        typeof(bool),
        typeof(NavigationView),
        new FrameworkPropertyMetadata(true, OnIsPaneOpenChanged)
    );

    /// <summary>Identifies the <see cref="IsPaneVisible"/> dependency property.</summary>
    public static readonly DependencyProperty IsPaneVisibleProperty = DependencyProperty.Register(
        nameof(IsPaneVisible),
        typeof(bool),
        typeof(NavigationView),
        new FrameworkPropertyMetadata(false)
    );

    /// <summary>Identifies the <see cref="OpenPaneLength"/> dependency property.</summary>
    public static readonly DependencyProperty OpenPaneLengthProperty = DependencyProperty.Register(
        nameof(OpenPaneLength),
        typeof(double),
        typeof(NavigationView),
        new FrameworkPropertyMetadata(0D)
    );

    /// <summary>Identifies the <see cref="CompactPaneLength"/> dependency property.</summary>
    public static readonly DependencyProperty CompactPaneLengthProperty = DependencyProperty.Register(
        nameof(CompactPaneLength),
        typeof(double),
        typeof(NavigationView),
        new FrameworkPropertyMetadata(0D)
    );

    /// <summary>Identifies the <see cref="PaneHeader"/> dependency property.</summary>
    public static readonly DependencyProperty PaneHeaderProperty = DependencyProperty.Register(
        nameof(PaneHeader),
        typeof(object),
        typeof(NavigationView),
        new FrameworkPropertyMetadata(null)
    );

    /// <summary>Identifies the <see cref="PaneTitle"/> dependency property.</summary>
    public static readonly DependencyProperty PaneTitleProperty = DependencyProperty.Register(
        nameof(PaneTitle),
        typeof(string),
        typeof(NavigationView),
        new FrameworkPropertyMetadata(null)
    );

    /// <summary>Identifies the <see cref="PaneFooter"/> dependency property.</summary>
    public static readonly DependencyProperty PaneFooterProperty = DependencyProperty.Register(
        nameof(PaneFooter),
        typeof(object),
        typeof(NavigationView),
        new FrameworkPropertyMetadata(null)
    );

    /// <summary>Identifies the <see cref="PaneDisplayMode"/> dependency property.</summary>
    public static readonly DependencyProperty PaneDisplayModeProperty = DependencyProperty.Register(
        nameof(PaneDisplayMode),
        typeof(NavigationViewPaneDisplayMode),
        typeof(NavigationView),
        new FrameworkPropertyMetadata(NavigationViewPaneDisplayMode.Left, OnPaneDisplayModeChanged)
    );

    /// <summary>Identifies the <see cref="AutoSuggestBox"/> dependency property.</summary>
    public static readonly DependencyProperty AutoSuggestBoxProperty = DependencyProperty.Register(
        nameof(AutoSuggestBox),
        typeof(AutoSuggestBox),
        typeof(NavigationView),
        new FrameworkPropertyMetadata(null, OnAutoSuggestBoxChanged)
    );

    /// <summary>Identifies the <see cref="TitleBar"/> dependency property.</summary>
    public static readonly DependencyProperty TitleBarProperty = DependencyProperty.Register(
        nameof(TitleBar),
        typeof(TitleBar),
        typeof(NavigationView),
        new FrameworkPropertyMetadata(null, OnTitleBarChanged)
    );

    /// <summary>Identifies the <see cref="BreadcrumbBar"/> dependency property.</summary>
    public static readonly DependencyProperty BreadcrumbBarProperty = DependencyProperty.Register(
        nameof(BreadcrumbBar),
        typeof(BreadcrumbBar),
        typeof(NavigationView),
        new FrameworkPropertyMetadata(null, OnBreadcrumbBarChanged)
    );

    /// <summary>Identifies the <see cref="ItemTemplate"/> dependency property.</summary>
    public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.Register(
        nameof(ItemTemplate),
        typeof(ControlTemplate),
        typeof(NavigationView),
        new FrameworkPropertyMetadata(
            null,
            FrameworkPropertyMetadataOptions.AffectsMeasure,
            OnItemTemplateChanged
        )
    );

    /// <summary>Identifies the <see cref="TransitionDuration"/> dependency property.</summary>
    public static readonly DependencyProperty TransitionDurationProperty = DependencyProperty.Register(
        nameof(TransitionDuration),
        typeof(int),
        typeof(NavigationView),
        new FrameworkPropertyMetadata(200)
    );

    /// <summary>Identifies the <see cref="Transition"/> dependency property.</summary>
    public static readonly DependencyProperty TransitionProperty = DependencyProperty.Register(
        nameof(Transition),
        typeof(Transition),
        typeof(NavigationView),
        new FrameworkPropertyMetadata(Transition.FadeInWithSlide)
    );

    /// <summary>Identifies the <see cref="FrameMargin"/> dependency property.</summary>
    public static readonly DependencyProperty FrameMarginProperty = DependencyProperty.Register(
        nameof(FrameMargin),
        typeof(Thickness),
        typeof(NavigationView),
        new FrameworkPropertyMetadata(default(Thickness))
    );

    /// <summary>Identifies the <see cref="IsPaneResizable"/> dependency property.</summary>
    public static readonly DependencyProperty IsPaneResizableProperty = DependencyProperty.Register(
        nameof(IsPaneResizable),
        typeof(bool),
        typeof(NavigationView),
        new FrameworkPropertyMetadata(true)
    );

    /// <summary>Identifies the <see cref="PaneResizeMinWidth"/> dependency property.</summary>
    public static readonly DependencyProperty PaneResizeMinWidthProperty = DependencyProperty.Register(
        nameof(PaneResizeMinWidth),
        typeof(double),
        typeof(NavigationView),
        new FrameworkPropertyMetadata(200.0)
    );

    /// <summary>Identifies the <see cref="PaneResizeMaxWidth"/> dependency property.</summary>
    public static readonly DependencyProperty PaneResizeMaxWidthProperty = DependencyProperty.Register(
        nameof(PaneResizeMaxWidth),
        typeof(double),
        typeof(NavigationView),
        new FrameworkPropertyMetadata(500.0)
    );

    /// <summary>
    /// Gets or sets a value indicating whether debugging messages for this control are enabled
    /// </summary>
    public bool EnableDebugMessages
    {
        get => (bool)GetValue(EnableDebugMessagesProperty);
        set => SetValue(EnableDebugMessagesProperty, value);
    }

    /// <inheritdoc/>
    public object? Header
    {
        get => GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }

    /// <inheritdoc/>
    public Visibility HeaderVisibility
    {
        get => (Visibility)GetValue(HeaderVisibilityProperty);
        set => SetValue(HeaderVisibilityProperty, value);
    }

    /// <inheritdoc/>
    public bool AlwaysShowHeader
    {
        get => (bool)GetValue(AlwaysShowHeaderProperty);
        set => SetValue(AlwaysShowHeaderProperty, value);
    }

    /// <inheritdoc/>
    public IList MenuItems => (ObservableCollection<object>)GetValue(MenuItemsProperty);

    /// <inheritdoc/>
    [Bindable(true)]
    public object? MenuItemsSource
    {
        get => GetValue(MenuItemsSourceProperty);
        set
        {
            if (value is null)
            {
                ClearValue(MenuItemsSourceProperty);
            }
            else
            {
                SetValue(MenuItemsSourceProperty, value);
            }
        }
    }

    /// <inheritdoc/>
    public IList FooterMenuItems => (ObservableCollection<object>)GetValue(FooterMenuItemsProperty);

    /// <inheritdoc/>
    [Bindable(true)]
    public object? FooterMenuItemsSource
    {
        get => GetValue(FooterMenuItemsSourceProperty);
        set
        {
            if (value is null)
            {
                ClearValue(FooterMenuItemsSourceProperty);
            }
            else
            {
                SetValue(FooterMenuItemsSourceProperty, value);
            }
        }
    }

    /// <inheritdoc/>
    public object? ContentOverlay
    {
        get => GetValue(ContentOverlayProperty);
        set => SetValue(ContentOverlayProperty, value);
    }

    /// <inheritdoc/>
    public bool IsBackEnabled
    {
        get => (bool)GetValue(IsBackEnabledProperty);
        protected set => SetValue(IsBackEnabledProperty, value);
    }

    /// <inheritdoc/>
    public NavigationViewBackButtonVisible IsBackButtonVisible
    {
        get => (NavigationViewBackButtonVisible)GetValue(IsBackButtonVisibleProperty);
        set => SetValue(IsBackButtonVisibleProperty, value);
    }

    /// <inheritdoc/>
    public bool IsPaneToggleVisible
    {
        get => (bool)GetValue(IsPaneToggleVisibleProperty);
        set => SetValue(IsPaneToggleVisibleProperty, value);
    }

    /// <inheritdoc/>
    public bool IsPaneOpen
    {
        get => (bool)GetValue(IsPaneOpenProperty);
        set => SetValue(IsPaneOpenProperty, value);
    }

    /// <inheritdoc/>
    public bool IsPaneVisible
    {
        get => (bool)GetValue(IsPaneVisibleProperty);
        set => SetValue(IsPaneVisibleProperty, value);
    }

    /// <inheritdoc/>
    public double OpenPaneLength
    {
        get => (double)GetValue(OpenPaneLengthProperty);
        set => SetValue(OpenPaneLengthProperty, value);
    }

    /// <inheritdoc/>
    public double CompactPaneLength
    {
        get => (double)GetValue(CompactPaneLengthProperty);
        set => SetValue(CompactPaneLengthProperty, value);
    }

    /// <inheritdoc/>
    public object? PaneHeader
    {
        get => GetValue(PaneHeaderProperty);
        set => SetValue(PaneHeaderProperty, value);
    }

    /// <inheritdoc/>
    public string? PaneTitle
    {
        get => (string?)GetValue(PaneTitleProperty);
        set => SetValue(PaneTitleProperty, value);
    }

    /// <inheritdoc/>
    public object? PaneFooter
    {
        get => GetValue(PaneFooterProperty);
        set => SetValue(PaneFooterProperty, value);
    }

    /// <inheritdoc/>
    public NavigationViewPaneDisplayMode PaneDisplayMode
    {
        get => (NavigationViewPaneDisplayMode)GetValue(PaneDisplayModeProperty);
        set => SetValue(PaneDisplayModeProperty, value);
    }

    /// <inheritdoc/>
    public AutoSuggestBox? AutoSuggestBox
    {
        get => (AutoSuggestBox?)GetValue(AutoSuggestBoxProperty);
        set => SetValue(AutoSuggestBoxProperty, value);
    }

    /// <inheritdoc/>
    public TitleBar? TitleBar
    {
        get => (TitleBar?)GetValue(TitleBarProperty);
        set => SetValue(TitleBarProperty, value);
    }

    /// <inheritdoc/>
    public BreadcrumbBar? BreadcrumbBar
    {
        get => (BreadcrumbBar?)GetValue(BreadcrumbBarProperty);
        set => SetValue(BreadcrumbBarProperty, value);
    }

    /// <inheritdoc/>
    public ControlTemplate? ItemTemplate
    {
        get => (ControlTemplate?)GetValue(ItemTemplateProperty);
        set => SetValue(ItemTemplateProperty, value);
    }

    /// <inheritdoc/>
    [Bindable(true)]
    [Category("Appearance")]
    public int TransitionDuration
    {
        get => (int)GetValue(TransitionDurationProperty);
        set => SetValue(TransitionDurationProperty, value);
    }

    /// <inheritdoc/>
    public Transition Transition
    {
        get => (Transition)GetValue(TransitionProperty);
        set => SetValue(TransitionProperty, value);
    }

    /// <inheritdoc/>
    public Thickness FrameMargin
    {
        get => (Thickness)GetValue(FrameMarginProperty);
        set => SetValue(FrameMarginProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the navigation pane is resizable.
    /// </summary>
    public bool IsPaneResizable
    {
        get => (bool)GetValue(IsPaneResizableProperty);
        set => SetValue(IsPaneResizableProperty, value);
    }

    /// <summary>
    /// Gets or sets the minimum width for resizing the navigation pane.
    /// </summary>
    public double PaneResizeMinWidth
    {
        get => (double)GetValue(PaneResizeMinWidthProperty);
        set => SetValue(PaneResizeMinWidthProperty, value);
    }

    /// <summary>
    /// Gets or sets the maximum width for resizing the navigation pane.
    /// </summary>
    public double PaneResizeMaxWidth
    {
        get => (double)GetValue(PaneResizeMaxWidthProperty);
        set => SetValue(PaneResizeMaxWidthProperty, value);
    }

    private void OnMenuItemsSource_CollectionChanged(
        object? sender,
        IList collection,
        NotifyCollectionChangedEventArgs e
    )
    {
        if (ReferenceEquals(sender, collection))
        {
            return;
        }

        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                if (e.NewItems is not null)
                {
                    foreach (var item in e.NewItems)
                    {
                        _ = collection.Add(item);
                    }
                }

                break;

            case NotifyCollectionChangedAction.Remove:
                if (e.OldItems is not null && e.NewItems is not null)
                {
                    foreach (var item in e.OldItems)
                    {
                        if (!e.NewItems.Contains(item))
                        {
                            collection.Remove(item);
                        }
                    }
                }

                break;

            case NotifyCollectionChangedAction.Move:
                var moveItem = MenuItems[e.OldStartingIndex];
                collection.RemoveAt(e.OldStartingIndex);
                collection.Insert(e.NewStartingIndex, moveItem);
                break;

            case NotifyCollectionChangedAction.Replace:
                collection.RemoveAt(e.OldStartingIndex);
                if (e.NewItems is not null)
                {
                    collection.Insert(e.OldStartingIndex, e.NewItems[0]);
                }

                break;

            case NotifyCollectionChangedAction.Reset:
                collection.Clear();
                break;
        }
    }

    private void OnMenuItems_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.NewItems is null)
        {
            return;
        }

        UpdateMenuItemsTemplate(e.NewItems);
        AddItemsToDictionaries(e.NewItems);
    }

    private static void OnMenuItemsSourceChanged(DependencyObject? d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not NavigationView navigationView)
        {
            return;
        }

        navigationView.MenuItems.Clear();

        if (e.NewValue is IEnumerable newItemsSource and not string)
        {
            foreach (var item in newItemsSource)
            {
                _ = navigationView.MenuItems.Add(item);
            }
        }
        else if (e.NewValue != null)
        {
            _ = navigationView.MenuItems.Add(e.NewValue);
        }

        if (e.NewValue is INotifyCollectionChanged oc)
        {
            oc.CollectionChanged += (s, e) =>
                navigationView.OnMenuItemsSource_CollectionChanged(oc, navigationView.MenuItems, e);
        }
    }

    private void OnFooterMenuItems_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.NewItems is null)
        {
            return;
        }

        UpdateMenuItemsTemplate(e.NewItems);
        AddItemsToDictionaries(e.NewItems);
    }

    private static void OnFooterMenuItemsSourceChanged(
        DependencyObject? d,
        DependencyPropertyChangedEventArgs e
    )
    {
        if (d is not NavigationView navigationView)
        {
            return;
        }

        navigationView.FooterMenuItems.Clear();

        if (e.NewValue is IEnumerable newItemsSource and not string)
        {
            foreach (var item in newItemsSource)
            {
                _ = navigationView.FooterMenuItems.Add(item);
            }
        }
        else if (e.NewValue != null)
        {
            _ = navigationView.FooterMenuItems.Add(e.NewValue);
        }

        if (e.NewValue is INotifyCollectionChanged oc)
        {
            oc.CollectionChanged += (s, e) =>
                navigationView.OnMenuItemsSource_CollectionChanged(oc, navigationView.FooterMenuItems, e);
        }
    }

    private static void OnPaneDisplayModeChanged(DependencyObject? d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not NavigationView navigationView)
        {
            return;
        }

        navigationView.OnPaneDisplayModeChanged();
    }

    private static void OnItemTemplateChanged(DependencyObject? d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not NavigationView navigationView)
        {
            return;
        }

        navigationView.OnItemTemplateChanged();
    }

    private static void OnIsPaneOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not NavigationView navigationView)
        {
            return;
        }

        if ((bool)e.NewValue == (bool)e.OldValue)
        {
            return;
        }

        if (navigationView.IsPaneOpen)
        {
            navigationView.OnPaneOpened();
        }
        else
        {
            navigationView.OnPaneClosed();
        }

        navigationView.CloseNavigationViewItemMenus();

        navigationView.TitleBar?.SetCurrentValue(
            MarginProperty,
            navigationView.IsPaneOpen ? TitleBarPaneOpenMarginDefault : TitleBarPaneCompactMarginDefault
        );

        UpdateVisualState(navigationView);
    }

    private static void OnTitleBarChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not NavigationView navigationView)
        {
            return;
        }

        if (e.NewValue is null && e.OldValue is TitleBar oldValue)
        {
            navigationView.FrameMargin = new Thickness(0);
            oldValue.Margin = new Thickness(0);

            if (navigationView.AutoSuggestBox?.Margin == AutoSuggestBoxMarginDefault)
            {
                navigationView.AutoSuggestBox.SetCurrentValue(MarginProperty, new Thickness(0));
            }

            return;
        }

        if (e.NewValue is not TitleBar titleBar)
        {
            return;
        }

        navigationView.FrameMargin = FrameMarginDefault;
        titleBar.Margin = TitleBarPaneOpenMarginDefault;

        if (navigationView.AutoSuggestBox?.Margin is { Bottom: 0, Left: 0, Right: 0, Top: 0 })
        {
            navigationView.AutoSuggestBox.SetCurrentValue(MarginProperty, AutoSuggestBoxMarginDefault);
        }
    }

    private static void OnAutoSuggestBoxChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not NavigationView navigationView)
        {
            return;
        }

        if (e.NewValue is null && e.OldValue is AutoSuggestBox oldValue)
        {
            oldValue.SuggestionChosen -= navigationView.AutoSuggestBoxOnSuggestionChosen;
            oldValue.QuerySubmitted -= navigationView.AutoSuggestBoxOnQuerySubmitted;
            return;
        }

        if (e.NewValue is not AutoSuggestBox autoSuggestBox)
        {
            return;
        }

        autoSuggestBox.OriginalItemsSource = navigationView._autoSuggestBoxItems;
        autoSuggestBox.SuggestionChosen += navigationView.AutoSuggestBoxOnSuggestionChosen;
        autoSuggestBox.QuerySubmitted += navigationView.AutoSuggestBoxOnQuerySubmitted;

        if (
            navigationView.TitleBar?.Margin == TitleBarPaneOpenMarginDefault
            && autoSuggestBox.Margin is { Bottom: 0, Left: 0, Right: 0, Top: 0 }
        )
        {
            autoSuggestBox.Margin = AutoSuggestBoxMarginDefault;
        }
    }

    private static void OnBreadcrumbBarChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not NavigationView navigationView)
        {
            return;
        }

        if (e.NewValue is null && e.OldValue is BreadcrumbBar oldValue)
        {
            oldValue.ItemClicked -= navigationView.BreadcrumbBarOnItemClicked;
            {
                return;
            }
        }

        if (e.NewValue is not BreadcrumbBar breadcrumbBar)
        {
            return;
        }

        breadcrumbBar.ItemsSource = navigationView._breadcrumbBarItems;
        breadcrumbBar.ItemTemplate ??=
            UiApplication.Current.TryFindResource("NavigationViewItemDataTemplate") as DataTemplate;
        breadcrumbBar.ItemClicked += navigationView.BreadcrumbBarOnItemClicked;
    }
}
