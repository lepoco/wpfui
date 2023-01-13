// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

namespace Wpf.Ui.Controls.Navigation;

/// <summary>
/// Notifies class about being navigated.
/// </summary>
public interface INavigationAware
{
    /// <summary>
    /// Method triggered when the class is navigated.
    /// </summary>
    void OnNavigatedTo();

    /// <summary>
    /// Method triggered when the navigation leaves the current class.
    /// </summary>
    void OnNavigatedFrom();
}
