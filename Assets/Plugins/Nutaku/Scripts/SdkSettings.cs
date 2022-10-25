namespace Nutaku.Unity
{
    /// <summary>
    /// SDK Settings
    /// </summary>
    public class SdkSettings
    {
        /// <summary>
        /// Nutaku App ID
        /// </summary>
        public string appId { get; private set; }

        /// <summary>
        /// Environment
        /// </summary>
        public string environment { get; private set; }

        /// <summary>
        /// Is Development mode?
        /// </summary>
        public bool isDevelopmentMode { get; private set; }

        /// <summary>
		/// Version Code
        /// </summary>
        public int versionCode { get; private set; }

        /// <summary>
        /// Consumer Key for OAuth
        /// </summary>
        public string consumerKey { get; private set; }

        /// <summary>
        /// Consumer Key for OAuth
        /// </summary>
        public string consumerSecret { get; private set; }

        /// <summary>
		/// Returns true if the development mode is sandbox
        /// </summary>
        public bool IsSandbox()
        {
            return environment.ToLower().Contains("sandbox");
        }

        public SdkSettings(
            string appId,
            string environment,
            bool isDevelopmentMode,
            int versionCode,
            string consumerKey,
            string consumerSecret)
        {
            this.appId = appId;
            this.environment = environment.ToLower();
            this.isDevelopmentMode = isDevelopmentMode;
            this.versionCode = versionCode;
            this.consumerKey = consumerKey;
            this.consumerSecret = consumerSecret;
        }

        public SdkSettings() { }
    }
}
