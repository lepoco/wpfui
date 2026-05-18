// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows.Controls;
using Wpf.Ui.Controls;
using Wpf.Ui.Gallery.Controls;
using Wpf.Ui.Gallery.ControlsLookup;
using Wpf.Ui.Gallery.ViewModels.Pages.Layout;

namespace Wpf.Ui.Gallery.Views.Pages.Layout;

[GalleryPage("Expander control.", SymbolRegular.Code24)]
public partial class ExpanderPage : INavigableView<ExpanderViewModel>
{
    public ExpanderPage(ExpanderViewModel viewModel)
    {
        InitializeComponent();
        ViewModel = viewModel;
    }

    public ExpanderViewModel ViewModel { get; }

    private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var comboBox = (ComboBox)sender;
        string selectedText = (string)((ComboBoxItem)comboBox.SelectedValue).Content;
        ExpandDirection direction = Enum.Parse<ExpandDirection>(selectedText);

        Expander1.SetCurrentValue(Expander.ExpandDirectionProperty, direction);

        switch (direction)
        {
            case ExpandDirection.Down:
                Expander1.SetCurrentValue(HorizontalAlignmentProperty, HorizontalAlignment.Left);
                Expander1.SetCurrentValue(VerticalAlignmentProperty, VerticalAlignment.Top);

                ControlExample1.SetCurrentValue(ControlExample.XamlCodeProperty, """
                    <Expander HorizontalAlignment="Left" VerticalAlignment="Top" Content="This is in the content" ExpandDirection="Down" Header="This text is in the header" />
                    """);

                break;
            case ExpandDirection.Up:
                Expander1.SetCurrentValue(HorizontalAlignmentProperty, HorizontalAlignment.Left);
                Expander1.SetCurrentValue(VerticalAlignmentProperty, VerticalAlignment.Bottom);

                ControlExample1.SetCurrentValue(ControlExample.XamlCodeProperty, """
                    <Expander HorizontalAlignment="Left" VerticalAlignment="Bottom" Content="This is in the content" ExpandDirection="Up" Header="This text is in the header" />
                    """);

                break;
            case ExpandDirection.Left:
                Expander1.SetCurrentValue(HorizontalAlignmentProperty, HorizontalAlignment.Right);
                Expander1.SetCurrentValue(VerticalAlignmentProperty, VerticalAlignment.Top);

                ControlExample1.SetCurrentValue(ControlExample.XamlCodeProperty, """
                    <Expander HorizontalAlignment="Right" VerticalAlignment="Top" Content="This is in the content" ExpandDirection="Left" Header="This text is in the header" />
                    """);

                break;
            case ExpandDirection.Right:
                Expander1.SetCurrentValue(HorizontalAlignmentProperty, HorizontalAlignment.Left);
                Expander1.SetCurrentValue(VerticalAlignmentProperty, VerticalAlignment.Top);

                ControlExample1.SetCurrentValue(ControlExample.XamlCodeProperty, """
                    <Expander HorizontalAlignment="Left" VerticalAlignment="Top" Content="This is in the content" ExpandDirection="Right" Header="This text is in the header" />
                    """);

                break;
            default:
                break;
        }
    }
}
