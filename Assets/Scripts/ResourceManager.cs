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
        private static Dictionary<string, Texture2D> loadTextures = new Dictionary<string, Texture2D>();
        private static Dictionary<string, AudioClip> loadSounds = new Dictionary<string, AudioClip>();
        private static Dictionary<string, AssetBundle> loadAssetBundles = new Dictionary<string, AssetBundle>();

        //
        public static T InstantiateAsset<T>(string assetPath) where T : UnityEngine.Object
        {
            var tempAsset = Resources.Load<T>(assetPath);
            var instAsset = UnityEngine.Object.Instantiate(tempAsset);
            Resources.UnloadAsset(tempAsset);
            return instAsset;
        }

        public static GameObject InstantiateScreenAsset(string assetPath, Transform parent)
        {
            var screenPrefab = Resources.Load<GameObject>(assetPath);
            var instScreen = UnityEngine.Object.Instantiate(screenPrefab, parent);
            var screenRectTransform = instScreen.GetComponent<RectTransform>();
            UIManager.SetStretch(screenRectTransform);
            return instScreen;
        }

        public static GameObject InstantiateWidgetAsset(string assetPath, Transform parent)
        {
            var widgetPrefab = Resources.Load<GameObject>(assetPath);
            var instWidget = UnityEngine.Object.Instantiate(widgetPrefab, parent);
            return instWidget;
        }

        public static void UnloadUnusedAssets()
        {
            Resources.UnloadUnusedAssets();
        }
        //

        private static async Task WaitRequestDoneAsync(UnityWebRequest request)
        {
            while (!request.isDone)
            {
                await Task.Delay(5);
            }
        }

        private static async Task WaitBundleRequestDoneAsync(AssetBundleRequest request)
        {
            while (!request.isDone)
            {
                await Task.Delay(5);
            }
        }

        private static Sprite TextureToSprite(Texture2D texture)
        {
            if (texture != null)
            {
                return Sprite.Create(texture,
                    new Rect(0.0f, 0.0f, texture.width, texture.height),
                    new Vector2(0.5f, 0.5f));
            }
            return null;
        }

        public static string GetRootPath()
        {
            return Caching.currentCacheForWriting.path; ;
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

        public static async Task<Texture2D> LoadTextureAsync(string fileName)
        {
            if (loadTextures.ContainsKey(fileName))
            {
                return loadTextures[fileName];
            }

            var filePath = "file://" + GetResourcesFilePath(fileName);
            using (var request = UnityWebRequestTexture.GetTexture(filePath))
            {
                request.SendWebRequest();

                await WaitRequestDoneAsync(request);

                if (loadTextures.ContainsKey(fileName))
                {
                    return loadTextures[fileName];
                }

                if (request.result != UnityWebRequest.Result.Success)
                {
                    return null;
                }

                var texture = DownloadHandlerTexture.GetContent(request);
                loadTextures.Add(fileName, texture);
                return texture;
            }
        }

        public static async Task<Texture2D> LoadTextureByIdAsync(string id)
        {
            var resourceMeta = GetResourceMetaById(id);
            if (resourceMeta == null)
            {
                return null;
            }
            return await LoadTextureAsync(resourceMeta.hash);
        }

        public static async Task<Sprite> LoadSpriteByIdAsync(string id)
        {
            return TextureToSprite(await LoadTextureByIdAsync(id));
        }

        public static Texture2D LoadTexture(string fileName)
        {
            if (loadTextures.ContainsKey(fileName))
            {
                return loadTextures[fileName];
            }

            var filePath = GetResourcesFilePath(fileName);
            var texture = new Texture2D(1, 1);
            var data = File.ReadAllBytes(filePath);
            texture.LoadImage(data, true);
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
            return TextureToSprite(LoadTextureById(id));
        }

        public static Texture2D LoadTexture(string bundleFilePath, string bundleName)
        {
            var textureKey = Path.Combine(bundleName, bundleFilePath);
            if (loadTextures.ContainsKey(textureKey))
            {
                return loadTextures[textureKey];
            }

            AssetBundle assetBundle = LoadAssetBundle(bundleName);

            if (assetBundle == null)
            {
                return null;
            }

            var texture = assetBundle.LoadAsset<Texture2D>(bundleFilePath);

            if (texture == null)
            {
                return null;
            }

            loadTextures.Add(textureKey, texture);
            return texture;
        }

        public static async Task<Texture2D> LoadTextureAsync(string bundleFilePath, string bundleName)
        {
            var textureKey = Path.Combine(bundleName, bundleFilePath);
            if (loadTextures.ContainsKey(textureKey))
            {
                return loadTextures[textureKey];
            }

            var assetBundle = await LoadAssetBundleAsync(bundleName);

            if (assetBundle == null)
            {
                return null;
            }

            var request = assetBundle.LoadAssetAsync<Texture2D>(bundleFilePath);
            await WaitBundleRequestDoneAsync(request);

            if (loadTextures.ContainsKey(textureKey))
            {
                return loadTextures[textureKey];
            }

            if (request.asset == null)
            {
                return null;
            }

            var texture = (Texture2D)(request.asset);
            loadTextures.Add(textureKey, texture);
            return texture;
        }

        public static async Task<AudioClip> LoadAudioClipAsync(string fileName, AudioType audioType)
        {
            if (loadSounds.ContainsKey(fileName))
            {
                return loadSounds[fileName];
            }

            var filePath = "file://" + GetResourcesFilePath(fileName);
            using (var request = UnityWebRequestMultimedia.GetAudioClip(filePath, audioType))
            {
                request.SendWebRequest();

                await WaitRequestDoneAsync(request);

                if (loadSounds.ContainsKey(fileName))
                {
                    return loadSounds[fileName];
                }

                if (request.result != UnityWebRequest.Result.Success)
                {
                    return null;
                }

                var sound = DownloadHandlerAudioClip.GetContent(request);
                loadSounds.Add(fileName, sound);
                return sound;
            }
        }

        public static AudioClip LoadAudioClip(string bundleFilePath, string bundleName)
        {
            var soundKey = Path.Combine(bundleName, bundleFilePath);
            if (loadSounds.ContainsKey(soundKey))
            {
                return loadSounds[soundKey];
            }

            AssetBundle assetBundle = LoadAssetBundle(bundleName);

            if (assetBundle == null)
            {
                return null;
            }

            var sound = assetBundle.LoadAsset<AudioClip>(bundleFilePath);

            if (sound == null)
            {
                return null;
            }

            loadSounds.Add(soundKey, sound);
            return sound;
        }

        public static async Task<AudioClip> LoadAudioClipAsync(string bundleFilePath, string bundleName)
        {
            var soundKey = Path.Combine(bundleName, bundleFilePath);
            if (loadSounds.ContainsKey(soundKey))
            {
                return loadSounds[soundKey];
            }

            AssetBundle assetBundle = await LoadAssetBundleAsync(bundleName);

            if (assetBundle == null)
            {
                return null;
            }

            var request = assetBundle.LoadAssetAsync<AudioClip>(bundleFilePath);
            await WaitBundleRequestDoneAsync(request);

            if (loadSounds.ContainsKey(soundKey))
            {
                return loadSounds[soundKey];
            }

            if (request.asset == null)
            {
                return null;
            }

            var sound = (AudioClip)(request.asset);
            loadSounds.Add(soundKey, sound);
            return sound;
        }

        public static AssetBundle LoadAssetBundle(string fileName)
        {
            if (loadAssetBundles.ContainsKey(fileName))
            {
                return loadAssetBundles[fileName];
            }

            var filePath = Path.Combine(GetResourcesPath(), fileName);
            var assetBundle = AssetBundle.LoadFromFile(filePath);
            loadAssetBundles.Add(fileName, assetBundle);
            return assetBundle;
        }

        public static async Task<AssetBundle> LoadAssetBundleAsync(string fileName)
        {
            if (loadAssetBundles.ContainsKey(fileName))
            {
                return loadAssetBundles[fileName];
            }

            var filePath = "file://" + GetResourcesFilePath(fileName);
            using (var request = UnityWebRequestAssetBundle.GetAssetBundle(filePath))
            {
                request.SendWebRequest();

                await WaitRequestDoneAsync(request);

                if (loadAssetBundles.ContainsKey(fileName))
                {
                    return loadAssetBundles[fileName];
                }

                if (request.result != UnityWebRequest.Result.Success)
                {
                    return null;
                }

                var assetBundle = DownloadHandlerAssetBundle.GetContent(request);
                loadAssetBundles.Add(fileName, assetBundle);
                return assetBundle;
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
                    using (var request = await NetworkHelper.GetAsync(resourceMeta.url))
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
