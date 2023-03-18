// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using Wpf.Ui.Appearance;
using Wpf.Ui.Common;
using Wpf.Ui.Controls.NumberBoxControl;
using Wpf.Ui.Extensions;
using Color = System.Windows.Media.Color;

namespace Wpf.Ui.Controls;

/// <summary>
/// Formats and display a fragment of the source code.
/// </summary>
[ToolboxItem(true)]
[ToolboxBitmap(typeof(CodeBlock), "CodeBlock.bmp")]
public class CodeBlock : System.Windows.Controls.ContentControl
{
    private string _sourceCode = String.Empty;

    /// <summary>
    /// Property for <see cref="SyntaxContent"/>.
    /// </summary>
    public static readonly DependencyProperty SyntaxContentProperty = DependencyProperty.Register(nameof(SyntaxContent),
        typeof(object), typeof(CodeBlock),
        new PropertyMetadata(null));

    /// <summary>
    /// Property for <see cref="ButtonCommand"/>.
    /// </summary>
    public static readonly DependencyProperty ButtonCommandProperty =
        DependencyProperty.Register(nameof(NumberBox),
            typeof(IRelayCommand), typeof(CodeBlock));

    /// <summary>
    /// Formatted <see cref="System.Windows.Controls.ContentControl.Content"/>.
    /// </summary>
    public object SyntaxContent
    {
        get => GetValue(SyntaxContentProperty);
        internal set => SetValue(SyntaxContentProperty, value);
    }

    /// <summary>
    /// Command triggered after clicking the control button.
    /// </summary>
    public IRelayCommand ButtonCommand => (Common.IRelayCommand)GetValue(ButtonCommandProperty);

    /// <summary>
    /// Creates new instance and assigns <see cref="ButtonCommand"/> default action.
    /// </summary>
    public CodeBlock()
    {
        SetValue(ButtonCommandProperty, new RelayCommand<string>(OnTemplateButtonClick));

        Appearance.Theme.Changed += ThemeOnChanged;
    }

    private void ThemeOnChanged(ThemeType currentTheme, Color systemAccent)
    {
        UpdateSyntax();
    }

    /// <summary>
    /// This method is invoked when the Content property changes.
    /// </summary>
    /// <param name="oldContent">The old value of the Content property.</param>
    /// <param name="newContent">The new value of the Content property.</param>
    protected override void OnContentChanged(object oldContent, object newContent)
    {
        UpdateSyntax();
    }

    protected virtual void UpdateSyntax()
    {
        _sourceCode = Syntax.Highlighter.Clean(Content as string ?? string.Empty);

        RichTextBox richTextBox = new RichTextBox()
        {
            IsTextSelectionEnabled = true,
            VerticalContentAlignment = VerticalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Left,
            HorizontalContentAlignment = HorizontalAlignment.Left
        };

        richTextBox.Document.Blocks.Clear();
        richTextBox.Document.Blocks.Add(Syntax.Highlighter.FormatAsParagraph(_sourceCode));

        SyntaxContent = richTextBox;
    }

    private void OnTemplateButtonClick(string? _)
    {
        Debug.WriteLine($"INFO | CodeBlock source: \n{_sourceCode}", "Wpf.Ui.CodeBlock");

        try
        {
            Clipboard.Clear();
            Clipboard.SetText(_sourceCode);
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
        }
    }
}
