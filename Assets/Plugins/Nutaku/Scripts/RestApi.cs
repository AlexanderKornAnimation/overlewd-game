using Nutaku.Unity.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Nutaku.Unity
{
    public static class RestApi
    {
        private static string makeRequest_callbackUrl;
        private static IEnumerable<KeyValuePair<string, string>> makeRequest_postData;
        private static IEnumerable<KeyValuePair<string, string>> makeRequest_queryParams;
        private static OAuthKind makeRequest_oauthKind;
        private static MonoBehaviour makeRequest_myMonoBehaviour;
        private static UnityWebRequestUtil.callbackFunctionDelegate makeRequest_callbackFunctionDelegate;
        private static OAuthParams makeRequest_oauthParams;

        public static class Guid
        {
            /// <summary>
            /// Indicate the ID of the Nutaku user who is running the game.
            /// </summary>
            public const string Me = "@me";
        }

        public static class Selector
        {
            /// <summary>
            /// Indicates that the user's own profile information specified by the guid parameter is to be acquired.
            /// </summary>
            public const string Self = "@self";

            /// <summary>
            /// Indicates that the friend information of the user specified by the guid parameter is to be acquired.
            /// </summary>
            public const string Friends = "@friends";

            /// <summary>
            /// Same as Friends
            /// </summary>
            public const string All = "@all";
        }

        public static class AppId
        {
            /// <summary>
            /// Indicates the ID of the game being executed.
            /// </summary>
            public const string App = "@app";
        }

        /// <summary>
        /// People API.
        /// </summary>
        /// <param name="guid">Required. Guid.ME or a Nutaku user ID.</param>
        /// <param name="selector">Required. Selector.SELF, Selector.FRIENDS or Selector.ALL</param>
        /// <param name="pid">Optional Nutaku user ID, specified if selector is Selector.FRIENDS or Selector.ALL.</param>
        /// <param name="queryParams">Optional. Query parameters.</param>
        /// <param name="oauthKind">OAuth type</param>
        /// <param name="myMonoBehaviour">the parent monoBehaviour</param>
        /// <param name="callbackFunctionDelegate">Callback function to process the result</param>
        public static void GetPeople(
            string guid,
            string selector,
            string pid,
            IEnumerable<KeyValuePair<string, string>> queryParams,
            OAuthKind oauthKind,
            MonoBehaviour myMonoBehaviour,
            UnityWebRequestUtil.callbackFunctionDelegate callbackFunctionDelegate)
        {
            if (string.IsNullOrEmpty(guid))
                throw new ArgumentException("guid must not be Null or Empty");
            if (string.IsNullOrEmpty(selector))
                throw new ArgumentException("selector must not be Null or Empty");
            if (oauthKind == OAuthKind.None)
                throw new ArgumentException("oauthKind must not be " + OAuthKind.None);

            Request<Person>(
                "GET",
                "people",
                new List<string> { guid, selector, pid },
                queryParams,
                null,
                (oauthKind == OAuthKind.ThreeLegged || guid == Guid.Me) ? SdkPlugin.loginInfo.userId : null,
                oauthKind,
                myMonoBehaviour,
                callbackFunctionDelegate);
        }

        public static void GetPayment(
            string guid,
            string paymentId,
            IEnumerable<KeyValuePair<string, string>> queryParams,
            MonoBehaviour myMonoBehaviour,
            UnityWebRequestUtil.callbackFunctionDelegate callbackFunctionDelegate)
        {
            if (string.IsNullOrEmpty(guid))
                throw new ArgumentException("guid must not be Null or Empty");
            if (guid == Guid.Me)
                throw new ArgumentException("guid must not be " + Guid.Me);
            if (string.IsNullOrEmpty(paymentId))
                throw new ArgumentException("paymentId must not be Null or Empty");

            Request<Payment>(
                "GET",
                "payment",
                new List<string> { guid, Selector.Self, AppId.App, paymentId },
                queryParams,
                null,
                (guid == Guid.Me) ? SdkPlugin.loginInfo.userId : null,
                OAuthKind.TwoLegged,
                myMonoBehaviour,
                callbackFunctionDelegate);
        }

        public static void PostPayment(
            IEnumerable<KeyValuePair<string, string>> queryParams,
            Payment payment,
            MonoBehaviour myMonoBehaviour,
            UnityWebRequestUtil.callbackFunctionDelegate callbackFunctionDelegate)
        {
            if (payment == null)
                throw new ArgumentNullException("payment");
            if (payment.paymentItems == null || payment.paymentItems.Count == 0)
                throw new Exception("You have not defined any item to buy");
            else if (payment.paymentItems.Count > 1)
                throw new Exception("Count of PaymentItems is no longer allowed to be higher than 1. Please use the bundle purchase approach for offering multiple things.");
            if (payment.paymentItems[0].quantity > 1)
                throw new Exception("Quantity property of a PaymentItem is no longer allowed to be higher than 1. Please use the bundle purchase approach for offering multiple things.");
            payment.paymentItems[0].quantity = 1;

            Request<Payment>(
               "POST",
               "payment",
               new List<string> { Guid.Me, Selector.Self, AppId.App },
               queryParams,
               payment,
               SdkPlugin.loginInfo.userId,
               OAuthKind.ThreeLegged,
               myMonoBehaviour,
               callbackFunctionDelegate);
        }

        public static void PostMakeRequest(
            string callbackUrl,
            IEnumerable<KeyValuePair<string, string>> postData,
            IEnumerable<KeyValuePair<string, string>> queryParams,
            MonoBehaviour myMonoBehaviour,
            UnityWebRequestUtil.callbackFunctionDelegate callbackFunctionDelegate)
        {
            if (string.IsNullOrEmpty(callbackUrl))
                throw new ArgumentException("callbackUrl must not be Null or Empty");

            var settings = SdkPlugin.settings;
            var loginInfo = SdkPlugin.loginInfo;
            var accessToken = loginInfo.accessToken;
            var oauthParams = new OAuthParams()
            {
                consumerKey = settings.consumerKey,
                consumerSecret = settings.consumerSecret,
                accessToken = accessToken,
            };

            var securityToken = SdkPlugin.loginInfo.securityToken;
            
            RestApi.makeRequest_callbackUrl = callbackUrl;
            RestApi.makeRequest_postData = postData;
            RestApi.makeRequest_queryParams = queryParams;
            RestApi.makeRequest_oauthKind = OAuthKind.TwoLegged;
            RestApi.makeRequest_myMonoBehaviour = myMonoBehaviour;
            RestApi.makeRequest_callbackFunctionDelegate = callbackFunctionDelegate;
            RestApi.makeRequest_oauthParams = oauthParams;

            if (securityToken.token == null || securityToken.dateTime.AddHours(SecurityToken.ValidDurationHours) <= DateTime.Now)
            {
                CoreApi.UserSt(
                   loginInfo.userId,
                   settings.consumerKey,
                   settings.consumerSecret,
                   myMonoBehaviour,
                   PostMakeRequestObtainSTCallback);
            }
            else
            {
                var endpoint = new ApiInfo(
                    SdkPlugin.socialApiInfo.scheme,
                    SdkPlugin.socialApiInfo.host,
                    "/gadgets/makeRequest"
                ).getUri();

                var postDataStrings = new List<string>();
                foreach (KeyValuePair<string, string> kvp in postData)
                    if (!string.IsNullOrEmpty(kvp.Key))
                        postDataStrings.Add(kvp.Key + "=" + kvp.Value);

                var bodyParams = new Dictionary<string, string>
                {
                    ["url"] = callbackUrl,
                    ["httpMethod"] = "POST",
                    ["headers"] = "application/x-www-form-urlencoded",
                    ["postData"] = string.Join("&", postDataStrings.ToArray()),
                    ["authz"] = "signed",
                    ["st"] = SdkPlugin.loginInfo.securityToken.token,
                    ["contentType"] = "JSON",
                    ["numEntries"] = "3",
                    ["getSummaries"] = "false",
                    ["signOwner"] = "true",
                    ["signViewer"] = "true",
                    ["container"] = "nutaku",
                    ["bypassSpecCache"] = "",
                    ["getFullHeaders"] = "false",
                    ["oauthState"] = ""
                };

                var bodyStrings = new List<string>();
                foreach (KeyValuePair<string, string> kvp in bodyParams)
                    if (kvp.Value != null)
                        bodyStrings.Add(kvp.Key + "=" + (kvp.Key == "st" ? kvp.Value : Uri.EscapeDataString(kvp.Value)));

                var body = Encoding.UTF8.GetBytes(string.Join("&", bodyStrings.ToArray()));

                CoreApi.RawRequest(
                    "POST",
                    endpoint,
                    queryParams,
                    new Dictionary<string, string>() { { "Content-Type", "application/x-www-form-urlencoded" } },
                    body,
                    OAuthKind.TwoLegged,
                    oauthParams,
                    null,
                    myMonoBehaviour,
                    callbackFunctionDelegate);
            }
        }

        private static void PostMakeRequestObtainSTCallback(RawResult rawResult)
        {
            var updateSt = CoreApi.HandlePostCommandCallback<UserStResult>(rawResult);

            if (updateSt.code != "ok")
                throw new WebException("Failed to update security token. error_no:" + updateSt.error_no.ToString());
            SdkPlugin.UpdateSecurityToken(new SecurityToken(updateSt.result.st, DateTime.Now));

            var endpoint = new ApiInfo(
                   SdkPlugin.socialApiInfo.scheme,
                   SdkPlugin.socialApiInfo.host,
                   "/gadgets/makeRequest"
               ).getUri();

            var postDataStrings = new List<string>();
            foreach (KeyValuePair<string, string> kvp in makeRequest_postData)
                if (!string.IsNullOrEmpty(kvp.Key))
                    postDataStrings.Add(kvp.Key + "=" + kvp.Value);

            var bodyParams = new Dictionary<string, string>
            {
                ["url"] = makeRequest_callbackUrl,
                ["httpMethod"] = "POST",
                ["headers"] = "application/x-www-form-urlencoded",
                ["postData"] = string.Join("&", postDataStrings.ToArray()),
                ["authz"] = "signed",
                ["st"] = SdkPlugin.loginInfo.securityToken.token,
                ["contentType"] = "JSON",
                ["numEntries"] = "3",
                ["getSummaries"] = "false",
                ["signOwner"] = "true",
                ["signViewer"] = "true",
                ["container"] = "nutaku",
                ["bypassSpecCache"] = "",
                ["getFullHeaders"] = "false",
                ["oauthState"] = ""
            };

            var bodyStrings = new List<string>();
            foreach (KeyValuePair<string, string> kvp in bodyParams)
                if (kvp.Value != null)
                    bodyStrings.Add(kvp.Key + "=" +
                                    (kvp.Key == "st" ? kvp.Value : Uri.EscapeDataString(kvp.Value)));

            var body = Encoding.UTF8.GetBytes(string.Join("&", bodyStrings.ToArray()));

            CoreApi.RawRequest(
                "POST",
                endpoint,
                makeRequest_queryParams,
                new Dictionary<string, string>() { { "Content-Type", "application/x-www-form-urlencoded" } },
                body,
                makeRequest_oauthKind,
                makeRequest_oauthParams,
                null,
                makeRequest_myMonoBehaviour,
                makeRequest_callbackFunctionDelegate);
        }

        public static MakeRequestResult HandlePostMakeRequestCallback
               (RawResult rawResult)
        {
            const string pattern = @"throw 1; < don't be evil' >{""(?:.+?)"":(?<json>{.+})}";

            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            Match match = regex.Match(Encoding.UTF8.GetString(rawResult.body));

            if (!match.Success)
                throw new WebException("TODO");

            var json = match.Groups["json"].Value;
            var result = JsonMapper.ToObject<MakeRequestResult>(json);
            result.statusCode = rawResult.statusCode;
            return result;
        }

        /// <summary>
        /// Request API to handle Request before sending to server
        /// </summary>
        /// <typeparam name="TResult">Result Type</typeparam>
        /// <param name="method">HTTP Method</param>
        /// <param name="apiEndpoint">API Endpoint</param>
        /// <param name="templateParams">Template parameters</param>
        /// <param name="queryParams">Query parameters</param>
        /// <param name="body">Request Body</param>
        /// <param name="requestorId">UserID for the OAuth header</param>
        /// <param name="oauthKind">OAuth kind</param>
        /// <param name="myMonoBehaviour">the parent monoBehaviour</param>
        /// <param name="callbackFunctionDelegate">Callback function to process the result</param>
        public static void Request<TResult>(
            string method,
            string apiEndpoint,
            IEnumerable<string> templateParams,
            IEnumerable<KeyValuePair<string, string>> queryParams,
            object body,
            string requestorId,
            OAuthKind oauthKind,
            MonoBehaviour myMonoBehaviour,
            UnityWebRequestUtil.callbackFunctionDelegate callbackFunctionDelegate)
        {
            var basePath = SdkPlugin.socialApiInfo;
            var settings = SdkPlugin.settings;
            var accessToken = SdkPlugin.loginInfo.accessToken;
            var oauthParams = new OAuthParams()
            {
                consumerKey = settings.consumerKey,
                consumerSecret = settings.consumerSecret,
                accessToken = accessToken,
            };

            var builder = new UriBuilder
            {
                Scheme = basePath.scheme,
                Host = basePath.host
            };

            List<string> paths = new List<string>()
            {
                basePath.endpoint,
                apiEndpoint,
            };
            if (templateParams != null)
            {
                foreach (var templateParam in templateParams)
                    if (!string.IsNullOrEmpty(templateParam))
                        paths.Add(templateParam);
            }
            builder.Path = string.Join("/", paths.ToArray()).Replace("//", "/");

            Dictionary<string, string> additionalOAuthHeaderParams = null;
            if (!string.IsNullOrEmpty(requestorId))
            {
                additionalOAuthHeaderParams = new Dictionary<string, string>
                {
                    { "xoauth_requestor_id", requestorId }
                };
            }

            CoreApi.RawRequest(
                method,
                builder.Uri.GetLeftPart(UriPartial.Path),
                queryParams,
                body == null ? null : new Dictionary<string, string>() { { "Content-Type", "application/json" } },
                body == null ? null : Encoding.UTF8.GetBytes(JsonMapper.ToJson(body)),
                oauthKind,
                oauthParams,
                additionalOAuthHeaderParams,
                myMonoBehaviour,
                callbackFunctionDelegate);
        }

        public static RestApiResult<TResult> HandleRequestCallback<TResult>(RawResult apiResult)
        {
            try
            {
                var json = Encoding.UTF8.GetString(apiResult.body);
                var root = JsonMapper.ToObject(json);
                RestApiResult<TResult> result;
                if (root.Keys.Contains("entry"))
                {
                    var entry = root["entry"];
                    if (entry.IsArray)
                        result = JsonMapper.ToObject<RestApiResult<TResult>>(json);
                    else if (entry.IsObject)
                        result = JsonMapper.ToObject<RestApiSimplexResult<TResult>>(json);
                    else
                        result = new RestApiResult<TResult>();
                }
                else
                    result = new RestApiResult<TResult>();
                result.statusCode = apiResult.statusCode;
                return result;
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.Log("Error:" + ex.StackTrace);
                throw ex;
            }
        }
    }
}
