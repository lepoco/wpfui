// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Wpf.Ui.Common.Media;

namespace Wpf.Ui.Common;

#if NET5_0_OR_GREATER
public delegate void ColorChangedEventHandler<T>([System.Diagnostics.CodeAnalysis.NotNull] T sender, ColorChangedEventArgs args);
#else
public delegate void ColorChangedEventHandler<T>(T sender, ColorChangedEventArgs args);
#endif
