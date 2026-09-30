// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.IO;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;
using Wpf.Ui.Markup;
using Button = System.Windows.Controls.Button;
using TextBlock = System.Windows.Controls.TextBlock;

namespace Wpf.Ui.UnitTests.Controls;

public sealed class TimePickerTests
{
    [Theory]
    [InlineData(ApplicationTheme.Light)]
    [InlineData(ApplicationTheme.Dark)]
    public void Popup_UsesOpaqueNineRowTemplateAndAlignedCenter(ApplicationTheme theme) =>
        RunSta(() =>
        {
            using var host = new PickerWindow(theme);
            host.Picker.ClockIdentifier = ClockIdentifier.Clock12Hour;
            host.Picker.SelectedTime = new TimeSpan(13, 30, 0);
            host.Open();
            var selection = host.Part<Grid>("PickerSelectionGrid");
            var border = host.Part<Border>("PopupBorder");
            var closed = host.Part<Border>("ContentBorder");
            Assert.Equal(360, selection.ActualHeight, 3);
            Assert.Equal(new Thickness(1), border.BorderThickness);
            var brush = Assert.IsType<SolidColorBrush>(border.Background);
            Assert.Equal(byte.MaxValue, brush.Color.A);
            Assert.Equal(1, brush.Opacity);
            Assert.Equal(1, border.Opacity);
            var highlight = host.Part<Border>("SelectionHighlight");
            var highlightBrush = Assert.IsType<SolidColorBrush>(highlight.Background);
            Assert.Equal(byte.MaxValue, highlightBrush.Color.A);
            Assert.Equal(
                ((SolidColorBrush)host.Picker.FindResource("AccentFillColorSelectedTextBackgroundBrush")).Color,
                highlightBrush.Color
            );
            Assert.NotEqual(brush.Color, highlightBrush.Color);
            var hourColumn = host.Part<Grid>("HoursPickerColumn");
            var minuteColumn = host.Part<Grid>("MinutesPickerColumn");
            var amPmColumn = host.Part<Grid>("AmPmPickerColumn");
            Assert.InRange(Math.Abs(hourColumn.ActualWidth - minuteColumn.ActualWidth), 0, 1);
            Assert.InRange(Math.Abs(hourColumn.ActualWidth - amPmColumn.ActualWidth), 0, 1);
            Assert.DoesNotContain(Descendants<TextBlock>(selection), text => text.Text == ":");
            double selectionCenter = selection.TranslatePoint(new Point(0, 180), border).Y;
            Assert.InRange(Math.Abs(selectionCenter - 181), 0, 1);
            Assert.Equal(PlacementMode.Custom, host.Popup.Placement);
            var placements = host.Popup.CustomPopupPlacementCallback!(
                DeviceSize(border),
                DeviceSize(host.Picker),
                new Point()
            );
            double expected =
                closed.TranslatePoint(new Point(0, closed.ActualHeight / 2), host.Picker).Y - selectionCenter;
            Assert.Equal(expected * VisualTreeHelper.GetDpi(border).DpiScaleY, placements[0].Point.Y, 3);
            Assert.Equal(PopupPrimaryAxis.Vertical, placements[0].PrimaryAxis);
            host.SaveScreenshot($"TimePicker-{theme}");
            double screenCenter = closed.PointToScreen(new Point(0, closed.ActualHeight / 2)).Y;
            double popupCenter = selection.PointToScreen(new Point(0, 180)).Y;
            Assert.True(
                Math.Abs(screenCenter - popupCenter) <= 2,
                $"Screen center={screenCenter}, popup center={popupCenter}, target={closed.RenderSize}, popup={border.RenderSize}, workarea={SystemParameters.WorkArea}, dpi={VisualTreeHelper.GetDpi(closed)}, selectionY={selectionCenter}"
            );
            foreach (string name in new[] { "PART_HoursList", "PART_MinutesList" })
            {
                var list = host.Part<ListBox>(name);
                var viewer = Descendants<ScrollViewer>(list).Single();
                Assert.Equal(360, viewer.ViewportHeight, 3);
                var rows = Descendants<ListBoxItem>(list)
                    .Where(item =>
                    {
                        double y = item.TranslatePoint(new Point(), viewer).Y;
                        return y >= -0.5 && y + item.ActualHeight <= viewer.ActualHeight + 0.5;
                    })
                    .ToArray();
                Assert.Equal(9, rows.Length);
                Assert.All(rows, item => Assert.Equal(40, item.ActualHeight, 3));
                AssertCentered(list);
                var selectedRow = (ListBoxItem)
                    list.ItemContainerGenerator.ContainerFromIndex(list.SelectedIndex);
                var foreground = Assert.IsType<SolidColorBrush>(selectedRow.Foreground);
                var expectedForeground = Assert.IsType<SolidColorBrush>(
                    host.Picker.FindResource("TextOnAccentFillColorSelectedTextBrush")
                );
                Assert.Equal(expectedForeground.Color, foreground.Color);
            }
            foreach (
                var pair in new[]
                {
                    ("HoursTextBlock", hourColumn),
                    ("MinutesTextBlock", minuteColumn),
                    ("AmPmTextBlock", amPmColumn),
                }
            )
            {
                var text = host.Part<TextBlock>(pair.Item1);
                double textCenter = text.PointToScreen(new Point(text.ActualWidth / 2, 0)).X;
                double columnCenter = pair.Item2.PointToScreen(new Point(pair.Item2.ActualWidth / 2, 0)).X;
                Assert.InRange(Math.Abs(textCenter - columnCenter), 0, 2);
            }
            AssertCentered(host.Part<ListBox>("PART_AmPmList"));
            host.SaveScreenshot($"TimePicker-{theme}");
            host.Popup.IsOpen = false;
            Pump();
            host.SaveScreenshot($"TimePicker-{theme}-closed", host.Picker);
            host.Picker.SelectedTime = null;
            Pump();
            host.SaveScreenshot($"TimePicker-{theme}-placeholder", host.Picker);
        });

    [Fact]
    public void ExplicitMidnightAndNull_SynchronizeWithoutLosingPlaceholder() =>
        RunSta(() =>
        {
            using var host = new PickerWindow();
            Assert.Null(host.Picker.SelectedTime);
            Assert.Equal("hour", host.Part<TextBlock>("HoursTextBlock").Text);
            Assert.Equal("minute", host.Part<TextBlock>("MinutesTextBlock").Text);
            host.Picker.Time = TimeSpan.Zero;
            Pump();
            Assert.Equal(TimeSpan.Zero, host.Picker.SelectedTime);
            Assert.Equal("00", host.Part<TextBlock>("HoursTextBlock").Text);
            host.Picker.SelectedTime = new TimeSpan(18, 45, 0);
            Assert.Equal(new TimeSpan(18, 45, 0), host.Picker.Time);
            host.Picker.SelectedTime = null;
            Pump();
            Assert.Null(host.Picker.SelectedTime);
            Assert.Equal(TimeSpan.Zero, host.Picker.Time);
            Assert.Equal("hour", host.Part<TextBlock>("HoursTextBlock").Text);
            host.Picker.PlaceholderText = "Choose a time";
            Pump();
            Assert.Equal(Visibility.Visible, host.Part<TextBlock>("PlaceholderTextBlock").Visibility);
            host.Picker.SetValue(TimePicker.TimeProperty, TimeSpan.Zero);
            Pump();
            Assert.Equal(TimeSpan.Zero, host.Picker.SelectedTime);
            Assert.Equal(Visibility.Collapsed, host.Part<TextBlock>("PlaceholderTextBlock").Visibility);
            host.Picker.SelectedTime = null;
            host.Open();
            host.Click("PART_DismissButton");
            Assert.Null(host.Picker.SelectedTime);
            host.Open();
            host.Click("PART_AcceptButton");
            Assert.Equal(TimeSpan.Zero, host.Picker.SelectedTime);
        });

    [Fact]
    public void SelectedTimeBinding_RemainsAttachedThroughAcceptAndNull() =>
        RunSta(() =>
        {
            using var host = new PickerWindow();
            var source = new TimePicker { SelectedTime = TimeSpan.FromHours(9) };
            BindingOperations.SetBinding(
                host.Picker,
                TimePicker.SelectedTimeProperty,
                new Binding(nameof(TimePicker.SelectedTime)) { Source = source, Mode = BindingMode.TwoWay }
            );
            Pump();
            Assert.Equal(source.SelectedTime, host.Picker.Time);
            host.Open();
            host.Click("PART_HoursDownButton");
            host.Click("PART_AcceptButton");
            Assert.Equal(TimeSpan.FromHours(10), source.SelectedTime);
            Assert.True(BindingOperations.IsDataBound(host.Picker, TimePicker.SelectedTimeProperty));
            source.SelectedTime = null;
            Pump();
            Assert.Null(host.Picker.SelectedTime);
            host.Picker.SetCurrentValue(TimePicker.TimeProperty, TimeSpan.Zero);
            Pump();
            Assert.Equal(TimeSpan.Zero, source.SelectedTime);
        });

    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(15)]
    [InlineData(30)]
    [InlineData(60)]
    public void Arrows_WrapHoursAndMinutesAndKeepSelectionCentered(int increment) =>
        RunSta(() =>
        {
            using var host = new PickerWindow();
            host.Picker.MinuteIncrement = increment;
            host.Picker.SelectedTime = TimeSpan.Zero;
            host.Open();
            host.Click("PART_HoursUpButton");
            host.Click("PART_MinutesUpButton");
            Assert.Equal("23", host.Part<ListBox>("PART_HoursList").SelectedItem);
            Assert.Equal(
                (60 - increment).ToString("D2"),
                host.Part<ListBox>("PART_MinutesList").SelectedItem
            );
            AssertCentered(host.Part<ListBox>("PART_HoursList"));
            AssertCentered(host.Part<ListBox>("PART_MinutesList"));
            Assert.Equal(TimeSpan.Zero, host.Picker.SelectedTime);
            host.Click("PART_AcceptButton");
            Assert.Equal(new TimeSpan(23, 60 - increment, 0), host.Picker.SelectedTime);
            Assert.False(host.Popup.IsOpen);
            host.Open();
            host.Click("PART_HoursDownButton");
            host.Click("PART_MinutesDownButton");
            host.Click("PART_AcceptButton");
            Assert.Equal(TimeSpan.Zero, host.Picker.SelectedTime);
        });

    [Fact]
    public void AmPmArrowsAndRowSelection_CenterAndCommitNoonAndMidnight() =>
        RunSta(() =>
        {
            using var host = new PickerWindow();
            host.Picker.ClockIdentifier = ClockIdentifier.Clock12Hour;
            host.Picker.SelectedTime = TimeSpan.Zero;
            host.Open();
            var list = host.Part<ListBox>("PART_AmPmList");
            Assert.Equal(2, list.Items.Count);
            Assert.Equal("12", host.Part<ListBox>("PART_HoursList").SelectedItem);
            AssertCentered(list);
            host.Click("PART_AmPmUpButton");
            Assert.Equal("PM", list.SelectedItem);
            AssertCentered(list);
            Assert.Equal(40, Descendants<ScrollViewer>(list).Single().VerticalOffset, 3);
            host.Click("PART_AcceptButton");
            Assert.Equal(TimeSpan.FromHours(12), host.Picker.SelectedTime);
            host.Open();
            host.Click("PART_AmPmDownButton");
            Assert.Equal("AM", list.SelectedItem);
            AssertCentered(list);
            list.SelectedIndex = 1; // Same SelectionChanged route as clicking a row.
            Pump();
            AssertCentered(list);
            list.SelectedIndex = 0;
            Pump();
            host.Click("PART_AcceptButton");
            Assert.Equal(TimeSpan.Zero, host.Picker.SelectedTime);
        });

    [Fact]
    public void ClickedRowsAndWheelScroll_RecenterAndCancelRestoresCommittedTime() =>
        RunSta(() =>
        {
            using var host = new PickerWindow();
            host.Picker.SelectedTime = new TimeSpan(10, 20, 0);
            host.Open();
            var hours = host.Part<ListBox>("PART_HoursList");
            var minutes = host.Part<ListBox>("PART_MinutesList");
            (
                (ListBoxItem)hours.ItemContainerGenerator.ContainerFromIndex(hours.SelectedIndex + 2)
            ).IsSelected = true;
            Pump();
            AssertCentered(hours);
            var viewer = Descendants<ScrollViewer>(minutes).Single();
            viewer.ScrollToVerticalOffset(viewer.VerticalOffset + 80);
            Pump(300);
            Assert.Equal("22", minutes.SelectedItem);
            AssertCentered(minutes);
            host.Click("PART_DismissButton");
            Assert.Equal(new TimeSpan(10, 20, 0), host.Picker.SelectedTime);
            for (int i = 0; i < 3; i++)
            {
                host.Open();
                Assert.Equal("10", hours.SelectedItem);
                Assert.Equal("20", minutes.SelectedItem);
                viewer.ScrollToVerticalOffset(viewer.VerticalOffset + 40);
                Pump(30); // Close with debounce still pending.
                host.Popup.IsOpen = false;
                Pump(250);
            }
            host.Open();
            Assert.Equal("20", minutes.SelectedItem);
            AssertCentered(minutes);
            host.Click("PART_AcceptButton");
            Assert.Equal(new TimeSpan(10, 20, 0), host.Picker.SelectedTime);
        });

    [Fact]
    public void OpenPopup_TracksExternalSelectionAndClockChanges() =>
        RunSta(() =>
        {
            using var host = new PickerWindow();
            host.Picker.SelectedTime = new TimeSpan(18, 42, 0);
            host.Open();
            host.Picker.ClockIdentifier = ClockIdentifier.Clock12Hour;
            host.Picker.MinuteIncrement = 15;
            Pump();
            Assert.Equal("6", host.Part<ListBox>("PART_HoursList").SelectedItem);
            Assert.Equal("PM", host.Part<ListBox>("PART_AmPmList").SelectedItem);
            Assert.Equal("30", host.Part<ListBox>("PART_MinutesList").SelectedItem);
            host.Picker.SelectedTime = TimeSpan.Zero;
            Pump();
            Assert.Equal("12", host.Part<ListBox>("PART_HoursList").SelectedItem);
            Assert.Equal("AM", host.Part<ListBox>("PART_AmPmList").SelectedItem);
            AssertCentered(host.Part<ListBox>("PART_AmPmList"));
            host.Picker.SelectedTime = null;
            Pump();
            host.Click("PART_DismissButton");
            Assert.Null(host.Picker.SelectedTime);
        });

    private static Size DeviceSize(FrameworkElement element)
    {
        DpiScale dpi = VisualTreeHelper.GetDpi(element);
        return new Size(element.ActualWidth * dpi.DpiScaleX, element.ActualHeight * dpi.DpiScaleY);
    }

    private static void AssertCentered(ListBox list)
    {
        var viewer = Descendants<ScrollViewer>(list).Single();
        var item = Assert.IsType<ListBoxItem>(
            list.ItemContainerGenerator.ContainerFromIndex(list.SelectedIndex)
        );
        double center = item.TranslatePoint(new Point(0, item.ActualHeight / 2), viewer).Y;
        Assert.InRange(Math.Abs(center - viewer.ActualHeight / 2), 0, 1);
    }

    [Fact]
    public void CustomPlacement_MeasuresSelectionAreaInsteadOfHardCodingDefaultHeight() =>
        RunSta(() =>
        {
            using var host = new PickerWindow();
            host.Open();
            var selection = host.Part<Grid>("PickerSelectionGrid");
            var border = host.Part<Border>("PopupBorder");
            selection.Height = 280;
            border.BorderThickness = new Thickness(3);
            border.UpdateLayout();
            Pump();
            var placement = host.Popup.CustomPopupPlacementCallback!(
                DeviceSize(border),
                DeviceSize(host.Picker),
                new Point(5, 7)
            )[0];
            var closed = host.Part<Border>("ContentBorder");
            double targetCenter = closed.TranslatePoint(new Point(0, closed.ActualHeight / 2), host.Picker).Y;
            double selectionCenter = selection
                .TranslatePoint(new Point(0, selection.ActualHeight / 2), border)
                .Y;
            Assert.Equal(
                (targetCenter - selectionCenter + 7) * VisualTreeHelper.GetDpi(border).DpiScaleY,
                placement.Point.Y,
                3
            );
            Assert.Equal(5 * VisualTreeHelper.GetDpi(border).DpiScaleX, placement.Point.X, 3);
        });

    [Fact]
    public void ReapplyingTemplate_DetachesOldArrowsAndRestartsScrolling() =>
        RunSta(() =>
        {
            using var host = new PickerWindow();
            host.Picker.SelectedTime = TimeSpan.FromHours(8);
            host.Open();
            var oldPopup = host.Popup;
            var oldArrow = host.Part<Button>("PART_HoursDownButton");
            var template = host.Picker.Template;
            host.Picker.Template = null;
            host.Picker.ApplyTemplate();
            Pump();
            host.Picker.Template = template;
            host.Picker.ApplyTemplate();
            Pump();
            Assert.False(oldPopup.IsOpen);
            host.Open();
            oldArrow.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
            Pump();
            Assert.Equal("08", host.Part<ListBox>("PART_HoursList").SelectedItem);
            host.Click("PART_HoursDownButton");
            Assert.Equal("09", host.Part<ListBox>("PART_HoursList").SelectedItem);
            AssertCentered(host.Part<ListBox>("PART_HoursList"));
            host.Click("PART_AcceptButton");
            Assert.Equal(TimeSpan.FromHours(9), host.Picker.SelectedTime);
        });

    private static IEnumerable<T> Descendants<T>(DependencyObject root)
        where T : DependencyObject
    {
        for (int i = 0; i < VisualTreeHelper.GetChildrenCount(root); i++)
        {
            var child = VisualTreeHelper.GetChild(root, i);
            if (child is T match)
                yield return match;
            foreach (var nested in Descendants<T>(child))
                yield return nested;
        }
    }

    private static void Pump(int milliseconds = 60)
    {
        var frame = new DispatcherFrame();
        var timer = new DispatcherTimer(DispatcherPriority.ApplicationIdle)
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

    private static void RunSta(Action action)
    {
        ExceptionDispatchInfo? failure = null;
        var thread = new Thread(() =>
        {
            try
            {
                action();
            }
            catch (Exception exception)
            {
                failure = ExceptionDispatchInfo.Capture(exception);
            }
            finally
            {
                Dispatcher.CurrentDispatcher.InvokeShutdown();
            }
        })
        {
            IsBackground = true,
        };
        thread.SetApartmentState(ApartmentState.STA);
        thread.Start();
        Assert.True(thread.Join(TimeSpan.FromSeconds(30)), "STA test timed out.");
        failure?.Throw();
    }

    private sealed class TestPicker : TimePicker
    {
        public void Open() => OnClick();
    }

    private sealed class PickerWindow : IDisposable
    {
        private readonly Window _window;
        public TestPicker Picker { get; } =
            new()
            {
                Width = 300,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
            };
        public Popup Popup => Part<Popup>("PART_Popup");

        public PickerWindow(ApplicationTheme theme = ApplicationTheme.Light)
        {
            _window = new Window
            {
                Width = 600,
                Height = 650,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                ShowInTaskbar = false,
                Content = Picker,
            };
            _window.Resources.MergedDictionaries.Add(new ThemesDictionary { Theme = theme });
            _window.Resources.MergedDictionaries.Add(new ControlsDictionary());
            UiApplication.Current.Resources = _window.Resources;
            ApplicationAccentColorManager.Apply(Color.FromRgb(0, 120, 212), theme);
            Picker.Style = (Style)_window.Resources[typeof(TimePicker)];
            _window.Show();
            _window.UpdateLayout();
            Picker.ApplyTemplate();
            Pump();
        }

        public T Part<T>(string name)
            where T : DependencyObject => Assert.IsAssignableFrom<T>(Picker.Template.FindName(name, Picker));

        public void Open()
        {
            Picker.Open();
            Pump(100);
            Assert.True(Popup.IsOpen);
        }

        public void Click(string name)
        {
            Part<Button>(name).RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
            Pump();
        }

        // Opt in with WPFUI_TIMEPICKER_SCREENSHOTS=<directory>; no screenshot dependencies.
        public void SaveScreenshot(string name, FrameworkElement? element = null)
        {
            string? directory = Environment.GetEnvironmentVariable("WPFUI_TIMEPICKER_SCREENSHOTS");
            if (string.IsNullOrWhiteSpace(directory))
                return;
            Directory.CreateDirectory(directory);
            var visual = element ?? (FrameworkElement)Popup.Child;
            var bitmap = new RenderTargetBitmap(
                (int)Math.Ceiling(visual.ActualWidth),
                (int)Math.Ceiling(visual.ActualHeight),
                96,
                96,
                PixelFormats.Pbgra32
            );
            var drawing = new DrawingVisual();
            using (DrawingContext context = drawing.RenderOpen())
            {
                context.DrawRectangle(
                    (Brush)_window.FindResource("SolidBackgroundFillColorBaseBrush"),
                    null,
                    new Rect(0, 0, visual.ActualWidth, visual.ActualHeight)
                );
                context.DrawRectangle(
                    new VisualBrush(visual),
                    null,
                    new Rect(0, 0, visual.ActualWidth, visual.ActualHeight)
                );
            }
            bitmap.Render(drawing);
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmap));
            using var stream = File.Create(Path.Combine(directory, name + ".png"));
            encoder.Save(stream);
        }

        public void Dispose()
        {
            Popup.IsOpen = false;
            _window.Close();
            Pump();
        }
    }
}
