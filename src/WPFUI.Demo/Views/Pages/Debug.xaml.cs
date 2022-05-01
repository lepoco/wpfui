using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using WPFUI.Controls;

namespace WPFUI.Demo.Views.Pages
{
    /// <summary>
    /// Interaction logic for Debug.xaml
    /// </summary>
    public partial class Debug : Page
    {
        public ObservableCollection<UIElement> SControlsCollection { get; internal set; }
        public ObservableCollection<UIElement> SControlsCollectionSecondary { get; internal set; }
        public ObservableCollection<UIElement> SControlsCollectionTertiary { get; internal set; }
        public ObservableCollection<UIElement> SControlsCollectionQuaternary { get; internal set; }

        public Debug()
        {
            InitializeComponent();

            SControlsCollection = new()
            {
                new Anchor { Content = "Test Anchor" },
                new Anchor { Content = "Test Anchor", Appearance = Common.Appearance.Danger },
                new Badge { Content = "Test Badge" },
                new CheckBox { Content = "Checkbox" },
                new ToggleButton() { Content = "ToggleButton" },
                new ToggleSwitch() { Content = "ToggleSwitch" },
            };

            SControlsCollectionSecondary = new()
            {
                new Anchor { Content = "Test Anchor" },
                new Anchor { Content = "Test Anchor", Appearance = Common.Appearance.Danger },
                new Badge { Content = "Test Badge" },
                new CheckBox { Content = "Checkbox" },
                new ToggleButton() { Content = "ToggleButton" },
                new ToggleSwitch() { Content = "ToggleSwitch" },
            };

            SControlsCollectionTertiary = new()
            {
                new Anchor { Content = "Test Anchor" },
                new Anchor { Content = "Test Anchor", Appearance = Common.Appearance.Danger },
                new Badge { Content = "Test Badge" },
                new CheckBox { Content = "Checkbox" },
                new ToggleButton() { Content = "ToggleButton" },
                new ToggleSwitch() { Content = "ToggleSwitch" },
            };

            SControlsCollectionQuaternary = new()
            {
                new Anchor { Content = "Test Anchor" },
                new Anchor { Content = "Test Anchor", Appearance = Common.Appearance.Danger },
                new Badge { Content = "Test Badge" },
                new CheckBox { Content = "Checkbox" },
                new ToggleButton() { Content = "ToggleButton" },
                new ToggleSwitch() { Content = "ToggleSwitch" },
            };

            DataContext = this;
        }
    }
}
