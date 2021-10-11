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

        private static bool RequestCheckError(UnityWebRequest request)
        {
            if (request.result != UnityWebRequest.Result.ProtocolError &&
                request.result != UnityWebRequest.Result.ConnectionError)
            {
                return false;
            }

            return true;
        }

        public static IEnumerator Get(string url, Action<DownloadHandler> successResponse, Action<string> errorResponse = null)
        {
            using (var request = UnityWebRequest.Get(url))
            {
                yield return request.SendWebRequest();

                if (!RequestCheckError(request))
                {
                    successResponse?.Invoke(request.downloadHandler);
                }
                else
                {
                    var errorMsg = String.Format("error request [{0}, {1}]", url, request.error);
                    Debug.LogErrorFormat(errorMsg);
                    errorResponse?.Invoke(errorMsg);
                }
            }
        }

        public static IEnumerator GetWithToken(string url, string token, Action<DownloadHandler> successResponse, Action<string> errorResponse = null)
        {
            using (var request = UnityWebRequest.Get(url))
            {
                request.SetRequestHeader("Authorization", "Bearer " + token);

                yield return request.SendWebRequest();

                if (!RequestCheckError(request))
                {
                    successResponse?.Invoke(request.downloadHandler);
                }
                else
                {
                    var errorMsg = String.Format("error request [{0}, {1}]", url, request.error);
                    Debug.LogErrorFormat(errorMsg);
                    errorResponse?.Invoke(errorMsg);
                }
            }
        }

        public static IEnumerator Post(string url, WWWForm form, Action<DownloadHandler> successResponse, Action<string> errorResponse = null)
        {
            using (var request = UnityWebRequest.Post(url, form))
            {
                yield return request.SendWebRequest();

                if (!RequestCheckError(request))
                {
                    successResponse?.Invoke(request.downloadHandler);
                }
                else
                {
                    var errorMsg = String.Format("error request [{0}, {1}]", url, request.error);
                    Debug.LogErrorFormat(errorMsg);
                    errorResponse?.Invoke(errorMsg);
                }
            }
        }

        public static IEnumerator PostWithToken(string url, WWWForm form, string token, Action<DownloadHandler> successResponse,
            Action<string> errorResponse = null)
        {
            using (var request = UnityWebRequest.Post(url, form))
            {
                request.SetRequestHeader("Authorization", "Bearer " + token);

                yield return request.SendWebRequest();

                if (!RequestCheckError(request))
                {
                    successResponse?.Invoke(request.downloadHandler);
                }
                else
                {
                    var errorMsg = String.Format("error request [{0}, {1}]", url, request.error);
                    Debug.LogErrorFormat(errorMsg);
                    errorResponse?.Invoke(errorMsg);
                }
            }
        }

        public static IEnumerator LoadAudioFromServer(string url, AudioType audioType, Action<AudioClip, byte[]> successResponse, Action<string> errorResponse = null)
        {
            using (var request = UnityWebRequestMultimedia.GetAudioClip(url, audioType))
            {
                yield return request.SendWebRequest();

                if (!RequestCheckError(request))
                {
                    successResponse?.Invoke(DownloadHandlerAudioClip.GetContent(request), request.downloadHandler.data);
                }
                else
                {
                    var errorMsg = String.Format("error request [{0}, {1}]", url, request.error);
                    Debug.LogErrorFormat(errorMsg);
                    errorResponse?.Invoke(errorMsg);
                }
            }
        }

        public static IEnumerator LoadTextureFromServer(string url, Action<Texture2D, byte[]> successResponse, Action<string> errorResponse = null)
        {
            using (var request = UnityWebRequestTexture.GetTexture(url))
            {
                yield return request.SendWebRequest();

                if (!RequestCheckError(request))
                {
                    successResponse?.Invoke(DownloadHandlerTexture.GetContent(request), request.downloadHandler.data);
                }
                else
                {
                    var errorMsg = String.Format("error request [{0}, {1}]", url, request.error);
                    Debug.LogErrorFormat(errorMsg);
                    errorResponse?.Invoke(errorMsg);
                }
            }
        }

        public static IEnumerator LoadAssetBundleFromServer(string url, Action<AssetBundle, byte[]> successResponse, Action<string> errorResponse = null)
        {
            using (var request = UnityWebRequestAssetBundle.GetAssetBundle(url))
            {
                yield return request.SendWebRequest();

                if (!RequestCheckError(request))
                {
                    successResponse?.Invoke(DownloadHandlerAssetBundle.GetContent(request), request.downloadHandler.data);
                }
                else
                {
                    var errorMsg = String.Format("error request [{0}, {1}]", url, request.error);
                    Debug.LogErrorFormat(errorMsg);
                    errorResponse?.Invoke(errorMsg);
                }
            }
        }
    }

}
