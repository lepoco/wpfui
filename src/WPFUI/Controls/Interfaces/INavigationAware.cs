// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

namespace WPFUI.Controls.Interfaces;

/// <summary>
/// Notifies class about being navigated.
/// </summary>
public interface INavigationAware
{
    /// <summary>
    /// Method triggered when the class is navigated.
    /// </summary>
    /// <param name="sender">Navigation service, from which the navigation was made.</param>
    void OnNavigatedTo(INavigation sender);

    /// <summary>
    /// Method triggered when the navigation leaves the current class.
    /// </summary>
    /// <param name="sender">Navigation service, from which the navigation was made.</param>
    void OnNavigatedFrom(INavigation sender);
}
