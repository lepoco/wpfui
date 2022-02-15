using System;
using System.Windows;

namespace WPFUI.Controls
{
    /// <summary>
    /// Extended textbox with additional parameters.
    /// </summary>
    public class TextBox : System.Windows.Controls.TextBox
    {
        /// <summary>
        /// Property for <see cref="Placeholder"/>.
        /// </summary>
        public static readonly DependencyProperty PlaceholderProperty = DependencyProperty.Register(nameof(Placeholder),
            typeof(string), typeof(TextBox), new PropertyMetadata(String.Empty));

        /// <summary>
        /// Property for <see cref="PlaceholderVisible"/>.
        /// </summary>
        public static readonly DependencyProperty PlaceholderVisibleProperty = DependencyProperty.Register(nameof(PlaceholderVisible),
            typeof(bool), typeof(TextBox), new PropertyMetadata(true));

        /// <summary>
        /// Gets or sets numbers pattern.
        /// </summary>
        public string Placeholder
        {
            get => (string)GetValue(PlaceholderProperty);
            set => SetValue(PlaceholderProperty, value);
        }

        /// <summary>
        /// Gets or sets value determining whether to display the placeholder.
        /// </summary>
        public bool PlaceholderVisible
        {
            get => (bool)GetValue(PlaceholderVisibleProperty);
            set => SetValue(PlaceholderVisibleProperty, value);
        }

        /// <summary>
        /// Creates a new instance and assigns default events.
        /// </summary>
        public TextBox()
        {
            TextChanged += TextBox_TextChanged;
        }

        private void TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (sender is not TextBox control) return;

            if (PlaceholderVisible && control.Text.Length > 0)
                PlaceholderVisible = false;

            if (!PlaceholderVisible && control.Text.Length < 1)
                PlaceholderVisible = true;
        }
    }
}
