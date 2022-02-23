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
using System.Windows.Navigation;
using WPFUI.Controls.Interfaces;

namespace WPFUI.Controls
{
    // TODO: It's still a disgusting mix. Requirements are: preview in the designer, and the ability to add items with XAML.

    /// <summary>
    /// Base class for creating new navigation controls.
    /// </summary>
    public abstract class Navigation : Control, INavigation
    {
        #region Dependencies

        /// <summary>
        /// Property for <see cref="CurrentPageId"/>.
        /// </summary>
        public static readonly DependencyProperty CurrentPageIdProperty = DependencyProperty.Register(
            nameof(CurrentPageId),
            typeof(int), typeof(Navigation),
            new PropertyMetadata(0));

        /// <summary>
        /// Property for <see cref="PreviousPageId"/>.
        /// </summary>
        public static readonly DependencyProperty PreviousPageIdProperty = DependencyProperty.Register(
            nameof(PreviousPageId),
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

        /// <summary>
        /// Property for <see cref="ItemStyle"/>.
        /// </summary>
        public static readonly DependencyProperty ItemStyleProperty = DependencyProperty.Register(nameof(ItemStyle),
            typeof(Style), typeof(Navigation),
            new PropertyMetadata(null, ItemStyle_OnChanged));

        /// <summary>
        /// Routed event for <see cref="Navigated"/>.
        /// </summary>
        public static readonly RoutedEvent NavigatedEvent = EventManager.RegisterRoutedEvent(
            nameof(Navigated), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Navigation));

        /// <summary>
        /// Routed event for <see cref="NavigatedForward"/>.
        /// </summary>
        public static readonly RoutedEvent NavigatedForwardEvent = EventManager.RegisterRoutedEvent(
            nameof(NavigatedForward), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Navigation));

        /// <summary>
        /// Routed event for <see cref="NavigatedBackward"/>.
        /// </summary>
        public static readonly RoutedEvent NavigatedBackwardEvent = EventManager.RegisterRoutedEvent(
            nameof(NavigatedBackward), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Navigation));

        #endregion

        #region Public variables

        /// <inheritdoc/>
        public int CurrentPageId
        {
            get => (int)GetValue(CurrentPageIdProperty);
            set => SetValue(CurrentPageIdProperty, value);
        }

        /// <inheritdoc/>
        public int PreviousPageId
        {
            get => (int)GetValue(PreviousPageIdProperty);
            set => SetValue(PreviousPageIdProperty, value);
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
        public Style ItemStyle
        {
            get => GetValue(ItemStyleProperty) as Style;
            set => SetValue(ItemStyleProperty, value);
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
        public event RoutedEventHandler Navigated
        {
            add => AddHandler(NavigatedEvent, value);
            remove => RemoveHandler(NavigatedEvent, value);
        }

        /// <summary>
        /// Event triggered when navigated forward.
        /// </summary>
        public event RoutedEventHandler NavigatedForward
        {
            add => AddHandler(NavigatedForwardEvent, value);
            remove => RemoveHandler(NavigatedForwardEvent, value);
        }

        /// <summary>
        /// Event triggered when navigated backward.
        /// </summary>
        public event RoutedEventHandler NavigatedBackward
        {
            add => AddHandler(NavigatedBackwardEvent, value);
            remove => RemoveHandler(NavigatedBackwardEvent, value);
        }

        /// <inheritdoc/>
        public object Current { get; internal set; } = null;

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
                for (int i = 0; i < Items.Count; i++)
                    Items[i].Instance = null;

            if (Footer != null)
                for (int i = 0; i < Footer.Count; i++)
                    Footer[i].Instance = null;
        }

        /// <inheritdoc/>
        public void Navigate(string pageTag, bool refresh = false)
        {
            if (Items == null || Items?.Count == 0 || Frame == null || pageTag == PageNow)
                return;

            //NavigationItem navigationElement = Items.SingleOrDefault(item => (string)item.Tag == pageTag);

            INavigationItem navigationElement = null;

            for (int i = 0; i < Items.Count; i++)
            {
                if ((string)Items[i].Tag == pageTag)
                {
                    PreviousPageId = CurrentPageId;
                    CurrentPageId = i;
                    navigationElement = Items[i];

                    break;
                }
            }

            if (navigationElement is { IsValid: true })
            {
                NavigateToElement(navigationElement, refresh);

                return;
            }

            if (Footer == null || Footer.Count == 0)
                return;

            navigationElement = Footer.SingleOrDefault(item => (string)item.Tag == pageTag);

            if (navigationElement is { IsValid: true })
                NavigateToElement(navigationElement, refresh);
        }

        #endregion

        #region Private Methods

        private void NavigateToElement(INavigationItem element, bool refresh)
        {
            string pageTag = element.Tag as string;

            if (String.IsNullOrEmpty(pageTag))
                throw new InvalidOperationException("NavigationItem has to have a string Tag.");

            if (pageTag == PageNow && !refresh) return;

            if (element.Instance == null || refresh)
            {
                if (element.Type == null)
                    throw new InvalidOperationException("NavigationItem has to have a Page Type.");

                element.Instance = Activator.CreateInstance(element.Type);
            }

            InactivateElements(pageTag);

            Current = element;
            PageNow = pageTag;

            History.Add(pageTag);
            Frame.Navigate(element.Instance);

            if (element.Instance is INavigable navigable)
                navigable?.OnNavigationRequest(this, element);

            element.IsActive = true;

            RaiseEvent(new RoutedEventArgs(NavigatedEvent, this));

            RaiseEvent(CurrentPageId > PreviousPageId
                ? new RoutedEventArgs(NavigatedForwardEvent, this)
                : new RoutedEventArgs(NavigatedBackwardEvent, this));
        }

        private void InactivateElements(string exceptElement)
        {
            foreach (INavigationItem singleNavItem in Items)
                if ((string)singleNavItem.Tag != exceptElement)
                    singleNavItem.IsActive = false;

            foreach (INavigationItem singleNavItem in Footer)
                if ((string)singleNavItem.Tag != exceptElement)
                    singleNavItem.IsActive = false;
        }

        #endregion

        #region Handlers

        private void Items_OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (var addedItem in e.NewItems)
                {
                    ((INavigationItem)addedItem).Click += Item_OnClicked;

                    if (ItemStyle != null && ((INavigationItem)addedItem).Style != ItemStyle)
                        ((INavigationItem)addedItem).Style = ItemStyle;
                }
            }

            if (e.OldItems == null) return;

            foreach (var deletedItem in e.OldItems)
                ((INavigationItem)deletedItem).Click -= Item_OnClicked;
        }

        private void Footer_OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (var addedItem in e.NewItems)
                {
                    ((INavigationItem)addedItem).Click += Item_OnClicked;

                    if (ItemStyle != null && ((INavigationItem)addedItem).Style != ItemStyle)
                        ((INavigationItem)addedItem).Style = ItemStyle;
                }
            }

            if (e.OldItems == null) return;

            foreach (var deletedItem in e.OldItems)
                ((INavigationItem)deletedItem).Click -= Item_OnClicked;
        }

        private static void Items_OnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // TODO: Fix Navigation direct class reference
            if (d is not Navigation navigation) return;

            if (navigation.Items == null) return;

            foreach (var navigationItem in navigation.Items)
            {
                navigationItem.Click += navigation.Item_OnClicked;

                if (navigation.ItemStyle != null && navigationItem.Style != navigation.ItemStyle)
                    navigationItem.Style = navigation.ItemStyle;
            }

            navigation.Items.CollectionChanged += navigation.Items_OnCollectionChanged;
        }

        private static void Footer_OnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not Navigation navigation) return;

            if (navigation.Footer == null) return;

            foreach (var navigationItem in navigation.Footer)
            {
                navigationItem.Click += navigation.Item_OnClicked;

                if (navigation.ItemStyle != null && navigationItem.Style != navigation.ItemStyle)
                    navigationItem.Style = navigation.ItemStyle;
            }

            navigation.Footer.CollectionChanged += navigation.Footer_OnCollectionChanged;
        }

        private static void ItemStyle_OnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not INavigation navigation) return;

            if (navigation.ItemStyle == null) return;

            if (navigation.Items != null)
                foreach (var navigationItem in navigation.Items)
                    navigationItem.Style = navigation.ItemStyle;

            if (navigation.Footer != null)
                foreach (var navigationItem in navigation.Footer)
                    navigationItem.Style = navigation.ItemStyle;
        }

        private void Item_OnClicked(object sender, RoutedEventArgs e)
        {
            if (sender is not INavigationItem item) return;

            string pageTag = (string)item.Tag;

            if (String.IsNullOrEmpty(pageTag)) return;

            Navigate(pageTag);
        }

        private void Navigation_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (Frame == null) return;

            Frame.NavigationUIVisibility = NavigationUIVisibility.Hidden;
            Frame.Navigating += Frame_OnNavigating;
        }

        private void Frame_OnNavigating(object sender, NavigatingCancelEventArgs e)
        {
            if (e.Content == null) return;

            Frame.NavigationService.RemoveBackEntry();

            // TODO: Nothing is more permanent than temporary fixes... However, navigate using internal methods
            if (e.NavigationMode == NavigationMode.Back)
                e.Cancel = true;
        }

        #endregion
    }
}