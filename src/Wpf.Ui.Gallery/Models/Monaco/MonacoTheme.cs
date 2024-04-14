// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

namespace Wpf.Ui.Gallery.Models.Monaco;

[Serializable]
public record MonacoTheme
{
    public string? Base { get; init; }

    public bool Inherit { get; init; }

    public IDictionary<string, string>? Rules { get; init; }

    public IDictionary<string, string>? Colors { get; init; }
}
