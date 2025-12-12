// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Threading;
using System.Windows.Automation.Peers;
using System.Windows.Controls;

namespace Wpf.Ui.UnitTests.AutomationPeers;

public class FilteringWindowAutomationPeerTests
{
    [Fact]
    public void GetChildren_IncludesChildByDefault()
    {
        Exception? ex = null;

        var thread = new Thread(() =>
        {
            try
            {
                var win = new TestWindow();

                var grid = new Grid();
                var btn = new Button();
                grid.Children.Add(btn);
                win.Content = grid;

                // create peers
                UIElementAutomationPeer.CreatePeerForElement(win);
                UIElementAutomationPeer.CreatePeerForElement(btn);

                var winPeer = new TestFilteringWindowAutomationPeer(win);

                Assert.NotNull(winPeer);

                var peer = new ButtonAutomationPeer(btn);

                // Should include by default
                Assert.True(winPeer.ExposeShouldIncludeChild(peer, btn));
            }
            catch (Exception e)
            {
                ex = e;
            }
        });

        thread.SetApartmentState(ApartmentState.STA);
        thread.Start();
        thread.Join();

        if (ex != null)
        {
            throw ex;
        }
    }

    [Fact]
    public void RegisterSuppressed_ExcludesChild()
    {
        Exception? ex = null;

        var thread = new Thread(() =>
        {
            try
            {
                var win = new TestWindow();

                var grid = new Grid();
                var btn = new Button();
                grid.Children.Add(btn);
                win.Content = grid;

                // create peers
                UIElementAutomationPeer.CreatePeerForElement(win);
                UIElementAutomationPeer.CreatePeerForElement(btn);

                var winPeer = new TestFilteringWindowAutomationPeer(win);

                Assert.NotNull(winPeer);

                var peer = new ButtonAutomationPeer(btn);

                // Ensure included initially
                Assert.True(winPeer.ExposeShouldIncludeChild(peer, btn));

                // Register suppressed and verify exclusion
                winPeer.RegisterSuppressed(btn);

                // Reset cache to force re-evaluation
                winPeer.ResetChildrenCache();

                Assert.False(winPeer.ExposeShouldIncludeChild(peer, btn));
            }
            catch (Exception e)
            {
                ex = e;
            }
        });

        thread.SetApartmentState(ApartmentState.STA);
        thread.Start();
        thread.Join();

        if (ex != null)
        {
            throw ex;
        }
    }

    [Fact]
    public void MultipleChildren_PartialSuppressed()
    {
        Exception? ex = null;

        var thread = new Thread(() =>
        {
            try
            {
                var win = new TestWindow();

                var panel = new StackPanel();
                var btnA = new Button();
                var btnB = new Button();
                panel.Children.Add(btnA);
                panel.Children.Add(btnB);
                win.Content = panel;

                var winPeer = new TestFilteringWindowAutomationPeer(win);

                var peerA = new ButtonAutomationPeer(btnA);
                var peerB = new ButtonAutomationPeer(btnB);

                // Initially both included
                Assert.True(winPeer.ExposeShouldIncludeChild(peerA, btnA));
                Assert.True(winPeer.ExposeShouldIncludeChild(peerB, btnB));

                // Suppress A only
                winPeer.RegisterSuppressed(btnA);

                Assert.False(winPeer.ExposeShouldIncludeChild(peerA, btnA));
                Assert.True(winPeer.ExposeShouldIncludeChild(peerB, btnB));
            }
            catch (Exception e)
            {
                ex = e;
            }
        });

        thread.SetApartmentState(ApartmentState.STA);
        thread.Start();
        thread.Join();

        if (ex != null)
        {
            throw ex;
        }
    }

    [Fact]
    public void AncestorSuppressed_ExcludesDescendant()
    {
        Exception? ex = null;

        var thread = new Thread(() =>
        {
            try
            {
                var win = new TestWindow();

                var grid = new Grid();
                var stack = new StackPanel();
                var btn = new Button();
                grid.Children.Add(stack);
                stack.Children.Add(btn);
                win.Content = grid;

                var winPeer = new TestFilteringWindowAutomationPeer(win);

                var peer = new ButtonAutomationPeer(btn);

                // Suppress the Grid (ancestor of button)
                winPeer.RegisterSuppressed(grid);

                Assert.False(winPeer.ExposeShouldIncludeChild(peer, btn));
            }
            catch (Exception e)
            {
                ex = e;
            }
        });

        thread.SetApartmentState(ApartmentState.STA);
        thread.Start();
        thread.Join();

        if (ex != null)
        {
            throw ex;
        }
    }

    [Fact]
    public void UnregisterSuppressed_RestoresInclusion()
    {
        Exception? ex = null;

        var thread = new Thread(() =>
        {
            try
            {
                var win = new TestWindow();

                var btn = new Button();
                win.Content = btn;

                var winPeer = new TestFilteringWindowAutomationPeer(win);
                var peer = new ButtonAutomationPeer(btn);

                winPeer.RegisterSuppressed(btn);
                Assert.False(winPeer.ExposeShouldIncludeChild(peer, btn));

                winPeer.UnregisterSuppressed(btn);
                Assert.True(winPeer.ExposeShouldIncludeChild(peer, btn));
            }
            catch (Exception e)
            {
                ex = e;
            }
        });

        thread.SetApartmentState(ApartmentState.STA);
        thread.Start();
        thread.Join();

        if (ex != null)
        {
            throw ex;
        }
    }

    [Fact]
    public void ClearRegisteredSuppressed_ClearsAll()
    {
        Exception? ex = null;

        var thread = new Thread(() =>
        {
            try
            {
                var win = new TestWindow();

                var btn1 = new Button();
                var btn2 = new Button();
                var panel = new StackPanel();
                panel.Children.Add(btn1);
                panel.Children.Add(btn2);
                win.Content = panel;

                var winPeer = new TestFilteringWindowAutomationPeer(win);
                var p1 = new ButtonAutomationPeer(btn1);
                var p2 = new ButtonAutomationPeer(btn2);

                winPeer.RegisterSuppressed(btn1);
                winPeer.RegisterSuppressed(btn2);

                Assert.False(winPeer.ExposeShouldIncludeChild(p1, btn1));
                Assert.False(winPeer.ExposeShouldIncludeChild(p2, btn2));

                winPeer.ClearRegisteredSuppressed();

                Assert.True(winPeer.ExposeShouldIncludeChild(p1, btn1));
                Assert.True(winPeer.ExposeShouldIncludeChild(p2, btn2));
            }
            catch (Exception e)
            {
                ex = e;
            }
        });

        thread.SetApartmentState(ApartmentState.STA);
        thread.Start();
        thread.Join();

        if (ex != null)
        {
            throw ex;
        }
    }

    private class TestWindow : Window
    {
        protected override AutomationPeer OnCreateAutomationPeer()
        {
            return new Wpf.Ui.AutomationPeers.FilteringWindowAutomationPeer(this);
        }
    }

    private class TestFilteringWindowAutomationPeer : Wpf.Ui.AutomationPeers.FilteringWindowAutomationPeer
    {
        public TestFilteringWindowAutomationPeer(Window owner)
            : base(owner)
        {
        }

        public bool ExposeShouldIncludeChild(AutomationPeer peer, DependencyObject? owner)
        {
            return ShouldIncludeChild(peer, owner);
        }
    }
}
