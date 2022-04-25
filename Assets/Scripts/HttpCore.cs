using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Overlewd
{
    public static class HttpCore
    {
        public const string ApiVersion = "4";

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

        public static async Task<UnityWebRequest> GetAsync(string url, string token = null)
        {
            try
            {
                var request = UnityWebRequest.Get(url);
                UIManager.AddUserInputLocker(new UserInputLocker(request));

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

        public static async Task<UnityWebRequest> PostAsync(string url, WWWForm form, string token = null)
        {
            try
            {
                var request = UnityWebRequest.Post(url, form);
                UIManager.AddUserInputLocker(new UserInputLocker(request));

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

        public static async Task<UnityWebRequest> PostAsync(string url, string postData, string token = null)
        {
            try
            {
                var request = UnityWebRequest.Post(url, postData);
                UIManager.AddUserInputLocker(new UserInputLocker(request));

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

        public static async Task<UnityWebRequest> DeleteAsync(string url, string token = null)
        {
            try
            {
                var request = UnityWebRequest.Delete(url);
                UIManager.AddUserInputLocker(new UserInputLocker(request));

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
