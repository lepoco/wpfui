#nullable enable
using System.Diagnostics;
using Wpf.Ui.Controls.Interfaces;
using Wpf.Ui.Mvvm.Contracts;

namespace Wpf.Ui.Mvvm.Services;
 
public class DialogService : IDialogService
{
    private IDialogControl? _dialogControl;

    public void SetDialogControl(IDialogControl dialog)
    {
        _dialogControl = dialog;
    }

    public IDialogControl GetIDialogControl()
    {
        Debug.Assert(_dialogControl != null, nameof(_dialogControl) + " != null");
        return _dialogControl;
    }
}
