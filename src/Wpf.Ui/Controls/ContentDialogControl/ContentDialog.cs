// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Common;
using Wpf.Ui.Controls.IconElements;

namespace Wpf.Ui.Controls.ContentDialogControl;

public class ContentDialog : ContentControl
{
    #region Static proerties

    /// <summary>
    /// Property for <see cref="Title"/>.
    /// </summary>
    public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(nameof(Title),
        typeof(object), typeof(ContentDialog), new PropertyMetadata(null));

    /// <summary>
    /// Property for <see cref="TitleTemplate"/>.
    /// </summary>
    public static readonly DependencyProperty TitleTemplateProperty = DependencyProperty.Register(nameof(TitleTemplate),
        typeof(DataTemplate), typeof(ContentDialog), new PropertyMetadata(null));

    /// <summary>
    /// Property for <see cref="DialogWidth"/>.
    /// </summary>
    public static readonly DependencyProperty DialogWidthProperty =
        DependencyProperty.Register(nameof(DialogWidth),
            typeof(double), typeof(ContentDialog), new PropertyMetadata(double.PositiveInfinity));

    /// <summary>
    /// Property for <see cref="DialogHeight"/>.
    /// </summary>
    public static readonly DependencyProperty DialogHeightProperty =
        DependencyProperty.Register(nameof(DialogHeight),
            typeof(double), typeof(ContentDialog), new PropertyMetadata(double.PositiveInfinity));

    /// <summary>
    /// Property for <see cref="DialogMaxWidth"/>.
    /// </summary>
    public static readonly DependencyProperty DialogMaxWidthProperty =
        DependencyProperty.Register(nameof(DialogMaxWidth),
            typeof(double), typeof(ContentDialog), new PropertyMetadata(double.PositiveInfinity));

    /// <summary>
    /// Property for <see cref="DialogMaxHeight"/>.
    /// </summary>
    public static readonly DependencyProperty DialogMaxHeightProperty =
        DependencyProperty.Register(nameof(DialogMaxHeight),
            typeof(double), typeof(ContentDialog), new PropertyMetadata(double.PositiveInfinity));

    /// <summary>
    /// Property for <see cref="DialogMargin"/>.
    /// </summary>
    public static readonly DependencyProperty DialogMarginProperty =
        DependencyProperty.Register(nameof(DialogMargin),
            typeof(Thickness), typeof(ContentDialog));

    /// <summary>
    /// Property for <see cref="PrimaryButtonText"/>.
    /// </summary>
    public static readonly DependencyProperty PrimaryButtonTextProperty =
        DependencyProperty.Register(nameof(PrimaryButtonText),
            typeof(string), typeof(ContentDialog), new PropertyMetadata(string.Empty));

    /// <summary>
    /// Property for <see cref="SecondaryButtonText"/>.
    /// </summary>
    public static readonly DependencyProperty SecondaryButtonTextProperty =
        DependencyProperty.Register(nameof(SecondaryButtonText),
            typeof(string), typeof(ContentDialog), new PropertyMetadata(string.Empty));

    /// <summary>
    /// Property for <see cref="CloseButtonText"/>.
    /// </summary>
    public static readonly DependencyProperty CloseButtonTextProperty =
        DependencyProperty.Register(nameof(CloseButtonText),
            typeof(string), typeof(ContentDialog), new PropertyMetadata("Close"));

    /// <summary>
    /// Property for <see cref="PrimaryButtonIcon"/>.
    /// </summary>
    public static readonly DependencyProperty PrimaryButtonIconProperty =
        DependencyProperty.Register(nameof(PrimaryButtonIcon),
            typeof(IconElement), typeof(ContentDialog), new PropertyMetadata(null));

    /// <summary>
    /// Property for <see cref="SecondaryButtonIcon"/>.
    /// </summary>
    public static readonly DependencyProperty SecondaryButtonIconProperty =
        DependencyProperty.Register(nameof(SecondaryButtonIcon),
            typeof(IconElement), typeof(ContentDialog), new PropertyMetadata(null));

    /// <summary>
    /// Property for <see cref="CloseButtonIcon"/>.
    /// </summary>
    public static readonly DependencyProperty CloseButtonIconProperty =
        DependencyProperty.Register(nameof(CloseButtonIcon),
            typeof(IconElement), typeof(ContentDialog), new PropertyMetadata(null));

    /// <summary>
    /// Property for <see cref="IsPrimaryButtonEnabled"/>.
    /// </summary>
    public static readonly DependencyProperty IsPrimaryButtonEnabledProperty =
        DependencyProperty.Register(nameof(IsPrimaryButtonEnabled),
            typeof(bool), typeof(ContentDialog), new PropertyMetadata(true));

    /// <summary>
    /// Property for <see cref="IsSecondaryButtonEnabled"/>.
    /// </summary>
    public static readonly DependencyProperty IsSecondaryButtonEnabledProperty =
        DependencyProperty.Register(nameof(IsSecondaryButtonEnabled),
            typeof(bool), typeof(ContentDialog), new PropertyMetadata(true));

    /// <summary>
    /// Property for <see cref="PrimaryButtonAppearance"/>.
    /// </summary>
    public static readonly DependencyProperty PrimaryButtonAppearanceProperty =
        DependencyProperty.Register(nameof(PrimaryButtonAppearance),
            typeof(ControlAppearance), typeof(ContentDialog), new PropertyMetadata(ControlAppearance.Primary));

    /// <summary>
    /// Property for <see cref="SecondaryButtonAppearance"/>.
    /// </summary>
    public static readonly DependencyProperty SecondaryButtonAppearanceProperty =
        DependencyProperty.Register(nameof(SecondaryButtonAppearance),
            typeof(ControlAppearance), typeof(ContentDialog), new PropertyMetadata(ControlAppearance.Secondary));

    /// <summary>
    /// Property for <see cref="CloseButtonAppearance"/>.
    /// </summary>
    public static readonly DependencyProperty CloseButtonAppearanceProperty =
        DependencyProperty.Register(nameof(CloseButtonAppearance),
            typeof(ControlAppearance), typeof(ContentDialog), new PropertyMetadata(ControlAppearance.Secondary));

    /// <summary>
    /// Property for <see cref="DefaultButton"/>.
    /// </summary>
    public static readonly DependencyProperty DefaultButtonProperty =
        DependencyProperty.Register(nameof(DefaultButton),
            typeof(ContentDialogButton), typeof(ContentDialog), new PropertyMetadata(ContentDialogButton.Primary));

    /// <summary>
    /// Property for <see cref="IsFooterVisible"/>.
    /// </summary>
    public static readonly DependencyProperty IsFooterVisibleProperty =
        DependencyProperty.Register(nameof(IsFooterVisible),
            typeof(bool), typeof(ContentDialog), new PropertyMetadata(true));

    /// <summary>
    /// Property for <see cref="TemplateButtonCommand"/>.
    /// </summary>
    public static readonly DependencyProperty TemplateButtonCommandProperty =
        DependencyProperty.Register(nameof(TemplateButtonCommand),
            typeof(IRelayCommand), typeof(ContentDialog), new PropertyMetadata(null));


    /// <summary>
    /// Property for <see cref="Opened"/>.
    /// </summary>
    public static readonly RoutedEvent OpenedEvent = EventManager.RegisterRoutedEvent(nameof(Opened),
        RoutingStrategy.Bubble, typeof(TypedEventHandler<ContentDialog, RoutedEventArgs>), typeof(ContentDialog));

    /// <summary>
    /// Property for <see cref="Closing"/>.
    /// </summary>
    public static readonly RoutedEvent ClosingEvent = EventManager.RegisterRoutedEvent(nameof(Closing),
        RoutingStrategy.Bubble, typeof(TypedEventHandler<ContentDialog, ContentDialogClosingEventArgs>), typeof(ContentDialog));

    /// <summary>
    /// Property for <see cref="Closed"/>.
    /// </summary>
    public static readonly RoutedEvent ClosedEvent = EventManager.RegisterRoutedEvent(nameof(Closed),
        RoutingStrategy.Bubble, typeof(TypedEventHandler<ContentDialog, ContentDialogClosedEventArgs>), typeof(ContentDialog));

    /// <summary>
    /// Property for <see cref="ButtonClicked"/>.
    /// </summary>
    public static readonly RoutedEvent ButtonClickedEvent = EventManager.RegisterRoutedEvent(nameof(ButtonClicked),
        RoutingStrategy.Bubble, typeof(TypedEventHandler<ContentDialog, ContentDialogButtonClickEventArgs>), typeof(ContentDialog));

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets the title of the <see cref="ContentDialog"/>.
    /// </summary>
    public object Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    /// <summary>
    /// Gets or sets the title template of the <see cref="ContentDialog"/>.
    /// </summary>
    public DataTemplate TitleTemplate
    {
        get => (DataTemplate) GetValue(TitleTemplateProperty);
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
        get => (IconElement)GetValue(PrimaryButtonIconProperty);
        set => SetValue(PrimaryButtonIconProperty, value);
    }

    /// <summary>
    /// Gets or sets the <see cref="SymbolRegular"/> on the primary button.
    /// </summary>
    public IconElement? SecondaryButtonIcon
    {
        get => (IconElement)GetValue(SecondaryButtonIconProperty);
        set => SetValue(SecondaryButtonIconProperty, value);
    }

    /// <summary>
    /// Gets or sets the <see cref="SymbolRegular"/> on the close button.
    /// </summary>
    public IconElement? CloseButtonIcon
    {
        get => (IconElement)GetValue(CloseButtonIconProperty);
        set => SetValue(CloseButtonIconProperty, value);
    }

    /// <summary>
    /// Gets or sets whether the <see cref="ContentDialog"/> primary button is enabled.
    /// </summary>
    public bool IsPrimaryButtonEnabled
    {
        get => (bool)GetValue(IsPrimaryButtonEnabledProperty);
        set => SetValue(IsPrimaryButtonEnabledProperty, value);
    }

    /// <summary>
    /// Gets or sets whether the <see cref="ContentDialog"/> secondary button is enabled.
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
    /// Gets or sets a value that indicates the visibility of the footer buttons.
    /// </summary>
    public bool IsFooterVisible
    {
        get => (bool)GetValue(IsFooterVisibleProperty);
        set => SetValue(IsFooterVisibleProperty, value);
    }

    /// <summary>
    /// Command triggered after clicking the button in the template.
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

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="ContentDialog"/> class.
    /// </summary>
    /// <param name="contentPresenter"></param>
    public ContentDialog(ContentPresenter contentPresenter)
    {
        ContentPresenter = contentPresenter;

        SetValue(TemplateButtonCommandProperty, new RelayCommand<ContentDialogButton>(OnButtonClick));

        Loaded += static (sender, _) =>
        {
            var self = (ContentDialog)sender;
            self.OnLoaded();
        };
    }

    protected readonly ContentPresenter ContentPresenter;
    protected TaskCompletionSource<ContentDialogResult>? Tcs;

    #region Public methos

    /// <summary>
    /// Shows the dialog
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns><see cref="ContentDialogResult"/></returns>
    /// <exception cref="TaskCanceledException"></exception>
    public async Task<ContentDialogResult> ShowAsync(CancellationToken cancellationToken = default)
    {
        Tcs = new TaskCompletionSource<ContentDialogResult>();
        CancellationTokenRegistration tokenRegistration = cancellationToken.Register(o => Tcs.TrySetCanceled((CancellationToken)o!), cancellationToken);

        ContentDialogResult result = ContentDialogResult.None;

        try
        {
            ContentPresenter.Content = this;
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
            ContentPresenter.Content = null;
            OnClosed(result);
        }
    }

    /// <summary>
    /// Hides the dialog with result
    /// </summary>
    public virtual void Hide(ContentDialogResult result = ContentDialogResult.None)
    {
        ContentDialogClosingEventArgs closingEventArgs = new ContentDialogClosingEventArgs(ClosingEvent, this)
        {
            Result = result
        };

        RaiseEvent(closingEventArgs);

        if (!closingEventArgs.Cancel)
            Tcs?.TrySetResult(result);
    }

    #endregion

    #region Protected methods

    /// <summary>
    /// Occurs after ContentPresenter.Content = null
    /// </summary>
    protected virtual void OnClosed(ContentDialogResult result)
    {
        ContentDialogClosedEventArgs closedEventArgs =
            new ContentDialogClosedEventArgs(ClosingEvent, this) { Result = result };

        RaiseEvent(closedEventArgs);
    }

    /// <summary>
    /// Occurs after the <see cref="ContentDialogButton"/> is clicked 
    /// </summary>
    /// <param name="button"></param>
    protected virtual void OnButtonClick(ContentDialogButton button)
    {
        ContentDialogButtonClickEventArgs buttonClickEventArgs =
            new ContentDialogButtonClickEventArgs(ButtonClickedEvent, this) { Button = button };

        RaiseEvent(buttonClickEventArgs);

        ContentDialogResult result = button switch
        {
            ContentDialogButton.Primary => ContentDialogResult.Primary,
            ContentDialogButton.Secondary => ContentDialogResult.Secondary,
            _ => ContentDialogResult.None
        };

        Hide(result);
    }

    #endregion

    #region Base methods

    protected override Size MeasureOverride(Size availableSize)
    {
        var rootElement = (UIElement) GetVisualChild(0)!;

        rootElement.Measure(availableSize);
        Size desiredSize = rootElement.DesiredSize;

        Size newSize = GetNewDialogSize(desiredSize);

        DialogHeight = newSize.Height;
        DialogWidth = newSize.Width;

        ResizeWidth(rootElement);
        ResizeHeight(rootElement);

        return desiredSize;
    }

    /// <summary>
    /// Occurs after Loaded event
    /// </summary>
    protected virtual void OnLoaded()
    {
        Focus();

        RaiseEvent(new RoutedEventArgs(OpenedEvent));
    }

    #endregion

    #region Resize private methods

    private Size GetNewDialogSize(Size desiredSize)
    {
        var paddingWidth = Padding.Left + Padding.Right;

        var marginHeight = DialogMargin.Bottom + DialogMargin.Top;
        var marginWidth = DialogMargin.Left + DialogMargin.Right;

        var width = desiredSize.Width - marginWidth + paddingWidth;
        var height = desiredSize.Height - marginHeight;

        return new Size(width, height);
    }

    private void ResizeWidth(UIElement element)
    {
        if (DialogWidth <= DialogMaxWidth)
            return;

        DialogWidth = DialogMaxWidth;
        element.UpdateLayout();

        DialogHeight = element.DesiredSize.Height;

        if (DialogHeight > DialogMaxHeight)
        {
            DialogMaxHeight = DialogHeight;
            //Debug.WriteLine($"DEBUG | {GetType()} | WARNING | DialogHeight > DialogMaxHeight after resizing width!");
        }
    }

    private void ResizeHeight(UIElement element)
    {
        if (DialogHeight <= DialogMaxHeight)
            return;

        DialogHeight = DialogMaxHeight;
        element.UpdateLayout();

        DialogWidth = element.DesiredSize.Width;

        if (DialogWidth > DialogMaxWidth)
        {
            DialogMaxWidth = DialogWidth;
            //Debug.WriteLine($"DEBUG | {GetType()} | WARNING | DialogWidth > DialogMaxWidth after resizing height!");
        }
    }

    #endregion
}
