namespace Wpf.Ui.Controls;

public class ListView : System.Windows.Controls.ListView
{
    public ListViewViewState ViewState
    {
        get => (ListViewViewState)GetValue(ViewStateProperty);
        set => SetValue(ViewStateProperty, value);
    }

    /// <summary>Identifies the <see cref="ViewState"/> dependency property.</summary>
    public static readonly DependencyProperty ViewStateProperty = DependencyProperty.Register(nameof(ViewState), typeof(ListViewViewState), typeof(ListView), new FrameworkPropertyMetadata(ListViewViewState.Default, OnViewStateChanged));

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
        ListViewViewState viewState = View switch
        {
            System.Windows.Controls.GridView => ListViewViewState.GridView,
            null => ListViewViewState.Default,
            _ => ListViewViewState.Default
        };
        SetCurrentValue(ViewStateProperty, viewState);
    }

    static ListView()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(ListView), new FrameworkPropertyMetadata(typeof(ListView)));
    }
}
