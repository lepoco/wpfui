namespace WPFUI.Background
{
    /// <summary>
    /// Collection of available background effects.
    /// </summary>
    public enum BackgroundType
    {
        /// <summary>
        /// Leave as is.
        /// </summary>
        Default,

        /// <summary>
        /// Sets DWMWA_SYSTEMBACKDROP_TYPE to 0.
        /// </summary>
        Auto,

        /// <summary>
        /// Windows 11 Mica effect.
        /// </summary>
        Mica,

        /// <summary>
        /// Windows Acrylic effect.
        /// </summary>
        Acrylic,

        /// <summary>
        /// Windows 11 wallpaper blur effect.
        /// </summary>
        Tabbed
    }
}
