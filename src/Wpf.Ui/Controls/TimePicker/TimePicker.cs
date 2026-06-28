// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

// ReSharper disable once CheckNamespace
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
[TemplatePart(Name = PartAcceptButton, Type = typeof(Button))]
[TemplatePart(Name = PartDismissButton, Type = typeof(Button))]
public class TimePicker : ButtonBase
{
    private const string PartPopup = "PART_Popup";
    private const string PartHoursList = "PART_HoursList";
    private const string PartMinutesList = "PART_MinutesList";
    private const string PartAmPmList = "PART_AmPmList";
    private const string PartAcceptButton = "PART_AcceptButton";
    private const string PartDismissButton = "PART_DismissButton";

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
    private ScrollViewer? _hoursScrollViewer;
    private ScrollViewer? _minutesScrollViewer;
    private TimeSpan _temporaryTime;
    private bool _isUpdatingSelection; // Flag to prevent re-entrant event handling while programmatically updating selection.
    private bool _isScrolling; // Flag to prevent selection changes during programmatic scrolling (e.g., in infinite scroll jump).

    private DispatcherTimer? _hoursDebounceTimer;
    private DispatcherTimer? _minutesDebounceTimer;

    /// <summary>Identifies the <see cref="Header"/> dependency property.</summary>
    public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
        nameof(Header), typeof(object), typeof(TimePicker), new PropertyMetadata(null));

    /// <summary>Identifies the <see cref="Time"/> dependency property.</summary>
    public static readonly DependencyProperty TimeProperty = DependencyProperty.Register(
        nameof(Time), typeof(TimeSpan), typeof(TimePicker), new PropertyMetadata(TimeSpan.Zero, OnTimeChanged));

    /// <summary>Identifies the <see cref="SelectedTime"/> dependency property.</summary>
    public static readonly DependencyProperty SelectedTimeProperty = DependencyProperty.Register(
        nameof(SelectedTime), typeof(TimeSpan?), typeof(TimePicker), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    /// <summary>Identifies the <see cref="MinuteIncrement"/> dependency property.</summary>
    public static readonly DependencyProperty MinuteIncrementProperty = DependencyProperty.Register(
        nameof(MinuteIncrement), typeof(int), typeof(TimePicker), new PropertyMetadata(1, OnMinuteIncrementChanged));

    /// <summary>Identifies the <see cref="ClockIdentifier"/> dependency property.</summary>
    public static readonly DependencyProperty ClockIdentifierProperty = DependencyProperty.Register(
        nameof(ClockIdentifier), typeof(ClockIdentifier), typeof(TimePicker), new PropertyMetadata(ClockIdentifier.Clock24Hour, OnClockIdentifierChanged));

    public static readonly DependencyProperty AcceptButtonTextProperty = DependencyProperty.Register(
    nameof(AcceptButtonText), typeof(string), typeof(TimePicker), new PropertyMetadata("OK"));

    public string AcceptButtonText
    {
        get => (string)GetValue(AcceptButtonTextProperty);
        set => SetValue(AcceptButtonTextProperty, value);
    }

    public static readonly DependencyProperty DismissButtonTextProperty = DependencyProperty.Register(
        nameof(DismissButtonText), typeof(string), typeof(TimePicker), new PropertyMetadata("Cancel"));

    public string DismissButtonText
    {
        get => (string)GetValue(DismissButtonTextProperty);
        set => SetValue(DismissButtonTextProperty, value);
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
        // Detach old event handlers to prevent memory leaks
        if (_acceptButton != null)
        {
            _acceptButton.Click -= OnAcceptButtonClick;
        }

        if (_dismissButton != null)
        {
            _dismissButton.Click -= OnDismissButtonClick;
        }

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

        base.OnApplyTemplate();

        // Get template parts
        _popup = GetTemplateChild(PartPopup) as Popup;
        _hoursList = GetTemplateChild(PartHoursList) as ListBox;
        _minutesList = GetTemplateChild(PartMinutesList) as ListBox;
        _amPmList = GetTemplateChild(PartAmPmList) as ListBox;
        _acceptButton = GetTemplateChild(PartAcceptButton) as ORGButton;
        _dismissButton = GetTemplateChild(PartDismissButton) as ORGButton;

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

        _temporaryTime = Time;
        UpdateListSelection();
        _popup.SetCurrentValue(Popup.IsOpenProperty, true);

        // The ScrollViewers are only available after the popup is open and rendered.
        if (_hoursScrollViewer == null || _minutesScrollViewer == null)
        {
            SetupScrollViewers();
        }

        // Defer scrolling into view to ensure the ListBox items are rendered.
        _ = Dispatcher.BeginInvoke(DispatcherPriority.Input, () =>
        {
            ScrollToSelectedIndex(_hoursList, _hoursScrollViewer);
            ScrollToSelectedIndex(_minutesList, _minutesScrollViewer);
        });
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
            singleSet.Add(ClockIdentifier == ClockIdentifier.Clock24Hour ? value.ToString("D2") : value.ToString());
        }

        var repeatedSet = new List<string>();
        for (int i = 0; i < InfiniteScrollRepeatCount; i++)
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
        for (int i = 0; i < InfiniteScrollRepeatCount; i++)
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
        var displayHour = (ClockIdentifier == ClockIdentifier.Clock12Hour) ? (hour % 12 == 0 ? 12 : hour % 12) : hour;
        var hourString = (ClockIdentifier == ClockIdentifier.Clock24Hour) ? displayHour.ToString("D2") : displayHour.ToString();
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
        var minuteString = _temporaryTime.Minutes.ToString("D2");
        var singleMinuteSetCount = 60 / MinuteIncrement;
        if (_minutesList.ItemsSource is IList<string> minuteItems)
        {
            var minuteIndex = minuteItems.IndexOf(minuteString);
            if (minuteIndex != -1)
            {
                // Select the item in the middle set for infinite scrolling.
                _minutesList.SetCurrentValue(Selector.SelectedIndexProperty, minuteIndex + singleMinuteSetCount);
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
        if (listBox?.SelectedIndex < 0 || scrollViewer == null)
        {
            return;
        }

        _isScrolling = true;

        int totalItems = listBox!.Items.Count;
        if (totalItems == 0 || scrollViewer.ExtentHeight <= 0)
        {
            _isScrolling = false;
            return;
        }

        double singleItemHeight = scrollViewer.ExtentHeight / totalItems;

        // Calculate the offset to bring the top of the selected item to the top of the viewport.
        double targetOffset = listBox.SelectedIndex * singleItemHeight;

        // Adjust the offset to center the item within the viewport.
        targetOffset = targetOffset - (scrollViewer.ViewportHeight / 2) + (singleItemHeight / 2);

        scrollViewer.ScrollToVerticalOffset(targetOffset);

        // Defer resetting the flag to ensure the scroll operation completes.
        _ = Dispatcher.BeginInvoke(DispatcherPriority.Input, () => { _isScrolling = false; });
    }

    /// <summary>
    /// Finds the ScrollViewer controls within the ListBoxes and sets up scroll event handling.
    /// </summary>
    private void SetupScrollViewers()
    {
        if (_hoursScrollViewer != null)
        {
            _hoursScrollViewer.ScrollChanged -= OnHoursScrollChanged;
        }

        if (_minutesScrollViewer != null)
        {
            _minutesScrollViewer.ScrollChanged -= OnMinutesScrollChanged;
        }

        _hoursScrollViewer = FindVisualChild<ScrollViewer>(_hoursList);
        _minutesScrollViewer = FindVisualChild<ScrollViewer>(_minutesList);

        if (_hoursScrollViewer != null)
        {
            _hoursScrollViewer.ScrollChanged += OnHoursScrollChanged;
        }

        if (_minutesScrollViewer != null)
        {
            _minutesScrollViewer.ScrollChanged += OnMinutesScrollChanged;
        }

        // Initialize debouncing timers for scroll events.
        _hoursDebounceTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(150) };
        _hoursDebounceTimer.Tick += (s, e) =>
        {
            _hoursDebounceTimer.Stop();
            PerformDeferredJump(_hoursScrollViewer, (ClockIdentifier == ClockIdentifier.Clock24Hour) ? 24 : 12);
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
        if (scrollViewer == null || _isScrolling || scrollViewer.ExtentHeight <= 0)
        {
            return;
        }

        double singleSetHeight = scrollViewer.ExtentHeight / InfiniteScrollRepeatCount;
        if (singleSetHeight <= 0)
        {
            return;
        }

        bool needsJump = false;
        double newOffset = scrollViewer.VerticalOffset;

        // Check if scrolled to the top buffer area.
        if (scrollViewer.VerticalOffset < singleSetHeight)
        {
            newOffset += singleSetHeight;
            needsJump = true;
        }

        // Check if scrolled to the bottom buffer area.
        else if (scrollViewer.VerticalOffset >= singleSetHeight * (InfiniteScrollRepeatCount - 1))
        {
            newOffset -= singleSetHeight;
            needsJump = true;
        }

        if (needsJump)
        {
            _isScrolling = true;
            scrollViewer.ScrollToVerticalOffset(newOffset);

            // Use Dispatcher to reset the flag after the UI has updated from the programmatic scroll.
            _ = Dispatcher.BeginInvoke(DispatcherPriority.Input, () => { _isScrolling = false; });
        }
    }

    private void OnHoursScrollChanged(object? sender, ScrollChangedEventArgs e)
    {
        // Restart the debounce timer on each scroll event.
        _hoursDebounceTimer?.Stop();
        _hoursDebounceTimer?.Start();
    }

    private void OnMinutesScrollChanged(object? sender, ScrollChangedEventArgs e)
    {
        // Restart the debounce timer on each scroll event.
        _minutesDebounceTimer?.Stop();
        _minutesDebounceTimer?.Start();
    }

    private void OnHourChanged(object sender, SelectionChangedEventArgs e)
    {
        if (_isUpdatingSelection || _isScrolling || e.AddedItems.Count == 0)
        {
            return;
        }

        UpdateTemporaryTimeFromSelection();
    }

    private void OnMinuteChanged(object sender, SelectionChangedEventArgs e)
    {
        if (_isUpdatingSelection || _isScrolling || e.AddedItems.Count == 0)
        {
            return;
        }

        UpdateTemporaryTimeFromSelection();
    }

    private void OnAmPmChanged(object sender, SelectionChangedEventArgs e)
    {
        if (_isUpdatingSelection || _isScrolling || e.AddedItems.Count == 0)
        {
            return;
        }

        UpdateTemporaryTimeFromSelection();
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

        if (!int.TryParse((string)_hoursList.SelectedItem, out var hour) || !int.TryParse((string)_minutesList.SelectedItem, out var minute))
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

    private static void OnTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var control = (TimePicker)d;
        var newTime = (TimeSpan)e.NewValue;

        control.SelectedTime = newTime;

        // If the popup is open, update the list selections to reflect the new Time.
        if (control._popup is { IsOpen: true })
        {
            control._temporaryTime = newTime;
            control.UpdateListSelection();
            _ = control.Dispatcher.BeginInvoke(DispatcherPriority.Input, () =>
             {
                 control.ScrollToSelectedIndex(control._hoursList, control._hoursScrollViewer);
                 control.ScrollToSelectedIndex(control._minutesList, control._minutesScrollViewer);
             });
        }
    }

    private static void OnClockIdentifierChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var control = (TimePicker)d;
        control.PopulateHours();
        control.PopulateAmPm();
    }

    private static void OnMinuteIncrementChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var control = (TimePicker)d;

        // Ensure the increment is a valid divisor of 60.
        if (control.MinuteIncrement < 1 || 60 % control.MinuteIncrement != 0)
        {
            control.MinuteIncrement = 1; // Reset to a valid default.
        }

        control.PopulateMinutes();
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