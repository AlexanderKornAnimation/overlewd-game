using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Networking;

namespace Overlewd
{
    public class LoadingScreen : BaseScreen
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

        private async Task LoadResourcesAsync()
        {
            SetDownloadBarTitle("_");

            var resourcesFileNames = ResourceManager.GetResourcesFileNames();
            var localResourcesMeta = ResourceManager.GetLocalResourcesMeta();
            var serverResourcesMeta = await AdminBRO.resourcesAsync();

            if (serverResourcesMeta.Any())
            {
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

                int filesCount = serverResourcesMeta.Count;
                int fileNum = 0;
                //download updated, added and restored resources
                foreach (var serverItem in serverResourcesMeta)
                {
                    var added = !localResourcesMeta.Exists(localItem => localItem.id == serverItem.id);
                    var updatedId = localResourcesMeta.FindIndex(0, localItem => 
                        localItem.id == serverItem.id &&
                        localItem.hash != serverItem.hash);
                    var restored = localResourcesMeta.Exists(localItem => 
                        localItem.id == serverItem.id && localItem.hash == serverItem.hash) &&
                        !resourcesFileNames.Exists(resourceFileName => resourceFileName == serverItem.id);

                    if (added || updatedId != -1 || restored)
                    {
                        var downloadSizeMB = ((float)serverItem.size / 1024f) / 1024f;
                        if (ResourceManager.GetStorageFreeMB() < downloadSizeMB)
                        {
                            UIManager.ShowDialogBox("Not enough free space", "", () => Game.Quit());
                            while (true)
                            {
                                await Task.Delay(1000);
                            }
                        }

                        using (var request = await HttpCore.GetAsync(serverItem.url))
                        {
                            var fileData = request.downloadHandler.data;
                            var filePath = ResourceManager.GetResourcesFilePath(serverItem.id);
                            await Task.Run(() =>
                            {
                                //save resorce file
                                ResourceManager.WriteBinary(filePath, fileData);

                                //update meta file
                                if (!restored)
                                {
                                    if (added)
                                    {
                                        localResourcesMeta.Add(serverItem);
                                    }
                                    else if (updatedId != -1)
                                    {
                                        localResourcesMeta[updatedId] = serverItem;
                                    }
                                    ResourceManager.SaveLocalResourcesMeta(localResourcesMeta);
                                }
                            });
                        }

                        fileNum++;
                        SetDownloadBarProgress(0.3f + 0.7f * fileNum / filesCount);
                    }
                }
            }
            else
            {
                UIManager.ShowDialogBox("Server error", "No load resources", () => Game.Quit());

                while (true)
                {
                    await Task.Delay(1000);
                }
            }
        }

        private async Task ParallelLoadResourcesAsync()
        {
            var resourcesFileNames = ResourceManager.GetResourcesFileNames();
            var localResourcesMeta = ResourceManager.GetLocalResourcesMeta();

            SetDownloadBarTitle("Check new resources");
            var serverResourcesMeta = await AdminBRO.resourcesAsync();

            if (serverResourcesMeta.Any())
            {
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

                    if (state != DownloadResourceInfo.State.None)
                    {
                        downloadResourcesInfo.Add(new DownloadResourceInfo 
                        { 
                            resourceMeta = serverItem,
                            state = DownloadResourceInfo.State.Add
                        });
                    }
                }
                //sort by file size descending (group files by size)
                var downloadResourcesInfoSort = downloadResourcesInfo.OrderByDescending(item => item.resourceMeta.size).ToList();

                SetDownloadBarProgress(0.4f);
                if (!downloadResourcesInfoSort.Any())
                {
                    SetDownloadBarTitle("Starting");
                }

                //parallel download
                var totalFilesCount = downloadResourcesInfoSort.Count;
                var currentFilesCount = 0;
                foreach (var split in SplitList<DownloadResourceInfo>(downloadResourcesInfoSort, 22))
                {
                    SetDownloadBarTitle($"Load resources {currentFilesCount + split.Count}/{totalFilesCount}");

                    var downloadTasks = new List<Task<UnityWebRequest>>();
                    foreach (var item in split)
                    {
                        downloadTasks.Add(HttpCore.GetAsync(item.resourceMeta.url));
                    }

                    await Task.WhenAll(downloadTasks);

                    var saveTasks = new List<Task>();
                    var taskId = 0;
                    foreach (var task in downloadTasks)
                    {
                        var resourceInfo = split[taskId++];
                        var request = task.Result;
                        var fileData = request.downloadHandler.data;
                        var filePath = ResourceManager.GetResourcesFilePath(resourceInfo.resourceMeta.id);

                        saveTasks.Add(Task.Run(() =>
                        {
                            //save resorce file
                            ResourceManager.WriteBinary(filePath, fileData);

                            //update meta data
                            if (resourceInfo.state != DownloadResourceInfo.State.Restore)
                            {
                                if (resourceInfo.state == DownloadResourceInfo.State.Add)
                                {
                                    localResourcesMeta.Add(resourceInfo.resourceMeta);
                                }
                                else if (resourceInfo.state == DownloadResourceInfo.State.Update)
                                {
                                    var updateId = localResourcesMeta.FindIndex(0, localItem =>
                                        localItem.id == resourceInfo.resourceMeta.id);
                                    localResourcesMeta[updateId] = resourceInfo.resourceMeta;
                                }
                            }
                        }));
                    }

                    await Task.WhenAll(saveTasks);
                    ResourceManager.SaveLocalResourcesMeta(localResourcesMeta);

                    foreach (var task in downloadTasks)
                    {
                        task.Result.Dispose();
                    }

                    currentFilesCount += split.Count;
                    SetDownloadBarProgress(0.4f + 0.6f * currentFilesCount / totalFilesCount);
                }
            }
            else
            {
                UIManager.ShowDialogBox("Server error", "No load resources", () => Game.Quit());

                while (true)
                {
                    await Task.Delay(1000);
                }
            }
        }

        private async void DoLoading()
        {
            SetDownloadBarProgress(0.0f);
            SetDownloadBarTitle("Autorize");

            await AdminBRO.authLoginAsync();

            await AdminBRO.eventStagesResetAsync();

            //
            SetDownloadBarProgress(0.1f);
            SetDownloadBarTitle("Download game data");

            GameData.playerInfo = await AdminBRO.meAsync();

            var locale = await AdminBRO.localizationAsync("en");

            GameData.currenies = await AdminBRO.currenciesAsync();

            GameData.events = await AdminBRO.eventsAsync();

            GameData.eventMarkets = await AdminBRO.eventMarketsAsync();

            GameData.eventQuests = await AdminBRO.eventQuestsAsync();

            GameData.eventStages = await AdminBRO.eventStagesAsync();

            GameData.dialogs = await AdminBRO.dialogsAsync();

            GameData.battles = await AdminBRO.battlesAsync();

            var ftue = await AdminBRO.ftueAsync();

            SetDownloadBarProgress(0.3f);

            //await LoadResourcesAsync();
            await ParallelLoadResourcesAsync();

            SetDownloadBarProgress(1.0f);

            while (!ProgressBarIsFull())
            {
                await Task.Delay(100);
            }

            SetDownloadBarTitle("Done");

            await Task.Delay(500);

            UIManager.ShowScreen<StartingScreen>();
        }

        public override void AfterShow()
        {
            if (HttpCore.HasNetworkConection())
            {
                DoLoading();
                StartCoroutine(UpdateProgressBarPercent());
            }
            else
            {
                UIManager.ShowDialogBox("No Internet ñonnection", "", () => Game.Quit());
            }
        }

        private void SetDownloadBarProgress(float progressPercent)
        {
            progressBarPercent = progressPercent;
        }

        private void SetDownloadBarTitle(string title)
        {
            text.text = title;
        }

        private IEnumerator UpdateProgressBarPercent()
        {
            while (true)
            {
                loadingProgress.fillAmount += Math.Min(0.01f, progressBarPercent - loadingProgress.fillAmount);
                yield return new WaitForSeconds(0.02f);
            }
        }

        private bool ProgressBarIsFull()
        {
            return (progressBarPercent - loadingProgress.fillAmount) < 0.001f;
        }

        private IEnumerable<List<T>> SplitList<T>(List<T> list, int splitSize)
        {
            for (int i = 0; i < list.Count; i += splitSize)
            {
                yield return list.GetRange(i, Math.Min(splitSize, list.Count - i));
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
}
