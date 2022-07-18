// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Wpf.Ui.Common;
using Wpf.Ui.Controls.Interfaces;
using Wpf.Ui.Controls.Navigation;
using Wpf.Ui.Extensions;
using Brush = System.Windows.Media.Brush;
using SystemColors = System.Windows.SystemColors;

namespace Wpf.Ui.Controls;

/// <summary>
/// Navigation element.
/// </summary>
[ToolboxItem(true)]
[ToolboxBitmap(typeof(NavigationItem), "NavigationItem.bmp")]
public class NavigationItem : System.Windows.Controls.Primitives.ButtonBase, IUriContext, INavigationItem, INavigationControl, IIconControl
{
    /// <summary>
    /// Property for <see cref="PageTag"/>.
    /// </summary>
    public static readonly DependencyProperty PageTagProperty = DependencyProperty.Register(nameof(PageTag),
        typeof(string), typeof(NavigationItem), new PropertyMetadata(String.Empty));

    /// <summary>
    /// Property for <see cref="PageSource"/>.
    /// </summary>
    public static readonly DependencyProperty PageSourceProperty = DependencyProperty.Register(nameof(PageSource),
        typeof(Uri), typeof(NavigationItem), new PropertyMetadata((Uri)null, OnPageSourceChanged));

    /// <summary>
    /// Property for <see cref="PageType"/>.
    /// </summary>
    public static readonly DependencyProperty PageTypeProperty = DependencyProperty.Register(nameof(PageType),
        typeof(Type), typeof(NavigationItem), new PropertyMetadata((Type)null, OnPageTypeChanged));

    /// <summary>
    /// Property for <see cref="IsActive"/>.
    /// </summary>
    public static readonly DependencyProperty IsActiveProperty = DependencyProperty.Register(nameof(IsActive),
        typeof(bool), typeof(NavigationItem), new PropertyMetadata(false));

    /// <summary>
    /// Property for <see cref="Cache"/>.
    /// </summary>
    public static readonly DependencyProperty CacheProperty = DependencyProperty.Register(nameof(Cache),
        typeof(bool), typeof(NavigationItem), new PropertyMetadata(true));

    /// <summary>
    /// Property for <see cref="Icon"/>.
    /// </summary>
    public static readonly DependencyProperty IconProperty = DependencyProperty.Register(nameof(Icon),
        typeof(Common.SymbolRegular), typeof(NavigationItem),
        new PropertyMetadata(Common.SymbolRegular.Empty));

    /// <summary>
    /// Property for <see cref="IconSize"/>.
    /// </summary>
    public static readonly DependencyProperty IconSizeProperty = DependencyProperty.Register(nameof(IconSize),
        typeof(double), typeof(NavigationItem),
        new PropertyMetadata(18d));

    /// <summary>
    /// Property for <see cref="IconFilled"/>.
    /// </summary>
    public static readonly DependencyProperty IconFilledProperty = DependencyProperty.Register(nameof(IconFilled),
        typeof(bool), typeof(NavigationItem), new PropertyMetadata(false));

    /// <summary>
    /// Property for <see cref="IconForeground"/>.
    /// </summary>
    public static readonly DependencyProperty IconForegroundProperty = DependencyProperty.Register(nameof(IconForeground),
        typeof(Brush), typeof(NavigationItem), new FrameworkPropertyMetadata(SystemColors.ControlTextBrush,
            FrameworkPropertyMetadataOptions.Inherits));

    /// <summary>
    /// Property for <see cref="Image"/>.
    /// </summary>
    public static readonly DependencyProperty ImageProperty = DependencyProperty.Register(nameof(Image),
        typeof(BitmapSource), typeof(NavigationItem),
        new PropertyMetadata(null));

    /// <summary>
    /// Routed event for <see cref="Activated"/>.
    /// </summary>
    public static readonly RoutedEvent ActivatedEvent = EventManager.RegisterRoutedEvent(
        nameof(Activated), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(NavigationItem));

    /// <summary>
    /// Routed event for <see cref="Deactivated"/>.
    /// </summary>
    public static readonly RoutedEvent DeactivatedEvent = EventManager.RegisterRoutedEvent(
        nameof(Deactivated), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(NavigationItem));

    /// <inheritdoc />
    public string PageTag
    {
        get => (string)GetValue(PageTagProperty);
        set => SetValue(PageTagProperty, value);
    }

    /// <inheritdoc />
    public Uri PageSource
    {
        get => (Uri)GetValue(PageSourceProperty);
        set => SetValue(PageSourceProperty, value);

    }

    /// <inheritdoc />
    public Type PageType
    {
        get => (Type)GetValue(PageTypeProperty);
        set => SetValue(PageTypeProperty, value);
    }

    /// <inheritdoc />
    public bool IsActive
    {
        get => (bool)GetValue(IsActiveProperty);
        set
        {
            if (value == IsActive)
                return;

            RaiseEvent(value
                ? new RoutedEventArgs(ActivatedEvent, this)
                : new RoutedEventArgs(DeactivatedEvent, this));

            SetValue(IsActiveProperty, value);
        }
    }

    /// <inheritdoc />
    public bool Cache
    {
        get => (bool)GetValue(CacheProperty);
        set => SetValue(CacheProperty, value);
    }

    /// <inheritdoc />
    [Bindable(true), Category("Appearance")]
    [Localizability(LocalizationCategory.None)]
    public Common.SymbolRegular Icon
    {
        get => (Common.SymbolRegular)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    /// <inheritdoc />
    [Bindable(true), Category("Appearance")]
    [Localizability(LocalizationCategory.None)]
    public bool IconFilled
    {
        get => (bool)GetValue(IconFilledProperty);
        set => SetValue(IconFilledProperty, value);
    }

    /// <summary>
    /// Size of the <see cref="Wpf.Ui.Controls.SymbolIcon"/>.
    /// </summary>
    [TypeConverter(typeof(FontSizeConverter))]
    [Bindable(true), Category("Appearance")]
    [Localizability(LocalizationCategory.None)]
    public double IconSize
    {
        get => (double)GetValue(IconSizeProperty);
        set => SetValue(IconSizeProperty, value);
    }

    /// <summary>
    /// Foreground of the <see cref="Wpf.Ui.Controls.SymbolIcon"/>.
    /// </summary>
    [Bindable(true), Category("Appearance")]
    public Brush IconForeground
    {
        get => (Brush)GetValue(IconForegroundProperty);
        set => SetValue(IconForegroundProperty, value);
    }

    /// <summary>
    /// Gets or sets image displayed next to the card name instead of the icon.
    /// </summary>
    [Bindable(true), Category("Appearance")]
    public BitmapSource Image
    {
        get => GetValue(ImageProperty) as BitmapSource;
        set => SetValue(ImageProperty, value);
    }

    /// <summary>
    /// Occurs when <see cref="NavigationItem"/> is activated via <see cref="IsActive"/>.
    /// </summary>
    public event RoutedEventHandler Activated
    {
        add => AddHandler(ActivatedEvent, value);
        remove => RemoveHandler(ActivatedEvent, value);
    }

    /// <summary>
    /// Occurs when <see cref="NavigationItem"/> is deactivated via <see cref="IsActive"/>.
    /// </summary>
    public event RoutedEventHandler Deactivated
    {
        add => AddHandler(DeactivatedEvent, value);
        remove => RemoveHandler(DeactivatedEvent, value);
    }

    /// <inheritdoc />
    [Bindable(false)]
    public Uri AbsolutePageSource { get; internal set; }

    /// <inheritdoc />
    Uri IUriContext.BaseUri
    {
        get => BaseUri;
        set => BaseUri = value;
    }

    /// <summary>
    /// Implementation for BaseUri.
    /// </summary>
    protected virtual Uri BaseUri
    {
        get => (Uri)GetValue(BaseUriHelper.BaseUriProperty);
        set => SetValue(BaseUriHelper.BaseUriProperty, value);
    }

    /// <inheritdoc />
    protected override void OnContentChanged(object oldContent, object newContent)
    {
        base.OnContentChanged(oldContent, newContent);

        if (newContent is string && String.IsNullOrEmpty(PageTag))
            PageTag = newContent?.ToString()?.ToLower()?.Trim() ?? String.Empty;
    }

    /// <inheritdoc/>
    protected override void OnKeyDown(KeyEventArgs e)
    {
        if (Keyboard.Modifiers is not ModifierKeys.None)
        {
            // We handle Left/Up/Right/Down keys for keyboard navigation only,
            // so no modifiers are needed.
            return;
        }

        switch (e.Key)
        {
            // We use Direction Left/Up/Right/Down instead of Previous/Next to make sure
            // that the KeyboardNavigation.DirectionalNavigation property works correctly.
            case Key.Left:
                MoveFocus(this, FocusNavigationDirection.Left);
                e.Handled = true;
                break;

            case Key.Up:
                MoveFocus(this, FocusNavigationDirection.Up);
                e.Handled = true;
                break;

            case Key.Right:
                MoveFocus(this, FocusNavigationDirection.Right);
                e.Handled = true;
                break;

            case Key.Down:
                MoveFocus(this, FocusNavigationDirection.Down);
                e.Handled = true;
                break;

            case Key.Space:
            case Key.Enter:

                // Item doesn't define a page, skip navigation.
                if (PageSource == null && PageType == null)
                    break;

                if (NavigationBase.GetNavigationParent(this) is { } navigation
                    && PageTag is { } pageTag
                    && !String.IsNullOrEmpty(pageTag))
                {
                    e.Handled = true;

                    navigation.Navigate(pageTag);
                }
                break;
        }

        // If it is simply treated as a button, pass the information about the click on.
        if (!e.Handled)
            base.OnKeyDown(e);

        static void MoveFocus(FrameworkElement element, FocusNavigationDirection direction)
        {
            var request = new TraversalRequest(direction);
            element.MoveFocus(request);
        }
    }

    /// <summary>
    /// This virtual method is called when <see cref="Uri"/> of the selected page is changed.
    /// </summary>
    protected virtual void OnPageSourceChanged(Uri pageUri)
    {
        if (!pageUri.OriginalString.EndsWith(".xaml"))
            throw new ArgumentException($"URI in {typeof(NavigationItem)} must point to the XAML Page.");

        AbsolutePageSource = ResolvePageUri(pageUri);
    }

    private static void OnPageSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not NavigationItem navigationItem)
            return;

        navigationItem.OnPageSourceChanged(e.NewValue as Uri);
    }

    /// <summary>
    /// This virtual method is called when <see cref="Type"/> of the selected page is changed.
    /// </summary>
    protected virtual void OnPageTypeChanged(Type pageType)
    {
        if (pageType == null)
            return;

        if (!typeof(System.Windows.FrameworkElement).IsAssignableFrom(pageType))
            throw new ArgumentException($"{pageType} is not inherited from {typeof(System.Windows.FrameworkElement)}.");
    }

    private static void OnPageTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not NavigationItem navigationItem)
            return;

        navigationItem.OnPageTypeChanged(e.NewValue as Type);
    }

    /// <summary>
    /// Tries to resolve absolute path to the Page template.
    /// </summary>
    private Uri ResolvePageUri(Uri pageUri)
    {
        if (pageUri == null || pageUri.IsAbsoluteUri)
            return pageUri;

        if (!pageUri.EndsWith(".xaml"))
            throw new ArgumentException("PageSource must point to the .xaml file.");

        var baseUri = BaseUri;

        if (baseUri == null)
        {
            // TODO: Force extracting BaseUri for Designer
            // This is a hackery solution that needs to be refined.

            if (!DesignerHelper.IsInDesignMode)
                throw new UriFormatException("Unable to resolve absolute URI for selected page");

            // The navigation simply prints a blank page during the design process.
            PageType = typeof(System.Windows.Controls.Page);

            return null;
        }

        if (!baseUri.IsAbsoluteUri)
            throw new ApplicationException("Unable to resolve base URI for selected page");

        if (baseUri.EndsWith(".xaml"))
            baseUri = baseUri.TrimLastSegment();

        return baseUri.Append(pageUri);
    }
}
