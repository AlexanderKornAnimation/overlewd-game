using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Overlewd
{
    public static class HttpCore
    {
#if !UNITY_EDITOR && !DEV_BUILD
        public const string ApiVersion = "12"; //active api version
#else
        public const string ApiVersion = "13"; //dev api version
#endif

        public static bool HasNetworkConection()
        {
            return Application.internetReachability != NetworkReachability.NotReachable;
        }
        public static bool NetworkTypeMobile()
        {
            return Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork;
        }
        public static bool NetworkTypeWIFI()
        {
            return Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork;
        }

        public static async Task<UnityWebRequest> GetAsync(string url,string token = null,
            bool lockUserInput = true)
        {
            try
            {
                var request = UnityWebRequest.Get(url);
                if (lockUserInput)
                {
                    UIManager.AddUserInputLocker(new UserInputLocker(request));
                }

                if (token != null)
                {
                    request.SetRequestHeader("Authorization", "Bearer " + token);
                }
                request.SetRequestHeader("Version", ApiVersion);
            
                await request.SendWebRequest();
                UIManager.RemoveUserInputLocker(new UserInputLocker(request));

                return request;
            }
            catch (UnityWebRequestException e)
            {
                UIManager.RemoveUserInputLocker(new UserInputLocker(e.UnityWebRequest));

                Debug.LogError(e.UnityWebRequest.url);
                Debug.LogError(e.Message);
                return default;
            }
        }

        public static async Task<UnityWebRequest> PostAsync(string url, WWWForm form,
            string token = null, bool lockUserInput = true)
        {
            try
            {
                var request = UnityWebRequest.Post(url, form);
                if (lockUserInput)
                {
                    UIManager.AddUserInputLocker(new UserInputLocker(request));
                }

                if (token != null)
                {
                    request.SetRequestHeader("Authorization", "Bearer " + token);
                }
                request.SetRequestHeader("Version", ApiVersion);

                await request.SendWebRequest();
                UIManager.RemoveUserInputLocker(new UserInputLocker(request));

                return request;
            }
            catch (UnityWebRequestException e)
            {
                UIManager.RemoveUserInputLocker(new UserInputLocker(e.UnityWebRequest));

                Debug.LogError(e.UnityWebRequest.url);
                Debug.LogError(e.Message);
                return default;
            }
        }

        public static async Task<UnityWebRequest> DeleteAsync(string url, string token = null,
            bool lockUserInput = true)
        {
            try
            {
                var request = UnityWebRequest.Delete(url);
                if (lockUserInput)
                {
                    UIManager.AddUserInputLocker(new UserInputLocker(request));
                }

                if (token != null)
                {
                    request.SetRequestHeader("Authorization", "Bearer " + token);
                }
                request.SetRequestHeader("Version", ApiVersion);

                await request.SendWebRequest();
                UIManager.RemoveUserInputLocker(new UserInputLocker(request));

                return request;
            }
            catch (UnityWebRequestException e)
            {
                UIManager.RemoveUserInputLocker(new UserInputLocker(e.UnityWebRequest));

                Debug.LogError(e.UnityWebRequest.url);
                Debug.LogError(e.Message);
                return default;
            }
        }
    }
}
