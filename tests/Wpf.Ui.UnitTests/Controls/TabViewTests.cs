// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using Wpf.Ui.Controls;
using Button = System.Windows.Controls.Button;

namespace Wpf.Ui.UnitTests.Controls;

public class TabViewTests
{
    [Fact]
    public void Drop_UsesModelIdentityAndRejectsOtherViews() =>
        RunSta(() =>
        {
            var first = new Document("same");
            var second = new Document("same");
            var third = new Document("target");
            var models = new ObservableCollection<Document> { first, second, third };
            TabView view = CreateView();
            view.ItemsSource = models;
            Layout(view);
            TabView other = CreateView();
            var target = (TabViewItem)view.ItemContainerGenerator.ContainerFromIndex(2);
            Invoke(view, "OnDrop", CreateDrop(other, second, view, target));
            Assert.Same(second, models[1]);
            view.SelectedItem = second;
            SetDragState(view, (TabViewItem)view.ItemContainerGenerator.ContainerFromIndex(1));
            Invoke(view, "OnDrop", CreateDrop(view, second, view, target));
            Assert.Same(first, models[0]);
            Assert.Same(third, models[1]);
            Assert.Same(second, models[2]);
            Assert.Same(second, view.SelectedItem);
        });

    [Fact]
    public void DropFailureAndInvalidation_ClearDragState() =>
        RunSta(() =>
        {
            var models = new ThrowingCollection { new("one"), new("two") };
            TabView view = CreateView();
            view.ItemsSource = models;
            Layout(view);
            var tab = (TabViewItem)view.ItemContainerGenerator.ContainerFromIndex(0);
            var target = (TabViewItem)view.ItemContainerGenerator.ContainerFromIndex(1);
            SetDragState(view, tab);
            Assert.Throws<InvalidOperationException>(() =>
                Invoke(view, "OnDrop", CreateDrop(view, models[0], view, target))
            );
            AssertDragCleared(view);
            SetDragState(view, tab);
            view.CanReorderTabs = false;
            AssertDragCleared(view);
            view.CanReorderTabs = true;
            DragEventArgs cancelledDrop = CreateDrop(view, models[0], view, target);
            Invoke(view, "OnDrop", cancelledDrop);
            Assert.Equal(DragDropEffects.None, cancelledDrop.Effects);
            Assert.Equal("one", models[0].Header);
            SetDragState(view, tab);
            view.OnApplyTemplate();
            AssertDragCleared(view);
            SetDragState(view, tab);
            view.RaiseEvent(new RoutedEventArgs(FrameworkElement.UnloadedEvent));
            AssertDragCleared(view);
            SetDragState(view, tab);
            view.ItemsSource = new ObservableCollection<Document>();
            AssertDragCleared(view);
        });

    [Fact]
    public void NonNotifyingListsAndValueModels_AreUpdatedSafely() =>
        RunSta(() =>
        {
            var models = new List<int> { 1, 2 };
            TabView view = CreateView();
            view.ItemsSource = models;
            Layout(view);
            Reorder(view, 0, 1);
            Assert.Equal(new[] { 2, 1 }, view.Items.Cast<int>().ToArray());
            Layout(view);
            Close((TabViewItem)view.ItemContainerGenerator.ContainerFromIndex(0));
            Assert.Equal(new[] { 1 }, models);
            view.TabAdding += (_, e) => e.Item = 3;
            view.AddTab();
            Assert.Equal(new[] { 1, 3 }, view.Items.Cast<int>().ToArray());
            Assert.Equal(3, view.SelectedItem);
        });

    [Fact]
    public void Items_AddCloseAndReorder_PreserveSelectionAndCancellation() =>
        RunSta(() =>
        {
            TabView view = CreateView();
            view.AddTab("first content", "first");
            view.AddTab("second content", "second");
            TabViewItem second = Assert.IsType<TabViewItem>(view.SelectedItem);
            TabViewItem first = Assert.IsType<TabViewItem>(view.Items[0]);
            Assert.Equal("second content", second.Content);
            Layout(view);
            Reorder(view, 0, 1);
            Assert.Same(first, view.Items[1]);
            Assert.Same(second, view.SelectedItem);
            int closes = 0;
            EventHandler<TabViewItemClosingEventArgs> cancel = (_, e) =>
            {
                closes++;
                e.Cancel = true;
            };
            view.TabClosing += cancel;
            Close(second);
            Assert.Equal(2, view.Items.Count);
            Assert.Equal(1, closes);
            view.TabClosing -= cancel;
            Close(second);
            Assert.Single(view.Items);
            Assert.Same(first, view.SelectedItem);
            first.IsClosable = false;
            Close(first);
            Assert.Single(view.Items);
            view.TabAdding += (_, e) => e.Cancel = true;
            view.AddTab();
            Assert.Single(view.Items);
        });

    [Fact]
    public void Models_UseGeneratedContainersAndMoveNotificationWithoutOverwritingUserSettings() =>
        RunSta(() =>
        {
            var models = new ObservableCollection<Document> { new("one"), new("two"), new("three") };
            TabView view = CreateView();
            var headerTemplate = new DataTemplate();
            var style = new Style(typeof(TabViewItem), (Style)view.Resources[typeof(TabViewItem)]);
            style.Setters.Add(
                new Setter(HeaderedContentControl.HeaderProperty, new Binding(nameof(Document.Header)))
            );
            view.ItemTemplate = headerTemplate;
            view.ItemContainerStyle = style;
            view.ItemsSource = models;
            view.SetBinding(
                System.Windows.Controls.Primitives.Selector.SelectedItemProperty,
                new Binding(nameof(Selection.Item))
                {
                    Source = new Selection { Item = models[1] },
                    Mode = BindingMode.TwoWay,
                }
            );
            Document selected = models[1];
            Layout(view);
            TabViewItem container = Assert.IsType<TabViewItem>(
                view.ItemContainerGenerator.ContainerFromIndex(0)
            );
            container.Tag = "user tag";
            var changes = new List<NotifyCollectionChangedAction>();
            models.CollectionChanged += (_, e) => changes.Add(e.Action);
            Reorder(view, 0, 2);
            Assert.Equal(new[] { NotifyCollectionChangedAction.Move }, changes);
            Assert.Same(selected, view.SelectedItem);
            Assert.True(
                BindingOperations.IsDataBound(
                    view,
                    System.Windows.Controls.Primitives.Selector.SelectedItemProperty
                )
            );
            Assert.Same(headerTemplate, view.ItemTemplate);
            Assert.Same(style, view.ItemContainerStyle);
            Assert.Equal("user tag", container.Tag);
            Layout(view);
            object? closedItem = null;
            view.TabClosing += (_, e) => closedItem = e.Item;
            Close((TabViewItem)view.ItemContainerGenerator.ContainerFromIndex(0));
            Assert.Same(selected, closedItem);
            Assert.DoesNotContain(selected, models);
            Assert.Contains(view.SelectedItem, models);
            var added = new Document("new");
            view.TabAdding += (_, e) => e.Item = added;
            view.AddTab();
            Assert.Same(added, models[^1]);
            Assert.Same(added, view.SelectedItem);
        });

    [Fact]
    public void ModelCollection_DoesNotReceiveAnImplicitContainerOrWrongType() =>
        RunSta(() =>
        {
            var models = new ObservableCollection<Document> { new("one") };
            TabView view = CreateView();
            view.ItemsSource = models;
            view.AddTab();
            view.TabAdding += (_, e) => e.TabItem = new TabViewItem();
            view.AddTab();
            Assert.Single(models);
        });

    [Fact]
    public void ReadOnlyFixedSizeAndSortedSources_AreNotAutomaticallyChanged() =>
        RunSta(() =>
        {
            foreach (
                IEnumerable source in new IEnumerable[]
                {
                    new ReadOnlyObservableCollection<Document>(
                        new ObservableCollection<Document> { new("one"), new("two") }
                    ),
                    new[] { new Document("one"), new Document("two") },
                    new ListCollectionView(new ObservableCollection<Document> { new("one"), new("two") })
                    {
                        SortDescriptions =
                        {
                            new SortDescription(nameof(Document.Header), ListSortDirection.Descending),
                        },
                    },
                    new ListCollectionView(new ObservableCollection<Document> { new("one"), new("two") })
                    {
                        Filter = _ => true,
                    },
                }
            )
            {
                TabView view = CreateView();
                view.ItemsSource = source;
                Layout(view);
                object[] original = view.Items.Cast<object>().ToArray();
                view.TabAdding += (_, e) => e.Item = new Document("new");
                view.AddTab();
                Reorder(view, 0, 1);
                Close((TabViewItem)view.ItemContainerGenerator.ContainerFromIndex(0));
                Assert.Equal(original, view.Items.Cast<object>().ToArray());
            }
        });

    [Fact]
    public void HandlersAndReentrantCalls_DoNotPerformDuplicateOperations() =>
        RunSta(() =>
        {
            var models = new ObservableCollection<Document> { new("one"), new("two") };
            TabView view = CreateView();
            view.ItemsSource = models;
            int adds = 0;
            view.TabAdding += (_, e) =>
            {
                adds++;
                view.AddTab();
                e.Item = new Document("handler added");
                models.Add((Document)e.Item);
                e.Cancel = true;
            };
            view.AddTab();
            Assert.Equal(1, adds);
            Assert.Equal(3, models.Count);
            Layout(view);
            int closes = 0;
            view.TabClosing += (_, e) =>
            {
                closes++;
                models.Remove((Document)e.Item!);
                e.Cancel = true;
            };
            Close((TabViewItem)view.ItemContainerGenerator.ContainerFromIndex(0));
            Assert.Equal(1, closes);
            Assert.Equal(2, models.Count);
        });

    [Fact]
    public void ReapplyingTemplates_DetachesOldButtonsAndAttachesNewButtonsExactlyOnce() =>
        RunSta(() =>
        {
            TabView view = CreateView();
            view.Template = AddTemplate();
            Layout(view);
            var oldAdd = (Button)view.Template.FindName("PART_AddButton", view);
            view.OnApplyTemplate();
            int adds = 0;
            view.TabAdding += (_, e) =>
            {
                adds++;
                e.Cancel = true;
            };
            oldAdd.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            Assert.Equal(1, adds);
            view.Template = AddTemplate();
            Layout(view);
            oldAdd.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            Assert.Equal(1, adds);
            var newAdd = (Button)view.Template.FindName("PART_AddButton", view);
            newAdd.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            Assert.Equal(2, adds);
            view.CanAddTabs = false;
            newAdd.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            Assert.Equal(2, adds);

            var item = new TabViewItem { Template = CloseTemplate() };
            Layout(item);
            var oldClose = (Button)item.Template.FindName("PART_CloseButton", item);
            item.OnApplyTemplate();
            int closes = 0;
            item.CloseRequested += (_, _) => closes++;
            oldClose.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            Assert.Equal(1, closes);
            item.Template = CloseTemplate();
            Layout(item);
            oldClose.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            Assert.Equal(1, closes);
            var newClose = (Button)item.Template.FindName("PART_CloseButton", item);
            newClose.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            Assert.Equal(2, closes);
            item.IsClosable = false;
            newClose.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            Assert.Equal(2, closes);
            item.IsClosable = true;
            newClose.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            Assert.Equal(3, closes);
        });

    [Fact]
    public void RemovedContainersAndReloadedViews_DoNotRetainDuplicateHandlers() =>
        RunSta(() =>
        {
            TabView view = CreateView();
            var item = new TabViewItem();
            view.Items.Add(item);
            Layout(view);
            int closes = 0;
            view.TabClosing += (_, e) =>
            {
                closes++;
                e.Cancel = true;
            };
            view.RaiseEvent(new RoutedEventArgs(FrameworkElement.UnloadedEvent));
            Close(item);
            Assert.Equal(0, closes);
            view.RaiseEvent(new RoutedEventArgs(FrameworkElement.LoadedEvent));
            view.RaiseEvent(new RoutedEventArgs(FrameworkElement.LoadedEvent));
            Close(item);
            Assert.Equal(1, closes);
            view.Items.Remove(item);
            Layout(view);
            Assert.Null(
                typeof(TabViewItem)
                    .GetField("CloseRequested", BindingFlags.NonPublic | BindingFlags.Instance)!
                    .GetValue(item)
            );
            Close(item);
            Assert.Equal(1, closes);
            view.Items.Add(item);
            Layout(view);
            Close(item);
            Assert.Equal(2, closes);
        });

    [Fact]
    public void ReorderException_RestoresSelectionAndAllowsLaterOperations() =>
        RunSta(() =>
        {
            var models = new ThrowingCollection { new("one"), new("two") };
            TabView view = CreateView();
            view.ItemsSource = models;
            view.SelectedItem = models[1];
            object selected = view.SelectedItem;
            Assert.Throws<InvalidOperationException>(() => Reorder(view, 0, 1));
            Assert.Same(selected, view.SelectedItem);
            models.ThrowOnMove = false;
            Reorder(view, 0, 1);
            Assert.Equal("two", models[0].Header);
            Assert.Same(selected, view.SelectedItem);
        });

    [Fact]
    public void DefaultStyles_SeparateStandardTabsAndDocumentTabs() =>
        RunSta(() =>
        {
            TabView resources = CreateView();
            var standard = new TabControl { Resources = resources.Resources };
            standard.Items.Add(new TabItem { Header = "standard" });
            Layout(standard);
            Assert.Null(standard.Template.FindName("AddButton", standard));
            Assert.Null(standard.Template.FindName("PART_AddButton", standard));
            var standardItem = (TabItem)standard.ItemContainerGenerator.ContainerFromIndex(0);
            Assert.Null(standardItem.Template.FindName("CloseButton", standardItem));
            Assert.Null(standardItem.Template.FindName("PART_CloseButton", standardItem));
            Assert.NotNull(standard.Template.FindName("PART_SelectedContentHost", standard));
            var document = new TabViewItem { Header = "document", IsClosable = false };
            resources.Items.Add(document);
            Layout(resources);
            Assert.NotNull(resources.Template.FindName("PART_AddButton", resources));
            var close = (Button)document.Template.FindName("PART_CloseButton", document);
            Assert.Equal(Visibility.Collapsed, close.Visibility);
            document.IsClosable = true;
            Assert.Equal(Visibility.Visible, close.Visibility);
        });

    private static TabView CreateView()
    {
        var view = new TabView();
        view.Resources.MergedDictionaries.Add(
            new ResourceDictionary
            {
                Source = new Uri("pack://application:,,,/Wpf.Ui;component/Resources/Theme/Light.xaml"),
            }
        );
        view.Resources.MergedDictionaries.Add(
            new ResourceDictionary
            {
                Source = new Uri("pack://application:,,,/Wpf.Ui;component/Resources/Wpf.Ui.xaml"),
            }
        );
        return view;
    }

    private static void Layout(FrameworkElement element)
    {
        element.Measure(new Size(800, 400));
        element.Arrange(new Rect(0, 0, 800, 400));
        element.UpdateLayout();
    }

    private static ControlTemplate AddTemplate() =>
        Assert.IsType<ControlTemplate>(
            XamlReader.Parse(
                "<ControlTemplate xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml'><Button x:Name='PART_AddButton'/></ControlTemplate>"
            )
        );

    private static ControlTemplate CloseTemplate() =>
        Assert.IsType<ControlTemplate>(
            XamlReader.Parse(
                "<ControlTemplate xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml'><Button x:Name='PART_CloseButton'/></ControlTemplate>"
            )
        );

    private static void Close(TabViewItem item) => Invoke(item, "OnCloseRequested");

    private static DragEventArgs CreateDrop(
        TabView owner,
        object item,
        TabView targetView,
        TabViewItem target
    )
    {
        Type payloadType = typeof(TabView).GetNestedType("TabDragData", BindingFlags.NonPublic)!;
        object payload = Activator.CreateInstance(payloadType, owner, item)!;
        Point point = target
            .TransformToAncestor(targetView)
            .Transform(new Point(target.ActualWidth / 2, target.ActualHeight / 2));
        DragEventArgs args = Assert.IsType<DragEventArgs>(
            Activator.CreateInstance(
                typeof(DragEventArgs),
                BindingFlags.NonPublic | BindingFlags.Instance,
                null,
                new object[]
                {
                    new DataObject(payload),
                    DragDropKeyStates.None,
                    DragDropEffects.Move,
                    targetView,
                    point,
                },
                null
            )
        );
        args.RoutedEvent = DragDrop.DropEvent;
        args.Source = target;
        return args;
    }

    private static void SetDragState(TabView view, TabViewItem item)
    {
        typeof(TabView)
            .GetField("_draggedTab", BindingFlags.NonPublic | BindingFlags.Instance)!
            .SetValue(view, item);
        typeof(TabView)
            .GetField("_isDragging", BindingFlags.NonPublic | BindingFlags.Instance)!
            .SetValue(view, true);
    }

    private static void AssertDragCleared(TabView view)
    {
        Assert.Null(
            typeof(TabView)
                .GetField("_draggedTab", BindingFlags.NonPublic | BindingFlags.Instance)!
                .GetValue(view)
        );
        Assert.False(
            Assert.IsType<bool>(
                typeof(TabView)
                    .GetField("_isDragging", BindingFlags.NonPublic | BindingFlags.Instance)!
                    .GetValue(view)
            )
        );
    }

    private static void Reorder(TabView view, int oldIndex, int newIndex) =>
        Invoke(view, "ReorderTabItem", oldIndex, newIndex);

    private static void Invoke(object instance, string method, params object[] args)
    {
        try
        {
            instance
                .GetType()
                .GetMethod(method, BindingFlags.NonPublic | BindingFlags.Instance)!
                .Invoke(instance, args);
        }
        catch (TargetInvocationException e) when (e.InnerException != null)
        {
            ExceptionDispatchInfo.Capture(e.InnerException).Throw();
        }
    }

    private static void RunSta(Action action)
    {
        Exception? error = null;
        var thread = new Thread(() =>
        {
            try
            {
                action();
            }
            catch (Exception e)
            {
                error = e;
            }
            finally
            {
                System.Windows.Threading.Dispatcher.CurrentDispatcher.InvokeShutdown();
            }
        });
        thread.SetApartmentState(ApartmentState.STA);
        thread.Start();
        thread.Join();
        if (error != null)
        {
            ExceptionDispatchInfo.Capture(error).Throw();
        }
    }

    private sealed record Document(string Header);

    private sealed class Selection
    {
        public object? Item { get; set; }
    }

    private sealed class ThrowingCollection : ObservableCollection<Document>
    {
        public bool ThrowOnMove { get; set; } = true;

        protected override void MoveItem(int oldIndex, int newIndex)
        {
            if (ThrowOnMove)
            {
                throw new InvalidOperationException("Move rejected");
            }

            base.MoveItem(oldIndex, newIndex);
        }
    }
}
