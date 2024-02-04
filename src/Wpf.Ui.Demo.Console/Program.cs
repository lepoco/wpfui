// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Threading;
using System.Windows;

public static class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        if (Application.Current is null)
            Console.WriteLine($"Application.Current is null.");

        try
        {
            new Wpf.Ui.Demo.Console.Views.SimpleView().ShowDialog();
            new Wpf.Ui.Demo.Console.Views.MainView().ShowDialog();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            Thread.Sleep(10000);
            throw;
        }
    }
}