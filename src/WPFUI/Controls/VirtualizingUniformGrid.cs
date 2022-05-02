// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Collections;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

// TODO: For now, this is a rude implementation to try

namespace WPFUI.Controls
{
    /// <summary>
    /// Tries to asynchronously create a large list of controls in the selected number of columns, and then virtualize it to speed up the UI performance.
    /// <para><c>Work in progress.</c></para>
    /// </summary>
    [Obsolete]
    public class VirtualizingUniformGrid : System.Windows.Controls.Control
    {
        private bool _compiled = false;

        /// <summary>
        /// Property for <see cref="IsVirtualizing"/>.
        /// </summary>
        public static readonly DependencyProperty IsVirtualizingProperty = DependencyProperty.Register(
            nameof(IsVirtualizing),
            typeof(bool), typeof(VirtualizingUniformGrid), new PropertyMetadata(true));

        /// <summary>
        /// Property for <see cref="VirtualizationMode"/>.
        /// </summary>
        public static readonly DependencyProperty VirtualizationModeProperty = DependencyProperty.Register(
            nameof(VirtualizationMode),
            typeof(VirtualizationMode), typeof(VirtualizingUniformGrid),
            new PropertyMetadata(VirtualizationMode.Standard));

        /// <summary>
        /// Property for <see cref="CanScroll"/>.
        /// </summary>
        public static readonly DependencyProperty CanScrollProperty = DependencyProperty.Register(
            nameof(CanScroll),
            typeof(bool), typeof(VirtualizingUniformGrid), new PropertyMetadata(true));

        /// <summary>
        /// Property for <see cref="Columns"/>.
        /// </summary>
        public static readonly DependencyProperty ColumnsProperty = DependencyProperty.Register(nameof(Columns),
            typeof(int), typeof(VirtualizingUniformGrid), new PropertyMetadata(1));

        /// <summary>
        /// Property for <see cref="ItemsSource"/>.
        /// </summary>
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(nameof(ItemsSource),
            typeof(IEnumerable), typeof(VirtualizingUniformGrid),
            new PropertyMetadata((IEnumerable)null, OnItemsSourceChanged));

        /// <summary>
        /// Property for <see cref="Content"/>.
        /// </summary>
        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register(nameof(Content),
            typeof(object), typeof(VirtualizingUniformGrid), new PropertyMetadata(null));

        /// <summary>
        /// Property for <see cref="ItemTemplate"/>.
        /// </summary>
        public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.Register(
            nameof(ItemTemplate),
            typeof(object), typeof(VirtualizingUniformGrid), new PropertyMetadata((DataTemplate)null,
                OnItemTemplateChanged));

        /// <summary>
        /// Property for <see cref="HorizontalScrollBarVisibility"/>.
        /// </summary>
        public static readonly DependencyProperty HorizontalScrollBarVisibilityProperty = DependencyProperty.Register(
            nameof(HorizontalScrollBarVisibility),
            typeof(ScrollBarVisibility), typeof(VirtualizingUniformGrid),
            new PropertyMetadata(ScrollBarVisibility.Disabled));

        /// <summary>
        /// Property for <see cref="VerticalScrollBarVisibility"/>.
        /// </summary>
        public static readonly DependencyProperty VerticalScrollBarVisibilityProperty = DependencyProperty.Register(
            nameof(VerticalScrollBarVisibility),
            typeof(ScrollBarVisibility), typeof(VirtualizingUniformGrid),
            new PropertyMetadata(ScrollBarVisibility.Visible));

        /// <summary>
        /// Turns virtualization on or fff.
        /// </summary>
        public bool IsVirtualizing
        {
            get => (bool)GetValue(IsVirtualizingProperty);
            set => SetValue(IsVirtualizingProperty, value);
        }

        /// <summary>
        /// Gets or sets the virtualization mode.
        /// </summary>
        public VirtualizationMode VirtualizationMode
        {
            get => (VirtualizationMode)GetValue(VirtualizationModeProperty);
            set => SetValue(VirtualizationModeProperty, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the content can be scrolled.
        /// </summary>
        public bool CanScroll
        {
            get => (bool)GetValue(CanScrollProperty);
            set => SetValue(CanScrollProperty, value);
        }

        /// <summary>
        /// Gets or sets number of grid columns.
        /// </summary>
        public int Columns
        {
            get => (int)GetValue(ColumnsProperty);
            set => SetValue(ColumnsProperty, value);
        }

        /// <summary>
        /// Gets or sets source of the presented items.
        /// </summary>
        public IEnumerable ItemsSource
        {
            set
            {
                if (value == null)
                    ClearValue(ItemsSourceProperty);
                else
                    SetValue(ItemsSourceProperty, value);
            }

            internal get => (IEnumerable)GetValue(ItemsSourceProperty);
        }

        /// <summary>
        /// Gets or sets template of the displayed items.
        /// </summary>
        public DataTemplate ItemTemplate
        {
            get => (DataTemplate)GetValue(ItemTemplateProperty);
            set => SetValue(ItemTemplateProperty, value);
        }

        /// <summary>
        /// Gets the displayed content.
        /// </summary>
        public object Content
        {
            get => GetValue(ContentProperty);
            internal set => SetValue(ContentProperty, value);
        }

        /// <summary>
        /// Gets the displayed content.
        /// </summary>
        public ScrollBarVisibility HorizontalScrollBarVisibility
        {
            get => (ScrollBarVisibility)GetValue(HorizontalScrollBarVisibilityProperty);
            set => SetValue(HorizontalScrollBarVisibilityProperty, value);
        }

        /// <summary>
        /// Gets the displayed content.
        /// </summary>
        public ScrollBarVisibility VerticalScrollBarVisibility
        {
            get => (ScrollBarVisibility)GetValue(VerticalScrollBarVisibilityProperty);
            set => SetValue(VerticalScrollBarVisibilityProperty, value);
        }

        /// <summary>
        /// This virtual method is invoked when <see cref="ItemsSource"/> is changed.
        /// </summary>
        protected virtual async Task OnItemsSourceChanged()
        {
            _compiled = false;

            await Compile();
        }

        /// <summary>
        /// This virtual method is invoked when <see cref="ItemTemplate"/> is changed.
        /// </summary>
        protected virtual async Task OnItemTemplateChanged()
        {
        }

        /// <summary>
        /// Compiles all <see cref="VirtualizingUniformGrid"/> items on the code side, puts <see cref="ScrollViewer"/> into <see cref="Content"/> then adds <see cref="ItemsSource"/> to the <see cref="VirtualizingStackPanel"/>.
        /// </summary>
        // Version 5 - Single, virtualized stack panel with grid groups
        private async Task Compile()
        {
            if (_compiled)
                return;

            var itemsSource = ItemsSource;
            var template = ItemTemplate;
            var columns = Columns;

            if (itemsSource == null)
            {
                Content = null;

                return;
            }

            // It's a mess, please don't judge me
            // This control was written quickly to display 5000 icons in one window.

            var scrollViewer = new ScrollViewer
            {
                HorizontalScrollBarVisibility =
                    CanScroll ? HorizontalScrollBarVisibility : ScrollBarVisibility.Disabled,
                VerticalScrollBarVisibility = CanScroll ? VerticalScrollBarVisibility : ScrollBarVisibility.Disabled
            };
            var panelPresenter = new VirtualizingStackPanel { ScrollOwner = scrollViewer };

            panelPresenter.SetValue(VirtualizingStackPanel.IsVirtualizingProperty, IsVirtualizing);
            panelPresenter.SetValue(VirtualizingStackPanel.VirtualizationModeProperty, VirtualizationMode);

            scrollViewer.Content = panelPresenter;
            Content = scrollViewer;

            await Task.Delay(15);

            var grid = new Grid();

            await Task.Run(async () =>
            {
                int itemsCount = 0;
                var panelIndex = 0;

                foreach (var singleItem in itemsSource)
                {
                    await panelPresenter.Dispatcher.InvokeAsync(() =>
                    {
                        if (panelIndex == 0)
                            grid = CreateGrid(columns);

                        var contentPresenter = new ContentPresenter
                        {
                            Content = singleItem
                        };

                        if (template != null)
                            contentPresenter.ContentTemplate = template;

                        contentPresenter.SetValue(Grid.ColumnProperty, panelIndex);
                        grid.Children.Add(contentPresenter);

                        panelIndex++;

                        if (panelIndex != columns)
                            return;

                        // Adding controls in groups greatly speeds up the process.
                        panelPresenter.Children.Add(grid);
                        panelIndex = 0;
                    });

                    // Giving a UI thread a moment to rest is some method, but it blocks it anyway
                    // However, it does give a minimal UI response to Messages from WinApi so at least the user doesn't see one big hang
                    if (itemsCount++ % 200 == 0)
                        await Task.Delay(50);
                }

                // If, after iteration, there will be a few elements
                await scrollViewer.Dispatcher.InvokeAsync(() =>
                {
                    if (grid.Children.Count > 0)
                        panelPresenter.Children.Add(grid);
                });
            });

            _compiled = true;
        }

        private Grid CreateGrid(int columns)
        {
            var grid = new Grid();
            for (int i = 0; i < columns; i++)
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            return grid;
        }

        private static async void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not VirtualizingUniformGrid virtualizingUniformGrid)
                return;

            await virtualizingUniformGrid.OnItemsSourceChanged();
        }

        private static async void OnItemTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not VirtualizingUniformGrid virtualizingUniformGrid)
                return;

            await virtualizingUniformGrid.OnItemTemplateChanged();
        }
    }
}
