// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Linq;
using System.Reflection;
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
            var self = (MessageBox)sender;
            self.OnLoaded();
        };

        ContentRendered += static (sender, _) =>
        {
            var self = (MessageBox)sender;
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

            // If Owner is not set, try to set it to the active window
            if (Owner == null)
            {
                var activeWindow = System.Windows.Application.Current?.Windows
                    .OfType<System.Windows.Window>()
                    .FirstOrDefault(w => w.IsActive);
                
                if (activeWindow != null)
                {
                    Owner = activeWindow;
                }
            }

            // Set WindowStartupLocation to CenterOwner if not explicitly set
            if (WindowStartupLocation == WindowStartupLocation.Manual)
            {
                WindowStartupLocation = Owner != null 
                    ? WindowStartupLocation.CenterOwner 
                    : WindowStartupLocation.CenterScreen;
            }

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

        // Set focus to the first available button after the visual tree is fully loaded
        // Loaded event provides the appropriate timing for setting focus, so we call it directly
        // without delay to avoid interfering with other developers' focus logic
        // Skip IsFocused check at this point as the window may not have focus yet
        SetFocusToFirstAvailableButton(checkIsFocused: false);
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
        
        // After template is applied, try to set focus
        // OnLoaded will also try, but this ensures we try as early as possible
        // Skip IsFocused check at this point as the window may not have focus yet
        SetFocusToFirstAvailableButton(checkIsFocused: false);
    }

    /// <summary>
    /// Occurs after ContentRendered event
    /// </summary>
    protected virtual void OnContentRendered()
    {
        // Set focus after content is rendered
        // This ensures the window is fully displayed before setting focus
        // At this point, check IsFocused to avoid overriding developer's focus logic
        SetFocusToFirstAvailableButton(checkIsFocused: true);
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

    protected virtual void CenterWindowOnScreen()
    {
        double screenWidth = SystemParameters.PrimaryScreenWidth;
        double screenHeight = SystemParameters.PrimaryScreenHeight;

        SetCurrentValue(LeftProperty, (screenWidth / 2) - (Width / 2));
        SetCurrentValue(TopProperty, (screenHeight / 2) - (Height / 2));
    }

    private void CenterWindowOnOwner()
    {
        double left = Owner.Left + ((Owner.Width - Width) / 2);
        double top = Owner.Top + ((Owner.Height - Height) / 2);

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

    /// <summary>
    /// Checks if a button is available and enabled for focus.
    /// </summary>
    private static bool IsButtonAvailableForFocus(Button? button, bool isEnabled)
    {
        return button != null 
            && isEnabled 
            && button.IsEnabled 
            && button.IsVisible 
            && button.Focusable;
    }

    /// <summary>
    /// Attempts to set focus to the specified button using multiple methods.
    /// </summary>
    private static bool TrySetFocusToButton(Button button)
    {
        // Method 1: Keyboard.Focus (most reliable for modal dialogs)
        if (System.Windows.Input.Keyboard.Focus(button) == button)
        {
            return true;
        }
        
        // Method 2: Focus() method
        if (button.Focus())
        {
            return true;
        }
        
        // Method 3: MoveFocus
        var request = new System.Windows.Input.TraversalRequest(System.Windows.Input.FocusNavigationDirection.First);
        return button.MoveFocus(request);
    }

    /// <summary>
    /// Sets focus to the first available button in the MessageBox.
    /// Focus will be set to the first available button (Primary > Secondary > Close).
    /// </summary>
    /// <param name="checkIsFocused">If true, check IsFocused before setting focus. If false, skip the check.</param>
    /// <returns>True if focus was successfully set, false otherwise.</returns>
    private bool SetFocusToFirstAvailableButton(bool checkIsFocused = true)
    {
        // Get buttons directly from template using GetTemplateChild for efficiency
        Button? primaryButton = GetTemplateChild("PART_MessageBoxPrimaryButton") as Button;
        Button? secondaryButton = GetTemplateChild("PART_MessageBoxSecondaryButton") as Button;
        Button? closeButton = GetTemplateChild("PART_MessageBoxCloseButton") as Button;

        // If template is not applied yet, buttons will be null
        if (primaryButton == null && secondaryButton == null && closeButton == null)
        {
            return false;
        }

        // Check if focus is already set to a button or to a non-button control
        // If checkIsFocused is true, we need to verify that:
        // 1. Focus is not already on any button (if so, don't override)
        // 2. Focus is not on a non-button control (e.g., TextBox) - developer's focus logic is active
        // 3. If focus is on MessageBox itself or nowhere, we should set focus to primaryButton
        if (checkIsFocused)
        {
            var currentFocusedElement = System.Windows.Input.Keyboard.FocusedElement;
            
            // Check if focus is already on any button
            if (currentFocusedElement == primaryButton || 
                currentFocusedElement == secondaryButton || 
                currentFocusedElement == closeButton)
            {
                // Focus is already on a button, don't override
                return false;
            }
            
            // Check if focus is on a non-button control within MessageBox
            // If IsFocused is false and currentFocusedElement is not the MessageBox itself,
            // it means an internal control (e.g., TextBox) has focus
            if (!IsFocused && currentFocusedElement != this)
            {
                // An internal control (not the MessageBox itself) has focus, avoid overriding
                return false;
            }
            
            // If we reach here, focus is either on MessageBox itself or nowhere
            // In this case, we should set focus to primaryButton
        }

        // Ensure the window is active and can receive focus
        if (!IsActive)
        {
            Activate();
        }

        // Update layout once for the window
        UpdateLayout();

        // If DefaultFocusedButton is specified, try to set focus to that button
        if (DefaultFocusedButton.HasValue)
        {
            Button? targetButton = DefaultFocusedButton.Value switch
            {
                MessageBoxButton.Primary => primaryButton,
                MessageBoxButton.Secondary => secondaryButton,
                MessageBoxButton.Close => closeButton,
                _ => null,
            };

            bool isButtonEnabled = DefaultFocusedButton.Value switch
            {
                MessageBoxButton.Primary => IsPrimaryButtonEnabled,
                MessageBoxButton.Secondary => IsSecondaryButtonEnabled,
                MessageBoxButton.Close => IsCloseButtonEnabled,
                _ => false,
            };

            if (IsButtonAvailableForFocus(targetButton, isButtonEnabled))
            {
                return TrySetFocusToButton(targetButton!);
            }
        }

        // Fallback to automatic selection: Set focus to the first available button
        if (IsButtonAvailableForFocus(primaryButton, IsPrimaryButtonEnabled))
        {
            return TrySetFocusToButton(primaryButton!);
        }
        
        if (IsButtonAvailableForFocus(secondaryButton, IsSecondaryButtonEnabled))
        {
            return TrySetFocusToButton(secondaryButton!);
        }
        
        if (IsButtonAvailableForFocus(closeButton, IsCloseButtonEnabled))
        {
            return TrySetFocusToButton(closeButton!);
        }

        return false;
    }
}
