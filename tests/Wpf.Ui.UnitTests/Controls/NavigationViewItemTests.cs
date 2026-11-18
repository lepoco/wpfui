// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Runtime.ExceptionServices;
using System.Threading;
using System.Windows.Controls;
using Wpf.Ui.Controls;

namespace Wpf.Ui.UnitTests.Controls;

public class NavigationViewItemTests
{
    private static void RunOnStaThread(Action action)
    {
        Exception? exception = null;
        var thread = new Thread(() =>
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                exception = ex;
            }
        });

        thread.SetApartmentState(ApartmentState.STA);
        thread.Start();
        thread.Join();

        if (exception is not null)
        {
            ExceptionDispatchInfo.Capture(exception).Throw();
        }
    }

    [Fact]
    public void NavigationViewItem_ExpandDirection_DefaultsToDown()
    {
        RunOnStaThread(() =>
        {
            var item = new NavigationViewItem();
            Assert.Equal(ExpandDirection.Down, item.ExpandDirection);
        });
    }

    [Fact]
    public void NavigationViewItem_ExpandDirection_CanBeSetToUp()
    {
        RunOnStaThread(() =>
        {
            var item = new NavigationViewItem
            {
                ExpandDirection = ExpandDirection.Up,
            };

            Assert.Equal(ExpandDirection.Up, item.ExpandDirection);
        });
    }

    [Fact]
    public void NavigationView_ExpandDirection_DefaultsToDown()
    {
        RunOnStaThread(() =>
        {
            var navigationView = new NavigationView();
            Assert.Equal(ExpandDirection.Down, navigationView.ExpandDirection);
        });
    }

    [Fact]
    public void NavigationView_ExpandDirection_CanBeSetToUp()
    {
        RunOnStaThread(() =>
        {
            var navigationView = new NavigationView
            {
                ExpandDirection = ExpandDirection.Up,
            };

            Assert.Equal(ExpandDirection.Up, navigationView.ExpandDirection);
        });
    }

    [Fact]
    public void NavigationViewItem_AttachedAccessors_WorkCorrectly()
    {
        RunOnStaThread(() =>
        {
            var item = new NavigationViewItem();
            NavigationViewItem.SetExpandDirection(item, ExpandDirection.Up);

            Assert.Equal(ExpandDirection.Up, NavigationViewItem.GetExpandDirection(item));
            Assert.Equal(ExpandDirection.Up, item.ExpandDirection);
        });
    }
}
