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
            DependencyProperty.Register(nameof(IsClosable), typeof(bool), typeof(InfoBar),
                new PropertyMetadata(true));

        public static readonly DependencyProperty IsOpenProperty =
            DependencyProperty.Register(nameof(IsOpen), typeof(bool), typeof(InfoBar),
                new PropertyMetadata(false));

        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register(nameof(Message), typeof(string), typeof(InfoBar),
                new PropertyMetadata(""));

        public static readonly DependencyProperty SeverityProperty =
            DependencyProperty.Register(nameof(Severity), typeof(InfoBarSeverity), typeof(InfoBar),
                new PropertyMetadata(InfoBarSeverity.Informational));

        /// <summary>
        /// Property for <see cref="TemplateButtonCommand"/>.
        /// </summary>
        public static readonly DependencyProperty TemplateButtonCommandProperty =
            DependencyProperty.Register(nameof(TemplateButtonCommand), typeof(Common.IRelayCommand), typeof(InfoBar),
                new PropertyMetadata(null));

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
        /// Gets the <see cref="Common.RelayCommand"/> triggered after clicking
        /// the close button.
        /// </summary>
        public Common.IRelayCommand TemplateButtonCommand => (Common.IRelayCommand)GetValue(TemplateButtonCommandProperty);

        /// <summary>
        /// Gets or sets the title of the <see cref="InfoBar" />.
        /// </summary>
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        /// <inheritdoc />
        public InfoBar()
        {
            SetValue(TemplateButtonCommandProperty,
                     new Common.RelayCommand(o => IsOpen = false));
        }
    }
}
