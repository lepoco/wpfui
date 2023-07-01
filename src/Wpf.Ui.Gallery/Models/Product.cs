// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

namespace Wpf.Ui.Gallery.Models;

public class Product
{
    public int ProductId { get; set; }

    public int ProductCode { get; set; }

    public string ProductName { get; set; }

    public string QuantityPerUnit { get; set; }

    public double UnitPrice { get; set; }

    public string UnitPriceString => UnitPrice.ToString("F2");

    public int UnitsInStock { get; set; }

    public bool IsVirtual { get; set; }
}
