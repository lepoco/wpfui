using Wpf.Ui.Controls.Interfaces;

namespace Wpf.Ui.Mvvm.Contracts;

/// <summary>
/// Represents a contract with the service that provides global <see cref="IDialogControl"/>.
/// </summary>
public interface IDialogService
{
    /// <summary>
    /// Sets the <see cref="IDialogControl"/>
    /// </summary>
    /// <param name="dialog"></param>
    void SetDialogControl(IDialogControl dialog);

    /// <summary>
    /// Provides direct access to the <see cref="IDialogControl"/>
    /// </summary>
    /// <returns>Instance of the <see cref="IDialogControl"/> control.</returns>
    IDialogControl GetIDialogControl();
}
