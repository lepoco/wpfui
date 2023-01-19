// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Wpf.Ui.Common;
using Wpf.Ui.Controls.Window;
using Wpf.Ui.Interop;

namespace Wpf.Ui.Controls;

/// <summary>
/// Defines constants that specify the default button on a <see cref="MessageBox"/>.
/// </summary>
public enum MessageBoxButton
{
    /// <summary>
    /// The primary button
    /// </summary>
    Primary,
    /// <summary>
    /// The secondary button
    /// </summary>
    Secondary,
    /// <summary>
    /// The close button
    /// </summary>
    Close
}

/// <summary>
/// Specifies identifiers to indicate the return value of a <see cref="MessageBox"/>.
/// </summary>
public enum MessageBoxResult
{
    /// <summary>
    /// No button was tapped.
    /// </summary>
    None,
    /// <summary>
    /// The primary button was tapped by the user.
    /// </summary>
    Primary,
    /// <summary>
    /// The secondary button was tapped by the user.
    /// </summary>
    Secondary
}

/// <summary>
/// Customized window for notifications.
/// </summary>
[ToolboxItem(true)]
[ToolboxBitmap(typeof(MessageBox), "MessageBox.bmp")]
public class MessageBox : System.Windows.Window
{
    #region Static properties

    /// <summary>
    /// Property for <see cref="ShowTitle"/>.
    /// </summary>
    public static readonly DependencyProperty ShowTitleProperty = DependencyProperty.Register(nameof(ShowTitle),
        typeof(bool), typeof(MessageBox), new PropertyMetadata(true));

    /// <summary>
    /// Property for <see cref="PrimaryButtonText"/>.
    /// </summary>
    public static readonly DependencyProperty PrimaryButtonTextProperty = DependencyProperty.Register(
        nameof(PrimaryButtonText),
        typeof(string), typeof(MessageBox), new PropertyMetadata(string.Empty));

    /// <summary>
    /// Property for <see cref="SecondaryButtonText"/>.
    /// </summary>
    public static readonly DependencyProperty SecondaryButtonTextProperty = DependencyProperty.Register(
        nameof(SecondaryButtonText),
        typeof(string), typeof(MessageBox), new PropertyMetadata(string.Empty));

    /// <summary>
    /// Property for <see cref="CloseButtonText"/>.
    /// </summary>
    public static readonly DependencyProperty CloseButtonTextProperty = DependencyProperty.Register(
        nameof(CloseButtonText),
        typeof(string), typeof(MessageBox), new PropertyMetadata("Close"));

    /// <summary>
    /// Property for <see cref="PrimaryButtonIcon"/>.
    /// </summary>
    public static readonly DependencyProperty PrimaryButtonIconProperty =
        DependencyProperty.Register(nameof(PrimaryButtonIcon),
            typeof(SymbolRegular), typeof(MessageBox), new PropertyMetadata(SymbolRegular.Empty));

    /// <summary>
    /// Property for <see cref="SecondaryButtonIcon"/>.
    /// </summary>
    public static readonly DependencyProperty SecondaryButtonIconProperty =
        DependencyProperty.Register(nameof(SecondaryButtonIcon),
            typeof(SymbolRegular), typeof(MessageBox), new PropertyMetadata(SymbolRegular.Empty));

    /// <summary>
    /// Property for <see cref="CloseButtonIcon"/>.
    /// </summary>
    public static readonly DependencyProperty CloseButtonIconProperty =
        DependencyProperty.Register(nameof(CloseButtonIcon),
            typeof(SymbolRegular), typeof(MessageBox), new PropertyMetadata(SymbolRegular.Empty));

    /// <summary>
    /// Property for <see cref="PrimaryButtonAppearance"/>.
    /// </summary>
    public static readonly DependencyProperty PrimaryButtonAppearanceProperty = DependencyProperty.Register(
        nameof(PrimaryButtonAppearance),
        typeof(ControlAppearance), typeof(MessageBox),
        new PropertyMetadata(ControlAppearance.Primary));

    /// <summary>
    /// Property for <see cref="SecondaryButtonAppearance"/>.
    /// </summary>
    public static readonly DependencyProperty SecondaryButtonAppearanceProperty = DependencyProperty.Register(
        nameof(SecondaryButtonAppearance),
        typeof(ControlAppearance), typeof(MessageBox),
        new PropertyMetadata(ControlAppearance.Secondary));

    /// <summary>
    /// Property for <see cref="CloseButtonAppearance"/>.
    /// </summary>
    public static readonly DependencyProperty CloseButtonAppearanceProperty = DependencyProperty.Register(
        nameof(CloseButtonAppearance),
        typeof(ControlAppearance), typeof(MessageBox),
        new PropertyMetadata(ControlAppearance.Secondary));

    /// <summary>
    /// Property for <see cref="IsPrimaryButtonEnabled"/>.
    /// </summary>
    public static readonly DependencyProperty IsPrimaryButtonEnabledProperty = DependencyProperty.Register(
        nameof(IsPrimaryButtonEnabled),
        typeof(bool), typeof(MessageBox),
        new PropertyMetadata(true));

    /// <summary>
    /// Property for <see cref="IsSecondaryButtonEnabled"/>.
    /// </summary>
    public static readonly DependencyProperty IsSecondaryButtonEnabledProperty = DependencyProperty.Register(
        nameof(IsSecondaryButtonEnabled),
        typeof(bool), typeof(MessageBox),
        new PropertyMetadata(true));

    /// <summary>
    /// Property for <see cref="TemplateButtonCommand"/>.
    /// </summary>
    public static readonly DependencyProperty TemplateButtonCommandProperty =
        DependencyProperty.Register(nameof(TemplateButtonCommand),
            typeof(IRelayCommand), typeof(MessageBox), new PropertyMetadata(null));

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets a value that determines whether to show the <see cref="System.Windows.Window.Title"/> in <see cref="Wpf.Ui.Controls.TitleBar"/>.
    /// </summary>
    public bool ShowTitle
    {
        get => (bool)GetValue(ShowTitleProperty);
        set => SetValue(ShowTitleProperty, value);
    }

    /// <summary>
    /// Gets or sets the text to display on the primary button.
    /// </summary>
    public string PrimaryButtonText
    {
        get => (string)GetValue(PrimaryButtonTextProperty);
        set => SetValue(PrimaryButtonTextProperty, value);
    }

    /// <summary>
    /// Gets or sets the text to be displayed on the secondary button.
    /// </summary>
    public string SecondaryButtonText
    {
        get => (string)GetValue(SecondaryButtonTextProperty);
        set => SetValue(SecondaryButtonTextProperty, value);
    }

    /// <summary>
    /// Gets or sets the text to display on the close button.
    /// </summary>
    public string CloseButtonText
    {
        get => (string)GetValue(CloseButtonTextProperty);
        set => SetValue(CloseButtonTextProperty, value);
    }

    /// <summary>
    /// Gets or sets the <see cref="SymbolRegular"/> on the primary button
    /// </summary>
    public SymbolRegular PrimaryButtonIcon
    {
        get => (SymbolRegular)GetValue(PrimaryButtonIconProperty);
        set => SetValue(PrimaryButtonIconProperty, value);
    }

    /// <summary>
    /// Gets or sets the <see cref="SymbolRegular"/> on the secondary button
    /// </summary>
    public SymbolRegular SecondaryButtonIcon
    {
        get => (SymbolRegular)GetValue(SecondaryButtonIconProperty);
        set => SetValue(SecondaryButtonIconProperty, value);
    }

    /// <summary>
    /// Gets or sets the <see cref="SymbolRegular"/> on the close button
    /// </summary>
    public SymbolRegular CloseButtonIcon
    {
        get => (SymbolRegular)GetValue(CloseButtonIconProperty);
        set => SetValue(CloseButtonIconProperty, value);
    }

    /// <summary>
    /// Gets or sets the <see cref="ControlAppearance"/> on the primary button
    /// </summary>
    public ControlAppearance PrimaryButtonAppearance
    {
        get => (ControlAppearance)GetValue(PrimaryButtonAppearanceProperty);
        set => SetValue(PrimaryButtonAppearanceProperty, value);
    }

    /// <summary>
    /// Gets or sets the <see cref="ControlAppearance"/> on the secondary button
    /// </summary>
    public ControlAppearance SecondaryButtonAppearance
    {
        get => (ControlAppearance)GetValue(SecondaryButtonAppearanceProperty);
        set => SetValue(SecondaryButtonAppearanceProperty, value);
    }

    /// <summary>
    /// Gets or sets the <see cref="ControlAppearance"/> on the close button
    /// </summary>
    public ControlAppearance CloseButtonAppearance
    {
        get => (ControlAppearance)GetValue(CloseButtonAppearanceProperty);
        set => SetValue(CloseButtonAppearanceProperty, value);
    }

    /// <summary>
    /// Gets or sets whether the <see cref="MessageBox"/> primary button is enabled.
    /// </summary>
    public bool IsSecondaryButtonEnabled
    {
        get => (bool)GetValue(IsSecondaryButtonEnabledProperty);
        set => SetValue(IsSecondaryButtonEnabledProperty, value);
    }

    /// <summary>
    /// Gets or sets whether the <see cref="MessageBox"/> secondary button is enabled.
    /// </summary>
    public bool IsPrimaryButtonEnabled
    {
        get => (bool)GetValue(IsPrimaryButtonEnabledProperty);
        set => SetValue(IsPrimaryButtonEnabledProperty, value);
    }

    /// <summary>
    /// Command triggered after clicking the button on the Footer.
    /// </summary>
    public IRelayCommand TemplateButtonCommand => (IRelayCommand)GetValue(TemplateButtonCommandProperty);

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="MessageBox"/> class.
    /// </summary>
    public MessageBox()
    {
        Topmost = true;
        SetValue(TemplateButtonCommandProperty, new RelayCommand<MessageBoxButton>(OnTemplateButtonClick));

        PreviewMouseDoubleClick += static (_, args) => args.Handled = true;

        Loaded += OnLoaded;
        Unloaded += OnUnloaded;
    }

    private TaskCompletionSource<MessageBoxResult>? _tcs; 

    [Obsolete($"Use {nameof(ShowDialogAsync)} instead")]
    public new void Show()
    {
        throw new InvalidOperationException($"Use {nameof(ShowDialogAsync)} instead");
    }

    [Obsolete($"Use {nameof(ShowDialogAsync)} instead")]
    public new bool? ShowDialog()
    {
        RemoveTitleBarAndApplyMica();
        return base.ShowDialog();
    }

    /// <summary>
    /// Displays a message box
    /// </summary>
    /// <returns><see cref="MessageBoxResult"/></returns>
    /// <exception cref="TaskCanceledException"></exception>
    public async Task<MessageBoxResult> ShowDialogAsync(CancellationToken cancellationToken = default)
    {
        var tokenRegistration = InitializeTCs(cancellationToken);

        try
        {
#pragma warning disable CS0618
            ShowDialog();
#pragma warning restore CS0618

            return await _tcs!.Task;
        }
        finally
        {
#if NET6_0_OR_GREATER
            await tokenRegistration.DisposeAsync();
#else
            tokenRegistration.Dispose();
#endif
        }
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        if (VisualChildrenCount <= 0)
            return;

        if (GetVisualChild(0) is not FrameworkElement frameworkElement)
            return;

        //left and right margin
        const double margin = 12.0 * 2;

        var currentWidth = frameworkElement.DesiredSize.Width + margin;
        if (currentWidth > MaxWidth)
            MaxWidth = currentWidth;

        Width = currentWidth;
        Height = frameworkElement.DesiredSize.Height;

        CenterWindowOnScreen();
    }

    private void CenterWindowOnScreen()
    {
        double screenWidth = SystemParameters.PrimaryScreenWidth;
        double screenHeight = SystemParameters.PrimaryScreenHeight;

        Left = (screenWidth / 2) - (Width / 2);
        Top = (screenHeight / 2) - (Height / 2);
    }

    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
        Loaded -= OnLoaded;
        Unloaded -= OnUnloaded;
    }

    private CancellationTokenRegistration InitializeTCs(CancellationToken cancellationToken)
    {
        _tcs = new TaskCompletionSource<MessageBoxResult>();
        return cancellationToken.Register(o => _tcs.TrySetCanceled((CancellationToken)o!), cancellationToken);
    }

    private void RemoveTitleBarAndApplyMica()
    {
        UnsafeNativeMethods.RemoveWindowTitlebarContents(this);
        WindowBackdrop.ApplyBackdrop(this, WindowBackdropType.Mica);
    }

    private void OnTemplateButtonClick(MessageBoxButton button)
    {
#if DEBUG
        System.Diagnostics.Debug.WriteLine($"INFO | {typeof(MessageBox)} button clicked with param: {button}", "Wpf.Ui.MessageBox");
#endif

        MessageBoxResult result = button switch
        {
            MessageBoxButton.Primary => MessageBoxResult.Primary,
            MessageBoxButton.Secondary => MessageBoxResult.Secondary,
            _ => MessageBoxResult.None
        };

        _tcs?.TrySetResult(result);
        Close();
    }
}
