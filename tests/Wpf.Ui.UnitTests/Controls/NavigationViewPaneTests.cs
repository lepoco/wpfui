// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Runtime.ExceptionServices;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using Wpf.Ui.Controls;
using Wpf.Ui.Markup;

namespace Wpf.Ui.UnitTests.Controls;

public class NavigationViewPaneTests
{
    [ThreadStatic]
    private static HwndSource? _host;

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void Toggle_AnimatesPaneAndSearchTogether(bool splitterEnabled) =>
        RunInSta(() =>
        {
            TestNavigationView view = CreateView(splitterEnabled);
            Grid pane = Part<Grid>(view, "PaneGrid");
            ContentPresenter search = Part<ContentPresenter>(view, "AutoSuggestBoxContentPresenter");
            ColumnDefinition column = Part<ColumnDefinition>(view, "PART_PaneColumn");
            view.IsPaneOpen = false;
            Assert.True(column.Width.IsAuto);
            Assert.True(pane.HasAnimatedProperties);
            DoubleAnimation closeAnimation = States(view)
                .CurrentState.Storyboard.Children.OfType<DoubleAnimation>()
                .Single(timeline => Storyboard.GetTargetName(timeline) == "PaneGrid");
            Assert.Equal(320, closeAnimation.From!.Value, 1);
            Pump(80);
            Assert.InRange(pane.Width, 40, 320);
            Assert.InRange(search.Opacity, 0, 1);
            Pump(200);
            Assert.Equal(40, pane.Width, 1);
            Assert.Equal(48, column.ActualWidth, 1); // The original pane includes two 4px margins.
            Assert.Equal(Visibility.Hidden, search.Visibility);

            view.IsPaneOpen = true;
            Pump(250);
            Assert.Equal(320, pane.Width, 1);
            Assert.Equal(Visibility.Visible, search.Visibility);
            Assert.Equal(1, search.Opacity, 1);
            foreach (VisualState state in States(view).States)
            {
                DoubleAnimation animation = state
                    .Storyboard.Children.OfType<DoubleAnimation>()
                    .Single(timeline => Storyboard.GetTargetName(timeline) == "PaneGrid");
                Assert.Equal(TimeSpan.FromSeconds(0.16), animation.Duration.TimeSpan);
                Assert.Equal(0.4, animation.AccelerationRatio);
            }
        });

    [Theory]
    [InlineData(430)]
    [InlineData(180)]
    public void Resize_RestoresWidthAfterToggleAndTemplateReplacement(double width) =>
        RunInSta(() =>
        {
            TestNavigationView view = CreateView(true);
            Resize(view, width);
            Assert.Equal(width, Part<Grid>(view, "PaneGrid").Width, 1);
            Assert.Equal(320, view.OpenPaneLength);
            Assert.True(BindingOperations.IsDataBound(view, NavigationView.OpenPaneLengthProperty));
            view.IsPaneOpen = false;
            Pump(250);
            view.Measure(new Size(800, 600));
            view.Arrange(new Rect(0, 0, 800, 600));
            Assert.Equal(40, Part<Grid>(view, "PaneGrid").Width, 1);
            view.IsPaneOpen = true;
            Pump(250);
            Assert.Equal(width, Part<Grid>(view, "PaneGrid").Width, 1);

            GridSplitter oldSplitter = Part<GridSplitter>(view, "PART_GridSplitter");
            ControlTemplate template = view.Template;
            view.Template = null;
            view.Template = template;
            view.ApplyTemplate();
            view.UpdateLayout();
            view.UpdateState();
            Pump(250);
            oldSplitter.RaiseEvent(new DragCompletedEventArgs(0, 0, false));
            Assert.Equal(width, Part<Grid>(view, "PaneGrid").Width, 1);
            Resize(view, width + 20);
            Assert.Equal(width + 20, Part<Grid>(view, "PaneGrid").Width, 1);
        });

    [Theory]
    [InlineData(320, 0)]
    [InlineData(430, 12)]
    [InlineData(180, 0)]
    public void Splitter_HitAreaIsCenteredOnContentBorder(double width, double frameMargin) =>
        RunInSta(() =>
        {
            TestNavigationView view = CreateView(true);
            view.FrameMargin = new Thickness(frameMargin, 0, 0, 0);
            Resize(view, width);
            GridSplitter splitter = Part<GridSplitter>(view, "PART_GridSplitter");
            var content = Part<NavigationViewContentPresenter>(view, "PART_NavigationViewContentPresenter");
            var border = (Border)((Grid)content.Parent).Parent;
            Point splitterCenter = splitter.TranslatePoint(new Point(splitter.ActualWidth / 2, 100), view);
            Point borderCenter = border.TranslatePoint(new Point(border.BorderThickness.Left / 2, 100), view);
            Assert.Equal(borderCenter.X, splitterCenter.X, 5);
            foreach (double x in new[] { 0.25, splitter.ActualWidth - 0.25 })
            {
                Point point = splitter.TranslatePoint(new Point(x, 100), view);
                HitTestResult hit = VisualTreeHelper.HitTest(view, point)!;
                Assert.True(hit.VisualHit == splitter || splitter.IsAncestorOf(hit.VisualHit));
            }
        });

    [Fact]
    public void RapidToggle_StartsFromCurrentAnimatedWidth() =>
        RunInSta(() =>
        {
            TestNavigationView view = CreateView(true);
            Resize(view, 430);
            view.IsPaneOpen = false;
            Pump(60);
            double intermediateWidth = Part<Grid>(view, "PaneGrid").Width;
            double intermediateOpacity = Part<ContentPresenter>(
                view,
                "AutoSuggestBoxContentPresenter"
            ).Opacity;
            view.IsPaneOpen = true;
            DoubleAnimation animation = States(view)
                .CurrentState.Storyboard.Children.OfType<DoubleAnimation>()
                .Single(timeline => Storyboard.GetTargetName(timeline) == "PaneGrid");
            Assert.Equal(intermediateWidth, animation.From!.Value, 1);
            DoubleAnimation fade = States(view)
                .CurrentState.Storyboard.Children.OfType<DoubleAnimation>()
                .Single(timeline => Storyboard.GetTargetName(timeline) == "AutoSuggestBoxContentPresenter");
            Assert.Equal(intermediateOpacity, fade.From!.Value, 2);
            view.IsPaneOpen = false;
            view.IsPaneOpen = true;
            Pump(250);
            Assert.Equal(430, Part<Grid>(view, "PaneGrid").Width, 1);
            Assert.True(Part<GridSplitter>(view, "PART_GridSplitter").IsEnabled);
        });

    [Fact]
    public void SplitterToggleAndOpenPaneLength_PreserveBindingsAndClosedWidth() =>
        RunInSta(() =>
        {
            TestNavigationView view = CreateView(true);
            Resize(view, 430);
            view.IsGridSplitterEnabled = false;
            view.UpdateLayout();
            Assert.Equal(320, Part<Grid>(view, "PaneGrid").Width, 1);
            Assert.Equal(Visibility.Collapsed, Part<GridSplitter>(view, "PART_GridSplitter").Visibility);
            view.IsGridSplitterEnabled = true;
            Assert.Equal(430, Part<Grid>(view, "PaneGrid").Width, 1);
            view.IsPaneOpen = false;
            Pump(250);
            view.SetCurrentValue(NavigationView.OpenPaneLengthProperty, 360.0);
            Assert.Equal(40, Part<Grid>(view, "PaneGrid").Width, 1);
            Assert.False(Part<GridSplitter>(view, "PART_GridSplitter").IsEnabled);
            view.IsPaneOpen = true;
            Pump(250);
            Assert.Equal(360, Part<Grid>(view, "PaneGrid").Width, 1);
            Assert.True(BindingOperations.IsDataBound(view, NavigationView.OpenPaneLengthProperty));
        });

    [Theory]
    [InlineData(NavigationViewPaneDisplayMode.Top)]
    [InlineData(NavigationViewPaneDisplayMode.Bottom)]
    [InlineData(NavigationViewPaneDisplayMode.LeftFluent)]
    public void OtherModes_DoNotAnimatePaneWidth(NavigationViewPaneDisplayMode mode) =>
        RunInSta(() =>
        {
            TestNavigationView view = CreateView(true);
            view.PaneDisplayMode = mode;
            view.ApplyTemplate();
            view.UpdateLayout();
            view.IsPaneOpen = false;
            view.UpdateState();
            Pump(250);
            Grid? pane = view.Template.FindName("PaneGrid", view) as Grid;
            if (pane is not null)
            {
                Assert.False(pane.HasAnimatedProperties);
            }
        });

    [Fact]
    public void CanceledResize_RestoresPreviousWidth() =>
        RunInSta(() =>
        {
            TestNavigationView view = CreateView(true);
            Resize(view, 430);
            GridSplitter splitter = Part<GridSplitter>(view, "PART_GridSplitter");
            splitter.RaiseEvent(new DragStartedEventArgs(0, 0));
            splitter.RaiseEvent(new DragDeltaEventArgs(-80, 0));
            splitter.RaiseEvent(new DragCompletedEventArgs(0, 0, true));
            view.UpdateLayout();
            Assert.Equal(430, Part<Grid>(view, "PaneGrid").Width, 1);
        });

    [Theory]
    [InlineData(Key.Left, 310)]
    [InlineData(Key.Right, 330)]
    public void KeyboardResize_PreservesWidthAfterToggling(Key key, double width) =>
        RunInSta(() =>
        {
            TestNavigationView view = CreateView(true);
            GridSplitter splitter = Part<GridSplitter>(view, "PART_GridSplitter");
            splitter.RaiseEvent(
                new KeyEventArgs(Keyboard.PrimaryDevice, _host!, Environment.TickCount, key)
                {
                    RoutedEvent = Keyboard.KeyDownEvent,
                }
            );
            view.IsPaneOpen = false;
            Pump(250);
            view.IsPaneOpen = true;
            Pump(250);
            Assert.Equal(width, Part<Grid>(view, "PaneGrid").Width, 1);
        });

    private static TestNavigationView CreateView(bool splitterEnabled)
    {
        var view = new TestNavigationView
        {
            IsGridSplitterEnabled = splitterEnabled,
            AutoSuggestBox = new AutoSuggestBox(),
        };
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

    private static void Resize(NavigationView view, double width)
    {
        GridSplitter splitter = Part<GridSplitter>(view, "PART_GridSplitter");
        double previousWidth = Part<Grid>(view, "PaneGrid").Width;
        splitter.RaiseEvent(new DragStartedEventArgs(0, 0));
        splitter.RaiseEvent(new DragDeltaEventArgs(width - previousWidth, 0));
        splitter.RaiseEvent(new DragCompletedEventArgs(0, 0, false));
        view.UpdateLayout();
    }

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
