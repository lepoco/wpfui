// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Runtime.ExceptionServices;
using System.Threading;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Threading;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;
using Wpf.Ui.Markup;

namespace Wpf.Ui.UnitTests.Controls;

public class NavigationViewLeftCompactTests
{
    [ThreadStatic]
    private static HwndSource? _host;

    [Fact]
    public void LeftCompact_PreservesEnumValues()
    {
        Assert.Equal(0, (int)NavigationViewPaneDisplayMode.Left);
        Assert.Equal(1, (int)NavigationViewPaneDisplayMode.LeftMinimal);
        Assert.Equal(2, (int)NavigationViewPaneDisplayMode.LeftFluent);
        Assert.Equal(3, (int)NavigationViewPaneDisplayMode.Top);
        Assert.Equal(4, (int)NavigationViewPaneDisplayMode.Bottom);
        Assert.Equal(5, (int)NavigationViewPaneDisplayMode.LeftCompact);
    }

    [Theory]
    [InlineData(NavigationViewPaneDisplayMode.Left)]
    [InlineData(NavigationViewPaneDisplayMode.LeftMinimal)]
    public void ExistingLeftModes_PreserveOpenAndClosedPaneWidths(NavigationViewPaneDisplayMode mode) =>
        RunInSta(() =>
        {
            var view = CreateView();
            view.PaneDisplayMode = mode;
            view.ApplyTemplate();
            view.IsPaneOpen = false;
            Pump(250);
            Assert.Equal(40, Part<Grid>(view, "PaneGrid").Width, 1);
            view.IsPaneOpen = true;
            Pump(250);
            Assert.Equal(320, Part<Grid>(view, "PaneGrid").Width, 1);
        });

    [Fact]
    public void LeftCompact_OpenPaneOverlaysContent() =>
        RunInSta(() =>
        {
            var view = CreateCompactView();
            Assert.Equal(40, Part<ColumnDefinition>(view, "PART_PaneColumn").ActualWidth, 1);
            view.CompactPaneLength = 56;
            view.UpdateLayout();
            Assert.Equal(56, Part<ColumnDefinition>(view, "PART_PaneColumn").ActualWidth, 1);
            view.IsPaneOpen = true;
            Pump(250);
            var content = Part<NavigationViewContentPresenter>(view, "PART_NavigationViewContentPresenter");
            Point openPosition = content.TranslatePoint(new Point(), view);
            Assert.Equal(320, Part<Grid>(view, "PaneGrid").Width, 1);
            Assert.Equal(56, Part<ColumnDefinition>(view, "PART_PaneColumn").ActualWidth, 1);
            view.IsPaneOpen = false;
            Pump(250);
            Point closedPosition = content.TranslatePoint(new Point(), view);
            Assert.Equal(openPosition.X, closedPosition.X, 1);
            Assert.Equal(56, Part<Grid>(view, "PaneGrid").Width, 1);
        });

    [Fact]
    public void LeftCompact_ParentOpensFlyoutWithoutOpeningPane() =>
        RunInSta(() =>
        {
            var view = CreateCompactView();
            var parent = AddHierarchy(view);
            parent.Invoke();
            Pump(100);
            Assert.False(view.IsPaneOpen);
            Assert.True(Flyout(parent).IsOpen);
            var child = (NavigationViewItem)parent.MenuItems[0]!;
            Assert.True(child.IsPaneOpen);
            Assert.Equal(
                Visibility.Visible,
                ((ContentPresenter)child.Template.FindName("ElementContentPresenter", child)).Visibility
            );
            parent.Invoke();
            Pump(50);
            Assert.False(Flyout(parent).IsOpen);
            Assert.False(parent.IsExpanded);
        });

    [Fact]
    public void LeftCompact_GrandchildNavigatesAndClosesFlyout() =>
        RunInSta(() =>
        {
            var view = CreateCompactView();
            var parent = AddHierarchy(view);
            var child = (InvokableNavigationItem)parent.MenuItems[0]!;
            var leaf = (InvokableNavigationItem)child.MenuItems[0]!;
            int invoked = 0;
            int selected = 0;
            view.ItemInvoked += (_, _) => invoked++;
            view.SelectionChanged += (_, _) => selected++;
            parent.Invoke();
            Pump(50);
            child.Invoke();
            Pump(50);
            Assert.True(child.IsExpanded);
            Assert.True(leaf.IsVisible);
            leaf.Invoke();
            Pump(50);
            Assert.Same(leaf, view.SelectedItem);
            Assert.True(leaf.IsActive);
            Assert.True(parent.IsActive);
            Assert.Same(child, leaf.NavigationViewItemParent);
            Assert.Same(parent, child.NavigationViewItemParent);
            Assert.False(Flyout(parent).IsOpen);
            Assert.False(view.IsPaneOpen);
            Assert.Equal(1, invoked);
            Assert.Equal(1, selected);
        });

    [Theory]
    [InlineData(NavigationViewPaneDisplayMode.Left)]
    [InlineData(NavigationViewPaneDisplayMode.LeftMinimal)]
    [InlineData(NavigationViewPaneDisplayMode.Top)]
    public void LeftCompact_ModeSwitchClosesFlyoutAndPreservesItems(NavigationViewPaneDisplayMode mode) =>
        RunInSta(() =>
        {
            var view = CreateCompactView();
            var parent = AddHierarchy(view);
            var child = parent.MenuItems[0];
            parent.Invoke();
            Pump(50);
            Popup popup = Flyout(parent);
            Assert.True(popup.IsOpen);
            view.PaneDisplayMode = mode;
            view.ApplyTemplate();
            Pump(100);
            Assert.False(popup.IsOpen);
            Assert.Same(child, parent.MenuItems[0]);
            view.PaneDisplayMode = NavigationViewPaneDisplayMode.LeftCompact;
            view.ApplyTemplate();
            Pump(100);
            parent.Invoke();
            Pump(50);
            Assert.False(view.IsPaneOpen);
            Assert.True(Flyout(parent).IsOpen);
        });

    [Fact]
    public void LeftCompact_EscapeRestoresFocusAndPaneToggleClosesFlyout() =>
        RunInSta(() =>
        {
            var view = CreateCompactView();
            var parent = AddHierarchy(view);
            parent.Invoke();
            Pump(50);
            var child = (InvokableNavigationItem)parent.MenuItems[0]!;
            child.Key(Key.Escape);
            Pump(50);
            Assert.False(Flyout(parent).IsOpen);
            Assert.Same(parent, Keyboard.FocusedElement);
            parent.Invoke();
            Pump(50);
            view.IsPaneOpen = true;
            Pump(250);
            Assert.False(Flyout(parent).IsOpen);
            Assert.True(parent.IsPaneOpen);
        });

    [Fact]
    public void LeftCompact_OpeningAnotherParentClosesPreviousFlyout() =>
        RunInSta(() =>
        {
            var view = CreateCompactView();
            var first = AddHierarchy(view);
            var second = AddHierarchy(view);
            first.Invoke();
            Pump(50);
            second.Invoke();
            Pump(50);
            Assert.False(Flyout(first).IsOpen);
            Assert.True(Flyout(second).IsOpen);
        });

    [Fact]
    public void LeftCompact_ThemeChangesUpdateOpenFlyout() =>
        RunInSta(() =>
        {
            var view = CreateCompactView();
            var parent = AddHierarchy(view);
            parent.Invoke();
            Pump(50);
            var border = (Border)Flyout(parent).Child;
            Assert.Same(view.FindResource("FlyoutBackground"), border.Background);
            ((ThemesDictionary)view.Resources.MergedDictionaries[0]).Theme = ApplicationTheme.Dark;
            Pump(50);
            Assert.Same(view.FindResource("FlyoutBackground"), border.Background);
            Assert.True(Flyout(parent).IsOpen);
            ((ThemesDictionary)view.Resources.MergedDictionaries[0]).Theme = ApplicationTheme.Light;
            Pump(50);
            Assert.Same(view.FindResource("FlyoutBackground"), border.Background);
        });

    [Fact]
    public void LeftCompact_AutomationExpansionCoordinatesFlyoutsAndOutsideDismissal() =>
        RunInSta(() =>
        {
            var view = CreateCompactView();
            var first = AddHierarchy(view);
            var second = AddHierarchy(view);
            first.Invoke();
            Pump(50);
            var peer = UIElementAutomationPeer.CreatePeerForElement(second)!;
            ((IExpandCollapseProvider)peer.GetPattern(PatternInterface.ExpandCollapse)!).Expand();
            Pump(50);
            Assert.False(Flyout(first).IsOpen);
            Assert.True(Flyout(second).IsOpen);
            Flyout(second).SetCurrentValue(Popup.IsOpenProperty, false);
            Pump(50);
            Assert.False(second.IsExpanded);
            second.Invoke();
            Pump(50);
            Assert.True(Flyout(second).IsOpen);
        });

    [Fact]
    public void LeftCompact_ArrowsNavigateExpandedHierarchyAndFooter() =>
        RunInSta(() =>
        {
            var view = CreateCompactView();
            var root = AddHierarchy(view);
            var child = (InvokableNavigationItem)root.MenuItems[0]!;
            var leaf = (InvokableNavigationItem)child.MenuItems[0]!;
            var footer = new InvokableNavigationItem
            {
                Content = "Settings",
                TargetPageType = typeof(CompactTestPage),
            };
            view.FooterMenuItems.Add(footer);
            view.UpdateLayout();
            Pump(50);
            root.Focus();
            root.Key(Key.Down);
            Assert.Same(footer, Keyboard.FocusedElement);
            footer.Key(Key.Up);
            Assert.Same(root, Keyboard.FocusedElement);
            root.Key(Key.Right);
            Pump(50);
            Assert.Same(child, Keyboard.FocusedElement);
            child.Key(Key.Right);
            Pump(50);
            Assert.Same(leaf, Keyboard.FocusedElement);
            leaf.Key(Key.Up);
            Assert.Same(child, Keyboard.FocusedElement);
            child.Key(Key.Left);
            Assert.False(child.IsExpanded);
        });

    [Theory]
    [InlineData(NavigationViewPaneDisplayMode.Left)]
    [InlineData(NavigationViewPaneDisplayMode.LeftMinimal)]
    [InlineData(NavigationViewPaneDisplayMode.Top)]
    public void LeftCompact_ModeSwitchRetainsSelectionAndNavigation(NavigationViewPaneDisplayMode mode) =>
        RunInSta(() =>
        {
            var view = CreateCompactView();
            var root = AddHierarchy(view);
            var child = (InvokableNavigationItem)root.MenuItems[0]!;
            var leaf = (InvokableNavigationItem)child.MenuItems[0]!;
            root.Invoke();
            Pump(50);
            child.Invoke();
            Pump(50);
            leaf.Invoke();
            Pump(50);
            object content = Part<NavigationViewContentPresenter>(
                view,
                "PART_NavigationViewContentPresenter"
            ).Content;
            view.PaneDisplayMode = mode;
            view.ApplyTemplate();
            Pump(50);
            Assert.Same(leaf, view.SelectedItem);
            Assert.True(leaf.IsActive);
            Assert.Same(
                content,
                Part<NavigationViewContentPresenter>(view, "PART_NavigationViewContentPresenter").Content
            );
            view.PaneDisplayMode = NavigationViewPaneDisplayMode.LeftCompact;
            view.ApplyTemplate();
            Pump(50);
            Assert.Same(leaf, view.SelectedItem);
            Assert.Same(
                content,
                Part<NavigationViewContentPresenter>(view, "PART_NavigationViewContentPresenter").Content
            );
            root.Invoke();
            Pump(50);
            Assert.True(Flyout(root).IsOpen);
        });

    [Fact]
    public void LeftCompact_LongMenusScrollAndRemainOwnedAfterReopening() =>
        RunInSta(() =>
        {
            var view = CreateCompactView();
            var root = AddHierarchy(view);
            for (int i = 0; i < 60; i++)
            {
                root.MenuItems.Add(new NavigationViewItem { Content = $"Document {i}" });
            }
            root.Invoke();
            Pump(50);
            var scroll = (ScrollViewer)((Border)Flyout(root).Child).Child;
            Assert.True(scroll.ScrollableHeight > 0);
            scroll.ScrollToEnd();
            Pump(50);
            Assert.True(scroll.VerticalOffset > 0);
            root.Invoke();
            Pump(50);
            root.Invoke();
            Pump(50);
            Assert.Equal(61, root.MenuItems.Count);
            Assert.Same(root, ((NavigationViewItem)root.MenuItems[60]!).NavigationViewItemParent);
        });

    [Fact]
    public void LeftCompact_ParentWithTargetNavigatesAndTogglesFlyout() =>
        RunInSta(() =>
        {
            var view = CreateCompactView();
            var root = AddHierarchy(view);
            root.TargetPageType = typeof(CompactTestPage);
            root.Invoke();
            Pump(50);
            Assert.Same(root, view.SelectedItem);
            Assert.True(Flyout(root).IsOpen);
            root.Invoke();
            Pump(50);
            Assert.False(Flyout(root).IsOpen);
        });

    [Theory]
    [InlineData(Key.Enter)]
    [InlineData(Key.Space)]
    public void LeftCompact_EnterAndSpaceInvokeItems(Key key) =>
        RunInSta(() =>
        {
            var view = CreateCompactView();
            var root = AddHierarchy(view);
            root.Focus();
            root.Key(key);
            root.ReleaseKey(key);
            Pump(50);
            Assert.True(Flyout(root).IsOpen);
            var child = (InvokableNavigationItem)root.MenuItems[0]!;
            child.Key(Key.Right);
            Pump(50);
            var leaf = (InvokableNavigationItem)child.MenuItems[0]!;
            Assert.Same(leaf, Keyboard.FocusedElement);
            leaf.Key(key);
            leaf.ReleaseKey(key);
            Pump(50);
            Assert.Same(leaf, view.SelectedItem);
            Assert.False(Flyout(root).IsOpen);
        });

    private static TestNavigationView CreateCompactView()
    {
        TestNavigationView view = CreateView();
        view.PaneDisplayMode = NavigationViewPaneDisplayMode.LeftCompact;
        view.ApplyTemplate();
        Pump(250);
        return view;
    }

    [Fact]
    public void LeftCompact_FromTopInitializesCompactVisualState() =>
        RunInSta(() =>
        {
            var view = CreateCompactView();
            view.PaneDisplayMode = NavigationViewPaneDisplayMode.Top;
            view.ApplyTemplate();
            Pump(50);
            view.PaneDisplayMode = NavigationViewPaneDisplayMode.LeftCompact;
            view.ApplyTemplate();
            Pump(250);
            Assert.Equal("PaneCompact", States(view).CurrentState.Name);
            Assert.Equal(
                Visibility.Hidden,
                Part<ContentPresenter>(view, "AutoSuggestBoxContentPresenter").Visibility
            );
        });

    [Fact]
    public void LeftCompact_RemovingChildrenClearsParentOwnership() =>
        RunInSta(() =>
        {
            var view = CreateCompactView();
            var root = AddHierarchy(view);
            var child = (NavigationViewItem)root.MenuItems[0]!;
            root.MenuItems.Clear();
            Assert.Null(child.NavigationViewItemParent);
            root.MenuItems.Add(child);
            Assert.Same(root, child.NavigationViewItemParent);
            root.MenuItems.Remove(child);
            Assert.Null(child.NavigationViewItemParent);
        });

    private static InvokableNavigationItem AddHierarchy(NavigationView view)
    {
        var root = new InvokableNavigationItem
        {
            Content = "Documents",
            Icon = new SymbolIcon(SymbolRegular.Document24),
        };
        var child = new InvokableNavigationItem
        {
            Content = "Projects",
            Icon = new SymbolIcon(SymbolRegular.Folder24),
        };
        child.MenuItems.Add(
            new InvokableNavigationItem { Content = "Project A", TargetPageType = typeof(CompactTestPage) }
        );
        root.MenuItems.Add(child);
        view.MenuItems.Add(root);
        view.UpdateLayout();
        Pump(50);
        return root;
    }

    private static Popup Flyout(NavigationViewItem item) =>
        (Popup)item.Template.FindName("PART_CompactFlyout", item);

    public sealed class CompactTestPage : Page { }

    private sealed class InvokableNavigationItem : NavigationViewItem
    {
        public void Invoke() => OnClick();

        public void ReleaseKey(Key key) =>
            OnKeyUp(
                new KeyEventArgs(Keyboard.PrimaryDevice, _host!, Environment.TickCount, key)
                {
                    RoutedEvent = Keyboard.KeyUpEvent,
                }
            );

        public void Key(Key key) =>
            OnKeyDown(
                new KeyEventArgs(Keyboard.PrimaryDevice, _host!, Environment.TickCount, key)
                {
                    RoutedEvent = Keyboard.KeyDownEvent,
                }
            );
    }

    private static TestNavigationView CreateView()
    {
        var view = new TestNavigationView { AutoSuggestBox = new AutoSuggestBox() };
        view.Resources.MergedDictionaries.Add(new ThemesDictionary());
        view.Resources.MergedDictionaries.Add(new ControlsDictionary());
        view.Style = (Style)view.FindResource("DefaultNavigationViewStyle");
        BindingOperations.SetBinding(
            view,
            NavigationView.OpenPaneLengthProperty,
            new Binding { Source = 320.0 }
        );
        _host = new HwndSource(
            new HwndSourceParameters("NavigationView pane tests")
            {
                Width = 1000,
                Height = 600,
                WindowStyle = unchecked((int)0x80000000),
            }
        );
        _host.RootVisual = view;
        view.Measure(new Size(1000, 600));
        view.Arrange(new Rect(0, 0, 1000, 600));
        view.ApplyTemplate();
        view.UpdateState();
        Pump(250);
        return view;
    }

    private static T Part<T>(NavigationView view, string name) => (T)view.Template.FindName(name, view);

    private static VisualStateGroup States(NavigationView view) =>
        VisualStateManager
            .GetVisualStateGroups((FrameworkElement)Part<Grid>(view, "PaneGrid").Parent)
            .OfType<VisualStateGroup>()
            .Single(group => group.Name == "PaneStates");

    private static void Pump(int milliseconds)
    {
        var frame = new DispatcherFrame();
        var timer = new DispatcherTimer(DispatcherPriority.Background)
        {
            Interval = TimeSpan.FromMilliseconds(milliseconds),
        };
        timer.Tick += (_, _) =>
        {
            timer.Stop();
            frame.Continue = false;
        };
        timer.Start();
        Dispatcher.PushFrame(frame);
    }

    private static void RunInSta(Action action)
    {
        Exception? error = null;
        var thread = new Thread(() =>
        {
            try
            {
                action();
            }
            catch (Exception exception)
            {
                error = exception;
            }
            finally
            {
                _host?.Dispose();
                Dispatcher.CurrentDispatcher.InvokeShutdown();
            }
        });
        thread.SetApartmentState(ApartmentState.STA);
        thread.Start();
        thread.Join();
        if (error is not null)
        {
            ExceptionDispatchInfo.Capture(error).Throw();
        }
    }

    private sealed class TestNavigationView : NavigationView
    {
        public void UpdateState() => UpdateVisualState(this);
    }
}
