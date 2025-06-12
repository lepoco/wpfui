// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows.Controls;
using System.Windows.Controls.Primitives;

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

/// <summary>
/// Represents a button with two parts that can be invoked separately. One part behaves like a standard button and the other part invokes a flyout.
/// </summary>
[TemplatePart(Name = TemplateElementToggleButton, Type = typeof(ToggleButton))]
public class SplitButton : Button
{
    /// <summary>
    /// Template element represented by the <c>ToggleButton</c> name.
    /// </summary>
    private const string TemplateElementToggleButton = "PART_ToggleButton";

    private ContextMenu? _contextMenu;

    /// <summary>
    /// Gets or sets control responsible for toggling the drop-down button.
    /// </summary>
    protected ToggleButton SplitButtonToggleButton { get; set; } = null!;

    /// <summary>Identifies the <see cref="Flyout"/> dependency property.</summary>
    public static readonly DependencyProperty FlyoutProperty = DependencyProperty.Register(
        nameof(Flyout),
        typeof(object),
        typeof(SplitButton),
        new PropertyMetadata(null, OnFlyoutChanged)
    );

    /// <summary>Identifies the <see cref="IsDropDownOpen"/> dependency property.</summary>
    public static readonly DependencyProperty IsDropDownOpenProperty = DependencyProperty.Register(
        nameof(IsDropDownOpen),
        typeof(bool),
        typeof(SplitButton),
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

    public SplitButton()
    {
        Unloaded += static (sender, _) =>
        {
            var self = (SplitButton)sender;

            self.ReleaseTemplateResources();
        };
        Loaded += static (sender, _) =>
        {
            var self = (SplitButton)sender;
            if (self.SplitButtonToggleButton != null)
            {
                self.AttachToggleButtonClick();
            }
        };
    }

    protected virtual void AttachTemplateResources()
    {
        base.OnApplyTemplate();

        if (GetTemplateChild(TemplateElementToggleButton) is ToggleButton toggleButton)
        {
            SplitButtonToggleButton = toggleButton;
            AttachToggleButtonClick();
        }
        else
        {
            throw new NullReferenceException(
                $"Element {nameof(TemplateElementToggleButton)} of type {typeof(ToggleButton)} not found in {typeof(SplitButton)}"
            );
        }
    }

    private void AttachToggleButtonClick()
    {
        if (SplitButtonToggleButton != null)
        {
            SplitButtonToggleButton.Click -= OnSplitButtonToggleButtonOnClick;
            SplitButtonToggleButton.Click += OnSplitButtonToggleButtonOnClick;
        }
    }

    private static void OnFlyoutChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is SplitButton dropDownButton)
        {
            dropDownButton.OnFlyoutChanged(e.NewValue);
        }
    }

    /// <summary>This method is invoked when the <see cref="FlyoutProperty"/> changes.</summary>
    /// <param name="value">The new value of <see cref="FlyoutProperty"/>.</param>
    protected virtual void OnFlyoutChanged(object value)
    {
        if (value is ContextMenu contextMenu)
        {
            _contextMenu = contextMenu;
            _contextMenu.Opened += OnContextMenuOpened;
            _contextMenu.Closed += OnContextMenuClosed;
        }
    }

    private static void OnIsDropDownOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is SplitButton dropDownButton)
        {
            dropDownButton.OnIsDropDownOpenChanged(e.NewValue is bool boolVal && boolVal);
        }
    }

    protected virtual void OnContextMenuClosed(object sender, RoutedEventArgs e)
    {
        SetCurrentValue(IsDropDownOpenProperty, false);
    }

    protected virtual void OnContextMenuOpened(object sender, RoutedEventArgs e)
    {
        SetCurrentValue(IsDropDownOpenProperty, true);
    }

    /// <summary>This method is invoked when the <see cref="IsDropDownOpenProperty"/> changes.</summary>
    /// <param name="currentValue">The new value of <see cref="IsDropDownOpenProperty"/>.</param>
    protected virtual void OnIsDropDownOpenChanged(bool currentValue) { }

    /// <inheritdoc />
    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        if (GetTemplateChild(TemplateElementToggleButton) is ToggleButton toggleButton)
        {
            SplitButtonToggleButton = toggleButton;
            AttachToggleButtonClick();
        }
        else
        {
            throw new NullReferenceException(
                $"Element {nameof(TemplateElementToggleButton)} of type {typeof(ToggleButton)} not found in {typeof(SplitButton)}"
            );
        }
    }

    /// <summary>
    /// Triggered when the control is unloaded. Releases resource bindings.
    /// </summary>
    protected virtual void ReleaseTemplateResources()
    {
        SplitButtonToggleButton.Click -= OnSplitButtonToggleButtonOnClick;
    }

    private void OnSplitButtonToggleButtonOnClick(object sender, RoutedEventArgs e)
    {
        if (sender is not ToggleButton || _contextMenu is null)
        {
            return;
        }

        _contextMenu.SetCurrentValue(MinWidthProperty, ActualWidth);
        _contextMenu.SetCurrentValue(ContextMenu.PlacementTargetProperty, this);
        _contextMenu.SetCurrentValue(ContextMenu.PlacementProperty, PlacementMode.Bottom);
        _contextMenu.SetCurrentValue(ContextMenu.IsOpenProperty, true);
    }
}
