// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;
using Wpf.Ui.Gallery.Helpers;

namespace Wpf.Ui.Gallery.Controls;

public class GalleryNavigationPresenter : System.Windows.Controls.Control
{
    /// <summary>
    /// Property for <see cref="ItemsSource"/>.
    /// </summary>
    public static readonly DependencyProperty ItemsSourceProperty =
        DependencyProperty.Register(nameof(ItemsSource),
            typeof(object), typeof(GalleryNavigationPresenter), new PropertyMetadata(null));

    /// <summary>
    /// Property for <see cref="TemplateButtonCommand"/>.
    /// </summary>
    public static readonly DependencyProperty TemplateButtonCommandProperty =
        DependencyProperty.Register(nameof(TemplateButtonCommand),
            typeof(Common.IRelayCommand), typeof(GalleryNavigationPresenter), new PropertyMetadata(null));

    public object ItemsSource
    {
        get => GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    /// <summary>
    /// Command triggered after clicking the titlebar button.
    /// </summary>
    public Common.IRelayCommand TemplateButtonCommand => (Common.IRelayCommand)GetValue(TemplateButtonCommandProperty);

    /// <summary>
    /// Creates a new instance of the class and sets the default <see cref="FrameworkElement.Loaded"/> event.
    /// </summary>
    public GalleryNavigationPresenter()
    {
        SetValue(TemplateButtonCommandProperty, new Common.RelayCommand<string>(o => OnTemplateButtonClick(o ?? String.Empty)));
    }

    private void OnTemplateButtonClick(string parameter)
    {
        var navigationService = App.GetService<INavigationService>();

        if (navigationService == null)
            return;

        var pageType = NameToPageTypeConverter.Convert(parameter);

        if (pageType != null)
            navigationService.Navigate(pageType);

#if DEBUG
        System.Diagnostics.Debug.WriteLine($"INFO | {nameof(GalleryNavigationPresenter)} navigated, {parameter} ({pageType})", "Wpf.Ui.Gallery");
#endif
    }
}
