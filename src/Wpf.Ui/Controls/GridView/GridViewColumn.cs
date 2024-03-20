// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Reflection;

namespace Wpf.Ui.Controls;

/// <summary>
/// Extends <see cref="System.Windows.Controls.GridViewColumn"/> with MinWidth and MaxWidth properties.
/// It can be used with <see cref="ListView"/> when in GridView mode.
/// </summary>
/// <example>
/// <code lang="xml">
/// &lt;ui:ListView&gt;
///     &lt;ui:ListView.View&gt;
///         &lt;ui:GridView&gt;
///             &lt;ui:GridViewColumn
///                 MinWidth="100"
///                 MaxWidth="200"
///                 DisplayMemberBinding="{Binding FirstName}"
///                 Header="First Name" /&gt;
///         &lt;/ui:GridView&gt;
///     &lt;/ui:ListView.View&gt;
/// &lt;/ui:ListView&gt;
/// </code>
/// </example>
public class GridViewColumn : System.Windows.Controls.GridViewColumn
{
    // use reflection to the `_desiredWidth` private field.
    private static readonly FieldInfo _desiredWidthField = typeof(System.Windows.Controls.GridViewColumn).GetField("_desiredWidth", BindingFlags.NonPublic | BindingFlags.Instance) ?? throw new InvalidOperationException("The `_desiredWidth` field was not found.");

    /// <summary>
    /// Updates the desired width of the column to be clamped between MinWidth and MaxWidth).
    /// </summary>
    /// <remarks>
    /// Uses reflection to directly set the private `_desiredWidth` field on the `System.Windows.Controls.GridViewColumn`.
    /// </remarks>
    /// <exception cref="InvalidOperationException">
    /// Thrown if reflection fails to access the `_desiredWidth` field
    /// </exception>
    internal void UpdateDesiredWidth()
    {
        var currentWidth = (double)(_desiredWidthField.GetValue(this) ?? throw new InvalidOperationException("Failed to get the current `_desiredWidth`."));
        var clampedWidth = Math.Max(MinWidth, Math.Min(currentWidth, MaxWidth));
        _desiredWidthField.SetValue(this, clampedWidth);
    }

    /// <summary>
    /// Gets or sets the minimum width of the column.
    /// </summary>
    public double MinWidth
    {
        get => (double)GetValue(MinWidthProperty);
        set => SetValue(MinWidthProperty, value);
    }

    /// <summary>Identifies the <see cref="MinWidth"/> dependency property.</summary>
    public static readonly DependencyProperty MinWidthProperty = DependencyProperty.Register(nameof(MinWidth), typeof(double), typeof(GridViewColumn), new FrameworkPropertyMetadata(0.0, OnMinWidthChanged));

    private static void OnMinWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not GridViewColumn self)
        {
            return;
        }

        self.OnMinWidthChanged(e);
    }

    protected virtual void OnMinWidthChanged(DependencyPropertyChangedEventArgs e)
    {
        // Hook for derived classes to react to MinWidth property changes
    }

    /// <summary>
    /// gets or sets the maximum width of the column.
    /// </summary>
    public double MaxWidth
    {
        get => (double)GetValue(MaxWidthProperty);
        set => SetValue(MaxWidthProperty, value);
    }

    /// <summary>Identifies the <see cref="MaxWidth"/> dependency property.</summary>
    public static readonly DependencyProperty MaxWidthProperty = DependencyProperty.Register(nameof(MaxWidth), typeof(double), typeof(GridViewColumn), new FrameworkPropertyMetadata(Double.PositiveInfinity, OnMaxWidthChanged));

    private static void OnMaxWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not GridViewColumn self)
        {
            return;
        }

        self.OnMaxWidthChanged(e);
    }

    protected virtual void OnMaxWidthChanged(DependencyPropertyChangedEventArgs e)
    {
        // Hook for derived classes to react to MaxWidth property changes
    }
}
