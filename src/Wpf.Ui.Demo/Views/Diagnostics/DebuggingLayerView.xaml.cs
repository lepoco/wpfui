// This Source Code is partially based on the source code provided by the .NET Foundation.
// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski.
// All Rights Reserved.

#nullable enable

using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Input;

namespace Wpf.Ui.Demo.Views.Diagnostics;

public partial class DebuggingLayerView : UserControl
{
    private bool _isFocusIndicatorEnabled;

    public DebuggingLayerView()
    {
        InitializeComponent();
        Loaded += OnLoaded;
    }

    public Rect? FocusBounds
    {
        get => FocusIndicator.IsVisible
            ? new Rect(FocusIndicatorTranslateTransform.X, FocusIndicatorTranslateTransform.Y, FocusIndicator.ActualWidth, FocusIndicator.ActualHeight)
            : null;
        set
        {
            if (value is null)
            {
                FocusIndicator.Visibility = Visibility.Collapsed;
            }
            else
            {
                var bounds = value.Value;
                FocusIndicator.Visibility = Visibility.Visible;
                FocusIndicatorTranslateTransform.X = bounds.X;
                FocusIndicatorTranslateTransform.Y = bounds.Y;
                FocusIndicator.Width = bounds.Width;
                FocusIndicator.Height = bounds.Height;
            }
        }
    }

    public bool IsFocusIndicatorEnabled
    {
        get => _isFocusIndicatorEnabled;
        set
        {
            _isFocusIndicatorEnabled = value;
            if (value)
            {
                ShowFocusBounds(Keyboard.FocusedElement);
            }
            else
            {
                FocusBounds = null;
            }
        }
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        var window = Window.GetWindow(this);
        if (window is not null)
        {
            window.GotKeyboardFocus += Window_GotKeyboardFocus;
            window.LostKeyboardFocus += Window_LostKeyboardFocus;
        }
    }

    private void Window_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
    {
        FocusBounds = null;
    }

    private void Window_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
    {
        if (IsFocusIndicatorEnabled
            && Keyboard.FocusedElement is UIElement element
            && Window.GetWindow(element) is { } window
            && window == sender)
        {
            ShowFocusBounds(element);
        }
    }

    private void ShowFocusBounds(IInputElement focusedElement)
    {
        if (Keyboard.FocusedElement is UIElement element)
        {
            var topLeft = element.TranslatePoint(default, this);
            var bottomRight = element.TranslatePoint(new(element.RenderSize.Width, element.RenderSize.Height), this);
            FocusBounds = new(topLeft, bottomRight);
            FocusIndicatorTextBlock.Text = (focusedElement as FrameworkElement)?.Name is { } name
                ? (string.IsNullOrEmpty(name) ? focusedElement.GetType().Name : name)
                : focusedElement.GetType().Name;
        }
    }

    protected override AutomationPeer? OnCreateAutomationPeer()
    {
        return null;
    }
}
