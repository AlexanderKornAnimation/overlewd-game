namespace Nutaku.Unity
{
    /// <summary>
	/// Class that stores login information.
    /// </summary>
    public class LoginInfo
    {
        /// <summary>
        /// Nutaku user ID
        /// </summary>
        public string userId { get; private set; }

        /// <summary>
		/// Access Token (for 3-legged requests)
        /// </summary>
        public AccessToken accessToken { get; private set; }

        /// <summary>
		/// Security Token (for makeRequest)
        /// </summary>
        public SecurityToken securityToken { get; private set; }

        public LoginInfo(string userId, AccessToken accessToken)
        {
            this.userId = userId;
            this.accessToken = accessToken;
        }

        public LoginInfo(
            string userId,
            AccessToken accessToken,
            SecurityToken securityToken)
            : this(userId, accessToken)
        {
            this.securityToken = securityToken;
        }

        public LoginInfo() { }
    }
}
