using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
            var localResourcesMeta = ResourceManager.GetLocalResourcesMeta();
            var serverResourcesMeta = await AdminBRO.resourcesAsync();
            if (serverResourcesMeta != null)
            {
                if (!ResourceManager.HasFreeSpaceForNewResources(serverResourcesMeta))
                {
                    UIManager.ShowDialogBox("Not enough free space", "", () => Game.Quit());
                }
                else
                {
                    await ResourceManager.ActualizeResourcesAsync(serverResourcesMeta,
                        (resourceItemMeta) =>
                        {
                            //loadingLabel = "Download: " + resourceItemMeta.url;
                        });

                    ResourceManager.SaveLocalResourcesMeta(serverResourcesMeta);
                    ResourceManager.runtimeResourcesMeta = serverResourcesMeta;
                }
            }
            else
            {
                UIManager.ShowDialogBox("Server error", "No load resources", () => Game.Quit());
            }
        }

        private async void DoLoading()
        {
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

            await LoadResourcesAsync();

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
    }
}
