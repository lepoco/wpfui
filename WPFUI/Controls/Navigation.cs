// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Navigation;
using WPFUI.Common;

namespace WPFUI.Controls
{
    /// <summary>
    /// Base class for creating new navigation controls.
    /// </summary>
    public abstract class Navigation : ContentControl, INavigation, IAddChild
    {
        // TODO
        private class NavigationItemsCollection : Collection<NavigationItem>
        {
            public NavigationItemsCollection(Navigation parent)
            {
                _parent = parent;
            }

            protected override void InsertItem(int index, NavigationItem navigationItem)
            {
                base.InsertItem(index, navigationItem);

                _parent.AddLogicalChild(navigationItem);
                _parent.AddVisualChild(navigationItem);
                _parent.InvalidateMeasure();
            }

            protected override void SetItem(int index, NavigationItem navigationItem)
            {
                NavigationItem currentNavigationItem = Items[index];

                if (navigationItem != currentNavigationItem)
                {
                    base.SetItem(index, navigationItem);

                    // remove old item visual and logical links
                    _parent.RemoveVisualChild(currentNavigationItem);
                    _parent.RemoveLogicalChild(currentNavigationItem);

                    // add new item visual and logical links
                    _parent.AddLogicalChild(navigationItem);
                    _parent.AddVisualChild(navigationItem);
                    _parent.InvalidateMeasure();
                }
            }

            protected override void RemoveItem(int index)
            {
                NavigationItem currentNavigationItem = this[index];
                base.RemoveItem(index);

                // remove old item visual and logical links
                _parent.RemoveVisualChild(currentNavigationItem);
                _parent.RemoveLogicalChild(currentNavigationItem);
                _parent.InvalidateMeasure();
            }

            protected override void ClearItems()
            {
                int count = Count;
                if (count > 0)
                {
                    for (int i = 0; i < count; i++)
                    {
                        NavigationItem currentNavigationItem = this[i];
                        _parent.RemoveVisualChild(currentNavigationItem);
                        _parent.RemoveLogicalChild(currentNavigationItem);
                    }
                    _parent.InvalidateMeasure();
                }

                base.ClearItems();
            }


            // Ref to a visual/logical ToolBarTray parent
            private readonly Navigation _parent;
        }

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
            typeof(ObservableCollection<NavItem>), typeof(Navigation),
            new PropertyMetadata(new ObservableCollection<NavItem>() { }));

        /// <summary>
        /// Property for <see cref="Footer"/>.
        /// </summary>
        public static readonly DependencyProperty FooterProperty = DependencyProperty.Register(nameof(Footer),
            typeof(ObservableCollection<NavItem>), typeof(Navigation),
            new PropertyMetadata(new ObservableCollection<NavItem>() { }));

        /// <summary>
        /// Routed event for <see cref="Navigated"/>.
        /// </summary>
        public static readonly RoutedEvent NavigatedEvent = EventManager.RegisterRoutedEvent(
            nameof(Navigated), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Navigation));

        /// <summary>
        /// Navigation history containing pages tags.
        /// </summary>
        public List<string> History { get; set; } = new List<string>() { };

        /// <inheritdoc/>
        public Frame Frame
        {
            get => GetValue(FrameProperty) as Frame;
            set
            {
                SetValue(FrameProperty, value);
            }
        }

        //private NavigationItemsCollection _itemsCollection = null;

        //public Collection<NavigationItem> Items
        //{
        //    get
        //    {
        //        if (_itemsCollection == null)
        //            _itemsCollection = new NavigationItemsCollection(this);

        //        return _itemsCollection;
        //    }
        //}

        //private NavigationItemsCollection _footerCollection = null;

        //public Collection<NavigationItem> Footer
        //{
        //    get
        //    {
        //        if (_footerCollection == null)
        //            _footerCollection = new NavigationItemsCollection(this);

        //        return _footerCollection;
        //    }
        //}

        /// <summary>
        /// Gets or sets the list of <see cref="NavItem"/> that will be displayed on the menu.
        /// </summary>
        public ObservableCollection<NavItem> Items
        {
            get => GetValue(ItemsProperty) as ObservableCollection<NavItem>;
            set => SetValue(ItemsProperty, value);
        }

        /// <summary>
        /// Gets or sets the list of <see cref="NavItem"/> which will be displayed at the bottom of the navigation and will not be scrolled.
        /// </summary>
        public ObservableCollection<NavItem> Footer
        {
            get => GetValue(FooterProperty) as ObservableCollection<NavItem>;
            set => SetValue(FooterProperty, value);
        }

        /// <inheritdoc/>
        public string PageNow { get; internal set; } = String.Empty;

        /// <summary>
        /// Namespace containing the pages.
        /// </summary>
        public string Namespace { get; set; } = String.Empty;

        /// <inheritdoc/>
        public event RoutedEventHandler Navigated
        {
            add => AddHandler(NavigatedEvent, value);
            remove => RemoveHandler(NavigatedEvent, value);
        }

        /// <summary>
        /// Creates new instance of <see cref="INavigation"/> and sets it's default <see cref="FrameworkElement.Loaded"/> event.
        /// </summary>
        public Navigation()
        {
            Loaded += Navigation_Loaded;
        }

        /// <inheritdoc/>
        public void Flush()
        {
            Items.Clear();
            Footer.Clear();

            Items = new ObservableCollection<NavItem>();
            Footer = new ObservableCollection<NavItem>();

            PageNow = String.Empty;
        }

        /// <inheritdoc/>
        public void Navigate(string pageTypeName, bool refresh = false)
        {
            if (Items == null || Items.Count == 0 || Frame == null || pageTypeName == PageNow)
                return;

            NavItem navigationElement = Items.SingleOrDefault(item => item.Tag == pageTypeName);

            if (navigationElement != null && navigationElement.IsValid)
            {
                NavigateToElement(navigationElement, refresh);

                return;
            }

            if (Footer == null || Footer.Count == 0)
                return;

            navigationElement = Footer.SingleOrDefault(item => item.Tag == pageTypeName);

            if (navigationElement != null && navigationElement.IsValid)
                NavigateToElement(navigationElement, refresh);
        }

        private void Navigation_Loaded(object sender, RoutedEventArgs e)
        {
            if (Frame == null)
            {
                return;
            }

#if DEBUG
            System.Diagnostics.Debug.WriteLine("Navigation loaded");
#endif

            Frame.NavigationUIVisibility = NavigationUIVisibility.Hidden;
            Frame.Navigating += Frame_OnNavigating;
        }

        private void NavigateToElement(NavItem element, bool refresh)
        {
            if (element.Tag == PageNow && !refresh)
            {
                return;
            }

            if (element.Instance == null || refresh)
            {
                if (element.Type != null)
                {
                    element.Instance = Activator.CreateInstance(element.Type);
                }
                else if (element.Type == null && !String.IsNullOrEmpty(Namespace))
                {
                    // TODO: Fix this stuff

                    //We assume that we will always enter the correct name
                    Type pageType = Type.GetType(Namespace + element.Tag);

                    if (!refresh && Frame.Content != null &&
                        Frame.Content.GetType() == pageType)
                        return;

                    element.Instance = Activator.CreateInstance(pageType);
                }
                else
                {
                    // Something went wrong...
                    return;
                }
            }

            InactivateElements(element.Tag);

            History.Add(element.Tag);

            PageNow = element.Tag;

            Frame.Navigate(element.Instance);

            element.Invoke(this);

            RaiseEvent(new RoutedEventArgs(NavigatedEvent, this));
        }

        private void InactivateElements(string exceptElement)
        {
            foreach (NavItem singleNavItem in Items)
            {
                if (singleNavItem.Tag != exceptElement)
                    singleNavItem.IsActive = false;
            }

            foreach (NavItem singleNavItem in Footer)
            {
                if (singleNavItem.Tag != exceptElement)
                    singleNavItem.IsActive = false;
            }
        }

        private void Frame_OnNavigating(object sender, NavigatingCancelEventArgs e)
        {
            if (e.Content == null)
                return;

            Frame.NavigationService.RemoveBackEntry();
        }
    }
}
