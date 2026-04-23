// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

public class SnackbarPresenter : System.Windows.Controls.ContentPresenter
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "WpfAnalyzers.DependencyProperty",
        "WPF0012:CLR property type should match registered type",
        Justification = "seems harmless"
    )]
    public new Snackbar? Content
    {
        get => (Snackbar?)GetValue(ContentProperty);
        protected set => SetValue(ContentProperty, value);
    }

    public SnackbarPresenter()
    {
        Unloaded += static (sender, _) =>
        {
            var self = (SnackbarPresenter)sender;
            self.OnUnloaded();
        };
    }

    protected Queue<Snackbar> Queue { get; } = new();

    protected CancellationTokenSource CancellationTokenSource { get; set; } = new();

    protected virtual void OnUnloaded()
    {
        if (CancellationTokenSource.IsCancellationRequested)
        {
            return;
        }

        ImmediatelyHideCurrent();
        ResetCancellationTokenSource();
    }

    private void ImmediatelyHideCurrent()
    {
        if (Content is null)
        {
            return;
        }

        CancellationTokenSource.Cancel();
        ImmediatelyHidSnackbar(Content);
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "WpfAnalyzers.DependencyProperty",
        "WPF0041:Set mutable dependency properties using SetCurrentValue",
        Justification = "SetCurrentValue(ContentProperty, ...) will not work"
    )]
    private void ImmediatelyHidSnackbar(Snackbar snackbar)
    {
        snackbar.SetCurrentValue(Snackbar.IsShownProperty, false);

        Content = null;
    }

    protected void ResetCancellationTokenSource()
    {
        CancellationTokenSource.Dispose();
        CancellationTokenSource = new CancellationTokenSource();
    }

    public virtual void AddToQue(Snackbar snackbar)
    {
        Queue.Enqueue(snackbar);

        if (Content is null)
        {
            _ = ShowQueuedSnackbarsAsync(); // TODO: Fix detached process
        }
    }

    public virtual async Task ImmediatelyDisplay(Snackbar snackbar)
    {
        await HideCurrent();
        await ShowSnackbar(snackbar);
        await ShowQueuedSnackbarsAsync();
    }

    public virtual Task HideCurrent(CancellationToken token = default)
    {
        if (Content is null)
        {
            return Task.CompletedTask;
        }

        CancellationTokenSource.Cancel();

        return HideSnackbar(Content, delay: TimeSpan.Zero, resetSource: true, cancellationToken: token);
    }

    private async Task ShowQueuedSnackbarsAsync()
    {
        while (Queue.Count > 0 && !CancellationTokenSource.IsCancellationRequested)
        {
            Snackbar snackbar = Queue.Dequeue();

            await ShowSnackbar(snackbar);
        }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "WpfAnalyzers.DependencyProperty",
        "WPF0041:Set mutable dependency properties using SetCurrentValue",
        Justification = "SetCurrentValue(ContentProperty, ...) will not work"
    )]
    private Task ShowSnackbar(Snackbar snackbar)
    {
        Content = snackbar;

        snackbar.SetCurrentValue(Snackbar.IsShownProperty, true);

        return HideSnackbar(
            snackbarToHide: snackbar,
            delay: snackbar.Timeout,
            resetSource: false,
            cancellationToken: CancellationTokenSource.Token
        );
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "WpfAnalyzers.DependencyProperty",
        "WPF0041:Set mutable dependency properties using SetCurrentValue",
        Justification = "SetCurrentValue(ContentProperty, ...) will not work"
    )]
    private async Task HideSnackbar(
        Snackbar snackbarToHide,
        TimeSpan delay = default,
        bool resetSource = false,
        CancellationToken cancellationToken = default
    )
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return;
        }

        if (delay != TimeSpan.Zero)
        {
            try
            {
                await Task.Delay(delay, cancellationToken);
            }
            catch
            {
                return;
            }
        }

        snackbarToHide.SetCurrentValue(Snackbar.IsShownProperty, false);

        // NOTE: Post hide token, can we handle it better?
        await Task.Delay(300, cancellationToken);

        if (Content is IDisposable disposableContent)
        {
            disposableContent.Dispose();
        }

        Content = null;

        if (resetSource)
        {
            ResetCancellationTokenSource();
        }
    }

    ~SnackbarPresenter()
    {
        // TODO: Fe, fix
        if (!CancellationTokenSource.IsCancellationRequested)
        {
            CancellationTokenSource.Cancel();
        }

        CancellationTokenSource.Dispose();
    }
}
