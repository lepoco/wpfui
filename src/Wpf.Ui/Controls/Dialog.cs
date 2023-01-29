// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

#nullable enable

using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows;
using Wpf.Ui.Common;

namespace Wpf.Ui.Controls;

/// <summary>
/// Displays a large card with a slightly transparent background and two action buttons.
/// </summary>
[ToolboxItem(true)]
[ToolboxBitmap(typeof(Dialog), "Dialog.bmp")]
public class Dialog : System.Windows.Controls.ContentControl, IDialogControl
{
    private TaskCompletionSource<DialogButtonPressed>? _tcs = null;

    private bool _automaticHide;

    /// <summary>
    /// Property for <see cref="IsFooterVisible"/>.
    /// </summary>
    public static readonly DependencyProperty IsFooterVisibleProperty = DependencyProperty.Register(nameof(IsFooterVisible),
        typeof(bool), typeof(Dialog), new PropertyMetadata(true));

    /// <summary>
    /// Property for <see cref="IsShown"/>.
    /// </summary>
    public static readonly DependencyProperty IsShownProperty = DependencyProperty.Register(nameof(IsShown),
        typeof(bool), typeof(Dialog), new PropertyMetadata(false, OnIsShownChange));

    /// <summary>
    /// Property for <see cref="Header"/>.
    /// </summary>
    public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(nameof(Header),
        typeof(object), typeof(Dialog), new PropertyMetadata(null));

    /// <summary>
    /// Property for <see cref="Footer"/>.
    /// </summary>
    public static readonly DependencyProperty FooterProperty = DependencyProperty.Register(nameof(Footer),
        typeof(object), typeof(Dialog), new PropertyMetadata(null));

    /// <summary>
    /// Property for <see cref="DialogWidth"/>.
    /// </summary>
    public static readonly DependencyProperty DialogWidthProperty =
        DependencyProperty.Register(nameof(DialogWidth),
            typeof(double), typeof(Dialog), new PropertyMetadata(420.0));

    /// <summary>
    /// Property for <see cref="DialogHeight"/>.
    /// </summary>
    public static readonly DependencyProperty DialogHeightProperty =
        DependencyProperty.Register(nameof(DialogHeight),
            typeof(double), typeof(Dialog), new PropertyMetadata(200.0));

    /// <summary>
    /// Property for <see cref="ButtonLeft"/>.
    /// </summary>
    public static readonly DependencyProperty ButtonLeftProperty = DependencyProperty.Register(
        nameof(ButtonLeft),
        typeof(System.Windows.Controls.Primitives.ButtonBase), typeof(Dialog), new PropertyMetadata(null));

    /// <summary>
    /// Property for <see cref="ButtonRight"/>.
    /// </summary>
    public static readonly DependencyProperty ButtonRightProperty = DependencyProperty.Register(
        nameof(ButtonRight),
        typeof(System.Windows.Controls.Primitives.ButtonBase), typeof(Dialog), new PropertyMetadata(null));

    /// <summary>
    /// Property for <see cref="TemplateButtonCommand"/>.
    /// </summary>
    public static readonly DependencyProperty TemplateButtonCommandProperty =
        DependencyProperty.Register(nameof(TemplateButtonCommand),
            typeof(Common.IRelayCommand), typeof(Dialog), new PropertyMetadata(null));

    /// <summary>
    /// Gets a value indicating whether the Footer grid with text and/or buttons should be displayed.
    /// </summary>
    public bool IsFooterVisible
    {
        get => (bool)GetValue(IsFooterVisibleProperty);
        internal set => SetValue(IsFooterVisibleProperty, value);
    }

    /// <inheritdoc />
    public bool IsShown
    {
        get => (bool)GetValue(IsShownProperty);
        protected set => SetValue(IsShownProperty, value);
    }

    /// <inheritdoc />
    public object Footer
    {
        get => GetValue(FooterProperty);
        set => SetValue(FooterProperty, value);
    }

    /// <inheritdoc />
    public object Header
    {
        get => GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }

    /// <inheritdoc />
    public double DialogWidth
    {
        get => (int)GetValue(DialogWidthProperty);
        set => SetValue(DialogWidthProperty, value);
    }

    /// <inheritdoc />
    public double DialogHeight
    {
        get => (int)GetValue(DialogHeightProperty);
        set => SetValue(DialogHeightProperty, value);
    }

    /// <inheritdoc />
    public System.Windows.Controls.Primitives.ButtonBase ButtonLeft
    {
        get => (System.Windows.Controls.Primitives.ButtonBase)GetValue(ButtonLeftProperty);
        set => SetValue(ButtonLeftProperty, value);
    }

    /// <inheritdoc />
    public System.Windows.Controls.Primitives.ButtonBase ButtonRight
    {
        get => (System.Windows.Controls.Primitives.ButtonBase)GetValue(ButtonRightProperty);
        set => SetValue(ButtonRightProperty, value);
    }

    /// <summary>
    /// Command triggered after clicking the button in the template.
    /// </summary>
    public Common.IRelayCommand TemplateButtonCommand =>
        (Common.IRelayCommand)GetValue(TemplateButtonCommandProperty);

    /// <summary>
    /// Event triggered when <see cref="Dialog"/> opens.
    /// </summary>
    public static readonly RoutedEvent OpenedEvent = EventManager.RegisterRoutedEvent(nameof(Opened),
        RoutingStrategy.Bubble, typeof(RoutedDialogEvent), typeof(Dialog));

    /// <inheritdoc />
    public event RoutedDialogEvent Opened
    {
        add => AddHandler(OpenedEvent, value);
        remove => RemoveHandler(OpenedEvent, value);
    }

    /// <summary>
    /// Event triggered when <see cref="Dialog"/> opens.
    /// </summary>
    public static readonly RoutedEvent ClosedEvent = EventManager.RegisterRoutedEvent(nameof(Closed),
        RoutingStrategy.Bubble, typeof(RoutedDialogEvent), typeof(Dialog));

    /// <inheritdoc />
    public event RoutedDialogEvent Closed
    {
        add => AddHandler(ClosedEvent, value);
        remove => RemoveHandler(ClosedEvent, value);
    }

    /// <summary>
    /// Creates new instance and sets default <see cref="TemplateButtonCommandProperty"/>.
    /// </summary>
    public Dialog()
    {
        SetValue(TemplateButtonCommandProperty, new Common.RelayCommand<string>(o => OnTemplateButtonClick(o ?? String.Empty)));
    }

    /// <inheritdoc />
    public Task<DialogButtonPressed> ShowAndWaitAsync()
    {
        _automaticHide = false;

        Show();

        _tcs = new TaskCompletionSource<DialogButtonPressed>();

        return _tcs.Task;
    }

    /// <inheritdoc />
    public Task<DialogButtonPressed> ShowAndWaitAsync(bool hideOnClick)
    {
        _automaticHide = hideOnClick;

        Show();

        _tcs = new TaskCompletionSource<DialogButtonPressed>();

        return _tcs.Task;
    }

    /// <inheritdoc />
    public Task<DialogButtonPressed> ShowAndWaitAsync(string title, string message)
    {
        _automaticHide = false;

        if (IsShown)
            Hide();

        Show(title, message);

        _tcs = new TaskCompletionSource<DialogButtonPressed>();

        return _tcs.Task;
    }

    /// <inheritdoc />
    public Task<DialogButtonPressed> ShowAndWaitAsync(string title, string message, bool hideOnClick)
    {
        _automaticHide = hideOnClick;

        if (IsShown)
            Hide();

        Show(title, message);

        _tcs = new TaskCompletionSource<DialogButtonPressed>();

        return _tcs.Task;
    }

    /// <inheritdoc />
    public bool Show()
    {
        if (IsShown)
            return false;

        _automaticHide = false;

        IsShown = true;

        FocusFirstButton();

        return true;
    }

    /// <inheritdoc />
    public bool Show(string title, string message)
    {
        if (IsShown)
            Hide();

        _automaticHide = false;

        Header = title;
        Content = message;
        IsShown = true;

        FocusFirstButton();

        return true;
    }

    /// <inheritdoc />
    public bool Hide()
    {
        if (!IsShown)
            return false;

        IsShown = false;

        return true;
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        //if (GetTemplateChild(ElementFooterButtonLeft) is System.Windows.Controls.Primitives.ButtonBase leftButton)
        //    _leftFooterButton = leftButton;

        //if (GetTemplateChild(ElementFooterButtonLeft) is System.Windows.Controls.Primitives.ButtonBase rightButton)
        //    _rightFooterButton = rightButton;
    }

    /// <summary>
    /// This virtual method is called when <see cref="Dialog"/> is opening and it raises the <see cref="Opened"/> <see langword="event"/>.
    /// </summary>
    protected virtual void OnOpened()
    {
        var newEvent = new RoutedEventArgs(Dialog.OpenedEvent, this);
        RaiseEvent(newEvent);
    }

    /// <summary>
    /// This virtual method is called when <see cref="Dialog"/> is closing and it raises the <see cref="Closed"/> <see langword="event"/>.
    /// </summary>
    protected virtual void OnClosed()
    {
        var newEvent = new RoutedEventArgs(Dialog.ClosedEvent, this);
        RaiseEvent(newEvent);
    }

    /// <summary>
    /// Triggered by clicking a button in the control template.
    /// </summary>
    protected virtual void OnTemplateButtonClick(string parameter)
    {
        //#if DEBUG
        //        System.Diagnostics.Debug.WriteLine($"INFO: {typeof(Dialog)} button clicked with param: {parameter}",
        //            "Wpf.Ui.Dialog");
        //#endif

        //        switch (parameter)
        //        {
        //            case "left":
        //                RaiseEvent(new RoutedEventArgs(ButtonLeftClickEvent, this));

        //                _tcs?.TrySetResult(ButtonPressed.Left);

        //                break;

        //            case "right":
        //                RaiseEvent(new RoutedEventArgs(ButtonRightClickEvent, this));

        //                _tcs?.TrySetResult(ButtonPressed.Right);

        //                break;
        //        }

        //        if (_automaticHide)
        //            Hide();
    }

    private static void OnIsShownChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not Dialog control)
            return;

        if (control.IsShown)
            control.OnOpened();
        else
            control.OnClosed();
    }

    private void FocusFirstButton()
    {
        //if (Footer != null)
        //    return;

        //if (ButtonLeftVisibility == Visibility.Visible)
        //{
        //    if (_leftFooterButton != null)
        //        _leftFooterButton.Focus();

        //    return;
        //}

        //if (ButtonRightVisibility != Visibility.Visible)
        //    return;

        //if (_rightFooterButton != null)
        //    _rightFooterButton.Focus();
    }
}
