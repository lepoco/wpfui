// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Runtime.ExceptionServices;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Wpf.Ui.Controls;
using Wpf.Ui.Markup;
using NavigationView = Wpf.Ui.Controls.NavigationView;

namespace Wpf.Ui.UnitTests.Controls;

public class NavigationViewPaneInputTests
{
    [Fact]
    public void EmptySpaceToggle_IsDisabledByDefault() =>
        RunSta(() =>
        {
            NavigationView view = CreateView();
            Assert.False(view.IsPaneToggleOnEmptySpaceEnabled);
            Click(Part(view, "PART_PaneMenuSpace"));
            Assert.True(view.IsPaneOpen);
        });

    [Theory]
    [InlineData("Light", NavigationViewPaneDisplayMode.Left, "PART_PaneToggleSpace")]
    [InlineData("Light", NavigationViewPaneDisplayMode.Left, "PART_PaneMenuSpace")]
    [InlineData("Dark", NavigationViewPaneDisplayMode.Left, "PART_PaneToggleSpace")]
    [InlineData("Dark", NavigationViewPaneDisplayMode.Left, "PART_PaneMenuSpace")]
    [InlineData("Light", NavigationViewPaneDisplayMode.LeftMinimal, "PART_PaneMenuSpace")]
    [InlineData("Dark", NavigationViewPaneDisplayMode.LeftMinimal, "PART_PaneMenuSpace")]
    public void EmptySpaceClick_TogglesOnceAndPreservesBinding(
        string theme,
        NavigationViewPaneDisplayMode mode,
        string partName
    ) =>
        RunSta(() =>
        {
            NavigationView view = CreateView(theme, mode);
            BindingSource source = new BindingSource();
            BindingOperations.SetBinding(
                view,
                NavigationView.IsPaneOpenProperty,
                new Binding(nameof(BindingSource.IsOpen)) { Source = source, Mode = BindingMode.TwoWay }
            );
            int opened = 0;
            int closed = 0;
            view.PaneOpened += (_, _) => opened++;
            view.PaneClosed += (_, _) => closed++;
            view.IsPaneToggleOnEmptySpaceEnabled = true;
            FrameworkElement space = Part(view, partName);

            Click(space);
            Assert.False(view.IsPaneOpen);
            Assert.False(source.IsOpen);
            Assert.Equal(1, closed);
            Assert.Equal(0, opened);
            Click(space);
            Assert.True(view.IsPaneOpen);
            Assert.True(source.IsOpen);
            Assert.Equal(1, opened);
            Assert.True(BindingOperations.IsDataBound(view, NavigationView.IsPaneOpenProperty));

            view.IsPaneToggleOnEmptySpaceEnabled = false;
            Click(space);
            Assert.True(view.IsPaneOpen);
            Assert.Equal(1, closed);
        });

    [Theory]
    [InlineData(0, false, 360)]
    [InlineData(2, true, 500)]
    [InlineData(5, false, 800)]
    public void TemplateBackgrounds_AreHitTestableOnlyInUnusedSpace(int count, bool footer, int height) =>
        RunSta(() =>
        {
            NavigationView view = CreateView();
            view.IsPaneToggleOnEmptySpaceEnabled = true;
            for (int i = 0; i < count; i++)
            {
                view.MenuItems.Add(new NavigationViewItem { Content = $"Item {i}" });
            }

            if (footer)
            {
                view.FooterMenuItems.Add(new NavigationViewItem { Content = "Settings" });
            }

            Layout(view, height);
            Window window = new Window
            {
                Content = view,
                Width = 900,
                Height = height,
                ShowActivated = false,
                ShowInTaskbar = false,
            };
            try
            {
                window.Show();
                window.UpdateLayout();
                CompletePaneAnimation();
                AssertBackgroundHit(view, "PART_PaneToggleSpace", new Point(200, 10));
                FrameworkElement space = Part(view, "PART_PaneMenuSpace");
                Assert.True(space.ActualHeight > (count * 40) + 10);
                AssertBackgroundHit(view, "PART_PaneMenuSpace", new Point(20, space.ActualHeight - 10));
                view.SetCurrentValue(NavigationView.IsPaneOpenProperty, false);
                CompletePaneAnimation();
                window.UpdateLayout();
                AssertBackgroundHit(view, "PART_PaneMenuSpace", new Point(20, space.ActualHeight - 10));
                view.MenuItems.Clear();
                window.Height += 100;
                window.UpdateLayout();
                AssertBackgroundHit(view, "PART_PaneMenuSpace", new Point(20, space.ActualHeight - 10));
            }
            finally
            {
                window.Close();
            }
        });

    [Theory]
    [InlineData("menu")]
    [InlineData("settings")]
    [InlineData("button")]
    [InlineData("text")]
    [InlineData("search")]
    [InlineData("toggle")]
    [InlineData("combo")]
    [InlineData("scroll")]
    [InlineData("content")]
    [InlineData("header")]
    public void ChildAndContentClicks_DoNotToggle(string target) =>
        RunSta(() =>
        {
            NavigationView view = CreateView();
            view.IsPaneToggleOnEmptySpaceEnabled = true;
            NavigationViewItem menu = new NavigationViewItem
            {
                Content = "Menu",
                Icon = new SymbolIcon(SymbolRegular.Home24),
            };
            NavigationViewItem settings = new NavigationViewItem { Content = "Settings" };
            view.MenuItems.Add(menu);
            view.FooterMenuItems.Add(settings);
            FrameworkElement child = target switch
            {
                "menu" => menu,
                "settings" => settings,
                "button" => new System.Windows.Controls.Button(),
                "text" => new System.Windows.Controls.TextBox(),
                "search" => new AutoSuggestBox(),
                "toggle" => new CheckBox(),
                "combo" => new ComboBox(),
                "scroll" => new System.Windows.Controls.Primitives.ScrollBar(),
                _ => new Border { Background = System.Windows.Media.Brushes.Transparent },
            };
            if (target == "content")
            {
                view.ContentOverlay = child;
            }
            else if (target == "header")
            {
                view.PaneHeader = child;
            }
            else if (target is not ("menu" or "settings" or "search"))
            {
                view.PaneFooter = child;
            }

            // AutoSuggestBox installs a window hook when applying its template.
            Window? window = null;
            try
            {
                if (target == "search")
                {
                    window = new Window
                    {
                        Content = view,
                        ShowActivated = false,
                        ShowInTaskbar = false,
                    };
                    window.Show();
                    view.PaneFooter = child;
                }

                Layout(view);
                Click(child);
                Assert.True(view.IsPaneOpen);
                if (target == "menu")
                {
                    Click((UIElement)menu.Icon!);
                    Assert.True(view.IsPaneOpen);
                }
            }
            finally
            {
                window?.Close();
            }
        });

    [Fact]
    public void OverflowingMenu_PreservesScrollingAndItemHitTargets() =>
        RunSta(() =>
        {
            NavigationView view = CreateView();
            view.IsPaneToggleOnEmptySpaceEnabled = true;
            for (int i = 0; i < 30; i++)
            {
                view.MenuItems.Add(new NavigationViewItem { Content = $"Item {i}" });
            }

            Window window = new Window
            {
                Content = view,
                Width = 900,
                Height = 500,
                ShowActivated = false,
                ShowInTaskbar = false,
            };
            try
            {
                window.Show();
                CompletePaneAnimation();
                ScrollViewer scroll = (ScrollViewer)Part(view, "PART_ScrollViewer");
                Assert.True(scroll.ScrollableHeight > 0);
                scroll.ScrollToEnd();
                CompletePaneAnimation();
                window.UpdateLayout();
                Assert.True(scroll.VerticalOffset > 0);
                Point point = scroll.TranslatePoint(new Point(20, scroll.ActualHeight / 2), view);
                Assert.NotSame(Part(view, "PART_PaneMenuSpace"), view.InputHitTest(point));
                Assert.True(view.IsPaneOpen);
            }
            finally
            {
                window.Close();
            }
        });

    [Fact]
    public void WheelAndUnpairedRelease_DoNotToggle() =>
        RunSta(() =>
        {
            NavigationView view = CreateView();
            view.IsPaneToggleOnEmptySpaceEnabled = true;
            FrameworkElement space = Part(view, "PART_PaneMenuSpace");
            Press(space);
            space.RaiseEvent(
                new MouseWheelEventArgs(Mouse.PrimaryDevice, 0, 120)
                {
                    RoutedEvent = Mouse.PreviewMouseWheelEvent,
                }
            );
            Release(space);
            Release(space);
            Assert.True(view.IsPaneOpen);
            Press(space);
            Release(Part(view, "PART_PaneToggleSpace"));
            Assert.True(view.IsPaneOpen);
        });

    [Fact]
    public void DisablingOrChangingModeDuringPress_CancelsClick() =>
        RunSta(() =>
        {
            NavigationView view = CreateView();
            view.IsPaneToggleOnEmptySpaceEnabled = true;
            FrameworkElement space = Part(view, "PART_PaneMenuSpace");
            Press(space);
            view.IsPaneToggleOnEmptySpaceEnabled = false;
            view.IsPaneToggleOnEmptySpaceEnabled = true;
            Release(space);
            Assert.True(view.IsPaneOpen);
            Press(space);
            view.PaneDisplayMode = NavigationViewPaneDisplayMode.Top;
            view.PaneDisplayMode = NavigationViewPaneDisplayMode.Left;
            Release(space);
            Assert.True(view.IsPaneOpen);
        });

    [Fact]
    public void HamburgerAndBackButton_KeepExistingBehavior() =>
        RunSta(() =>
        {
            NavigationView view = CreateView();
            view.IsPaneToggleOnEmptySpaceEnabled = true;
            Part(view, "PART_BackButton").ApplyTemplate();
            Click(Part(view, "PART_BackButton"));
            Assert.True(view.IsPaneOpen);
            Part(view, "PART_ToggleButton")
                .RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Primitives.ButtonBase.ClickEvent));
            Assert.False(view.IsPaneOpen);
            Click(Part(view, "PART_PaneMenuSpace"));
            Assert.True(view.IsPaneOpen);
        });

    [Fact]
    public void DisplayModeAndTemplateChanges_DoNotLeaveStaleTargets() =>
        RunSta(() =>
        {
            NavigationView view = CreateView();
            view.IsPaneToggleOnEmptySpaceEnabled = true;
            FrameworkElement oldSpace = Part(view, "PART_PaneMenuSpace");
            foreach (
                NavigationViewPaneDisplayMode mode in new[]
                {
                    NavigationViewPaneDisplayMode.Top,
                    NavigationViewPaneDisplayMode.Bottom,
                    NavigationViewPaneDisplayMode.LeftFluent,
                }
            )
            {
                view.PaneDisplayMode = mode;
                Layout(view);
                Click(oldSpace);
                if (mode == NavigationViewPaneDisplayMode.LeftFluent)
                {
                    Click(Part(view, "PART_PaneMenuSpace"));
                }

                Assert.True(view.IsPaneOpen);
            }

            view.PaneDisplayMode = NavigationViewPaneDisplayMode.LeftMinimal;
            Layout(view);
            Click(Part(view, "PART_PaneMenuSpace"));
            Assert.False(view.IsPaneOpen);
            view.OnApplyTemplate();
            Click(Part(view, "PART_PaneMenuSpace"));
            Assert.True(view.IsPaneOpen);
        });

    [Theory]
    [InlineData(NavigationViewPaneDisplayMode.Top, true, false)]
    [InlineData(NavigationViewPaneDisplayMode.Top, false, false)]
    [InlineData(NavigationViewPaneDisplayMode.Bottom, true, false)]
    [InlineData(NavigationViewPaneDisplayMode.Bottom, false, false)]
    [InlineData(NavigationViewPaneDisplayMode.LeftFluent, true, false)]
    [InlineData(NavigationViewPaneDisplayMode.LeftFluent, false, false)]
    [InlineData(NavigationViewPaneDisplayMode.Top, true, true)]
    [InlineData(NavigationViewPaneDisplayMode.Top, false, true)]
    [InlineData(NavigationViewPaneDisplayMode.Bottom, true, true)]
    [InlineData(NavigationViewPaneDisplayMode.Bottom, false, true)]
    [InlineData(NavigationViewPaneDisplayMode.LeftFluent, true, true)]
    [InlineData(NavigationViewPaneDisplayMode.LeftFluent, false, true)]
    public void DisplayModeChange_InLoadedWindow_DoesNotLeaveInvalidAnimations(
        NavigationViewPaneDisplayMode mode,
        bool isPaneOpen,
        bool duringAnimation
    ) =>
        RunSta(() =>
        {
            NavigationView view = CreateView(mode: NavigationViewPaneDisplayMode.LeftMinimal);
            view.IsPaneToggleOnEmptySpaceEnabled = true;
            view.IsPaneOpen = isPaneOpen;
            view.MenuItems.Add(
                new NavigationViewItem { Content = "Dashboard", Icon = new SymbolIcon(SymbolRegular.Home24) }
            );
            view.FooterMenuItems.Add(
                new NavigationViewItem
                {
                    Content = "Settings",
                    Icon = new SymbolIcon(SymbolRegular.Settings24),
                }
            );
            Window window = new Window
            {
                Content = view,
                Width = 900,
                Height = 500,
                ShowActivated = false,
                ShowInTaskbar = false,
            };
            try
            {
                window.Show();
                CompletePaneAnimation();
                if (duringAnimation)
                {
                    view.IsPaneOpen = !isPaneOpen;
                    CompletePaneAnimation();
                    view.IsPaneOpen = isPaneOpen;
                }

                view.PaneDisplayMode = mode;
                CompletePaneAnimation();
                window.UpdateLayout();
                Assert.Equal(isPaneOpen, view.IsPaneOpen);
                view.PaneDisplayMode = NavigationViewPaneDisplayMode.Left;
                CompletePaneAnimation();
                window.UpdateLayout();
                FrameworkElement pane = Part(view, "PaneGrid");
                Assert.Equal(isPaneOpen ? view.OpenPaneLength : 40, pane.ActualWidth, 1);
            }
            finally
            {
                window.Close();
            }
        });

    [Theory]
    [InlineData(NavigationViewPaneDisplayMode.Left, true)]
    [InlineData(NavigationViewPaneDisplayMode.LeftMinimal, true)]
    [InlineData(NavigationViewPaneDisplayMode.Left, false)]
    [InlineData(NavigationViewPaneDisplayMode.LeftMinimal, false)]
    public void LeavingLeftFluent_RestoresConfiguredButtonVisibility(
        NavigationViewPaneDisplayMode mode,
        bool visible
    ) =>
        RunSta(() =>
        {
            NavigationView view = CreateView();
            view.IsPaneToggleVisible = visible;
            view.IsBackButtonVisible = NavigationViewBackButtonVisible.Visible;
            for (int i = 0; i < 2; i++)
            {
                view.PaneDisplayMode = NavigationViewPaneDisplayMode.LeftFluent;
                Assert.False(view.IsPaneToggleVisible);
                Assert.Equal(NavigationViewBackButtonVisible.Collapsed, view.IsBackButtonVisible);
                view.PaneDisplayMode = mode;
                Assert.Equal(visible, view.IsPaneToggleVisible);
                Assert.Equal(NavigationViewBackButtonVisible.Visible, view.IsBackButtonVisible);
            }
        });

    [Fact]
    public void LeftFluentButtonVisibility_PreservesBindingsAndLatestSourceValues() =>
        RunSta(() =>
        {
            NavigationView view = CreateView();
            NavigationView source = new NavigationView();
            view.SetBinding(
                NavigationView.IsPaneToggleVisibleProperty,
                new Binding(nameof(NavigationView.IsPaneToggleVisible))
                {
                    Source = source,
                    Mode = BindingMode.TwoWay,
                }
            );
            view.SetBinding(
                NavigationView.IsBackButtonVisibleProperty,
                new Binding(nameof(NavigationView.IsBackButtonVisible))
                {
                    Source = source,
                    Mode = BindingMode.TwoWay,
                }
            );
            view.PaneDisplayMode = NavigationViewPaneDisplayMode.LeftFluent;
            Assert.False(view.IsPaneToggleVisible);
            Assert.True(source.IsPaneToggleVisible);
            Assert.Equal(NavigationViewBackButtonVisible.Auto, source.IsBackButtonVisible);
            source.IsPaneToggleVisible = false;
            source.IsPaneToggleVisible = true;
            source.IsBackButtonVisible = NavigationViewBackButtonVisible.Visible;
            Assert.False(view.IsPaneToggleVisible);
            Assert.Equal(NavigationViewBackButtonVisible.Collapsed, view.IsBackButtonVisible);
            view.PaneDisplayMode = NavigationViewPaneDisplayMode.Left;
            Assert.True(view.IsPaneToggleVisible);
            Assert.Equal(NavigationViewBackButtonVisible.Visible, view.IsBackButtonVisible);
            Assert.True(BindingOperations.IsDataBound(view, NavigationView.IsPaneToggleVisibleProperty));
            Assert.True(BindingOperations.IsDataBound(view, NavigationView.IsBackButtonVisibleProperty));
        });

    private static NavigationView CreateView(
        string theme = "Light",
        NavigationViewPaneDisplayMode mode = NavigationViewPaneDisplayMode.Left
    )
    {
        NavigationView view = new NavigationView
        {
            PaneDisplayMode = mode,
            IsBackButtonVisible = NavigationViewBackButtonVisible.Collapsed,
        };
        view.Resources.MergedDictionaries.Add(
            new ResourceDictionary
            {
                Source = new Uri($"pack://application:,,,/Wpf.Ui;component/Resources/Theme/{theme}.xaml"),
            }
        );
        view.Resources.MergedDictionaries.Add(new ControlsDictionary());
        view.Style = (Style)view.FindResource(typeof(NavigationView));
        Layout(view);
        return view;
    }

    private static void Layout(NavigationView view, double height = 500)
    {
        view.Measure(new Size(900, height));
        view.Arrange(new Rect(0, 0, 900, height));
        view.UpdateLayout();
    }

    private static FrameworkElement Part(NavigationView view, string name) =>
        (FrameworkElement)view.Template.FindName(name, view);

    private static void CompletePaneAnimation()
    {
        System.Windows.Threading.DispatcherFrame frame = new System.Windows.Threading.DispatcherFrame();
        System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(250),
        };
        timer.Tick += (_, _) =>
        {
            timer.Stop();
            frame.Continue = false;
        };
        timer.Start();
        System.Windows.Threading.Dispatcher.PushFrame(frame);
    }

    private static void AssertBackgroundHit(NavigationView view, string name, Point position)
    {
        FrameworkElement part = Part(view, name);
        Assert.Same(part, view.InputHitTest(part.TranslatePoint(position, view)));
    }

    private static void Click(UIElement target)
    {
        Press(target);
        Release(target);
    }

    private static void Press(UIElement target) =>
        target.RaiseEvent(
            new MouseButtonEventArgs(Mouse.PrimaryDevice, 0, MouseButton.Left)
            {
                RoutedEvent = Mouse.PreviewMouseDownEvent,
            }
        );

    private static void Release(UIElement target) =>
        target.RaiseEvent(
            new MouseButtonEventArgs(Mouse.PrimaryDevice, 0, MouseButton.Left)
            {
                RoutedEvent = Mouse.MouseUpEvent,
            }
        );

    private static void RunSta(Action action)
    {
        Exception? failure = null;
        Thread thread = new Thread(() =>
        {
            try
            {
                action();
            }
            catch (Exception e)
            {
                failure = e;
            }
            finally
            {
                System.Windows.Threading.Dispatcher.CurrentDispatcher.InvokeShutdown();
            }
        });
        thread.SetApartmentState(ApartmentState.STA);
        thread.Start();
        thread.Join();
        if (failure is not null)
        {
            ExceptionDispatchInfo.Capture(failure).Throw();
        }
    }

    private sealed class BindingSource
    {
        public bool IsOpen { get; set; } = true;
    }
}
