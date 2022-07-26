// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

namespace Wpf.Ui.Animations;

/// <summary>
/// Available types of transitions.
/// </summary>
public enum TransitionType
{
    /// <summary>
    /// None.
    /// </summary>
    None,

    /// <summary>
    /// Change opacity.
    /// </summary>
    FadeIn,

    /// <summary>
    /// Change opacity and slide from bottom.
    /// </summary>
    FadeInWithSlide,

    /// <summary>
    /// Slide from bottom.
    /// </summary>
    SlideBottom,

    /// <summary>
    /// Slide from the right side.
    /// </summary>
    SlideRight,

    /// <summary>
    /// Slide from the left side.
    /// </summary>
    SlideLeft,
}
