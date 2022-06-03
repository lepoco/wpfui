// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;

namespace WPFUI.Demo.Services.Contracts;

public interface ITestWindowService
{
    public void Show(Type windowType);

    public T Show<T>() where T : class;
}

