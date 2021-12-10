// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WPFUI.Demo.Views.Pages
{
    public struct DisplayableIcon
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public Common.Icon Icon { get; set; }
    }

    /// <summary>
    /// Interaction logic for Icons.xaml
    /// </summary>
    public partial class Icons : Page
    {
        private DisplayableIcon _activeGlyph;

        private List<DisplayableIcon> icons;

        public Icons()
        {
            InitializeComponent();
            FillIcons();
        }

        private async void FillIcons()
        {
            icons = new List<DisplayableIcon> { };
            DataContext = this;

            await Task.Run(() =>
            {
                int id = 0;
                string[] names = Enum.GetNames(typeof(Common.Icon));
                names = names.OrderBy(n => n).ToArray();

                foreach (string iconName in names)
                {
                    Common.Icon icon = Common.Glyph.Parse(iconName);
                    //System.Diagnostics.Debug.WriteLine(icon);

                    icons.Add(new DisplayableIcon
                    {
                        ID = id++,
                        Name = iconName,
                        Icon = icon,
                        Code = ((int)icon).ToString("X4")
                    });
                }

                Application.Current.Dispatcher.Invoke(() =>
                {
                    IconsItemsControl.ItemsSource = icons;

                    if (icons.Count > 4)
                    {
                        _activeGlyph = icons[4];

                        ChangeGlyps();
                    }
                });

                Thread.Sleep(1000);

                Application.Current.Dispatcher.Invoke(() =>
                {
                    //gridLoading.Visibility = Visibility.Hidden;
                });
            });
        }

        private void ChangeGlyps()
        {
            TextIconName.Text = _activeGlyph.Name;
            IconCodeBlock.Content = "<wpfui:Icon Glyph=\"" + _activeGlyph.Name + "\">";
            IconActiveIcon.Glyph = _activeGlyph.Icon;
            TextIconGlyph.Text = "\\u" + _activeGlyph.Code;
        }

        private void Border_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            int id = Int32.Parse((sender as Border)?.Tag.ToString() ?? string.Empty);

            _activeGlyph = icons[id];
            ChangeGlyps();
        }
    }
}
