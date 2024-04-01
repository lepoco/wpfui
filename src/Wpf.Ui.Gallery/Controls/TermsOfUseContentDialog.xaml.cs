// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows.Controls;
using Wpf.Ui.Controls;

namespace Wpf.Ui.Gallery.Controls;

public partial class TermsOfUseContentDialog : ContentDialog
{
    public TermsOfUseContentDialog(ContentPresenter contentPresenter)
        : base(contentPresenter)
    {
        InitializeComponent();
    }

    protected override void OnButtonClick(ContentDialogButton button)
    {
        if (CheckBox.IsChecked != false)
        {
            base.OnButtonClick(button);
            return;
        }

        TextBlock.SetCurrentValue(VisibilityProperty, Visibility.Visible);
        CheckBox.Focus();
    }
}
