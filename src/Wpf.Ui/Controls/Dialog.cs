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
using Wpf.Ui.Controls.Interfaces;
using static Wpf.Ui.Controls.Interfaces.IDialogControl;

namespace Wpf.Ui.Controls;

/// <summary>
/// Displays a large card with a slightly transparent background and two action buttons.
/// </summary>
[ToolboxItem(true)]
[ToolboxBitmap(typeof(Dialog), "Dialog.bmp")]
[TemplatePart(Name = "PART_FooterButtonLeft", Type = typeof(System.Windows.Controls.Primitives.ButtonBase))]
[TemplatePart(Name = "PART_FooterButtonRight", Type = typeof(System.Windows.Controls.Primitives.ButtonBase))]
public class Dialog : System.Windows.Controls.ContentControl, IDialogControl
{
    private TaskCompletionSource<ButtonPressed>? _tcs = null;

    private bool _automaticHide;

    private System.Windows.Controls.Primitives.ButtonBase? _leftFooterButton = null;

    private System.Windows.Controls.Primitives.ButtonBase? _rightFooterButton = null;

    /// <summary>
    /// Template element represented by the <c>PART_FooterButtonLeft</c> name.
    /// </summary>
    private const string ElementFooterButtonLeft = "PART_FooterButtonLeft";

    /// <summary>
    /// Template element represented by the <c>PART_FooterButtonRight</c> name.
    /// </summary>
    private const string ElementFooterButtonRight = "PART_FooterButtonRight";

    #region Static properties

    /// <summary>
    /// Property for <see cref="IsShown"/>.
    /// </summary>
    public static readonly DependencyProperty IsShownProperty = DependencyProperty.Register(nameof(IsShown),
        typeof(bool), typeof(Dialog), new PropertyMetadata(false, OnIsShownChange));

    /// <summary>
    /// Property for <see cref="Footer"/>.
    /// </summary>
    public static readonly DependencyProperty FooterProperty = DependencyProperty.Register(nameof(Footer),
        typeof(object), typeof(Dialog), new PropertyMetadata(null));

    /// <summary>
    /// Property for <see cref="Title"/>.
    /// </summary>
    public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(nameof(Title),
        typeof(string), typeof(Dialog), new PropertyMetadata(String.Empty));

    /// <summary>
    /// Property for <see cref="Message"/>.
    /// </summary>
    public static readonly DependencyProperty MessageProperty = DependencyProperty.Register(nameof(Message),
        typeof(string), typeof(Dialog), new PropertyMetadata(String.Empty));

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
    /// Property for <see cref="ButtonLeftName"/>.
    /// </summary>
    public static readonly DependencyProperty ButtonLeftNameProperty = DependencyProperty.Register(
        nameof(ButtonLeftName),
        typeof(string), typeof(Dialog), new PropertyMetadata("Action"));

    /// <summary>
    /// Routed event for <see cref="ButtonLeftClick"/>.
    /// </summary>
    public static readonly RoutedEvent ButtonLeftClickEvent = EventManager.RegisterRoutedEvent(
        nameof(ButtonLeftClick), RoutingStrategy.Bubble, typeof(Dialog), typeof(Dialog));

    /// <summary>
    /// Property for <see cref="ButtonRightName"/>.
    /// </summary>
    public static readonly DependencyProperty ButtonRightNameProperty = DependencyProperty.Register(
        nameof(ButtonRightName),
        typeof(string), typeof(Dialog), new PropertyMetadata("Hide"));

    /// <summary>
    /// Property for <see cref="ButtonLeftAppearance"/>.
    /// </summary>
    public static readonly DependencyProperty ButtonLeftAppearanceProperty = DependencyProperty.Register(
        nameof(ButtonLeftAppearance),
        typeof(Common.ControlAppearance), typeof(Dialog),
        new PropertyMetadata(Common.ControlAppearance.Primary));

    /// <summary>
    /// Property for <see cref="ButtonLeftVisibility"/>.
    /// </summary>
    public static readonly DependencyProperty ButtonLeftVisibilityProperty = DependencyProperty.Register(
        nameof(ButtonLeftVisibility),
        typeof(System.Windows.Visibility), typeof(Dialog),
        new PropertyMetadata(System.Windows.Visibility.Visible));

    /// <summary>
    /// Routed event for <see cref="ButtonRightClick"/>.
    /// </summary>
    public static readonly RoutedEvent ButtonRightClickEvent = EventManager.RegisterRoutedEvent(
        nameof(ButtonRightClick), RoutingStrategy.Bubble, typeof(Dialog), typeof(Dialog));


    /// <summary>
    /// Property for <see cref="ButtonRightAppearance"/>.
    /// </summary>
    public static readonly DependencyProperty ButtonRightAppearanceProperty = DependencyProperty.Register(
        nameof(ButtonRightAppearance),
        typeof(Common.ControlAppearance), typeof(Dialog),
        new PropertyMetadata(Common.ControlAppearance.Secondary));

    /// <summary>
    /// Property for <see cref="ButtonRightVisibility"/>.
    /// </summary>
    public static readonly DependencyProperty ButtonRightVisibilityProperty = DependencyProperty.Register(
        nameof(ButtonRightVisibility),
        typeof(System.Windows.Visibility), typeof(Dialog),
        new PropertyMetadata(System.Windows.Visibility.Visible));

    /// <summary>
    /// Property for <see cref="TemplateButtonCommand"/>.
    /// </summary>
    public static readonly DependencyProperty TemplateButtonCommandProperty =
        DependencyProperty.Register(nameof(TemplateButtonCommand),
            typeof(Common.IRelayCommand), typeof(Dialog), new PropertyMetadata(null));

    #endregion Static properties

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
    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    /// <inheritdoc />
    public string Message
    {
        get => (string)GetValue(MessageProperty);
        set => SetValue(MessageProperty, value);
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
    public string ButtonLeftName
    {
        get => (string)GetValue(ButtonLeftNameProperty);
        set => SetValue(ButtonLeftNameProperty, value);
    }

    /// <summary>
    /// Gets or sets the <see cref="ControlAppearance"/> of the button on the left, if available.
    /// </summary>
    public Common.ControlAppearance ButtonLeftAppearance
    {
        get => (Common.ControlAppearance)GetValue(ButtonLeftAppearanceProperty);
        set => SetValue(ButtonLeftAppearanceProperty, value);
    }

    /// <summary>
    /// Gets or sets the visibility of the button on the left.
    /// </summary>
    public System.Windows.Visibility ButtonLeftVisibility
    {
        get => (System.Windows.Visibility)GetValue(ButtonLeftVisibilityProperty);
        set => SetValue(ButtonLeftVisibilityProperty, value);
    }

    /// <inheritdoc />
    public event RoutedEventHandler ButtonLeftClick
    {
        add => AddHandler(ButtonLeftClickEvent, value);
        remove => RemoveHandler(ButtonLeftClickEvent, value);
    }

    /// <inheritdoc />
    public string ButtonRightName
    {
        get => (string)GetValue(ButtonRightNameProperty);
        set => SetValue(ButtonRightNameProperty, value);
    }

    /// <summary>
    /// Gets or sets the <see cref="ControlAppearance"/> of the button on the right, if available.
    /// </summary>
    public Common.ControlAppearance ButtonRightAppearance
    {
        get => (Common.ControlAppearance)GetValue(ButtonRightAppearanceProperty);
        set => SetValue(ButtonRightAppearanceProperty, value);
    }

    /// <summary>
    /// Gets or sets the visibility of the button on the right.
    /// </summary>
    public System.Windows.Visibility ButtonRightVisibility
    {
        get => (System.Windows.Visibility)GetValue(ButtonRightVisibilityProperty);
        set => SetValue(ButtonRightVisibilityProperty, value);
    }

    /// <inheritdoc />
    public event RoutedEventHandler ButtonRightClick
    {
        add => AddHandler(ButtonRightClickEvent, value);
        remove => RemoveHandler(ButtonRightClickEvent, value);
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
        SetValue(TemplateButtonCommandProperty, new Common.RelayCommand(o => OnTemplateButtonClick(this, o)));
    }

    /// <inheritdoc />
    public Task<ButtonPressed> ShowAndWaitAsync()
    {
        _automaticHide = false;

        Show();

        _tcs = new TaskCompletionSource<ButtonPressed>();

        return _tcs.Task;
    }

    /// <inheritdoc />
    public Task<ButtonPressed> ShowAndWaitAsync(bool hideOnClick)
    {
        _automaticHide = hideOnClick;

        Show();

        _tcs = new TaskCompletionSource<ButtonPressed>();

        return _tcs.Task;
    }

    /// <inheritdoc />
    public Task<ButtonPressed> ShowAndWaitAsync(string title, string message)
    {
        _automaticHide = false;

        if (IsShown)
            Hide();

        Show(title, message);

        _tcs = new TaskCompletionSource<ButtonPressed>();

        return _tcs.Task;
    }

    /// <inheritdoc />
    public Task<ButtonPressed> ShowAndWaitAsync(string title, string message, bool hideOnClick)
    {
        _automaticHide = hideOnClick;

        if (IsShown)
            Hide();

        Show(title, message);

        _tcs = new TaskCompletionSource<ButtonPressed>();

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

        Title = title;
        Message = message;
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

        if (GetTemplateChild(ElementFooterButtonLeft) is System.Windows.Controls.Primitives.ButtonBase leftButton)
            _leftFooterButton = leftButton;

        if (GetTemplateChild(ElementFooterButtonLeft) is System.Windows.Controls.Primitives.ButtonBase rightButton)
            _rightFooterButton = rightButton;
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
    /// <param name="sender">Sender of the click event.</param>
    /// <param name="parameter">Additional parameters.</param>
    protected virtual void OnTemplateButtonClick(object sender, object? parameter)
    {
        if (parameter is not String parameterString)
            return;

#if DEBUG
        System.Diagnostics.Debug.WriteLine($"INFO: {typeof(Dialog)} button clicked with param: {parameterString}",
            "Wpf.Ui.Dialog");
#endif

        switch (parameterString)
        {
            case "left":
                RaiseEvent(new RoutedEventArgs(ButtonLeftClickEvent, this));

                _tcs?.TrySetResult(ButtonPressed.Left);

                break;

            case "right":
                RaiseEvent(new RoutedEventArgs(ButtonRightClickEvent, this));

                _tcs?.TrySetResult(ButtonPressed.Right);

                break;
        }

        if (_automaticHide)
            Hide();
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
        if (Footer != null)
            return;

        if (ButtonLeftVisibility == Visibility.Visible)
        {
            if (_leftFooterButton != null)
                _leftFooterButton.Focus();

            return;
        }

        if (ButtonRightVisibility != Visibility.Visible)
            return;

        if (_rightFooterButton != null)
            _rightFooterButton.Focus();
    }
}
