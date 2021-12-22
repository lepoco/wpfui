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
        /// Property for <see cref="Frame"/>.
        /// </summary>
        public static readonly DependencyProperty FrameProperty = DependencyProperty.Register(nameof(Frame),
            typeof(Frame), typeof(Navigation),
            new PropertyMetadata(null));

        /// <summary>
        /// Property for <see cref="Items"/>.
        /// </summary>
        public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register(nameof(Items),
            typeof(ObservableCollection<NavigationItem>), typeof(Navigation),
            new PropertyMetadata(default(ObservableCollection<NavigationItem>), Items_OnChanged));

        /// <summary>
        /// Property for <see cref="Footer"/>.
        /// </summary>
        public static readonly DependencyProperty FooterProperty = DependencyProperty.Register(nameof(Footer),
            typeof(ObservableCollection<NavigationItem>), typeof(Navigation),
            new PropertyMetadata(default(ObservableCollection<NavigationItem>), Footer_OnChanged));

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

        #endregion

        #region Public variables

        /// <inheritdoc/>
        public Frame Frame
        {
            get => GetValue(FrameProperty) as Frame;
            set => SetValue(FrameProperty, value);
        }

        /// <inheritdoc/>
        public ObservableCollection<NavigationItem> Items
        {
            get => GetValue(ItemsProperty) as ObservableCollection<NavigationItem>;
            set => SetValue(ItemsProperty, value);
        }

        /// <inheritdoc/>
        public ObservableCollection<NavigationItem> Footer
        {
            get => GetValue(FooterProperty) as ObservableCollection<NavigationItem>;
            set => SetValue(FooterProperty, value);
        }

        /// <summary>
        /// <see cref="Style"/> for <see cref="Items"/> and <see cref="Footer"/>.
        /// </summary>
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

        /// <inheritdoc/>
        public object Current { get; internal set; } = null;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates new instance of <see cref="INavigation"/> and sets it's default <see cref="FrameworkElement.Loaded"/> event.
        /// </summary>
        protected Navigation()
        {
            Items = new ObservableCollection<NavigationItem>();
            Footer = new ObservableCollection<NavigationItem>();

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
        public void Navigate(string pageTag, bool refresh = false)
        {
            if (Items == null || Items?.Count == 0 || Frame == null || pageTag == PageNow)
                return;

            NavigationItem navigationElement = Items.SingleOrDefault(item => (string)item.Tag == pageTag);

            if (navigationElement != null && navigationElement.IsValid)
            {
                NavigateToElement(navigationElement, refresh);

                return;
            }

            if (Footer == null || Footer.Count == 0)
                return;

            navigationElement = Footer.SingleOrDefault(item => (string)item.Tag == pageTag);

            if (navigationElement != null && navigationElement.IsValid)
                NavigateToElement(navigationElement, refresh);
        }

        #endregion

        #region Private Methods

        private void NavigateToElement(NavigationItem element, bool refresh)
        {
            string pageTag = element.Tag as string;

            if (String.IsNullOrEmpty(pageTag))
            {
                throw new InvalidOperationException("NavigationItem has to have a string Tag.");
            }

            if (pageTag == PageNow && !refresh)
            {
                return;
            }

            if (element.Instance == null || refresh)
            {
                if (element.Type != null)
                {
                    element.Instance = Activator.CreateInstance(element.Type);
                }
                else
                {
                    throw new InvalidOperationException("NavigationItem has to have a Page Type.");
                }
            }

            InactivateElements(pageTag);

            Current = element;
            PageNow = pageTag;

            History.Add(pageTag);
            Frame.Navigate(element.Instance);

            if (element.Instance is INavigable navigable)
            {
                navigable?.OnNavigationRequest(this, element);
            }

            element.IsActive = true;

            RaiseEvent(new RoutedEventArgs(NavigatedEvent, this));
        }

        private void InactivateElements(string exceptElement)
        {
            foreach (NavigationItem singleNavItem in Items)
            {
                if ((string)singleNavItem.Tag != exceptElement)
                    singleNavItem.IsActive = false;
            }

            foreach (NavigationItem singleNavItem in Footer)
            {
                if ((string)singleNavItem.Tag != exceptElement)
                    singleNavItem.IsActive = false;
            }
        }

        #endregion

        #region Handlers

        private void Items_OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (var addedItem in e.NewItems)
                {
                    ((NavigationItem)addedItem).Click += Item_OnClicked;

                    if (ItemStyle != null && ((NavigationItem)addedItem).Style != ItemStyle)
                    {
                        ((NavigationItem)addedItem).Style = ItemStyle;
                    }
                }
            }

            if (e.OldItems == null)
            {
                return;
            }

            foreach (var deletedItem in e.OldItems)
            {
                ((NavigationItem)deletedItem).Click -= Item_OnClicked;
            }
        }

        private void Footer_OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (var addedItem in e.NewItems)
                {
                    ((NavigationItem)addedItem).Click += Item_OnClicked;

                    if (ItemStyle != null && ((NavigationItem)addedItem).Style != ItemStyle)
                    {
                        ((NavigationItem)addedItem).Style = ItemStyle;
                    }
                }
            }

            if (e.OldItems == null)
            {
                return;
            }

            foreach (var deletedItem in e.OldItems)
            {
                ((NavigationItem)deletedItem).Click -= Item_OnClicked;
            }
        }

        private static void Items_OnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not Navigation navigation) return;

            if (navigation.Items == null)
            {
                return;
            }

            foreach (NavigationItem navigationItem in navigation.Items)
            {
                navigationItem.Click += navigation.Item_OnClicked;

                if (navigation.ItemStyle != null && navigationItem.Style != navigation.ItemStyle)
                {
                    navigationItem.Style = navigation.ItemStyle;
                }
            }

            navigation.Items.CollectionChanged += navigation.Items_OnCollectionChanged;
        }

        private static void Footer_OnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not Navigation navigation) return;

            if (navigation.Footer == null)
            {
                return;
            }

            foreach (NavigationItem navigationItem in navigation.Footer)
            {
                navigationItem.Click += navigation.Item_OnClicked;

                if (navigation.ItemStyle != null && navigationItem.Style != navigation.ItemStyle)
                {
                    navigationItem.Style = navigation.ItemStyle;
                }
            }

            navigation.Footer.CollectionChanged += navigation.Footer_OnCollectionChanged;
        }

        private static void ItemStyle_OnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not Navigation navigation) return;

            if (navigation.ItemStyle == null)
            {
                return;
            }

            if (navigation.Items != null)
            {
                foreach (NavigationItem navigationItem in navigation.Items)
                {
                    navigationItem.Style = navigation.ItemStyle;
                }
            }

            if (navigation.Footer != null)
            {
                foreach (NavigationItem navigationItem in navigation.Footer)
                {
                    navigationItem.Style = navigation.ItemStyle;
                }
            }
        }

        private void Item_OnClicked(object sender, RoutedEventArgs e)
        {
            if (sender is not NavigationItem item) return;

            string pageTag = (string)item.Tag;

            if (String.IsNullOrEmpty(pageTag))
            {
                return;
            }

            Navigate(pageTag);
        }

        private void Navigation_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (Frame == null)
            {
                return;
            }

            Frame.NavigationUIVisibility = NavigationUIVisibility.Hidden;
            Frame.Navigating += Frame_OnNavigating;
        }

        private void Frame_OnNavigating(object sender, NavigatingCancelEventArgs e)
        {
            if (e.Content == null)
                return;

            Frame.NavigationService.RemoveBackEntry();
        }

        #endregion
    }
}
