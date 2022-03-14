using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;

namespace WPFUI.Demo.Views.Windows
{
    /// <summary>
    /// Interaction logic for Backdrop.xaml
    /// </summary>
    public partial class Backdrop : Window
    {
        private bool _isDarkTheme = false;
        public Backdrop()
        {
            InitializeComponent();
        }

        private void ThemeSelector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is not ComboBox comboBox) return;

            _isDarkTheme = comboBox.SelectedIndex == 0;

            WPFUI.Appearance.Theme.Set(comboBox.SelectedIndex == 0 ? Appearance.ThemeType.Dark : Appearance.ThemeType.Light);


            if (ComboBoxBackground != null)
            {
                ApplyBackgroundEffect(ComboBoxBackground.SelectedIndex);
            }
        }

        private void BackgroundSelector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is not ComboBox comboBox) return;

            ApplyBackgroundEffect(comboBox.SelectedIndex);
        }

        private void ApplyBackgroundEffect(int index)
        {
            IntPtr windowHandle = new WindowInteropHelper(this).Handle;

            WPFUI.Appearance.Background.Remove(windowHandle);

            if (_isDarkTheme)
            {
                WPFUI.Appearance.Background.ApplyDarkMode(windowHandle);
            }
            else
            {
                WPFUI.Appearance.Background.RemoveDarkMode(windowHandle);
            }

            switch (index)
            {
                case 1:
                    this.Background = Brushes.Transparent;
                    WPFUI.Appearance.Background.Apply(windowHandle, WPFUI.Appearance.BackgroundType.Auto);
                    break;

                case 2:
                    this.Background = Brushes.Transparent;
                    WPFUI.Appearance.Background.Apply(windowHandle, WPFUI.Appearance.BackgroundType.Mica);
                    break;

                case 3:
                    this.Background = Brushes.Transparent;
                    WPFUI.Appearance.Background.Apply(windowHandle, WPFUI.Appearance.BackgroundType.Tabbed);
                    break;

                case 4:
                    this.Background = Brushes.Transparent;
                    WPFUI.Appearance.Background.Apply(windowHandle, WPFUI.Appearance.BackgroundType.Acrylic);
                    break;
            }
        }
    }
}
