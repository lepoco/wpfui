// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows.Controls;
using Wpf.Ui.Controls;

namespace Wpf.Ui.Gallery.Controls;

public class PageControlDocumentation : Control
{
    public static readonly DependencyProperty ShowProperty = DependencyProperty.RegisterAttached(
        "Show",
        typeof(bool),
        typeof(PageControlDocumentation),
        new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsRender)
    );

    public static readonly DependencyProperty DocumentationTypeProperty = DependencyProperty.RegisterAttached(
        "DocumentationType",
        typeof(Type),
        typeof(PageControlDocumentation),
        new FrameworkPropertyMetadata(null)
    );

    /// <summary>Helper for getting <see cref="ShowProperty"/> from <paramref name="target"/>.</summary>
    /// <param name="target"><see cref="FrameworkElement"/> to read <see cref="ShowProperty"/> from.</param>
    /// <returns>Show property value.</returns>
    [AttachedPropertyBrowsableForType(typeof(FrameworkElement))]
    public static bool GetShow(FrameworkElement target) => (bool)target.GetValue(ShowProperty);

    /// <summary>Helper for setting <see cref="ShowProperty"/> on <paramref name="target"/>.</summary>
    /// <param name="target"><see cref="FrameworkElement"/> to set <see cref="ShowProperty"/> on.</param>
    /// <param name="show">Show property value.</param>
    public static void SetShow(FrameworkElement target, bool show) => target.SetValue(ShowProperty, show);

    /// <summary>Helper for getting <see cref="DocumentationTypeProperty"/> from <paramref name="target"/>.</summary>
    /// <param name="target"><see cref="FrameworkElement"/> to read <see cref="DocumentationTypeProperty"/> from.</param>
    /// <returns>DocumentationType property value.</returns>
    [AttachedPropertyBrowsableForType(typeof(FrameworkElement))]
    public static Type? GetDocumentationType(FrameworkElement target) =>
        (Type?)target.GetValue(DocumentationTypeProperty);

    /// <summary>Helper for setting <see cref="DocumentationTypeProperty"/> on <paramref name="target"/>.</summary>
    /// <param name="target"><see cref="FrameworkElement"/> to set <see cref="DocumentationTypeProperty"/> on.</param>
    /// <param name="type">DocumentationType property value.</param>
    public static void SetDocumentationType(FrameworkElement target, Type? type) =>
        target.SetValue(DocumentationTypeProperty, type);

    /// <summary>Identifies the <see cref="NavigationView"/> dependency property.</summary>
    public static readonly DependencyProperty NavigationViewProperty = DependencyProperty.Register(
        nameof(NavigationView),
        typeof(INavigationView),
        typeof(PageControlDocumentation),
        new FrameworkPropertyMetadata(null)
    );

    /// <summary>Identifies the <see cref="IsDocumentationLinkVisible"/> dependency property.</summary>
    public static readonly DependencyProperty IsDocumentationLinkVisibleProperty =
        DependencyProperty.Register(
            nameof(IsDocumentationLinkVisible),
            typeof(Visibility),
            typeof(PageControlDocumentation),
            new FrameworkPropertyMetadata(Visibility.Collapsed)
        );

    /// <summary>Identifies the <see cref="TemplateButtonCommand"/> dependency property.</summary>
    public static readonly DependencyProperty TemplateButtonCommandProperty = DependencyProperty.Register(
        nameof(TemplateButtonCommand),
        typeof(ICommand),
        typeof(PageControlDocumentation),
        new PropertyMetadata(null)
    );

    public INavigationView? NavigationView
    {
        get => (INavigationView?)GetValue(NavigationViewProperty);
        set => SetValue(NavigationViewProperty, value);
    }

    public Visibility IsDocumentationLinkVisible
    {
        get => (Visibility)GetValue(IsDocumentationLinkVisibleProperty);
        set => SetValue(IsDocumentationLinkVisibleProperty, value);
    }

    public ICommand TemplateButtonCommand => (ICommand)GetValue(TemplateButtonCommandProperty);

    public PageControlDocumentation()
    {
        Loaded += static (sender, _) => ((PageControlDocumentation)sender).OnLoaded();
        Unloaded += static (sender, _) => ((PageControlDocumentation)sender).OnUnloaded();

        SetValue(
            TemplateButtonCommandProperty,
            new CommunityToolkit.Mvvm.Input.RelayCommand<string>(OnClick)
        );
    }

    private FrameworkElement? _page;

    private void OnLoaded()
    {
        if (NavigationView is null)
        {
            throw new ArgumentNullException(nameof(NavigationView));
        }

        NavigationView.Navigated += NavigationViewOnNavigated;
    }

    private void OnUnloaded()
    {
        NavigationView!.Navigated -= NavigationViewOnNavigated;
        _page = null;
    }

    private void NavigationViewOnNavigated(NavigationView sender, NavigatedEventArgs args)
    {
        SetCurrentValue(IsDocumentationLinkVisibleProperty, Visibility.Collapsed);

        if (args.Page is not FrameworkElement page || !GetShow(page))
        {
            SetCurrentValue(VisibilityProperty, Visibility.Collapsed);
            return;
        }

        _page = page;
        SetCurrentValue(VisibilityProperty, Visibility.Visible);

        if (GetDocumentationType(page) is not null)
        {
            SetCurrentValue(IsDocumentationLinkVisibleProperty, Visibility.Visible);
        }
    }

    private void OnClick(string? param)
    {
        if (string.IsNullOrWhiteSpace(param) || _page is null)
        {
            return;
        }

        // TODO: Refactor switch
        if (param == "theme")
        {
            SwitchThemes();
            return;
        }

        string navigationUrl = param switch
        {
            "doc" when GetDocumentationType(_page) is { } documentationType
                => CreateUrlForDocumentation(documentationType),
            "xaml" => CreateUrlForGithub(_page.GetType(), ".xaml"),
            "c#" => CreateUrlForGithub(_page.GetType(), ".xaml.cs"),
            _ => string.Empty
        };

        if (string.IsNullOrEmpty(navigationUrl))
        {
            return;
        }

        try
        {
            ProcessStartInfo sInfo = new(navigationUrl) { UseShellExecute = true };

            _ = Process.Start(sInfo);
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
        }
    }

    private static string CreateUrlForGithub(Type pageType, ReadOnlySpan<char> fileExtension)
    {
        const string baseUrl = "https://github.com/lepoco/wpfui/tree/main/src/Wpf.Ui.Gallery/";
        const string baseNamespace = "Wpf.Ui.Gallery";

        ReadOnlySpan<char> pageFullNameWithoutBaseNamespace = pageType.FullName.AsSpan()[(baseNamespace.Length + 1)..];

        Span<char> pageUrl = stackalloc char[pageFullNameWithoutBaseNamespace.Length];
        pageFullNameWithoutBaseNamespace.CopyTo(pageUrl);

        for (int i = 0; i < pageUrl.Length; i++)
        {
            if (pageUrl[i] == '.')
            {
                pageUrl[i] = '/';
            }
        }

        return string.Concat(baseUrl, pageUrl, fileExtension);
    }

    private static string CreateUrlForDocumentation(Type type)
    {
        const string baseUrl = "https://wpfui.lepo.co/api/";

        return string.Concat(baseUrl, type.FullName, ".html");
    }

    private static void SwitchThemes()
    {
        Appearance.ApplicationTheme currentTheme = Wpf.Ui.Appearance.ApplicationThemeManager.GetAppTheme();

        Wpf.Ui.Appearance.ApplicationThemeManager.Apply(
            currentTheme == Wpf.Ui.Appearance.ApplicationTheme.Light
                ? Wpf.Ui.Appearance.ApplicationTheme.Dark
                : Wpf.Ui.Appearance.ApplicationTheme.Light
        );
    }
}
