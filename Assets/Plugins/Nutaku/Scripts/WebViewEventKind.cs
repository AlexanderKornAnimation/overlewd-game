namespace Nutaku.Unity
{
    /// <summary>
	/// WebView event type
    /// </summary>
    public enum WebViewEventKind
    {
        /// <summary>
		/// WebView processing succeeded
        /// </summary>
        Succeeded = 1,

        /// <summary>
		/// WebView processing failed
        /// </summary>
        Failed = 2,

        /// <summary>
		/// WebView closed by user's action
        /// </summary>
        Cancelled = 3,
    }
}
