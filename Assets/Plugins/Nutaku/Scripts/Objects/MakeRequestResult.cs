using System.Collections.Generic;

namespace Nutaku.Unity
{
    /// <summary>
	/// Store the result of MakeRequest API.
    /// </summary>
    public class MakeRequestResult
    {
        /// <summary>
		/// HTTP Status Code of the response of the API itself
        /// </summary>
        public int statusCode;

        /// <summary>
		/// HTTP Status Code of the response of the callback URL
        /// </summary>
        public int rc;

        /// <summary>
		/// Message Body of the response from the callback URL
        /// </summary>
        public string body;

        /// <summary>
		/// Headers of the response from the callback URL
        /// </summary>
        public Dictionary<string, string> headers;
    }
}
