// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
//
// Code from https://github.com/microsoft/microsoft-ui-xaml/
//

namespace Wpf.Ui.Common;

#if NET5_0_OR_GREATER
public delegate void ColorChangedEventHandler<T>([System.Diagnostics.CodeAnalysis.NotNull] T sender, ColorChangedEventArgs args);
#else
public delegate void ColorChangedEventHandler<T>(T sender, ColorChangedEventArgs args);
#endif
