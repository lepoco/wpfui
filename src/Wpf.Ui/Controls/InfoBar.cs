using System.Windows;

namespace Wpf.Ui.Controls
{
    /// <summary>
    /// An <see cref="InfoBar" /> is an inline notification for essential app-
    /// wide messages. The InfoBar will take up space in a layout and will not
    /// cover up other content or float on top of it. It supports rich content
    /// (including titles, messages, and icons) and can be configured to be
    /// user-dismissable or persistent.
    /// </summary>
    public class InfoBar : System.Windows.Controls.ContentControl
    {
        public static readonly DependencyProperty IsClosableProperty =
            DependencyProperty.Register("IsClosable", typeof(bool), typeof(InfoBar),
                new PropertyMetadata(true));

        public static readonly DependencyProperty IsOpenProperty =
            DependencyProperty.Register("IsOpen", typeof(bool), typeof(InfoBar),
                new PropertyMetadata(false));

        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register("Message", typeof(string), typeof(InfoBar),
                new PropertyMetadata(""));

        public static readonly DependencyProperty SeverityProperty =
            DependencyProperty.Register("Severity", typeof(InfoBarSeverity), typeof(InfoBar),
                new PropertyMetadata(InfoBarSeverity.Informational));

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(InfoBar),
                new PropertyMetadata(""));

        /// <summary>
        /// Gets or sets a value that indicates whether the user can close the
        /// <see cref="InfoBar" />. Defaults to <c>true</c>.
        /// </summary>
        public bool IsClosable
        {
            get { return (bool)GetValue(IsClosableProperty); }
            set { SetValue(IsClosableProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether the
        /// <see cref="InfoBar" /> is open.
        /// </summary>
        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }

        /// <summary>
        /// Gets or sets the message of the <see cref="InfoBar" />.
        /// </summary>
        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        /// <summary>
        /// Gets or sets the type of the <see cref="InfoBar" /> to apply
        /// consistent status color, icon, and assistive technology settings
        /// dependent on the criticality of the notification.
        /// </summary>
        public InfoBarSeverity Severity
        {
            get { return (InfoBarSeverity)GetValue(SeverityProperty); }
            set { SetValue(SeverityProperty, value); }
        }

        /// <summary>
        /// Gets or sets the title of the <see cref="InfoBar" />.
        /// </summary>
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
    }
}
