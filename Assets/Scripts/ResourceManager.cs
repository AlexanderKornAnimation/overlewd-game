using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

namespace Overlewd
{

    public static class ResourceManager
    {
        public static List<AdminBRO.NetworkResource> runtimeResourcesMeta { get; set; }
        private static Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();
        private class AssetBundlePair
        {
            public AssetBundle assetBundle;
            public bool use;
        }
        private static Dictionary<string, AssetBundlePair> assetBundles = new Dictionary<string, AssetBundlePair>();

        public static GameObject InstantiateScreenPrefab(string prefabPath, Transform parent)
        {
            var screenPrefab = Resources.Load<GameObject>(prefabPath);
            var instScreen = UnityEngine.Object.Instantiate(screenPrefab, parent);
            var screenRectTransform = instScreen.GetComponent<RectTransform>();
            UIManager.SetStretch(screenRectTransform);
            return instScreen;
        }

        public static T InstantiateScreenPrefab<T>(string prefabPath, Transform parent) where T : MonoBehaviour
        {
            var instScreen = InstantiateScreenPrefab(prefabPath, parent);
            instScreen.name = typeof(T).FullName;
            return instScreen.AddComponent<T>();
        }

        public static GameObject InstantiateWidgetPrefab(string prefabPath, Transform parent)
        {
            var widgetPrefab = Resources.Load<GameObject>(prefabPath);
            return UnityEngine.Object.Instantiate(widgetPrefab, parent);
        }

        public static T InstantiateWidgetPrefab<T>(string prefabPath, Transform parent) where T : MonoBehaviour
        {
            var instWidget = InstantiateWidgetPrefab(prefabPath, parent);
            instWidget.name = typeof(T).FullName;
            return instWidget.AddComponent<T>();
        }

        public static T InstantiateAsset<T>(string assetPath) where T : UnityEngine.Object
        {
            var asset = Resources.Load<T>(assetPath);
            return UnityEngine.Object.Instantiate(asset);
        }

        public static T InstantiateRemoteAsset<T>(string assetPath, string assetBundleId) where T : UnityEngine.Object
        {
            var assetBundle = LoadAssetBundleById(assetBundleId);
            var asset = assetBundle.LoadAsset<T>(assetPath);
            return UnityEngine.Object.Instantiate(asset);
        }

        public static string GetRootPath()
        {
            return Caching.currentCacheForWriting.path;
        }

        public static string GetRootFilePath(string fileName)
        {
            return Path.Combine(GetRootPath(), fileName);
        }

        public static string GetResourcesPath()
        {
            return Path.Combine(GetRootPath(), "Resources");
        }

        public static string GetResourcesFilePath(string fileName)
        {
            return Path.Combine(GetResourcesPath(), fileName);
        }

        public static List<string> GetDirectoryFileNames(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                return new List<string>();
            }

            var directoryFilePaths = Directory.GetFiles(directoryPath);

            var result = new List<string>();
            foreach (var filePath in directoryFilePaths)
            {
                result.Add(Path.GetFileName(filePath));
            }

            return result;
        }

        public static List<string> GetResourcesFileNames()
        {
            return GetDirectoryFileNames(GetResourcesPath());
        }

        public static long GetSpaceFreeBytes()
        {
            return Caching.currentCacheForWriting.spaceFree;
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

        public static void WriteText(string filePath, string text)
        {
            File.WriteAllText(filePath, text);
        }

        public static string LoadText(string filePath)
        {
            return File.ReadAllText(filePath);
        }

        private static void WriteBinary(string filePath, byte[] data)
        {
            File.WriteAllBytes(filePath, data);
        }

        public static bool Exists(string filePath)
        {
            return File.Exists(filePath);
        }

        public static void Delete(string filePath)
        {
            File.Delete(filePath);
        }

        public static long Size(string filePath)
        {
            var fileInfo = new FileInfo(filePath);
            return fileInfo.Length;
        }

        public static AdminBRO.NetworkResource GetResourceMetaById(string id)
        {
            return runtimeResourcesMeta.Find(item => item.id == id);
        }

        public static Texture2D LoadTexture(string fileName)
        {
            if (textures.ContainsKey(fileName))
            {
                return textures[fileName];
            }

            var filePath = GetResourcesFilePath(fileName);
            var texture = new Texture2D(1, 1);
            var data = File.ReadAllBytes(filePath);
            texture.LoadImage(data, true);
            textures.Add(fileName, texture);
            return texture;
        }

        public static Texture2D LoadTextureById(string id)
        {
            var resourceMeta = GetResourceMetaById(id);
            if (resourceMeta == null)
            {
                return null;
            }
            return LoadTexture(resourceMeta.hash);
        }

        public static Sprite LoadSpriteById(string id)
        {
            var texture = LoadTextureById(id);
            if (texture == null)
            {
                return null;
            }
            return Sprite.Create(texture,
                new Rect(0.0f, 0.0f, texture.width, texture.height),
                new Vector2(0.5f, 0.5f));
        }

        private static AssetBundle LoadAssetBundle(string fileName)
        {
            if (assetBundles.ContainsKey(fileName))
            {
                var item = assetBundles[fileName];
                item.use = true;
                return item.assetBundle;
            }

            var filePath = Path.Combine(GetResourcesPath(), fileName);
            var assetBundle = AssetBundle.LoadFromFile(filePath);
            assetBundles.Add(fileName, new AssetBundlePair { assetBundle = assetBundle, use = true });
            return assetBundle;
        }

        private static AssetBundle LoadAssetBundleById(string id)
        {
            var resourceMeta = GetResourceMetaById(id);
            if (resourceMeta == null)
            {
                return null;
            }
            return LoadAssetBundle(resourceMeta.hash);
        }

        public static void UnloadUnusedAssetBundles()
        {
            foreach (var key in assetBundles.Keys)
            {
                var item = assetBundles[key];
                if (!item.use)
                {
                    item.assetBundle.Unload(true);
                    assetBundles.Remove(key);
                }
            }
        }

        public static void MarkUnusedAssetBundles()
        {
            foreach (var key in assetBundles.Keys)
            {
                assetBundles[key].use = false;
            }
        }

        public static List<AdminBRO.NetworkResource> GetLocalResourcesMeta()
        {
            var metaFilePath = GetRootFilePath("ResourcesMeta");
            if (Exists(metaFilePath))
            {
                var metaJson = LoadText(metaFilePath);
                var meta = JsonConvert.DeserializeObject<List<AdminBRO.NetworkResource>>(metaJson);
                return meta;
            }

            return null;
        }

        public static void SaveLocalResourcesMeta(List<AdminBRO.NetworkResource> meta)
        {
            if (meta != null)
            {
                var metaFilePath = GetRootFilePath("ResourcesMeta");
                var metaJson = JsonConvert.SerializeObject(meta);
                WriteText(metaFilePath, metaJson);
            }
        }

        public static bool HasFreeSpaceForNewResources(List<AdminBRO.NetworkResource> serverResourcesMeta)
        {
            var existingFiles = GetResourcesFileNames();

            long deleteSize = 0;
            foreach (var localItemHash in existingFiles)
            {
                if (!serverResourcesMeta.Exists(serverItem => serverItem.hash == localItemHash))
                {
                    deleteSize += Size(GetResourcesFilePath(localItemHash));
                }
            }

            long downloadSize = 0;
            foreach (var serverItem in serverResourcesMeta)
            {
                if (!existingFiles.Exists(localItemHash => serverItem.hash == localItemHash))
                {
                    downloadSize += serverItem.size;
                }
            }

            return (GetSpaceFreeBytes() + deleteSize - downloadSize) > 0;
        }

        public static async Task ActualizeResourcesAsync(List<AdminBRO.NetworkResource> resourcesMeta, Action<AdminBRO.NetworkResource> downloadItem)
        {
            var existingFiles = GetResourcesFileNames();

            //DeleteNotRelevantResources
            foreach (var fileName in existingFiles)
            {
                if (!resourcesMeta.Exists(item => item.hash == fileName))
                {
                    Delete(GetResourcesFilePath(fileName));
                }
            }

            //DownloadMissingResources
            foreach (var resourceMeta in resourcesMeta)
            {
                if (!existingFiles.Exists(item => item == resourceMeta.hash))
                {
                    downloadItem?.Invoke(resourceMeta);
                    using (var request = await HttpCore.GetAsync(resourceMeta.url))
                    {
                        var fileData = request.downloadHandler.data;
                        var filePath = GetResourcesFilePath(resourceMeta.hash);
                        await Task.Run(() => 
                        {
                            WriteBinary(filePath, fileData);
                        });
                    }
                }
            }
        }
    }

}
