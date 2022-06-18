using Wpf.Ui.Controls.Interfaces;

namespace Wpf.Ui.Mvvm.Contracts;

public interface IDialogService
{
    void SetDialogControl(IDialogControl dialog);
    IDialogControl GetIDialogControl();
}
