// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;

namespace Wpf.Ui.Styles.Controls
{
    /// <summary>
    /// Overwrites ContextMenu-Style for some UIElements (like RichTextBox) that don't take the default ContextMenu-Style by default.
    /// <para>The code inside this CodeBehind-Class forces this ContextMenu-Style on these UIElements through Reflection (because it is only accessible through Reflection it is also only possible through CodeBehind and not XAML)</para>
    /// </summary>
    // This Code is based on a StackOverflow-Answer: https://stackoverflow.com/a/56736232/9759874
    partial class ContextMenu : ResourceDictionary
    {
        /// <summary>
        /// Registers editing <see cref="ContextMenu"/> styles with <see cref="Dispatcher"/>.
        /// </summary>
        public ContextMenu()
        {
            // Run OnResourceDictionaryLoaded asynchronously to ensure other ResourceDictionary are already loaded before adding new entries
            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(OnResourceDictionaryLoaded));
        }

        private void OnResourceDictionaryLoaded()
        {
            var currentAssembly = typeof(Application).Assembly;

            AddEditorContextMenuDefaultStyle(currentAssembly);
        }

        private void AddEditorContextMenuDefaultStyle(Assembly currentAssembly)
        {
            var contextMenuStyle = this["UiContextMenu"] as Style;
            var editorContextMenuType = Type.GetType("System.Windows.Documents.TextEditorContextMenu+EditorContextMenu, " + currentAssembly);

            if (editorContextMenuType == null || contextMenuStyle == null)
                return;

            var editorContextMenuStyle = new Style(editorContextMenuType, contextMenuStyle);
            Add(editorContextMenuType, editorContextMenuStyle);
        }

    }
}
