// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

// ReSharper disable once CheckNamespace
using System.Windows.Controls.Primitives;
using System.Windows.Input;

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

/// <summary>
/// A control that drop downs a flyout of choices from which one can be chosen.
/// </summary>
[TemplatePart(Name = TemplateElementPopup, Type = typeof(Popup))]
[TemplatePart(Name = TemplateElementToggleButton, Type = typeof(ToggleButton))]
public class DropDownButton : Button
{
    /// <summary>
    /// Template element represented by the <c>ToggleButton</c> name.
    /// </summary>
    private const string TemplateElementPopup = "Popup";

    /// <summary>
    /// Template element represented by the <c>ToggleButton</c> name.
    /// </summary>
    private const string TemplateElementToggleButton = "ToggleButton";

    /// <summary>
    /// Control responsible for displaying the flyout elements.
    /// </summary>
    protected Popup DropDownButtonPopup = null!;

    /// <summary>
    /// Control responsible for toggling the drop-down button.
    /// </summary>
    protected ToggleButton DropDownButtonToggleButton = null!;

    /// <summary>
    /// Property for <see cref="Flyout"/>.
    /// </summary>
    public static readonly DependencyProperty FlyoutProperty = DependencyProperty.Register(
        nameof(Flyout),
        typeof(object),
        typeof(DropDownButton),
        new PropertyMetadata(null, OnFlyoutChangedCallback)
    );

    /// <summary>
    /// Property for <see cref="IsDropDownOpen"/>.
    /// </summary>
    public static readonly DependencyProperty IsDropDownOpenProperty = DependencyProperty.Register(
        nameof(IsDropDownOpen),
        typeof(bool),
        typeof(DropDownButton),
        new PropertyMetadata(false, OnIsDropDownOpenChanged)
    );

    /// <summary>
    /// Gets or sets the flyout associated with this button.
    /// </summary>
    [Bindable(true)]
    public object? Flyout
    {
        get => GetValue(FlyoutProperty);
        set => SetValue(FlyoutProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the drop-down for a button is currently open.
    /// </summary>
    /// <returns>
    /// <see langword="true" /> if the drop-down is open; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
    [Bindable(true)]
    [Browsable(false)]
    [Category("Appearance")]
    public bool IsDropDownOpen
    {
        get => (bool)GetValue(IsDropDownOpenProperty);
        set => SetValue(IsDropDownOpenProperty, value);
    }

    public DropDownButton()
    {
        Unloaded += static (sender, _) =>
        {
            var self = (DropDownButton)sender;

            self.ReleaseTemplateResources();
        };
    }

    private static void OnFlyoutChangedCallback(
        DependencyObject d,
        DependencyPropertyChangedEventArgs e
    )
    {
        if (d is DropDownButton dropDownButton)
        {
            dropDownButton.OnFlyoutChangedCallback(e.NewValue);
        }
    }

    protected virtual void OnFlyoutChangedCallback(object value) { }

    private static void OnIsDropDownOpenChanged(
        DependencyObject d,
        DependencyPropertyChangedEventArgs e
    )
    {
        if (d is DropDownButton dropDownButton)
        {
            dropDownButton.OnIsDropDownOpenChanged(e.NewValue is bool ? (bool)e.NewValue : false);
        }
    }

    protected virtual void OnIsDropDownOpenChanged(bool currentValue) { }

    protected override void OnClick()
    {
        base.OnClick();

        SetCurrentValue(IsDropDownOpenProperty, !IsDropDownOpen);

        if (DropDownButtonToggleButton is not null)
        {
            DropDownButtonToggleButton.SetCurrentValue(
                ToggleButton.IsCheckedProperty,
                IsDropDownOpen
            );
        }
    }

    /// <inheritdoc />
    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        if (GetTemplateChild(TemplateElementPopup) is Popup popup)
        {
            DropDownButtonPopup = popup;
        }
        else
        {
            throw new NullReferenceException(
                $"Element {nameof(TemplateElementPopup)} of type {typeof(Popup)} not found in {typeof(DropDownButton)}"
            );
        }

        if (GetTemplateChild(TemplateElementToggleButton) is ToggleButton toggleButton)
        {
            DropDownButtonToggleButton = toggleButton;

            DropDownButtonToggleButton.Click -= OnDropDownButtonToggleButtonOnClick;
            DropDownButtonToggleButton.Click += OnDropDownButtonToggleButtonOnClick;
        }
        else
        {
            throw new NullReferenceException(
                $"Element {nameof(TemplateElementToggleButton)} of type {typeof(ToggleButton)} not found in {typeof(DropDownButton)}"
            );
        }
    }

    /// <summary>
    /// Triggered when the control is unloaded. Releases resource bindings.
    /// </summary>
    protected virtual void ReleaseTemplateResources()
    {
        DropDownButtonToggleButton.Click -= OnDropDownButtonToggleButtonOnClick;
    }

    /// <inheritdoc />
    protected override void OnLostFocus(RoutedEventArgs e)
    {
        base.OnLostFocus(e);

        if (IsDropDownOpen)
        {
            SetCurrentValue(IsDropDownOpenProperty, false);
        }
    }

    /// <inheritdoc />
    protected override void OnLostKeyboardFocus(KeyboardFocusChangedEventArgs e)
    {
        base.OnLostKeyboardFocus(e);

        if (IsDropDownOpen)
        {
            SetCurrentValue(IsDropDownOpenProperty, false);
        }
    }

    private void OnDropDownButtonToggleButtonOnClick(object sender, RoutedEventArgs e)
    {
        if (sender is not ToggleButton toggleButton)
        {
            return;
        }

        SetCurrentValue(IsDropDownOpenProperty, toggleButton.IsChecked);
    }

    protected T GetTemplateChild<T>(string name) where T : DependencyObject
    {
        if (GetTemplateChild(name) is not T dependencyObject)
        {
            throw new ArgumentNullException(name);
        }

        return dependencyObject;
    }
}
