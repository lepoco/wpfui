// Based on Windows UI Library
// Copyright(c) Microsoft Corporation.All rights reserved.

// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

#nullable enable

using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Navigation;
using Wpf.Ui.Animations;
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Controls.Interfaces;
using Wpf.Ui.Mvvm.Contracts;

namespace Wpf.Ui.Controls.Navigation;

public partial class NavigationView
{
    private IServiceProvider? _serviceProvider = null;

    private IPageService? _pageService = null;

    private readonly List<string> _journal = new();

    private int _currentIndexInJournal = 0;

    /// <inheritdoc />
    public bool CanGoBack
        => _journal.Count > 1;

    /// <inheritdoc />
    public void SetPageService(IPageService pageService)
        => _pageService = pageService;

    /// <inheritdoc />
    public void SetServiceProvider(IServiceProvider serviceProvider)
        => _serviceProvider = serviceProvider;

    /// <inheritdoc />
    public bool Navigate(Type pageType)
        => Navigate(pageType, null!);

    /// <inheritdoc />
    public bool Navigate(Type pageType, object dataContext)
    {
        if (MenuItems is IEnumerable enumerableMenuItems)
            foreach (var singleMenuItem in enumerableMenuItems)
                if (singleMenuItem is NavigationViewItem singleNavigationViewItem)
                    if (singleNavigationViewItem?.TargetPageType != null && singleNavigationViewItem?.TargetPageType == pageType)
                        return NavigateInternal(singleNavigationViewItem, dataContext);

        if (FooterMenuItems is IEnumerable enumerableFooterMenuItems)
            foreach (var singleMenuItem in enumerableFooterMenuItems)
                if (singleMenuItem is NavigationViewItem singleNavigationViewItem)
                    if (singleNavigationViewItem?.TargetPageType != null && singleNavigationViewItem?.TargetPageType == pageType)
                        return NavigateInternal(singleNavigationViewItem, dataContext);

        return false;
    }

    /// <inheritdoc />
    public bool Navigate(string pageIdOrTargetTag)
        => Navigate(pageIdOrTargetTag, null!);

    /// <inheritdoc />
    public bool Navigate(string pageIdOrTargetTag, object dataContext)
    {
        if (MenuItems is IEnumerable enumerableMenuItems)
            foreach (var singleMenuItem in enumerableMenuItems)
                if (singleMenuItem is NavigationViewItem singleNavigationViewItem)
                    if (singleNavigationViewItem.Id == pageIdOrTargetTag || singleNavigationViewItem?.TargetPageTag == pageIdOrTargetTag)
                        return NavigateInternal(singleNavigationViewItem, dataContext);

        if (FooterMenuItems is IEnumerable enumerableFooterMenuItems)
            foreach (var singleMenuItem in enumerableFooterMenuItems)
                if (singleMenuItem is NavigationViewItem singleNavigationViewItem)
                    if (singleNavigationViewItem.Id == pageIdOrTargetTag || singleNavigationViewItem?.TargetPageTag == pageIdOrTargetTag)
                        return NavigateInternal(singleNavigationViewItem, dataContext);

        return false;
    }

    /// <inheritdoc />
    public bool GoForward()
    {
        if (_journal.Count <= 1)
            return false;

        _currentIndexInJournal += 1;

        if (_currentIndexInJournal > _journal.Count - 1)
            return false;

        return Navigate(_journal[_currentIndexInJournal], null!);
    }

    /// <inheritdoc />
    public bool GoBack()
    {
        if (_journal.Count <= 1)
            return false;

        _currentIndexInJournal -= 1;

        if (_currentIndexInJournal < 0)
            return false;

        return Navigate(_journal[_currentIndexInJournal], null!);
    }

    /// <inheritdoc />
    public void ClearJournal()
    {
        _journal.Clear();
        _currentIndexInJournal = 0;
    }

    private bool NavigateInternal(INavigationViewItem viewItem, object? dataContext)
    {
        if (viewItem == SelectedItem)
            return false;

        UpdateJournal(viewItem);

#if DEBUG
        System.Diagnostics.Debug.WriteLine($"DEBUG | {viewItem.Id} - {viewItem.TargetPageTag ?? "NO_TAG"} | NAVIGATED");
#endif

        RenderSelectedItemContent(viewItem, dataContext);

        SelectedItem = viewItem;

        UpdateSelectionForMenuItems();
        OnSelectionChanged();

        return true;
    }

    private void UpdateJournal(INavigationViewItem viewItem)
    {
#if DEBUG
        System.Diagnostics.Debug.WriteLine($"JOURNAL INDEX {_currentIndexInJournal}");
        if (_journal.Count > 0)
            System.Diagnostics.Debug.WriteLine($"JOURNAL LAST ELEMENT {_journal[_journal.Count - 1]}");
#endif

        if (_journal.Count == 0)
        {
            _currentIndexInJournal = 0;
            _journal.Add(viewItem.Id);

            return;
        }

        // TODO: Fix at last position

        if (_currentIndexInJournal == _journal.Count - 1)
        {
            _journal.Add(viewItem.Id);

            _currentIndexInJournal++;
        }

        if (_journal.Count > 20)
            _journal.RemoveAt(0);
    }

    private void RenderSelectedItemContent(INavigationViewItem viewItem, object? dataContext)
    {
        if (_serviceProvider != null)
        {
            if (viewItem.TargetPageType == null)
                return;

            UpdateContent(_serviceProvider.GetService(viewItem.TargetPageType) ?? null!, dataContext);

            return;
        }

        if (_pageService != null)
        {
            if (viewItem.TargetPageType == null)
                return;

            UpdateContent(_pageService.GetPage(viewItem.TargetPageType) ?? null!, dataContext);

            return;
        }

        if (viewItem.TargetPageType == null)
            return;

        var pageInstance = NavigationViewActivator.CreateInstance(viewItem.TargetPageType);

        if (pageInstance == null)
            return;

        UpdateContent(pageInstance, dataContext);
    }

    private void UpdateContent(object? content, object? dataContext)
    {
        if (NavigationViewContentPresenter == null)
            return;

        NotifyContentAboutNavigatingFrom(NavigationViewContentPresenter?.Content ?? null);

        if (dataContext != null && content is FrameworkElement frameworkViewContent)
            frameworkViewContent.DataContext = dataContext;

        NavigationViewContentPresenter!.Navigate(content);
    }

    protected virtual void OnNavigationViewContentPresenterNavigated(object sender, NavigationEventArgs e)
    {
        if (sender is not NavigationViewContentPresenter contentPresenter)
            return;

        NotifyContentAboutNavigatingTo(contentPresenter?.Content ?? null);

        ApplyTransitionEffectToNavigatedPage(contentPresenter);
    }

    private void NotifyContentAboutNavigatingFrom(object? content)
    {
        if (content is INavigationAware navigationAwareNavigationContent)
            navigationAwareNavigationContent.OnNavigatedFrom();

        if (content is INavigableView<object> navigableView && navigableView.ViewModel is INavigationAware navigationAwareNavigableViewViewModel)
            navigationAwareNavigableViewViewModel.OnNavigatedFrom();

        if (content is FrameworkElement { DataContext: INavigationAware navigationAwareCurrentContent })
            navigationAwareCurrentContent.OnNavigatedFrom();
    }

    private void NotifyContentAboutNavigatingTo(object? content)
    {
        if (content is INavigationAware navigationAwareNavigationContent)
            navigationAwareNavigationContent.OnNavigatedTo();

        if (content is INavigableView<object> navigableView && navigableView.ViewModel is INavigationAware navigationAwareNavigableViewViewModel)
            navigationAwareNavigableViewViewModel.OnNavigatedTo();

        if (content is FrameworkElement { DataContext: INavigationAware navigationAwareCurrentContent })
            navigationAwareCurrentContent.OnNavigatedTo();
    }

    private void ApplyTransitionEffectToNavigatedPage(NavigationViewContentPresenter contentPresenter)
    {
        if (TransitionDuration < 1)
            return;

        if (contentPresenter.Content == null)
            return;

        Transitions.ApplyTransition(contentPresenter.Content, TransitionType, TransitionDuration);
    }

    private void UpdateSelectionForMenuItems()
    {
        if (MenuItems is IEnumerable enumerableMenuItems)
        {
            foreach (var singleMenuItem in enumerableMenuItems)
            {
                if (singleMenuItem is not NavigationViewItem navigationViewItem)
                    continue;

                navigationViewItem.IsActive = navigationViewItem == SelectedItem;

                if (navigationViewItem.MenuItems is IEnumerable enumerableSubMenuItems)
                {
                    foreach (var singleSubMenuItem in enumerableSubMenuItems)
                    {
                        if (singleSubMenuItem is not NavigationViewItem navigationViewSubItem)
                            continue;

                        navigationViewSubItem.IsActive = navigationViewSubItem == SelectedItem;
                    }
                }
            }
        }

        if (FooterMenuItems is IEnumerable enumerableFooterMenuItems)
        {
            foreach (var singleFooterMenuItem in enumerableFooterMenuItems)
            {
                if (singleFooterMenuItem is not NavigationViewItem navigationViewItem)
                    continue;

                navigationViewItem.IsActive = navigationViewItem == SelectedItem;

                if (navigationViewItem.MenuItems is IEnumerable enumerableSubMenuItems)
                {
                    foreach (var singleSubMenuItem in enumerableSubMenuItems)
                    {
                        if (singleSubMenuItem is not NavigationViewItem navigationViewSubItem)
                            continue;

                        navigationViewSubItem.IsActive = navigationViewSubItem == SelectedItem;
                    }
                }
            }
        }
    }
}
