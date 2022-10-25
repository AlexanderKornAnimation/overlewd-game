namespace Nutaku.Unity
{
    /// <summary>
	/// Structure of the access token used by OAuth for 3-legged requests
    /// </summary>
    public struct AccessToken
    {
        public string token;

        public string tokenSecret;

        public AccessToken(string token, string tokenSecret)
        {
            this.token = token;
            this.tokenSecret = tokenSecret;
        }
    }
}
