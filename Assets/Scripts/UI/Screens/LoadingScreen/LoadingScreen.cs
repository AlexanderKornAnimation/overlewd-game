using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace Overlewd
{
    public class LoadingScreen : BaseScreen
    {
        private Image loadingProgress;
        private TextMeshProUGUI text;
        
        void Awake()
        {
            var screenInst = ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Screens/LoadingScreen/LoadingScreen", transform);
            
            var canvas = screenInst.transform.Find("Canvas");
            var loadingBar = canvas.Find("LoadingBar");
            
            loadingProgress = loadingBar.Find("Progress").GetComponent<Image>();
            text = loadingBar.Find("Text").GetComponent<TextMeshProUGUI>();
        }

        private async Task LoadResourcesAsync()
        {
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
                        SetDownloadBarState(0.2f + 0.75f * fileNum / filesCount);
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

        private async void DoLoading()
        {
            SetDownloadBarState(0.0f);

            await AdminBRO.authLoginAsync();

            await AdminBRO.eventStagesResetAsync();

            //
            GameData.playerInfo = await AdminBRO.meAsync();

            var locale = await AdminBRO.localizationAsync("en");

            GameData.currenies = await AdminBRO.currenciesAsync();

            GameData.events = await AdminBRO.eventsAsync();

            GameData.eventMarkets = await AdminBRO.eventMarketsAsync();

            GameData.eventQuests = await AdminBRO.eventQuestsAsync();

            GameData.eventStages = await AdminBRO.eventStagesAsync();

            GameData.dialogs = await AdminBRO.dialogsAsync();

            GameData.battles = await AdminBRO.battlesAsync();

            SetDownloadBarState(0.2f);

            await LoadResourcesAsync();

            SetDownloadBarState(1.0f);
            await Task.Delay(500);

            UIManager.ShowScreen<StartingScreen>();
        }

        public override void AfterShow()
        {
            if (HttpCore.HasNetworkConection())
            {
                DoLoading();
            }
            else
            {
                UIManager.ShowDialogBox("No Internet ñonnection", "", () => Game.Quit());
            }
        }

        private void SetDownloadBarState(float progressPercent)
        {
            loadingProgress.fillAmount = progressPercent;
        }
    }
}
