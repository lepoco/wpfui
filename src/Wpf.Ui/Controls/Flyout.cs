// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Windows;

namespace Wpf.Ui.Controls;

/// <summary>
/// Represents a control that creates a pop-up window that displays information for an element in the interface.
/// </summary>
[TemplatePart(Name = "PART_Popup", Type = typeof(System.Windows.Controls.Primitives.Popup))]
public class Flyout : System.Windows.Controls.ContentControl
{
    private const string ElementPopup = "PART_Popup";

    private System.Windows.Controls.Primitives.Popup _popup = null;

    /// <summary>
    /// Property for <see cref="IsOpen"/>.
    /// </summary>
    public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(nameof(IsOpen),
        typeof(bool), typeof(Flyout), new PropertyMetadata(false));

    /// <summary>
    /// Invoked whenever application code or an internal process,
    /// such as a rebuilding layout pass, calls the ApplyTemplate method.
    /// </summary>
    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        _popup = GetTemplateChild(ElementPopup) as System.Windows.Controls.Primitives.Popup;

        if (_popup != null)
        {
            _popup.Opened -= OnPopupOpened;
            _popup.Opened += OnPopupOpened;

            _popup.Closed -= OnPopupClosed;
            _popup.Closed += OnPopupClosed;
        }
    }

    public bool IsOpen
    {
        get => (bool)GetValue(IsOpenProperty);
        protected set => SetValue(IsOpenProperty, value);
    }

    public void Show()
    {
        if (!IsOpen)
            IsOpen = true;
    }

    public void Hide()
    {
        if (IsOpen)
            IsOpen = false;
    }

    protected virtual void OnPopupOpened(object sender, EventArgs e)
    {

    }

    protected virtual void OnPopupClosed(object sender, EventArgs e)
    {
        Hide();
    }
}
