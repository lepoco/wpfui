// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using WPFUI.Common;

using static System.String;

namespace WPFUI.Controls
{
    /// <summary>
    /// Interaction logic for NavigationFluent.xaml
    /// </summary>
    public partial class NavigationFluent : UserControl
    {
        private Frame _rootFrame;

        private Action _onNavigate = () => { };

        private string
            _currentPage = Empty,
            _pagesFolder = Empty;

        private bool
            _navExpanded = false,
            _isLoading = false;

        public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register("Items",
            typeof(ObservableCollection<NavItem>), typeof(Navigation),
            new PropertyMetadata(new ObservableCollection<NavItem>()));

        public static readonly DependencyProperty FooterProperty = DependencyProperty.Register("Footer",
            typeof(ObservableCollection<NavItem>), typeof(Navigation),
            new PropertyMetadata(new ObservableCollection<NavItem>()));

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

        /// <summary>
        /// Gets or sets the action that will be triggered during navigation.
        /// </summary>
        public Action OnNavigate
        {
            get => _onNavigate;
            set => _onNavigate = value;
        }

        /// <summary>
        /// Gets or sets absolute Namespace in the application files where the <see cref="System.Windows.Controls.Page"/> are located. <c>eg.: MyApp.Views.Pages</c>
        /// </summary>
        public string Catalog
        {
            get => _pagesFolder;
            set => _pagesFolder = value;
        }

        /// <summary>
        /// Gets currently active <see cref="System.Windows.Controls.Page"/> tag.
        /// </summary>
        public string PageNow => _currentPage;

        /// <summary>
        /// Gets or sets the <see cref="System.Windows.Controls.Frame"/> in which the <see cref="System.Windows.Controls.Page"/> will be loaded after navigation.
        /// </summary>
        public Frame Frame
        {
            get => _rootFrame;
            set
            {
                _rootFrame = value;

                _rootFrame.NavigationUIVisibility = NavigationUIVisibility.Hidden;
                _rootFrame.Navigating += FrameOnNavigating;
            }
        }

        public NavigationFluent()
        {
            InitializeComponent();
        }

        public void Flush()
        {
            Items = new ObservableCollection<NavItem>();
            Footer = new ObservableCollection<NavItem>();

            _onNavigate = () => { };
        }

        public void InitializeNavigation(string navigate = "", string activePage = "")
        {
            if (this.Items == null)
                return;

            if (!IsNullOrEmpty(navigate))
                this.Navigate(navigate, true);

            if (!IsNullOrEmpty(activePage))
                for (int i = 0; i < this.Items.Count; i++)
                    if (this.Items[i].Tag == activePage)
                        this.Items[i].IsActive = true;
        }

        /// <summary>
        /// Loads a <see cref="System.Windows.Controls.Page"/> instance into <see cref="Navigation.Frame"/> based on the <see cref="NavItem.Tag"/>.
        /// </summary>
        public void Navigate(string pageTypeName, bool refresh = false)
        {
            if (Items == null || Items.Count == 0 || _rootFrame == null || pageTypeName == _currentPage)
            {
                return;
            }

            for (int i = 0; i < Items.Count; i++)
                if (Items[i].Tag == pageTypeName)
                {
                    Items[i].Action?.Invoke();

                    if (Items[i].Instance == null || refresh)
                    {
                        if (Items[i].Type != null)
                        {
                            Items[i].Instance = Activator.CreateInstance(Items[i].Type);
                        }
                        else if (Items[i].Type == null && !IsNullOrEmpty(_pagesFolder))
                        {
                            //We assume that we will always enter the correct name
                            Type pageType = Type.GetType(_pagesFolder + pageTypeName);

                            if (!refresh && this._rootFrame.Content != null &&
                                this._rootFrame.Content.GetType() == pageType)
                                return;

                            this.Items[i].Instance = Activator.CreateInstance(pageType);
                        }
                        else
                        {
                            //Brake!
                            return;
                        }
                    }

                    _rootFrame.Navigate(Items[i].Instance);
                    Items[i].IsActive = true;

                    if (Items[i].Type != null && this.Items[i].Type.GetMethod("OnNavigationRequest") != null)
                        Items[i].Type.GetMethod("OnNavigationRequest").Invoke(this.Items[i].Instance, null);
                }
                else
                {
                    Items[i].IsActive = false;
                }

            if (Footer != null && this.Footer.Count > 0)
            {
                for (int i = 0; i < this.Footer.Count; i++)
                    if (Footer[i].Tag == pageTypeName)
                    {
                        Footer[i].Action?.Invoke();

                        if (Footer[i].Instance == null || refresh)
                        {
                            if (Footer[i].Type != null)
                            {
                                this.Footer[i].Instance = Activator.CreateInstance(this.Footer[i].Type);
                            }
                            else if (this.Footer[i].Type == null && !IsNullOrEmpty(this._pagesFolder))
                            {
                                //We assume that we will always enter the correct name
                                Type pageType = Type.GetType(this._pagesFolder + pageTypeName);

                                if (!refresh && this._rootFrame.Content != null &&
                                    this._rootFrame.Content.GetType() == pageType)
                                    return;

                                this.Footer[i].Instance = Activator.CreateInstance(pageType);
                            }
                            else
                            {
                                //Brake!
                                return;
                            }
                        }

                        _rootFrame.Navigate(this.Footer[i].Instance);

                        Footer[i].IsActive = true;

                        if (Footer[i].Type != null && Footer[i].Type.GetMethod("OnNavigationRequest") != null)
                        {
                            Footer[i].Type.GetMethod("OnNavigationRequest")?.Invoke(Footer[i].Instance, null);
                        }

                    }
                    else
                    {
                        Footer[i].IsActive = false;
                    }
            }

            _currentPage = pageTypeName;

            _onNavigate();
        }

        private void Button_NavItem(object sender, RoutedEventArgs e)
        {
            Navigate((sender as System.Windows.Controls.Button)?.Tag.ToString());
        }

        private void FrameOnNavigating(object sender, NavigatingCancelEventArgs e)
        {
            if (e.Content == null)
            {
                return;
            }

            _rootFrame.NavigationService.RemoveBackEntry();
        }
    }
}