// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

// Based on Windows UI Library
// Copyright(c) Microsoft Corporation.All rights reserved.

using System;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Animation;

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

/// <content>
/// Defines the template parts for the <see cref="NavigationView"/> control
/// </content>
[TemplatePart(
    Name = TemplateElementNavigationViewContentPresenter,
    Type = typeof(NavigationViewContentPresenter)
)]
[TemplatePart(
    Name = TemplateElementMenuItemsItemsControl,
    Type = typeof(System.Windows.Controls.ItemsControl)
)]
[TemplatePart(
    Name = TemplateElementFooterMenuItemsItemsControl,
    Type = typeof(System.Windows.Controls.ItemsControl)
)]
[TemplatePart(Name = TemplateElementBackButton, Type = typeof(System.Windows.Controls.Button))]
[TemplatePart(Name = TemplateElementToggleButton, Type = typeof(System.Windows.Controls.Button))]
[TemplatePart(
    Name = TemplateElementAutoSuggestBoxSymbolButton,
    Type = typeof(System.Windows.Controls.Button)
)]
[TemplatePart(Name = TemplateElementPaneColumn, Type = typeof(System.Windows.Controls.ColumnDefinition))]
[TemplatePart(Name = TemplateElementPaneGrid, Type = typeof(System.Windows.Controls.Grid))]
[TemplatePart(Name = TemplateElementGridSplitter, Type = typeof(System.Windows.Controls.GridSplitter))]
public partial class NavigationView
{
    /// <summary>
    /// Template element represented by the <c>PART_MenuItemsItemsControl</c> name.
    /// </summary>
    private const string TemplateElementNavigationViewContentPresenter =
        "PART_NavigationViewContentPresenter";

    /// <summary>
    /// Template element represented by the <c>PART_MenuItemsItemsControl</c> name.
    /// </summary>
    private const string TemplateElementMenuItemsItemsControl = "PART_MenuItemsItemsControl";

    /// <summary>
    /// Template element represented by the <c>PART_FooterMenuItemsItemsControl</c> name.
    /// </summary>
    private const string TemplateElementFooterMenuItemsItemsControl = "PART_FooterMenuItemsItemsControl";

    /// <summary>
    /// Template element represented by the <c>PART_BackButton</c> name.
    /// </summary>
    private const string TemplateElementBackButton = "PART_BackButton";

    /// <summary>
    /// Template element represented by the <c>PART_ToggleButton</c> name.
    /// </summary>
    private const string TemplateElementToggleButton = "PART_ToggleButton";

    /// <summary>
    /// Template element represented by the <c>PART_AutoSuggestBoxSymbolButton</c> name.
    /// </summary>
    private const string TemplateElementAutoSuggestBoxSymbolButton = "PART_AutoSuggestBoxSymbolButton";

    /// <summary>
    /// Template element represented by the <c>PART_PaneColumn</c> name.
    /// </summary>
    private const string TemplateElementPaneColumn = "PART_PaneColumn";

    /// <summary>
    /// Template element representing the animated navigation pane.
    /// </summary>
    private const string TemplateElementPaneGrid = "PaneGrid";

    /// <summary>
    /// Template element representing the pane resize handle.
    /// </summary>
    private const string TemplateElementGridSplitter = "PART_GridSplitter";

    // Keep the layout target separate from the animated value of PaneGrid.Width.
    private static readonly DependencyProperty PaneWidthProperty = DependencyProperty.Register(
        "PaneWidth",
        typeof(double),
        typeof(NavigationView),
        new PropertyMetadata(double.NaN)
    );

    /// <summary>
    /// Gets or sets the control responsible for rendering the content.
    /// </summary>
    protected NavigationViewContentPresenter NavigationViewContentPresenter { get; set; } = null!;

    /// <summary>
    /// Gets or sets the control located at the top of the pane with left arrow icon.
    /// </summary>
    protected System.Windows.Controls.ItemsControl MenuItemsItemsControl { get; set; } = null!;

    /// <summary>
    /// Gets or sets the control located at the top of the pane with hamburger icon.
    /// </summary>
    protected System.Windows.Controls.ItemsControl FooterMenuItemsItemsControl { get; set; } = null!;

    /// <summary>
    /// Gets or sets the control located at the top of the pane with left arrow icon.
    /// </summary>
    protected System.Windows.Controls.Button? BackButton { get; set; }

    /// <summary>
    /// Gets or sets the control located at the top of the pane with hamburger icon.
    /// </summary>
    protected System.Windows.Controls.Button? ToggleButton { get; set; }

    /// <summary>
    /// Gets or sets the control that is visitable if PaneDisplayMode="Left" and in compact state
    /// </summary>
    protected System.Windows.Controls.Button? AutoSuggestBoxSymbolButton { get; set; }

    /// <summary>
    /// Gets or sets the pane column definition for GridSplitter support.
    /// </summary>
    protected System.Windows.Controls.ColumnDefinition? PaneColumn { get; set; }

    private System.Windows.Controls.Grid? _paneGrid;
    private System.Windows.Controls.GridSplitter? _gridSplitter;
    private VisualStateGroup? _paneStates;
    private double _resizedPaneWidth = double.NaN;
    private double _paneResizeStartWidth = double.NaN;
    private bool _isPaneWidthAnimating;

    private double ExpandedPaneWidth =>
        IsGridSplitterEnabled && !double.IsNaN(_resizedPaneWidth) ? _resizedPaneWidth : OpenPaneLength;

    /// <inheritdoc />
    public override void OnApplyTemplate()
    {
        DetachPaneResizeHandlers();
        base.OnApplyTemplate();

        NavigationViewContentPresenter = GetTemplateChild<NavigationViewContentPresenter>(
            TemplateElementNavigationViewContentPresenter
        );
        MenuItemsItemsControl = GetTemplateChild<System.Windows.Controls.ItemsControl>(
            TemplateElementMenuItemsItemsControl
        );
        FooterMenuItemsItemsControl = GetTemplateChild<System.Windows.Controls.ItemsControl>(
            TemplateElementFooterMenuItemsItemsControl
        );

        MenuItemsItemsControl.SetCurrentValue(
            System.Windows.Controls.ItemsControl.ItemsSourceProperty,
            MenuItems
        );
        FooterMenuItemsItemsControl.SetCurrentValue(
            System.Windows.Controls.ItemsControl.ItemsSourceProperty,
            FooterMenuItems
        );

        if (NavigationViewContentPresenter is not null)
        {
            NavigationViewContentPresenter.Navigated -= OnNavigationViewContentPresenterNavigated;
            NavigationViewContentPresenter.Navigated += OnNavigationViewContentPresenterNavigated;
        }

        if (
            GetTemplateChild(TemplateElementAutoSuggestBoxSymbolButton)
            is System.Windows.Controls.Button autoSuggestBoxSymbolButton
        )
        {
            AutoSuggestBoxSymbolButton = autoSuggestBoxSymbolButton;

            AutoSuggestBoxSymbolButton.Click -= AutoSuggestBoxSymbolButtonOnClick;
            AutoSuggestBoxSymbolButton.Click += AutoSuggestBoxSymbolButtonOnClick;
        }

        if (GetTemplateChild(TemplateElementBackButton) is System.Windows.Controls.Button backButton)
        {
            BackButton = backButton;

            BackButton.Click -= OnBackButtonClick;
            BackButton.Click += OnBackButtonClick;
        }

        if (GetTemplateChild(TemplateElementToggleButton) is System.Windows.Controls.Button toggleButton)
        {
            ToggleButton = toggleButton;

            ToggleButton.Click -= OnToggleButtonClick;
            ToggleButton.Click += OnToggleButtonClick;
        }

        PaneColumn = GetTemplateChild(TemplateElementPaneColumn) as System.Windows.Controls.ColumnDefinition;
        _paneGrid = GetTemplateChild(TemplateElementPaneGrid) as System.Windows.Controls.Grid;
        _gridSplitter = GetTemplateChild(TemplateElementGridSplitter) as System.Windows.Controls.GridSplitter;
        if (_paneGrid is not null && PaneColumn is not null)
        {
            BindingOperations.SetBinding(
                _paneGrid,
                WidthProperty,
                new Binding { Source = this, Path = new PropertyPath(PaneWidthProperty) }
            );
        }
        _paneStates = _paneGrid?.Parent is FrameworkElement root
            ? VisualStateManager
                .GetVisualStateGroups(root)
                .OfType<VisualStateGroup>()
                .FirstOrDefault(group => group.Name == "PaneStates")
            : null;

        if (_paneStates is not null)
        {
            foreach (VisualState state in _paneStates.States)
            {
                if (state.Storyboard is not null)
                {
                    // Templates may share frozen timelines. Change only this instance's storyboards.
                    state.Storyboard = state.Storyboard.Clone();
                    state.Storyboard.Completed += OnPaneAnimationCompleted;
                }
            }
        }

        if (_gridSplitter is not null)
        {
            _gridSplitter.DragStarted += OnPaneResizeStarted;
            _gridSplitter.DragDelta += OnPaneResizeDelta;
            _gridSplitter.DragCompleted += OnPaneResizeCompleted;
            _gridSplitter.AddHandler(Keyboard.KeyDownEvent, new KeyEventHandler(OnPaneSplitterKeyDown), true);
        }

        _isPaneWidthAnimating = false;
        UpdatePaneWidth();
        UpdatePaneSplitter();
    }

    private void DetachPaneResizeHandlers()
    {
        if (_gridSplitter is not null)
        {
            _gridSplitter.DragStarted -= OnPaneResizeStarted;
            _gridSplitter.DragDelta -= OnPaneResizeDelta;
            _gridSplitter.DragCompleted -= OnPaneResizeCompleted;
            _gridSplitter.RemoveHandler(Keyboard.KeyDownEvent, new KeyEventHandler(OnPaneSplitterKeyDown));
        }

        if (_paneStates is not null)
        {
            foreach (VisualState state in _paneStates.States)
            {
                if (state.Storyboard is not null)
                {
                    state.Storyboard.Completed -= OnPaneAnimationCompleted;
                }
            }
        }
    }

    private void UpdatePaneSplitter()
    {
        _gridSplitter?.SetCurrentValue(
            IsEnabledProperty,
            IsGridSplitterEnabled
                && IsPaneOpen
                && !_isPaneWidthAnimating
                && PaneDisplayMode
                    is NavigationViewPaneDisplayMode.Left
                        or NavigationViewPaneDisplayMode.LeftMinimal
                && !double.IsNaN(ExpandedPaneWidth)
        );
    }

    private void UpdatePaneWidthForDisplayMode()
    {
        if (
            PaneDisplayMode
            is not (NavigationViewPaneDisplayMode.Left or NavigationViewPaneDisplayMode.LeftMinimal)
        )
        {
            _paneGrid?.BeginAnimation(WidthProperty, null);
            _isPaneWidthAnimating = false;
        }

        UpdatePaneWidth();
        UpdatePaneSplitter();
    }

    private void UpdatePaneWidth()
    {
        if (_paneGrid is null || PaneColumn is null)
        {
            return;
        }

        PaneColumn.SetCurrentValue(System.Windows.Controls.ColumnDefinition.WidthProperty, GridLength.Auto);
        SetValue(
            PaneWidthProperty,
            PaneDisplayMode is NavigationViewPaneDisplayMode.Left or NavigationViewPaneDisplayMode.LeftMinimal
                ? IsPaneOpen
                    ? ExpandedPaneWidth
                    : 40.0
                : OpenPaneLength
        );
    }

    private void PreparePaneWidthAnimation()
    {
        if (
            _paneGrid is null
            || _paneStates is null
            || _paneStates.CurrentState?.Name == (IsPaneOpen ? "PaneOpen" : "PaneCompact")
        )
        {
            return;
        }

        double currentWidth = _paneGrid.Width;
        if (_paneStates.CurrentState is null)
        {
            currentWidth = IsPaneOpen ? 40.0 : ExpandedPaneWidth;
        }
        if (double.IsNaN(currentWidth))
        {
            currentWidth = _paneGrid.ActualWidth;
        }

        UpdatePaneWidth();
        foreach (VisualState state in _paneStates.States)
        {
            if (state.Storyboard is null)
            {
                continue;
            }

            foreach (DoubleAnimation animation in state.Storyboard.Children.OfType<DoubleAnimation>())
            {
                if (Storyboard.GetTargetName(animation) == "PaneGrid")
                {
                    animation.From = currentWidth;
                    animation.To = state.Name == "PaneOpen" ? ExpandedPaneWidth : 40.0;
                }
                else if (
                    _paneStates.CurrentState is not null
                    && Storyboard.GetTargetProperty(animation)?.Path == nameof(Opacity)
                    && GetTemplateChild(Storyboard.GetTargetName(animation)) is UIElement element
                )
                {
                    // Reverse an interrupted fade from its current opacity, just like the pane width.
                    animation.From =
                        Storyboard.GetTargetName(animation) == TemplateElementAutoSuggestBoxSymbolButton
                        && _paneStates.CurrentState?.Name == "PaneOpen"
                            ? 0.0
                            : element.Opacity;
                }
            }
        }

        _isPaneWidthAnimating = true;
        UpdatePaneSplitter();
    }

    private void OnPaneAnimationCompleted(object? sender, EventArgs e)
    {
        _isPaneWidthAnimating = false;
        UpdatePaneSplitter();
    }

    private void OnPaneResizeStarted(object sender, DragStartedEventArgs e)
    {
        if (
            _paneGrid is null
            || PaneColumn is null
            || !IsGridSplitterEnabled
            || !IsPaneOpen
            || _isPaneWidthAnimating
        )
        {
            return;
        }

        // The splitter owns the column during a drag; the pane stretches inside it.
        _paneResizeStartWidth = _resizedPaneWidth;
        PaneColumn.SetCurrentValue(
            System.Windows.Controls.ColumnDefinition.WidthProperty,
            new GridLength(_paneGrid.ActualWidth + _paneGrid.Margin.Left + _paneGrid.Margin.Right)
        );
        SetValue(PaneWidthProperty, double.NaN);
    }

    private void OnPaneResizeDelta(object sender, DragDeltaEventArgs e) => SaveResizedPaneWidth();

    private void OnPaneSplitterKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Handled && e.Key is Key.Left or Key.Right && _gridSplitter is { IsEnabled: true })
        {
            SaveResizedPaneWidth();
            UpdatePaneWidth();
        }
    }

    private void SaveResizedPaneWidth()
    {
        if (
            _paneGrid is null
            || PaneColumn is null
            || !IsGridSplitterEnabled
            || !IsPaneOpen
            || _isPaneWidthAnimating
        )
        {
            return;
        }

        if (PaneColumn.Width.IsAbsolute)
        {
            _resizedPaneWidth = Math.Max(
                0,
                PaneColumn.Width.Value - _paneGrid.Margin.Left - _paneGrid.Margin.Right
            );
        }
    }

    private void OnPaneResizeCompleted(object sender, DragCompletedEventArgs e)
    {
        if (e.Canceled)
        {
            _resizedPaneWidth = _paneResizeStartWidth;
        }

        UpdatePaneWidth();
    }

    private static void OnOpenPaneLengthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var navigationView = (NavigationView)d;
        navigationView._resizedPaneWidth = double.NaN;
        navigationView.UpdatePaneWidth();
        navigationView.UpdatePaneSplitter();
    }

    private static void OnIsGridSplitterEnabledChanged(
        DependencyObject d,
        DependencyPropertyChangedEventArgs e
    )
    {
        var navigationView = (NavigationView)d;
        navigationView.UpdatePaneWidth();
        navigationView.UpdatePaneSplitter();
    }

    protected T GetTemplateChild<T>(string name)
        where T : DependencyObject
    {
        if (GetTemplateChild(name) is not T dependencyObject)
        {
            throw new ArgumentNullException(name);
        }

        return dependencyObject;
    }
}
