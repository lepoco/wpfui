// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

#nullable enable

using System.ComponentModel;
using System.Windows;
using Wpf.Ui.Controls.Interfaces;

namespace Wpf.Ui.Controls.Navigation;

/// <summary>
/// Inherited from the <see cref="System.Windows.Controls.Button"/>, used to navigate backwards inside the <see cref="INavigation"/>.
/// </summary>
public class NavigationBackButton : System.Windows.Controls.Button
{
    /// <summary>
    /// Property for <see cref="Navigation"/>.
    /// </summary>
    public static readonly DependencyProperty NavigationProperty = DependencyProperty.Register(nameof(Navigation),
        typeof(INavigation), typeof(NavigationBackButton), new PropertyMetadata(null));

    /// <summary>
    /// Parent <see cref="INavigation"/> control.
    /// </summary>
    [Bindable(true), Category("Behavior")]
    public INavigation? Navigation
    {
        get => (INavigation)GetValue(NavigationProperty);
        set => SetValue(NavigationProperty, value);
    }

    public NavigationBackButton()
    {
        SetValue(CommandProperty, new Common.RelayCommand(_ => Navigation?.NavigateBack(), () => Navigation is not null && Navigation.CanGoBack));
    }
}
