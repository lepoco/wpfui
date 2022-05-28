// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using WPFUI.Common;
using WPFUI.Controls.Interfaces;

namespace WPFUI.Controls;

/// <summary>
/// Small card with buttons displayed at the bottom for a short time.
/// </summary>
public class Snackbar : System.Windows.Controls.ContentControl, IIconControl, IAppearanceControl
{
    private readonly EventIdentifier _identifier = new();

    /// <summary>
    /// Property for <see cref="IsShown"/>.
    /// </summary>
    public static readonly DependencyProperty IsShownProperty = DependencyProperty.Register(nameof(IsShown),
        typeof(bool), typeof(Snackbar), new PropertyMetadata(false));

    /// <summary>
    /// Property for <see cref="Timeout"/>.
    /// </summary>
    public static readonly DependencyProperty TimeoutProperty = DependencyProperty.Register(nameof(Timeout),
        typeof(int), typeof(Snackbar), new PropertyMetadata(2000));

    /// <summary>
    /// Property for <see cref="Icon"/>.
    /// </summary>
    public static readonly DependencyProperty IconProperty = DependencyProperty.Register(nameof(Icon),
        typeof(Common.SymbolRegular), typeof(Snackbar),
        new PropertyMetadata(Common.SymbolRegular.Empty));

    /// <summary>
    /// Property for <see cref="IconFilled"/>.
    /// </summary>
    public static readonly DependencyProperty IconFilledProperty = DependencyProperty.Register(nameof(IconFilled),
        typeof(bool), typeof(Snackbar), new PropertyMetadata(false));

    /// <summary>
    /// Property for <see cref="IconForeground"/>.
    /// </summary>
    public static readonly DependencyProperty IconForegroundProperty = DependencyProperty.Register(nameof(IconForeground),
        typeof(Brush), typeof(Snackbar), new FrameworkPropertyMetadata(SystemColors.ControlTextBrush,
            FrameworkPropertyMetadataOptions.Inherits));

    /// <summary>
    /// Property for <see cref="Title"/>.
    /// </summary>
    public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(nameof(Title),
        typeof(string), typeof(Snackbar), new PropertyMetadata(String.Empty));

    /// <summary>
    /// Property for <see cref="Message"/>.
    /// </summary>
    public static readonly DependencyProperty MessageProperty = DependencyProperty.Register(nameof(Message),
        typeof(string), typeof(Snackbar), new PropertyMetadata(String.Empty));

    /// <summary>
    /// Property for <see cref="MessageForeground"/>.
    /// </summary>
    public static readonly DependencyProperty MessageForegroundProperty = DependencyProperty.Register(nameof(MessageForeground),
        typeof(Brush), typeof(Snackbar), new FrameworkPropertyMetadata(SystemColors.ControlTextBrush,
            FrameworkPropertyMetadataOptions.Inherits));

    /// <summary>
    /// Property for <see cref="Appearance"/>.
    /// </summary>
    public static readonly DependencyProperty AppearanceProperty = DependencyProperty.Register(nameof(Appearance),
        typeof(Common.ControlAppearance), typeof(Snackbar),
        new PropertyMetadata(Common.ControlAppearance.Secondary));

    /// <summary>
    /// Property for <see cref="ShowCloseButton"/>.
    /// </summary>
    public static readonly DependencyProperty ShowCloseButtonProperty = DependencyProperty.Register(nameof(ShowCloseButton),
        typeof(bool), typeof(Snackbar), new PropertyMetadata(true));

    /// <summary>
    /// Property for <see cref="SlideTransform"/>.
    /// </summary>
    public static readonly DependencyProperty SlideTransformProperty = DependencyProperty.Register(nameof(SlideTransform),
        typeof(TranslateTransform), typeof(Snackbar), new PropertyMetadata(new TranslateTransform()));

    /// <summary>
    /// Property for <see cref="ButtonCloseCommand"/>.
    /// </summary>
    public static readonly DependencyProperty ButtonCloseCommandProperty =
        DependencyProperty.Register(nameof(ButtonCloseCommand),
            typeof(Common.IRelayCommand), typeof(Snackbar), new PropertyMetadata(null));

    /// <summary>
    /// Gets or sets information whether the snackbar should be displayed.
    /// </summary>
    public bool IsShown
    {
        get => (bool)GetValue(IsShownProperty);
        set => SetValue(IsShownProperty, value);
    }

    /// <summary>
    /// Time for which the snackbar is to be displayed.
    /// </summary>
    public int Timeout
    {
        get => (int)GetValue(TimeoutProperty);
        set => SetValue(TimeoutProperty, value);
    }

    /// <inheritdoc />
    [Bindable(true), Category("Appearance")]
    public Common.SymbolRegular Icon
    {
        get => (Common.SymbolRegular)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    /// <inheritdoc />
    [Bindable(true), Category("Appearance")]
    public bool IconFilled
    {
        get => (bool)GetValue(IconFilledProperty);
        set => SetValue(IconFilledProperty, value);
    }

    /// <summary>
    /// Foreground of the <see cref="WPFUI.Controls.SymbolIcon"/>.
    /// </summary>
    [Bindable(true), Category("Appearance")]
    public Brush IconForeground
    {
        get => (Brush)GetValue(IconForegroundProperty);
        set => SetValue(IconForegroundProperty, value);
    }

    /// <summary>
    /// Gets or sets the text displayed on the top of the snackbar.
    /// </summary>
    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    /// <summary>
    /// Gets or sets the text displayed on the bottom of the snackbar.
    /// </summary>
    public string Message
    {
        get => (string)GetValue(MessageProperty);
        set => SetValue(MessageProperty, value);
    }

    /// <summary>
    /// Foreground of the <see cref="Message"/>.
    /// </summary>
    [Bindable(true), Category("Appearance")]
    public Brush MessageForeground
    {
        get => (Brush)GetValue(MessageForegroundProperty);
        set => SetValue(MessageForegroundProperty, value);
    }

    /// <inheritdoc />
    [Bindable(true), Category("Appearance")]
    public Common.ControlAppearance Appearance
    {
        get => (Common.ControlAppearance)GetValue(AppearanceProperty);
        set => SetValue(AppearanceProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the <see cref="Snackbar"/> close button should be visible.
    /// </summary>
    public bool ShowCloseButton
    {
        get => (bool)GetValue(ShowCloseButtonProperty);
        set => SetValue(ShowCloseButtonProperty, value);
    }

    /// <summary>
    /// Gets or sets the transform.
    /// </summary>
    public TranslateTransform SlideTransform
    {
        get => (TranslateTransform)GetValue(SlideTransformProperty);
        set => SetValue(SlideTransformProperty, value);
    }

    /// <summary>
    /// Gets the <see cref="Common.RelayCommand"/> triggered after clicking close button.
    /// </summary>
    public Common.IRelayCommand ButtonCloseCommand => (Common.IRelayCommand)GetValue(ButtonCloseCommandProperty);

    /// <summary>
    /// Event triggered when <see cref="Snackbar"/> opens.
    /// </summary>
    public static readonly RoutedEvent OpenedEvent = EventManager.RegisterRoutedEvent(nameof(Opened), RoutingStrategy.Bubble, typeof(RoutedSnackbarEvent), typeof(Snackbar));

    /// <summary>
    /// Add / Remove <see cref="OpenedEvent"/> handler.
    /// </summary>
    public event RoutedSnackbarEvent Opened
    {
        add => AddHandler(OpenedEvent, value);
        remove => RemoveHandler(OpenedEvent, value);
    }

    /// <summary>
    /// Event triggered when <see cref="Snackbar"/> opens.
    /// </summary>
    public static readonly RoutedEvent ClosedEvent = EventManager.RegisterRoutedEvent(nameof(Closed), RoutingStrategy.Bubble, typeof(RoutedSnackbarEvent), typeof(Snackbar));

    /// <summary>
    /// Add / Remove <see cref="ClosedEvent"/> handler.
    /// </summary>
    public event RoutedSnackbarEvent Closed
    {
        add => AddHandler(ClosedEvent, value);
        remove => RemoveHandler(ClosedEvent, value);
    }

    /// <summary>
    /// Creates new instance and sets default <see cref="ButtonCloseCommand"/>.
    /// </summary>
    public Snackbar() => SetValue(ButtonCloseCommandProperty, new Common.RelayCommand(o => HideComponentAsync(0).GetAwaiter()));

    /// <summary>
    /// Shows the <see cref="Snackbar"/> for the amount of time specified in <see cref="Timeout"/>.
    /// </summary>
    public void Show() => ShowComponentAsync(String.Empty, String.Empty).GetAwaiter();

    /// <summary>
    /// Sets <see cref="Title"/> and <see cref="Message"/>, then shows the <see cref="Snackbar"/> for the amount of time specified in <see cref="Timeout"/>.
    /// </summary>
    public void Show(string title, string message) => ShowComponentAsync(title, message).GetAwaiter();

    /// <summary>
    /// Asynchronously shows the <see cref="Snackbar"/> for the amount of time specified in <see cref="Timeout"/>.
    /// </summary>
    public async Task<bool> ShowAsync() => await ShowComponentAsync(String.Empty, String.Empty);

    /// <summary>
    /// Asynchronously sets <see cref="Title"/> and <see cref="Message"/>, then shows the <see cref="Snackbar"/> for the amount of time specified in <see cref="Timeout"/>.
    /// </summary>
    public async Task<bool> ShowAsync(string title, string message) => await ShowComponentAsync(title, message);

    /// <summary>
    /// Hides <see cref="Snackbar"/>.
    /// </summary>
    public void Hide() => HideComponentAsync(0).GetAwaiter();

    /// <summary>
    /// Hides <see cref="Snackbar"/> after provided timeout.
    /// </summary>
    public void Hide(int timeout) => HideComponentAsync(timeout).GetAwaiter();

    /// <summary>
    /// Asynchronously hides <see cref="Snackbar"/>.
    /// </summary>
    public async Task<bool> HideAsync() => await HideComponentAsync(0);

    /// <summary>
    /// Asynchronously ides <see cref="Snackbar"/> after provided timeout.
    /// </summary>
    public async Task<bool> HideAsync(int timeout) => await HideComponentAsync(timeout);

    /// <summary>
    /// This virtual method is called by <see cref="Show()"/> or <see cref="ShowAsync()"/> to reveal the <see cref="Snackbar"/>.
    /// </summary>
    protected virtual async Task<bool> ShowComponentAsync(string title, string message)
    {
        if (IsShown)
        {
            await HideComponentAsync(0);

            await Task.Delay(300);
        }

        if (!String.IsNullOrWhiteSpace(title))
            Title = title;

        if (!String.IsNullOrWhiteSpace(message))
            Message = message;

        IsShown = true;

        OnOpened();

        if (Timeout > 0)
            await HideComponentAsync(Timeout);

        return true;
    }

    /// <summary>
    /// This virtual method is called by <see cref="Hide()"/> or <see cref="HideAsync()"/> to collapse the <see cref="Snackbar"/>.
    /// </summary>
    protected virtual async Task<bool> HideComponentAsync(int timeout)
    {
        if (!IsShown)
            return false;

        if (timeout < 1)
            IsShown = false;

        var currentEvent = _identifier.GetNext();

        await Task.Delay(timeout);

        if (Application.Current == null)
            return false;

        if (!_identifier.IsEqual(currentEvent))
            return false;

        IsShown = false;

        OnClosed();

        return true;
    }

    /// <summary>
    /// This virtual method is called when <see cref="Snackbar"/> is opening and it raises the <see cref="Opened"/> <see langword="event"/>.
    /// </summary>
    protected virtual void OnOpened()
    {
        var newEvent = new RoutedEventArgs(Snackbar.OpenedEvent, this);
        RaiseEvent(newEvent);
    }

    /// <summary>
    /// This virtual method is called when <see cref="Snackbar"/> is closing and it raises the <see cref="Closed"/> <see langword="event"/>.
    /// </summary>
    protected virtual void OnClosed()
    {
        var newEvent = new RoutedEventArgs(Snackbar.ClosedEvent, this);
        RaiseEvent(newEvent);
    }
}
