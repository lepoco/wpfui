// Based on Windows UI Library
// Copyright(c) Microsoft Corporation.All rights reserved.

// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.ComponentModel;
using System.Diagnostics.Tracing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using Wpf.Ui.Animations;

namespace Wpf.Ui.Controls.Navigation;

public class NavigationViewContentPresenter : Frame
{
    /// <summary>
    /// Property for <see cref="TransitionDuration"/>.
    /// </summary>
    public static readonly DependencyProperty TransitionDurationProperty =
        DependencyProperty.Register(nameof(TransitionDuration), typeof(int), typeof(NavigationViewContentPresenter),
            new FrameworkPropertyMetadata(200));

    /// <summary>
    /// Property for <see cref="TransitionType"/>.
    /// </summary>
    public static readonly DependencyProperty TransitionTypeProperty =
        DependencyProperty.Register(nameof(TransitionType), typeof(TransitionType),
            typeof(NavigationViewContentPresenter), new FrameworkPropertyMetadata(TransitionType.FadeInWithSlide));

    /// <summary>
    /// Property for <see cref="IsDynamicScrollViewerEnabled"/>.
    /// </summary>
    public static readonly DependencyProperty IsDynamicScrollViewerEnabledProperty =
        DependencyProperty.Register(nameof(IsDynamicScrollViewerEnabled), typeof(bool),
            typeof(NavigationViewContentPresenter),
            new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsMeasure));

    [Bindable(true), Category("Appearance")]
    public int TransitionDuration
    {
        get => (int)GetValue(TransitionDurationProperty);
        set => SetValue(TransitionDurationProperty, value);
    }

    /// <summary>
    /// Gets or sets type of <see cref="NavigationViewContentPresenter"/> transitions during navigation.
    /// </summary>
    public TransitionType TransitionType
    {
        get => (TransitionType)GetValue(TransitionTypeProperty);
        set => SetValue(TransitionTypeProperty, value);
    }

    /// <summary>
    /// TODO
    /// </summary>
    public bool IsDynamicScrollViewerEnabled
    {
        get => (bool)GetValue(IsDynamicScrollViewerEnabledProperty);
        protected set => SetValue(IsDynamicScrollViewerEnabledProperty, value);
    }

    static NavigationViewContentPresenter()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
                typeof(NavigationViewContentPresenter),
                new FrameworkPropertyMetadata(typeof(NavigationViewContentPresenter)));

        NavigationUIVisibilityProperty.OverrideMetadata(
            typeof(NavigationViewContentPresenter),
            new FrameworkPropertyMetadata(NavigationUIVisibility.Hidden));

        SandboxExternalContentProperty.OverrideMetadata(
            typeof(NavigationViewContentPresenter),
            new FrameworkPropertyMetadata(true));

        JournalOwnershipProperty.OverrideMetadata(
            typeof(NavigationViewContentPresenter),
            new FrameworkPropertyMetadata(JournalOwnership.UsesParentJournal));

        ScrollViewer.CanContentScrollProperty.OverrideMetadata(typeof(Page), new FrameworkPropertyMetadata(true));
    }

    public NavigationViewContentPresenter()
    {
        Navigating += static (sender, eventArgs) =>
        {
            if (eventArgs.Content is null)
                return;

            var self = (NavigationViewContentPresenter)sender;
            self.OnNavigating(eventArgs);
        };

        Navigated += static (sender, eventArgs) =>
        {
            var self = (NavigationViewContentPresenter)sender;

            if (eventArgs.Content is null)
                return;

            self.OnNavigated(eventArgs);
        };
    }

    protected override void OnInitialized(EventArgs e)
    {
        base.OnInitialized(e);

        //I didn't understand something, but why is it necessary?
        Unloaded += static (sender, _) =>
        {
            if (sender is NavigationViewContentPresenter navigator)
            {
                NotifyContentAboutNavigatingFrom(navigator.Content);
            }
        };
    }

    protected override void OnMouseDown(MouseButtonEventArgs e)
    {
        if (e.ChangedButton is MouseButton.XButton1 or MouseButton.XButton2)
        {
            e.Handled = true;
        }

        base.OnMouseDown(e);
    }

    protected virtual void OnNavigating(System.Windows.Navigation.NavigatingCancelEventArgs eventArgs)
    {
        NotifyContentAboutNavigatingTo(eventArgs.Content);

        if (eventArgs.Navigator is not NavigationViewContentPresenter navigator)
            return;

        NotifyContentAboutNavigatingFrom(navigator.Content);
    }

    protected virtual void OnNavigated(NavigationEventArgs eventArgs)
    {
        ApplyTransitionEffectToNavigatedPage(eventArgs.Content);

        if (eventArgs.Content is not DependencyObject dependencyObject)
            return;

        IsDynamicScrollViewerEnabled = ScrollViewer.GetCanContentScroll(dependencyObject);
    }

    private void ApplyTransitionEffectToNavigatedPage(object content)
    {
        if (TransitionDuration < 1)
            return;

        Transitions.ApplyTransition(content, TransitionType, TransitionDuration);
    }

    private static void NotifyContentAboutNavigatingTo(object content)
    {
        if (content is INavigationAware navigationAwareNavigationContent)
            navigationAwareNavigationContent.OnNavigatedTo();

        if (content is INavigableView<object> { ViewModel: INavigationAware navigationAwareNavigableViewViewModel })
            navigationAwareNavigableViewViewModel.OnNavigatedTo();

        if (content is FrameworkElement { DataContext: INavigationAware navigationAwareCurrentContent })
            navigationAwareCurrentContent.OnNavigatedTo();
    }

    private static void NotifyContentAboutNavigatingFrom(object content)
    {
        if (content is INavigationAware navigationAwareNavigationContent)
            navigationAwareNavigationContent.OnNavigatedFrom();

        if (content is INavigableView<object> { ViewModel: INavigationAware navigationAwareNavigableViewViewModel })
            navigationAwareNavigableViewViewModel.OnNavigatedFrom();

        if (content is FrameworkElement { DataContext: INavigationAware navigationAwareCurrentContent })
            navigationAwareCurrentContent.OnNavigatedFrom();
    }
}
