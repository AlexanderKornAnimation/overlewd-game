using System.Collections.Generic;

namespace Nutaku.Unity
{
    /// <summary>
	/// Structure that stores WebView events such as friend invitation screen and payment screen.
    /// </summary>
    public struct WebViewEvent
    {
        /// <summary>
        /// Event type
        /// </summary>
        public WebViewEventKind kind;

        /// <summary>
		/// WebView processing result, in case of an array of string values
		/// User IDs of the friends who are invited, etc
        /// </summary>
        public List<string> values;

        /// <summary>
		/// WebView processing result, in case of a singular string value
		/// Payment ID, etc
        /// </summary>
        public string value;

        /// <summary>
		/// Event message (if any).
        /// </summary>
        public string message;
    }
}
