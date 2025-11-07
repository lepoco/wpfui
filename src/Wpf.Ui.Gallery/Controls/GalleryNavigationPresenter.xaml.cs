// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

namespace Wpf.Ui.Gallery.Controls;

public class GalleryNavigationPresenter : System.Windows.Controls.Control
{
    /// <summary>Identifies the <see cref="ItemsSource"/> dependency property.</summary>
    public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
        nameof(ItemsSource),
        typeof(object),
        typeof(GalleryNavigationPresenter),
        new PropertyMetadata(null)
    );

    /// <summary>Identifies the <see cref="TemplateButtonCommand"/> dependency property.</summary>
    public static readonly DependencyProperty TemplateButtonCommandProperty = DependencyProperty.Register(
        nameof(TemplateButtonCommand),
        typeof(Wpf.Ui.Input.IRelayCommand),
        typeof(GalleryNavigationPresenter),
        new PropertyMetadata(null)
    );

    public object? ItemsSource
    {
        get => GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    /// <summary>
    /// Gets the command triggered after clicking the titlebar button.
    /// </summary>
    public Wpf.Ui.Input.IRelayCommand TemplateButtonCommand =>
        (Wpf.Ui.Input.IRelayCommand)GetValue(TemplateButtonCommandProperty);

    /// <summary>
    /// Initializes a new instance of the <see cref="GalleryNavigationPresenter"/> class.
    /// Creates a new instance of the class and sets the default <see cref="FrameworkElement.Loaded"/> event.
    /// </summary>
    public GalleryNavigationPresenter()
    {
        SetValue(TemplateButtonCommandProperty, new Input.RelayCommand<Type>(o => OnTemplateButtonClick(o)));
    }

    private void OnTemplateButtonClick(Type? pageType)
    {
        INavigationService navigationService = App.GetRequiredService<INavigationService>();

        if (pageType is not null)
        {
            _ = navigationService.Navigate(pageType);
        }

        System.Diagnostics.Debug.WriteLine(
            $"INFO | {nameof(GalleryNavigationPresenter)} navigated, ({pageType})",
            "Wpf.Ui.Gallery"
        );
    }
}
