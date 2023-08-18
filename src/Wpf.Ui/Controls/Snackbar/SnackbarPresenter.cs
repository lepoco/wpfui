// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

public class SnackbarPresenter : System.Windows.Controls.ContentPresenter
{
    public new Snackbar? Content
    {
        get => (Snackbar)GetValue(ContentProperty);
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

    protected readonly Queue<Snackbar> Queue = new();

    protected CancellationTokenSource CancellationTokenSource = new();

    protected virtual void OnUnloaded()
    {
        CancellationTokenSource.Cancel();
        CancellationTokenSource.Dispose();
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
            ShowQueuedSnackbars(); // TODO: Fix detached process
        }
    }

    public virtual async Task ImmediatelyDisplay(Snackbar snackbar)
    {
        await HideCurrent();
        await ShowSnackbar(snackbar);

        await ShowQueuedSnackbars();
    }

    public virtual async Task HideCurrent()
    {
        if (Content is null)
        {
            return;
        }

        CancellationTokenSource.Cancel();
        await HidSnackbar(Content);
        ResetCancellationTokenSource();
    }

    private async Task ShowQueuedSnackbars()
    {
        while (Queue.Count > 0 && !CancellationTokenSource.IsCancellationRequested)
        {
            Snackbar snackbar = Queue.Dequeue();

            await ShowSnackbar(snackbar);
        }
    }

    private async Task ShowSnackbar(Snackbar snackbar)
    {
        Content = snackbar;

        snackbar.IsShown = true;

        try
        {
            await Task.Delay(snackbar.Timeout, CancellationTokenSource.Token);
        }
        catch
        {
            return;
        }

        await HidSnackbar(snackbar);
    }

    private async Task HidSnackbar(Snackbar snackbar)
    {
        snackbar.IsShown = false;

        await Task.Delay(300);

        Content = null;
    }
}
