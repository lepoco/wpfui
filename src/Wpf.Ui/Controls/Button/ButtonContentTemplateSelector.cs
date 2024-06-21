// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows.Controls;
// ReSharper disable CheckNamespace

namespace Wpf.Ui.Controls;

public class ButtonContentTemplateSelector : DataTemplateSelector
{ 
    public DataTemplate? StringDataTemplate { get; set; }

    public override DataTemplate? SelectTemplate(object? item, DependencyObject container)
    {
        if (container is ContentPresenter { ContentTemplate: not null } contentPresenter)
        {
            return contentPresenter.ContentTemplate;
        }

        return item is string ? StringDataTemplate : base.SelectTemplate(item, container);
    }
}