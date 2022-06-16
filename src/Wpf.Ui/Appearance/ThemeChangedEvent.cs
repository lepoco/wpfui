// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows.Media;

namespace Wpf.Ui.Appearance;

/// <summary>
/// Event triggered when application theme is updated.
/// </summary>
/// <param name="currentTheme">Current application <see cref="ThemeType"/>.</param>
/// <param name="systemAccent">Current base system accent <see cref="Color"/>.</param>
public delegate void ThemeChangedEvent(ThemeType currentTheme, Color systemAccent);
