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

public enum MessageBoxButton
{
    None,
    Primary,
    Secondary,
    Close
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
    /// Property for <see cref="PrimaryButtonAppearance"/>.
    /// </summary>
    public static readonly DependencyProperty PrimaryButtonAppearanceProperty = DependencyProperty.Register(
        nameof(PrimaryButtonAppearance),
        typeof(ControlAppearance), typeof(MessageBox),
        new PropertyMetadata(ControlAppearance.Primary));

    /// <summary>
    /// Property for <see cref="IsPrimaryButtonEnabled"/>.
    /// </summary>
    public static readonly DependencyProperty IsPrimaryButtonEnabledProperty = DependencyProperty.Register(
        nameof(IsPrimaryButtonEnabled),
        typeof(bool), typeof(MessageBox),
        new PropertyMetadata(true));

    /// <summary>
    /// Property for <see cref="SecondaryButtonText"/>.
    /// </summary>
    public static readonly DependencyProperty SecondaryButtonTextProperty = DependencyProperty.Register(
        nameof(SecondaryButtonText),
        typeof(string), typeof(MessageBox), new PropertyMetadata(string.Empty));

    /// <summary>
    /// Property for <see cref="SecondaryButtonAppearance"/>.
    /// </summary>
    public static readonly DependencyProperty SecondaryButtonAppearanceProperty = DependencyProperty.Register(
        nameof(SecondaryButtonAppearance),
        typeof(ControlAppearance), typeof(MessageBox),
        new PropertyMetadata(ControlAppearance.Secondary));

    /// <summary>
    /// Property for <see cref="IsSecondaryButtonEnabled"/>.
    /// </summary>
    public static readonly DependencyProperty IsSecondaryButtonEnabledProperty = DependencyProperty.Register(
        nameof(IsSecondaryButtonEnabled),
        typeof(bool), typeof(MessageBox),
        new PropertyMetadata(true));

    /// <summary>
    /// Property for <see cref="CloseButtonText"/>.
    /// </summary>
    public static readonly DependencyProperty CloseButtonTextProperty = DependencyProperty.Register(
        nameof(CloseButtonText),
        typeof(string), typeof(MessageBox), new PropertyMetadata("Close"));

    /// <summary>
    /// Property for <see cref="CloseButtonAppearance"/>.
    /// </summary>
    public static readonly DependencyProperty CloseButtonAppearanceProperty = DependencyProperty.Register(
        nameof(CloseButtonAppearance),
        typeof(ControlAppearance), typeof(MessageBox),
        new PropertyMetadata(ControlAppearance.Secondary));

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
    /// Text of primary button
    /// </summary>
    public string PrimaryButtonText
    {
        get => (string)GetValue(PrimaryButtonTextProperty);
        set => SetValue(PrimaryButtonTextProperty, value);
    }

    /// <summary>
    /// Gets or sets the <see cref="ControlAppearance"/>
    /// </summary>
    public ControlAppearance PrimaryButtonAppearance
    {
        get => (ControlAppearance)GetValue(PrimaryButtonAppearanceProperty);
        set => SetValue(PrimaryButtonAppearanceProperty, value);
    }

    public bool IsPrimaryButtonEnabled
    {
        get => (bool)GetValue(IsPrimaryButtonEnabledProperty);
        set => SetValue(IsPrimaryButtonEnabledProperty, value);
    }

    /// <summary>
    /// Text of secondary button
    /// </summary>
    public string SecondaryButtonText
    {
        get => (string)GetValue(SecondaryButtonTextProperty);
        set => SetValue(SecondaryButtonTextProperty, value);
    }

    /// <summary>
    /// Gets or sets the <see cref="ControlAppearance"/>
    /// </summary>
    public ControlAppearance SecondaryButtonAppearance
    {
        get => (ControlAppearance)GetValue(SecondaryButtonAppearanceProperty);
        set => SetValue(SecondaryButtonAppearanceProperty, value);
    }

    public bool IsSecondaryButtonEnabled
    {
        get => (bool)GetValue(IsSecondaryButtonEnabledProperty);
        set => SetValue(IsSecondaryButtonEnabledProperty, value);
    }

    /// <summary>
    /// Text of close button
    /// </summary>
    public string CloseButtonText
    {
        get => (string)GetValue(CloseButtonTextProperty);
        set => SetValue(CloseButtonTextProperty, value);
    }

    /// <summary>
    /// Gets or sets the <see cref="ControlAppearance"/>
    /// </summary>
    public ControlAppearance CloseButtonAppearance
    {
        get => (ControlAppearance)GetValue(CloseButtonAppearanceProperty);
        set => SetValue(CloseButtonAppearanceProperty, value);
    }

    /// <summary>
    /// Command triggered after clicking the button on the Footer.
    /// </summary>
    public IRelayCommand TemplateButtonCommand => (IRelayCommand)GetValue(TemplateButtonCommandProperty);

    #endregion

    /// <summary>
    /// Creates new instance and sets default <see cref="FrameworkElement.Loaded"/> event.
    /// </summary>
    public MessageBox()
    {
        SetWindowStartupLocation();

        WindowStartupLocation = WindowStartupLocation.CenterOwner;
        
        SetValue(TemplateButtonCommandProperty, new RelayCommand<MessageBoxButton>(OnTemplateButtonClick));

        PreviewMouseDoubleClick += static (_, args) => args.Handled = true;

        Loaded += OnLoaded;
        Unloaded += OnUnloaded;
    }

    private TaskCompletionSource<MessageBoxButton>? _tcs; 

    [Obsolete($"Use {nameof(ShowAsync)} instead")]
    public new void Show()
    {
        RemoveTitleBarAndApplyMica();
        base.Show();
    }

    [Obsolete($"Use {nameof(ShowDialogAsync)} instead")]
    public new bool? ShowDialog()
    {
        RemoveTitleBarAndApplyMica();
        return base.ShowDialog();
    }

    /// <summary>
    /// Sets <see cref="System.Windows.Window.Title"/> and content of <see cref="System.Windows.Window"/>, then calls <see cref="ShowAsync"/>.
    /// </summary>
    /// <returns><see cref="MessageBoxButton"/></returns>
    /// <exception cref="TaskCanceledException"></exception>
    public async Task<MessageBoxButton> ShowAsync(CancellationToken cancellationToken = default)
    {
        var tokenRegistration = InitializeTCs(cancellationToken);

        try
        {
#pragma warning disable CS0618
            Show();
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

    /// <summary>
    /// Sets <see cref="System.Windows.Window.Title"/> and content of <see cref="System.Windows.Window"/>, then calls <see cref="ShowDialog"/>.
    /// </summary>
    /// <returns><see cref="MessageBoxButton"/></returns>
    /// <exception cref="TaskCanceledException"></exception>
    public async Task<MessageBoxButton> ShowDialogAsync(CancellationToken cancellationToken = default)
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

        Width = frameworkElement.DesiredSize.Width;
        Height = frameworkElement.DesiredSize.Height;
    }

    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
        Loaded -= OnLoaded;
        Unloaded -= OnUnloaded;
    }

    private CancellationTokenRegistration InitializeTCs(CancellationToken cancellationToken)
    {
        _tcs = new TaskCompletionSource<MessageBoxButton>();
        return cancellationToken.Register(o => _tcs.TrySetCanceled((CancellationToken)o!), cancellationToken);
    }

    private void RemoveTitleBarAndApplyMica()
    {
        UnsafeNativeMethods.RemoveWindowTitlebarContents(this);
        WindowBackdrop.ApplyBackdrop(this, WindowBackdropType.Mica);
    }

    private void SetWindowStartupLocation()
    {
        if (Application.Current?.MainWindow != null)
            Owner = Application.Current.MainWindow;
        else
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
    }

    private void OnTemplateButtonClick(MessageBoxButton button)
    {
#if DEBUG
        System.Diagnostics.Debug.WriteLine($"INFO | {typeof(MessageBox)} button clicked with param: {button}", "Wpf.Ui.MessageBox");
#endif
        _tcs?.TrySetResult(button);
        Close();
    }

    private void CancelTcs(object? obj) => _tcs?.TrySetCanceled((CancellationToken)obj!);
}
