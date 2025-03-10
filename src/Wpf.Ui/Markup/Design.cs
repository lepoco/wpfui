// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

namespace Wpf.Ui.Markup;

/// <summary>
/// Custom design time attributes based on Marcin Najder implementation.
/// </summary>
/// <example>
/// <code lang="xml">
/// &lt;ui:FluentWindow
///     xmlns:ui="http://schemas.lepo.co/wpfui/2022/xaml"
///     ui:Design.Background="{DynamicResource ApplicationBackgroundBrush}"
///     ui:Design.Foreground="{DynamicResource TextFillColorPrimaryBrush}"&gt;
///     &lt;Button Content="Hello World" /&gt;
/// &lt;/FluentWindow&gt;
/// </code>
/// </example>
public static class Design
{
    private static readonly string[] DesignProcesses =
    [
        "devenv",
        "dotnet",
        "RiderWpfPreviewerLauncher64"
    ];

    private static bool? _inDesignMode;

    /// <summary>
    /// Gets a value indicating whether the framework is in design-time mode. (Caliburn.Micro implementation)
    /// </summary>
    private static bool InDesignMode =>
        _inDesignMode ??=
            (bool)
                DependencyPropertyDescriptor
                    .FromProperty(DesignerProperties.IsInDesignModeProperty, typeof(FrameworkElement))
                    .Metadata.DefaultValue
            || DesignProcesses.Any(process => System
                .Diagnostics.Process.GetCurrentProcess()
                .ProcessName.StartsWith(process, StringComparison.Ordinal));

    public static readonly DependencyProperty BackgroundProperty = DependencyProperty.RegisterAttached(
        "Background",
        typeof(System.Windows.Media.Brush),
        typeof(Design),
        new PropertyMetadata(OnBackgroundChanged)
    );

    public static readonly DependencyProperty ForegroundProperty = DependencyProperty.RegisterAttached(
        "Foreground",
        typeof(System.Windows.Media.Brush),
        typeof(Design),
        new PropertyMetadata(OnForegroundChanged)
    );

    /// <summary>Helper for getting <see cref="BackgroundProperty"/> from <paramref name="dependencyObject"/>.</summary>
    /// <param name="dependencyObject"><see cref="DependencyObject"/> to read <see cref="BackgroundProperty"/> from.</param>
    /// <returns>Background property value.</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "WpfAnalyzers.DependencyProperty",
        "WPF0033:Add [AttachedPropertyBrowsableForType]",
        Justification = "Because"
    )]
    public static System.Windows.Media.Brush? GetBackground(DependencyObject dependencyObject) =>
        (System.Windows.Media.Brush)dependencyObject.GetValue(BackgroundProperty);

    /// <summary>Helper for setting <see cref="BackgroundProperty"/> on <paramref name="dependencyObject"/>.</summary>
    /// <param name="dependencyObject"><see cref="DependencyObject"/> to set <see cref="BackgroundProperty"/> on.</param>
    /// <param name="value">Background property value.</param>
    public static void SetBackground(DependencyObject dependencyObject, System.Windows.Media.Brush? value) =>
        dependencyObject.SetValue(BackgroundProperty, value);

    /// <summary>Helper for getting <see cref="ForegroundProperty"/> from <paramref name="dependencyObject"/>.</summary>
    /// <param name="dependencyObject"><see cref="DependencyObject"/> to read <see cref="ForegroundProperty"/> from.</param>
    /// <returns>Foreground property value.</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "WpfAnalyzers.DependencyProperty",
        "WPF0033:Add [AttachedPropertyBrowsableForType]",
        Justification = "Because"
    )]
    public static System.Windows.Media.Brush? GetForeground(DependencyObject dependencyObject) =>
        (System.Windows.Media.Brush)dependencyObject.GetValue(ForegroundProperty);

    /// <summary>Helper for setting <see cref="ForegroundProperty"/> on <paramref name="dependencyObject"/>.</summary>
    /// <param name="dependencyObject"><see cref="DependencyObject"/> to set <see cref="ForegroundProperty"/> on.</param>
    /// <param name="value">Foreground property value.</param>
    public static void SetForeground(DependencyObject dependencyObject, System.Windows.Media.Brush? value) =>
        dependencyObject.SetValue(ForegroundProperty, value);

    private static void OnBackgroundChanged(DependencyObject? d, DependencyPropertyChangedEventArgs e)
    {
        if (!InDesignMode)
        {
            return;
        }

        d?.GetType()?.GetProperty("Background")?.SetValue(d, e.NewValue, null);
    }

    private static void OnForegroundChanged(DependencyObject? d, DependencyPropertyChangedEventArgs e)
    {
        if (!InDesignMode)
        {
            return;
        }

        d?.GetType()?.GetProperty("Foreground")?.SetValue(d, e.NewValue, null);
    }
}
