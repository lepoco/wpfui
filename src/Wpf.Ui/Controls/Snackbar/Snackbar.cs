// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.
//
// TODO: Refactor as popup, detach from the window renderer

using System.Windows.Controls;
using Wpf.Ui.Input;

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

/// <summary>
/// Snackbar inform user of a process that an app has performed or will perform. It appears temporarily, towards the bottom of the window.
/// </summary>
public class Snackbar : ContentControl, IAppearanceControl, IIconControl
{
    /// <summary>Identifies the <see cref="IsCloseButtonEnabled"/> dependency property.</summary>
    public static readonly DependencyProperty IsCloseButtonEnabledProperty = DependencyProperty.Register(
        nameof(IsCloseButtonEnabled),
        typeof(bool),
        typeof(Snackbar),
        new PropertyMetadata(true)
    );

    /// <summary>Identifies the <see cref="SlideTransform"/> dependency property.</summary>
    public static readonly DependencyProperty SlideTransformProperty = DependencyProperty.Register(
        nameof(SlideTransform),
        typeof(TranslateTransform),
        typeof(Snackbar),
        new PropertyMetadata(new TranslateTransform())
    );

    /// <summary>Identifies the <see cref="IsShown"/> dependency property.</summary>
    public static readonly DependencyProperty IsShownProperty = DependencyProperty.Register(
        nameof(IsShown),
        typeof(bool),
        typeof(Snackbar),
        new PropertyMetadata(false)
    );

    /// <summary>Identifies the <see cref="Timeout"/> dependency property.</summary>
    public static readonly DependencyProperty TimeoutProperty = DependencyProperty.Register(
        nameof(Timeout),
        typeof(TimeSpan),
        typeof(Snackbar),
        new PropertyMetadata(TimeSpan.FromSeconds(2))
    );

    /// <summary>Identifies the <see cref="Title"/> dependency property.</summary>
    public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
        nameof(Title),
        typeof(object),
        typeof(Snackbar),
        new PropertyMetadata(null)
    );

    /// <summary>Identifies the <see cref="TitleTemplate"/> dependency property.</summary>
    public static readonly DependencyProperty TitleTemplateProperty = DependencyProperty.Register(
        nameof(TitleTemplate),
        typeof(DataTemplate),
        typeof(Snackbar),
        new PropertyMetadata(null)
    );

    /// <summary>Identifies the <see cref="Icon"/> dependency property.</summary>
    public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
        nameof(Icon),
        typeof(IconElement),
        typeof(Snackbar),
        new PropertyMetadata(null, null, IconElement.Coerce)
    );

    /// <summary>Identifies the <see cref="Appearance"/> dependency property.</summary>
    public static readonly DependencyProperty AppearanceProperty = DependencyProperty.Register(
        nameof(Appearance),
        typeof(ControlAppearance),
        typeof(Snackbar),
        new PropertyMetadata(ControlAppearance.Secondary)
    );

    /// <summary>Identifies the <see cref="TemplateButtonCommand"/> dependency property.</summary>
    public static readonly DependencyProperty TemplateButtonCommandProperty = DependencyProperty.Register(
        nameof(TemplateButtonCommand),
        typeof(IRelayCommand),
        typeof(Snackbar),
        new PropertyMetadata(null)
    );

    /// <summary>Identifies the <see cref="ContentForeground"/> dependency property.</summary>
    public static readonly DependencyProperty ContentForegroundProperty = DependencyProperty.Register(
        nameof(ContentForeground),
        typeof(Brush),
        typeof(Snackbar),
        new FrameworkPropertyMetadata(
            SystemColors.ControlTextBrush,
            FrameworkPropertyMetadataOptions.Inherits
        )
    );

    /// <summary>Identifies the <see cref="Opened"/> routed event.</summary>
    public static readonly RoutedEvent OpenedEvent = EventManager.RegisterRoutedEvent(
        nameof(Opened),
        RoutingStrategy.Bubble,
        typeof(TypedEventHandler<Snackbar, RoutedEventArgs>),
        typeof(Snackbar)
    );

    /// <summary>Identifies the <see cref="Closed"/> routed event.</summary>
    public static readonly RoutedEvent ClosedEvent = EventManager.RegisterRoutedEvent(
        nameof(Closed),
        RoutingStrategy.Bubble,
        typeof(TypedEventHandler<Snackbar, RoutedEventArgs>),
        typeof(Snackbar)
    );

    /// <summary>
    /// Gets or sets a value indicating whether the <see cref="Snackbar"/> close button should be visible.
    /// </summary>
    public bool IsCloseButtonEnabled
    {
        get => (bool)GetValue(IsCloseButtonEnabledProperty);
        set => SetValue(IsCloseButtonEnabledProperty, value);
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
    /// Gets or sets a value indicating whether the <see cref="Snackbar"/> is visible.
    /// </summary>
    public bool IsShown
    {
        get => (bool)GetValue(IsShownProperty);
        set
        {
            SetValue(IsShownProperty, value);

            if (value)
            {
                OnOpened();
            }
            else
            {
                OnClosed();
            }
        }
    }

    /// <summary>
    /// Gets or sets a time for which the <see cref="Snackbar"/> should be visible.
    /// </summary>
    public TimeSpan Timeout
    {
        get => (TimeSpan)GetValue(TimeoutProperty);
        set => SetValue(TimeoutProperty, value);
    }

    /// <summary>
    /// Gets or sets the title of the <see cref="Snackbar"/>.
    /// </summary>
    public object? Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    /// <summary>
    /// Gets or sets the title template of the <see cref="Snackbar"/>.
    /// </summary>
    public DataTemplate? TitleTemplate
    {
        get => (DataTemplate?)GetValue(TitleTemplateProperty);
        set => SetValue(TitleTemplateProperty, value);
    }

    /// <summary>
    /// Gets or sets the icon
    /// </summary>
    [Bindable(true)]
    [Category("Appearance")]
    public IconElement? Icon
    {
        get => (IconElement?)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    /// <inheritdoc />
    [Bindable(true)]
    [Category("Appearance")]
    public ControlAppearance Appearance
    {
        get => (ControlAppearance)GetValue(AppearanceProperty);
        set => SetValue(AppearanceProperty, value);
    }

    /// <summary>
    /// Gets or sets the foreground of the <see cref="ContentControl.Content"/>.
    /// </summary>
    [Bindable(true)]
    [Category("Appearance")]
    public Brush ContentForeground
    {
        get => (Brush)GetValue(ContentForegroundProperty);
        set => SetValue(ContentForegroundProperty, value);
    }

    /// <summary>
    /// Gets the command triggered after clicking the button in the template.
    /// </summary>
    public IRelayCommand TemplateButtonCommand => (IRelayCommand)GetValue(TemplateButtonCommandProperty);

    /// <summary>
    /// Occurs when the snackbar is about to open.
    /// </summary>
    public event TypedEventHandler<Snackbar, RoutedEventArgs> Opened
    {
        add => AddHandler(OpenedEvent, value);
        remove => RemoveHandler(OpenedEvent, value);
    }

    /// <summary>
    /// Occurs when the snackbar is about to close.
    /// </summary>
    public event TypedEventHandler<Snackbar, RoutedEventArgs> Closed
    {
        add => AddHandler(ClosedEvent, value);
        remove => RemoveHandler(ClosedEvent, value);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Snackbar"/> class with a specified presenter.
    /// </summary>
    /// <param name="presenter">The <see cref="SnackbarPresenter"/> to manage the snackbar's display and interactions.</param>
    public Snackbar(SnackbarPresenter presenter)
    {
        Presenter = presenter;

        SetValue(TemplateButtonCommandProperty, new RelayCommand<object>(_ => Hide()));
    }

    protected SnackbarPresenter Presenter { get; }

    /// <summary>
    /// Shows the <see cref="Snackbar"/>
    /// </summary>
    public virtual void Show()
    {
        Show(false);
    }

    /// <summary>
    /// Shows the <see cref="Snackbar"/>
    /// </summary>
    public virtual void Show(bool immediately)
    {
        if (immediately)
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            _ = Presenter.ImmediatelyDisplay(this);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }
        else
        {
            Presenter.AddToQue(this);
        }
    }

    /// <summary>
    /// Shows the <see cref="Snackbar"/>.
    /// </summary>
    public virtual Task ShowAsync()
    {
        return ShowAsync(false);
    }

    /// <summary>
    /// Shows the <see cref="Snackbar"/>.
    /// </summary>
    public virtual async Task ShowAsync(bool immediately)
    {
        if (immediately)
        {
            await Presenter.ImmediatelyDisplay(this);
        }
        else
        {
            Presenter.AddToQue(this);
        }
    }

    /// <summary>
    /// Hides the <see cref="Snackbar"/>
    /// </summary>
    protected virtual void Hide()
    {
        _ = Presenter.HideCurrent();
    }

    /// <summary>
    /// This virtual method is called when <see cref="Snackbar"/> is opening and it raises the <see cref="Opened"/> <see langword="event"/>.
    /// </summary>
    protected virtual void OnOpened()
    {
        RaiseEvent(new RoutedEventArgs(OpenedEvent, this));
    }

    /// <summary>
    /// This virtual method is called when <see cref="Snackbar"/> is closing and it raises the <see cref="Closed"/> <see langword="event"/>.
    /// </summary>
    protected virtual void OnClosed()
    {
        RaiseEvent(new RoutedEventArgs(ClosedEvent, this));
    }
}
