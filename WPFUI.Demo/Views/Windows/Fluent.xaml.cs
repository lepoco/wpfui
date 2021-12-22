// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using WPFUI.Controls;

namespace WPFUI.Demo.Views.Windows
{

    public class MyPropertyChangedEventArgs : PropertyChangedEventArgs
    {
        public MyPropertyChangedEventArgs(string propertyName, object oldValue,
            object newValue) : base(propertyName)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }

        public object OldValue;
        public object NewValue;
    }

    public class DemoNavigation : Control
    {
        public static readonly DependencyProperty MyItemsProperty =
            DependencyProperty.Register(
                "MyItems", typeof(ObservableCollection<NavigationItem>),
                typeof(DemoNavigation),
                new PropertyMetadata(default(ObservableCollection<NavigationItem>)));

        public ObservableCollection<NavigationItem> MyItems
        {
            get { return (ObservableCollection<NavigationItem>)GetValue(MyItemsProperty); }
            set { SetValue(MyItemsProperty, value); }
        }

        //private void MyItem_Changed(object s, PropertyChangedEventArgs args)
        //{
        //    var e = args as MyPropertyChangedEventArgs;
        //    if (e == null)
        //        OnPropertyChanged(new object(), new object());
        //    else
        //        OnPropertyChanged(e.OldValue, e.NewValue);
        //}

        void MyItems_Changed(object d, NotifyCollectionChangedEventArgs e)
        {
            var addedItems = e.NewItems as IList;
            var deletedItems = e.OldItems as IList;

            if (addedItems != null)
            {
                foreach (var addedItem in addedItems)
                {
                    ((NavigationItem)addedItem).Click += (sender, args) =>
                    {
                        System.Diagnostics.Debug.WriteLine("Clicked");
                    };

                    this.AddLogicalChild((NavigationItem)addedItem);
                    //((NavigationItem)addedItem).PropertyChanged += MyItem_Changed;
                }
            }
            if (deletedItems != null)
            {
                foreach (var deletedItem in deletedItems)
                {
                    this.RemoveLogicalChild((NavigationItem)deletedItem);
                    //((NavigationItem)deletedItem).PropertyChanged -= MyItem_Changed;
                }
            }
        }

        public DemoNavigation()
        {
            MyItems = new ObservableCollection<NavigationItem>();
            MyItems.CollectionChanged += MyItems_Changed;
        }

        static DemoNavigation()
        {
            // Use standard label template
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DemoNavigation),
                new FrameworkPropertyMetadata(typeof(Control)));
        }
    }
    /// <summary>
    /// Interaction logic for Fluent.xaml
    /// </summary>
    public partial class Fluent : Window
    {
        public Fluent()
        {
            InitializeComponent();
        }
    }
}
