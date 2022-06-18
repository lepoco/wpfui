#nullable enable
using System.Threading.Tasks;
using System.Windows;

namespace Wpf.Ui.Controls.Interfaces;

public interface IDialogControl
{
    public enum ButtonPressed
    {
        None,
        Left,
        Right
    }

    string Title { get; set; }
    string Message { get; set; }

    string ButtonLeftName { get; set; }
    string ButtonRightName { get; set; }

    event RoutedEventHandler ButtonRightClick;
    event RoutedEventHandler ButtonLeftClick;

    Task<ButtonPressed> Show(string message, bool automaticHide = true);

    void Hide();
}
