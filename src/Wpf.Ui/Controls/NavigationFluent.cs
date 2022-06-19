// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;

namespace Wpf.Ui.Controls;

/// <summary>
/// Modern navigation styled according to the principles of Fluent Design for Windows 11.
/// </summary>
public class NavigationFluent : Wpf.Ui.Controls.Navigation.NavigationBase
{
    /// <summary>
    /// Property for <see cref="TemplateButtonCommand"/>.
    /// </summary>
    public static readonly DependencyProperty TemplateButtonCommandProperty =
        DependencyProperty.Register(nameof(TemplateButtonCommand),
            typeof(Common.IRelayCommand), typeof(NavigationFluent), new PropertyMetadata(null));

    /// <summary>
    /// Command triggered after clicking the button.
    /// </summary>
    public Common.IRelayCommand TemplateButtonCommand => (Common.IRelayCommand)GetValue(TemplateButtonCommandProperty);


    /// <summary>
    /// Creates new instance and sets default <see cref="TemplateButtonCommandProperty"/>.
    /// </summary>
    public NavigationFluent() =>
        SetValue(TemplateButtonCommandProperty, new Common.RelayCommand(o => Button_OnClick(this, o), () => ReadyToNavigateBack));

    private void Button_OnClick(NavigationFluent navigationFluent, object o)
    {
        NavigateBack();
    }
}
