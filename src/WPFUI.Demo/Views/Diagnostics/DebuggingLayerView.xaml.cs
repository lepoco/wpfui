using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPFUI.Demo.Views.Diagnostics;

public partial class DebuggingLayerView : UserControl
{
    public DebuggingLayerView()
    {
        InitializeComponent();
        Loaded += OnLoaded;
    }
    public Rect? FocusBounds
    {
        get => FocusIndicator.IsVisible
            ? new Rect(FocusIndicatorTranslateTransform.X, FocusIndicatorTranslateTransform.Y, FocusIndicator.ActualWidth, FocusIndicator.ActualHeight)
            : null;
        set
        {
            if (value is null)
            {
                FocusIndicator.Visibility = Visibility.Collapsed;
            }
            else
            {
                var bounds = value.Value;
                FocusIndicator.Visibility = Visibility.Visible;
                FocusIndicatorTranslateTransform.X = bounds.X;
                FocusIndicatorTranslateTransform.Y = bounds.Y;
                FocusIndicator.Width = bounds.Width;
                FocusIndicator.Height = bounds.Height;
            }
        }
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        var window = Window.GetWindow(this);
        if (window is not null)
        {
            window.GotKeyboardFocus += Window_GotKeyboardFocus;
            window.LostKeyboardFocus += Window_LostKeyboardFocus;
        }
    }

    private void Window_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
    {
        FocusBounds = null;
    }

    private void Window_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
    {
        if (Keyboard.FocusedElement is UIElement element
            && Window.GetWindow(element) is { } window
            && window == sender)
        {
            var topLeft = element.TranslatePoint(default, window);
            var bottomRight = element.TranslatePoint(new(element.RenderSize.Width, element.RenderSize.Height), window);
            FocusBounds = new(topLeft, bottomRight);
            FocusIndicatorTextBlock.Text = (element as FrameworkElement)?.Name is { } name
                ? (string.IsNullOrEmpty(name) ? element.GetType().Name : name)
                : element.GetType().Name;
        }
    }

    protected override AutomationPeer? OnCreateAutomationPeer()
    {
        return null;
    }
}
