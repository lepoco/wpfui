// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.Web.WebView2.Wpf;
using Wpf.Ui.Appearance;
using Wpf.Ui.Gallery.Models.Monaco;

namespace Wpf.Ui.Gallery.Controllers;

public class MonacoController
{
    private const string EditorContainerSelector = "#root";

    private const string EditorObject = "wpf_ui_monaco_editor";

    private volatile WebView2 _webView;

    public MonacoController(WebView2 webView)
    {
        _webView = webView;
    }

    public async Task CreateAsync()
    {
        await _webView.ExecuteScriptAsync("const " + EditorObject + " = monaco.editor.create(document.querySelector('" + EditorContainerSelector + "'));");
        await _webView.ExecuteScriptAsync("window.onresize = () => {" + EditorObject + ".layout();}");
    }

    public async Task SetThemeAsync(ThemeType appTheme)
    {
        var baseMonacoTheme = appTheme == ThemeType.Light ? "vs" : "vs-dark";

        await _webView.ExecuteScriptAsync("monaco.editor.defineTheme('wpf-ui-app-theme', {base: '" + baseMonacoTheme + "',inherit: true, rules: [{ background: 'FFFFFF00' }], colors: {'editor.background': '#FFFFFF00','minimap.background': '#FFFFFF00',}});");
        await _webView.ExecuteScriptAsync("monaco.editor.setTheme('wpf-ui-app-theme');");
    }

    public async Task SetLanguageAsync(MonacoLanguage monacoLanguage)
    {
        var languageId = monacoLanguage == MonacoLanguage.ObjectiveC ? "objective-c" : monacoLanguage.ToString().ToLower();

        await _webView.ExecuteScriptAsync("monaco.editor.setModelLanguage(" + EditorObject + $".getModel(), \"{languageId}\");");
    }

    public async Task SetContentAsync(string contents)
    {
        var literalContents = SymbolDisplay.FormatLiteral(contents, false);

        await _webView.ExecuteScriptAsync(EditorObject + $".setValue(\"{literalContents}\");");
    }

    public void DispatchScript(string script)
    {
        if (_webView == null)
            return;

        Application.Current.Dispatcher.InvokeAsync(async () => await _webView!.ExecuteScriptAsync(script));
    }
}
