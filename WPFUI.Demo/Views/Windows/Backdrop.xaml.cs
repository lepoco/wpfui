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

            WPFUI.Theme.Manager.Switch(comboBox.SelectedIndex == 0 ? Theme.Style.Dark : Theme.Style.Light);


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

            WPFUI.Background.Manager.Remove(windowHandle);

            if (_isDarkTheme)
            {
                WPFUI.Background.Manager.ApplyDarkMode(windowHandle);
            }
            else
            {
                WPFUI.Background.Manager.RemoveDarkMode(windowHandle);
            }

            switch (index)
            {
                case 1:
                    this.Background = Brushes.Transparent;
                    WPFUI.Background.Manager.Apply(WPFUI.Background.BackgroundType.Auto, windowHandle);
                    break;

                case 2:
                    this.Background = Brushes.Transparent;
                    WPFUI.Background.Manager.Apply(WPFUI.Background.BackgroundType.Mica, windowHandle);
                    break;

                case 3:
                    this.Background = Brushes.Transparent;
                    WPFUI.Background.Manager.Apply(WPFUI.Background.BackgroundType.Tabbed, windowHandle);
                    break;

                case 4:
                    this.Background = Brushes.Transparent;
                    WPFUI.Background.Manager.Apply(WPFUI.Background.BackgroundType.Acrylic, windowHandle);
                    break;
            }
        }
    }
}
