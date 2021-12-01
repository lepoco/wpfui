// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using WPFUI.Common;

namespace WPFUI.Controls
{
    /// <summary>
    /// Base class for creating new navigations;
    /// </summary>
    public abstract class Navigation : ContentControl, INavigation
    {
        private Action<INavigation, string> _navigated;

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

        /// <inheritdoc/>
        public Frame Frame
        {
            get => GetValue(FrameProperty) as Frame;
            set
            {
                value.Loaded += Frame_OnLoaded;

                SetValue(FrameProperty, value);
            }
        }

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
        public Action<INavigation, string> Navigated
        {
            get => _navigated;
            set => _navigated = value;
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

            Frame.Navigate(element.Instance);

            element.Invoke(this);

            if (null != _navigated)
                _navigated(this, element.Tag);
        }

        private void InactivateElements(string exceptElement)
        {
            foreach (NavItem singleNavItem in Items)
            {
                if (singleNavItem.Tag != exceptElement)
                    singleNavItem.IsActive = false;
            }
        }

        private void Frame_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (sender == null)
                return;

            ((Frame)sender).NavigationUIVisibility = NavigationUIVisibility.Hidden;
            ((Frame)sender).Navigating += Frame_OnNavigating;
        }

        private void Frame_OnNavigating(object sender, NavigatingCancelEventArgs e)
        {
            if (e.Content == null)
                return;

            Frame.NavigationService.RemoveBackEntry();
        }
    }
}
