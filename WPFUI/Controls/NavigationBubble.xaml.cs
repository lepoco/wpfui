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

namespace WPFUI.Controls
{
    /// <summary>
    /// Interaction logic for NavigationBubble.xaml
    /// </summary>
    public partial class NavigationBubble : UserControl
    {
        protected Frame _rootFrame;

        protected Action?
            _onNavigate = null;

        protected string
            _currentPage = string.Empty,
            _pagesFolder = string.Empty;

        protected bool
            _navExpanded = false,
            _isLoading = false;

        public static readonly DependencyProperty
            FrameProperty = DependencyProperty.Register("Frame", typeof(Frame), typeof(NavigationBubble)),
            ItemsProperty = DependencyProperty.Register("Items", typeof(ObservableCollection<NavItem>), typeof(NavigationBubble), new PropertyMetadata(new ObservableCollection<NavItem>())),
            FooterProperty = DependencyProperty.Register("Footer", typeof(ObservableCollection<NavItem>), typeof(NavigationBubble), new PropertyMetadata(new ObservableCollection<NavItem>()));

        /// <summary>
        /// Gets or sets the <see cref="System.Windows.Controls.Frame"/> in which the <see cref="System.Windows.Controls.Page"/> will be loaded after navigation.
        /// </summary>
        public Frame Frame
        {
            get => this.GetValue(FrameProperty) as Frame;
            set => this.SetValue(FrameProperty, value);
        }

        /// <summary>
        /// Gets or sets the list of <see cref="NavItem"/> that will be displayed on the menu.
        /// </summary>
        public ObservableCollection<NavItem> Items
        {
            get => this.GetValue(ItemsProperty) as ObservableCollection<NavItem>;
            set => this.SetValue(ItemsProperty, value);
        }

        /// <summary>
        /// Gets or sets the list of <see cref="NavItem"/> which will be displayed at the bottom of the navigation and will not be scrolled.
        /// </summary>
        public ObservableCollection<NavItem> Footer
        {
            get => this.GetValue(FooterProperty) as ObservableCollection<NavItem>;
            set => this.SetValue(FooterProperty, value);
        }

        /// <summary>
        /// Gets or sets the action that will be triggered during navigation.
        /// </summary>
        public Action? OnNavigate
        {
            get => this._onNavigate;
            set => this._onNavigate = value;
        }

        /// <summary>
        /// Gets or sets absolute Namespace in the application files where the <see cref="System.Windows.Controls.Page"/> are located. <c>eg.: MyApp.Views.Pages</c>
        /// </summary>
        public string Catalog
        {
            get { return this._pagesFolder; }
            set { this._pagesFolder = value; }
        }

        /// <summary>
        /// Gets currently active <see cref="System.Windows.Controls.Page"/> tag.
        /// </summary>
        public string PageNow => this._currentPage;

        public NavigationBubble()
        {
            InitializeComponent();
        }

        public void InitializeNavigation(string navigate = "", string activepage = "")
        {
            if (this.Items == null)
                return;

            if (!String.IsNullOrEmpty(navigate))
                this.Navigate(navigate, true);

            if (!String.IsNullOrEmpty(activepage))
                for (int i = 0; i < this.Items.Count; i++)
                    if (this.Items[i].Tag == activepage)
                        this.Items[i].IsActive = true;
        }

        /// <summary>
        /// Loads a <see cref="System.Windows.Controls.Page"/> instance into <see cref="Navigation.Frame"/> based on the <see cref="NavItem.Tag"/>.
        /// </summary>
        public void Navigate(string pageTypeName, bool refresh = false)
        {
            if (this.Items == null || this.Items.Count == 0 || this._rootFrame == null)
                return;

            if (pageTypeName == this._currentPage)
                return;

            for (int i = 0; i < this.Items.Count; i++)
                if (this.Items[i].Tag == pageTypeName)
                {
                    Items[i].Action?.Invoke();

                    if (this.Items[i].Instance == null || refresh)
                    {
                        if (this.Items[i].Type != null)
                        {
                            this.Items[i].Instance = Activator.CreateInstance(this.Items[i].Type);
                        }
                        else if (this.Items[i].Type == null && !string.IsNullOrEmpty(this._pagesFolder))
                        {
                            //We assume that we will always enter the correct name
                            Type pageType = Type.GetType(this._pagesFolder + pageTypeName);

                            if (!refresh && this._rootFrame.Content != null && this._rootFrame.Content.GetType() == pageType)
                                return;

                            this.Items[i].Instance = Activator.CreateInstance(pageType);
                        }
                        else
                        {
                            //Brake!
                            return;
                        }
                    }

                    this._rootFrame.Navigate(this.Items[i].Instance);
                    this.Items[i].IsActive = true;

                    if (this.Items[i].Type != null && this.Items[i].Type.GetMethod("OnNavigationRequest") != null)
                        this.Items[i].Type.GetMethod("OnNavigationRequest").Invoke(this.Items[i].Instance, null);
                }
                else
                {
                    this.Items[i].IsActive = false;
                }

            if (this.Footer != null && this.Footer.Count > 0)
            {
                for (int i = 0; i < this.Footer.Count; i++)
                    if (this.Footer[i].Tag == pageTypeName)
                    {
                        this.Footer[i].Action?.Invoke();

                        if (this.Footer[i].Instance == null || refresh)
                        {
                            if (this.Footer[i].Type != null)
                            {
                                this.Footer[i].Instance = Activator.CreateInstance(this.Footer[i].Type);
                            }
                            else if (this.Footer[i].Type == null && !string.IsNullOrEmpty(this._pagesFolder))
                            {
                                //We assume that we will always enter the correct name
                                Type pageType = Type.GetType(this._pagesFolder + pageTypeName);

                                if (!refresh && this._rootFrame.Content != null && this._rootFrame.Content.GetType() == pageType)
                                    return;

                                this.Footer[i].Instance = Activator.CreateInstance(pageType);
                            }
                            else
                            {
                                //Brake!
                                return;
                            }
                        }

                        this._rootFrame.Navigate(this.Footer[i].Instance);
                        this.Footer[i].IsActive = true;

                        if (this.Footer[i].Type != null && this.Footer[i].Type.GetMethod("OnNavigationRequest") != null)
                            this.Footer[i].Type.GetMethod("OnNavigationRequest").Invoke(this.Footer[i].Instance, null);
                    }
                    else
                    {
                        this.Footer[i].IsActive = false;
                    }
            }

            this._currentPage = pageTypeName;

            if (_onNavigate != null)
                _onNavigate();
        }

        private void Button_NavItem(object sender, RoutedEventArgs e)
        {
            this.Navigate((sender as System.Windows.Controls.Button).Tag.ToString());
        }

        private void FrameOnNavigating(object sender, NavigatingCancelEventArgs e)
        {
            if (e.Content == null)
                return;

            this._rootFrame.NavigationService.RemoveBackEntry();
        }
    }
}
