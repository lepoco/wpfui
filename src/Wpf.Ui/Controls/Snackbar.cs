// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Wpf.Ui.Common;
using Wpf.Ui.Controls.Interfaces;
using Brush = System.Windows.Media.Brush;
using SystemColors = System.Windows.SystemColors;

namespace Wpf.Ui.Controls;

/// <summary>
/// Snackbar inform user of a process that an app has performed or will perform. It appears temporarily, towards the bottom of the window.
/// </summary>
[ToolboxItem(true)]
[ToolboxBitmap(typeof(Snackbar), "Snackbar.bmp")]
public class Snackbar : System.Windows.Controls.ContentControl, ISnackbarControl, IIconControl, IAppearanceControl
{
    private readonly EventIdentifier _eventIdentifier;

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
    public static readonly DependencyProperty IconForegroundProperty = DependencyProperty.Register(
        nameof(IconForeground),
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
    public static readonly DependencyProperty MessageForegroundProperty = DependencyProperty.Register(
        nameof(MessageForeground),
        typeof(Brush), typeof(Snackbar), new FrameworkPropertyMetadata(SystemColors.ControlTextBrush,
            FrameworkPropertyMetadataOptions.Inherits));

    /// <summary>
    /// Property for <see cref="Appearance"/>.
    /// </summary>
    public static readonly DependencyProperty AppearanceProperty = DependencyProperty.Register(nameof(Appearance),
        typeof(Common.ControlAppearance), typeof(Snackbar),
        new PropertyMetadata(Common.ControlAppearance.Secondary));

    /// <summary>
    /// Property for <see cref="CloseButtonEnabled"/>.
    /// </summary>
    public static readonly DependencyProperty CloseButtonEnabledProperty = DependencyProperty.Register(
        nameof(CloseButtonEnabled),
        typeof(bool), typeof(Snackbar), new PropertyMetadata(true));

    /// <summary>
    /// Property for <see cref="SlideTransform"/>.
    /// </summary>
    public static readonly DependencyProperty SlideTransformProperty = DependencyProperty.Register(
        nameof(SlideTransform),
        typeof(TranslateTransform), typeof(Snackbar), new PropertyMetadata(new TranslateTransform()));

    /// <summary>
    /// Property for <see cref="TemplateButtonCommand"/>.
    /// </summary>
    public static readonly DependencyProperty TemplateButtonCommandProperty =
        DependencyProperty.Register(nameof(TemplateButtonCommand),
            typeof(Common.IRelayCommand), typeof(Snackbar), new PropertyMetadata(null));

    /// <inheritdoc/>
    public bool IsShown
    {
        get => (bool)GetValue(IsShownProperty);
        protected set => SetValue(IsShownProperty, value);
    }

    /// <inheritdoc/>
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
    /// Foreground of the <see cref="Wpf.Ui.Controls.SymbolIcon"/>.
    /// </summary>
    [Bindable(true), Category("Appearance")]
    public Brush IconForeground
    {
        get => (Brush)GetValue(IconForegroundProperty);
        set => SetValue(IconForegroundProperty, value);
    }

    /// <inheritdoc/>
    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    /// <inheritdoc/>
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

    /// <inheritdoc />
    public bool CloseButtonEnabled
    {
        get => (bool)GetValue(CloseButtonEnabledProperty);
        set => SetValue(CloseButtonEnabledProperty, value);
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
    /// Event triggered when <see cref="Snackbar"/> opens.
    /// </summary>
    public static readonly RoutedEvent OpenedEvent = EventManager.RegisterRoutedEvent(nameof(Opened),
        RoutingStrategy.Bubble, typeof(RoutedSnackbarEvent), typeof(Snackbar));

    /// <inheritdoc />
    public event RoutedSnackbarEvent Opened
    {
        add => AddHandler(OpenedEvent, value);
        remove => RemoveHandler(OpenedEvent, value);
    }

    /// <summary>
    /// Event triggered when <see cref="Snackbar"/> opens.
    /// </summary>
    public static readonly RoutedEvent ClosedEvent = EventManager.RegisterRoutedEvent(nameof(Closed),
        RoutingStrategy.Bubble, typeof(RoutedSnackbarEvent), typeof(Snackbar));

    /// <inheritdoc />
    public event RoutedSnackbarEvent Closed
    {
        add => AddHandler(ClosedEvent, value);
        remove => RemoveHandler(ClosedEvent, value);
    }

    /// <summary>
    /// Gets the <see cref="Common.RelayCommand"/> triggered after clicking close button.
    /// </summary>
    public Common.IRelayCommand TemplateButtonCommand => (Common.IRelayCommand)GetValue(TemplateButtonCommandProperty);


    /// <inheritdoc />
    public Snackbar()
    {
        _eventIdentifier = new EventIdentifier();

        SetValue(TemplateButtonCommandProperty, new Common.RelayCommand(o => OnTemplateButtonClick(this, o)));
    }

    /// <inheritdoc />
    public bool Show()
    {
#pragma warning disable CS4014
        ShowComponentAsync();
#pragma warning restore CS4014

        return true;
    }

    /// <inheritdoc />
    public bool Show(string title)
    {
#pragma warning disable CS4014
        ShowComponentAsync(title);
#pragma warning restore CS4014

        return true;
    }

    /// <inheritdoc />
    public bool Show(string title, string message)
    {
#pragma warning disable CS4014
        ShowComponentAsync(title, message);
#pragma warning restore CS4014

        return true;
    }

    /// <inheritdoc />
    public bool Show(string title, string message, SymbolRegular icon)
    {
#pragma warning disable CS4014
        ShowComponentAsync(title, message, icon);
#pragma warning restore CS4014

        return true;
    }

    /// <inheritdoc />
    public bool Show(string title, string message, SymbolRegular icon, ControlAppearance appearance)
    {
#pragma warning disable CS4014
        ShowComponentAsync(title, message, icon, appearance);
#pragma warning restore CS4014

        return true;
    }

    /// <inheritdoc />
    public async Task<bool> ShowAsync()
        => await ShowComponentAsync();

    /// <inheritdoc />
    public async Task<bool> ShowAsync(string title)
        => await ShowComponentAsync(title);

    /// <inheritdoc />
    public async Task<bool> ShowAsync(string title, string message)
        => await ShowComponentAsync(title, message);

    /// <inheritdoc />
    public async Task<bool> ShowAsync(string title, string message, SymbolRegular icon)
        => await ShowComponentAsync(title, message, icon);

    /// <inheritdoc />
    public async Task<bool> ShowAsync(string title, string message, SymbolRegular icon, ControlAppearance appearance)
        => await ShowComponentAsync(title, message, icon, appearance);

    /// <inheritdoc />
    public bool Hide()
    {
#pragma warning disable CS4014
        HideComponentAsync(0);
#pragma warning restore CS4014

        return true;
    }

    /// <inheritdoc />
    public async Task<bool> HideAsync()
    {
        return await HideComponentAsync(0);
    }

    /// <summary>
    /// Triggered by clicking a button in the control template.
    /// </summary>
    /// <param name="sender">Sender of the click event.</param>
    /// <param name="parameter">Additional parameters.</param>
    protected virtual async void OnTemplateButtonClick(object sender, object parameter)
    {
        if (parameter is not String parameterString)
            return;

        if (parameterString == "close")
            await HideAsync();
    }

    /// <summary>
    /// This virtual method is called when <see cref="Snackbar"/> is opening and it raises the <see cref="Opened"/> <see langword="event"/>.
    /// </summary>
    protected virtual void OnOpened()
    {
        RaiseEvent(new RoutedEventArgs(
            Snackbar.OpenedEvent,
            this));
    }

    /// <summary>
    /// This virtual method is called when <see cref="Snackbar"/> is closing and it raises the <see cref="Closed"/> <see langword="event"/>.
    /// </summary>
    protected virtual void OnClosed()
    {
        RaiseEvent(new RoutedEventArgs(
            Snackbar.ClosedEvent,
            this));
    }

    private async Task<bool> ShowComponentAsync()
    {
        await HideIfVisible();

        IsShown = true;

        OnOpened();

        if (Timeout > 0)
            await HideComponentAsync(Timeout);

        return true;
    }

    private async Task<bool> ShowComponentAsync(string title)
    {
        await HideIfVisible();

        Title = title;
        IsShown = true;

        OnOpened();

        if (Timeout > 0)
            await HideComponentAsync(Timeout);

        return true;
    }

    private async Task<bool> ShowComponentAsync(string title, string message)
    {
        await HideIfVisible();

        Title = title;
        Message = message;
        IsShown = true;

        OnOpened();

        if (Timeout > 0)
            await HideComponentAsync(Timeout);

        return true;
    }

    private async Task<bool> ShowComponentAsync(string title, string message, SymbolRegular icon)
    {
        await HideIfVisible();

        Title = title;
        Message = message;
        Icon = icon;

        IsShown = true;

        OnOpened();

        if (Timeout > 0)
            await HideComponentAsync(Timeout);

        return true;
    }

    private async Task<bool> ShowComponentAsync(string title, string message, SymbolRegular icon, ControlAppearance appearance)
    {
        await HideIfVisible();

        Title = title;
        Message = message;
        Icon = icon;
        Appearance = appearance;

        IsShown = true;

        OnOpened();

        if (Timeout > 0)
            await HideComponentAsync(Timeout);

        return true;
    }

    private async Task<bool> HideComponentAsync(int timeout)
    {
        if (!IsShown)
            return false;

        if (timeout < 1)
            IsShown = false;

        var currentEvent = _eventIdentifier.GetNext();

        await Task.Delay(timeout);

        if (Application.Current == null)
            return false;

        if (!_eventIdentifier.IsEqual(currentEvent))
            return false;

        IsShown = false;

        OnClosed();

        return true;
    }

    private async Task HideIfVisible()
    {
        if (!IsShown)
            return;

        await HideComponentAsync(0);

        await Task.Delay(300);
    }
}
