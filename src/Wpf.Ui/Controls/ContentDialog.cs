using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Wpf.Ui.Common;

namespace Wpf.Ui.Controls;

public enum ContentDialogResult
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

public enum ContentDialogButton
{
    /// <summary>
    /// The primary button is the default.
    /// </summary>
    Primary,
    /// <summary>
    /// The secondary button is the default.
    /// </summary>
    Secondary,
    /// <summary>
    /// The close button is the default.
    /// </summary>
    Close
}

[TemplatePart(Name = ContentScrollKey, Type = typeof(FrameworkElement))]
public class ContentDialog : ContentControl, IDisposable
{
    private const string ContentScrollKey = "PART_ContentScroll";

    #region Static proerties

    public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(nameof(Title),
        typeof(object), typeof(ContentDialog), new PropertyMetadata(null));

    public static readonly DependencyProperty TitleTemplateProperty = DependencyProperty.Register(nameof(TitleTemplate),
        typeof(DataTemplate), typeof(ContentDialog), new PropertyMetadata(null));

    public static readonly DependencyProperty DialogWidthProperty =
        DependencyProperty.Register(nameof(DialogWidth),
            typeof(double), typeof(ContentDialog), new PropertyMetadata(double.NaN));

    public static readonly DependencyProperty DialogHeightProperty =
        DependencyProperty.Register(nameof(DialogHeight),
            typeof(double), typeof(ContentDialog), new PropertyMetadata(double.NaN));

    public static readonly DependencyProperty PrimaryButtonTextProperty =
        DependencyProperty.Register(nameof(PrimaryButtonText),
            typeof(string), typeof(ContentDialog), new PropertyMetadata(string.Empty));

    public static readonly DependencyProperty SecondaryButtonTextProperty =
        DependencyProperty.Register(nameof(SecondaryButtonText),
            typeof(string), typeof(ContentDialog), new PropertyMetadata(string.Empty));

    public static readonly DependencyProperty CloseButtonTextProperty =
        DependencyProperty.Register(nameof(CloseButtonText),
            typeof(string), typeof(ContentDialog), new PropertyMetadata("Close"));

    public static readonly DependencyProperty IsPrimaryButtonEnabledProperty =
        DependencyProperty.Register(nameof(IsPrimaryButtonEnabled),
            typeof(bool), typeof(ContentDialog), new PropertyMetadata(true));

    public static readonly DependencyProperty IsSecondaryButtonEnabledProperty =
        DependencyProperty.Register(nameof(IsSecondaryButtonEnabled),
            typeof(bool), typeof(ContentDialog), new PropertyMetadata(true));

    public static readonly DependencyProperty PrimaryButtonAppearanceProperty =
        DependencyProperty.Register(nameof(PrimaryButtonAppearance),
            typeof(ControlAppearance), typeof(ContentDialog), new PropertyMetadata(ControlAppearance.Primary));

    public static readonly DependencyProperty SecondaryButtonAppearanceProperty =
        DependencyProperty.Register(nameof(SecondaryButtonAppearance),
            typeof(ControlAppearance), typeof(ContentDialog), new PropertyMetadata(ControlAppearance.Secondary));

    public static readonly DependencyProperty CloseButtonAppearanceProperty =
        DependencyProperty.Register(nameof(CloseButtonAppearance),
            typeof(ControlAppearance), typeof(ContentDialog), new PropertyMetadata(ControlAppearance.Secondary));

    /// <summary>
    /// Property for <see cref="TemplateButtonCommand"/>.
    /// </summary>
    public static readonly DependencyProperty TemplateButtonCommandProperty =
        DependencyProperty.Register(nameof(TemplateButtonCommand),
            typeof(IRelayCommand), typeof(ContentDialog), new PropertyMetadata(null));


    public static readonly DependencyProperty DefaultButtonProperty =
        DependencyProperty.Register(nameof(DefaultButton),
            typeof(ContentDialogButton), typeof(ContentDialog), new PropertyMetadata(ContentDialogButton.Primary));

    public static readonly DependencyProperty IsFooterVisibleProperty =
        DependencyProperty.Register(nameof(IsFooterVisible),
            typeof(bool), typeof(ContentDialog), new PropertyMetadata(true));

    #endregion

    #region Properties

    public object Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public DataTemplate TitleTemplate
    {
        get => (DataTemplate) GetValue(TitleTemplateProperty);
        set => SetValue(TitleTemplateProperty, value);
    }

    public double DialogWidth
    {
        get => (int)GetValue(DialogWidthProperty);
        set => SetValue(DialogWidthProperty, value);
    }

    public double DialogHeight
    {
        get => (int)GetValue(DialogHeightProperty);
        set => SetValue(DialogHeightProperty, value);
    }

    public string PrimaryButtonText
    {
        get => (string)GetValue(PrimaryButtonTextProperty);
        set => SetValue(PrimaryButtonTextProperty, value);
    }

    public string SecondaryButtonText
    {
        get => (string)GetValue(SecondaryButtonTextProperty);
        set => SetValue(SecondaryButtonTextProperty, value);
    }

    public string CloseButtonText
    {
        get => (string)GetValue(CloseButtonTextProperty);
        set => SetValue(CloseButtonTextProperty, value);
    }

    public bool IsPrimaryButtonEnabled
    {
        get => (bool)GetValue(IsPrimaryButtonEnabledProperty);
        set => SetValue(IsPrimaryButtonEnabledProperty, value);
    }

    public bool IsSecondaryButtonEnabled
    {
        get => (bool)GetValue(IsSecondaryButtonEnabledProperty);
        set => SetValue(IsSecondaryButtonEnabledProperty, value);
    }

    public ControlAppearance PrimaryButtonAppearance
    {
        get => (ControlAppearance)GetValue(PrimaryButtonAppearanceProperty);
        set => SetValue(PrimaryButtonAppearanceProperty, value);
    }

    public ControlAppearance SecondaryButtonAppearance
    {
        get => (ControlAppearance)GetValue(SecondaryButtonAppearanceProperty);
        set => SetValue(SecondaryButtonAppearanceProperty, value);
    }

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

    public bool IsFooterVisible
    {
        get => (bool)GetValue(IsFooterVisibleProperty);
        set => SetValue(IsFooterVisibleProperty, value);
    }

    /// <summary>
    /// Command triggered after clicking the button in the template.
    /// </summary>
    public IRelayCommand TemplateButtonCommand => (IRelayCommand)GetValue(TemplateButtonCommandProperty);

    #endregion

    public ContentDialog(ContentPresenter contentPresenter)
    {
        _contentPresenter = contentPresenter;

        SetValue(TemplateButtonCommandProperty,
            new RelayCommand<ContentDialogButton>(OnTemplateButtonClick));

        Loaded += OnLoaded;
        Unloaded += OnUnloaded;
    }

    private readonly ContentPresenter _contentPresenter;
    private TaskCompletionSource<ContentDialogResult>? _tcs;

    /// <summary>
    /// Shows the dialog
    /// </summary>
    /// <returns><see cref="ContentDialogResult"/></returns>
    /// <exception cref="TaskCanceledException"></exception>
    public async Task<ContentDialogResult> ShowAsync(CancellationToken cancellationToken = default)
    {
        _tcs = new TaskCompletionSource<ContentDialogResult>();
        var tokenRegistration = cancellationToken.Register(o => _tcs.TrySetCanceled((CancellationToken)o), cancellationToken);

        try
        {
            _contentPresenter.Content = this;
            return await _tcs.Task;
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
    /// Hides the dialog manually.
    /// </summary>
    public void Hide()
    {
        _contentPresenter.Content = null;
    }

    public void Dispose() => Hide();

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        var scroll = (FrameworkElement)GetTemplateChild(ContentScrollKey)!;
        scroll.Focus();
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        if (VisualChildrenCount <= 0)
            return;

        if (GetVisualChild(0) is not FrameworkElement frameworkElement)
            return;

        //left and right margin
        const double margin = 24.0 * 2;

        //Adding margin in order for the text to fit in all buttons
        DialogWidth = frameworkElement.DesiredSize.Width + margin;
        DialogHeight = frameworkElement.DesiredSize.Height;
    }

    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
        Loaded -= OnLoaded;
        Unloaded -= OnUnloaded;
    }

    private void OnTemplateButtonClick(ContentDialogButton button)
    {
        ContentDialogResult result = button switch
        {
            ContentDialogButton.Primary => ContentDialogResult.Primary,
            ContentDialogButton.Secondary => ContentDialogResult.Secondary,
            _ => ContentDialogResult.None
        };

        _tcs?.TrySetResult(result);
        Hide();
    }
}
