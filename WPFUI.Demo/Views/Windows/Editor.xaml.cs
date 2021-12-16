using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace WPFUI.Demo.Views.Windows
{
    /// <summary>
    /// Interaction logic for Editor.xaml
    /// </summary>
    public partial class Editor : Window
    {
        internal class EditorDataStack : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;

            protected void OnPropertyChanged(string name)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }

            private int
                _line = 1,
                _character = 0,
                _progress = 80;

            private string _file = "Draft";

            public int Line
            {
                get => _line;
                set
                {
                    if (value != _line)
                    {
                        _line = value;
                        OnPropertyChanged(nameof(Line));
                    }
                }
            }

            public int Character
            {
                get => _character;
                set
                {
                    if (value != _character)
                    {
                        _character = value;
                        OnPropertyChanged(nameof(Character));
                    }
                }
            }

            public int Progress
            {
                get => _progress;
                set
                {
                    if (value != _progress)
                    {
                        _progress = value;
                        OnPropertyChanged(nameof(Progress));
                    }
                }
            }

            public string File
            {
                get => _file;
                set
                {
                    if (value != _file)
                    {
                        _file = value;
                        OnPropertyChanged(nameof(File));
                    }
                }
            }
        }

        private EditorDataStack DataStack = new();

        public string Line { get; set; } = "0";

        public Editor()
        {
            if (WPFUI.Background.Mica.IsSupported() && WPFUI.Background.Mica.IsSystemThemeCompatible())
                WPFUI.Background.Mica.Apply(this);

            InitializeComponent();

            DataContext = DataStack;
        }

        private void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            if (sender is not MenuItem item) return;

            string tag = item?.Tag as string ?? String.Empty;

#if DEBUG
            System.Diagnostics.Debug.WriteLine("Clicked: " + tag);
#endif

            switch (tag)
            {
                case "exit":
                    Close();

                    break;

                case "save":
                    Save();

                    break;

                case "open":
                    Open();

                    break;

                case "new_file":
                    RootTextBox.Document = new();
                    DataStack.File = "Draft";

                    break;

                case "new_window":
                    Editor editorWindow = new();
                    editorWindow.Owner = null;
                    editorWindow.Show();

                    break;

                case "word_wrap":
                    RootSnackbar.Title = "Word wrapping changed!";
                    RootSnackbar.Message = "Currently word wrapping is " + (item.IsChecked ? "Enabled" : "Disabled");
                    RootSnackbar.Show = true;

                    break;

                case "status_bar":
                    RootStatusBar.Visibility = item.IsChecked ? Visibility.Visible : Visibility.Collapsed;

                    break;




                default:
                    ActionDialog.Show = true;

                    break;
            }
        }

        private void Save()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                DataStack.File = openFileDialog.FileName;
                // Save
            }
        }

        private void Open()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                DataStack.File = openFileDialog.FileName;
                // Load
            }
        }

        private void UpdateLine()
        {
            TextPointer caretPosition = RootTextBox.CaretPosition;
            TextPointer p = RootTextBox.Document.ContentStart.GetLineStartPosition(0);

            RootTextBox.CaretPosition.GetLineStartPosition(-Int32.MaxValue, out int lineMoved);

            DataStack.Line = -lineMoved;
            DataStack.Character = Math.Max(p.GetOffsetToPosition(caretPosition) - 1, 0);
        }

        private void RootTextBox_OnGotFocus(object sender, RoutedEventArgs e)
        {
#if DEBUG
            System.Diagnostics.Debug.WriteLine("Got focus");
#endif
            UpdateLine();
        }

        private void RootTextBox_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount > 2)
            {
                return;
            }
#if DEBUG
            System.Diagnostics.Debug.WriteLine("Mouse down");
#endif
            UpdateLine();
        }

        private void RootTextBox_OnPreviewKeyUp(object sender, KeyEventArgs e)
        {
#if DEBUG
            System.Diagnostics.Debug.WriteLine("Key up");
#endif
            UpdateLine();
        }
    }
}
