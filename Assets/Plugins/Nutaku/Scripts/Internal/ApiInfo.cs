using System;

namespace Nutaku.Unity
{
    /// <summary>
    /// API path information
    /// </summary>
    public class ApiInfo
    {
        /// <summary>
        /// scheme
        /// </summary>
        public string scheme { get; private set; }

        /// <summary>
        /// host
        /// </summary>
        public string host { get; private set; }

        /// <summary>
        /// endpoint
        /// </summary>
        public string endpoint { get; private set; }

        public ApiInfo() { }

        public ApiInfo(string scheme, string host, string endpoint)
        {
            this.scheme = scheme;
            this.host = host;
            this.endpoint = endpoint;
        }

        public UriBuilder getUriBuilder()
        {
            var uriBuilder = new UriBuilder();
            uriBuilder.Scheme = scheme;
            uriBuilder.Host = host;
            uriBuilder.Path = endpoint;
            return uriBuilder;
        }

        public string getUri()
        {
            return getUriBuilder().Uri.ToString();
        }
    }
}
