namespace Nutaku.Unity
{
    /// <summary>
	/// Structure that stores the content of HTTP response.
    /// </summary>
    public struct RawResult
    {
        /// <summary>
        /// HTTP status code
        /// </summary>
        public int statusCode;

        /// <summary>
        /// HTTP response body
        /// </summary>
        public byte[] body;
    }
}
