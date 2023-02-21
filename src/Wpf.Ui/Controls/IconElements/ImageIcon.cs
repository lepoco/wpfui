using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;

namespace Wpf.Ui.Controls.IconElements;

public class ImageIcon : IconElement
{
    /// <summary>
    /// Gets/Sets the Source on this Image.
    /// The Source property is the ImageSource that holds the actual image drawn.
    /// </summary>
    public static readonly DependencyProperty SourceProperty =
        DependencyProperty.Register(nameof(Source), typeof(ImageSource), typeof(ImageIcon),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, OnSourcePropertyChanged));

    /// <summary>
    /// Gets/Sets the Source on this Image.
    /// The Source property is the ImageSource that holds the actual image drawn.
    /// </summary>
    public ImageSource? Source
    {
        get => (ImageSource)GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }

    private System.Windows.Controls.Image? _image;

    private static void OnSourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var self = (ImageIcon)d;
        if (self._image is null)
            return;

        self._image.Source = (ImageSource)e.NewValue;
    }

    protected override void InitializeChildren()
    {
        _image = new System.Windows.Controls.Image()
        {
            Source = Source,
            Stretch = Stretch.UniformToFill
        };

        Children.Add(_image);
    }
}
