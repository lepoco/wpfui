// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

// ReSharper disable once CheckNamespace
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Threading;
using ORGButton = System.Windows.Controls.Button;

namespace Wpf.Ui.Controls;

/// <summary>
/// Represents a control that allows a user to pick a time value.
/// </summary>
[TemplatePart(Name = PartPopup, Type = typeof(Popup))]
[TemplatePart(Name = PartHoursList, Type = typeof(ListBox))]
[TemplatePart(Name = PartMinutesList, Type = typeof(ListBox))]
[TemplatePart(Name = PartAmPmList, Type = typeof(ListBox))]
[TemplatePart(Name = PartAcceptButton, Type = typeof(ORGButton))]
[TemplatePart(Name = PartDismissButton, Type = typeof(ORGButton))]
[TemplatePart(Name = PartHoursUpButton, Type = typeof(ORGButton))]
[TemplatePart(Name = PartHoursDownButton, Type = typeof(ORGButton))]
[TemplatePart(Name = PartMinutesUpButton, Type = typeof(ORGButton))]
[TemplatePart(Name = PartMinutesDownButton, Type = typeof(ORGButton))]
[TemplatePart(Name = PartAmPmUpButton, Type = typeof(ORGButton))]
[TemplatePart(Name = PartAmPmDownButton, Type = typeof(ORGButton))]
[TemplatePart(Name = PartPickerSelectionGrid, Type = typeof(Grid))]
public class TimePicker : ButtonBase
{
    private const string PartPopup = "PART_Popup";
    private const string PartHoursList = "PART_HoursList";
    private const string PartMinutesList = "PART_MinutesList";
    private const string PartAmPmList = "PART_AmPmList";
    private const string PartAcceptButton = "PART_AcceptButton";
    private const string PartDismissButton = "PART_DismissButton";
    private const string PartHoursUpButton = "PART_HoursUpButton";
    private const string PartHoursDownButton = "PART_HoursDownButton";
    private const string PartMinutesUpButton = "PART_MinutesUpButton";
    private const string PartMinutesDownButton = "PART_MinutesDownButton";
    private const string PartAmPmUpButton = "PART_AmPmUpButton";
    private const string PartAmPmDownButton = "PART_AmPmDownButton";
    private const string PartPickerSelectionGrid = "PickerSelectionGrid";

    /// <summary>
    /// Defines the number of times the item source is repeated to create the illusion of infinite scrolling.
    /// </summary>
    private const int InfiniteScrollRepeatCount = 3;

    private Popup? _popup;
    private ListBox? _hoursList;
    private ListBox? _minutesList;
    private ListBox? _amPmList;
    private ORGButton? _acceptButton;
    private ORGButton? _dismissButton;
    private ORGButton? _hoursUpButton;
    private ORGButton? _hoursDownButton;
    private ORGButton? _minutesUpButton;
    private ORGButton? _minutesDownButton;
    private ORGButton? _amPmUpButton;
    private ORGButton? _amPmDownButton;
    private Grid? _pickerSelectionGrid;
    private ScrollViewer? _amPmScrollViewer;
    private ScrollViewer? _hoursScrollViewer;
    private ScrollViewer? _minutesScrollViewer;
    private TimeSpan _temporaryTime;
    private bool _isUpdatingSelection; // Flag to prevent re-entrant event handling while programmatically updating selection.
    private bool _isScrolling; // Flag to prevent selection changes during programmatic scrolling (e.g., in infinite scroll jump).

    private DispatcherTimer? _hoursDebounceTimer;
    private DispatcherTimer? _minutesDebounceTimer;
    private DispatcherOperation? _pendingCentering;
    private bool _isSynchronizingTime;

    /// <summary>Initializes a new instance of the <see cref="TimePicker"/> class.</summary>
    public TimePicker()
    {
        Unloaded += (_, _) =>
        {
            StopScrolling();
            _popup?.SetCurrentValue(Popup.IsOpenProperty, false);
        };
    }

    /// <summary>Identifies the <see cref="Header"/> dependency property.</summary>
    public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
        nameof(Header),
        typeof(object),
        typeof(TimePicker),
        new PropertyMetadata(null)
    );

    /// <summary>Identifies the <see cref="Time"/> dependency property.</summary>
    public static readonly DependencyProperty TimeProperty = DependencyProperty.Register(
        nameof(Time),
        typeof(TimeSpan),
        typeof(TimePicker),
        new FrameworkPropertyMetadata(TimeSpan.Zero, OnTimeChanged, CoerceTime)
    );

    /// <summary>Identifies the <see cref="SelectedTime"/> dependency property.</summary>
    public static readonly DependencyProperty SelectedTimeProperty = DependencyProperty.Register(
        nameof(SelectedTime),
        typeof(TimeSpan?),
        typeof(TimePicker),
        new FrameworkPropertyMetadata(
            null,
            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
            OnSelectedTimeChanged
        )
    );

    /// <summary>Identifies the <see cref="MinuteIncrement"/> dependency property.</summary>
    public static readonly DependencyProperty MinuteIncrementProperty = DependencyProperty.Register(
        nameof(MinuteIncrement),
        typeof(int),
        typeof(TimePicker),
        new PropertyMetadata(1, OnMinuteIncrementChanged)
    );

    /// <summary>Identifies the <see cref="ClockIdentifier"/> dependency property.</summary>
    public static readonly DependencyProperty ClockIdentifierProperty = DependencyProperty.Register(
        nameof(ClockIdentifier),
        typeof(ClockIdentifier),
        typeof(TimePicker),
        new PropertyMetadata(ClockIdentifier.Clock24Hour, OnClockIdentifierChanged)
    );

    public static readonly DependencyProperty AcceptButtonTextProperty = DependencyProperty.Register(
        nameof(AcceptButtonText),
        typeof(string),
        typeof(TimePicker),
        new PropertyMetadata("OK")
    );

    public string AcceptButtonText
    {
        get => (string)GetValue(AcceptButtonTextProperty);
        set => SetValue(AcceptButtonTextProperty, value);
    }

    public static readonly DependencyProperty DismissButtonTextProperty = DependencyProperty.Register(
        nameof(DismissButtonText),
        typeof(string),
        typeof(TimePicker),
        new PropertyMetadata("Cancel")
    );

    /// <summary>Identifies the <see cref="PlaceholderText"/> dependency property.</summary>
    public static readonly DependencyProperty PlaceholderTextProperty = DependencyProperty.Register(
        nameof(PlaceholderText),
        typeof(string),
        typeof(TimePicker),
        new PropertyMetadata(string.Empty)
    );

    public string DismissButtonText
    {
        get => (string)GetValue(DismissButtonTextProperty);
        set => SetValue(DismissButtonTextProperty, value);
    }

    /// <summary>
    /// Gets or sets the text shown before a time has been selected.
    /// </summary>
    public string PlaceholderText
    {
        get => (string)GetValue(PlaceholderTextProperty);
        set => SetValue(PlaceholderTextProperty, value);
    }

    /// <summary>
    /// Gets or sets the content for the control's header.
    /// </summary>
    public object? Header
    {
        get => GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }

    /// <summary>
    /// Gets or sets the time currently set in the time picker.
    /// </summary>
    public TimeSpan Time
    {
        get => (TimeSpan)GetValue(TimeProperty);
        set => SetValue(TimeProperty, value);
    }

    /// <summary>
    /// Gets or sets the time currently selected in the time picker. This is a nullable version of <see cref="Time"/>.
    /// </summary>
    public TimeSpan? SelectedTime
    {
        get => (TimeSpan?)GetValue(SelectedTimeProperty);
        set => SetValue(SelectedTimeProperty, value);
    }

    /// <summary>
    /// Gets or sets a value that indicates the time increments shown in the minute picker.
    /// The value must be a divisor of 60.
    /// </summary>
    public int MinuteIncrement
    {
        get => (int)GetValue(MinuteIncrementProperty);
        set => SetValue(MinuteIncrementProperty, value);
    }

    /// <summary>
    /// Gets or sets the clock system to use (12-hour or 24-hour).
    /// </summary>
    public ClockIdentifier ClockIdentifier
    {
        get => (ClockIdentifier)GetValue(ClockIdentifierProperty);
        set => SetValue(ClockIdentifierProperty, value);
    }

    /// <inheritdoc />
    public override void OnApplyTemplate()
    {
        StopScrolling();
        if (_popup != null)
        {
            _popup.Closed -= OnPopupClosed;
            _popup.CustomPopupPlacementCallback = null;
            _popup.SetCurrentValue(Popup.IsOpenProperty, false);
        }
        // Detach old event handlers to prevent memory leaks
        if (_acceptButton != null)
        {
            _acceptButton.Click -= OnAcceptButtonClick;
        }

        if (_dismissButton != null)
        {
            _dismissButton.Click -= OnDismissButtonClick;
        }

        if (_hoursUpButton != null)
        {
            _hoursUpButton.Click -= OnHoursUpButtonClick;
        }
        if (_hoursDownButton != null)
        {
            _hoursDownButton.Click -= OnHoursDownButtonClick;
        }
        if (_minutesUpButton != null)
        {
            _minutesUpButton.Click -= OnMinutesUpButtonClick;
        }
        if (_minutesDownButton != null)
        {
            _minutesDownButton.Click -= OnMinutesDownButtonClick;
        }
        if (_amPmUpButton != null)
            _amPmUpButton.Click -= OnAmPmUpButtonClick;
        if (_amPmDownButton != null)
            _amPmDownButton.Click -= OnAmPmDownButtonClick;

        if (_hoursList != null)
        {
            _hoursList.SelectionChanged -= OnHourChanged;
        }

        if (_minutesList != null)
        {
            _minutesList.SelectionChanged -= OnMinuteChanged;
        }

        if (_amPmList != null)
        {
            _amPmList.SelectionChanged -= OnAmPmChanged;
        }

        if (_hoursScrollViewer != null)
        {
            _hoursScrollViewer.ScrollChanged -= OnHoursScrollChanged;
        }

        if (_minutesScrollViewer != null)
        {
            _minutesScrollViewer.ScrollChanged -= OnMinutesScrollChanged;
        }

        _hoursDebounceTimer?.Stop();
        _minutesDebounceTimer?.Stop();
        _hoursScrollViewer = null;
        _minutesScrollViewer = null;
        _amPmScrollViewer = null;

        base.OnApplyTemplate();

        // Get template parts
        _popup = GetTemplateChild(PartPopup) as Popup;
        _hoursList = GetTemplateChild(PartHoursList) as ListBox;
        _minutesList = GetTemplateChild(PartMinutesList) as ListBox;
        _amPmList = GetTemplateChild(PartAmPmList) as ListBox;
        _acceptButton = GetTemplateChild(PartAcceptButton) as ORGButton;
        _dismissButton = GetTemplateChild(PartDismissButton) as ORGButton;
        _hoursUpButton = GetTemplateChild(PartHoursUpButton) as ORGButton;
        _hoursDownButton = GetTemplateChild(PartHoursDownButton) as ORGButton;
        _minutesUpButton = GetTemplateChild(PartMinutesUpButton) as ORGButton;
        _minutesDownButton = GetTemplateChild(PartMinutesDownButton) as ORGButton;
        _amPmUpButton = GetTemplateChild(PartAmPmUpButton) as ORGButton;
        _amPmDownButton = GetTemplateChild(PartAmPmDownButton) as ORGButton;
        _pickerSelectionGrid = GetTemplateChild(PartPickerSelectionGrid) as Grid;

        // Populate lists
        PopulateHours();
        PopulateMinutes();
        PopulateAmPm();

        // Attach new event handlers
        if (_acceptButton != null)
        {
            _acceptButton.Click += OnAcceptButtonClick;
        }

        if (_dismissButton != null)
        {
            _dismissButton.Click += OnDismissButtonClick;
        }

        if (_hoursUpButton != null)
        {
            _hoursUpButton.Click += OnHoursUpButtonClick;
        }
        if (_hoursDownButton != null)
        {
            _hoursDownButton.Click += OnHoursDownButtonClick;
        }
        if (_minutesUpButton != null)
        {
            _minutesUpButton.Click += OnMinutesUpButtonClick;
        }
        if (_minutesDownButton != null)
        {
            _minutesDownButton.Click += OnMinutesDownButtonClick;
        }
        if (_amPmUpButton != null)
            _amPmUpButton.Click += OnAmPmUpButtonClick;
        if (_amPmDownButton != null)
            _amPmDownButton.Click += OnAmPmDownButtonClick;

        if (_hoursList != null)
        {
            _hoursList.SelectionChanged += OnHourChanged;
        }

        if (_minutesList != null)
        {
            _minutesList.SelectionChanged += OnMinuteChanged;
        }

        if (_amPmList != null)
        {
            _amPmList.SelectionChanged += OnAmPmChanged;
        }

        _amPmScrollViewer = FindVisualChild<ScrollViewer>(_amPmList);
        if (_popup != null)
        {
            _popup.SetCurrentValue(Popup.PlacementProperty, PlacementMode.Custom);
            _popup.CustomPopupPlacementCallback = PlacePopup;
            _popup.Closed += OnPopupClosed;
        }
    }

    /// <inheritdoc />
    protected override void OnClick()
    {
        base.OnClick();
        OpenPopup();
    }

    private void OpenPopup()
    {
        if (_popup == null)
        {
            return;
        }

        StopScrolling();
        _temporaryTime = SelectedTime ?? TimeSpan.Zero;
        UpdateListSelection();
        _popup.SetCurrentValue(Popup.IsOpenProperty, true);
        QueueCenterSelection();
    }

    private void QueueCenterSelection()
    {
        _pendingCentering?.Abort();
        _pendingCentering = Dispatcher.BeginInvoke(
            DispatcherPriority.Input,
            () =>
            {
                if (_popup is not { IsOpen: true })
                {
                    return;
                }

                _popup.Child?.UpdateLayout();
                SetupScrollViewers();
                ScrollToSelectedIndex(_hoursList, _hoursScrollViewer);
                ScrollToSelectedIndex(_minutesList, _minutesScrollViewer);
                CenterAmPmSelection();
            }
        );
    }

    private CustomPopupPlacement[] PlacePopup(Size popupSize, Size targetSize, Point offset)
    {
        // The selection area excludes the footer. Its center includes the popup border.
        double selectionCenter = 181;
        if (_pickerSelectionGrid is { ActualHeight: > 0 } && _popup?.Child is UIElement child)
        {
            selectionCenter = _pickerSelectionGrid
                .TranslatePoint(new Point(0, _pickerSelectionGrid.ActualHeight / 2), child)
                .Y;
        }

        // WPF supplies the callback sizes in device pixels. The template geometry
        // above is in DIPs, so use the popup presentation source to convert it.
        Matrix toDevice =
            PresentationSource.FromVisual(_popup?.Child ?? this)?.CompositionTarget?.TransformToDevice
            ?? Matrix.Identity;
        selectionCenter = toDevice.Transform(new Point(0, selectionCenter)).Y;
        Point deviceOffset = toDevice.Transform(offset);
        double targetCenter = targetSize.Height / 2;
        // WPF still performs its normal screen-edge adjustment for custom placements.
        return new[]
        {
            new CustomPopupPlacement(
                new Point(
                    (targetSize.Width - popupSize.Width) / 2 + deviceOffset.X,
                    targetCenter - selectionCenter + deviceOffset.Y
                ),
                PopupPrimaryAxis.Vertical
            ),
        };
    }

    private void OnPopupClosed(object? sender, EventArgs e) => StopScrolling();

    private void StopScrolling()
    {
        _hoursDebounceTimer?.Stop();
        _minutesDebounceTimer?.Stop();
        _pendingCentering?.Abort();
        _isScrolling = false;
    }

    private void PopulateHours()
    {
        if (_hoursList == null)
        {
            return;
        }

        var singleSet = new List<string>();
        int count = ClockIdentifier == ClockIdentifier.Clock24Hour ? 24 : 12;
        int start = ClockIdentifier == ClockIdentifier.Clock24Hour ? 0 : 1;

        for (int i = 0; i < count; i++)
        {
            var value = start + i;
            singleSet.Add(
                ClockIdentifier == ClockIdentifier.Clock24Hour ? value.ToString("D2") : value.ToString()
            );
        }

        var repeatedSet = new List<string>();
        for (int i = 0; i < GetRepeatCount(singleSet.Count); i++)
        {
            repeatedSet.AddRange(singleSet);
        }

        _hoursList.SetCurrentValue(ItemsControl.ItemsSourceProperty, repeatedSet);
    }

    private void PopulateMinutes()
    {
        if (_minutesList == null)
        {
            return;
        }

        var singleSet = new List<string>();
        for (int i = 0; i < 60; i += MinuteIncrement)
        {
            singleSet.Add(i.ToString("D2"));
        }

        var repeatedSet = new List<string>();
        for (int i = 0; i < GetRepeatCount(singleSet.Count); i++)
        {
            repeatedSet.AddRange(singleSet);
        }

        _minutesList.SetCurrentValue(ItemsControl.ItemsSourceProperty, repeatedSet);
    }

    private void PopulateAmPm()
    {
        if (_amPmList == null)
        {
            return;
        }

        if (ClockIdentifier == ClockIdentifier.Clock12Hour)
        {
            _amPmList.SetCurrentValue(ItemsControl.ItemsSourceProperty, new List<string> { "AM", "PM" });
            _amPmList.SetCurrentValue(VisibilityProperty, Visibility.Visible);
        }
        else
        {
            _amPmList.SetCurrentValue(ItemsControl.ItemsSourceProperty, null);
            _amPmList.SetCurrentValue(VisibilityProperty, Visibility.Collapsed);
        }
    }

    // Keep four rows available on either side even for increments of 15, 30 or 60.
    private static int GetRepeatCount(int count) =>
        Math.Max(InfiniteScrollRepeatCount, 2 * ((4 + count - 1) / count) + 1);

    private static int MiddleSetStart(ListBox list, int count) => (list.Items.Count / count / 2) * count;

    /// <summary>
    /// Updates the selection in the ListBoxes based on the _temporaryTime.
    /// </summary>
    private void UpdateListSelection()
    {
        if (_hoursList == null || _minutesList == null || _amPmList == null)
        {
            return;
        }

        _isUpdatingSelection = true; // Prevent selection events from firing during this update.

        // Update Hours
        var hour = _temporaryTime.Hours;
        var displayHour =
            (ClockIdentifier == ClockIdentifier.Clock12Hour) ? (hour % 12 == 0 ? 12 : hour % 12) : hour;
        var hourString =
            (ClockIdentifier == ClockIdentifier.Clock24Hour)
                ? displayHour.ToString("D2")
                : displayHour.ToString();
        var singleHourSetCount = (ClockIdentifier == ClockIdentifier.Clock24Hour) ? 24 : 12;

        if (_hoursList.ItemsSource is IList<string> hourItems)
        {
            var hourIndex = hourItems.IndexOf(hourString);
            if (hourIndex != -1)
            {
                // Select the item in the middle set for infinite scrolling.
                _hoursList.SetCurrentValue(Selector.SelectedIndexProperty, hourIndex + singleHourSetCount);
            }
        }

        // Update Minutes
        var minuteString = (_temporaryTime.Minutes / MinuteIncrement * MinuteIncrement).ToString("D2");
        var singleMinuteSetCount = 60 / MinuteIncrement;
        if (_minutesList.ItemsSource is IList<string> minuteItems)
        {
            var minuteIndex = minuteItems.IndexOf(minuteString);
            if (minuteIndex != -1)
            {
                // Select the item in the middle set for infinite scrolling.
                _minutesList.SetCurrentValue(
                    Selector.SelectedIndexProperty,
                    minuteIndex + MiddleSetStart(_minutesList, singleMinuteSetCount)
                );
            }
        }

        // Update AM/PM
        if (ClockIdentifier == ClockIdentifier.Clock12Hour)
        {
            _amPmList.SetCurrentValue(Selector.SelectedItemProperty, _temporaryTime.Hours < 12 ? "AM" : "PM");
        }

        _isUpdatingSelection = false; // Re-enable selection events.
    }

    /// <summary>
    /// Scrolls the ScrollViewer to center the currently selected item.
    /// </summary>
    private void ScrollToSelectedIndex(ListBox? listBox, ScrollViewer? scrollViewer)
    {
        if (listBox == null || listBox.SelectedIndex < 0 || scrollViewer == null)
        {
            return;
        }

        _isScrolling = true;

        if (listBox == _hoursList)
        {
            _hoursDebounceTimer?.Stop();
        }
        else if (listBox == _minutesList)
        {
            _minutesDebounceTimer?.Stop();
        }

        int totalItems = listBox.Items.Count;
        if (totalItems == 0 || scrollViewer.ExtentHeight <= 0)
        {
            _isScrolling = false;
            return;
        }

        double singleItemHeight = GetItemHeight(listBox);
        if (listBox != _amPmList)
        {
            int count =
                listBox == _hoursList
                    ? (ClockIdentifier == ClockIdentifier.Clock24Hour ? 24 : 12)
                    : 60 / MinuteIncrement;
            _isUpdatingSelection = true;
            listBox.SetCurrentValue(
                Selector.SelectedIndexProperty,
                MiddleSetStart(listBox, count) + listBox.SelectedIndex % count
            );
            _isUpdatingSelection = false;
        }

        // Calculate the offset to bring the top of the selected item to the top of the viewport.
        double targetOffset = listBox.Padding.Top + listBox.SelectedIndex * singleItemHeight;

        // Adjust the offset to center the item within the viewport.
        targetOffset = targetOffset - (scrollViewer.ViewportHeight / 2) + (singleItemHeight / 2);

        scrollViewer.ScrollToVerticalOffset(targetOffset);

        // Defer resetting the flag to ensure the scroll operation completes.
        _ = Dispatcher.BeginInvoke(
            DispatcherPriority.Input,
            () =>
            {
                _isScrolling = false;
            }
        );
    }

    private static double GetItemHeight(ListBox listBox) =>
        listBox.ItemContainerGenerator.ContainerFromIndex(listBox.SelectedIndex)
            is FrameworkElement { ActualHeight: > 0 } item
            ? item.ActualHeight
            : 40;

    private void CenterAmPmSelection() => ScrollToSelectedIndex(_amPmList, _amPmScrollViewer);

    /// <summary>
    /// Finds the ScrollViewer controls within the ListBoxes and sets up scroll event handling.
    /// </summary>
    private void SetupScrollViewers()
    {
        ScrollViewer? hoursViewer = FindVisualChild<ScrollViewer>(_hoursList);
        ScrollViewer? minutesViewer = FindVisualChild<ScrollViewer>(_minutesList);
        _amPmScrollViewer = FindVisualChild<ScrollViewer>(_amPmList);
        if (
            _hoursDebounceTimer != null
            && hoursViewer == _hoursScrollViewer
            && minutesViewer == _minutesScrollViewer
        )
        {
            return;
        }

        if (_hoursScrollViewer != null)
        {
            _hoursScrollViewer.ScrollChanged -= OnHoursScrollChanged;
        }

        if (_minutesScrollViewer != null)
        {
            _minutesScrollViewer.ScrollChanged -= OnMinutesScrollChanged;
        }

        _hoursScrollViewer = hoursViewer;
        _minutesScrollViewer = minutesViewer;

        if (_hoursScrollViewer != null)
        {
            _hoursScrollViewer.ScrollChanged += OnHoursScrollChanged;
        }

        if (_minutesScrollViewer != null)
        {
            _minutesScrollViewer.ScrollChanged += OnMinutesScrollChanged;
        }

        // Stop old timers before replacing handlers when the popup is reopened.
        _hoursDebounceTimer?.Stop();
        _minutesDebounceTimer?.Stop();
        // Initialize debouncing timers for scroll events.
        _hoursDebounceTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(150) };
        _hoursDebounceTimer.Tick += (s, e) =>
        {
            _hoursDebounceTimer.Stop();
            PerformDeferredJump(
                _hoursScrollViewer,
                (ClockIdentifier == ClockIdentifier.Clock24Hour) ? 24 : 12
            );
        };

        _minutesDebounceTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(150) };
        _minutesDebounceTimer.Tick += (s, e) =>
        {
            _minutesDebounceTimer.Stop();
            PerformDeferredJump(_minutesScrollViewer, 60 / MinuteIncrement);
        };
    }

    /// <summary>
    /// When scrolling stops near the top or bottom boundaries, this method "jumps" the scroll position
    /// to the corresponding item in the middle set to maintain the infinite-scrolling illusion.
    /// </summary>
    private void PerformDeferredJump(ScrollViewer? scrollViewer, int singleSetItemCount)
    {
        if (
            _popup is not { IsOpen: true }
            || scrollViewer == null
            || _isScrolling
            || scrollViewer.ExtentHeight <= 0
        )
        {
            return;
        }
        var listBox = scrollViewer == _hoursScrollViewer ? _hoursList : _minutesList;
        if (listBox == null || listBox.Items.Count == 0)
        {
            return;
        }
        double itemHeight = GetItemHeight(listBox);
        int centeredIndex = (int)
            Math.Round(
                (
                    scrollViewer.VerticalOffset
                    + scrollViewer.ViewportHeight / 2
                    - listBox.Padding.Top
                    - itemHeight / 2
                ) / itemHeight
            );
        centeredIndex = Math.Max(0, Math.Min(listBox.Items.Count - 1, centeredIndex));
        _isUpdatingSelection = true;
        listBox.SetCurrentValue(
            Selector.SelectedIndexProperty,
            MiddleSetStart(listBox, singleSetItemCount) + centeredIndex % singleSetItemCount
        );
        _isUpdatingSelection = false;
        UpdateTemporaryTimeFromSelection();
        ScrollToSelectedIndex(listBox, scrollViewer);
    }

    private void OnHoursScrollChanged(object? sender, ScrollChangedEventArgs e)
    {
        if (_isScrolling || _popup is not { IsOpen: true } || e.VerticalChange == 0)
            return;
        // Restart the debounce timer on each scroll event.
        _hoursDebounceTimer?.Stop();
        _hoursDebounceTimer?.Start();
    }

    private void OnMinutesScrollChanged(object? sender, ScrollChangedEventArgs e)
    {
        if (_isScrolling || _popup is not { IsOpen: true } || e.VerticalChange == 0)
            return;
        // Restart the debounce timer on each scroll event.
        _minutesDebounceTimer?.Stop();
        _minutesDebounceTimer?.Start();
    }

    private void OnHourChanged(object sender, SelectionChangedEventArgs e)
    {
        if (_isUpdatingSelection || e.AddedItems.Count == 0)
        {
            return;
        }

        UpdateTemporaryTimeFromSelection();
        QueueCenterSelection();
    }

    private void OnMinuteChanged(object sender, SelectionChangedEventArgs e)
    {
        if (_isUpdatingSelection || e.AddedItems.Count == 0)
        {
            return;
        }

        UpdateTemporaryTimeFromSelection();
        QueueCenterSelection();
    }

    private void OnAmPmChanged(object sender, SelectionChangedEventArgs e)
    {
        if (_isUpdatingSelection || e.AddedItems.Count == 0)
        {
            return;
        }

        UpdateTemporaryTimeFromSelection();
        QueueCenterSelection();
    }

    private void OnAmPmUpButtonClick(object sender, RoutedEventArgs e) => MoveAmPmSelection(-1);

    private void OnAmPmDownButtonClick(object sender, RoutedEventArgs e) => MoveAmPmSelection(1);

    private void MoveAmPmSelection(int offset)
    {
        if (_amPmList is not { Items.Count: > 0 })
            return;
        var index = (_amPmList.SelectedIndex + offset + 2) % 2;
        _isUpdatingSelection = true;
        _amPmList.SetCurrentValue(Selector.SelectedIndexProperty, index);
        _isUpdatingSelection = false;
        UpdateTemporaryTimeFromSelection();
        CenterAmPmSelection();
    }

    /// <summary>
    /// Updates the _temporaryTime field based on the current selections in the ListBoxes.
    /// </summary>
    private void UpdateTemporaryTimeFromSelection()
    {
        if (_hoursList?.SelectedItem == null || _minutesList?.SelectedItem == null)
        {
            return;
        }

        if (
            !int.TryParse((string)_hoursList.SelectedItem, out var hour)
            || !int.TryParse((string)_minutesList.SelectedItem, out var minute)
        )
        {
            return;
        }

        if (ClockIdentifier == ClockIdentifier.Clock12Hour)
        {
            var amPm = _amPmList?.SelectedItem as string;
            if (amPm == "PM" && hour != 12)
            {
                hour += 12;
            }
            else if (amPm == "AM" && hour == 12)
            {
                hour = 0; // Midnight case: 12 AM is 00:00
            }
        }

        _temporaryTime = new TimeSpan(hour % 24, minute, 0);
    }

    private void OnAcceptButtonClick(object sender, RoutedEventArgs e)
    {
        UpdateTemporaryTimeFromSelection(); // Ensure time is updated with the latest selection.
        SetCurrentValue(TimeProperty, _temporaryTime);
        SetCurrentValue(SelectedTimeProperty, Time);
        if (_popup != null)
        {
            _popup.SetCurrentValue(Popup.IsOpenProperty, false);
        }
    }

    private void OnDismissButtonClick(object sender, RoutedEventArgs e)
    {
        if (_popup != null)
        {
            _popup.SetCurrentValue(Popup.IsOpenProperty, false);
        }
    }

    private void OnHoursUpButtonClick(object sender, RoutedEventArgs e) => MoveSelection(_hoursList, -1);

    private void OnHoursDownButtonClick(object sender, RoutedEventArgs e) => MoveSelection(_hoursList, 1);

    private void OnMinutesUpButtonClick(object sender, RoutedEventArgs e) => MoveSelection(_minutesList, -1);

    private void OnMinutesDownButtonClick(object sender, RoutedEventArgs e) => MoveSelection(_minutesList, 1);

    private void MoveSelection(ListBox? listBox, int itemOffset)
    {
        if (listBox == null || listBox.Items.Count == 0 || listBox.SelectedIndex < 0)
        {
            return;
        }

        var singleSetItemCount =
            listBox == _hoursList
                ? (ClockIdentifier == ClockIdentifier.Clock24Hour ? 24 : 12)
                : 60 / MinuteIncrement;
        var selectedIndex =
            MiddleSetStart(listBox, singleSetItemCount)
            + (listBox.SelectedIndex + itemOffset + singleSetItemCount) % singleSetItemCount;

        _isUpdatingSelection = true;
        listBox.SetCurrentValue(Selector.SelectedIndexProperty, selectedIndex);
        _isUpdatingSelection = false;
        UpdateTemporaryTimeFromSelection();

        if (listBox == _hoursList)
        {
            ScrollToSelectedIndex(_hoursList, _hoursScrollViewer);
        }
        else
        {
            ScrollToSelectedIndex(_minutesList, _minutesScrollViewer);
        }
    }

    private static void OnTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var control = (TimePicker)d;
        var newTime = (TimeSpan)e.NewValue;

        if (!control._isSynchronizingTime)
        {
            control._isSynchronizingTime = true;
            control.SetCurrentValue(SelectedTimeProperty, newTime);
            control._isSynchronizingTime = false;
        }

        control.RefreshPopupSelection();
    }

    private static object CoerceTime(DependencyObject d, object baseValue)
    {
        var control = (TimePicker)d;
        // A property-changed callback does not run for an explicit Time=Zero
        // assignment when Zero is already the default. Coercion also covers
        // SetValue, styles and bindings, unlike adding logic to the CLR setter.
        if (!control._isSynchronizingTime)
        {
            control._isSynchronizingTime = true;
            try
            {
                control.SetCurrentValue(SelectedTimeProperty, baseValue);
            }
            finally
            {
                control._isSynchronizingTime = false;
            }
            control.RefreshPopupSelection();
        }
        return baseValue;
    }

    private static void OnSelectedTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var control = (TimePicker)d;
        if (control._isSynchronizingTime)
            return;
        control._isSynchronizingTime = true;
        try
        {
            control.SetCurrentValue(TimeProperty, e.NewValue is TimeSpan selected ? selected : TimeSpan.Zero);
        }
        finally
        {
            control._isSynchronizingTime = false;
        }
        control.RefreshPopupSelection();
    }

    private void RefreshPopupSelection()
    {
        if (_popup is not { IsOpen: true })
            return;
        StopScrolling();
        _temporaryTime = SelectedTime ?? TimeSpan.Zero;
        UpdateListSelection();
        QueueCenterSelection();
    }

    private static void OnClockIdentifierChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var control = (TimePicker)d;
        control._isUpdatingSelection = true;
        control.PopulateHours();
        control.PopulateAmPm();
        control._isUpdatingSelection = false;
        control.UpdateListSelection();
        control.QueueCenterSelection();
    }

    private static void OnMinuteIncrementChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var control = (TimePicker)d;

        // Ensure the increment is a valid divisor of 60.
        if (control.MinuteIncrement < 1 || 60 % control.MinuteIncrement != 0)
        {
            control.SetCurrentValue(MinuteIncrementProperty, 1); // Reset to a valid default.
            return;
        }

        control._isUpdatingSelection = true;
        control.PopulateMinutes();
        control._isUpdatingSelection = false;
        control.UpdateListSelection();
        control.QueueCenterSelection();
    }

    /// <summary>
    /// Finds a visual child of a specified type in the visual tree.
    /// </summary>
    private static T? FindVisualChild<T>(DependencyObject? obj)
        where T : DependencyObject
    {
        if (obj == null)
        {
            return null;
        }

        for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
        {
            DependencyObject child = VisualTreeHelper.GetChild(obj, i);
            if (child is T typedChild)
            {
                return typedChild;
            }

            T? childOfChild = FindVisualChild<T>(child);
            if (childOfChild != null)
            {
                return childOfChild;
            }
        }

        return null;
    }
}
