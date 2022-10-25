using System.Collections.Generic;
using UnityEngine;

namespace Nutaku.Unity
{
    public static class RestApiHelper
    {
        /// <summary>
        /// Retrieve the profile of the specified user.
        /// </summary>
        /// <param name="userId">ID of the user to acquire</param>
        /// <param name="queryParams"></param>
        /// <param name="myMonoBehaviour">the parent monoBehaviour</param>
        /// <param name="callbackFunctionDelegate">Callback function to process the result</param>
        /// <param name="queryParams">Optional for specifying the profile fields to be retrieved</param>
        public static void GetProfile(
            string userId,
            MonoBehaviour myMonoBehaviour,
            UnityWebRequestUtil.callbackFunctionDelegate callbackFunctionDelegate,
            PeopleQueryParameterBuilder queryParams = null)
        {
            RestApi.GetPeople(
               userId,
               RestApi.Selector.Self,
               null,
               queryParams?.Build(),
               OAuthKind.TwoLegged,
               myMonoBehaviour,
               callbackFunctionDelegate);
        }

        /// <summary>
        /// Get the profile list of followers of a specified user.
        /// </summary>
        /// <param name="userId">The user who's follower list we are requesting</param>
        /// <param name="myMonoBehaviour">the parent monoBehaviour</param>
        /// <param name="callbackFunctionDelegate">Callback function to process the result</param>
        /// <param name="queryParams">Optional for pagination and specifying the profile fields to be retrieved</param>
        public static void GetFriends(
            string userId,
            MonoBehaviour myMonoBehaviour,
            UnityWebRequestUtil.callbackFunctionDelegate callbackFunctionDelegate,
            PeopleQueryParameterBuilder queryParams = null)
        {
            RestApi.GetPeople(
               userId,
               RestApi.Selector.Friends,
               null,
               queryParams?.Build(),
               OAuthKind.TwoLegged,
               myMonoBehaviour,
               callbackFunctionDelegate);
        }

        /// <summary>
        /// Get the Payment object for a payment that has been registered on the server
        /// </summary>
        /// <param name="userId">ID of the user who made the payment</param>
        /// <param name="paymentId">ID of the payment</param>
        /// <param name="queryParams">Not relevant for this function</param>
        /// <param name="myMonoBehaviour">the parent monoBehaviour</param>
        /// <param name="callbackFunctionDelegate">Callback function to process the result</param>
        /// <param name="queryParams">Optional for specifying the profile fields to be retrieved</param>
        public static void GetPayment(
            string userId,
            string paymentId,
            MonoBehaviour myMonoBehaviour,
            UnityWebRequestUtil.callbackFunctionDelegate callbackFunctionDelegate,
            PaymentQueryParameterBuilder queryParams = null)
        {
            RestApi.GetPayment(
                userId,
                paymentId,
                queryParams?.Build(),
                myMonoBehaviour,
                callbackFunctionDelegate);
        }

        public static void PostMakeRequest(
            string callbackUrl,
            IEnumerable<KeyValuePair<string, string>> postData,
            MonoBehaviour myMonoBehaviour,
            UnityWebRequestUtil.callbackFunctionDelegate callbackFunctionDelegate)
        {
            RestApi.PostMakeRequest(
               callbackUrl,
               postData,
               null,
               myMonoBehaviour,
               callbackFunctionDelegate);
        }

        /// <summary>
        /// Get the profile of the user currently playing the game
        /// </summary>
        public static void GetMyProfile(
            MonoBehaviour myMonoBehaviour,
            UnityWebRequestUtil.callbackFunctionDelegate callbackFunctionDelegate,
            PeopleQueryParameterBuilder queryParams = null)
        {
            RestApi.GetPeople(
               RestApi.Guid.Me,
               RestApi.Selector.Self,
               null,
               queryParams?.Build(),
               OAuthKind.ThreeLegged,
               myMonoBehaviour,
               callbackFunctionDelegate);
        }

        /// <summary>
        /// Get the profile list of followers of the current user
        /// </summary>
        /// <param name="myMonoBehaviour">the parent monoBehaviour</param>
        /// <param name="callbackFunctionDelegate">Callback function to process the result</param>
        /// <param name="queryParams">Optional for specifying the profile fields to be retrieved</param>
        public static void GetMyFriends(
            MonoBehaviour myMonoBehaviour,
            UnityWebRequestUtil.callbackFunctionDelegate callbackFunctionDelegate,
            PeopleQueryParameterBuilder queryParams = null)
        {
            RestApi.GetPeople(
               RestApi.Guid.Me,
               RestApi.Selector.Friends,
               null,
               queryParams?.Build(),
               OAuthKind.ThreeLegged,
               myMonoBehaviour,
               callbackFunctionDelegate);
        }

        /// <summary>
        /// Initiates the payment creation process. Use the result object to continue with the payment flow.
        /// </summary>
        /// <param name="payment">The Payment object that contains all the payment creation information</param>
        /// <param name="queryParams">Not relevant for this function</param>
        /// <param name="myMonoBehaviour">the parent monoBehaviour</param>
        /// <param name="callbackFunctionDelegate">Callback function to process the result</param>
        /// <param name="queryParams">Optional for specifying the profile fields to be retrieved</param>
        public static void PostPayment(
            Payment payment,
            MonoBehaviour myMonoBehaviour,
            UnityWebRequestUtil.callbackFunctionDelegate callbackFunctionDelegate,
            PaymentQueryParameterBuilder queryParams = null)
        {
            RestApi.PostPayment(
                queryParams?.Build(),
                payment,
                myMonoBehaviour,
                callbackFunctionDelegate);
        }
    }
}
