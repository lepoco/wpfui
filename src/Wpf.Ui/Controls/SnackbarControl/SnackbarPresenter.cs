// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Wpf.Ui.Controls.SnackbarControl;

public class SnackbarPresenter : System.Windows.Controls.ContentPresenter
{
    public new NewSnackbar? Content
    {
        get => (NewSnackbar)GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
    }

    public SnackbarPresenter()
    {
        Unloaded += static (sender, _) =>
        {
            var self = (SnackbarPresenter) sender;
            self.OnUnloaded();
        };
    }

    protected readonly Queue<NewSnackbar> _queue = new();
    protected CancellationTokenSource _cancellationTokenSource = new();

    protected virtual void OnUnloaded()
    {
        _cancellationTokenSource.Cancel();
        _cancellationTokenSource.Dispose();
    }

    protected void ResetCancellationTokenSource()
    {
        _cancellationTokenSource.Dispose();
        _cancellationTokenSource = new CancellationTokenSource();
    }

    public virtual void AddToQue(NewSnackbar newSnackbar)
    {
        _queue.Enqueue(newSnackbar);

        if (Content is null)
            ShowQueuedSnackbars();
    }

    public virtual async void ImmediatelyDisplay(NewSnackbar snackbar)
    {
        await HideCurrent();
        await ShowSnackbar(snackbar);

        ShowQueuedSnackbars();
    }

    public virtual async Task HideCurrent()
    {
        if (Content is null)
            return;

        _cancellationTokenSource.Cancel();
        await HidSnackbar(Content);
        ResetCancellationTokenSource();
    }

    private async void ShowQueuedSnackbars()
    {
        while (_queue.Count > 0 && !_cancellationTokenSource.IsCancellationRequested)
        {
            var snackbar = _queue.Dequeue();
            await ShowSnackbar(snackbar);
        }
    }

    private async Task ShowSnackbar(NewSnackbar snackbar)
    {
        Content = snackbar;
        snackbar.IsShown = true;

        try
        {
            await Task.Delay(snackbar.Timeout, _cancellationTokenSource.Token);
        }
        catch
        {
            return;
        }

        await HidSnackbar(snackbar);
    }

    private async Task HidSnackbar(NewSnackbar snackbar)
    {
        snackbar.IsShown = false;
        await Task.Delay(300);
        Content = null;
    }
}
