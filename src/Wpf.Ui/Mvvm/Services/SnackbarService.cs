#nullable enable
using System.Threading.Tasks;
using Wpf.Ui.Common;
using Wpf.Ui.Controls.Interfaces;
using Wpf.Ui.Mvvm.Contracts;

namespace Wpf.Ui.Mvvm.Services;

/// <summary>
/// A service that provides methods related to displaying the <see cref="ISnackbarService"/>.
/// </summary>
public class SnackbarService : ISnackbarService
{
    private ISnackbarControl? _snackbar;

    /// <inheritdoc />
    public int Timeout
    {
        get => _snackbar?.Timeout ?? 0;
        set
        {
            if (_snackbar == null)
                return;

            _snackbar.Timeout = value;
        }
    }

    /// <inheritdoc />
    public void SetSnackbarControl(ISnackbarControl snackbar)
    {
        _snackbar = snackbar;
    }

    /// <inheritdoc />
    public bool Show()
    {
        if (_snackbar is null)
            return false;

        return _snackbar.Show();
    }

    /// <inheritdoc />
    public bool Show(string title)
    {
        if (_snackbar is null)
            return false;

        return _snackbar.Show(title);
    }

    /// <inheritdoc />
    public bool Show(string title, string message)
    {
        if (_snackbar is null)
            return false;

        return _snackbar.Show(title, message);
    }

    /// <inheritdoc />
    public bool Show(string title, string message, SymbolRegular icon)
    {
        if (_snackbar is null)
            return false;

        return _snackbar.Show(title, message, icon);
    }

    /// <inheritdoc />
    public async Task<bool> ShowAsync()
    {
        if (_snackbar is null)
            return false;

        return await _snackbar.ShowAsync();
    }

    /// <inheritdoc />
    public async Task<bool> ShowAsync(string title)
    {
        if (_snackbar is null)
            return false;

        return await _snackbar.ShowAsync(title);
    }

    /// <inheritdoc />
    public async Task<bool> ShowAsync(string title, string message)
    {
        if (_snackbar is null)
            return false;

        return await _snackbar.ShowAsync(title, message);
    }

    /// <inheritdoc />
    public async Task<bool> ShowAsync(string title, string message, SymbolRegular icon)
    {
        if (_snackbar is null)
            return false;

        return await _snackbar.ShowAsync(title, message, icon);
    }

    /// <inheritdoc />
    public bool Hide()
    {
        if (_snackbar is null)
            return false;

        return _snackbar.Hide();
    }

    /// <inheritdoc />
    public async Task<bool> HideAsync()
    {
        if (_snackbar is null)
            return false;

        return await _snackbar.HideAsync();
    }
}
