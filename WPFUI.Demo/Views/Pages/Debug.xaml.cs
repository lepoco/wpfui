using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using WPFUI.Common;
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



        public Debug()
        {
            InitializeComponent();

            SControlsCollection = new()
            {
                new Anchor { Content = "Test Anchor" },
                new Anchor { Content = "Test Anchor", Appearance = Appearance.Danger },
                new Badge { Content = "Test Badge" },
                new CheckBox { Content = "Checkbox" }
            };

            SControlsCollectionSecondary = new()
            {
                new Anchor { Content = "Test Anchor" },
                new Anchor { Content = "Test Anchor", Appearance = Appearance.Danger },
                new Badge { Content = "Test Badge" },
                new CheckBox { Content = "Checkbox" }
            };

            SControlsCollectionTertiary = new()
            {
                new Anchor { Content = "Test Anchor" },
                new Anchor { Content = "Test Anchor", Appearance = Appearance.Danger },
                new Badge { Content = "Test Badge" },
                new CheckBox { Content = "Checkbox" }
            };

            DataContext = this;
        }
    }
}
