using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "New ResourceManager", menuName = "Resource Manager", order = 51)]
public class ResourceManager : ScriptableObject
{
    [SerializeField]
    private NetworkHelper networkHelper;

    public string GetCachePath()
    {
        return Caching.currentCacheForWriting.path;
    }

    public string GetCacheFilePath(string fileName)
    {
        return Path.Combine(Caching.currentCacheForWriting.path, fileName);
    } 

    public string GetResourcesPath()
    {
        return Path.Combine(Caching.currentCacheForWriting.path, "Resources");
    }

    public string GetResourcesFilePath(string fileName)
    {
        return Path.Combine(GetResourcesPath(), fileName);
    }

    public void InitializeCache()
    {
        var cachePath = "OverlewdCache";
        if (!Directory.Exists(cachePath))
        {
            Directory.CreateDirectory(cachePath);
        }

        Cache newCache = Caching.AddCache(cachePath);

        if (newCache.valid)
        {
            Caching.currentCacheForWriting = newCache;
        }

        //Resources directory init
        if (!Directory.Exists(GetResourcesPath()))
        {
            Directory.CreateDirectory(GetResourcesPath());
        }
    }

    public List<string> GetDirectoryFileNames(string directoryPath)
    {
        if (!Directory.Exists(directoryPath))
        {
            return new List<string>();
        }

        var directoryFilePaths= Directory.GetFiles(directoryPath);

        var result = new List<string>();
        foreach (var filePath in directoryFilePaths)
        {
            result.Add(Path.GetFileName(filePath));
        }

        return result;
    }

    public List<string> GetFileNamesFormResources()
    {
        return GetDirectoryFileNames(GetResourcesPath());
    }

    public void CacheText(string filePath, string text)
    {
        File.WriteAllText(filePath, text);
    }

    public void TextFromCache(string filePath, Action<string> response)
    {
        response(File.ReadAllText(filePath));
    }

    private void CacheBinary(string filePath, byte[] data)
    {
        File.WriteAllBytes(filePath, data);
    }

    private void TextureFromCache(string filePath, Action<Texture2D> response)
    {
        var texture = new Texture2D(1, 1);
        var data = File.ReadAllBytes(filePath);
        texture.LoadImage(data, true);
        response(texture);
    }

    public bool FileExists(string filePath)
    {
        return File.Exists(filePath);
    }

    public void FileDelete(string filePath)
    {
        File.Delete(filePath);
    }

    public IEnumerator LoadResourcesMeta(Action<NetworkResources> success)
    {
        return networkHelper.GetWithToken("https://overlude-api.herokuapp.com/resources", networkHelper.tokens.accessToken, s =>
        {
            Debug.Log("GET RESOURCES META");
            Debug.Log(s);

            var resources = JsonUtility.FromJson<NetworkResources>(s);
            success(resources);
        });
    }

    public IEnumerator DeleteNotRelevantResources(NetworkResources resourcesMeta, List<string> existingFiles)
    {
        foreach (var fileName in existingFiles)
        {
            if (!resourcesMeta.items.Exists(item => item.hash == fileName))
            {
                FileDelete(GetResourcesFilePath(fileName));
            }
            yield return fileName;
        }
    }

    public IEnumerator DownloadMissingResources(NetworkResources resourcesMeta, List<string> existingFiles)
    {       
        foreach (var resourceMeta in resourcesMeta.items)
        {
            if (!existingFiles.Exists(item => item == resourceMeta.hash))
            {
                yield return networkHelper.LoadTextureFromServer(resourceMeta.url, tex =>
                {
                    var data = tex.EncodeToPNG();
                    CacheBinary(GetResourcesFilePath(resourceMeta.hash), data);
                });
            }
            else
            {
                yield return resourceMeta.hash;
            }
        }
    }

    [Serializable]
    public class NetworkResource
    {
        public int id;
        public string url;
        public string hash;
    }

    [Serializable]
    public class NetworkResources
    {
        public List<NetworkResource> items;
    }
}
