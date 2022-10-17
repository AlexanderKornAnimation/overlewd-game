using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Nutaku.Unity
{
    /// <summary>
    /// Utility functions for OAuth
    /// </summary>
    static class OAuthUtils
    {
        /// <summary>
        /// Generate OAuth Authorization header
        /// </summary>
        /// <param name="oauthKind">OAuth type</param>
        /// <param name="oauthParams">Consumer key, etc</param>
        /// <param name="method">HTTP Method</param>
        /// <param name="url">target URL for the request, without query parameters</param>
        /// <param name="queryParams">query parameters</param>
        /// <param name="additionalOAuthHeaderParams">Parameters to add to the Authorization header</param>
        /// <returns>String for the Authorization header: OAuth ..."</returns>
        public static string CreateAuthorizationHeader(
            OAuthKind oauthKind,
            OAuthParams oauthParams,
            string method,
            string url,
            IEnumerable<KeyValuePair<string, string>> queryParams = null,
            IEnumerable<KeyValuePair<string, string>> additionalOAuthHeaderParams = null)
        {
            if (oauthKind == OAuthKind.None)
                throw new ArgumentException("oauthKind must not be " + OAuthKind.None);
            if (string.IsNullOrEmpty(oauthParams.consumerKey))
                throw new ArgumentException("oauthParams.consumerKey must not be Null or Empty.");
            if (string.IsNullOrEmpty(oauthParams.consumerSecret))
                throw new ArgumentException("oauthParams.consumerSecret must not be Null or Empty.");
            if (oauthKind == OAuthKind.ThreeLegged)
            {
                if (string.IsNullOrEmpty(oauthParams.accessToken.token))
                    throw new ArgumentException("When using 3-legged OAuth, AccessToken.token must not be Null or Empty.");
                if (string.IsNullOrEmpty(oauthParams.accessToken.tokenSecret))
                    throw new ArgumentException("When using 3-legged OAuth, AccessToken.tokenSecret must not be Null or Empty.");
            }

            var oauthHeaderParams = new Dictionary<string, string>
            {
                { "oauth_consumer_key", oauthParams.consumerKey },
                { "oauth_nonce", System.Guid.NewGuid().ToString() },
                { "oauth_signature_method", "HMAC-SHA1" },
                {
                    "oauth_timestamp",
                    Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds).ToString()
                },
                { "oauth_version", "1.0" }
            };

            if (oauthKind == OAuthKind.ThreeLegged)
            {
                oauthHeaderParams.Add("oauth_token", oauthParams.accessToken.token);
            }

            if (additionalOAuthHeaderParams != null)
            {
                foreach (var param in additionalOAuthHeaderParams)
                {
                    oauthHeaderParams.Add(param.Key, param.Value);
                }
            }

            var thirdMemberParams = new List<string>();
            foreach (var kvp in oauthHeaderParams)
                if (kvp.Value != null)
                    thirdMemberParams.Add(Uri.EscapeDataString(kvp.Key) + "=" + Uri.EscapeDataString(kvp.Value));
            if (queryParams != null)
                foreach (var kvp in queryParams)
                    if (kvp.Value != null)
                        thirdMemberParams.Add(Uri.EscapeDataString(kvp.Key) + "=" + Uri.EscapeDataString(kvp.Value));
            thirdMemberParams.Sort();

            var thirdMember = string.Join("&", thirdMemberParams.ToArray());

            var signatureBaseString = Uri.EscapeDataString(method) + "&" + Uri.EscapeDataString(url) + "&" + Uri.EscapeDataString(thirdMember);

            var encoding = Encoding.UTF8;
            var hmac = new HMACSHA1(encoding.GetBytes(
                Uri.EscapeDataString(oauthParams.consumerSecret)
                + "&"
                + (oauthKind == OAuthKind.ThreeLegged ? Uri.EscapeDataString(oauthParams.accessToken.tokenSecret) : "")));
            var signature = Convert.ToBase64String(hmac.ComputeHash(encoding.GetBytes(signatureBaseString)));

            oauthHeaderParams.Add("oauth_signature", signature);

            var oauthHeaderStrings = new List<string>();

            foreach (var kvp in oauthHeaderParams)
                if (kvp.Value != null)
                    oauthHeaderStrings.Add(kvp.Key + "=\"" + Uri.EscapeDataString(kvp.Value) + "\"");

            oauthHeaderStrings.Sort();

            return "OAuth " + string.Join(",", oauthHeaderStrings.ToArray());
        }
    }
}
