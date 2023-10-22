// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

/// <summary>
/// Represents a control that allows a user to pick a date from a calendar display.
/// </summary>
/// <example>
/// <code lang="xml">
/// &lt;ui:CalendarDatePicker /&gt;
/// </code>
/// </example>
public class CalendarDatePicker : Wpf.Ui.Controls.Button
{
    private Popup? _popup;

    /// <summary>
    /// Property for <see cref="IsCalendarOpen"/>.
    /// </summary>
    public static readonly DependencyProperty IsCalendarOpenProperty = DependencyProperty.Register(
        nameof(IsCalendarOpen),
        typeof(bool),
        typeof(CalendarDatePicker),
        new PropertyMetadata(false)
    );

    /// <summary>
    /// Property for <see cref="IsTodayHighlighted"/>.
    /// </summary>
    public static readonly DependencyProperty IsTodayHighlightedProperty = DependencyProperty.Register(
        nameof(IsTodayHighlighted),
        typeof(bool),
        typeof(CalendarDatePicker),
        new PropertyMetadata(false)
    );

    /// <summary>
    /// Property for <see cref="Date"/>.
    /// </summary>
    public static readonly DependencyProperty DateProperty = DependencyProperty.Register(
        nameof(Date),
        typeof(DateTime?),
        typeof(CalendarDatePicker),
        new PropertyMetadata(null)
    );

    /// <summary>
    /// Property for <see cref="FirstDayOfWeek"/>.
    /// </summary>
    public static readonly DependencyProperty FirstDayOfWeekProperty = DependencyProperty.Register(
        nameof(FirstDayOfWeek),
        typeof(DayOfWeek),
        typeof(CalendarDatePicker),
        new PropertyMetadata(DateTimeHelper.GetCurrentDateFormat().FirstDayOfWeek)
    );

    /// <summary>
    /// Gets or sets a value indicating whether the current date is highlighted.
    /// </summary>
    public bool IsTodayHighlighted
    {
        get { return (bool)GetValue(IsTodayHighlightedProperty); }
        set { SetValue(IsTodayHighlightedProperty, value); }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the calendar view of the <see cref="CalendarDatePicker"/> is currently shown.
    /// </summary>
    [Bindable(true)]
    public bool IsCalendarOpen
    {
        get => (bool)GetValue(IsCalendarOpenProperty);
        set => SetValue(IsCalendarOpenProperty, value);
    }

    /// <summary>
    /// Gets or sets the day that is considered the beginning of the week.
    /// </summary>
    public DayOfWeek FirstDayOfWeek
    {
        get { return (DayOfWeek)GetValue(FirstDayOfWeekProperty); }
        set { SetValue(FirstDayOfWeekProperty, value); }
    }

    /// <summary>
    /// Gets or sets the date currently set in the calendar picker.
    /// </summary>
    [Bindable(true)]
    public DateTime? Date
    {
        get => (DateTime?)GetValue(DateProperty);
        set => SetValue(DateProperty, value);
    }

    /// <inheritdoc />
    protected override void OnClick()
    {
        base.OnClick();

        InitializePopup();

        SetCurrentValue(IsCalendarOpenProperty, !IsCalendarOpen);
    }

    private void InitializePopup()
    {
        if (_popup is not null)
        {
            return;
        }

        var calendar = new System.Windows.Controls.Calendar();
        _ = calendar.SetBinding(
            System.Windows.Controls.Calendar.SelectedDateProperty,
            new Binding(nameof(Date))
            {
                Source = this,
                Mode = BindingMode.TwoWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            }
        );
        _ = calendar.SetBinding(
            System.Windows.Controls.Calendar.IsTodayHighlightedProperty,
            new Binding(nameof(IsTodayHighlighted))
            {
                Source = this,
                Mode = BindingMode.TwoWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            }
        );
        _ = calendar.SetBinding(
            System.Windows.Controls.Calendar.FirstDayOfWeekProperty,
            new Binding(nameof(FirstDayOfWeek))
            {
                Source = this,
                Mode = BindingMode.TwoWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            }
        );

        calendar.SelectedDatesChanged += OnSelectedDatesChanged;

        _popup = new Popup
        {
            PlacementTarget = this,
            Placement = PlacementMode.Bottom,
            Child = calendar,
            Focusable = false,
            StaysOpen = false,
            VerticalOffset = 1D,
            VerticalAlignment = VerticalAlignment.Center,
            PopupAnimation = PopupAnimation.None,
            AllowsTransparency = true
        };

        _ = _popup.SetBinding(
            Popup.IsOpenProperty,
            new Binding(nameof(IsCalendarOpen))
            {
                Source = this,
                Mode = BindingMode.TwoWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            }
        );
    }

    protected virtual void OnPopupOpened(object? sender, EventArgs e)
    {
        if (sender is not Popup popup)
        {
            return;
        }

        if (popup.Child is null)
        {
            return;
        }

        _ = popup.Focus();
        _ = Keyboard.Focus(popup.Child);
    }

    protected virtual void OnSelectedDatesChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (IsCalendarOpen)
        {
            SetCurrentValue(IsCalendarOpenProperty, false);
        }
    }
}
