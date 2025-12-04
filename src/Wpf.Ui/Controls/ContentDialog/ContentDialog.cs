// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Wpf.Ui.Input;

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

/// <summary>
    /// Dialogue displayed inside the application covering its internals, displaying some content.
    /// </summary>
    /// <example>
    /// <code lang="xml">
    /// &lt;ContentPresenter x:Name="RootContentDialogPresenter" Grid.Row="0" /&gt;
    /// </code>
    /// <code lang="csharp">
    /// var contentDialog = new ContentDialog(RootContentDialogPresenter);
    ///
    /// contentDialog.SetCurrentValue(ContentDialog.TitleProperty, "Hello World");
    /// contentDialog.SetCurrentValue(ContentControl.ContentProperty, "This is a message");
    /// contentDialog.SetCurrentValue(ContentDialog.CloseButtonTextProperty, "Close this dialog");
    ///
    /// await contentDialog.ShowAsync(cancellationToken);
    /// </code>
    /// <code lang="csharp">
    /// var contentDialogService = new ContentDialogService();
    /// contentDialogService.SetContentPresenter(RootContentDialogPresenter);
    ///
    /// await _contentDialogService.ShowSimpleDialogAsync(
    ///     new SimpleContentDialogCreateOptions()
    ///         {
    ///             Title = "The cake?",
    ///             Content = "IS A LIE!",
    ///             PrimaryButtonText = "Save",
    ///             SecondaryButtonText = "Don't Save",
    ///             CloseButtonText = "Cancel"
    ///         }
    ///     );
    /// </code>
    /// </example>
    public class ContentDialog : ContentControl
{
    /// <summary>Identifies the <see cref="Title"/> dependency property.</summary>
    public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
        nameof(Title),
        typeof(object),
        typeof(ContentDialog),
        new PropertyMetadata(null)
    );

    /// <summary>Identifies the <see cref="TitleTemplate"/> dependency property.</summary>
    public static readonly DependencyProperty TitleTemplateProperty = DependencyProperty.Register(
        nameof(TitleTemplate),
        typeof(DataTemplate),
        typeof(ContentDialog),
        new PropertyMetadata(null)
    );

    /// <summary>Identifies the <see cref="DialogWidth"/> dependency property.</summary>
    public static readonly DependencyProperty DialogWidthProperty = DependencyProperty.Register(
        nameof(DialogWidth),
        typeof(double),
        typeof(ContentDialog),
        new PropertyMetadata(double.PositiveInfinity)
    );

    /// <summary>Identifies the <see cref="DialogHeight"/> dependency property.</summary>
    public static readonly DependencyProperty DialogHeightProperty = DependencyProperty.Register(
        nameof(DialogHeight),
        typeof(double),
        typeof(ContentDialog),
        new PropertyMetadata(double.PositiveInfinity)
    );

    /// <summary>Identifies the <see cref="DialogMaxWidth"/> dependency property.</summary>
    public static readonly DependencyProperty DialogMaxWidthProperty = DependencyProperty.Register(
        nameof(DialogMaxWidth),
        typeof(double),
        typeof(ContentDialog),
        new PropertyMetadata(double.PositiveInfinity)
    );

    /// <summary>Identifies the <see cref="DialogMaxHeight"/> dependency property.</summary>
    public static readonly DependencyProperty DialogMaxHeightProperty = DependencyProperty.Register(
        nameof(DialogMaxHeight),
        typeof(double),
        typeof(ContentDialog),
        new PropertyMetadata(double.PositiveInfinity)
    );

    /// <summary>Identifies the <see cref="DialogMargin"/> dependency property.</summary>
    public static readonly DependencyProperty DialogMarginProperty = DependencyProperty.Register(
        nameof(DialogMargin),
        typeof(Thickness),
        typeof(ContentDialog)
    );

    /// <summary>Identifies the <see cref="PrimaryButtonText"/> dependency property.</summary>
    public static readonly DependencyProperty PrimaryButtonTextProperty = DependencyProperty.Register(
        nameof(PrimaryButtonText),
        typeof(string),
        typeof(ContentDialog),
        new PropertyMetadata(string.Empty)
    );

    /// <summary>Identifies the <see cref="SecondaryButtonText"/> dependency property.</summary>
    public static readonly DependencyProperty SecondaryButtonTextProperty = DependencyProperty.Register(
        nameof(SecondaryButtonText),
        typeof(string),
        typeof(ContentDialog),
        new PropertyMetadata(string.Empty)
    );

    /// <summary>Identifies the <see cref="CloseButtonText"/> dependency property.</summary>
    public static readonly DependencyProperty CloseButtonTextProperty = DependencyProperty.Register(
        nameof(CloseButtonText),
        typeof(string),
        typeof(ContentDialog),
        new PropertyMetadata("Close")
    );

    /// <summary>Identifies the <see cref="PrimaryButtonIcon"/> dependency property.</summary>
    public static readonly DependencyProperty PrimaryButtonIconProperty = DependencyProperty.Register(
        nameof(PrimaryButtonIcon),
        typeof(IconElement),
        typeof(ContentDialog),
        new PropertyMetadata(null)
    );

    /// <summary>Identifies the <see cref="SecondaryButtonIcon"/> dependency property.</summary>
    public static readonly DependencyProperty SecondaryButtonIconProperty = DependencyProperty.Register(
        nameof(SecondaryButtonIcon),
        typeof(IconElement),
        typeof(ContentDialog),
        new PropertyMetadata(null)
    );

    /// <summary>Identifies the <see cref="CloseButtonIcon"/> dependency property.</summary>
    public static readonly DependencyProperty CloseButtonIconProperty = DependencyProperty.Register(
        nameof(CloseButtonIcon),
        typeof(IconElement),
        typeof(ContentDialog),
        new PropertyMetadata(null)
    );

    /// <summary>Identifies the <see cref="IsPrimaryButtonEnabled"/> dependency property.</summary>
    public static readonly DependencyProperty IsPrimaryButtonEnabledProperty = DependencyProperty.Register(
        nameof(IsPrimaryButtonEnabled),
        typeof(bool),
        typeof(ContentDialog),
        new PropertyMetadata(true)
    );

    /// <summary>Identifies the <see cref="IsSecondaryButtonEnabled"/> dependency property.</summary>
    public static readonly DependencyProperty IsSecondaryButtonEnabledProperty = DependencyProperty.Register(
        nameof(IsSecondaryButtonEnabled),
        typeof(bool),
        typeof(ContentDialog),
        new PropertyMetadata(true)
    );

    /// <summary>Identifies the <see cref="PrimaryButtonAppearance"/> dependency property.</summary>
    public static readonly DependencyProperty PrimaryButtonAppearanceProperty = DependencyProperty.Register(
        nameof(PrimaryButtonAppearance),
        typeof(ControlAppearance),
        typeof(ContentDialog),
        new PropertyMetadata(ControlAppearance.Primary)
    );

    /// <summary>Identifies the <see cref="SecondaryButtonAppearance"/> dependency property.</summary>
    public static readonly DependencyProperty SecondaryButtonAppearanceProperty = DependencyProperty.Register(
        nameof(SecondaryButtonAppearance),
        typeof(ControlAppearance),
        typeof(ContentDialog),
        new PropertyMetadata(ControlAppearance.Secondary)
    );

    /// <summary>Identifies the <see cref="CloseButtonAppearance"/> dependency property.</summary>
    public static readonly DependencyProperty CloseButtonAppearanceProperty = DependencyProperty.Register(
        nameof(CloseButtonAppearance),
        typeof(ControlAppearance),
        typeof(ContentDialog),
        new PropertyMetadata(ControlAppearance.Secondary)
    );

    /// <summary>Identifies the <see cref="DefaultButton"/> dependency property.</summary>
    public static readonly DependencyProperty DefaultButtonProperty = DependencyProperty.Register(
        nameof(DefaultButton),
        typeof(ContentDialogButton),
        typeof(ContentDialog),
        new PropertyMetadata(ContentDialogButton.Primary)
    );

    /// <summary>Identifies the <see cref="IsFooterVisible"/> dependency property.</summary>
    public static readonly DependencyProperty IsFooterVisibleProperty = DependencyProperty.Register(
        nameof(IsFooterVisible),
        typeof(bool),
        typeof(ContentDialog),
        new PropertyMetadata(true)
    );

    /// <summary>Identifies the <see cref="TemplateButtonCommand"/> dependency property.</summary>
    public static readonly DependencyProperty TemplateButtonCommandProperty = DependencyProperty.Register(
        nameof(TemplateButtonCommand),
        typeof(IRelayCommand),
        typeof(ContentDialog),
        new PropertyMetadata(null)
    );

    /// <summary>Identifies the <see cref="Opened"/> routed event.</summary>
    public static readonly RoutedEvent OpenedEvent = EventManager.RegisterRoutedEvent(
        nameof(Opened),
        RoutingStrategy.Bubble,
        typeof(TypedEventHandler<ContentDialog, RoutedEventArgs>),
        typeof(ContentDialog)
    );

    /// <summary>Identifies the <see cref="Closing"/> routed event.</summary>
    public static readonly RoutedEvent ClosingEvent = EventManager.RegisterRoutedEvent(
        nameof(Closing),
        RoutingStrategy.Bubble,
        typeof(TypedEventHandler<ContentDialog, ContentDialogClosingEventArgs>),
        typeof(ContentDialog)
    );

    /// <summary>Identifies the <see cref="Closed"/> routed event.</summary>
    public static readonly RoutedEvent ClosedEvent = EventManager.RegisterRoutedEvent(
        nameof(Closed),
        RoutingStrategy.Bubble,
        typeof(TypedEventHandler<ContentDialog, ContentDialogClosedEventArgs>),
        typeof(ContentDialog)
    );

    /// <summary>Identifies the <see cref="ButtonClicked"/> routed event.</summary>
    public static readonly RoutedEvent ButtonClickedEvent = EventManager.RegisterRoutedEvent(
        nameof(ButtonClicked),
        RoutingStrategy.Bubble,
        typeof(TypedEventHandler<ContentDialog, ContentDialogButtonClickEventArgs>),
        typeof(ContentDialog)
    );

    /// <summary>
    /// Gets or sets the title of the <see cref="ContentDialog"/>.
    /// </summary>
    public object? Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    /// <summary>
    /// Gets or sets the title template of the <see cref="ContentDialog"/>.
    /// </summary>
    public DataTemplate? TitleTemplate
    {
        get => (DataTemplate?)GetValue(TitleTemplateProperty);
        set => SetValue(TitleTemplateProperty, value);
    }

    /// <summary>
    /// Gets or sets the width of the <see cref="ContentDialog"/>.
    /// </summary>
    public double DialogWidth
    {
        get => (double)GetValue(DialogWidthProperty);
        set => SetValue(DialogWidthProperty, value);
    }

    /// <summary>
    /// Gets or sets the height of the <see cref="ContentDialog"/>.
    /// </summary>
    public double DialogHeight
    {
        get => (double)GetValue(DialogHeightProperty);
        set => SetValue(DialogHeightProperty, value);
    }

    /// <summary>
    /// Gets or sets the max width of the <see cref="ContentDialog"/>.
    /// </summary>
    public double DialogMaxWidth
    {
        get => (double)GetValue(DialogMaxWidthProperty);
        set => SetValue(DialogMaxWidthProperty, value);
    }

    /// <summary>
    /// Gets or sets the max height of the <see cref="ContentDialog"/>.
    /// </summary>
    public double DialogMaxHeight
    {
        get => (double)GetValue(DialogMaxHeightProperty);
        set => SetValue(DialogMaxHeightProperty, value);
    }

    /// <summary>
    /// Gets or sets the margin of the <see cref="ContentDialog"/>.
    /// </summary>
    public Thickness DialogMargin
    {
        get => (Thickness)GetValue(DialogMarginProperty);
        set => SetValue(DialogMarginProperty, value);
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
    /// Gets or sets the <see cref="SymbolRegular"/> on the secondary button.
    /// </summary>
    public IconElement? PrimaryButtonIcon
    {
        get => (IconElement?)GetValue(PrimaryButtonIconProperty);
        set => SetValue(PrimaryButtonIconProperty, value);
    }

    /// <summary>
    /// Gets or sets the <see cref="SymbolRegular"/> on the primary button.
    /// </summary>
    public IconElement? SecondaryButtonIcon
    {
        get => (IconElement?)GetValue(SecondaryButtonIconProperty);
        set => SetValue(SecondaryButtonIconProperty, value);
    }

    /// <summary>
    /// Gets or sets the <see cref="SymbolRegular"/> on the close button.
    /// </summary>
    public IconElement? CloseButtonIcon
    {
        get => (IconElement?)GetValue(CloseButtonIconProperty);
        set => SetValue(CloseButtonIconProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the <see cref="ContentDialog"/> primary button is enabled.
    /// </summary>
    public bool IsPrimaryButtonEnabled
    {
        get => (bool)GetValue(IsPrimaryButtonEnabledProperty);
        set => SetValue(IsPrimaryButtonEnabledProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the <see cref="ContentDialog"/> secondary button is enabled.
    /// </summary>
    public bool IsSecondaryButtonEnabled
    {
        get => (bool)GetValue(IsSecondaryButtonEnabledProperty);
        set => SetValue(IsSecondaryButtonEnabledProperty, value);
    }

    /// <summary>
    /// Gets or sets the <see cref="ControlAppearance"/> to apply to the primary button.
    /// </summary>
    public ControlAppearance PrimaryButtonAppearance
    {
        get => (ControlAppearance)GetValue(PrimaryButtonAppearanceProperty);
        set => SetValue(PrimaryButtonAppearanceProperty, value);
    }

    /// <summary>
    /// Gets or sets the <see cref="ControlAppearance"/> to apply to the secondary button.
    /// </summary>
    public ControlAppearance SecondaryButtonAppearance
    {
        get => (ControlAppearance)GetValue(SecondaryButtonAppearanceProperty);
        set => SetValue(SecondaryButtonAppearanceProperty, value);
    }

    /// <summary>
    /// Gets or sets the <see cref="ControlAppearance"/> to apply to the close button.
    /// </summary>
    public ControlAppearance CloseButtonAppearance
    {
        get => (ControlAppearance)GetValue(CloseButtonAppearanceProperty);
        set => SetValue(CloseButtonAppearanceProperty, value);
    }

    /// <summary>
    /// Gets or sets a value that indicates which button on the dialog is the default action.
    /// </summary>
    public ContentDialogButton DefaultButton
    {
        get => (ContentDialogButton)GetValue(DefaultButtonProperty);
        set => SetValue(DefaultButtonProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the footer buttons are visible.
    /// </summary>
    public bool IsFooterVisible
    {
        get => (bool)GetValue(IsFooterVisibleProperty);
        set => SetValue(IsFooterVisibleProperty, value);
    }

    /// <summary>
    /// Gets command triggered after clicking the button in the template.
    /// </summary>
    public IRelayCommand TemplateButtonCommand => (IRelayCommand)GetValue(TemplateButtonCommandProperty);

    /// <summary>
    /// Occurs after the dialog is opened.
    /// </summary>
    public event TypedEventHandler<ContentDialog, RoutedEventArgs> Opened
    {
        add => AddHandler(OpenedEvent, value);
        remove => RemoveHandler(OpenedEvent, value);
    }

    /// <summary>
    /// Occurs after the dialog starts to close, but before it is closed and before the <see cref="Closed"/> event occurs.
    /// </summary>
    public event TypedEventHandler<ContentDialog, ContentDialogClosingEventArgs> Closing
    {
        add => AddHandler(ClosingEvent, value);
        remove => RemoveHandler(ClosingEvent, value);
    }

    /// <summary>
    /// Occurs after the dialog is closed.
    /// </summary>
    public event TypedEventHandler<ContentDialog, ContentDialogClosedEventArgs> Closed
    {
        add => AddHandler(ClosedEvent, value);
        remove => RemoveHandler(ClosedEvent, value);
    }

    /// <summary>
    /// Occurs after the <see cref="ContentDialogButton"/> has been tapped.
    /// </summary>
    public event TypedEventHandler<ContentDialog, ContentDialogButtonClickEventArgs> ButtonClicked
    {
        add => AddHandler(ButtonClickedEvent, value);
        remove => RemoveHandler(ButtonClickedEvent, value);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ContentDialog"/> class.
    /// </summary>
    public ContentDialog()
    {
        SetValue(TemplateButtonCommandProperty, new RelayCommand<ContentDialogButton>(OnButtonClick));

        // Avoid registering runtime code that triggers designer behavior or throws exceptions
        // at design time (to reduce the possibility of designer crashes/rendering failures).
        if (!Designer.DesignerHelper.IsInDesignMode)
        {
            Loaded += static (sender, _) =>
            {
                var self = (ContentDialog)sender;
                self.OnLoaded();
            };
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ContentDialog"/> class.
    /// </summary>
    /// <param name="dialogHost"><see cref="DialogHost"/> inside of which the dialogue will be placed. The new <see cref="ContentDialog"/> will replace the current <see cref="ContentPresenter.Content"/>.</param>
    public ContentDialog(ContentPresenter? dialogHost)
    {
        if (dialogHost is null)
        {
            throw new ArgumentNullException(nameof(dialogHost));
        }

        DialogHost = dialogHost;

        SetValue(TemplateButtonCommandProperty, new RelayCommand<ContentDialogButton>(OnButtonClick));

        // Avoid registering runtime code that triggers designer behavior or throws exceptions
        // at design time (to reduce the possibility of designer crashes/rendering failures).
        if (!Designer.DesignerHelper.IsInDesignMode)
        {
            Loaded += static (sender, _) =>
            {
                var self = (ContentDialog)sender;
                self.OnLoaded();
            };
        }
    }

    /// <summary>
    ///  Gets or sets <see cref="DialogHost"/> inside of which the dialogue will be placed. The new <see cref="ContentDialog"/> will replace the current <see cref="ContentPresenter.Content"/>.
    /// </summary>
    public ContentPresenter? DialogHost { get; set; } = default;

    [Obsolete("ContentPresenter is deprecated. Please use DialogHost instead.")]
    public ContentPresenter? ContentPresenter { get; set; } = default;

    protected TaskCompletionSource<ContentDialogResult>? Tcs { get; set; }

    /// <summary>
    /// Shows the dialog
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "WpfAnalyzers.DependencyProperty",
        "WPF0041:Set mutable dependency properties using SetCurrentValue",
        Justification = "SetCurrentValue(ContentProperty, ...) will not work"
    )]
    public async Task<ContentDialogResult> ShowAsync(CancellationToken cancellationToken = default)
    {
        if (DialogHost is null)
        {
            throw new InvalidOperationException("DialogHost was not set");
        }

        // Before displaying the dialog, clear the Enter and Escape access key registrations of the host window.
        ClearHostWindowEnterAndEscapeAccessKey(DialogHost);

        Tcs = new TaskCompletionSource<ContentDialogResult>();
        CancellationTokenRegistration tokenRegistration = cancellationToken.Register(
            o => Tcs.TrySetCanceled((CancellationToken)o!),
            cancellationToken
        );

        ContentDialogResult result = ContentDialogResult.None;

        try
        {
            DialogHost.Content = this;
            result = await Tcs.Task;

            return result;
        }
        finally
        {
#if NET6_0_OR_GREATER
            await tokenRegistration.DisposeAsync();
#else
            tokenRegistration.Dispose();
#endif
            DialogHost.Content = null;
            OnClosed(result);
        }
    }

    /// <summary>
    /// Hides the dialog with result
    /// </summary>
    public virtual void Hide(ContentDialogResult result = ContentDialogResult.None)
    {
        var closingEventArgs = new ContentDialogClosingEventArgs(ClosingEvent, this) { Result = result };

        RaiseEvent(closingEventArgs);

        if (!closingEventArgs.Cancel)
        {
            _ = Tcs?.TrySetResult(result);

            // After closing the dialog, restore any previously removed access keys.
            RestoreHostWindowAccessKeys();
        }
    }

    /// <summary>
    /// Occurs after ContentPresenter.Content = null
    /// </summary>
    protected virtual void OnClosed(ContentDialogResult result)
    {
        var closedEventArgs = new ContentDialogClosedEventArgs(ClosedEvent, this) { Result = result };

        RaiseEvent(closedEventArgs);
    }

    /// <summary>
    /// Invoked when a <see cref="ContentDialogButton"/> is clicked.
    /// </summary>
    /// <param name="button">The button that was clicked.</param>
    protected virtual void OnButtonClick(ContentDialogButton button)
    {
        var buttonClickEventArgs = new ContentDialogButtonClickEventArgs(ButtonClickedEvent, this)
        {
            Button = button,
        };

        RaiseEvent(buttonClickEventArgs);

        ContentDialogResult result = button switch
        {
            ContentDialogButton.Primary => ContentDialogResult.Primary,
            ContentDialogButton.Secondary => ContentDialogResult.Secondary,
            _ => ContentDialogResult.None,
        };

        Hide(result);
    }

    protected override Size MeasureOverride(Size availableSize)
    {
        // Avoid throwing exceptions when visual child elements cannot be obtained (designer or template not applied).
        if (VisualChildrenCount == 0)
        {
            return base.MeasureOverride(availableSize);
        }

        var rootElement = (UIElement)GetVisualChild(0)!;

        rootElement.Measure(availableSize);
        Size desiredSize = rootElement.DesiredSize;

        Size newSize = GetNewDialogSize(desiredSize);

        SetCurrentValue(DialogHeightProperty, newSize.Height);
        SetCurrentValue(DialogWidthProperty, newSize.Width);

        ResizeWidth(rootElement);
        ResizeHeight(rootElement);

        return desiredSize;
    }

    /// <summary>
    /// Occurs after Loaded event
    /// </summary>
    protected virtual void OnLoaded()
    {
        // Focus is only needed at runtime.
        _ = Focus();

        // Set focus on the default button to match the default behavior of Windows dialog boxes.
        TryFocusDefaultButton();

        RaiseEvent(new RoutedEventArgs(OpenedEvent));
    }

    private Size GetNewDialogSize(Size desiredSize)
    {
        // TODO: Handle negative values
        var paddingWidth = Padding.Left + Padding.Right;
        var paddingHeight = Padding.Top + Padding.Bottom;

        var marginHeight = DialogMargin.Bottom + DialogMargin.Top;
        var marginWidth = DialogMargin.Left + DialogMargin.Right;

        var width = desiredSize.Width - marginWidth + paddingWidth;
        var height = desiredSize.Height - marginHeight + paddingHeight;

        return new Size(width, height);
    }

    private void ResizeWidth(UIElement element)
    {
        if (DialogWidth <= DialogMaxWidth)
        {
            return;
        }

        SetCurrentValue(DialogWidthProperty, DialogMaxWidth);
        element.UpdateLayout();

        SetCurrentValue(DialogHeightProperty, element.DesiredSize.Height);

        if (DialogHeight > DialogMaxHeight)
        {
            SetCurrentValue(DialogMaxHeightProperty, DialogHeight);
            /*Debug.WriteLine($"DEBUG | {GetType()} | WARNING | DialogHeight > DialogMaxHeight after resizing width!");*/
        }
    }

    private void ResizeHeight(UIElement element)
    {
        if (DialogHeight <= DialogMaxHeight)
        {
            return;
        }

        SetCurrentValue(DialogHeightProperty, DialogMaxHeight);
        element.UpdateLayout();

        SetCurrentValue(DialogWidthProperty, element.DesiredSize.Width);

        if (DialogWidth > DialogMaxWidth)
        {
            SetCurrentValue(DialogMaxWidthProperty, DialogWidth);
            /*Debug.WriteLine($"DEBUG | {GetType()} | WARNING | DialogWidth > DialogMaxWidth after resizing height!");*/
        }
    }

    /// <summary>
    /// Attempts to set keyboard focus to the available default button,
    /// enabling activation by the Space key in accordance with Windows dialog behavior.
    /// </summary>
    /// <remarks>
    /// While it's generally not recommended to set the secondary button as default with initial focus,
    /// this implementation respects the user's choice when explicitly configured.
    /// </remarks>
    private void TryFocusDefaultButton()
    {
        if (Dispatcher is not { HasShutdownStarted: false, HasShutdownFinished: false })
        {
            return;
        }

        try
        {
            // The user may have already set focus to a control in the Content before this point.
            // Here we check whether focus remains on this `ContentDialog`.
            // If so, the user has not overridden (or intends to defer overriding) the initial focus.
            // To maintain Windows dialog behavior conventions, we set focus to the default button.
            if (ReferenceEquals(Keyboard.FocusedElement, this))
            {
                // If primary button is present, visible, enabled and is default, focus it.
                if (GetTemplateChild("PrimaryButton")
                    is Button { IsVisible: true, IsEnabled: true, IsDefault: true } primaryButton)
                {
                    primaryButton.Focus();
                    return;
                }

                // Otherwise, if close button is present, visible, enabled and is default, focus it.
                if (GetTemplateChild("CloseButton")
                    is Button { IsVisible: true, IsEnabled: true, IsDefault: true } closeButton)
                {
                    closeButton.Focus();
                    return;
                }

                // If the user explicitly designates the secondary button as the default,
                // set focus to the secondary button (respecting the user's choice).
                if (GetTemplateChild("SecondaryButton")
                    is Button { IsVisible: true, IsEnabled: true, IsDefault: true } secondaryButton)
                {
                    secondaryButton.Focus();
                }

                // If the user chooses to defer focus setting (e.g., via Dispatcher with lower priority),
                // their code will override our focus setting here, so this won't interfere with user's intentions.
            }
        }
        catch
        {
            // ignore
        }
    }

    private void ClearHostWindowEnterAndEscapeAccessKey(DependencyObject hostElement)
    {
        try
        {
            var window = Window.GetWindow(hostElement);
            if (window is null)
            {
                return;
            }

            Dispatcher? dispatcher = window.Dispatcher;
            if (dispatcher is null || dispatcher.HasShutdownStarted || dispatcher.HasShutdownFinished)
            {
                return;
            }

            // Temporarily register an AccessKey handler to suppress Enter/Esc keys in host window.
            AccessKeyManager.AddAccessKeyPressedHandler(window, HostAccessKeySuppressHandler);
        }
        catch
        {
            // Ignore any errors to prevent affecting dialog display.
        }
    }

    private void RestoreHostWindowAccessKeys()
    {
        try
        {
            Window? window = DialogHost is null ? null : Window.GetWindow(DialogHost);
            if (window != null)
            {
                AccessKeyManager.RemoveAccessKeyPressedHandler(window, HostAccessKeySuppressHandler);
            }
        }
        catch
        {
            // ignore
        }
    }

    private bool IsPartOfThisDialog(DependencyObject d)
    {
        if (d == this)
        {
            return true;
        }

        // If DialogHost is not set, fallback to checking this dialog only.
        if (DialogHost == null)
        {
            return false;
        }

        // Walk up visual/logical tree and see if we reach DialogHost or this ContentDialog.
        DependencyObject? current = d;
        while (current != null)
        {
            if (current == DialogHost || current == this)
            {
                return true;
            }

            DependencyObject? parent;
            try
            {
                parent = VisualTreeHelper.GetParent(current);
            }
            catch
            {
                parent = null;
            }

            if (parent == null)
            {
                try
                {
                    parent = LogicalTreeHelper.GetParent(current);
                }
                catch
                {
                    parent = null;
                }
            }

            current = parent;
        }

        return false;
    }

    private void HostAccessKeySuppressHandler(object sender, AccessKeyPressedEventArgs e)
    {
        try
        {
            var key = e.Key ?? string.Empty;

            if (key is "\r" or "\e")
            {
                UIElement? target = e.Target;
                if (target is null)
                {
                    return; // nothing to do
                }

                // If the target is part of this dialog (or the dialog host), allow it.
                if (IsPartOfThisDialog(target))
                {
                    return;
                }

                // Otherwise suppress the access key for this window by clearing the Target.
                e.Target = null;
            }
        }
        catch
        {
            // ignore
        }
    }
}
