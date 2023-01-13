// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Marcin Najder, Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.ComponentModel;
using System.Windows;

namespace Wpf.Ui.Markup;

/// <summary>
/// Custom design time attributes based on Marcin Najder implementation.
/// </summary>
public static class Design
{
    private static readonly string DesignProcessName = "devenv";

    static bool? _inDesignMode;

    /// <summary>
    /// Indicates whether or not the framework is in design-time mode. (Caliburn.Micro implementation)
    /// </summary>
    private static bool InDesignMode
    {
        get
        {
            if (_inDesignMode != null)
                return _inDesignMode ?? false;

            _inDesignMode = (bool)DependencyPropertyDescriptor.FromProperty(
                DesignerProperties.IsInDesignModeProperty,
                typeof(FrameworkElement)).Metadata.DefaultValue;

            if (!(_inDesignMode ?? false)
                && System.Diagnostics.Process.GetCurrentProcess().ProcessName.StartsWith(DesignProcessName, System.StringComparison.Ordinal))
                _inDesignMode = true;

            return _inDesignMode ?? false;
        }
    }

    public static DependencyProperty BackgroundProperty = DependencyProperty.RegisterAttached(
        "Background", typeof(System.Windows.Media.Brush), typeof(Design),
        new PropertyMetadata(OnBackgroundPropertyChanged));

    public static DependencyProperty ForegroundProperty = DependencyProperty.RegisterAttached(
        "Foreground", typeof(System.Windows.Media.Brush), typeof(Design),
        new PropertyMetadata(OnForegroundPropertyChanged));

    public static System.Windows.Media.Brush GetBackground(DependencyObject dependencyObject)
        => (System.Windows.Media.Brush)dependencyObject.GetValue(BackgroundProperty);

    public static void SetBackground(DependencyObject dependencyObject, System.Windows.Media.Brush value)
        => dependencyObject.SetValue(BackgroundProperty, value);

    public static System.Windows.Media.Brush GetForeground(DependencyObject dependencyObject)
        => (System.Windows.Media.Brush)dependencyObject.GetValue(ForegroundProperty);

    public static void SetForeground(DependencyObject dependencyObject, System.Windows.Media.Brush value)
        => dependencyObject.SetValue(ForegroundProperty, value);

    private static void OnBackgroundPropertyChanged(DependencyObject? d, DependencyPropertyChangedEventArgs e)
    {
        if (!InDesignMode)
            return;

        d?.GetType()?.GetProperty("Background")?.SetValue(d, e.NewValue, null);
    }

    private static void OnForegroundPropertyChanged(DependencyObject? d, DependencyPropertyChangedEventArgs e)
    {
        if (!InDesignMode)
            return;

        d?.GetType()?.GetProperty("Foreground")?.SetValue(d, e.NewValue, null);
    }
}
