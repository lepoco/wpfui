// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;
using System.Windows.Threading;
using Microsoft.Web.WebView2.Wpf;
using Wpf.Ui.Gallery.Controllers;
using Wpf.Ui.Gallery.Models.Monaco;

namespace Wpf.Ui.Gallery.ViewModels.Windows;

public partial class MonacoWindowViewModel : ObservableObject
{
    private MonacoController? _monacoController;

    public void SetWebView(WebView2 webView)
    {
        webView.NavigationCompleted += OnWebViewNavigationCompleted;
        webView.UseLayoutRounding = true;
        webView.DefaultBackgroundColor = System.Drawing.Color.Transparent;
        webView.Source = new Uri(
            System.IO.Path.Combine(
                System.AppDomain.CurrentDomain.BaseDirectory,
                @"Assets\Monaco\index.html"
            )
        );

        _monacoController = new MonacoController(webView);
    }

    [RelayCommand]
    public void OnMenuAction(string parameter) { }

    private async Task InitializeEditorAsync()
    {
        if (_monacoController == null)
            return;

        await _monacoController.CreateAsync();
        await _monacoController.SetThemeAsync(Appearance.Theme.GetAppTheme());
        await _monacoController.SetLanguageAsync(MonacoLanguage.Csharp);
        await _monacoController.SetContentAsync(
            "// This Source Code Form is subject to the terms of the MIT License.\r\n// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.\r\n// Copyright (C) Leszek Pomianowski and WPF UI Contributors.\r\n// All Rights Reserved.\r\n\r\nnamespace Wpf.Ui.Gallery.Models.Monaco;\r\n\r\n[Serializable]\r\npublic record MonacoTheme\r\n{\r\n    public string Base { get; init; }\r\n\r\n    public bool Inherit { get; init; }\r\n\r\n    public IDictionary<string, string> Rules { get; init; }\r\n\r\n    public IDictionary<string, string> Colors { get; init; }\r\n}\r\n"
        );
    }

    private void OnWebViewNavigationCompleted(
        object? sender,
        Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs e
    )
    {
        DispatchAsync(InitializeEditorAsync);
    }

    private DispatcherOperation<TResult> DispatchAsync<TResult>(Func<TResult> callback)
    {
        return Application.Current.Dispatcher.InvokeAsync(callback);
    }
}
