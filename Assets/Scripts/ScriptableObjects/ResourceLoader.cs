using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

[CreateAssetMenu(fileName = "New ResourceLoader", menuName = "Resource Loader", order = 51)]
public class ResourceLoader : ScriptableObject
{
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

    public void AddCacheAtPath(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        Cache newCache = Caching.AddCache(path);

        if (newCache.valid)
        {
            Caching.currentCacheForWriting = newCache;
        }
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

    public IEnumerator LoadTextFromServer(string url, Action<string> response)
    {
        using (var request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (!RequestCheckError(request))
            {
                response(request.downloadHandler.text);
            }
            else
            {
                Debug.LogErrorFormat("error request [{0}, {1}]", url, request.error);
                response(null);
            }
        }
    }

    public IEnumerator Post(string url, WWWForm form, Action<string> response)
    {
        using (var request = UnityWebRequest.Post(url, form))
        {
            yield return request.SendWebRequest();

            if (!RequestCheckError(request))
            {
                response(request.downloadHandler.text);
            }
            else
            {
                Debug.LogErrorFormat("error request [{0}, {1}]", url, request.error);
                response(null);
            }
        }
    }

    public IEnumerator LoadAudioFromServer(string url, AudioType audioType, Action<AudioClip> response)
    {
        using (var request = UnityWebRequestMultimedia.GetAudioClip(url, audioType))
        {
            yield return request.SendWebRequest();

            if (!RequestCheckError(request))
            {
                response(DownloadHandlerAudioClip.GetContent(request));
            }
            else
            {
                Debug.LogErrorFormat("error request [{0}, {1}]", url, request.error);
                response(null);
            }
        }
    }

    public IEnumerator LoadTextureFromServer(string url, Action<Texture2D> response)
    {
        using (var request = UnityWebRequestTexture.GetTexture(url))
        {
            yield return request.SendWebRequest();

            if (!RequestCheckError(request))
            {
                response(DownloadHandlerTexture.GetContent(request));
            }
            else
            {
                Debug.LogErrorFormat("error request [{0}, {1}]", url, request.error);
                response(null);
            }
        }
    }

    public IEnumerator LoadAssetBundleFromServer(string url, Action<AssetBundle> response)
    {
        using (var request = UnityWebRequestAssetBundle.GetAssetBundle(url))
        {
            yield return request.SendWebRequest();

            if (!RequestCheckError(request))
            {
                response(DownloadHandlerAssetBundle.GetContent(request));
            }
            else
            {
                Debug.LogErrorFormat("error request [{0}, {1}]", url, request.error);
                response(null);
            }
        }
    }

    public IEnumerator LoadAssetBundleFromServerWithCache(string url, Action<AssetBundle> response)
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
                        response(DownloadHandlerAssetBundle.GetContent(requestAssetBundle));
                    }
                    else
                    {
                        Debug.LogErrorFormat("error request [{0}, {1}]", url, requestAssetBundle.error);
                        response(null);
                    }
                }
            }
            else
            {
                Debug.LogErrorFormat("error request [{0}, {1}]", url, requestManifest.error);
                response(null);
            }
        }
    }

    public void CacheText(string fileName, string text)
    {
        var path = Path.Combine(Caching.currentCacheForWriting.path, fileName);
        File.WriteAllText(path, text);
    }

    public void TextFromCache(string fileName, Action<string> response)
    {
        var path = Path.Combine(Caching.currentCacheForWriting.path, fileName);
        response(File.ReadAllText(path));
    }

    private void CacheBinary(string fileName, byte[] data)
    {
        var path = Path.Combine(Caching.currentCacheForWriting.path, fileName);
        File.WriteAllBytes(path, data);
    }

    private void TextureFromCache(string fileName, Action<Texture2D> response)
    {
        var path = Path.Combine(Caching.currentCacheForWriting.path, fileName);
        var texture = new Texture2D(1, 1);
        var data = File.ReadAllBytes(path);
        texture.LoadImage(data, true);
        response(texture);
    }

    public bool FileExists(string fileName)
    {
        return File.Exists(Path.Combine(Caching.currentCacheForWriting.path, fileName));
    }
}
