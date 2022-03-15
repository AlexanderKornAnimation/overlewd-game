using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;

namespace Overlewd
{

    public static class ResourceManager
    {
        private static string persistentDataPath;
        private static string streamingAssetsPath;

        private class TextureInfo
        {
            public string key;
            public Texture2D texture;
        }
        private static List<TextureInfo> textures = new List<TextureInfo>();

        private class AssetBundleInfo
        {
            public string key;
            public AssetBundle assetBundle;
            public bool use;
        }
        private static List<AssetBundleInfo> assetBundles = new List<AssetBundleInfo>();

        private class FMODBankInfo
        {
            public string key;
            public FMODBank bank;
            public bool use;
        }
        private static List<FMODBankInfo> fmodBanks = new List<FMODBankInfo>();

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

        public static T InstantiateAsset<T>(string assetPath, Transform parent = null) where T : UnityEngine.Object
        {
            var asset = Resources.Load<T>(assetPath);
            return UnityEngine.Object.Instantiate(asset, parent);
        }

        public static T InstantiateRemoteAsset<T>(string assetPath, string assetBundleId, Transform parent = null) where T : UnityEngine.Object
        {
            var assetBundle = LoadAssetBundle(assetBundleId);
            var asset = assetBundle?.LoadAsset<T>(assetPath.Trim());
            return asset != null ? UnityEngine.Object.Instantiate(asset, parent) : null;
        }

        public static string GetRootPath()
        {
#if UNITY_WEBGL
            return streamingAssetsPath;
#else
            return persistentDataPath;
#endif
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
            streamingAssetsPath = Application.streamingAssetsPath;

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
            var item = textures.Find(item => item.key == id);
            if (item != null)
            {
                return item.texture;
            }

            var filePath = GetResourcesFilePath(id);
            var texture = new Texture2D(1, 1);
            var data = File.ReadAllBytes(filePath);
            texture.LoadImage(data, true);
            textures.Add(new TextureInfo { key = id, texture = texture });
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
            var item = assetBundles.Find(item => item.key == id);
            if (item != null)
            {
                item.use = true;
                return item.assetBundle;
            }

            var filePath = Path.Combine(GetResourcesPath(), id);
            var assetBundle = AssetBundle.LoadFromFile(filePath);
            if (assetBundle != null)
            {
                assetBundles.Add(new AssetBundleInfo { key = id, assetBundle = assetBundle, use = true });
            }
            return assetBundle;
        }

        public static void UnloadUnusedAssetBundles()
        {
            foreach (var item in assetBundles.ToList())
            {
                if (!item.use)
                {
                    item.assetBundle.Unload(false);
                    assetBundles.Remove(item);
                }
            }
        }

        public static void MarkUnusedAssetBundles()
        {
            foreach (var item in assetBundles)
            {
               item.use = false;
            }
        }

        public static FMODBank LoadFMODBank(string id)
        {
            var item = fmodBanks.Find(item => item.key == id);
            if (item != null)
            {
                item.use = true;
                return item.bank;
            }

            var filePath = Path.Combine(GetResourcesPath(), id);
            if (Exists(filePath))
            {
                var fmodBank = FMODBank.LoadFromFile(filePath);
                fmodBanks.Add(new FMODBankInfo { key = id, bank = fmodBank, use = true });
                return fmodBank;
            }
            return null;
        }

        public static void UnloadUnusedFMODBanks()
        {
            foreach (var item in fmodBanks.ToList())
            {
                if (!item.use)
                {
                    item.bank.Unload();
                    fmodBanks.Remove(item);
                }
            }
        }

        public static void MarkUnusedFMODBanks()
        {
            foreach (var item in fmodBanks)
            {
                item.use = false;
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
