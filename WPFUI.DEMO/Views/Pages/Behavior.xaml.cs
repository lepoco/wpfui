using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFUI.Demo.Views.Pages
{
    /// <summary>
    /// Interaction logic for Behavior.xaml
    /// </summary>
    public partial class Behavior : Page
    {
        public Behavior()
        {
            InitializeComponent();
        }

        private void ButtonTaskbar_Click(object sender, RoutedEventArgs e)
        {
            int selectedState = TaskbarStatusCombo.SelectedIndex;

            Int32.TryParse(TaskbarValueText.Text, out int value);

            if(value > 100)
            {
                value = 100;
            }

            if(value < 0)
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
    }
}

//new System.Threading.Thread(() =>
//{
//    System.Threading.Thread.Sleep(2000);

//    Taskbar.Progress.SetState(Taskbar.ProgressState.Normal, true);

//    for (int i = 1; i <= 100; i += 10)
//    {
//        Taskbar.Progress.SetValue(i, 100, true);
//        System.Threading.Thread.Sleep(1000);
//    }

//    Taskbar.Progress.SetState(Taskbar.ProgressState.None, true);
//}).Start();
