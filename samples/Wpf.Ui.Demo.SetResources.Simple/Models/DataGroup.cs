// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

namespace Wpf.Ui.Demo.SetResources.Simple.Models;

public class DataGroup
{
    public DataGroup(bool selected, string name, string groupName)
    {
        Selected = selected;
        Name = name;
        GroupName = groupName;
    }

    public bool Selected { get; set; }
    public string Name { get; set; }
    public string GroupName { get; set; }
}