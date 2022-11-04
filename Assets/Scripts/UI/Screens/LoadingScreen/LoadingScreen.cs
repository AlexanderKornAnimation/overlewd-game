using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Networking;

namespace Overlewd
{
    public class LoadingScreen : BaseFullScreenParent<LoadingScreenInData>
    {
        private Image loadingProgress;
        private TextMeshProUGUI text;

        private float progressBarPercent;
        
        void Awake()
        {
            var screenInst = ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Screens/LoadingScreen/LoadingScreen", transform);
            
            var canvas = screenInst.transform.Find("Canvas");
            var loadingBar = canvas.Find("LoadingBar");
            
            loadingProgress = loadingBar.Find("Progress").GetComponent<Image>();
            loadingProgress.fillAmount = 0.0f;
            text = loadingBar.Find("Text").GetComponent<TextMeshProUGUI>();
            text.text = "";
        }

        private async Task WriteFile(string filePath, byte[] data)
        {
            ResourceManager.WriteBinary(filePath, data);
            await Task.CompletedTask;
        }

        private async Task ParallelLoadResourcesAsync()
        {
            var resourcesFileNames = ResourceManager.GetResourcesFileNames();
            var localResourcesMeta = ResourceManager.GetLocalResourcesMeta();

            SetDownloadBarTitle("Check new resources");
            var serverResourcesMeta = (await AdminBRO.resourcesAsync()).dData;

            if (serverResourcesMeta?.Any() ?? false)
            {
                //find download resources
                var downloadResourcesInfo = new List<DownloadResourceInfo>();
                foreach (var serverItem in serverResourcesMeta)
                {
                    var state = !localResourcesMeta.Exists(localItem => localItem.id == serverItem.id) ?
                        DownloadResourceInfo.State.Add : DownloadResourceInfo.State.None;
                    state = localResourcesMeta.Exists(localItem =>
                        localItem.id == serverItem.id &&
                        localItem.hash != serverItem.hash) ? DownloadResourceInfo.State.Update : state;
                    state = localResourcesMeta.Exists(localItem =>
                        localItem.id == serverItem.id && localItem.hash == serverItem.hash) &&
                        !resourcesFileNames.Exists(resourceFileName => resourceFileName == serverItem.id) ?
                        DownloadResourceInfo.State.Restore : state;

                    //add to downloaded resources list
                    if (state != DownloadResourceInfo.State.None)
                    {
                        downloadResourcesInfo.Add(new DownloadResourceInfo 
                        { 
                            resourceMeta = serverItem,
                            state = state
                        });
                    }
                }

                //remove missing resources
                foreach (var fileName in resourcesFileNames)
                {
                    if (!serverResourcesMeta.Exists(serverItem => serverItem.id == fileName))
                    {
                        localResourcesMeta.RemoveAll(localItem => localItem.id == fileName);
                        var filePath = ResourceManager.GetResourcesFilePath(fileName);
                        ResourceManager.Delete(filePath);
                    }
                }
                ResourceManager.SaveLocalResourcesMeta(localResourcesMeta);

                SetDownloadBarProgress(0.4f);

                //sort by file size ascending (group files by size)
                var downloadResourcesInfoSort = downloadResourcesInfo.OrderBy(item => item.resourceMeta.size).ToList();
                if (!downloadResourcesInfoSort.Any())
                {
                    SetDownloadBarTitle("Starting");
                }

                //parallel download
                var totalFilesCount = downloadResourcesInfoSort.Count;
                var currentFilesCount = 0;
                foreach (var split in SplitResourcesList<DownloadResourceInfo>(downloadResourcesInfoSort, 22, 70.0f))
                {
                    var downloadSizeMB = 0f;
                    foreach (var item in split)
                    {
                        downloadSizeMB = ((float)item.resourceMeta.size / 1024f) / 1024f;
                    }

                    if (ResourceManager.GetStorageFreeMB() < downloadSizeMB)
                    {
                        var errNotif = UIManager.MakeSystemNotif<SystemErrorNotif>();
                        errNotif.message = "Not enough free space";
                        await errNotif.WaitChangeState();
                        Game.Quit();
                        return;
                    }

                    SetDownloadBarTitle($"Load resources {currentFilesCount + 1}-{currentFilesCount + split.Count}/{totalFilesCount}");

                    var downloadTasks = new List<Task<HttpCoreResponse>>();
                    foreach (var item in split)
                    {
                        downloadTasks.Add(HttpCore.GetAsync(item.resourceMeta.url));
                    }
                    var downloadTasksResults = await Task.WhenAll(downloadTasks);

                    var saveTasks = new List<Task>();
                    var taskId = 0;
                    foreach (var downloadTaskResult in downloadTasksResults)
                    {
                        var resourceInfo = split[taskId++];
                        var fileData = downloadTaskResult.data;
                        var filePath = ResourceManager.GetResourcesFilePath(resourceInfo.resourceMeta.id);
                        saveTasks.Add(WriteFile(filePath, fileData));
                    }
                    await Task.WhenAll(saveTasks);

                    //update meta data
                    foreach (var resourceInfo in split)
                    {
                        switch (resourceInfo.state)
                        {
                            case DownloadResourceInfo.State.Add:
                                localResourcesMeta.Add(new AdminBRO.NetworkResourceShort
                                {
                                    id = resourceInfo.resourceMeta.id,
                                    hash = resourceInfo.resourceMeta.hash
                                });
                                break;
                            case DownloadResourceInfo.State.Update:
                                var updateId = localResourcesMeta.FindIndex(0, localItem =>
                                    localItem.id == resourceInfo.resourceMeta.id);
                                localResourcesMeta[updateId] = 
                                    new AdminBRO.NetworkResourceShort 
                                    {
                                        id = resourceInfo.resourceMeta.id,
                                        hash = resourceInfo.resourceMeta.hash
                                    };
                                break;
                        }
                    }
                    ResourceManager.SaveLocalResourcesMeta(localResourcesMeta);

                    currentFilesCount += split.Count;
                    SetDownloadBarProgress(0.4f + 0.6f * currentFilesCount / totalFilesCount);
                }
            }
            else
            {
                var errNotif = UIManager.MakeSystemNotif<SystemErrorNotif>();
                errNotif.message = "No load resources";
                await errNotif.WaitChangeState();
                Game.Quit();
                return;
            }
        }

        private async void DoLoading()
        {
            await NutakuApiHelper.WaitLoggedIn(this);
            await PlayFabSDKHelper.WaitLoggedIn();

            SetDownloadBarProgress(0.0f);
            SetDownloadBarTitle("Autorize");

#if !UNITY_EDITOR
            var apiVersion = await AdminBRO.versionAsync();
            if (apiVersion.version.ToString() != AdminBRO.ApiVersion)
            {
                var errNotif = UIManager.MakeSystemNotif<SystemErrorNotif>();
                errNotif.message = $"Need client update to version {apiVersion.version}";
                await errNotif.WaitChangeState();
                Game.Quit();
                return;
            }
#endif

            var rResult = await AdminBRO.authLoginAsync();
            if (!rResult.isSuccess)
            {
                var errNotif = UIManager.MakeSystemNotif<SystemErrorNotif>();
                errNotif.title = "Login error";
                errNotif.message = "Too many logins under one account. Log in as a different user.";
                await errNotif.WaitChangeState();
                Game.Quit();
                return;
            }
            await AdminBRO.mePostAsync();
            await AdminBRO.initAsync();

            //
            SetDownloadBarProgress(0.1f);
            SetDownloadBarTitle("Download game data");

            var gameMeta = new List<BaseGameMeta> {
                GameData.player,
                GameData.currencies,
                GameData.markets,
                GameData.events,
                GameData.quests,
                GameData.dialogs,
                GameData.battles,
                GameData.ftue,
                GameData.animations,
                GameData.sounds,
                GameData.buildings,
                GameData.characters,
                GameData.equipment,
                GameData.gacha,
                GameData.matriarchs,
                GameData.battlePass,
                GameData.potions,
                GameData.nutaku,
                GameData.alchemy
            };

            foreach (var metaSplit in SplitGameMeta(gameMeta, 10))
            {
                var metaTasks = new List<Task>();
                foreach (var meta in metaSplit)
                {
                    metaTasks.Add(meta.Get());
                }
                await Task.WhenAll(metaTasks);
            }

            SetDownloadBarProgress(0.3f);

            await ParallelLoadResourcesAsync();

            SetDownloadBarProgress(1.0f);

            while (!ProgressBarIsFull())
            {
                await UniTask.Delay(100);
            }

            SetDownloadBarTitle("Done");

            await UniTask.Delay(500);

            //
            RunFTUE();
        }

        public override async Task AfterShowAsync()
        {
            DoLoading();
            UpdateProgressBarPercent();
            await Task.CompletedTask;
        }

        private void SetDownloadBarProgress(float progressPercent)
        {
            progressBarPercent = progressPercent;
        }

        private void SetDownloadBarTitle(string title)
        {
            text.text = title;
        }

        private async void UpdateProgressBarPercent()
        {
            while (progressBarPercent < 1.0f || !ProgressBarIsFull())
            {
                loadingProgress.fillAmount += (progressBarPercent - loadingProgress.fillAmount) * 0.2f;
                await UniTask.Delay(15);
            }
        }

        private bool ProgressBarIsFull()
        {
            return (progressBarPercent - loadingProgress.fillAmount) < 0.001f;
        }

        public static void RunFTUE()
        {
            GameData.devMode = false;
            GameData.ftue.activeChapter.SetAsMapChapter();
            var firstSexStage = GameData.ftue.chapter1_stages.sex1;
            if (firstSexStage.isComplete)
            {
                UIManager.ShowScreen<MapScreen>();
            }
            else
            {
                UIManager.MakeScreen<SexScreen>().
                    SetData(new SexScreenInData
                    {
                        ftueStageId = firstSexStage.id,
                    }).RunShowScreenProcess();
            }
        }

        private IEnumerable<List<DownloadResourceInfo>> SplitResourcesList<T>(List<DownloadResourceInfo> resources,
            int splitSize, float splitSizeMB)
        {
            int splitInc = 1;
            for (int i = 0; i < resources.Count; i += splitInc)
            {
                int rangeSize = 0;
                float rangeSizeMB = 0.0f;
                while ((rangeSize < resources.Count - i) && 
                       (rangeSize < splitSize) &&
                       (rangeSizeMB < splitSizeMB))
                {
                    rangeSizeMB += (resources[i + rangeSize++].resourceMeta.size / 1024f) / 1024f;
                }

                yield return resources.GetRange(i, rangeSize);

                splitInc = rangeSize;
            }

            /*for (int i = 0; i < resources.Count; i += splitSize)
            {
                yield return resources.GetRange(i, Math.Min(splitSize, resources.Count - i));
            }*/
        }

        private IEnumerable<List<BaseGameMeta>> SplitGameMeta(List<BaseGameMeta> meta, int splitSize = 4)
        {
            for (int i = 0; i < meta.Count; i += splitSize)
            {
                yield return meta.GetRange(i, Math.Min(splitSize, meta.Count - i));
            }
        }

        private class DownloadResourceInfo
        {
            public enum State
            {
                None,
                Add,
                Update,
                Restore
            }

            public AdminBRO.NetworkResource resourceMeta;
            public State state;
        }
    }

    public class LoadingScreenInData : BaseFullScreenInData
    {

    }
}
