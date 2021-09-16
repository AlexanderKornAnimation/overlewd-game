using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public static class NetworkHelper
{
    [Serializable]
    public class Tokens
    {
        public string accessToken;
        public string refreshToken;
    }

    public static Tokens tokens;

    public static IEnumerator Authorization(Action<Tokens> success)
    {
        var formAuthorization = new WWWForm();
        formAuthorization.AddField("deviceId", GetDeviceId());
        yield return Post("https://overlude-api.herokuapp.com/auth/login", formAuthorization, s =>
        {
            tokens = JsonUtility.FromJson<Tokens>(s);
            success?.Invoke(tokens);
        });
    }

    public static string GetDeviceId()
    {
        return SystemInfo.deviceUniqueIdentifier;
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

    private static bool RequestCheckError(UnityWebRequest request)
    {
        if (request.result != UnityWebRequest.Result.ProtocolError &&
            request.result != UnityWebRequest.Result.ConnectionError)
        {
            return false;
        }

        return true;
    }

    public static IEnumerator Get(string url, Action<string> successResponse, Action<string> errorResponse = null)
    {
        using (var request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (!RequestCheckError(request))
            {
                successResponse?.Invoke(request.downloadHandler.text);
            }
            else
            {
                Debug.LogErrorFormat("error request [{0}, {1}]", url, request.error);
                errorResponse?.Invoke(null);
            }
        }
    }

    public static IEnumerator GetWithToken(string url, string token, Action<string> successResponse, Action<string> errorResponse = null)
    {
        using (var request = UnityWebRequest.Get(url))
        {
            request.SetRequestHeader("Authorization", "Bearer " + token);

            yield return request.SendWebRequest();

            if (!RequestCheckError(request))
            {
                successResponse?.Invoke(request.downloadHandler.text);
            }
            else
            {
                Debug.LogErrorFormat("error request [{0}, {1}]", url, request.error);
                errorResponse?.Invoke(null);
            }
        }
    }

    public static IEnumerator Post(string url, WWWForm form, Action<string> successResponse, Action<string> errorResponse = null)
    {
        using (var request = UnityWebRequest.Post(url, form))
        {
            yield return request.SendWebRequest();

            if (!RequestCheckError(request))
            {
                successResponse?.Invoke(request.downloadHandler.text);
            }
            else
            {
                Debug.LogErrorFormat("error request [{0}, {1}]", url, request.error);
                errorResponse?.Invoke(null);
            }
        }
    }

    public static IEnumerator PostWithToken(string url, WWWForm form, string token, Action<string> successResponse,
        Action<string> errorResponse = null)
    {
        using (var request = UnityWebRequest.Post(url, form))
        {
            request.SetRequestHeader("Authorization", "Bearer " + token);

            yield return request.SendWebRequest();

            if (!RequestCheckError(request))
            {
                successResponse?.Invoke(request.downloadHandler.text);
            }
            else
            {
                Debug.LogErrorFormat("error request [{0}, {1}]", url, request.error);
                errorResponse?.Invoke(null);
            }
        }
    }

    public static IEnumerator LoadAudioFromServer(string url, AudioType audioType, Action<AudioClip> successResponse, Action<string> errorResponse = null)
    {
        using (var request = UnityWebRequestMultimedia.GetAudioClip(url, audioType))
        {
            yield return request.SendWebRequest();

            if (!RequestCheckError(request))
            {
                successResponse?.Invoke(DownloadHandlerAudioClip.GetContent(request));
            }
            else
            {
                Debug.LogErrorFormat("error request [{0}, {1}]", url, request.error);
                errorResponse?.Invoke(null);
            }
        }
    }

    public static IEnumerator LoadTextureFromServer(string url, Action<Texture2D> successResponse, Action<string> errorResponse = null)
    {
        using (var request = UnityWebRequestTexture.GetTexture(url))
        {
            yield return request.SendWebRequest();

            if (!RequestCheckError(request))
            {
                successResponse?.Invoke(DownloadHandlerTexture.GetContent(request));
            }
            else
            {
                Debug.LogErrorFormat("error request [{0}, {1}]", url, request.error);
                errorResponse?.Invoke(null);
            }
        }
    }

    public static IEnumerator LoadAssetBundleFromServer(string url, Action<AssetBundle> successResponse, Action<string> errorResponse = null)
    {
        using (var request = UnityWebRequestAssetBundle.GetAssetBundle(url))
        {
            yield return request.SendWebRequest();

            if (!RequestCheckError(request))
            {
                successResponse?.Invoke(DownloadHandlerAssetBundle.GetContent(request));
            }
            else
            {
                Debug.LogErrorFormat("error request [{0}, {1}]", url, request.error);
                errorResponse?.Invoke(null);
            }
        }
    }

    public static IEnumerator LoadAssetBundleFromServerWithCache(string url, Action<AssetBundle> successResponse, Action<string> errorResponse = null)
    {
        while (!Caching.ready)
        {
            yield return null;
        }

        using (var requestManifest = UnityWebRequest.Get(url + ".manifest"))
        {
            yield return requestManifest.SendWebRequest();

            if (!RequestCheckError(requestManifest))
            {
                var hashRow = requestManifest.downloadHandler.text.ToString().Split("\n".ToCharArray())[5];
                var hash = Hash128.Parse(hashRow.Split(':')[1].Trim());

                using (var requestAssetBundle = UnityWebRequestAssetBundle.GetAssetBundle(url, hash, 0))
                {
                    yield return requestAssetBundle.SendWebRequest();

                    if (!RequestCheckError(requestAssetBundle))
                    {
                        successResponse?.Invoke(DownloadHandlerAssetBundle.GetContent(requestAssetBundle));
                    }
                    else
                    {
                        Debug.LogErrorFormat("error request [{0}, {1}]", url, requestAssetBundle.error);
                        errorResponse?.Invoke(null);
                    }
                }
            }
            else
            {
                Debug.LogErrorFormat("error request [{0}, {1}]", url, requestManifest.error);
                errorResponse?.Invoke(null);
            }
        }
    }
}
