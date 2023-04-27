// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using Wpf.Ui.Common;
using Wpf.Ui.Controls.IconElements;
using Wpf.Ui.Converters;

namespace Wpf.Ui.Controls.SnackbarControl;

public class NewSnackbar : System.Windows.Controls.ContentControl
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

    #endregion

    #region Properties

    /// <summary>
    /// TODO
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
    /// TODO
    /// </summary>
    public bool IsShown
    {
        get => (bool)GetValue(IsShownProperty);
        set => SetValue(IsShownProperty, value);
    }

    /// <summary>
    /// TODO
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

    /// <summary>
    /// TODO
    /// </summary>
    [Bindable(true), Category("Appearance")]
    public ControlAppearance Appearance
    {
        get => (ControlAppearance)GetValue(AppearanceProperty);
        set => SetValue(AppearanceProperty, value);
    }

    /// <summary>
    /// Command triggered after clicking the button in the template.
    /// </summary>
    public IRelayCommand TemplateButtonCommand => (IRelayCommand)GetValue(TemplateButtonCommandProperty);

    #endregion

    public NewSnackbar(SnackbarPresenter presenter)
    {
        _presenter = presenter;

        SetValue(TemplateButtonCommandProperty, new RelayCommand<object>(_ => Hide()));
    }

    private readonly SnackbarPresenter _presenter;

    public void Show(bool immediately = false)
    {
        if (immediately)
        {
            _presenter.ImmediatelyDisplay(this);
        }
        else
        {
            _presenter.AddToQue(this);
        }
    }

    protected virtual void Hide()
    {
        _ = _presenter.HideCurrent();
    }
}
