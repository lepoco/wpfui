// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

/* Based on Windows UI Library https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.controls.navigationviewitem?view=winrt-22621 */

using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

/// <summary>
/// Represents the container for an item in a NavigationView control.
/// When needed, it can be used as a normal button with a <see cref="System.Windows.Controls.Primitives.ButtonBase.Click"/> action.
/// </summary>
[TemplatePart(Name = TemplateElementChevronGrid, Type = typeof(Grid))]
[TemplatePart(Name = "PART_CompactFlyout", Type = typeof(Popup))]
public class NavigationViewItem
    : System.Windows.Controls.Primitives.ButtonBase,
        INavigationViewItem,
        IIconControl
{
    protected const string TemplateElementChevronGrid = "PART_ChevronGrid";

    private static readonly DependencyPropertyKey MenuItemsPropertyKey = DependencyProperty.RegisterReadOnly(
        nameof(MenuItems),
        typeof(ObservableCollection<object>),
        typeof(NavigationViewItem),
        new PropertyMetadata(null)
    );

    /// <summary>Identifies the <see cref="MenuItems"/> dependency property.</summary>
    public static readonly DependencyProperty MenuItemsProperty = MenuItemsPropertyKey.DependencyProperty;

    /// <summary>Identifies the <see cref="MenuItemsSource"/> dependency property.</summary>
    public static readonly DependencyProperty MenuItemsSourceProperty = DependencyProperty.Register(
        nameof(MenuItemsSource),
        typeof(object),
        typeof(NavigationViewItem),
        new PropertyMetadata(null, OnMenuItemsSourceChanged)
    );

    /// <summary>Identifies the <see cref="HasMenuItems"/> dependency property.</summary>
    internal static readonly DependencyPropertyKey HasMenuItemsPropertyKey =
        DependencyProperty.RegisterReadOnly(
            nameof(HasMenuItems),
            typeof(bool),
            typeof(NavigationViewItem),
            new PropertyMetadata(false)
        );

    /// <summary>Identifies the <see cref="HasMenuItems"/> dependency property.</summary>
    public static readonly DependencyProperty HasMenuItemsProperty =
        HasMenuItemsPropertyKey.DependencyProperty;

    /// <summary>Identifies the <see cref="IsActive"/> dependency property.</summary>
    public static readonly DependencyProperty IsActiveProperty = DependencyProperty.Register(
        nameof(IsActive),
        typeof(bool),
        typeof(NavigationViewItem),
        new PropertyMetadata(false)
    );

    /// <summary>Identifies the <see cref="IsPaneOpen"/> dependency property.</summary>
    public static readonly DependencyProperty IsPaneOpenProperty = DependencyProperty.Register(
        nameof(IsPaneOpen),
        typeof(bool),
        typeof(NavigationViewItem),
        new PropertyMetadata(false)
    );

    /// <summary>Identifies the <see cref="IsExpanded"/> dependency property.</summary>
    public static readonly DependencyProperty IsExpandedProperty = DependencyProperty.Register(
        nameof(IsExpanded),
        typeof(bool),
        typeof(NavigationViewItem),
        new PropertyMetadata(false)
    );

    /// <summary>Identifies the <see cref="Icon"/> dependency property.</summary>
    public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
        nameof(Icon),
        typeof(IconElement),
        typeof(NavigationViewItem),
        new PropertyMetadata(null, null, IconElement.Coerce)
    );

    /// <summary>Identifies the <see cref="TargetPageTag"/> dependency property.</summary>
    public static readonly DependencyProperty TargetPageTagProperty = DependencyProperty.Register(
        nameof(TargetPageTag),
        typeof(string),
        typeof(NavigationViewItem),
        new PropertyMetadata(string.Empty)
    );

    /// <summary>Identifies the <see cref="TargetPageType"/> dependency property.</summary>
    public static readonly DependencyProperty TargetPageTypeProperty = DependencyProperty.Register(
        nameof(TargetPageType),
        typeof(Type),
        typeof(NavigationViewItem),
        new PropertyMetadata(null)
    );

    /// <summary>Identifies the <see cref="InfoBadge"/> dependency property.</summary>
    public static readonly DependencyProperty InfoBadgeProperty = DependencyProperty.Register(
        nameof(InfoBadge),
        typeof(InfoBadge),
        typeof(NavigationViewItem),
        new PropertyMetadata(null)
    );

    /// <summary>Identifies the <see cref="NavigationCacheMode"/> dependency property.</summary>
    public static readonly DependencyProperty NavigationCacheModeProperty = DependencyProperty.Register(
        nameof(NavigationCacheMode),
        typeof(NavigationCacheMode),
        typeof(NavigationViewItem),
        new FrameworkPropertyMetadata(NavigationCacheMode.Disabled)
    );

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

    /// <summary>
    /// Gets a value indicating whether MenuItems.Count > 0
    /// </summary>
    [Browsable(false)]
    [ReadOnly(true)]
    public bool HasMenuItems
    {
        get => (bool)GetValue(HasMenuItemsProperty);
    }

    /// <inheritdoc />
    [Browsable(false)]
    [ReadOnly(true)]
    public bool IsActive
    {
        get => (bool)GetValue(IsActiveProperty);
        set => SetValue(IsActiveProperty, value);
    }

    /// <inheritdoc />
    [Browsable(false)]
    [ReadOnly(true)]
    public bool IsExpanded
    {
        get => (bool)GetValue(IsExpandedProperty);
        set => SetValue(IsExpandedProperty, value);
    }

    [Browsable(false)]
    [ReadOnly(true)]
    public bool IsPaneOpen
    {
        get => (bool)GetValue(IsPaneOpenProperty);
        set => SetValue(IsPaneOpenProperty, value);
    }

    /// <inheritdoc />
    [Bindable(true)]
    [Category("Appearance")]
    public IconElement? Icon
    {
        get => (IconElement?)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    /// <inheritdoc />
    public string TargetPageTag
    {
        get => (string)GetValue(TargetPageTagProperty);
        set => SetValue(TargetPageTagProperty, value);
    }

    /// <inheritdoc />
    public Type? TargetPageType
    {
        get => (Type?)GetValue(TargetPageTypeProperty);
        set => SetValue(TargetPageTypeProperty, value);
    }

    public InfoBadge? InfoBadge
    {
        get => (InfoBadge?)GetValue(InfoBadgeProperty);
        set => SetValue(InfoBadgeProperty, value);
    }

    /// <inheritdoc/>
    public NavigationCacheMode NavigationCacheMode
    {
        get => (NavigationCacheMode)GetValue(NavigationCacheModeProperty);
        set => SetValue(NavigationCacheModeProperty, value);
    }

    /// <inheritdoc />
    public INavigationViewItem? NavigationViewItemParent { get; set; }

    /// <inheritdoc />
    public bool IsMenuElement { get; set; }

    /// <inheritdoc />
    public string Id { get; }

    protected Grid? ChevronGrid { get; set; }

    private readonly HashSet<INavigationViewItem> _menuItemChildren = [];
    private NavigationView? _subscribedNavigationView;
    private Popup? _compactFlyout;

    private bool UsesCompactFlyout =>
        NavigationView.GetNavigationParent(this)
            is { PaneDisplayMode: NavigationViewPaneDisplayMode.LeftCompact, IsPaneOpen: false }
        && NavigationViewItemParent is null;

    static NavigationViewItem()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(NavigationViewItem),
            new FrameworkPropertyMetadata(typeof(NavigationViewItem))
        );
    }

    public NavigationViewItem()
    {
        Id = Guid.NewGuid().ToString("n");

        Unloaded += (_, _) =>
        {
            if (_compactFlyout is { IsOpen: true })
            {
                _subscribedNavigationView?.CloseCompactFlyout();
            }

            DetachNavigationViewEvents();
        };

        Loaded += (_, _) => InitializeNavigationViewEvents();

        // Initialize the `Items` collection
        var menuItems = new ObservableCollection<object>();
        menuItems.CollectionChanged += OnMenuItems_CollectionChanged;
        SetValue(MenuItemsPropertyKey, menuItems);
    }

    public NavigationViewItem(Type targetPageType)
        : this()
    {
        SetValue(TargetPageTypeProperty, targetPageType);
    }

    public NavigationViewItem(string name, Type targetPageType)
        : this(targetPageType)
    {
        SetValue(ContentProperty, name);
    }

    public NavigationViewItem(string name, SymbolRegular icon, Type targetPageType)
        : this(targetPageType)
    {
        SetValue(ContentProperty, name);
        SetValue(IconProperty, new SymbolIcon { Symbol = icon });
    }

    public NavigationViewItem(string name, SymbolRegular icon, Type targetPageType, IList menuItems)
        : this(name, icon, targetPageType)
    {
        SetValue(MenuItemsSourceProperty, menuItems);
    }

    /// <summary>
    /// Correctly activates
    /// </summary>
    public virtual void Activate(INavigationView navigationView)
    {
        SetCurrentValue(IsActiveProperty, true);

        if (!navigationView.IsPaneOpen && NavigationViewItemParent is not null)
        {
            NavigationViewItemParent.Activate(navigationView);
        }

        if (NavigationViewItemParent is not null)
        {
            if (
                navigationView.IsPaneOpen
                && navigationView.PaneDisplayMode != NavigationViewPaneDisplayMode.Top
            )
            {
                NavigationViewItemParent.IsExpanded = true;
            }
            else
            {
                NavigationViewItemParent.IsExpanded = false;
            }
        }

        if (
            Icon is SymbolIcon symbolIcon
            && navigationView.PaneDisplayMode == NavigationViewPaneDisplayMode.LeftFluent
        )
        {
            symbolIcon.Filled = true;
        }
    }

    /// <summary>
    /// Correctly deactivates
    /// </summary>
    public virtual void Deactivate(INavigationView navigationView)
    {
        SetCurrentValue(IsActiveProperty, false);
        NavigationViewItemParent?.Deactivate(navigationView);

        if (!navigationView.IsPaneOpen && HasMenuItems)
        {
            SetCurrentValue(IsExpandedProperty, false);
        }

        if (
            Icon is SymbolIcon symbolIcon
            && navigationView.PaneDisplayMode == NavigationViewPaneDisplayMode.LeftFluent
        )
        {
            symbolIcon.Filled = false;
        }
    }

    /// <inheritdoc />
    public override void OnApplyTemplate()
    {
        if (_compactFlyout is not null)
        {
            _compactFlyout.SetCurrentValue(Popup.IsOpenProperty, false);
            _compactFlyout.Opened -= OnCompactFlyoutOpened;
            _compactFlyout.Closed -= OnCompactFlyoutClosed;
        }

        base.OnApplyTemplate();

        _compactFlyout = GetTemplateChild("PART_CompactFlyout") as Popup;
        if (_compactFlyout is not null)
        {
            _compactFlyout.Opened += OnCompactFlyoutOpened;
            _compactFlyout.Closed += OnCompactFlyoutClosed;
        }

        if (GetTemplateChild(TemplateElementChevronGrid) is Grid chevronGrid)
        {
            ChevronGrid = chevronGrid;
        }
    }

    /// <inheritdoc />
    protected override void OnInitialized(EventArgs e)
    {
        base.OnInitialized(e);

        if (string.IsNullOrWhiteSpace(TargetPageTag) && Content is not null)
        {
            SetCurrentValue(
                TargetPageTagProperty,
                Content as string ?? Content.ToString()?.ToLower().Trim() ?? string.Empty
            );
        }
    }

    /// <inheritdoc />
    protected override void OnClick()
    {
        if (NavigationView.GetNavigationParent(this) is not { } navigationView)
        {
            return;
        }

        if (UsesCompactFlyout && HasMenuItems)
        {
            bool wasExpanded = IsExpanded;
            if (TargetPageType is not null)
            {
                navigationView.OnNavigationViewItemClick(this);
            }

            if (TargetPageType is null || !wasExpanded)
            {
                navigationView.ToggleCompactFlyout(this);
            }

            base.OnClick();
            return;
        }

        if (HasMenuItems && (navigationView.IsPaneOpen || IsPaneOpen))
        {
            SetCurrentValue(IsExpandedProperty, !IsExpanded);
        }

        if (TargetPageType is not null)
        {
            navigationView.OnNavigationViewItemClick(this);
        }

        base.OnClick();
    }

    /// <summary>
    /// Is called when mouse is clicked down.
    /// </summary>
    protected override void OnMouseDown(MouseButtonEventArgs e)
    {
        if (!HasMenuItems || e.LeftButton != MouseButtonState.Pressed)
        {
            base.OnMouseDown(e);
            return;
        }

        if (NavigationView.GetNavigationParent(this) is not { } navigationView)
        {
            return;
        }

        if (
            !navigationView.IsPaneOpen
            || navigationView.PaneDisplayMode
                is not (NavigationViewPaneDisplayMode.Left or NavigationViewPaneDisplayMode.LeftCompact)
            || ChevronGrid is null
        )
        {
            base.OnMouseDown(e);
            return;
        }

        var mouseOverChevron = ActualWidth < e.GetPosition(this).X + ChevronGrid.ActualWidth;
        if (!mouseOverChevron)
        {
            base.OnMouseDown(e);
            return;
        }

        SetCurrentValue(IsExpandedProperty, !IsExpanded);

        for (int i = 0; i < MenuItems.Count; i++)
        {
            object? menuItem = MenuItems[i];

            if (menuItem is not INavigationViewItem { IsActive: true })
            {
                continue;
            }

            if (IsExpanded)
            {
                Deactivate(navigationView);
            }
            else
            {
                Activate(navigationView);
            }

            break;
        }

        e.Handled = true;
    }

    protected override AutomationPeer OnCreateAutomationPeer()
    {
        return new NavigationViewItemAutomationPeer(this);
    }

    private void OnMenuItems_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        SetValue(HasMenuItemsPropertyKey, MenuItems.Count > 0);

        // Unloading a presenter does not remove an item from the hierarchy. Only collection
        // changes detach parent links, including Reset notifications from MenuItemsSource.
        foreach (INavigationViewItem child in _menuItemChildren)
        {
            if (child.NavigationViewItemParent == this)
            {
                child.NavigationViewItemParent = null;
            }
        }

        _menuItemChildren.Clear();

        foreach (INavigationViewItem item in MenuItems.OfType<INavigationViewItem>())
        {
            item.NavigationViewItemParent = this;
            _ = _menuItemChildren.Add(item);
        }
    }

    private static void OnMenuItemsSourceChanged(DependencyObject? d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not NavigationViewItem navigationViewItem)
        {
            return;
        }

        navigationViewItem.MenuItems.Clear();

        if (e.NewValue is IEnumerable newItemsSource and not string)
        {
            foreach (var item in newItemsSource)
            {
                _ = navigationViewItem.MenuItems.Add(item);
            }
        }
        else if (e.NewValue != null)
        {
            _ = navigationViewItem.MenuItems.Add(e.NewValue);
        }
    }

    private void InitializeNavigationViewEvents()
    {
        DetachNavigationViewEvents();
        if (NavigationView.GetNavigationParent(this) is { } navigationView)
        {
            _subscribedNavigationView = navigationView;
            UpdatePanePresentation();
            navigationView.PaneOpened += OnNavigationPaneChanged;
            navigationView.PaneClosed += OnNavigationPaneChanged;
        }
    }

    private void DetachNavigationViewEvents()
    {
        if (_subscribedNavigationView is { } view)
        {
            view.PaneOpened -= OnNavigationPaneChanged;
            view.PaneClosed -= OnNavigationPaneChanged;
            _subscribedNavigationView = null;
        }
    }

    private void OnNavigationPaneChanged(object sender, RoutedEventArgs e) => UpdatePanePresentation();

    internal void UpdatePanePresentation()
    {
        if (NavigationView.GetNavigationParent(this) is { } view)
        {
            SetCurrentValue(
                IsPaneOpenProperty,
                view.IsPaneOpen
                    || (
                        view.PaneDisplayMode == NavigationViewPaneDisplayMode.LeftCompact
                        && NavigationViewItemParent is not null
                    )
            );
        }
    }

    private void OnCompactFlyoutOpened(object? sender, EventArgs e)
    {
        NavigationView.GetNavigationParent(this)?.RegisterCompactFlyout(this);
        if (_compactFlyout?.Child is FrameworkElement child)
        {
            // Popup has a separate visual tree; preserve navigation and data binding ownership.
            child.SetValue(NavigationView.NavigationParentProperty, NavigationView.GetNavigationParent(this));
            child.DataContext = DataContext;
            _ = child.MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
        }
    }

    private void OnCompactFlyoutClosed(object? sender, EventArgs e)
    {
        bool restoreFocus = _compactFlyout?.Child is UIElement { IsKeyboardFocusWithin: true };
        SetCurrentValue(IsExpandedProperty, false);
        NavigationView.GetNavigationParent(this)?.OnCompactFlyoutClosed(this, restoreFocus);
        if (_compactFlyout?.Child is FrameworkElement child)
        {
            child.ClearValue(NavigationView.NavigationParentProperty);
            child.ClearValue(DataContextProperty);
        }
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        if (
            NavigationView.GetNavigationParent(this)
            is not { PaneDisplayMode: NavigationViewPaneDisplayMode.LeftCompact } view
        )
        {
            base.OnKeyDown(e);
            return;
        }

        // A descendant's key event must only be processed by that descendant.
        if (e.OriginalSource is NavigationViewItem source && source != this)
        {
            return;
        }

        switch (e.Key)
        {
            case Key.Enter:
            case Key.Space:
                if (!e.IsRepeat)
                {
                    OnClick();
                }

                e.Handled = true;
                return;
            case Key.Escape:
                view.CloseCompactFlyout(true);
                e.Handled = true;
                break;
            case Key.Up:
            case Key.Down:
                view.MoveCompactItemFocus(this, e.Key == Key.Up ? -1 : 1);
                e.Handled = true;
                break;
            case Key.Right when HasMenuItems:
                if (UsesCompactFlyout)
                {
                    if (!IsExpanded)
                    {
                        view.ToggleCompactFlyout(this);
                    }
                }
                else
                {
                    SetCurrentValue(IsExpandedProperty, true);
                    _ = Dispatcher.BeginInvoke(
                        () =>
                        {
                            if (IsExpanded && IsKeyboardFocusWithin)
                            {
                                _ = MenuItems
                                    .OfType<NavigationViewItem>()
                                    .FirstOrDefault(item => item.IsVisible && item.IsEnabled)
                                    ?.Focus();
                            }
                        },
                        System.Windows.Threading.DispatcherPriority.Input
                    );
                }

                e.Handled = true;
                break;
            case Key.Left:
                if (IsExpanded)
                {
                    SetCurrentValue(IsExpandedProperty, false);
                }
                else if (NavigationViewItemParent is NavigationViewItem parent)
                {
                    parent.SetCurrentValue(IsExpandedProperty, false);
                    _ = parent.Focus();
                }

                e.Handled = true;
                break;
        }

        base.OnKeyDown(e);
    }
}
