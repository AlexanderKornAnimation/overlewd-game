using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Overlewd
{

    public static class ResourceManager
    {
        private static string persistentDataPath;

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
            var assetBundle = LoadAssetBundle(assetBundleId);
            var asset = assetBundle.LoadAsset<T>(assetPath.Trim());
            return UnityEngine.Object.Instantiate(asset);
        }

        public static string GetRootPath()
        {
            return persistentDataPath;
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

        public static int GetStorageFreeMB()
        {
            return SimpleDiskUtils.DiskUtils.CheckAvailableSpace();
        }

        public static void Initialize()
        {
            persistentDataPath = Application.persistentDataPath;

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

        public static void WriteBinary(string filePath, byte[] data)
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

        public static Texture2D LoadTexture(string id)
        {
            if (textures.ContainsKey(id))
            {
                return textures[id];
            }

            var filePath = GetResourcesFilePath(id);
            var texture = new Texture2D(1, 1);
            var data = File.ReadAllBytes(filePath);
            texture.LoadImage(data, true);
            textures.Add(id, texture);
            return texture;
        }

        public static Sprite LoadSprite(string id)
        {
            var texture = LoadTexture(id);
            if (texture == null)
            {
                return null;
            }
            return Sprite.Create(texture,
                new Rect(0.0f, 0.0f, texture.width, texture.height),
                new Vector2(0.5f, 0.5f));
        }

        private static AssetBundle LoadAssetBundle(string id)
        {
            if (assetBundles.ContainsKey(id))
            {
                var item = assetBundles[id];
                item.use = true;
                return item.assetBundle;
            }

            var filePath = Path.Combine(GetResourcesPath(), id);
            var assetBundle = AssetBundle.LoadFromFile(filePath);
            assetBundles.Add(id, new AssetBundlePair { assetBundle = assetBundle, use = true });
            return assetBundle;
        }

        public static void UnloadUnusedAssetBundles()
        {
            var removeKeys = new List<string>();
            foreach (var key in assetBundles.Keys)
            {
                if (!assetBundles[key].use)
                {
                    removeKeys.Add(key);
                }
            }

            foreach (var key in removeKeys)
            {
                assetBundles[key].assetBundle.Unload(false);
                assetBundles.Remove(key);
            }
        }

        public static void MarkUnusedAssetBundles()
        {
            foreach (var key in assetBundles.Keys)
            {
                assetBundles[key].use = false;
            }
        }

        public static List<AdminBRO.NetworkResourceShort> GetLocalResourcesMeta()
        {
            var metaFilePath = GetRootFilePath("ResourcesMeta");
            if (Exists(metaFilePath))
            {
                var metaJson = LoadText(metaFilePath);
                return JsonHelper.DeserializeObject<List<AdminBRO.NetworkResourceShort>>(metaJson) ??
                    new List<AdminBRO.NetworkResourceShort>();
            }
            return new List<AdminBRO.NetworkResourceShort>();
        }

        public static void SaveLocalResourcesMeta(List<AdminBRO.NetworkResourceShort> meta)
        {
            if (meta != null)
            {
                var metaFilePath = GetRootFilePath("ResourcesMeta");
                var metaJson = JsonHelper.SerializeObject(meta);
                WriteText(metaFilePath, metaJson);
            }
        }
    }

}
