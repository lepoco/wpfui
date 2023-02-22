using System.Windows.Media;
using System.Windows;

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

    protected System.Windows.Controls.Image? Image;

    private static void OnSourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var self = (ImageIcon)d;
        if (self.Image is null)
            return;

        self.Image.Source = (ImageSource)e.NewValue;
    }

    protected override void OnShouldInheritForegroundFromVisualParentChanged()
    {
        
    }

    protected override UIElement InitializeChildren()
    {
        Image = new System.Windows.Controls.Image()
        {
            Source = Source,
            Stretch = Stretch.UniformToFill
        };

        return Image;
    }
}
