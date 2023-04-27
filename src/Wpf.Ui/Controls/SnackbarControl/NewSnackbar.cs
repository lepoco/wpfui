// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Wpf.Ui.Common;
using Wpf.Ui.Controls.IconElements;
using Wpf.Ui.Converters;

namespace Wpf.Ui.Controls.SnackbarControl;

/// <summary>
/// Snackbar inform user of a process that an app has performed or will perform. It appears temporarily, towards the bottom of the window.
/// </summary>
public class NewSnackbar : ContentControl, IAppearanceControl
{
    #region Static properties

    /// <summary>
    /// Property for <see cref="IsCloseButtonEnabled"/>.
    /// </summary>
    public static readonly DependencyProperty IsCloseButtonEnabledProperty = DependencyProperty.Register(
        nameof(IsCloseButtonEnabled),
        typeof(bool), typeof(NewSnackbar), new PropertyMetadata(true));

    /// <summary>
    /// Property for <see cref="SlideTransform"/>.
    /// </summary>
    public static readonly DependencyProperty SlideTransformProperty = DependencyProperty.Register(
        nameof(SlideTransform),
        typeof(TranslateTransform), typeof(NewSnackbar), new PropertyMetadata(new TranslateTransform()));

    /// <summary>
    /// Property for <see cref="IsShown"/>.
    /// </summary>
    public static readonly DependencyProperty IsShownProperty = DependencyProperty.Register(nameof(IsShown),
        typeof(bool), typeof(NewSnackbar), new PropertyMetadata(false));

    /// <summary>
    /// Property for <see cref="Timeout"/>.
    /// </summary>
    public static readonly DependencyProperty TimeoutProperty = DependencyProperty.Register(nameof(Timeout),
        typeof(TimeSpan), typeof(NewSnackbar), new PropertyMetadata(TimeSpan.FromSeconds(2)));

    /// <summary>
    /// Property for <see cref="Title"/>.
    /// </summary>
    public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(nameof(Title),
        typeof(object), typeof(NewSnackbar), new PropertyMetadata(null));

    /// <summary>
    /// Property for <see cref="TitleTemplate"/>.
    /// </summary>
    public static readonly DependencyProperty TitleTemplateProperty = DependencyProperty.Register(nameof(TitleTemplate),
        typeof(DataTemplate), typeof(NewSnackbar), new PropertyMetadata(null));

    /// <summary>
    /// Property for <see cref="Icon"/>.
    /// </summary>
    public static readonly DependencyProperty IconProperty = DependencyProperty.Register(nameof(Icon),
        typeof(IconElement), typeof(NewSnackbar),
        new PropertyMetadata(null, null, IconSourceElementConverter.ConvertToIconElement));

    /// <summary>
    /// Property for <see cref="Appearance"/>.
    /// </summary>
    public static readonly DependencyProperty AppearanceProperty = DependencyProperty.Register(nameof(Appearance),
        typeof(ControlAppearance), typeof(NewSnackbar),
        new PropertyMetadata(ControlAppearance.Secondary));

    /// <summary>
    /// Property for <see cref="TemplateButtonCommand"/>.
    /// </summary>
    public static readonly DependencyProperty TemplateButtonCommandProperty =
        DependencyProperty.Register(nameof(TemplateButtonCommand),
            typeof(IRelayCommand), typeof(NewSnackbar), new PropertyMetadata(null));

    /// <summary>
    /// Property for <see cref="ContentForeground"/>.
    /// </summary>
    public static readonly DependencyProperty ContentForegroundProperty = DependencyProperty.Register(
        nameof(ContentForeground),
        typeof(Brush), typeof(NewSnackbar), new FrameworkPropertyMetadata(SystemColors.ControlTextBrush,
            FrameworkPropertyMetadataOptions.Inherits));

    /// <summary>
    /// Property for <see cref="Opened"/>.
    /// </summary>
    public static readonly RoutedEvent OpenedEvent = EventManager.RegisterRoutedEvent(nameof(Opened),
        RoutingStrategy.Bubble, typeof(TypedEventHandler<NewSnackbar, RoutedEventArgs>), typeof(NewSnackbar));

    /// <summary>
    /// Property for <see cref="Closed"/>.
    /// </summary>
    public static readonly RoutedEvent ClosedEvent = EventManager.RegisterRoutedEvent(nameof(Closed),
        RoutingStrategy.Bubble, typeof(TypedEventHandler<NewSnackbar, RoutedEventArgs>), typeof(NewSnackbar));

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets a value indicating whether the <see cref="NewSnackbar"/> close button should be visible.
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
    /// Gets the information whether the <see cref="NewSnackbar"/> is visible.
    /// </summary>
    public bool IsShown
    {
        get => (bool)GetValue(IsShownProperty);
        set
        {
            SetValue(IsShownProperty, value);

            if (value)
                OnOpened();
            else
                OnClosed();
        }
    }

    /// <summary>
    /// Gets or sets a time for which the <see cref="NewSnackbar"/> should be visible.
    /// </summary>
    public TimeSpan Timeout
    {
        get => (TimeSpan)GetValue(TimeoutProperty);
        set => SetValue(TimeoutProperty, value);
    }

    /// <summary>
    /// Gets or sets the title of the <see cref="NewSnackbar"/>.
    /// </summary>
    public object Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    /// <summary>
    /// Gets or sets the title template of the <see cref="NewSnackbar"/>.
    /// </summary>
    public DataTemplate TitleTemplate
    {
        get => (DataTemplate) GetValue(TitleTemplateProperty);
        set => SetValue(TitleTemplateProperty, value);
    }

    /// <summary>
    /// TODO
    /// </summary>
    [Bindable(true), Category("Appearance")]
    public IconElement Icon
    {
        get => (IconElement)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    /// <inheritdoc />
    [Bindable(true), Category("Appearance")]
    public ControlAppearance Appearance
    {
        get => (ControlAppearance)GetValue(AppearanceProperty);
        set => SetValue(AppearanceProperty, value);
    }

    /// <summary>
    /// Foreground of the <see cref="ContentControl.Content"/>.
    /// </summary>
    [Bindable(true), Category("Appearance")]
    public Brush ContentForeground
    {
        get => (Brush)GetValue(ContentForegroundProperty);
        set => SetValue(ContentForegroundProperty, value);
    }

    /// <summary>
    /// Command triggered after clicking the button in the template.
    /// </summary>
    public IRelayCommand TemplateButtonCommand => (IRelayCommand)GetValue(TemplateButtonCommandProperty);

    /// <summary>
    /// Occurs when the snackbar is about to open.
    /// </summary>
    public event TypedEventHandler<NewSnackbar, RoutedEventArgs> Opened
    {
        add => AddHandler(OpenedEvent, value);
        remove => RemoveHandler(OpenedEvent, value);
    }

    /// <summary>
    /// Occurs when the snackbar is about to close.
    /// </summary>
    public event TypedEventHandler<NewSnackbar, RoutedEventArgs> Closed
    {
        add => AddHandler(ClosedEvent, value);
        remove => RemoveHandler(ClosedEvent, value);
    }

    #endregion

    public NewSnackbar(SnackbarPresenter presenter)
    {
        Presenter = presenter;

        SetValue(TemplateButtonCommandProperty, new RelayCommand<object>(_ => Hide()));
    }

    protected readonly SnackbarPresenter Presenter;

    public virtual void Show(bool immediately = false)
    {
        if (immediately)
        {
            Presenter.ImmediatelyDisplay(this);
        }
        else
        {
            Presenter.AddToQue(this);
        }
    }

    protected virtual void Hide()
    {
        _ = Presenter.HideCurrent();
    }

    protected virtual void OnOpened()
    {
        RaiseEvent(new RoutedEventArgs(OpenedEvent, this));
    }

    protected virtual void OnClosed()
    {
        RaiseEvent(new RoutedEventArgs(ClosedEvent, this));
    }
}
