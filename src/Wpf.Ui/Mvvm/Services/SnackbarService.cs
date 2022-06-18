#nullable enable
using System.Threading.Tasks;
using Wpf.Ui.Common;
using Wpf.Ui.Controls;
using Wpf.Ui.Mvvm.Contracts;

namespace Wpf.Ui.Mvvm.Services;

public class SnackbarService : ISnackbarService
{
    private Snackbar? _snackbar;

    public void SetSnackbarControl(Snackbar snackbar)
    {
        _snackbar = snackbar;
    }

    public void ShowSnackbar(string title, string message) => ShowSnackbar(title, message, SymbolRegular.Empty);

    public void ShowSnackbar(string title, string message, SymbolRegular icon)
    {
        if (_snackbar is null) return;

        _snackbar.Icon = icon;
        _snackbar.Show(title, message);
    }

    public Task ShowSnackbarAsync(string title, string message) => ShowSnackbarAsync(title, message, SymbolRegular.Empty);

    public Task ShowSnackbarAsync(string title, string message, SymbolRegular icon)
    {
        if (_snackbar is null) return Task.CompletedTask;

        _snackbar.Icon = icon;
        return _snackbar.ShowAsync(title, message);
    }
}
