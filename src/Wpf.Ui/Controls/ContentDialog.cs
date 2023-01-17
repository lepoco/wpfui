using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Common;

namespace Wpf.Ui.Controls;

[TemplatePart(Name = ContentScrollKey, Type = typeof(FrameworkElement))]
public class ContentDialog : ContentControl, IDisposable
{
    public enum Result
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

    public enum Button
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

    private const string ContentScrollKey = "PART_ContentScroll";

    #region Static proerties

    public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(nameof(Title),
        typeof(object), typeof(ContentDialog), new PropertyMetadata(null));

    public static readonly DependencyProperty TitleTemplateProperty = DependencyProperty.Register(nameof(TitleTemplate),
        typeof(DataTemplate), typeof(ContentDialog), new PropertyMetadata(null));

    public static readonly DependencyProperty DialogWidthProperty =
        DependencyProperty.Register(nameof(DialogWidth),
            typeof(double), typeof(ContentDialog), new PropertyMetadata(420.0));

    public static readonly DependencyProperty DialogHeightProperty =
        DependencyProperty.Register(nameof(DialogHeight),
            typeof(double), typeof(ContentDialog), new PropertyMetadata(260.0));

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
            typeof(Button), typeof(ContentDialog), new PropertyMetadata(Button.Primary));

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
    public Button DefaultButton
    {
        get => (Button)GetValue(DefaultButtonProperty);
        set => SetValue(DefaultButtonProperty, value);
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
            new RelayCommand<string>(o => OnTemplateButtonClick(o ?? string.Empty)));
    }

    private readonly ContentPresenter _contentPresenter;
    private TaskCompletionSource<Result>? _tcs;

    /// <summary>
    /// Shows the dialog
    /// </summary>
    /// <returns></returns>
    public Task<Result> ShowAsync()
    {
        _tcs = new TaskCompletionSource<Result>();
        _contentPresenter.Content = this;

        return _tcs.Task;
    }

    /// <summary>
    /// Hides the dialog.
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

    private void OnTemplateButtonClick(string buttonName)
    {
        Result result = buttonName switch
        {
            "primary" => Result.Primary,
            "secondary" => Result.Secondary,
            "close" => Result.None,
            _ => Result.None
        };

        _tcs?.TrySetResult(result);
    }
}
