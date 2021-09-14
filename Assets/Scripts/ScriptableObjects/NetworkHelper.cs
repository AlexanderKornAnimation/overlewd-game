using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

[CreateAssetMenu(fileName = "New NetworkHelper", menuName = "Network Helper", order = 51)]
public class NetworkHelper : ScriptableObject
{
    [Serializable]
    public class Tokens
    {
        public string accessToken;
        public string refreshToken;
    }

    public Tokens tokens;

    public IEnumerator Authorization(Action<Tokens> success)
    {
        var formAuthorization = new WWWForm();
        formAuthorization.AddField("deviceId", GetDeviceId());
        return Post("https://overlude-api.herokuapp.com/auth/login", formAuthorization, s =>
        {
            Debug.Log("Authorization");
            Debug.Log(s);
            tokens = JsonUtility.FromJson<NetworkHelper.Tokens>(s);
            success(tokens);
        });
    }

    public string GetDeviceId()
    {
        return SystemInfo.deviceUniqueIdentifier;
    }
    public bool HasNetworkConection()
    {
        return Application.internetReachability != NetworkReachability.NotReachable;
    }
    public bool NetworkTypeMobile()
    {
        return Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork;
    }
    public bool NetworkTypeWIFI()
    {
        return Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork;
    }

    private bool RequestCheckError(UnityWebRequest request)
    {
        if (request.result != UnityWebRequest.Result.ProtocolError &&
            request.result != UnityWebRequest.Result.ConnectionError)
        {
            return false;
        }

        return true;
    }

    public IEnumerator Get(string url, Action<string> successResponse, Action<string> errorResponse = null)
    {
        using (var request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (!RequestCheckError(request))
            {
                successResponse(request.downloadHandler.text);
            }
            else
            {
                Debug.LogErrorFormat("error request [{0}, {1}]", url, request.error);
                if (errorResponse != null)
                {
                    errorResponse(null);
                }
            }
        }
    }

    public IEnumerator GetWithToken(string url, string token, Action<string> successResponse, Action<string> errorResponse = null)
    {
        using (var request = UnityWebRequest.Get(url))
        {
            request.SetRequestHeader("Authorization", "Bearer " + token);

            yield return request.SendWebRequest();

            if (!RequestCheckError(request))
            {
                successResponse(request.downloadHandler.text);
            }
            else
            {
                Debug.LogErrorFormat("error request [{0}, {1}]", url, request.error);
                if (errorResponse != null)
                {
                    errorResponse(null);
                }
            }
        }
    }

    public IEnumerator Post(string url, WWWForm form, Action<string> successResponse, Action<string> errorResponse = null)
    {
        using (var request = UnityWebRequest.Post(url, form))
        {
            yield return request.SendWebRequest();

            if (!RequestCheckError(request))
            {
                successResponse(request.downloadHandler.text);
            }
            else
            {
                Debug.LogErrorFormat("error request [{0}, {1}]", url, request.error);
                if (errorResponse != null)
                {
                    errorResponse(null);
                }
            }
        }
    }

    public IEnumerator PostWithToken(string url, WWWForm form, string token, Action<string> successResponse,
        Action<string> errorResponse = null)
    {
        using (var request = UnityWebRequest.Post(url, form))
        {
            request.SetRequestHeader("Authorization", "Bearer " + token);

            yield return request.SendWebRequest();

            if (!RequestCheckError(request))
            {
                successResponse(request.downloadHandler.text);
            }
            else
            {
                Debug.LogErrorFormat("error request [{0}, {1}]", url, request.error);
                if (errorResponse != null)
                {
                    errorResponse(null);
                }
            }
        }
    }

    public IEnumerator LoadAudioFromServer(string url, AudioType audioType, Action<AudioClip> successResponse, Action<string> errorResponse = null)
    {
        using (var request = UnityWebRequestMultimedia.GetAudioClip(url, audioType))
        {
            yield return request.SendWebRequest();

            if (!RequestCheckError(request))
            {
                successResponse(DownloadHandlerAudioClip.GetContent(request));
            }
            else
            {
                Debug.LogErrorFormat("error request [{0}, {1}]", url, request.error);
                if (errorResponse != null)
                {
                    errorResponse(null);
                }
            }
        }
    }

    public IEnumerator LoadTextureFromServer(string url, Action<Texture2D> successResponse, Action<string> errorResponse = null)
    {
        using (var request = UnityWebRequestTexture.GetTexture(url))
        {
            yield return request.SendWebRequest();

            if (!RequestCheckError(request))
            {
                successResponse(DownloadHandlerTexture.GetContent(request));
            }
            else
            {
                Debug.LogErrorFormat("error request [{0}, {1}]", url, request.error);
                if (errorResponse != null)
                {
                    errorResponse(null);
                }
            }
        }
    }

    public IEnumerator LoadAssetBundleFromServer(string url, Action<AssetBundle> successResponse, Action<string> errorResponse = null)
    {
        using (var request = UnityWebRequestAssetBundle.GetAssetBundle(url))
        {
            yield return request.SendWebRequest();

            if (!RequestCheckError(request))
            {
                successResponse(DownloadHandlerAssetBundle.GetContent(request));
            }
            else
            {
                Debug.LogErrorFormat("error request [{0}, {1}]", url, request.error);
                if (errorResponse != null)
                {
                    errorResponse(null);
                }
            }
        }
    }

    public IEnumerator LoadAssetBundleFromServerWithCache(string url, Action<AssetBundle> successResponse, Action<string> errorResponse = null)
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
                        successResponse(DownloadHandlerAssetBundle.GetContent(requestAssetBundle));
                    }
                    else
                    {
                        Debug.LogErrorFormat("error request [{0}, {1}]", url, requestAssetBundle.error);
                        if (errorResponse != null)
                        {
                            errorResponse(null);
                        }
                    }
                }
            }
            else
            {
                Debug.LogErrorFormat("error request [{0}, {1}]", url, requestManifest.error);
                if (errorResponse != null)
                {
                    errorResponse(null);
                }
            }
        }
    }
}
