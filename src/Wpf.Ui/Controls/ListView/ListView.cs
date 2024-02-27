namespace Wpf.Ui.Controls;

public class ListView : System.Windows.Controls.ListView
{
    public string ViewState
    {
        get => (string)GetValue(ViewStateProperty);
        set => SetValue(ViewStateProperty, value);
    }

    /// <summary>Identifies the <see cref="ViewState"/> dependency property.</summary>
    public static readonly DependencyProperty ViewStateProperty = DependencyProperty.Register(nameof(ViewState), typeof(string), typeof(ListView), new FrameworkPropertyMetadata("Default", OnViewStateChanged));

    private static void OnViewStateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not ListView self)
        {
            return;
        }

        self.OnViewStateChanged(e);
    }

    protected virtual void OnViewStateChanged(DependencyPropertyChangedEventArgs e)
    {
        // derived classes can hook `ViewState` property changes by overriding this method
    }

    public ListView()
    {
        Loaded += OnLoaded;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        // immediately unsubscribe to prevent memory leaks
        Loaded -= OnLoaded;

        // get the descriptor for the `View` property since the framework doesn't provide a public hook for it
        var descriptor = DependencyPropertyDescriptor.FromProperty(System.Windows.Controls.ListView.ViewProperty, typeof(System.Windows.Controls.ListView));
        descriptor?.AddValueChanged(this, OnViewPropertyChanged);
        UpdateViewState(); // set the initial state
    }

    private void OnViewPropertyChanged(object? sender, EventArgs e)
    {
        UpdateViewState();
    }

    private void UpdateViewState()
    {
        var viewState = View is null ? "Default" : "GridView";
        SetCurrentValue(ViewStateProperty, viewState);
    }

    static ListView()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(ListView), new FrameworkPropertyMetadata(typeof(ListView)));
    }
}
