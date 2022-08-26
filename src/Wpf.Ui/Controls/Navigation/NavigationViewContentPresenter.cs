// Based on Windows UI Library
// Copyright(c) Microsoft Corporation.All rights reserved.

// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Wpf.Ui.Controls.Navigation;

public class NavigationViewContentPresenter : Frame
{
    static NavigationViewContentPresenter()
    {
        //DefaultStyleKeyProperty.OverrideMetadata(
        //        typeof(NavigationViewContentPresenter),
        //        new FrameworkPropertyMetadata(typeof(NavigationViewContentPresenter)));

        NavigationUIVisibilityProperty.OverrideMetadata(
            typeof(NavigationViewContentPresenter),
            new FrameworkPropertyMetadata(NavigationUIVisibility.Hidden));

        SandboxExternalContentProperty.OverrideMetadata(
            typeof(NavigationViewContentPresenter),
            new FrameworkPropertyMetadata(true));

        JournalOwnershipProperty.OverrideMetadata(
            typeof(NavigationViewContentPresenter),
            new FrameworkPropertyMetadata(JournalOwnership.UsesParentJournal));
    }
}
