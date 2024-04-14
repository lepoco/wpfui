// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Wpf.Ui.Input;

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

/// <summary>
/// An <see cref="InfoBar" /> is an inline notification for essential app-
/// wide messages. The InfoBar will take up space in a layout and will not
/// cover up other content or float on top of it. It supports rich content
/// (including titles, messages, and icons) and can be configured to be
/// user-dismissable or persistent.
/// </summary>
public class InfoBar : System.Windows.Controls.ContentControl
{
    /// <summary>Identifies the <see cref="IsClosable"/> dependency property.</summary>
    public static readonly DependencyProperty IsClosableProperty = DependencyProperty.Register(
        nameof(IsClosable),
        typeof(bool),
        typeof(InfoBar),
        new PropertyMetadata(true)
    );

    /// <summary>Identifies the <see cref="IsOpen"/> dependency property.</summary>
    public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(
        nameof(IsOpen),
        typeof(bool),
        typeof(InfoBar),
        new PropertyMetadata(false)
    );

    /// <summary>Identifies the <see cref="Title"/> dependency property.</summary>
    public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
        nameof(Title),
        typeof(string),
        typeof(InfoBar),
        new PropertyMetadata(string.Empty)
    );

    /// <summary>Identifies the <see cref="Message"/> dependency property.</summary>
    public static readonly DependencyProperty MessageProperty = DependencyProperty.Register(
        nameof(Message),
        typeof(string),
        typeof(InfoBar),
        new PropertyMetadata(string.Empty)
    );

    /// <summary>Identifies the <see cref="Severity"/> dependency property.</summary>
    public static readonly DependencyProperty SeverityProperty = DependencyProperty.Register(
        nameof(Severity),
        typeof(InfoBarSeverity),
        typeof(InfoBar),
        new PropertyMetadata(InfoBarSeverity.Informational)
    );

    /// <summary>Identifies the <see cref="TemplateButtonCommand"/> dependency property.</summary>
    public static readonly DependencyProperty TemplateButtonCommandProperty = DependencyProperty.Register(
        nameof(TemplateButtonCommand),
        typeof(IRelayCommand),
        typeof(InfoBar),
        new PropertyMetadata(null)
    );

    /// <summary>
    /// Gets or sets a value indicating whether the user can close the <see cref="InfoBar" />. Defaults to <c>true</c>.
    /// </summary>
    public bool IsClosable
    {
        get => (bool)GetValue(IsClosableProperty);
        set => SetValue(IsClosableProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the <see cref="InfoBar" /> is open.
    /// </summary>
    public bool IsOpen
    {
        get => (bool)GetValue(IsOpenProperty);
        set => SetValue(IsOpenProperty, value);
    }

    /// <summary>
    /// Gets or sets the title of the <see cref="InfoBar" />.
    /// </summary>
    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    /// <summary>
    /// Gets or sets the message of the <see cref="InfoBar" />.
    /// </summary>
    public string Message
    {
        get => (string)GetValue(MessageProperty);
        set => SetValue(MessageProperty, value);
    }

    /// <summary>
    /// Gets or sets the type of the <see cref="InfoBar" /> to apply
    /// consistent status color, icon, and assistive technology settings
    /// dependent on the criticality of the notification.
    /// </summary>
    public InfoBarSeverity Severity
    {
        get => (InfoBarSeverity)GetValue(SeverityProperty);
        set => SetValue(SeverityProperty, value);
    }

    /// <summary>
    /// Gets the <see cref="RelayCommand{T}"/> triggered after clicking
    /// the close button.
    /// </summary>
    public IRelayCommand TemplateButtonCommand => (IRelayCommand)GetValue(TemplateButtonCommandProperty);

    /// <summary>
    /// Initializes a new instance of the <see cref="InfoBar"/> class.
    /// </summary>
    public InfoBar()
    {
        SetValue(
            TemplateButtonCommandProperty,
            new RelayCommand<object>(_ => SetCurrentValue(IsOpenProperty, false))
        );
    }
}
