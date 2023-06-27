// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;

namespace Wpf.Ui.Gallery.Controls;

public class GalleryNavigationPresenter : System.Windows.Controls.Control
{
    /// <summary>
    /// Property for <see cref="ItemsSource"/>.
    /// </summary>
    public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
        nameof(ItemsSource),
        typeof(object),
        typeof(GalleryNavigationPresenter),
        new PropertyMetadata(null)
    );

    /// <summary>
    /// Property for <see cref="TemplateButtonCommand"/>.
    /// </summary>
    public static readonly DependencyProperty TemplateButtonCommandProperty =
        DependencyProperty.Register(
            nameof(TemplateButtonCommand),
            typeof(Common.IRelayCommand),
            typeof(GalleryNavigationPresenter),
            new PropertyMetadata(null)
        );

    public object ItemsSource
    {
        get => GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    /// <summary>
    /// Command triggered after clicking the titlebar button.
    /// </summary>
    public Common.IRelayCommand TemplateButtonCommand =>
        (Common.IRelayCommand)GetValue(TemplateButtonCommandProperty);

    /// <summary>
    /// Creates a new instance of the class and sets the default <see cref="FrameworkElement.Loaded"/> event.
    /// </summary>
    public GalleryNavigationPresenter()
    {
        SetValue(
            TemplateButtonCommandProperty,
            new Common.RelayCommand<Type>(o => OnTemplateButtonClick(o))
        );
    }

    private void OnTemplateButtonClick(Type? pageType)
    {
        var navigationService = App.GetService<INavigationService>();

        if (navigationService == null)
            return;

        if (pageType is not null)
            navigationService.Navigate(pageType);

#if DEBUG
        System.Diagnostics.Debug.WriteLine(
            $"INFO | {nameof(GalleryNavigationPresenter)} navigated, ({pageType})",
            "Wpf.Ui.Gallery"
        );
#endif
    }
}
