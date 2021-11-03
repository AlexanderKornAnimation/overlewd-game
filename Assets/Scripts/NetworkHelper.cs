using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Overlewd
{
    public static class NetworkHelper
    {
        private static async Task WaitRequestDoneAsync(UnityWebRequest request)
        {
            while (!request.isDone)
            {
                await Task.Delay(10);
            }
        }

        private static bool RequestCheckError(UnityWebRequest request)
        {
            if (request.result != UnityWebRequest.Result.ProtocolError &&
                request.result != UnityWebRequest.Result.ConnectionError)
            {
                return false;
            }
            return true;
        }

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
            var request = UnityWebRequest.Get(url);
            if (token != null)
            {
                request.SetRequestHeader("Authorization", "Bearer " + token);
            }
            request.SendWebRequest();

            await WaitRequestDoneAsync(request);

            return request;
        }

        public static async Task<UnityWebRequest> PostAsync(string url, WWWForm form, string token = null)
        {
            var request = UnityWebRequest.Post(url, form);
            if (token != null)
            {
                request.SetRequestHeader("Authorization", "Bearer " + token);
            }
            request.SendWebRequest();

            await WaitRequestDoneAsync(request);

            return request;
        }

        public static async Task<AudioClip> LoadAudioClipAsync(string url, AudioType audioType)
        {
            using (var request = UnityWebRequestMultimedia.GetAudioClip(url, audioType))
            {
                request.SendWebRequest();

                await WaitRequestDoneAsync(request);

                if (!RequestCheckError(request))
                {
                    return DownloadHandlerAudioClip.GetContent(request);
                }
                return null;
            }
        }

        public static async Task<Texture2D> LoadTexture2DAsync(string url)
        {
            using (var request = UnityWebRequestTexture.GetTexture(url))
            {
                request.SendWebRequest();

                await WaitRequestDoneAsync(request);

                if (!RequestCheckError(request))
                {
                    return DownloadHandlerTexture.GetContent(request);
                }
                return null;
            }
        }

        public static async Task<AssetBundle> LoadAssetBundleAsync(string url)
        {
            using (var request = UnityWebRequestAssetBundle.GetAssetBundle(url))
            {
                request.SendWebRequest();

                await WaitRequestDoneAsync(request);

                if (!RequestCheckError(request))
                {
                    return DownloadHandlerAssetBundle.GetContent(request);
                }
                return null;
            }
        }
    }
}
