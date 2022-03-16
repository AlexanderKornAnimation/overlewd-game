using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Overlewd
{
    public static class HttpCore
    {
        private const string ApiVersion = "3";

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
                if (token != null)
                {
                    request.SetRequestHeader("Authorization", "Bearer " + token);
                }
                request.SetRequestHeader("Version", ApiVersion);
            
                return await request.SendWebRequest();
            }
            catch (UnityWebRequestException e)
            {
                Debug.LogError(e.Message);
                return default;
            }
        }

        public static async Task<UnityWebRequest> PostAsync(string url, WWWForm form, string token = null)
        {
            try
            {
                var request = UnityWebRequest.Post(url, form);
                if (token != null)
                {
                    request.SetRequestHeader("Authorization", "Bearer " + token);
                }
                request.SetRequestHeader("Version", ApiVersion);
                return await request.SendWebRequest();
            }
            catch (UnityWebRequestException e)
            {
                Debug.LogError(e.Message);
                return default;
            }
        }
    }
}
