// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

/* This Code is based on a StackOverflow-Answer: https://stackoverflow.com/a/56736232/9759874 */

using System.Reflection;
using System.Windows.Threading;

namespace Wpf.Ui.Controls;

/// <summary>
/// Overwrites ContextMenu-Style for some UIElements (like RichTextBox) that don't take the default ContextMenu-Style by default.
/// <para>The code inside this CodeBehind-Class forces this ContextMenu-Style on these UIElements through Reflection (because it is only accessible through Reflection it is also only possible through CodeBehind and not XAML)</para>
/// </summary>
public partial class ContextMenuLoader : ResourceDictionary
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ContextMenuLoader"/> class and registers editing styles
    /// defined in "ContextMenu.xaml" with the <see cref="Dispatcher"/>.
    /// </summary>
    public ContextMenuLoader()
    {
        // Run OnResourceDictionaryLoaded asynchronously to ensure other ResourceDictionary are already loaded before adding new entries
        _ = Dispatcher.CurrentDispatcher.BeginInvoke(
            DispatcherPriority.Normal,
            new Action(OnResourceDictionaryLoaded)
        );
    }

    private void OnResourceDictionaryLoaded()
    {
        Assembly currentAssembly = typeof(Application).Assembly;

        AddEditorContextMenuDefaultStyle(currentAssembly);
    }

    private void AddEditorContextMenuDefaultStyle(Assembly currentAssembly)
    {
        var editorContextMenuType = Type.GetType(
            "System.Windows.Documents.TextEditorContextMenu+EditorContextMenu, " + currentAssembly
        );

        ResourceDictionary resourceDict = new()
        {
            Source = new Uri("pack://application:,,,/Wpf.Ui;component/Controls/ContextMenu/ContextMenu.xaml"),
        };

        Style contextMenuStyle = (Style)resourceDict["UiContextMenu"];

        if (editorContextMenuType is null || contextMenuStyle is null)
        {
            return;
        }

        var editorContextMenuStyle = new Style(editorContextMenuType, contextMenuStyle);
        Add(editorContextMenuType, editorContextMenuStyle);
    }
}
