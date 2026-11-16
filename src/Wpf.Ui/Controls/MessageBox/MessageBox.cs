// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Reflection;
using System.Windows.Input;
using System.Windows.Threading;
using Windows.Win32;
using Windows.Win32.Foundation;
using Wpf.Ui.Input;
using Wpf.Ui.Interop;
using Size = System.Windows.Size;
#if NET8_0_OR_GREATER
using System.Runtime.CompilerServices;
#endif

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

/// <summary>
/// Customized window for notifications.
/// </summary>
public class MessageBox : System.Windows.Window
{
    /// <summary>Identifies the <see cref="ShowTitle"/> dependency property.</summary>
    public static readonly DependencyProperty ShowTitleProperty = DependencyProperty.Register(
        nameof(ShowTitle),
        typeof(bool),
        typeof(MessageBox),
        new PropertyMetadata(true)
    );

    /// <summary>Identifies the <see cref="PrimaryButtonText"/> dependency property.</summary>
    public static readonly DependencyProperty PrimaryButtonTextProperty = DependencyProperty.Register(
        nameof(PrimaryButtonText),
        typeof(string),
        typeof(MessageBox),
        new PropertyMetadata(string.Empty)
    );

    /// <summary>Identifies the <see cref="SecondaryButtonText"/> dependency property.</summary>
    public static readonly DependencyProperty SecondaryButtonTextProperty = DependencyProperty.Register(
        nameof(SecondaryButtonText),
        typeof(string),
        typeof(MessageBox),
        new PropertyMetadata(string.Empty)
    );

    /// <summary>Identifies the <see cref="CloseButtonText"/> dependency property.</summary>
    public static readonly DependencyProperty CloseButtonTextProperty = DependencyProperty.Register(
        nameof(CloseButtonText),
        typeof(string),
        typeof(MessageBox),
        new PropertyMetadata("Close")
    );

    /// <summary>Identifies the <see cref="PrimaryButtonIcon"/> dependency property.</summary>
    public static readonly DependencyProperty PrimaryButtonIconProperty = DependencyProperty.Register(
        nameof(PrimaryButtonIcon),
        typeof(IconElement),
        typeof(MessageBox),
        new PropertyMetadata(null)
    );

    /// <summary>Identifies the <see cref="SecondaryButtonIcon"/> dependency property.</summary>
    public static readonly DependencyProperty SecondaryButtonIconProperty = DependencyProperty.Register(
        nameof(SecondaryButtonIcon),
        typeof(IconElement),
        typeof(MessageBox),
        new PropertyMetadata(null)
    );

    /// <summary>Identifies the <see cref="CloseButtonIcon"/> dependency property.</summary>
    public static readonly DependencyProperty CloseButtonIconProperty = DependencyProperty.Register(
        nameof(CloseButtonIcon),
        typeof(IconElement),
        typeof(MessageBox),
        new PropertyMetadata(null)
    );

    /// <summary>Identifies the <see cref="PrimaryButtonAppearance"/> dependency property.</summary>
    public static readonly DependencyProperty PrimaryButtonAppearanceProperty = DependencyProperty.Register(
        nameof(PrimaryButtonAppearance),
        typeof(ControlAppearance),
        typeof(MessageBox),
        new PropertyMetadata(ControlAppearance.Primary)
    );

    /// <summary>Identifies the <see cref="SecondaryButtonAppearance"/> dependency property.</summary>
    public static readonly DependencyProperty SecondaryButtonAppearanceProperty = DependencyProperty.Register(
        nameof(SecondaryButtonAppearance),
        typeof(ControlAppearance),
        typeof(MessageBox),
        new PropertyMetadata(ControlAppearance.Secondary)
    );

    /// <summary>Identifies the <see cref="CloseButtonAppearance"/> dependency property.</summary>
    public static readonly DependencyProperty CloseButtonAppearanceProperty = DependencyProperty.Register(
        nameof(CloseButtonAppearance),
        typeof(ControlAppearance),
        typeof(MessageBox),
        new PropertyMetadata(ControlAppearance.Secondary)
    );

    /// <summary>Identifies the <see cref="IsPrimaryButtonEnabled"/> dependency property.</summary>
    public static readonly DependencyProperty IsPrimaryButtonEnabledProperty = DependencyProperty.Register(
        nameof(IsPrimaryButtonEnabled),
        typeof(bool),
        typeof(MessageBox),
        new PropertyMetadata(true)
    );

    /// <summary>Identifies the <see cref="IsSecondaryButtonEnabled"/> dependency property.</summary>
    public static readonly DependencyProperty IsSecondaryButtonEnabledProperty = DependencyProperty.Register(
        nameof(IsSecondaryButtonEnabled),
        typeof(bool),
        typeof(MessageBox),
        new PropertyMetadata(true)
    );

    /// <summary>Identifies the <see cref="IsCloseButtonEnabled"/> dependency property.</summary>
    public static readonly DependencyProperty IsCloseButtonEnabledProperty = DependencyProperty.Register(
        nameof(IsCloseButtonEnabled),
        typeof(bool),
        typeof(MessageBox),
        new PropertyMetadata(true)
    );

    /// <summary>Identifies the <see cref="TemplateButtonCommand"/> dependency property.</summary>
    public static readonly DependencyProperty TemplateButtonCommandProperty = DependencyProperty.Register(
        nameof(TemplateButtonCommand),
        typeof(IRelayCommand),
        typeof(MessageBox),
        new PropertyMetadata(null)
    );

    /// <summary>Identifies the <see cref="DefaultFocusedButton"/> dependency property.</summary>
    public static readonly DependencyProperty DefaultFocusedButtonProperty = DependencyProperty.Register(
        nameof(DefaultFocusedButton),
        typeof(MessageBoxButton?),
        typeof(MessageBox),
        new PropertyMetadata(null)
    );

    /// <summary>
    /// Gets or sets a value indicating whether to show the <see cref="System.Windows.Window.Title"/> in <see cref="TitleBar"/>.
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
    public IconElement? PrimaryButtonIcon
    {
        get => (IconElement?)GetValue(PrimaryButtonIconProperty);
        set => SetValue(PrimaryButtonIconProperty, value);
    }

    /// <summary>
    /// Gets or sets the <see cref="SymbolRegular"/> on the secondary button
    /// </summary>
    public IconElement? SecondaryButtonIcon
    {
        get => (IconElement?)GetValue(SecondaryButtonIconProperty);
        set => SetValue(SecondaryButtonIconProperty, value);
    }

    /// <summary>
    /// Gets or sets the <see cref="SymbolRegular"/> on the close button
    /// </summary>
    public IconElement? CloseButtonIcon
    {
        get => (IconElement?)GetValue(CloseButtonIconProperty);
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
    /// Gets or sets a value indicating whether the <see cref="MessageBox"/> close button is enabled.
    /// </summary>
    public bool IsCloseButtonEnabled
    {
        get => (bool)GetValue(IsCloseButtonEnabledProperty);
        set => SetValue(IsCloseButtonEnabledProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the <see cref="MessageBox"/> secondary button is enabled.
    /// </summary>
    public bool IsSecondaryButtonEnabled
    {
        get => (bool)GetValue(IsSecondaryButtonEnabledProperty);
        set => SetValue(IsSecondaryButtonEnabledProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the <see cref="MessageBox"/> primary button is enabled.
    /// </summary>
    public bool IsPrimaryButtonEnabled
    {
        get => (bool)GetValue(IsPrimaryButtonEnabledProperty);
        set => SetValue(IsPrimaryButtonEnabledProperty, value);
    }

    /// <summary>
    /// Gets the command triggered after clicking the button on the Footer.
    /// </summary>
    public IRelayCommand TemplateButtonCommand => (IRelayCommand)GetValue(TemplateButtonCommandProperty);

    /// <summary>
    /// Gets or sets the button that should receive focus when the MessageBox is displayed.
    /// If null, focus will be set to the first available button (Primary > Secondary > Close).
    /// </summary>
    public MessageBoxButton? DefaultFocusedButton
    {
        get => (MessageBoxButton?)GetValue(DefaultFocusedButtonProperty);
        set => SetValue(DefaultFocusedButtonProperty, value);
    }

#if !NET8_0_OR_GREATER
    private static readonly PropertyInfo CanCenterOverWPFOwnerPropertyInfo = typeof(Window).GetProperty(
        "CanCenterOverWPFOwner",
        BindingFlags.NonPublic | BindingFlags.Instance
    )!;
#endif

    private DispatcherOperation? _initialFocusOperation;
    private bool _centerOnScreenWhenLoaded;

    /// <summary>
    /// Initializes a new instance of the <see cref="MessageBox"/> class.
    /// </summary>
    public MessageBox()
    {
        Topmost = true;
        SetValue(TemplateButtonCommandProperty, new RelayCommand<MessageBoxButton>(OnButtonClick));

        PreviewMouseDoubleClick += static (_, args) => args.Handled = true;

        Loaded += static (sender, _) =>
        {
            var self = (MessageBox)sender!;
            self.OnLoaded();
        };

        ContentRendered += static (sender, _) =>
        {
            var self = (MessageBox)sender!;
            self.OnContentRendered();
        };
    }

    protected TaskCompletionSource<MessageBoxResult>? Tcs { get; set; }

    [Obsolete($"Use {nameof(ShowDialogAsync)} instead")]
    public new void Show()
    {
        throw new InvalidOperationException($"Use {nameof(ShowDialogAsync)} instead");
    }

    [Obsolete($"Use {nameof(ShowDialogAsync)} instead")]
    public new bool? ShowDialog()
    {
        throw new InvalidOperationException($"Use {nameof(ShowDialogAsync)} instead");
    }

    [Obsolete($"Use {nameof(Close)} with MessageBoxResult instead")]
    public new void Close()
    {
        throw new InvalidOperationException($"Use {nameof(Close)} with MessageBoxResult instead");
    }

    /// <summary>
    /// Displays a message box
    /// </summary>
    /// <returns><see cref="MessageBoxResult"/></returns>
    /// <exception cref="TaskCanceledException">Thrown if the operation is canceled.</exception>
    public async Task<MessageBoxResult> ShowDialogAsync(
        bool showAsDialog = true,
        CancellationToken cancellationToken = default
    )
    {
        Tcs = new TaskCompletionSource<MessageBoxResult>();
        CancellationTokenRegistration tokenRegistration = cancellationToken.Register(
            o => Tcs.TrySetCanceled((CancellationToken)o!),
            cancellationToken
        );

        try
        {
            RemoveTitleBarAndApplyMica();

            // Remember unspecified coordinates before WPF supplies the initial window position.
            _centerOnScreenWhenLoaded =
                WindowStartupLocation == WindowStartupLocation.Manual
                && double.IsNaN(Left)
                && double.IsNaN(Top);

            if (showAsDialog)
            {
                base.ShowDialog();
            }
            else
            {
                base.Show();
            }

            return await Tcs.Task;
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
    /// Occurs after Loading event
    /// </summary>
    protected virtual void OnLoaded()
    {
        var rootElement = (UIElement)GetVisualChild(0)!;

        ResizeToContentSize(rootElement);

        switch (WindowStartupLocation)
        {
            case WindowStartupLocation.Manual:
                if (_centerOnScreenWhenLoaded || (double.IsNaN(Left) && double.IsNaN(Top)))
                {
                    CenterWindowOnScreen();
                }

                break;
            case WindowStartupLocation.CenterScreen:
                CenterWindowOnScreen();
                break;
            case WindowStartupLocation.CenterOwner:
                if (Owner == null || !CanCenterOverWPFOwner() || Owner.WindowState is WindowState.Minimized)
                {
                    CenterWindowOnScreen();
                }
                else
                {
                    CenterWindowOnOwner();
                }

                break;
            default:
                throw new InvalidOperationException();
        }

        QueueInitialFocus();
    }

    // CanCenterOverWPFOwner property see https://source.dot.net/#PresentationFramework/System/Windows/Window.cs,e679e433777b21b8
    private bool CanCenterOverWPFOwner()
    {
#if NET8_0_OR_GREATER
        return CanCenterOverWPFOwnerAccessor(this);
#else
        return (bool)CanCenterOverWPFOwnerPropertyInfo.GetValue(this)!;
#endif
    }

#if NET8_0_OR_GREATER
    [UnsafeAccessor(UnsafeAccessorKind.Method, Name = "get_CanCenterOverWPFOwner")]
    private static extern bool CanCenterOverWPFOwnerAccessor(Window w);
#endif

    /// <summary>
    /// Resizes the MessageBox to fit the content's size, including margins.
    /// </summary>
    /// <param name="rootElement">The root element of the MessageBox</param>
    protected virtual void ResizeToContentSize(UIElement rootElement)
    {
        Size desiredSize = rootElement.DesiredSize;

        // left and right margin
        const double margin = 12.0 * 2;

        SetCurrentValue(WidthProperty, desiredSize.Width + margin);
        SetCurrentValue(HeightProperty, desiredSize.Height);

        ResizeWidth(rootElement);
        ResizeHeight(rootElement);
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        if (IsLoaded)
        {
            QueueInitialFocus();
        }
    }

    /// <summary>
    /// Occurs after the content is rendered.
    /// </summary>
    protected virtual void OnContentRendered()
    {
        QueueInitialFocus();
    }

    protected override void OnActivated(EventArgs e)
    {
        base.OnActivated(e);

        if (IsLoaded)
        {
            QueueInitialFocus();
        }
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        base.OnClosing(e);

        if (e.Cancel)
        {
            return;
        }

        _ = Tcs?.TrySetResult(MessageBoxResult.None);
    }

    protected override void OnClosed(EventArgs e)
    {
        // Closing is not raised when an owner closes its non-modal owned windows.
        _initialFocusOperation?.Abort();
        _initialFocusOperation = null;
        _ = Tcs?.TrySetResult(MessageBoxResult.None);
        base.OnClosed(e);
    }

    protected virtual void CenterWindowOnScreen()
    {
        double screenWidth = SystemParameters.PrimaryScreenWidth;
        double screenHeight = SystemParameters.PrimaryScreenHeight;

        SetCurrentValue(LeftProperty, (screenWidth / 2) - (Width / 2));
        SetCurrentValue(TopProperty, (screenHeight / 2) - (Height / 2));
    }

    private void CenterWindowOnOwner()
    {
        if (
            PresentationSource.FromVisual(this)?.CompositionTarget is not { } compositionTarget
            || !PInvoke.GetWindowRect(new HWND(new WindowInteropHelper(Owner).Handle), out RECT ownerRect)
            || !PInvoke.GetWindowRect(new HWND(new WindowInteropHelper(this).Handle), out RECT messageBoxRect)
        )
        {
            return;
        }

        // Use the displayed bounds, including maximized owners, in the MessageBox's DPI coordinate space.
        Point ownerCenter = compositionTarget.TransformFromDevice.Transform(
            new Point(
                ownerRect.left + ((ownerRect.right - ownerRect.left) / 2.0),
                ownerRect.top + ((ownerRect.bottom - ownerRect.top) / 2.0)
            )
        );
        Vector windowSize = compositionTarget.TransformFromDevice.Transform(
            new Vector(messageBoxRect.right - messageBoxRect.left, messageBoxRect.bottom - messageBoxRect.top)
        );
        double left = ownerCenter.X - (windowSize.X / 2);
        double top = ownerCenter.Y - (windowSize.Y / 2);

        SetCurrentValue(LeftProperty, left);
        SetCurrentValue(TopProperty, top);
    }

    /// <summary>
    /// Occurs after the <see cref="MessageBoxButton"/> is clicked
    /// </summary>
    /// <param name="button">The MessageBox button</param>
    protected virtual void OnButtonClick(MessageBoxButton button)
    {
        MessageBoxResult result = button switch
        {
            MessageBoxButton.Primary => MessageBoxResult.Primary,
            MessageBoxButton.Secondary => MessageBoxResult.Secondary,
            _ => MessageBoxResult.None,
        };

        _ = Tcs?.TrySetResult(result);
        base.Close();
    }

    private void RemoveTitleBarAndApplyMica()
    {
        _ = UnsafeNativeMethods.RemoveWindowTitlebarContents(this);
        _ = WindowBackdrop.ApplyBackdrop(this, WindowBackdropType.Mica);
    }

    private void ResizeWidth(UIElement element)
    {
        if (Width <= MaxWidth)
        {
            return;
        }

        SetCurrentValue(WidthProperty, MaxWidth);
        element.UpdateLayout();

        SetCurrentValue(HeightProperty, element.DesiredSize.Height);

        if (Height > MaxHeight)
        {
            SetCurrentValue(MaxHeightProperty, Height);
        }
    }

    private void ResizeHeight(UIElement element)
    {
        if (Height <= MaxHeight)
        {
            return;
        }

        SetCurrentValue(HeightProperty, MaxHeight);
        element.UpdateLayout();

        SetCurrentValue(WidthProperty, element.DesiredSize.Width);

        if (Width > MaxWidth)
        {
            SetCurrentValue(MaxWidthProperty, Width);
        }
    }

    private void QueueInitialFocus()
    {
        if (_initialFocusOperation is { Status: DispatcherOperationStatus.Pending })
        {
            return;
        }

        // Allow Loaded and ContentRendered handlers to supply their own focus first.
        _initialFocusOperation = Dispatcher.BeginInvoke(
            DispatcherPriority.Input,
            new Action(() =>
            {
                _initialFocusOperation = null;
                SetFocusToFirstAvailableButton();
            })
        );
    }

    private bool IsValidContentFocus(IInputElement? element)
    {
        if (
            element is not DependencyObject dependencyObject
            || element == this
            || GetWindow(dependencyObject) != this
        )
        {
            return false;
        }

        if (element is UIElement uiElement)
        {
            if (
                (element == GetTemplateChild("PART_MessageBoxPrimaryButton") && !IsPrimaryButtonEnabled)
                || (
                    element == GetTemplateChild("PART_MessageBoxSecondaryButton") && !IsSecondaryButtonEnabled
                )
                || (element == GetTemplateChild("PART_MessageBoxCloseButton") && !IsCloseButtonEnabled)
            )
            {
                return false;
            }

            return uiElement.Focusable && uiElement.IsEnabled && uiElement.IsVisible;
        }

        if (element is ContentElement contentElement && contentElement.Focusable && contentElement.IsEnabled)
        {
            DependencyObject? parent = contentElement;
            while (parent is ContentElement current)
            {
                parent = ContentOperations.GetParent(current) ?? (current as FrameworkContentElement)?.Parent;
            }

            return parent is UIElement { IsVisible: true, IsEnabled: true };
        }

        return false;
    }

    private static bool TrySetFocusToButton(Button? button, bool isEnabled)
    {
        return isEnabled
            && button is { IsEnabled: true, IsVisible: true, Focusable: true }
            && Keyboard.Focus(button) == button;
    }

    private void SetFocusToFirstAvailableButton()
    {
        // Never activate an inactive non-modal window or focus an unapplied template.
        if (!IsLoaded || !IsVisible || !IsActive || IsValidContentFocus(Keyboard.FocusedElement))
        {
            return;
        }

        IInputElement? logicalFocus = FocusManager.GetFocusedElement(this);
        if (!IsValidContentFocus(logicalFocus) && Content is DependencyObject content)
        {
            logicalFocus = FocusManager.GetFocusedElement(content);
        }

        if (IsValidContentFocus(logicalFocus))
        {
            _ = logicalFocus!.Focus();
            return;
        }

        Button? primaryButton = GetTemplateChild("PART_MessageBoxPrimaryButton") as Button;
        Button? secondaryButton = GetTemplateChild("PART_MessageBoxSecondaryButton") as Button;
        Button? closeButton = GetTemplateChild("PART_MessageBoxCloseButton") as Button;

        bool focusedDefault = DefaultFocusedButton switch
        {
            MessageBoxButton.Primary => TrySetFocusToButton(primaryButton, IsPrimaryButtonEnabled),
            MessageBoxButton.Secondary => TrySetFocusToButton(secondaryButton, IsSecondaryButtonEnabled),
            MessageBoxButton.Close => TrySetFocusToButton(closeButton, IsCloseButtonEnabled),
            _ => false,
        };

        if (focusedDefault || TrySetFocusToButton(primaryButton, IsPrimaryButtonEnabled))
        {
            return;
        }

        if (!TrySetFocusToButton(secondaryButton, IsSecondaryButtonEnabled))
        {
            _ = TrySetFocusToButton(closeButton, IsCloseButtonEnabled);
        }
    }
}
