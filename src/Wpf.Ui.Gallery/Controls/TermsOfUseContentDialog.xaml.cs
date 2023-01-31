using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Controls;

namespace Wpf.Ui.Gallery.Controls;

public partial class TermsOfUseContentDialog : ContentDialog
{
    public TermsOfUseContentDialog(ContentPresenter contentPresenter) : base(contentPresenter)
    {
        InitializeComponent();
    }

    protected override bool OnButtonClick(ContentDialogButton button)
    {
        if (CheckBox.IsChecked != false)
            return true;

        TextBlock.Visibility = Visibility.Visible;
        CheckBox.Focus();

        return false;
    }
}
