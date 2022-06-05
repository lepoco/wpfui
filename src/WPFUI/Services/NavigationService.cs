// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using WPFUI.Common;
using WPFUI.Common.Interfaces;
using WPFUI.Controls.Interfaces;
using WPFUI.Mvvm.Contracts;
using WPFUI.Mvvm.Interfaces;

namespace WPFUI.Services;

// NOTE:
// This class is taped combining many weird tricks
// and workarounds. Please don't judge me,
// I'm just a student with a bit of free time

/// <summary>
/// Internal navigation service.
/// </summary>
internal sealed class NavigationService : IDisposable
{
    #region Private properties

    /// <summary>
    /// Whether the current class is disposed.
    /// </summary>
    private bool _disposed = false;

    /// <summary>
    /// Currently navigated page index.
    /// </summary>
    private int _currentPageIndex = -1;

    /// <summary>
    /// Previously navigated page index.
    /// </summary>
    private int _previousPageIndex = -1;

    /// <summary>
    /// Current frame.
    /// </summary>
    private Frame _frame;

    /// <summary>
    /// MVVM page service.
    /// </summary>
    private IPageService _pageService { get; set; }

    /// <summary>
    /// Current <see cref="EventIdentifier"/>.
    /// </summary>
    private long _currentActionIdentifier { get; set; }

    /// <summary>
    /// Identifies current Frame process.
    /// </summary>
    private readonly EventIdentifier _eventIdentifier;

    /// <summary>
    /// <see cref="INavigationItem"/>'s mirror with cached page contents.
    /// </summary>
    private NavigationServiceItem[] _navigationServiceItems;

    #endregion Private properties

    #region Public properties

    /// <summary>
    /// Whether to precache instances after rebuilding.
    /// </summary>
    public bool Precache { get; set; } = false;

    /// <summary>
    /// Transition duration.
    /// </summary>
    public int TransitionDuration { get; set; }

    /// <summary>
    /// Transition type.
    /// </summary>
    public Services.TransitionType TransitionType { get; set; }

    #endregion Public properties

    #region Constructors

    /// <summary>
    /// Creates new instance and prepares internal properties.
    /// </summary>
    public NavigationService()
    {
        _eventIdentifier = new EventIdentifier();
        _navigationServiceItems = new NavigationServiceItem[] { };
    }

    /// <summary>
    /// Control destructor.
    /// </summary>
    ~NavigationService()
    {
        Dispose(false);
    }

    #endregion Constructors

    #region Public methods

    /// <summary>
    /// Navigates the <see cref="Frame"/> based on provided item Id.
    /// </summary>
    /// <param name="pageId">Id of the selected page.</param>
    /// <param name="dataContext">Additional <see cref="FrameworkElement.DataContext"/>.</param>
    /// <returns></returns>
    public bool Navigate(int pageId, object dataContext)
    {
        return NavigateInternal(pageId, dataContext);
    }

    /// <summary>
    /// Navigates the <see cref="Frame"/> based on provided item <see cref="Type"/>.
    /// </summary>
    /// <param name="pageType"><see cref="Type"/> of the selected page.</param>
    /// <param name="dataContext">Additional <see cref="FrameworkElement.DataContext"/>.</param>
    /// <returns></returns>
    public bool Navigate(Type pageType, object dataContext)
    {
        var selectedIndex = -1;

        for (int i = 0; i < _navigationServiceItems.Length; i++)
        {
            if (_navigationServiceItems[i].Type != pageType)
                continue;

            selectedIndex = i;

            break;
        }

        if (selectedIndex < 0)
        {
            if (_pageService == null)
                return false;

            var servicePageInstance = _pageService.GetPage(pageType);

            if (servicePageInstance == null)
                throw new InvalidOperationException($"The {pageType} has not been registered in the {typeof(IPageService)} service.");

            _previousPageIndex = _currentPageIndex;
            _currentPageIndex = -1;

            _currentActionIdentifier = _eventIdentifier.GetNext();

            _frame.Navigate(servicePageInstance);

            return true;
        }

        return NavigateInternal(selectedIndex, dataContext);
    }

    /// <summary>
    /// Navigates the <see cref="Frame"/> based on provided item tag.
    /// </summary>
    /// <param name="pageTag">Tag of the page.</param>
    /// <param name="dataContext">Additional <see cref="FrameworkElement.DataContext"/>.</param>
    /// <returns></returns>
    public bool Navigate(string pageTag, object dataContext)
    {
        var selectedIndex = -1;

        for (int i = 0; i < _navigationServiceItems.Length; i++)
        {
            if (_navigationServiceItems[i].Tag != pageTag)
                continue;

            selectedIndex = i;

            break;
        }

        if (selectedIndex < 0)
            return false;

        return NavigateInternal(selectedIndex, dataContext);
    }

    /// <summary>
    /// Navigates statically outside of the current navigation scope.
    /// </summary>
    /// <param name="frameworkElement"><see cref="FrameworkElement"/> to navigate.</param>
    /// <param name="dataContext">Additional <see cref="FrameworkElement.DataContext"/>.</param>
    public bool NavigateExternal(object frameworkElement, object dataContext)
    {
        if (_frame == null)
            return false;

        if (frameworkElement is not FrameworkElement)
            throw new InvalidOperationException($"Only class derived {typeof(FrameworkElement)} can be used for navigation.");

        _previousPageIndex = _currentPageIndex;
        _currentPageIndex = -1;

        _currentActionIdentifier = _eventIdentifier.GetNext();

        _frame.Navigate(
            frameworkElement,
            new NavigationServiceExtraData
            {
                PageId = -1,
                Cache = false,
                DataContext = dataContext
            });

        return true;
    }

    /// <summary>
    /// Navigates statically outside of the current navigation scope.
    /// </summary>
    /// <param name="frameworkElementUri">Uri of the <see cref="FrameworkElement"/> to navigate.</param>
    /// <param name="dataContext">Additional <see cref="FrameworkElement.DataContext"/>.</param>
    public bool NavigateExternal(Uri frameworkElementUri, object dataContext)
    {
        if (_frame == null)
            return false;

        if (!frameworkElementUri.IsAbsoluteUri)
            throw new InvalidOperationException($"Navigation Uri must be absolute Uri pointing to an element derived from {typeof(FrameworkElement)}.");

        _previousPageIndex = _currentPageIndex;
        _currentPageIndex = -1;

        _currentActionIdentifier = _eventIdentifier.GetNext();

        _frame.Navigate(
            frameworkElementUri,
            new NavigationServiceExtraData
            {
                PageId = -1,
                Cache = false,
                DataContext = dataContext
            });

        return true;
    }

    /// <summary>
    /// Sets DataContext for the selected <see cref="NavigationServiceItem"/> instance.
    /// </summary>
    /// <param name="pageTag">Tag of the page.</param>
    /// <param name="dataContext">Context to set.</param>
    public bool SetContext(string pageTag, object dataContext)
    {
        for (int i = 0; i < _navigationServiceItems.Length; i++)
        {
            if (_navigationServiceItems[i].Tag != pageTag)
                continue;

            if (_navigationServiceItems[i].Instance is not FrameworkElement)
                return false;

            ((FrameworkElement)_navigationServiceItems[i].Instance).DataContext = dataContext;

            return true;
        }

        return false;
    }

    /// <summary>
    /// Sets DataContext for the selected <see cref="NavigationServiceItem"/> instance.
    /// </summary>
    /// <param name="serviceItemId">Selected page Id.</param>
    /// <param name="dataContext">Context to set.</param>
    public bool SetContext(int serviceItemId, object dataContext)
    {
        if (_navigationServiceItems.Length - 1 < serviceItemId)
            return false;

        if (_navigationServiceItems[serviceItemId].Instance is not FrameworkElement)
            return false;

        ((FrameworkElement)_navigationServiceItems[serviceItemId].Instance).DataContext = dataContext;

        return true;
    }

    /// <summary>
    /// Creates mirror of <see cref="INavigationItem"/> based on provided collection of <see cref="INavigationControl"/>'s.
    /// </summary>
    public void UpdateItems(IEnumerable<INavigationControl> mainItems, IEnumerable<INavigationControl> additionalItems)
    {
        var serviceItemCollection = new List<NavigationServiceItem> { };

        foreach (var singleNavigationControl in mainItems)
        {
            if (singleNavigationControl is not INavigationItem navigationItem)
                continue;

            serviceItemCollection.Add(NavigationServiceItem.Create(navigationItem));
        }

        foreach (var singleNavigationControl in additionalItems)
        {
            if (singleNavigationControl is not INavigationItem navigationItem)
                continue;

            serviceItemCollection.Add(NavigationServiceItem.Create(navigationItem));
        }

        _navigationServiceItems = serviceItemCollection.ToArray();

        // Should we precache here? It can be intensive cause the update can be fired multiple times during initialization
        //if (Precache)
        //{
        //}
    }

    /// <summary>
    /// Clears cache stored inside service items.
    /// </summary>
    public void ClearCache()
    {
        foreach (var singleServiceItem in _navigationServiceItems)
            singleServiceItem.Instance = null;
    }

    /// <summary>
    /// Sets currently used <see cref="Frame"/>.
    /// </summary>
    /// <param name="frame">Frame to set.</param>
    public void SetFrame(Frame frame)
    {
        if (frame == null)
            return;

        _frame = frame;

        _frame.NavigationUIVisibility = NavigationUIVisibility.Hidden;

        _frame.Navigating -= OnFrameNavigating; // Unsafe, but doubling can be catastrophic
        _frame.Navigating += OnFrameNavigating;

        _frame.Navigated -= OnFrameNavigated; // Unsafe, but doubling can be catastrophic
        _frame.Navigated += OnFrameNavigated;
    }

    /// <summary>
    /// Sets currently used <see cref="IPageService"/>.
    /// </summary>
    /// <param name="pageService">Service to set.</param>
    public void SetService(IPageService pageService)
    {
        _pageService = pageService;
    }

    /// <summary>
    /// Gets currently used <see cref="IPageService"/>.
    /// </summary>
    public IPageService GetService()
    {
        return _pageService ?? null;
    }

    /// <summary>
    /// Gets currently displayed item tag.
    /// </summary>
    public string GetCurrentTag()
    {
        if (_currentPageIndex < 0)
            return "__external__";

        if (_navigationServiceItems.Length == 0)
            return String.Empty;

        if (_navigationServiceItems.Length - 1 < _currentPageIndex)
            return String.Empty;

        return _navigationServiceItems[_currentPageIndex].Tag;
    }

    /// <summary>
    /// Currently displayed page Id.
    /// </summary>
    public int GetCurrentId()
    {
        return _currentPageIndex;
    }

    /// <summary>
    /// Previously displayed page Id.
    /// </summary>
    public int GetPreviousId()
    {
        return _previousPageIndex;
    }

    #endregion Public methods

    #region Disposing

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(true);

        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// If disposing equals <see langword="true"/>, the method has been called directly or indirectly
    /// by a user's code. Managed and unmanaged resources can be disposed. If disposing equals <see langword="false"/>,
    /// the method has been called by the runtime from inside the finalizer and you should not
    /// reference other objects.
    /// <para>Only unmanaged resources can be disposed.</para>
    /// </summary>
    /// <param name="disposing">If disposing equals <see langword="true"/>, dispose all managed and unmanaged resources.</param>
    private void Dispose(bool disposing)
    {
        if (_disposed)
            return;

        _disposed = true;

        if (!disposing)
            return;

#if DEBUG
        System.Diagnostics.Debug.WriteLine($"INFO | {typeof(NavigationService)} disposed.", "WPFUI.Navigation");
#endif

        _navigationServiceItems = null;
    }

    #endregion Disposing

    #region Internal navigation

    /// <summary>
    /// Navigates internally depending on current state of the service.
    /// </summary>
    /// <param name="serviceItemId">Id of the item to navigate.</param>
    /// <param name="dataContext">Additional <see cref="FrameworkElement.DataContext"/>.</param>
    /// <returns></returns>
    private bool NavigateInternal(int serviceItemId, object dataContext)
    {
        if (!_navigationServiceItems.Any())
            return false;

        _currentActionIdentifier = _eventIdentifier.GetNext();

        if (_navigationServiceItems.Length - 1 < serviceItemId)
            return false;

        // The navigation item is the same, skip the navigation
        if (_currentPageIndex == serviceItemId)
            return false;

        // An empty navigation item may be just a button, but as navigation fails, so return false.
        if (_navigationServiceItems[serviceItemId].Type == null &&
            _navigationServiceItems[serviceItemId].Source == null)
            return false;

        _previousPageIndex = _currentPageIndex;
        _currentPageIndex = serviceItemId;

        if (_pageService != null)
            return NavigateInternalByService(serviceItemId);


        if (!_navigationServiceItems[serviceItemId].Cache)
            return NavigateInternalByItemWithoutCache(serviceItemId, dataContext);

        return NavigateInternalByItemWithCache(serviceItemId, dataContext);
    }

    /// <summary>
    /// Navigates internally without service and with enabled cache.
    /// </summary>
    private bool NavigateInternalByItemWithCache(int serviceItemId, object dataContext)
    {
        if (_frame == null)
            return false;

        if (_navigationServiceItems.Length - 1 < serviceItemId)
            return false;

        // Navigate internally, with cache enabled, instance does exist so reuse it
        if (_navigationServiceItems[serviceItemId].Instance != null)
        {
            // Sometimes a user may want to update the context of a page that is already in the cache.
            if (dataContext != null && _navigationServiceItems[serviceItemId].Instance is FrameworkElement)
                ((FrameworkElement)_navigationServiceItems[serviceItemId].Instance).DataContext = dataContext;

            _frame.Navigate(
                _navigationServiceItems[serviceItemId].Instance,
                new NavigationServiceExtraData
                {
                    PageId = serviceItemId,
                    Cache = true,
                    DataContext = dataContext
                });

#if DEBUG
            System.Diagnostics.Debug.WriteLine(
                $"DEBUG | {_navigationServiceItems[serviceItemId].Tag} navigated internally, with cache by it's instance.");
#endif

            return true;
        }

        // Navigate internally, with cache enabled, instance does not exist so create it using type
        if (_navigationServiceItems[serviceItemId].Type != null)
        {
            _navigationServiceItems[serviceItemId].Instance = CreateFrameworkElementInstance(_navigationServiceItems[serviceItemId].Type, dataContext);

            _frame.Navigate(
                _navigationServiceItems[serviceItemId].Instance,
                new NavigationServiceExtraData
                {
                    PageId = serviceItemId,
                    Cache = true,
                    DataContext = null // DataContext used 
                });

#if DEBUG
            System.Diagnostics.Debug.WriteLine(
                $"DEBUG | {_navigationServiceItems[serviceItemId].Tag} navigated internally, with cache by it's type.");
#endif

            return true;
        }

        // Navigate internally, with cache enabled, instance does not exist so create it using source
        if (_navigationServiceItems[serviceItemId].Source != null)
        {
            _frame.Navigate(
                _navigationServiceItems[serviceItemId].Source,
                new NavigationServiceExtraData
                {
                    PageId = serviceItemId,
                    Cache = true,
                    DataContext = dataContext
                });

#if DEBUG
            System.Diagnostics.Debug.WriteLine(
                $"DEBUG | {_navigationServiceItems[serviceItemId].Tag} navigated internally, with cache by it's source.");
#endif

            return true;
        }

        return false;
    }

    /// <summary>
    /// Navigates internally without service and with cache disabled.
    /// </summary>
    private bool NavigateInternalByItemWithoutCache(int serviceItemId, object dataContext)
    {
        if (_frame == null)
            return false;

        if (_navigationServiceItems.Length - 1 < serviceItemId)
            return false;

        // Navigate internally, without cache, based on type
        if (_navigationServiceItems[serviceItemId].Type != null)
        {
            _frame.Navigate(
                CreateFrameworkElementInstance
                (
                    _navigationServiceItems[serviceItemId].Type,
                    dataContext
                ),
                new NavigationServiceExtraData
                {
                    PageId = serviceItemId,
                    Cache = false,
                    DataContext = null // DataContext set above by activator
                });
#if DEBUG
            System.Diagnostics.Debug.WriteLine(
                $"DEBUG | {_navigationServiceItems[serviceItemId].Tag} navigated internally, without cache by it's type.");
#endif
            return true;
        }

        if (_navigationServiceItems[serviceItemId].Source != null)
        {
            _frame.Navigate(
                _navigationServiceItems[serviceItemId].Source,
                new NavigationServiceExtraData
                {
                    PageId = serviceItemId,
                    Cache = false,
                    DataContext = dataContext
                });

#if DEBUG
            System.Diagnostics.Debug.WriteLine(
                $"DEBUG | {_navigationServiceItems[serviceItemId].Tag} navigated internally, without cache by it's source.");
#endif

            return true;
        }

        // Wait... this should not happen...
        return false;
    }

    private bool NavigateInternalByService(int serviceItemId)
    {
        if (_frame == null)
            return false;

        if (_navigationServiceItems.Length - 1 < serviceItemId)
            return false;

        var servicePageInstance = _pageService.GetPage(_navigationServiceItems[serviceItemId].Type);

        if (servicePageInstance == null)
            throw new InvalidOperationException($"The {_navigationServiceItems[serviceItemId].Type} has not been registered in the {typeof(IPageService)} service.");

        _frame.Navigate(servicePageInstance);

        return true;
    }

    #endregion Internal navigation

    #region Instance management

    /// <summary>
    /// Tries to create an instance from the selected page type.
    /// </summary>
    private FrameworkElement CreateFrameworkElementInstance(Type pageType, object dataContext)
    {
        return NavigationServiceActivator.CreateInstance(pageType, dataContext);
    }

    #endregion Instance management

    #region Frame events

    /// <summary>
    /// Event triggered when the frame has already loaded the view, if the page uses the Cache, Content of the Frame should be saved.
    /// </summary>
    private void OnFrameNavigated(object sender, NavigationEventArgs e)
    {
        if (_frame == null)
            return;

        if (_frame.CanGoBack)
            _frame.RemoveBackEntry();

        if (_frame.NavigationService.CanGoBack)
            _frame.NavigationService?.RemoveBackEntry();

        if (TransitionDuration > 0 && e.Content != null)
            TransitionService.ApplyTransition(e.Content, TransitionType, TransitionDuration);

        // If we are using the MVVM model,
        // do not perform internal operations on DataContext and Instances.
        if (_pageService != null)
        {
            // Instance and datacontext determined by the service, notify and leave
            NotifyFrameContentAboutEnter();

            return;
        }

        if (e.ExtraData is not NavigationServiceExtraData extraData)
        {
            // Instance determined by the frame, context not provided, notify and leave
            NotifyFrameContentAboutEnter();

            return;
        }

        if (!_currentActionIdentifier.Equals(_currentActionIdentifier))
        {
            // Only god knows what's broken, but notify anyway and then leave.
            NotifyFrameContentAboutEnter();

            return;
        }

        // DataContext provided by the frame extra data, set.
        if (extraData.DataContext != null && _frame.Content is FrameworkElement)
        {
            ((FrameworkElement)_frame.Content).DataContext = extraData.DataContext;

            if (extraData.DataContext is IViewModel)
                ((IViewModel)extraData.DataContext).OnMounted((FrameworkElement)_frame.Content);
        }

        if (!extraData.Cache)
        {
            // Instance determined by the frame, context set from extra data, but without cache, notify and leave
            NotifyFrameContentAboutEnter();

            return;
        }

        // We make sure that pageId exists, if it is wrong, the fault lies earlier.
        if (_navigationServiceItems.Length - 1 < extraData.PageId || extraData.PageId < 0)
        {
            // Only god knows what's broken, but notify anyway and then leave.
            NotifyFrameContentAboutEnter();

            return;
        }

        // If an instance already exists, do not overwrite it.
        if (_navigationServiceItems[extraData.PageId].Instance != null)
        {
            // Instance determined by the frame, context set from extra data, with cache, but instance cached, notify and leave
            NotifyFrameContentAboutEnter();

            return;
        }

        // Finally, the navigation took place internally,
        // the context was set from extra data, the cache has to be saved,
        // so we save it, notify it and this is the end of the method
        _navigationServiceItems[extraData.PageId].Instance = _frame.Content;

        NotifyFrameContentAboutEnter();
    }

    /// <summary>
    /// Event fired when Frame received a request to navigate.
    /// </summary>
    private void OnFrameNavigating(object sender, NavigatingCancelEventArgs e)
    {
        if (_frame == null)
            return;

        NotifyFrameContentAboutLeave();

        switch (e.NavigationMode)
        {
            case NavigationMode.Back:
                e.Cancel = true;

                if (_currentPageIndex > 0)
                    Navigate(_currentPageIndex - 1, null);
                break;

            case NavigationMode.Forward:
                e.Cancel = true;

                if (_currentPageIndex < _navigationServiceItems.Length - 1)
                    Navigate(_currentPageIndex + 1, null);
                break;
        }
    }

    /// <summary>
    /// Notifies <see cref="Frame"/> content about being navigated.
    /// </summary>
    private void NotifyFrameContentAboutEnter()
    {
        if (_frame == null)
            return;

        if (_frame.Content is INavigationAware)
            ((INavigationAware)_frame.Content).OnNavigatedTo();

        if (_frame.Content is FrameworkElement && ((FrameworkElement)_frame.Content).DataContext is INavigationAware)
            ((INavigationAware)((FrameworkElement)_frame.Content).DataContext).OnNavigatedTo();
    }

    /// <summary>
    /// Notifies <see cref="Frame"/> content about leaving the navigation context.
    /// </summary>
    private void NotifyFrameContentAboutLeave()
    {
        if (_frame == null)
            return;

        if (_frame.Content is INavigationAware)
            ((INavigationAware)_frame.Content).OnNavigatedFrom();

        if (_frame.Content is FrameworkElement && ((FrameworkElement)_frame.Content).DataContext is INavigationAware)
            ((INavigationAware)((FrameworkElement)_frame.Content).DataContext).OnNavigatedFrom();
    }

    #endregion Frame events

    #region Preache

    /// <summary>
    /// Precaches instances of the navigation items.
    /// </summary>
    private void PrecacheItems()
    {
        if (DesignerHelper.IsInDesignMode)
            return;

        if (_pageService != null)
            return;
    }

    #endregion
}
