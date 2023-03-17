// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace Wpf.Ui.Controls.IconElements;

/// <summary>
/// Represents the base class for an icon UI element.
/// </summary>
[TypeConverter(typeof(IconElementConverter))]
public abstract class IconElement : FrameworkElement
{
    static IconElement()
    {
        FocusableProperty.OverrideMetadata(typeof(IconElement), new FrameworkPropertyMetadata(false));
        KeyboardNavigation.IsTabStopProperty.OverrideMetadata(typeof(IconElement), new FrameworkPropertyMetadata(false));
    }

    /// <summary>
    /// Property for <see cref="Foreground"/>.
    /// </summary>
    public static readonly DependencyProperty ForegroundProperty =
        TextElement.ForegroundProperty.AddOwner(
            typeof(IconElement),
            new FrameworkPropertyMetadata(SystemColors.ControlTextBrush,
                FrameworkPropertyMetadataOptions.Inherits,
                static (d, args) => ((IconElement)d).OnForegroundPropertyChanged(args)));

    /// <inheritdoc cref="Control.Foreground"/>
    [Bindable(true), Category("Appearance")]
    public Brush Foreground
    {
        get => (Brush)GetValue(ForegroundProperty);
        set => SetValue(ForegroundProperty, value);
    }

    protected override int VisualChildrenCount => 1;

    private Grid? _layoutRoot;

    #region Protected methods

    protected abstract UIElement InitializeChildren();

    protected virtual void OnForegroundPropertyChanged(DependencyPropertyChangedEventArgs args) { }

    #endregion

    #region Layout methods

    private void EnsureLayoutRoot()
    {
        if (_layoutRoot != null)
            return;

        _layoutRoot = new Grid
        {
            Background = Brushes.Transparent,
            SnapsToDevicePixels = true,
        };

        _layoutRoot.Children.Add(InitializeChildren());
        AddVisualChild(_layoutRoot);
    }

    protected override Visual GetVisualChild(int index)
    {
        if (index != 0)
            throw new ArgumentOutOfRangeException(nameof(index));

        EnsureLayoutRoot();
        return _layoutRoot!;
    }

    protected override Size MeasureOverride(Size availableSize)
    {
        EnsureLayoutRoot();

        _layoutRoot!.Measure(availableSize);
        return _layoutRoot.DesiredSize;
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        EnsureLayoutRoot();

        _layoutRoot!.Arrange(new Rect(new Point(), finalSize));
        return finalSize;
    }

    #endregion
}
