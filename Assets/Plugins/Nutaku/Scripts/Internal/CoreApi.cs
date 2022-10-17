using Nutaku.Unity.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;

namespace Nutaku.Unity
{
    /// <summary>
    /// Execution class of Nutaku Mobile API
    /// </summary>
    static class CoreApi
    {

        /// <summary>
        /// Execute the command 'Config.Get'
        /// </summary>
        public static void GetConfig(
            string consumerKey,
            string consumerSecret,
            MonoBehaviour myMonoBehaviour,
            UnityWebRequestUtil.callbackFunctionDelegate callbackFunctionDelegate)
        {
            PostCommand<ConfigGetResult>(
               "Config.Get",
               new Dictionary<string, object>() { { "versionCode", SdkPlugin.settings.versionCode }, { "sdkVersion", SdkPlugin.sdkVersion }  },
               consumerKey,
               consumerSecret,
               myMonoBehaviour,
               callbackFunctionDelegate);
        }

        /// <summary>
        /// AutoLogin for the sandbox environment
        /// </summary>
        public static void Autologin(
            string autologinToken,
            string consumerKey,
            string consumerSecret,
            MonoBehaviour myMonoBehaviour,
            UnityWebRequestUtil.callbackFunctionDelegate callbackFunctionDelegate)
        {
            if (!SdkPlugin.settings.IsSandbox())
            {
                throw new InvalidOperationException("this method can be called in only sandbox mode");
            }

            PostCommand<LoginResult>(
               "Login",
               new Dictionary<string, object>() {
                    { "autologin_token", autologinToken },
               },
               consumerKey,
               consumerSecret,
               myMonoBehaviour,
               callbackFunctionDelegate);
        }

        /// <summary>
        /// Execute command 'User.St' for security token
        /// </summary>
        public static void UserSt(
            string userId,
            string consumerKey,
            string consumerSecret,
            MonoBehaviour myMonoBehaviour,
            UnityWebRequestUtil.callbackFunctionDelegate callbackFunctionDelegate)
        {
            PostCommand<UserStResult>(
                "User.St",
                new Dictionary<string, object>()
                {
                    { "user_id", userId },
                },
                consumerKey,
                consumerSecret,
                myMonoBehaviour,
                callbackFunctionDelegate);
        }

        /// <summary>
        /// Send a Command to the API
        /// </summary>
        public static void PostCommand<TResult>(
            string command,
            IEnumerable<KeyValuePair<string, object>> commandParams,
            string consumerKey,
            string consumerSecret,
            MonoBehaviour myMonoBehaviour,
            UnityWebRequestUtil.callbackFunctionDelegate callbackFunctionDelegate)
        {
            var body = new Dictionary<string, object>()
            {
                { "command", command },
                { "params", commandParams },
            };

            RawRequest(
                 "POST",
                 SdkPlugin.coreApiInfo.getUri(),
                 null,
                 new Dictionary<string, string>() {
                    { "Content-Type", "application/json" },
                    { "User-Agent", "Nutaku Unity SDK" },
                 },
                 Encoding.UTF8.GetBytes(JsonMapper.ToJson(body)),
                 OAuthKind.TwoLegged,
                 new OAuthParams() { consumerKey = consumerKey, consumerSecret = consumerSecret },
                 null,
                 myMonoBehaviour,
                 callbackFunctionDelegate);
        }

        /// <summary>
        /// a call back function after sending a command to API
        /// </summary>
        /// <typeparam name="rawResult">The JSON response in RawResult format</typeparam>
        public static TResult HandlePostCommandCallback<TResult>(RawResult apiResult)
        {
            return JsonMapper.ToObject<TResult>(Encoding.UTF8.GetString(apiResult.body));
        }

        /// <summary>
        /// Login for Sandbox environment with credentials
        /// </summary>
        public static void Login(
            string email,
            string password,
            string consumerKey,
            string consumerSecret,
            MonoBehaviour myMonoBehaviour,
            UnityWebRequestUtil.callbackFunctionDelegate callbackFunctionDelegate)
        {
            if (!SdkPlugin.settings.IsSandbox())
            {
                throw new InvalidOperationException("this method can be called in only sandbox mode");
            }

            PostCommand<LoginResult>(
               "Login",
               new Dictionary<string, object>() {
                    { "email", email },
                    { "password", password },
               },
               consumerKey,
               consumerSecret,
               myMonoBehaviour,
               callbackFunctionDelegate);
        }

        /// <summary>
        /// Perform an HTTP request
        /// </summary>
        /// <param name="method">HTTP method. GET, POST, etc</param>
        /// <param name="url">target URL for the request, without query parameters</param>
        /// <param name="queryParams">query parameters</param>
        /// <param name="headers">The header in HTTP request</param>
        /// <param name="oauthKind">OAuth type</param>
        /// <param name="additionalOAuthHeaderParams">Parameters to add to the OAuth header</param>
        /// <param name="myMonoBehaviour">the parent monoBehaviour</param>
        /// <param name="callbackFunctionDelegate">Callback function to process the result</param>
        public static void RawRequest(
            string method,
            string url,
            IEnumerable<KeyValuePair<string, string>> queryParams,
            IEnumerable<KeyValuePair<string, string>> headers,
            byte[] body,
            OAuthKind oauthKind,
            OAuthParams oauthParams,
            IEnumerable<KeyValuePair<string, string>> additionalOAuthHeaderParams,
            MonoBehaviour myMonoBehaviour,
            UnityWebRequestUtil.callbackFunctionDelegate callbackFunctionDelegate)
        {
            try
            {
                if (queryParams == null)
                {
                    queryParams = new Dictionary<string, string>();
                }

                var builder = new UriBuilder(url)
                {
                    Query = QueryParameterBuilder.ToQueryString(queryParams)
                };
                var uri = builder.Uri;
                Dictionary<string, string> headersTmp = new Dictionary<string, string>();
                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        headersTmp.Add(header.Key, header.Value);
                    }
                }
                if (oauthKind != OAuthKind.None)
                {
                    headersTmp.Add("Authorization", OAuthUtils.CreateAuthorizationHeader(
                        oauthKind,
                        oauthParams,
                        method,
                        uri.GetLeftPart(UriPartial.Path),
                        queryParams,
                        additionalOAuthHeaderParams));
                }
                DumpRequestBody(body);
                UnityWebRequestUtil unityWebRequestUtilInstance = new UnityWebRequestUtil();
                if (body == null)
                {
                    unityWebRequestUtilInstance.StartSendingRawRequest(method, myMonoBehaviour, headersTmp, uri.ToString(), "", callbackFunctionDelegate);
                }
                else
                {
                    unityWebRequestUtilInstance.StartSendingRawRequest(method, myMonoBehaviour, headersTmp, uri.ToString(), Encoding.UTF8.GetString(body), callbackFunctionDelegate);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        static CoreApi()
        {
            ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) =>
            {
                return true;
            };
        }

        public static byte[] ToBytesFromStream(Stream stream)
        {
            if (stream == null)
            {
                return null;
            }

            using (var mstrm = new MemoryStream())
            {
                var buffer = new byte[4096];
                int read;
                while ((read = stream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    mstrm.Write(buffer, 0, read);
                }
                return mstrm.ToArray();
            }
        }

        [Conditional("DEBUG")]
        public static void DumpRequestHeaderFields(HttpWebRequest req)
        {
            UnityEngine.Debug.LogFormat("## Request ##\n{0} {1}", req.Method, req.RequestUri.ToString());

            var headers = req.Headers;

            List<string> headerStrings = new List<string>();
            for (var i = 0; i < headers.Count; i++)
                headerStrings.Add(headers.GetKey(i) + ": " + headers[i]);

            UnityEngine.Debug.LogFormat("### Request Header Fields ###\n{0}", string.Join("\n", headerStrings.ToArray()));
        }

        [Conditional("DEBUG")]
        public static void DumpRequestBody(byte[] body)
        {
            UnityEngine.Debug.LogFormat("### Request Body ###\n{0}", body != null ? Encoding.UTF8.GetString(body) : "");
        }

        [Conditional("DEBUG")]
        public static void DumpResponseHeaderFields(HttpWebResponse res)
        {
            UnityEngine.Debug.LogFormat("## Response ##\nHttp Status Code: {0}", (int)res.StatusCode);

            var headers = res.Headers;

            List<string> headerStrings = new List<string>();
            for (var i = 0; i < headers.Count; i++)
                headerStrings.Add(headers.GetKey(i) + ": " + headers[i]);

            UnityEngine.Debug.LogFormat("### Response Header Fields ###\n{0}", string.Join("\n", headerStrings.ToArray()));
        }

        [Conditional("DEBUG")]
        public static void DumpResponseBody(byte[] body)
        {
            UnityEngine.Debug.LogFormat("### Response Body ###\n{0}", body != null ? Encoding.UTF8.GetString(body) : "");
        }
    }
}
