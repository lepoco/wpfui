namespace WPFUI.Controls
{
    internal interface IThemeElement
    {
        /// <summary>
        /// Indicates whether the application has a Mica effect applied at the moment.
        /// </summary>
        public int IsMica { get; }

        /// <summary>
        /// Indicates whether the application is in dark mode.
        /// </summary>
        public int IsDarkTheme { get; }
    }
}
