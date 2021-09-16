using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public static class ResourceManager
{
    public static string GetCachePath()
    {
        return Caching.currentCacheForWriting.path;
    }

    public static string GetCacheFilePath(string fileName)
    {
        return Path.Combine(Caching.currentCacheForWriting.path, fileName);
    } 

    public static string GetResourcesPath()
    {
        return Path.Combine(Caching.currentCacheForWriting.path, "Resources");
    }

    public static string GetResourcesFilePath(string fileName)
    {
        return Path.Combine(GetResourcesPath(), fileName);
    }

    public static void InitializeCache()
    {
        var cachePath = Path.Combine(Application.persistentDataPath, "OverlewdCache");
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

    public static List<string> GetDirectoryFileNames(string directoryPath)
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

    public static List<string> GetFileNamesFormResources()
    {
        return GetDirectoryFileNames(GetResourcesPath());
    }

    public static void CacheText(string filePath, string text)
    {
        File.WriteAllText(filePath, text);
    }

    public static void TextFromCache(string filePath, Action<string> response)
    {
        response?.Invoke(File.ReadAllText(filePath));
    }

    private static void CacheBinary(string filePath, byte[] data)
    {
        File.WriteAllBytes(filePath, data);
    }

    private static void TextureFromCache(string filePath, Action<Texture2D> response)
    {
        var texture = new Texture2D(1, 1);
        var data = File.ReadAllBytes(filePath);
        texture.LoadImage(data, true);
        response?.Invoke(texture);
    }

    public static bool FileExists(string filePath)
    {
        return File.Exists(filePath);
    }

    public static void FileDelete(string filePath)
    {
        File.Delete(filePath);
    }

    public static IEnumerator LoadResourcesMeta(Action<NetworkResources> success)
    {
        yield return NetworkHelper.GetWithToken("https://overlude-api.herokuapp.com/resources", NetworkHelper.tokens.accessToken, s =>
        {
            Debug.Log("GET RESOURCES META");
            Debug.Log(s);

            var resources = JsonUtility.FromJson<NetworkResources>(s);
            success?.Invoke(resources);
        });
    }

    public static IEnumerator DeleteNotRelevantResources(NetworkResources resourcesMeta, List<string> existingFiles)
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

    public static IEnumerator DownloadMissingResources(NetworkResources resourcesMeta, List<string> existingFiles, Action success = null)
    {
        foreach (var resourceMeta in resourcesMeta.items)
        {
            if (!existingFiles.Exists(item => item == resourceMeta.hash))
            {
                yield return NetworkHelper.LoadTextureFromServer(resourceMeta.url, tex =>
                {
                    var data = tex.EncodeToPNG();
                    CacheBinary(GetResourcesFilePath(resourceMeta.hash), data);
                });
            }
        }
        success?.Invoke();
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
