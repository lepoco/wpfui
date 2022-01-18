using System;
using System.Windows;
using System.Windows.Controls;

namespace WPFUI.Demo.Views.Pages
{
    /// <summary>
    /// Interaction logic for Actions.xaml
    /// </summary>
    public partial class Actions : Page
    {
        public Actions()
        {
            InitializeComponent();
        }

        private void ButtonTaskbar_Click(object sender, RoutedEventArgs e)
        {
            int selectedState = TaskbarStatusCombo.SelectedIndex;

            Int32.TryParse(TaskbarValueText.Text, out int value);

            if (value > 100)
            {
                value = 100;
            }

            if (value < 0)
            {
                value = 0;
            }

            switch (selectedState)
            {
                case 0: // None
                    Taskbar.Progress.SetState(Taskbar.ProgressState.None, false);
                    break;
                case 1: // Indeterminate
                    Taskbar.Progress.SetState(Taskbar.ProgressState.Indeterminate, false);
                    break;
                case 2: // Normal
                    Taskbar.Progress.SetValue(value, 100, false);
                    Taskbar.Progress.SetState(Taskbar.ProgressState.Normal, false);
                    break;
                case 3: // Error
                    Taskbar.Progress.SetValue(value, 100, false);
                    Taskbar.Progress.SetState(Taskbar.ProgressState.Error, false);
                    break;
                case 4: // Paused
                    Taskbar.Progress.SetValue(value, 100, false);
                    Taskbar.Progress.SetState(Taskbar.ProgressState.Paused, false);
                    break;
            }
        }

        private void Button_SwitchLightClick(object sender, RoutedEventArgs e)
        {
            Theme.Manager.Switch(Theme.Style.Light, true, true);
        }

        private void Button_SwitchDarkClick(object sender, RoutedEventArgs e)
        {
            Theme.Manager.Switch(Theme.Style.Dark, true, true);
        }
    }
}
